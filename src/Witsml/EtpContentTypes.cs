﻿//----------------------------------------------------------------------- 
// PDS.Witsml, 2016.1
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

using Energistics.Datatypes;

namespace PDS.Witsml
{
    /// <summary>
    /// Defines the supported list of ETP content types.
    /// </summary>
    public static class EtpContentTypes
    {
        /// <summary>
        /// The <see cref="EtpContentType"/> for prodml200
        /// </summary>
        public static readonly EtpContentType Prodml200 = new EtpContentType("application/x-prodml+xml;version=2.0");

        /// <summary>
        /// The <see cref="EtpContentType"/> for resqml200
        /// </summary>
        public static readonly EtpContentType Resqml200 = new EtpContentType("application/x-resqml+xml;version=2.0");

        /// <summary>
        /// The <see cref="EtpContentType"/> for resqml201
        /// </summary>
        public static readonly EtpContentType Resqml201 = new EtpContentType("application/x-resqml+xml;version=2.0.1");

        /// <summary>
        /// The <see cref="EtpContentType"/> for witsml131
        /// </summary>
        public static readonly EtpContentType Witsml131 = new EtpContentType("application/x-witsml+xml;version=1.3.1.1");

        /// <summary>
        /// The <see cref="EtpContentType"/> for witsml141
        /// </summary>
        public static readonly EtpContentType Witsml141 = new EtpContentType("application/x-witsml+xml;version=1.4.1.1");

        /// <summary>
        /// The <see cref="EtpContentType"/> for witsml200
        /// </summary>
        public static readonly EtpContentType Witsml200 = new EtpContentType("application/x-witsml+xml;version=2.0");
    }
}
