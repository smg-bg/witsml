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
using Energistics.DataAccess.WITSML131;
using PDS.Framework;
using System.Xml.Linq;

namespace PDS.Witsml.Server.Data.Messages
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="Message"/>.
    /// </summary>

    /// <seealso cref="PDS.Witsml.Server.Data.WitsmlDataProvider{MessageList, Message}" />
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<Message>))]
    [Export131(ObjectTypes.Message, typeof(IEtpDataProvider))]
    [Export131(ObjectTypes.Message, typeof(IWitsmlDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Message131DataProvider : WitsmlDataProvider<MessageList, Message>

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message131DataProvider"/> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="dataAdapter">The data adapter.</param>
        [ImportingConstructor]
        public Message131DataProvider(IContainer container, IWitsmlDataAdapter<Message> dataAdapter) : base(container, dataAdapter)
        {
        }

        /// <summary>
        /// Sets the default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        protected override void SetDefaultValues(Message dataObject)
        {
            dataObject.Uid = dataObject.NewUid();
            dataObject.CommonData = dataObject.CommonData.Create();

            SetAdditionalDefaultValues(dataObject);
        }

		/// <summary>
        /// Sets the default values for the specified data object during update.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
		/// <param name="element">The element.</param>
        protected override void UpdateDefaultValues(Message dataObject, XElement element)
        {
            UpdateAdditionalDefaultValues(dataObject, element);
        }

        /// <summary>
        /// Creates a new <see cref="MessageList" /> instance containing the specified data objects.
        /// </summary>
        /// <param name="dataObjects">The data objects.</param>
        /// <returns>A new <see cref="MessageList" /> instance.</returns>
        protected override MessageList CreateCollection(List<Message> dataObjects)
        {
            return new MessageList { Message = dataObjects };
        }

        /// <summary>
        /// Sets additional default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        partial void SetAdditionalDefaultValues(Message dataObject);

		/// <summary>
        /// Sets additional default values for the specified data object during update.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
		/// <param name="element">The element.</param>
        partial void UpdateAdditionalDefaultValues(Message dataObject, XElement element);

    }
}