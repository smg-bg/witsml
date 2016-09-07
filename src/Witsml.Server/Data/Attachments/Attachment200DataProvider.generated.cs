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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML200;
using PDS.Framework;
using System.Xml.Linq;

namespace PDS.Witsml.Server.Data.Attachments
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="Attachment"/>.
    /// </summary>

    /// <seealso cref="PDS.Witsml.Server.Data.EtpDataProvider{Attachment}" />
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<Attachment>))]
    [Export200(ObjectTypes.Attachment, typeof(IEtpDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Attachment200DataProvider : EtpDataProvider<Attachment>

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment200DataProvider"/> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="dataAdapter">The data adapter.</param>
        [ImportingConstructor]
        public Attachment200DataProvider(IContainer container, IWitsmlDataAdapter<Attachment> dataAdapter) : base(container, dataAdapter)
        {
        }

    }
}