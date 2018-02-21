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
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.WITSMLstudio.Store.Data.Logs
{
    [TestClass]
    public partial class Log131ValidatorTests : Log131TestBase
    {

        #region Error -401

        public static readonly string QueryInvalidPluralRoot =
            "<log xmlns=\"http://www.witsml.org/schemas/131\" version=\"1.3.1.1\">" + Environment.NewLine +
            "  <log>" + Environment.NewLine +
            "    <name>Test Plural Root Element</name>" + Environment.NewLine +
            "  </log>" + Environment.NewLine +
            "</log>";

        [TestMethod]
        public void Log131Validator_GetFromStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Log, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response.Result);
        }

        [TestMethod]
        public void Log131Validator_AddToStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.AddToStore(ObjectTypes.Log, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.UpdateInStore(ObjectTypes.Log, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        [TestMethod]
        public void Log131Validator_DeleteFromStore_Error_401_No_Plural_Root_Element()
        {
            var response = DevKit.DeleteFromStore(ObjectTypes.Log, QueryInvalidPluralRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingPluralRootElement, response?.Result);
        }

        #endregion Error -401

        #region Error -402

        #endregion Error -402

        #region Error -403

        [TestMethod]
        public void Log131Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_True_MissingNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Log, QueryMissingNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.True);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        [TestMethod]
        public void Log131Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_True_BadNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Log, QueryInvalidNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.True);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        [TestMethod]
        public void Log131Validator_GetFromStore_Error_403_RequestObjectSelectionCapability_None_BadNamespace()
        {
            var response = DevKit.GetFromStore(ObjectTypes.Log, QueryInvalidNamespace, null, optionsIn: OptionsIn.RequestObjectSelectionCapability.None);
            Assert.AreEqual((short)ErrorCodes.MissingDefaultWitsmlNamespace, response.Result);
        }

        #endregion Error -403

        #region Error -405

        [TestMethod]
        public void Log131Validator_AddToStore_Error_405_Log_Already_Exists()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            DevKit.AddAndAssert<LogList, Log>(Log, ErrorCodes.DataObjectUidAlreadyExists);
        }

        #endregion Error -405

        #region Error -406

        [TestMethod]
        public void Log131Validator_AddToStore_Error_406_Log_Missing_Parent_Uid()
        {
            AddParents();

            Log.UidWellbore = null;

            DevKit.AddAndAssert(Log, ErrorCodes.MissingElementUidForAdd);
        }

        #endregion Error -406

        #region Error -407

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_407_Log_Missing_Witsml_Object_Type()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            var response = DevKit.Update<LogList, Log>(Log, string.Empty);
            Assert.IsNotNull(response);
            Assert.AreEqual((short)ErrorCodes.MissingWmlTypeIn, response.Result);
        }

        #endregion Error -407

        #region Error -408

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_408_Log_Empty_QueryIn()
        {
            var response = DevKit.UpdateInStore(ObjectTypes.Log, string.Empty, null, null);
            Assert.IsNotNull(response);
            Assert.AreEqual((short)ErrorCodes.MissingInputTemplate, response.Result);
        }

        #endregion Error -408

        #region Error -409

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_409_Log_QueryIn_Must_Conform_To_Schema()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);

            var nonConformingXml = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid,

                $"<name>{Log.Name}</name><name>{Log.Name}</name>");

            var response = DevKit.UpdateInStore(ObjectTypes.Log, nonConformingXml, null, null);
            Assert.AreEqual((short)ErrorCodes.InputTemplateNonConforming, response.Result);
        }

        #endregion Error -409

        #region Error -415

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_415_Log_Update_Without_Specifing_UID()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            Log.Uid = string.Empty;
            DevKit.UpdateAndAssert<LogList, Log>(Log, ErrorCodes.DataObjectUidMissing);
        }

        #endregion Error -415

        #region Error -420

        [TestMethod]
        public void Log131Validator_DeleteFromStore_Error_420_Log_Specifying_A_Non_Recuring_Element_That_Is_Required()
        {

            AddParents();

            DevKit.AddAndAssert(Log);

            var deleteXml = string.Format(BasicXMLTemplate,Log.UidWell, Log.UidWellbore,Log.Uid,

                "<name />");
            var results = DevKit.DeleteFromStore(ObjectTypes.Log, deleteXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.EmptyMandatoryNodeSpecified, results.Result);
        }

        #endregion Error -420

        #region Error -433

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_433_Log_Does_Not_Exist()
        {
            AddParents();
            DevKit.UpdateAndAssert<LogList, Log>(Log, ErrorCodes.DataObjectNotExist);
        }

        #endregion Error -433

        #region Error -444

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_444_Log_Updating_More_Than_One_Data_Object()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);

            var updateXml = "<logs xmlns=\"http://www.witsml.org/schemas/131\" version=\"1.3.1.1\"><log uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\"></log><log uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\"></log></logs>";
            updateXml = string.Format(updateXml, Log.UidWell, Log.UidWellbore, Log.Uid);

            var response = DevKit.UpdateInStore(ObjectTypes.Log, updateXml, null, null);
            Assert.AreEqual((short)ErrorCodes.InputTemplateMultipleDataObjects, response.Result);
        }

        #endregion Error -444

        #region Error -468

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_468_Log_No_Schema_Version_Declared()
        {

            AddParents();

            DevKit.AddAndAssert<LogList, Log>(Log);
            var response = DevKit.UpdateInStore(ObjectTypes.Log, QueryMissingVersion, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingDataSchemaVersion, response.Result);
        }

        #endregion Error -468

        #region Error -478

        [TestMethod]
        public void Log131Validator_AddToStore_Error_478_Log_Parent_Uid_Case_Not_Matching()
        {

            Well.Uid = Well.Uid.ToUpper();
            Wellbore.Uid = Wellbore.Uid.ToUpper();
            Wellbore.UidWell = Well.Uid.ToUpper();
            AddParents();

            Log.UidWell = Well.Uid.ToLower();

            DevKit.AddAndAssert(Log, ErrorCodes.IncorrectCaseParentUid);
        }

        #endregion Error -478

        #region Error -481

        [TestMethod]
        public void Log131Validator_AddToStore_Error_481_Log_Parent_Does_Not_Exist()
        {
            DevKit.AddAndAssert(Log, ErrorCodes.MissingParentDataObject);
        }

        #endregion Error -481

        #region Error -483

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_483_Log_Update_With_Non_Conforming_Template()
        {
            AddParents();
            DevKit.AddAndAssert<LogList, Log>(Log);
            var response = DevKit.UpdateInStore(ObjectTypes.Log, QueryEmptyRoot, null, null);
            Assert.AreEqual((short)ErrorCodes.UpdateTemplateNonConforming, response.Result);
        }

        #endregion Error -483

        #region Error -484

        [TestMethod]
        public void Log131Validator_UpdateInStore_Error_484_Log_Update_Will_Delete_Required_Element()
        {

            AddParents();

            DevKit.AddAndAssert<LogList, Log>(Log);

            var nonConformingXml = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid,

                $"<name></name>");

            var response = DevKit.UpdateInStore(ObjectTypes.Log, nonConformingXml, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingRequiredData, response.Result);
        }

        #endregion Error -484

        #region Error -486

        [TestMethod]
        public void Log131Validator_AddToStore_Error_486_Log_Data_Object_Types_Dont_Match()
        {

            AddParents();

            var xmlIn = string.Format(BasicXMLTemplate, Log.UidWell, Log.UidWellbore, Log.Uid,

                string.Empty);

            var response = DevKit.AddToStore(ObjectTypes.Well, xmlIn, null, null);

            Assert.AreEqual((short)ErrorCodes.DataObjectTypesDontMatch, response.Result);
        }

        #endregion Error -486

    }
}
