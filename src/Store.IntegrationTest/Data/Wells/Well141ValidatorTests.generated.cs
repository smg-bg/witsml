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

namespace PDS.WITSMLstudio.Store.Data.Wells
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

        #region Error -402

        #endregion Error -402

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

        #region Error -416

        [TestMethod]
        public void Well141Validator_DeleteFromStore_Error_416_Well_Delete_With_Empty_UID()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            var deleteXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData><extensionNameValue uid=\"\" /></commonData>");

            var results = DevKit.DeleteFromStore(ObjectTypes.Well, deleteXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.EmptyUidSpecified, results.Result);
        }

        #endregion Error -416

        #region Error -418

        [TestMethod]
        public void Well141Validator_DeleteFromStore_Error_418_Well_Delete_With_Missing_Uid()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            var deleteXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData><extensionNameValue  uid=\"\" /></commonData>");

            var results = DevKit.DeleteFromStore(ObjectTypes.Well, deleteXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.EmptyUidSpecified, results.Result);
        }

        #endregion Error -418

        #region Error -419

        [TestMethod]
        public void Well141Validator_DeleteFromStore_Error_419_Well_Deleting_Empty_NonRecurring_Container_Element()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            var deleteXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData />");

            var results = DevKit.DeleteFromStore(ObjectTypes.Well, deleteXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.EmptyNonRecurringElementSpecified, results.Result);
        }

        #endregion Error -419

        #region Error -420

        [TestMethod]
        public void Well141Validator_DeleteFromStore_Error_420_Well_Specifying_A_Non_Recuring_Element_That_Is_Required()
        {

            DevKit.AddAndAssert(Well);

            var deleteXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<name />");
            var results = DevKit.DeleteFromStore(ObjectTypes.Well, deleteXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.EmptyMandatoryNodeSpecified, results.Result);
        }

        #endregion Error -420

        #region Error -433

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_433_Well_Does_Not_Exist()
        {
            AddParents();
            DevKit.UpdateAndAssert<WellList, Well>(Well, ErrorCodes.DataObjectNotExist);
        }

        #endregion Error -433

        #region Error -438

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_438_Well_Recurring_Elements_Have_Inconsistent_Selection()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            var ext2 = DevKit.ExtensionNameValue("Ext-2", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1, ext2
                }
            };

            DevKit.AddAndAssert(Well);

            var queryXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData>" +
                $"<extensionNameValue uid=\"\"><name>Ext-1</name></extensionNameValue>" +
                "<extensionNameValue uid=\"\"><value uom=\"\">1.0</value></extensionNameValue>" +
                "</commonData>");

            var results = DevKit.GetFromStore(ObjectTypes.Well, queryXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.RecurringItemsInconsistentSelection, results.Result);
        }

        #endregion Error -438

        #region Error -439

        [TestMethod]
        public void Well141Validator_GetFromStore_Error_439_Well_Recurring_Elements_Has_Empty_Selection_Value()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            var ext2 = DevKit.ExtensionNameValue("Ext-2", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1, ext2
                }
            };

            DevKit.AddAndAssert(Well);

            var queryXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData>" +
                $"<extensionNameValue uid=\"\"><name>Ext-1</name></extensionNameValue>" +
                "<extensionNameValue uid=\"\"><name></name></extensionNameValue>" +
                "</commonData>");

            var results = DevKit.GetFromStore(ObjectTypes.Well, queryXml, null, null);

            Assert.IsNotNull(results);
            Assert.AreEqual((short)ErrorCodes.RecurringItemsEmptySelection, results.Result);
        }

        #endregion Error -439

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

        #region Error -445

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_445_Well_Empty_New_Element()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            ext1 = DevKit.ExtensionNameValue("Ext-1", string.Empty, string.Empty, PrimitiveType.@double, string.Empty);
            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.UpdateAndAssert(Well, ErrorCodes.EmptyNewElementsOrAttributes);
        }

        #endregion Error -445

        #region Error -448

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_448_Well_Missing_Uid()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            var updateXml = string.Format(BasicXMLTemplate,Well.Uid,

                "<commonData>" +
                $"<extensionNameValue uid=\"\"><value uom=\"ft\" /></extensionNameValue>" +
                "</commonData>");

            var response = DevKit.UpdateInStore(ObjectTypes.Well, updateXml, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingElementUidForUpdate, response.Result);
        }

        #endregion Error -448

        #region Error -464

        [TestMethod]
        public void Well141Validator_AddToStore_Error_464_Well_Uid_Not_Unique()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");
            var ext2 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1, ext2
                }
            };

            DevKit.AddAndAssert(Well, ErrorCodes.ChildUidNotUnique);
        }

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_464_Well_Uid_Not_Unique()
        {

            var ext1 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1
                }
            };

            DevKit.AddAndAssert(Well);

            var ext2 = DevKit.ExtensionNameValue("Ext-1", "1.0", "m");

            Well.CommonData = new CommonData
            {
                ExtensionNameValue = new List<ExtensionNameValue>
                {
                    ext1, ext2
                }
            };

            DevKit.UpdateAndAssert(Well, ErrorCodes.ChildUidNotUnique);
        }

        #endregion Error -464

        #region Error -468

        [TestMethod]
        public void Well141Validator_UpdateInStore_Error_468_Well_No_Schema_Version_Declared()
        {

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

            DevKit.AddAndAssert<WellList, Well>(Well);

            var nonConformingXml = string.Format(BasicXMLTemplate, Well.Uid,

                $"<name></name>");

            var response = DevKit.UpdateInStore(ObjectTypes.Well, nonConformingXml, null, null);
            Assert.AreEqual((short)ErrorCodes.MissingRequiredData, response.Result);
        }

        #endregion Error -484

        #region Error -486

        [TestMethod]
        public void Well141Validator_AddToStore_Error_486_Well_Data_Object_Types_Dont_Match()
        {

            var xmlIn = string.Format(BasicXMLTemplate, Well.Uid,

                string.Empty);

            var response = DevKit.AddToStore(ObjectTypes.Wellbore, xmlIn, null, null);

            Assert.AreEqual((short)ErrorCodes.DataObjectTypesDontMatch, response.Result);
        }

        #endregion Error -486

    }
}
