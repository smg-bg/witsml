//----------------------------------------------------------------------- 
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

namespace PDS.Witsml.Server.Data.Wells
{
    [TestClass]
    public partial class Well141ValidatorTests : Well141TestBase
    {

        #region Error -401

        public static readonly string QueryInvalidPluralRoot =
            "<well xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\">" + Environment.NewLine +
            "  <well>" + Environment.NewLine +
            "    <name>Test Plural Root Element</name>" + Environment.NewLine +
            "  </well>" + Environment.NewLine +
            "</well>";

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Well, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response.Result);
        }

        [TestMethod]
        public void Well141Validator_AddToStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.AddToStore(ObjectTypes.Well, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.UpdateInStore(ObjectTypes.Well, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        [TestMethod]
        public void Well141Validator_DeleteFromStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.DeleteFromStore(ObjectTypes.Well, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        #endregion Error -401

        #region Error -403

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_True_MissingNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Well, QueryMissingNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.True);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_True_BadNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Well, QueryInvalidNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.True);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_None_BadNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Well, QueryInvalidNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.None);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        #endregion Error -403

		#region Error -405

		[TestMethod]
        public void Well141Validator_AddToStore_Error_405_Well_Already_Exists()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
			DevKit.AddAndAssert<WellList, Well>(Well, ErrorCodes.DataObjectUidAlreadyExists);
        }

		#endregion Error -405

        #region Error -407

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_407_Well_Missing_Witsml_Object_Type()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
			var response = DevKit.Update<WellList, Well>(Well, string.Empty);
            Assert.IsNotNull(response);
            Assert.AreEqual((short)ErrorCodes.MissingWmlTypeIn, response.Result);
        }

		#endregion Error -407

        #region Error -408

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_408_Well_Empty_QueryIn()
        {
			var response = DevKit.UpdateInStore(ObjectTypes.Well, string.Empty, null, null);
            Assert.IsNotNull(response);
            Assert.AreEqual((short)ErrorCodes.MissingInputTemplate, response.Result);
        }

		#endregion Error -408

        #region Error -409

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_409_Well_QueryIn_Must_Conform_To_Schema()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);

            var nonConformingXml = string.Format(BasicXMLTemplate, Well.Uid,
                $"<name>{Well.Name}</name><name>{Well.Name}</name>");

            var response = DevKit.UpdateInStore(ObjectTypes.Well, nonConformingXml, null, null);
            Assert.AreEqual((short)ErrorCodes.InputTemplateNonConforming, response.Result);
        }

		#endregion Error -409

        #region Error -415

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_415_Well_Update_Without_Specifing_UID()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            Well.Uid = string.Empty;
			DevKit.UpdateAndAssert<WellList, Well>(Well, ErrorCodes.DataObjectUidMissing);
        }

		#endregion Error -415

        #region Error -433

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_433_Well_Does_Not_Exist()
        {
            AddParents();
			DevKit.UpdateAndAssert<WellList, Well>(Well, ErrorCodes.DataObjectNotExist);
        }

		#endregion Error -433

        #region Error -444

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_444_Well_Updating_More_Than_One_Data_Object()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);

            var updateXml = "<wells xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><well uid=\"{0}\"></well><well uid=\"{0}\"></well></wells>";
            updateXml = string.Format(updateXml, Well.Uid);

            var response = DevKit.UpdateInStore(ObjectTypes.Well, updateXml, null, null);
            Assert.AreEqual((short)ErrorCodes.InputTemplateMultipleDataObjects, response.Result);
        }

		#endregion Error -444

        #region Error -468

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_468_Well_No_Schema_Version_Declared()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            var response = DevKit.UpdateInStore(ObjectTypes.Well, QueryMissingVersion, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingDataSchemaVersion, response.Result);
        }

		#endregion Error -468

        #region Error -483

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_483_Well_Update_With_Non_Conforming_Template()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);
            var response = DevKit.UpdateInStore(ObjectTypes.Well, QueryEmptyRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.UpdateTemplateNonConforming, response.Result);
        }

		#endregion Error -483

        #region Error -484

		[TestMethod]
        public void Well141Validator_UpdateInStore_Error_484_Well_Update_Will_Delete_Required_Element()
        {
            AddParents();
            DevKit.AddAndAssert<WellList, Well>(Well);

            var nonConformingXml = string.Format(BasicXMLTemplate, Well.Uid,
                $"<name></name>");

            var response = DevKit.UpdateInStore(ObjectTypes.Well, nonConformingXml, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingRequiredData, response.Result);
        }

		#endregion Error -484

    }
}