using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace GarmApp.Model
{
    public class ExtechVibrationMeterFile
    {
        private static readonly Regex fileLineExpression = new Regex(@"^(?<id>\d*)\s(?<date>\d*\/\d*\/\d*)\s(?<timestamp>\d*\:\d*\:\d*)\s(?<value>\d*\,\d*)\s(?<type>\w*)\s(?<unit>\S*)");

        public ExtechVibrationMeterFile(string filePath)
        {
            AxisNames = new List<string>() { nameof(EVMInput.Timestamp), nameof(EVMInput.Value), nameof(EVMInput.Date), nameof(EVMInput.Unit), nameof(EVMInput.Id), nameof(EVMInput.Type) };
            XAxisName = AxisNames[0];
            YAxisName = AxisNames[1];
            Inputs = ParseFile(filePath).OrderBy(inp => inp.Id).ToList();

        }

        public List<string> AxisNames { get; private set; }

        public string XAxisName { get; set; }

        public string YAxisName { get; set; }

        public List<EVMInput> Inputs { get; }


        public static string CreateFile(ExtechVibrationMeterFile soundLevelMeterFile, bool useAbsoluteValues)
        {
            var result = string.Empty;

            if (useAbsoluteValues)
            {
                var firstInputTimeStamp = soundLevelMeterFile.Inputs.FirstOrDefault().Timestamp;
                soundLevelMeterFile.Inputs.ForEach(input => input.Timestamp = new DateTime() + (input.Timestamp - firstInputTimeStamp));
            }

            if (soundLevelMeterFile.XAxisName == soundLevelMeterFile.YAxisName)
                return result;

            result += $"{soundLevelMeterFile.XAxisName}\t{soundLevelMeterFile.YAxisName}\n";
            foreach (var input in soundLevelMeterFile.Inputs)
            {
                var xAxisType = input!.GetType()?.GetProperty(soundLevelMeterFile.XAxisName)?.GetValue(input);
                var yAxisType = input!.GetType()?.GetProperty(soundLevelMeterFile.YAxisName)?.GetValue(input);

                string? firstOutputParameter = string.Empty;
                string? secondOutputParameter = string.Empty;

                if (xAxisType is DateTime xDateTime)
                    firstOutputParameter = xDateTime.ToString("HH:mm:ss");
                else
                    firstOutputParameter = xAxisType?.ToString();

                if (yAxisType is DateTime yDateTime)
                    secondOutputParameter = yDateTime.ToString("HH:mm:ss");
                else
                    secondOutputParameter = yAxisType?.ToString();

                result += $"{firstOutputParameter}\t{secondOutputParameter}\n";
            }

            return result;
        }


        private List<EVMInput> ParseFile(string filePath)
        {
            var result = new List<EVMInput>();
            var allLinesInFile = File.ReadAllLines(filePath);
            foreach (var line in allLinesInFile)
            {
                Match match = fileLineExpression.Match(line);
                if (match.Success)
                {
                    result.Add(new EVMInput(
                        Convert.ToInt32(match.Groups["id"].Value),
                        match.Groups["unit"].Value,
                        match.Groups["type"].Value,
                        Convert.ToDouble(match.Groups["value"].Value.Replace(",",".")),
                        DateTime.Parse(match.Groups["date"].Value),
                        DateTime.Parse(match.Groups["timestamp"].Value)));
                }
            }

            return result;
        }
    }

    public class EVMInput
    {
        public EVMInput(int id, string unit, string type, double value, DateTime date, DateTime timestamp)
        {
            Id = id;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value;
            Date = date;
            Timestamp = timestamp;
        }

        public int Id { get; }
        public string Unit { get; }
        public string Type { get; }
        public double Value { get; }
        public DateTime Date { get; }
        public DateTime Timestamp { get; set; }

    }
}
