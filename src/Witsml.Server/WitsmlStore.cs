﻿//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel.Web;
using log4net;
using PDS.Framework;
using PDS.Witsml.Properties;
using PDS.Witsml.Server.Configuration;
using PDS.Witsml.Server.Data;
using PDS.Witsml.Server.Logging;

namespace PDS.Witsml.Server
{
    /// <summary>
    /// The WITSML Store API server implementation.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.IWitsmlStore" />
    [Export(typeof(IWitsmlStore))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WitsmlStore : IWitsmlStore
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WitsmlStore));
        private static readonly string DefaultDataSchemaVersion = Settings.Default.DefaultDataSchemaVersion;

        private readonly IDictionary<string, ICapServerProvider> _capServerMap;
        private string _supportedVersions;

        /// <summary>
        /// Initializes a new instance of the <see cref="WitsmlStore"/> class.
        /// </summary>
        public WitsmlStore()
        {
            _capServerMap = new Dictionary<string, ICapServerProvider>();
        }

        /// <summary>
        /// Gets or sets the composition container used for dependency injection.
        /// </summary>
        /// <value>The composition container.</value>
        [Import]
        public IContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the cap server providers.
        /// </summary>
        /// <value>The cap server providers.</value>
        [ImportMany]
        public IEnumerable<ICapServerProvider> CapServerProviders { get; set; }

        /// <summary>
        /// Returns a string containing the Data Schema Version(s) that a server supports.
        /// </summary>
        /// <param name="request">The request object encapsulating the method input parameters.</param>
        /// <returns>A comma-separated list of Data Schema Versions (without spaces) that the server supports.</returns>
        public WMLS_GetVersionResponse WMLS_GetVersion(WMLS_GetVersionRequest request)
        {
            EnsureCapServerProviders();

            _log.Debug(WebOperationContext.Current.ToLogMessage());
            _log.Debug(request.ToLogMessage());

            var response = new WMLS_GetVersionResponse(_supportedVersions);
            _log.Debug(response.ToLogMessage());
            return response;
        }

        /// <summary>
        /// Returns the capServer object that describes the capabilities of the server for one Data Schema Version.
        /// </summary>
        /// <param name="request">The request object encapsulating the method input parameters.</param>
        /// <returns>A positive value indicates a success; a negative value indicates an error.</returns>
        public WMLS_GetCapResponse WMLS_GetCap(WMLS_GetCapRequest request)
        {
            try
            {
                EnsureCapServerProviders();

                _log.Debug(WebOperationContext.Current.ToLogMessage());
                _log.Debug(request.ToLogMessage());

                var options = OptionsIn.Parse(request.OptionsIn);
                var version = OptionsIn.GetValue(options, new OptionsIn.DataVersion(DefaultDataSchemaVersion));

                // return error if WITSML 1.3.1 not supported AND dataVersion not specified (required in WITSML 1.4.1)
                if (!_capServerMap.ContainsKey(OptionsIn.DataVersion.Version131.Value) && !options.ContainsKey(OptionsIn.DataVersion.Keyword))
                {
                    throw new WitsmlException(ErrorCodes.MissingDataVersion);
                }

                if (_capServerMap.ContainsKey(version))
                {
                    var response = new WMLS_GetCapResponse((short)ErrorCodes.Success, _capServerMap[version].ToXml(), string.Empty);
                    _log.Debug(response.ToLogMessage());
                    return response;
                }

                throw new WitsmlException(ErrorCodes.DataVersionNotSupported, "Data schema version not supported: " + version);
            }
            catch (WitsmlException ex)
            {
                var response = new WMLS_GetCapResponse((short)ex.ErrorCode, string.Empty, ex.Message);
                _log.Warn(response.ToLogMessage(_log.IsWarnEnabled));
                return response;
            }
        }

        /// <summary>
        /// Returns one or more WITSML data-objects from the server.
        /// </summary>
        /// <param name="request">The request object encapsulating the method input parameters.</param>
        /// <returns>
        /// A positive value indicating success along with one or more WITSML data-objects from the server, or a negative value indicating an error.
        /// </returns>
        public WMLS_GetFromStoreResponse WMLS_GetFromStore(WMLS_GetFromStoreRequest request)
        {
            var context = request.ToContext();
            var version = string.Empty;

            try
            {
                _log.Debug(WebOperationContext.Current.ToLogMessage());
                _log.Debug(context);

                WitsmlValidator.ValidateRequest(CapServerProviders, context, out version);

                var dataProvider = Container.Resolve<IWitsmlDataProvider>(new ObjectName(context.ObjectType, version));
                var result = dataProvider.GetFromStore(context);

                var response = new WMLS_GetFromStoreResponse(
                    (short)result.Code,
                    result.Results != null
                        ? WitsmlParser.ToXml(result.Results)
                        : string.Empty,
                    result.Message);

                _log.Debug(response.ToLogMessage());

                return response;
            }
            catch (ContainerException)
            {
                var response = new WMLS_GetFromStoreResponse((short)ErrorCodes.DataObjectNotSupported, string.Empty,
                    "WITSML object type not supported: " + context.ObjectType + "; Version: " + version);

                _log.Warn(response.ToLogMessage(_log.IsWarnEnabled));

                return response;
            }
            catch (WitsmlException ex)
            {
                var response = new WMLS_GetFromStoreResponse((short)ex.ErrorCode, string.Empty, ex.Message);
                _log.Warn(response.ToLogMessage(_log.IsWarnEnabled));
                return response;
            }
        }

        /// <summary>
        /// Returns the response for adding one WITSML data-object to the server
        /// </summary>
        /// <param name="request">The request object encapsulating the method input parameters.</param>
        /// <returns>A positive value indicates a success; a negative value indicates an error.</returns>
        public WMLS_AddToStoreResponse WMLS_AddToStore(WMLS_AddToStoreRequest request)
        {
            var context = request.ToContext();
            var version = string.Empty;

            try
            {
                _log.Debug(WebOperationContext.Current.ToLogMessage());
                _log.Debug(context);

                WitsmlValidator.ValidateRequest(CapServerProviders, context, out version);

                var dataWriter = Container.Resolve<IWitsmlDataProvider>(new ObjectName(context.ObjectType, version));
                var result = dataWriter.AddToStore(context);

                var response = new WMLS_AddToStoreResponse((short)result.Code, result.Message);
                _log.Debug(response.ToLogMessage());
                return response;
            }
            catch (ContainerException)
            {
                var response = new WMLS_AddToStoreResponse((short)ErrorCodes.DataObjectNotSupported,
                    "WITSML object type not supported: " + context.ObjectType + "; Version: " + version);

                _log.Warn(response.ToLogMessage(_log.IsWarnEnabled));

                return response;
            }
            catch (WitsmlException ex)
            {
                var response = new WMLS_AddToStoreResponse((short)ex.ErrorCode, ex.Message);
                _log.Warn(response.ToLogMessage(_log.IsWarnEnabled));
                return response;
            }
        }

        public WMLS_UpdateInStoreResponse WMLS_UpdateInStore(WMLS_UpdateInStoreRequest request)
        {
            var context = request.ToContext();
            var version = string.Empty;

            try
            {
                _log.Debug(WebOperationContext.Current.ToLogMessage());
                _log.Debug(context);

                WitsmlValidator.ValidateRequest(CapServerProviders, context, out version);

                var dataWriter = Container.Resolve<IWitsmlDataProvider>(new ObjectName(context.ObjectType, version));
                var result = dataWriter.UpdateInStore(context);

                return new WMLS_UpdateInStoreResponse((short)result.Code, result.Message);
            }
            catch (ContainerException)
            {
                return new WMLS_UpdateInStoreResponse(
                    (short)ErrorCodes.DataObjectNotSupported,
                    "WITSML object type not supported: " + context.ObjectType + "; Version: " + version);
            }
            catch (WitsmlException ex)
            {
                return new WMLS_UpdateInStoreResponse((short)ex.ErrorCode, ex.Message);
            }
        }

        public WMLS_DeleteFromStoreResponse WMLS_DeleteFromStore(WMLS_DeleteFromStoreRequest request)
        {
            var context = request.ToContext();
            var version = string.Empty;

            try
            {
                _log.Debug(WebOperationContext.Current.ToLogMessage());
                _log.Debug(context);

                WitsmlValidator.ValidateRequest(CapServerProviders, context, out version);

                var dataWriter = Container.Resolve<IWitsmlDataProvider>(new ObjectName(context.ObjectType, version));
                var result = dataWriter.DeleteFromStore(context);

                return new WMLS_DeleteFromStoreResponse((short)result.Code, result.Message);
            }
            catch (ContainerException)
            {
                return new WMLS_DeleteFromStoreResponse(
                    (short)ErrorCodes.DataObjectNotSupported,
                    "WITSML object type not supported: " + context.ObjectType + "; Version: " + version);
            }
            catch (WitsmlException ex)
            {
                return new WMLS_DeleteFromStoreResponse((short)ex.ErrorCode, ex.Message);
            }
        }

        /// <summary>
        /// Returns a string containing only the fixed (base) message text associated with a defined Return Value.
        /// </summary>
        /// <param name="request">The request object encapsulating the method input parameters.</param>
        /// <returns>The fixed descriptive message text associated with the Return Value.</returns>
        public WMLS_GetBaseMsgResponse WMLS_GetBaseMsg(WMLS_GetBaseMsgRequest request)
        {
            _log.Debug(WebOperationContext.Current.ToLogMessage());
            string message;

            if (Enum.IsDefined(typeof(ErrorCodes), request.ReturnValueIn))
            {
                var errorCode = (ErrorCodes)request.ReturnValueIn;
                message = errorCode.GetDescription();

                _log.DebugFormat("{0} - {1}", request.ReturnValueIn, message);
            }
            else
            {
                _log.Warn("Unknown WITSML error code: " + request.ReturnValueIn);
                message = null;
            }

            return new WMLS_GetBaseMsgResponse(message);
        }

        /// <summary>
        /// Ensures the <see cref="ICapServerProvider"/>s are loaded.
        /// </summary>
        private void EnsureCapServerProviders()
        {
            if (_capServerMap.Any())
                return;

            foreach (var provider in CapServerProviders)
            {
                var capServerXml = provider.ToXml();

                if (!string.IsNullOrWhiteSpace(capServerXml))
                {
                    _capServerMap[provider.DataSchemaVersion] = provider;
                }
            }

            var versions = _capServerMap.Keys.ToList();
            versions.Sort();

            _supportedVersions = string.Join(",", versions);
        }
    }
}
