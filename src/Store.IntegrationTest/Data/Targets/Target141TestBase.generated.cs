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
using System;
using System.Collections.Generic;
using System.Linq;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.Targets
{
    public abstract partial class Target141TestBase : IntegrationTestBase
    {

        public const string QueryMissingNamespace = "<targets version=\"1.4.1.1\"><target /></targets>";
        public const string QueryInvalidNamespace = "<targets xmlns=\"www.witsml.org/schemas/123\" version=\"1.4.1.1\"></targets>";
        public const string QueryMissingVersion = "<targets xmlns=\"http://www.witsml.org/schemas/1series\"></targets>";
        public const string QueryEmptyRoot = "<targets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"></targets>";
        public const string QueryEmptyObject = "<targets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><target /></targets>";

        public const string BasicXMLTemplate = "<targets xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><target uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\">{3}</target></targets>";

        public Well Well { get; set; }
        public Wellbore Wellbore { get; set; }
        public Target Target { get; set; }

        public DevKit141Aspect DevKit { get; set; }

        public List<Target> QueryEmptyList { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit141Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version141.Value)
                .ToArray();

            Well = new Well
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Well"),

                TimeZone = DevKit.TimeZone
            };
            Wellbore = new Wellbore
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Wellbore"),

                UidWell = Well.Uid,
                NameWell = Well.Name,
                MD = new MeasuredDepthCoord(0, MeasuredDepthUom.ft)

            };
            Target = new Target
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Target"),

                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name

            };

            QueryEmptyList = DevKit.List(new Target());

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

            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.AddAndAssert<WellboreList, Wellbore>(Wellbore);

        }
    }
}
