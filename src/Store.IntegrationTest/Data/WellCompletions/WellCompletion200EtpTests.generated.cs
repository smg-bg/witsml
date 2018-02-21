﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Energistics.Common;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.DataAccess.WITSML200.ReferenceData;
using Energistics.Datatypes;
using Energistics.Protocol;
using Energistics.Protocol.Core;
using Energistics.Protocol.Discovery;
using Energistics.Protocol.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.WellCompletions
{
    [TestClass]
    public partial class WellCompletion200EtpTests : WellCompletion200TestBase
    {
        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected override void OnTestSetUp()
        {
            EtpSetUp(DevKit.Container);
            BeforeEachTest();
            _server.Start();
        }

        protected override void OnTestCleanUp()
        {
            _server?.Stop();
            EtpCleanUp();
            AfterEachTest();
        }

        [TestMethod]
        public void WellCompletion200_Ensure_Creates_WellCompletion_With_Default_Values()
        {
            DevKit.EnsureAndAssert(WellCompletion);
        }

        [TestMethod]
        public async Task WellCompletion200_GetResources_Can_Get_All_WellCompletion_Resources()
        {
            AddParents();
            DevKit.AddAndAssert(WellCompletion);
            await RequestSessionAndAssert();

            var uri = WellCompletion.GetUri();
            var parentUri = uri.Parent;

            await GetResourcesAndAssert(WellCompletion.Well.GetUri());

            var folderUri = parentUri.Append(uri.ObjectType);
            await GetResourcesAndAssert(folderUri);
        }

        [TestMethod]
        public async Task WellCompletion200_PutObject_Can_Add_WellCompletion()
        {
            AddParents();
            await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = WellCompletion.GetUri();

            var dataObject = CreateDataObject(uri, WellCompletion);

            // Get Object Expecting it Not to Exist
            await GetAndAssert(handler, uri, Energistics.EtpErrorCodes.NotFound);

            // Put Object
            await PutAndAssert(handler, dataObject);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetString();

            var result = Parse<WellCompletion>(xml);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task WellCompletion200_PutObject_Can_Update_WellCompletion()
        {
            AddParents();
            await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = WellCompletion.GetUri();

            // Add a ExtensionNameValue to Data Object
            var envName = "TestPutObject";
            var env = DevKit.ExtensionNameValue(envName, envName);
            WellCompletion.ExtensionNameValue = new List<ExtensionNameValue>() {env};

            var dataObject = CreateDataObject(uri, WellCompletion);

            // Get Object Expecting it Not to Exist
            await GetAndAssert(handler, uri, Energistics.EtpErrorCodes.NotFound);

            // Put Object for Add
            await PutAndAssert(handler, dataObject);

            // Get Added Object
            var args =await GetAndAssert(handler, uri);

            // Check Added Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetString();

            var result = Parse<WellCompletion>(xml);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ExtensionNameValue.FirstOrDefault(e => e.Name.Equals(envName)));

            // Remove Comment from Data Object
            result.ExtensionNameValue.Clear();

            var updateDataObject = CreateDataObject(uri, result);

            // Put Object for Update
            await PutAndAssert(handler, updateDataObject);

            // Get Updated Object
            args = await GetAndAssert(handler, uri);

            // Check Added Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var updateXml = args.Message.DataObject.GetString();

            result = Parse<WellCompletion>(updateXml);
            Assert.IsNotNull(result);

            // Test Data Object overwrite
            Assert.IsNull(result.ExtensionNameValue.FirstOrDefault(e => e.Name.Equals(envName)));
        }

        [TestMethod]
        public async Task WellCompletion200_DeleteObject_Can_Delete_WellCompletion()
        {
            AddParents();
            await RequestSessionAndAssert();

            var handler = _client.Handler<IStoreCustomer>();
            var uri = WellCompletion.GetUri();

            var dataObject = CreateDataObject(uri, WellCompletion);

            // Get Object Expecting it Not to Exist
            await GetAndAssert(handler, uri, Energistics.EtpErrorCodes.NotFound);

            // Put Object
            await PutAndAssert(handler, dataObject);

            // Get Object
            var args = await GetAndAssert(handler, uri);

            // Check Data Object XML
            Assert.IsNotNull(args?.Message.DataObject);
            var xml = args.Message.DataObject.GetString();

            var result = Parse<WellCompletion>(xml);
            Assert.IsNotNull(result);

            // Delete Object
            await DeleteAndAssert(handler, uri);

            // Get Object Expecting it Not to Exist
            await GetAndAssert(handler, uri, Energistics.EtpErrorCodes.NotFound);
        }
    }
}
