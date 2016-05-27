﻿//----------------------------------------------------------------------- 
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

using System.Linq;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ComponentSchemas;
using Energistics.DataAccess.WITSML131.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Logs
{
    [TestClass]
    public class Log131DataAdapterAddTests
    {
        private DevKit131Aspect DevKit;
        private Well Well;
        private Wellbore Wellbore;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            DevKit = new DevKit131Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version131.Value)
                .ToArray();

            Well = new Well { Name = DevKit.Name("Well 01"), TimeZone = DevKit.TimeZone };

            Wellbore = new Wellbore()
            {
                NameWell = Well.Name,
                Name = DevKit.Name("Wellbore 01")
            };
        }

        [TestMethod]
        public void Log_can_be_added_without_depth_data()
        {
            Well.Uid = "804415d0-b5e7-4389-a3c6-cdb790f5485f";
            Well.Name = "Test Well 1.3.1.1";

            // check if well already exists
            var wlResults = DevKit.Query<WellList, Well>(Well);
            if (!wlResults.Any())
            {
                DevKit.Add<WellList, Well>(Well);
            }

            Wellbore.Uid = "d3e7d4bf-0f29-4c2b-974d-4871cf8001fd";
            Wellbore.Name = "Test Wellbore 1.3.1.1";
            Wellbore.UidWell = Well.Uid;
            Wellbore.NameWell = Well.Name;

            // check if wellbore already exists
            var wbResults = DevKit.Query<WellboreList, Wellbore>(Wellbore);
            if (!wbResults.Any())
            {
                DevKit.Add<WellboreList, Wellbore>(Wellbore);
            }

            var log = new Log()
            {
                Uid = "e2401b72-550f-4695-ab27-d5b0589bde17",
                Name = "Test Depth Log 1.3.1.1",
                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name,
            };

            // check if log already exists
            var logResults = DevKit.Query<LogList, Log>(log);
            if (!logResults.Any())
            {
                DevKit.InitHeader(log, LogIndexType.measureddepth);
                var response = DevKit.Add<LogList, Log>(log);
                Assert.AreEqual((short)ErrorCodes.Success, response.Result);
            }
        }

        [TestMethod]
        public void Log_can_be_added_without_time_data()
        {
            Well.Uid = "804415d0-b5e7-4389-a3c6-cdb790f5485f";
            Well.Name = "Test Well 1.3.1.1";

            // check if well already exists
            var wlResults = DevKit.Query<WellList, Well>(Well);
            if (!wlResults.Any())
            {
                DevKit.Add<WellList, Well>(Well);
            }

            Wellbore.Uid = "d3e7d4bf-0f29-4c2b-974d-4871cf8001fd";
            Wellbore.Name = "Test Wellbore 1.3.1.1";
            Wellbore.UidWell = Well.Uid;
            Wellbore.NameWell = Well.Name;

            // check if wellbore already exists
            var wbResults = DevKit.Query<WellboreList, Wellbore>(Wellbore);
            if (!wbResults.Any())
            {
                DevKit.Add<WellboreList, Wellbore>(Wellbore);
            }

            var log = new Log()
            {
                Uid = "e2401b72-550f-4695-ab27-d5b0589bde18",
                Name = "Test Time Log 1.3.1.1",
                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name,
            };

            // check if log already exists
            var logResults = DevKit.Query<LogList, Log>(log);
            if (!logResults.Any())
            {
                DevKit.InitHeader(log, LogIndexType.datetime);
                var response = DevKit.Add<LogList, Log>(log);
                Assert.AreEqual((short)ErrorCodes.Success, response.Result);
            }
        }

        [TestMethod]
        public void Log_can_be_added_with_depth_data()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 10);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);
        }

        [TestMethod]
        public void Log_can_be_added_with_time_data()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01")
            };

            DevKit.InitHeader(log, LogIndexType.datetime);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 10, 1, false, false);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);
        }

        [TestMethod]
        public void Test_append_log_data()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(5, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 10);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(17, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 6);

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            var logData = result.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(16, logData.Count);
        }

        [TestMethod]
        public void Test_prepend_log_data()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(17, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 10);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(5, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 6);

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            var logData = result.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(16, logData.Count);
        }

        [TestMethod]
        public void Test_update_overlapping_log_data()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(1, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 8);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(4.1, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 3, 0.9);

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            var logData = result.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(9, logData.Count);
        }

        [TestMethod]
        public void Test_overwrite_log_data_chunk()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(17, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 6);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(4.1, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 3, 0.9);

            var logData = log.LogData;
            logData.Add("21.5, 1, 21.7");

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            logData = result.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(5, logData.Count);
        }

        [TestMethod]
        public void Test_update_log_data_with_different_range_for_each_channel()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(15, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 8);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(13, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 6, 0.9);

            var logData = log.LogData;
            logData.Clear();

            logData.Add("13,13.1,");
            logData.Add("14,14.1,");
            logData.Add("15,15.1,");
            logData.Add("16,16.1,");
            logData.Add("17,17.1,");
            logData.Add("20,20.1,20.2");
            logData.Add("21,,21.2");
            logData.Add("22,,22.2");
            logData.Add("23,,23.2");

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            logData = result.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(11, logData.Count);

            var data = logData;
            Assert.AreEqual("15,15.1,15", data[2]);
        }

        [TestMethod]
        public void Test_update_log_data_and_index_range()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01"),
                StartIndex = new GenericMeasure(15, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 8);

            // Make sure there are 3 curves
            var lciUids = log.LogCurveInfo.Select(l => l.Uid).ToArray();
            Assert.AreEqual(3, lciUids.Length);

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var uidWellbore = log.UidWellbore;
            var uidLog = response.SuppMsgOut;

            var query = new Log
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            var logAdded = results.FirstOrDefault();

            Assert.IsNotNull(logAdded);
            Assert.AreEqual(15, logAdded.StartIndex.Value);
            Assert.AreEqual(22, logAdded.EndIndex.Value);

            var mdCurve = DevKit.GetLogCurveInfoByUid(logAdded.LogCurveInfo, lciUids[0]) as LogCurveInfo;
            Assert.AreEqual(logAdded.StartIndex.Value, mdCurve.MinIndex.Value);
            Assert.AreEqual(logAdded.EndIndex.Value, mdCurve.MaxIndex.Value);

            var curve2 = DevKit.GetLogCurveInfoByUid(logAdded.LogCurveInfo, lciUids[1]) as LogCurveInfo;
            Assert.IsNull(curve2);

            var curve3 = DevKit.GetLogCurveInfoByUid(logAdded.LogCurveInfo, lciUids[2]) as LogCurveInfo;
            Assert.AreEqual(logAdded.StartIndex.Value, curve3.MinIndex.Value);
            Assert.AreEqual(logAdded.EndIndex.Value, curve3.MaxIndex.Value);

            log = new Log()
            {
                UidWell = Wellbore.UidWell,
                UidWellbore = uidWellbore,
                Uid = uidLog,
                StartIndex = new GenericMeasure(13, "m")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);
            DevKit.InitDataMany(log, DevKit.Mnemonics(log), DevKit.Units(log), 6, 0.9);

            var logData = log.LogData;
            logData.Clear();

            logData.Add("13,13.1,");
            logData.Add("14,14.1,");
            logData.Add("15,15.1,");
            logData.Add("16,16.1,");
            logData.Add("17,17.1,");
            logData.Add("20,20.1,20.2");
            logData.Add("21,,21.2");
            logData.Add("22,,22.2");
            logData.Add("23,,23.2");

            var updateResponse = DevKit.Update<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, updateResponse.Result);

            results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.All);
            Assert.AreEqual(1, results.Count);

            var logUpdated = results.First();
            logData = logUpdated.LogData;

            Assert.IsNotNull(logData);
            Assert.AreEqual(11, logData.Count);
            Assert.AreEqual(13, logUpdated.StartIndex.Value);
            Assert.AreEqual(23, logUpdated.EndIndex.Value);

            mdCurve = DevKit.GetLogCurveInfoByUid(logUpdated.LogCurveInfo, lciUids[0]) as LogCurveInfo;
            Assert.AreEqual(logUpdated.StartIndex.Value, mdCurve.MinIndex.Value);
            Assert.AreEqual(logUpdated.EndIndex.Value, mdCurve.MaxIndex.Value);

            curve2 = DevKit.GetLogCurveInfoByUid(logUpdated.LogCurveInfo, lciUids[1]) as LogCurveInfo;
            Assert.AreEqual(13, curve2.MinIndex.Value);
            Assert.AreEqual(20, curve2.MaxIndex.Value);

            curve3 = DevKit.GetLogCurveInfoByUid(logUpdated.LogCurveInfo, lciUids[2]) as LogCurveInfo;
            Assert.AreEqual(15, curve3.MinIndex.Value);
            Assert.AreEqual(23, curve3.MaxIndex.Value);
        }

        [TestMethod]
        public void Log131DataAdapter_AddToStore_Structural_Ranges_Ignored()
        {
            var response = DevKit.Add<WellList, Well>(Well);

            Wellbore.UidWell = response.SuppMsgOut;
            response = DevKit.Add<WellboreList, Wellbore>(Wellbore);

            var log = new Log()
            {
                UidWell = Wellbore.UidWell,
                NameWell = Well.Name,
                UidWellbore = response.SuppMsgOut,
                NameWellbore = Wellbore.Name,
                Name = DevKit.Name("Log 01")
            };

            DevKit.InitHeader(log, LogIndexType.measureddepth);

            log.StartIndex = new GenericMeasure { Uom = "m", Value = 1.0 };
            log.EndIndex = new GenericMeasure { Uom = "m", Value = 10.0 };

            foreach (var curve in log.LogCurveInfo)
            {
                curve.MinIndex = log.StartIndex;
                curve.MaxIndex = log.EndIndex;
            }

            response = DevKit.Add<LogList, Log>(log);
            Assert.AreEqual((short)ErrorCodes.Success, response.Result);

            var query = new Log
            {
                Uid = response.SuppMsgOut,
                UidWell = log.UidWell,
                UidWellbore = log.UidWellbore
            };

            var results = DevKit.Query<LogList, Log>(query, optionsIn: OptionsIn.ReturnElements.HeaderOnly);
            Assert.AreEqual(1, results.Count);

            var result = results.First();
            Assert.IsNotNull(result);

            Assert.IsNull(result.StartIndex);
            Assert.IsNull(result.EndIndex);

            Assert.AreEqual(log.LogCurveInfo.Count, result.LogCurveInfo.Count);
            foreach (var curve in result.LogCurveInfo)
            {
                Assert.IsNull(curve.MinIndex);
                Assert.IsNull(curve.MaxIndex);
            }
        }
    }
}
