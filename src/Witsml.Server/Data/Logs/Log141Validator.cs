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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using PDS.Framework;
using PDS.Witsml.Server.Configuration;
using PDS.Witsml.Server.Properties;

namespace PDS.Witsml.Server.Data.Logs
{
    /// <summary>
    /// Provides validation for <see cref="Log" /> data objects.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.DataObjectValidator{Log}" />
    [Export(typeof(IDataObjectValidator<Log>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Log141Validator : DataObjectValidator<Log>
    {
        private readonly IWitsmlDataAdapter<Log> _logDataAdapter;
        private readonly IWitsmlDataAdapter<Wellbore> _wellboreDataAdapter;
        private readonly IWitsmlDataAdapter<Well> _wellDataAdapter;
        private static readonly int maxDataNodes = Settings.Default.MaxDataNodes;
        private static readonly int maxDataPoints = Settings.Default.MaxDataPoints;

        private static readonly char _seperator = ',';
        private readonly string[] _illegalColumnIdentifiers = new string[] { "'", "\"", "<", ">", "/", "\\", "&", "," };

        /// <summary>
        /// Initializes a new instance of the <see cref="Log141Validator" /> class.
        /// </summary>
        /// <param name="logDataAdapter">The log data adapter.</param>
        /// <param name="wellboreDataAdapter">The wellbore data adapter.</param>
        /// <param name="wellDataAdapter">The well data adapter.</param>
        [ImportingConstructor]
        public Log141Validator(IWitsmlDataAdapter<Log> logDataAdapter, IWitsmlDataAdapter<Wellbore> wellboreDataAdapter, IWitsmlDataAdapter<Well> wellDataAdapter)
        {
            _logDataAdapter = logDataAdapter;
            _wellboreDataAdapter = wellboreDataAdapter;
            _wellDataAdapter = wellDataAdapter;
        }


        [ImportMany]
        public IEnumerable<IWitsml141Configuration> Providers { get; set; }

        /// <summary>
        /// Validates the data object while executing GetFromStore
        /// </summary>
        /// <returns>A collection of validation results.</returns>
        protected override IEnumerable<ValidationResult> ValidateForGet()
        {
            var logDatas = DataObject.LogData;

            if (logDatas.Count>1)
            {
                yield return new ValidationResult(ErrorCodes.RecurringLogData.ToString(), new[] { "LogData", "LogData" });
            }
            else if (logDatas.Any(ld => ld.MnemonicList!=null && DuplicateUid(ld.MnemonicList.Split(_seperator))))
            {
                yield return new ValidationResult(ErrorCodes.DuplicateMnemonics.ToString(), new[] { "LogData", "Mnemonics" });
            }
        }

        /// <summary>
        /// Validates the data object while executing AddToStore.
        /// </summary>
        /// <returns>A collection of validation results.</returns>
        protected override IEnumerable<ValidationResult> ValidateForInsert()
        {
            var logCurves = DataObject.LogCurveInfo;
            var channelCount = logCurves != null ? logCurves.Count : 0;
            var uri = DataObject.GetUri();
            var uriWellbore = uri.Parent;
            var uriWell = uriWellbore.Parent;
            var wellbore = _wellboreDataAdapter.Get(uriWellbore);
            var indexCurve = DataObject.IndexCurve;

            var logDatas = DataObject.LogData;

            // Validate parent uid property
            if (string.IsNullOrWhiteSpace(DataObject.UidWell))
            {
                yield return new ValidationResult(ErrorCodes.MissingParentUid.ToString(), new[] { "UidWell" });
            }
            // Validate parent uid property
            else if (string.IsNullOrWhiteSpace(DataObject.UidWellbore))
            {
                yield return new ValidationResult(ErrorCodes.MissingParentUid.ToString(), new[] { "UidWellbore" });
            }

            // Validate parent exists
            else if (!_wellDataAdapter.Exists(uriWell))
            {
                yield return new ValidationResult(ErrorCodes.MissingParentDataObject.ToString(), new[] { "UidWell" });
            }
            // Validate parent exists
            else if (wellbore == null)
            {
                yield return new ValidationResult(ErrorCodes.MissingParentDataObject.ToString(), new[] { "UidWellbore" });
            }

            else if (!wellbore.UidWell.Equals(DataObject.UidWell) || !wellbore.Uid.Equals(DataObject.UidWellbore))
            {
                yield return new ValidationResult(ErrorCodes.IncorrectCaseParentUid.ToString(), new[] { "UidWellbore" });
            }

            // Validate UID does not exist
            else if (_logDataAdapter.Exists(uri))
            {
                yield return new ValidationResult(ErrorCodes.DataObjectUidAlreadyExists.ToString(), new[] { "Uid" });
            }

            // Validate that uid for LogParam exists
            else if (DataObject.LogParam != null && DataObject.LogParam.Any(lp => string.IsNullOrWhiteSpace(lp.Uid)))
            {
                yield return new ValidationResult(ErrorCodes.MissingElementUid.ToString(), new[] { "LogParam", "Uid" });
            }

            // Validate for a bad column identifier in LogCurveInfo Mnemonics
            else if (_illegalColumnIdentifiers.Any(s => logCurves.Any(m => m.Mnemonic.Value.Contains(s))))
            {
                yield return new ValidationResult(ErrorCodes.BadColumnIdentifier.ToString(), new[] { "LogCurveInfo.Mnemonic" });
            }

            // Validate that column-identifiers in LogCurveInfo are unique
            else if (DuplicateColumnIdentifier())
            {
                yield return new ValidationResult(ErrorCodes.DuplicateColumnIdentifiers.ToString(), new[] { "LogCurveInfo", "Mnemonic" });
            }

            // Validate that IndexCurve exists in LogCurveInfo
            else if (!string.IsNullOrEmpty(indexCurve) && logCurves != null
                && !logCurves.Any(lci => lci.Mnemonic != null && lci.Mnemonic.Value == indexCurve))
            {
                yield return new ValidationResult(ErrorCodes.IndexCurveNotFound.ToString(), new[] { "IndexCurve" });
            }

            // Validate structural-range indices for consistent index types
            else if ((DataObject.StartIndex != null || DataObject.EndIndex != null) && (DataObject.StartDateTimeIndex != null || DataObject.EndDateTimeIndex != null))
            {
                yield return new ValidationResult(ErrorCodes.MixedStructuralRangeIndices.ToString(), new[] { "StartIndex", "EndIndex", "StartDateTimeIndex", "EndDateTimeIndex" });
            }

            // Validate if MaxDataNodes has been exceeded
            else if (logDatas != null && logDatas.SelectMany(ld => ld.Data).Count() > maxDataNodes)
            {
                yield return new ValidationResult(ErrorCodes.MaxDataExceeded.ToString(), new[] { "LogData", "Data" });
            }

            // Validate if MaxDataPoints has been exceeded
            else if (logDatas != null && logDatas.Count > 0 )
            {
                if (logDatas.Any(l => string.IsNullOrWhiteSpace(l.MnemonicList)))
                    yield return new ValidationResult(ErrorCodes.MissingColumnIdentifiers.ToString(), new[] { "LogData", "MnemonicList" });
                else
                {
                    foreach (var logData in logDatas)
                    {
                        var mnemonics = logData.MnemonicList.Split(_seperator);
                        if (logData.Data != null && logData.Data.Count > 0 && (logData.Data.Count * logData.Data[0].Split(',').Count()) > maxDataPoints)
                            yield return new ValidationResult(ErrorCodes.MaxDataExceeded.ToString(), new[] { "LogData", "Data" });
                        else if (mnemonics.Distinct().Count() < mnemonics.Count())
                            yield return new ValidationResult(ErrorCodes.DuplicateColumnIdentifiers.ToString(), new[] { "LogData", "MnemonicList" });
                        else if (mnemonics.Any(m => _illegalColumnIdentifiers.Any(c => m.Contains(c))))
                            yield return new ValidationResult(ErrorCodes.BadColumnIdentifier.ToString(), new[] { "LogData", "MnemonicList" });
                        else if (!string.IsNullOrEmpty(indexCurve) && !mnemonics.Any(m => m == indexCurve))
                            yield return new ValidationResult(ErrorCodes.IndexCurveNotFound.ToString(), new[] { "IndexCurve" });
                        else if (!mnemonics[0].EqualsIgnoreCase(DataObject.IndexCurve))
                            yield return new ValidationResult(ErrorCodes.IndexNotFirstInDataColumnList.ToString(), new[] { "LogData", "MnemonicList" });
                        else if (!UnitSpecified(logDatas))
                            yield return new ValidationResult(ErrorCodes.MissingUnitForMeasureData.ToString(), new[] { "LogData", "UnitList" });
                        else if (mnemonics.Count() > channelCount)
                            yield return new ValidationResult(ErrorCodes.BadColumnIdentifier.ToString(), new[] { "LogData", "MnemonicList" });
                    }
                    
                }
            }
        }

        /// <summary>
        /// Validates the data object while executing UpdateInStore.
        /// </summary>
        /// <returns>
        /// A collection of validation results.
        /// </returns>
        protected override IEnumerable<ValidationResult> ValidateForUpdate()
        {
            // Validate Log uid property
            if (string.IsNullOrWhiteSpace(DataObject.UidWell) || string.IsNullOrWhiteSpace(DataObject.UidWellbore) || string.IsNullOrWhiteSpace(DataObject.Uid))
            {
                yield return new ValidationResult(ErrorCodes.DataObjectUidMissing.ToString(), new[] { "Uid", "UidWell", "UidWellbore" });
            }
            else
            {
                var uri = DataObject.GetUri();
                var logCurves = DataObject.LogCurveInfo;
                var logParams = DataObject.LogParam;
                var logData = DataObject.LogData;
                var current = ((WitsmlDataAdapter<Log>)_logDataAdapter).Get(uri);

                // Validate Log does not exist
                if (current == null)
                {
                    yield return new ValidationResult(ErrorCodes.DataObjectNotExist.ToString(), new[] { "Uid", "UidWell", "UidWellbore" });
                }

                // Validate that uid for LogCurveInfo exists
                else if (logCurves != null && logCurves.Any(l => string.IsNullOrWhiteSpace(l.Uid)))
                {
                    yield return new ValidationResult(ErrorCodes.MissingElementUid.ToString(), new[] { "LogCurveInfo", "Uid" });
                }

                // Validate that uid for LogParam exists
                else if (logParams != null && logParams.Any(lp => string.IsNullOrWhiteSpace(lp.Uid)))
                {
                    yield return new ValidationResult(ErrorCodes.MissingElementUid.ToString(), new[] { "LogParam", "Uid" });
                }

                // Validate that uids in LogCurveInfo are unique
                else if (logCurves != null && DuplicateUid(logCurves.Select(l => l.Uid)))
                {
                    yield return new ValidationResult(ErrorCodes.ChildUidNotUnique.ToString(), new[] { "LogCurveInfo", "Uid" });
                }

                // Validate that uids in LogParam are unique
                else if (logParams != null && DuplicateUid(logParams.Select(l => l.Uid)))
                {
                    yield return new ValidationResult(ErrorCodes.ChildUidNotUnique.ToString(), new[] { "LogParam", "Uid" });
                }

                else if (DuplicateColumnIdentifier())
                {
                    yield return new ValidationResult(ErrorCodes.DuplicateColumnIdentifiers.ToString(), new[] { "LogCurveInfo", "Mnemonic" });
                }

                // Validate that uids in LogCurveInfo are unique
                else if (logData != null && logData.Any(d => string.IsNullOrWhiteSpace(d.MnemonicList)))
                {
                    yield return new ValidationResult(ErrorCodes.MissingColumnIdentifiers.ToString(), new[] { "LogData", "MnemonicList" });
                }

                // Validate that uids in LogCurveInfo are unique
                else if (logData != null && logData.Any(d => string.IsNullOrWhiteSpace(d.UnitList)))
                {
                    yield return new ValidationResult(ErrorCodes.MissingUnitList.ToString(), new[] { "LogData", "UnitList" });
                }

                // Validate that LogCurveInfo index should not be specified
                else if (logCurves != null)
                {
                    var isTimeLog = current.IndexType == LogIndexType.datetime || current.IndexType == LogIndexType.elapsedtime;
                    var exist = current.LogCurveInfo != null ? current.LogCurveInfo : new List<LogCurveInfo>();
                    var uids = exist.Select(e => e.Uid.ToUpper()).ToList();
                    var newCurves = logCurves.Where(l => !uids.Contains(l.Uid.ToUpper())).ToList();
                    var updateCurves = logCurves.Where(l => uids.Contains(l.Uid.ToUpper())).ToList();
                    if (newCurves.Count > 0 && updateCurves.Count > 1)
                        yield return new ValidationResult(ErrorCodes.AddingUpdatingLogCurveAtTheSameTime.ToString(), new[] { "LogCurveInfo", "Uid" });

                    else if (isTimeLog && newCurves.Any(c => c.MinDateTimeIndex.HasValue || c.MaxDateTimeIndex.HasValue)
                        || !isTimeLog && newCurves.Any(c => c.MinIndex != null || c.MaxIndex != null))
                    {
                        yield return new ValidationResult(ErrorCodes.IndexRangeSpecified.ToString(), new[] { "LogCurveInfo", "Index" });
                    }
                    else if (logData != null && logData.Count > 0)
                    {
                        var indexCurve = logCurves.Count > 0 ? logCurves.First().Mnemonic.Value : current.IndexCurve;
                        yield return ValidateLogData(indexCurve, logCurves, logData);
                    }
                }

                // Validate LogData mnemonic list
                else if (logData != null && logData.Count > 0)
                {
                    yield return ValidateLogData(current.IndexCurve, logCurves, logData);
                }
            }
        }

        private bool DuplicateUid(IEnumerable<string> uids)
        {
            return uids.GroupBy(u => u)
                .Select(group => new { Uid = group.Key, Count = group.Count() })
                .Any(g => g.Count > 1);
        }

        private bool DuplicateColumnIdentifier()
        {
            var logCurves = DataObject.LogCurveInfo;
            if (logCurves == null || logCurves.Count == 0)
                return false;

            return logCurves.Where(l => l.Mnemonic != null).GroupBy(lci => lci.Mnemonic.Value)
                .Select(group => new { Mnemonic = group.Key, Count = group.Count() })
                .Any(g => g.Count > 1);
        }

        private bool UnitsMatch(List<LogCurveInfo> logCurves, LogData logData)
        {
            var mnemonics = logData.MnemonicList.Split(_seperator);
            var units = logData.UnitList.Split(_seperator);

            for (var i = 0; i < mnemonics.Length; i++)
            {
                var mnemonic = mnemonics[i];
                var logCurve = logCurves.FirstOrDefault(l => l.Mnemonic.Value.EqualsIgnoreCase(mnemonic));
                if (logCurve == null)
                    continue;

                if (!units[i].EqualsIgnoreCase(logCurve.Unit))
                    return false;
            }
            return true;
        }

        private ValidationResult ValidateLogData(string indexCurve, List<LogCurveInfo> logCurves, List<LogData> logData)
        {
            if (logData.Any(ld => !ld.MnemonicList.Split(_seperator).Contains(indexCurve)))
            {
                return new ValidationResult(ErrorCodes.IndexCurveNotFound.ToString(), new[] { "LogData", "MnemonicList" });
            }
            else if (logData.Any(ld => DuplicateUid(ld.MnemonicList.Split(_seperator))))
            {
                return new ValidationResult(ErrorCodes.MnemonicsNotUnique.ToString(), new[] { "LogData", "MnemonicList" });
            }
            else if (!UnitSpecified(logData))
            {
                return new ValidationResult(ErrorCodes.MissingUnitForMeasureData.ToString(), new[] { "LogData", "UnitList" });
            }
            else if (logCurves != null && logData.Any(ld => !UnitsMatch(logCurves, ld)))
            {
                return new ValidationResult(ErrorCodes.UnitListNotMatch.ToString(), new[] { "LogData", "UnitList" });
            }

            return null;
        }

        private bool UnitSpecified(List<LogData> logDatas)
        {
            foreach (var logData in logDatas)
            {
                var data = logData.Data;
                if (data == null || data.Count == 0)
                    continue;

                var firstRow = data.First().Split(_seperator);
                var units = logData.UnitList.Split(_seperator);
                for (var i = 0; i < firstRow.Length; i++)
                {
                    if (units.Length <= i || string.IsNullOrWhiteSpace(units[i]))
                        return false;
                }
            }

            return true; ;
        }
    }
}
