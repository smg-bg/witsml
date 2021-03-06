//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.3
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
using System;
using System.Collections.Generic;
using System.Linq;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.ToolErrorTermSets
{
    public abstract partial class ToolErrorTermSet141TestBase : IntegrationTestBase
    {

        public const string QueryMissingNamespace = "<toolErrorTermSets version=\"1.4.1.1\"><toolErrorTermSet /></toolErrorTermSets>";
        public const string QueryInvalidNamespace = "<toolErrorTermSets xmlns=\"www.witsml.org/schemas/123\" version=\"1.4.1.1\"></toolErrorTermSets>";
        public const string QueryMissingVersion = "<toolErrorTermSets xmlns=\"http://www.witsml.org/schemas/1series\"></toolErrorTermSets>";
        public const string QueryEmptyRoot = "<toolErrorTermSets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"></toolErrorTermSets>";
        public const string QueryEmptyObject = "<toolErrorTermSets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><toolErrorTermSet /></toolErrorTermSets>";

        public const string BasicXMLTemplate = "<toolErrorTermSets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><toolErrorTermSet uid=\"{0}\">{1}</toolErrorTermSet></toolErrorTermSets>";

        public ToolErrorTermSet ToolErrorTermSet { get; set; }

        public DevKit141Aspect DevKit { get; set; }

        public List<ToolErrorTermSet> QueryEmptyList { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit141Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version141.Value)
                .ToArray();

            ToolErrorTermSet = new ToolErrorTermSet
            {

                Uid = DevKit.Uid(),
                Name = DevKit.Name("ToolErrorTermSet")
            };

            QueryEmptyList = DevKit.List(new ToolErrorTermSet());

            BeforeEachTest();
            OnTestSetUp();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AfterEachTest();
            OnTestCleanUp();
            DevKit.Container.Dispose();
            DevKit = null;
        }

        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected virtual void OnTestSetUp() { }

        protected virtual void OnTestCleanUp() { }

        protected virtual void AddParents()
        {

        }
    }
}