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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Energistics.Common;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.Datatypes;
using Energistics.Datatypes.Object;
using Energistics.Protocol.Discovery;
using PDS.Witsml.Server.Data;

namespace PDS.Witsml.Server.Providers.Discovery
{
    /// <summary>
    /// Provides information about resources available in a WITSML store for version 1.3.1.1.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Providers.Discovery.IDiscoveryStoreProvider" />
    [Export(typeof(IDiscoveryStoreProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DiscoveryStore131Provider : IDiscoveryStoreProvider
    {
        private readonly IEtpDataProvider<Well> _wellDataProvider;
        private readonly IEtpDataProvider<Wellbore> _wellboreDataProvider;
        private readonly IEtpDataProvider<Log> _logDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryStore131Provider" /> class.
        /// </summary>
        /// <param name="wellDataProvider">The well data provider.</param>
        /// <param name="wellboreDataProvider">The wellbore data provider.</param>
        /// <param name="logDataProvider">The log data provider.</param>
        [ImportingConstructor]
        public DiscoveryStore131Provider(
            IEtpDataProvider<Well> wellDataProvider,
            IEtpDataProvider<Wellbore> wellboreDataProvider,
            IEtpDataProvider<Log> logDataProvider)
        {
            _wellDataProvider = wellDataProvider;
            _wellboreDataProvider = wellboreDataProvider;
            _logDataProvider = logDataProvider;
        }

        /// <summary>
        /// Gets the data schema version supported by the provider.
        /// </summary>
        /// <value>The data schema version.</value>
        public string DataSchemaVersion
        {
            get { return OptionsIn.DataVersion.Version131.Value; }
        }

        /// <summary>
        /// Gets a collection of resources associated to the specified URI.
        /// </summary>
        /// <param name="args">The <see cref="ProtocolEventArgs{GetResources, IList}"/> instance containing the event data.</param>
        public void GetResources(ProtocolEventArgs<GetResources, IList<Resource>> args)
        {
            if (EtpUri.IsRoot(args.Message.Uri))
            {
                args.Context.Add(DiscoveryStoreProvider.NewProtocol(EtpUris.Witsml131, "WITSML Store (1.3.1.1)"));
                return;
            }

            var uri = new EtpUri(args.Message.Uri);

            if (!uri.IsRelatedTo(EtpUris.Witsml131))
            {
                return;
            }
            if (args.Message.Uri == EtpUris.Witsml131)
            {
                _wellDataProvider.GetAll()
                    .ForEach(x => args.Context.Add(ToResource(x)));
            }
            else if (uri.ObjectType == ObjectTypes.Well)
            {
                _wellboreDataProvider.GetAll(uri)
                    .ForEach(x => args.Context.Add(ToResource(x)));
            }
            else if (uri.ObjectType == ObjectTypes.Wellbore)
            {
                _logDataProvider.GetAll(uri)
                    .ForEach(x => args.Context.Add(ToResource(x)));
            }
            else if (uri.ObjectType == ObjectTypes.Log)
            {
                var log = _logDataProvider.Get(uri);
                log.LogCurveInfo.ForEach(x => args.Context.Add(ToResource(log, x)));
            }
        }

        private Resource ToResource(Well entity)
        {
            return DiscoveryStoreProvider.New(
                uuid: entity.Uid,
                uri: entity.GetUri(),
                resourceType: ResourceTypes.DataObject,
                name: entity.Name,
                count: -1);
        }

        private Resource ToResource(Wellbore entity)
        {
            return DiscoveryStoreProvider.New(
                uuid: entity.Uid,
                uri: entity.GetUri(),
                resourceType: ResourceTypes.DataObject,
                name: entity.Name,
                count: -1);
        }

        private Resource ToResource(Log entity)
        {
            return DiscoveryStoreProvider.New(
                uuid: entity.Uid,
                uri: entity.GetUri(),
                resourceType: ResourceTypes.DataObject,
                name: entity.Name,
                count: -1);
        }

        private Resource ToResource(Log log, LogCurveInfo curve)
        {
            return DiscoveryStoreProvider.New(
                uuid: curve.Uid,
                uri: curve.GetUri(log),
                resourceType: ResourceTypes.DataObject,
                name: curve.Mnemonic);
        }
    }
}
