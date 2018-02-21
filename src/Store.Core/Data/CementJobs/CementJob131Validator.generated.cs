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
using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML131;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.CementJobs
{
    /// <summary>
    /// Provides validation for <see cref="CementJob" /> data objects.
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Store.Data.DataObjectValidator{CementJob}" />
    [Export(typeof(IDataObjectValidator<CementJob>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CementJob131Validator : DataObjectValidator<CementJob, Wellbore, Well>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CementJob131Validator" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="cementJobDataAdapter">The cementJob data adapter.</param>
        /// <param name="wellboreDataAdapter">The wellbore data adapter.</param>
        /// <param name="wellDataAdapter">The well data adapter.</param>
        [ImportingConstructor]
        public CementJob131Validator(
            IContainer container,
            IWitsmlDataAdapter<CementJob> cementJobDataAdapter,
            IWitsmlDataAdapter<Wellbore> wellboreDataAdapter,
            IWitsmlDataAdapter<Well> wellDataAdapter)
            : base(container, cementJobDataAdapter, wellboreDataAdapter, wellDataAdapter)
        {
        }
    }
}
