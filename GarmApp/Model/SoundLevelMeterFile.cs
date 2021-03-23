using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GarmApp.Model
{
    public class SoundLevelMeterFile
    {
        private static readonly Regex fileLineExpression = new Regex(@"^(?<id>\d*)\s(?<unit>\w*)\s(?<value>\d*\.\d*)\s(?<date>\d*\-\d*-\d*)\,(?<timestamp>\d*\:\d*\:\d*)");

        public SoundLevelMeterFile(string filePath)
        {
            AxisNames = new List<string>() { nameof(SLMInput.Timestamp), nameof(SLMInput.Value), nameof(SLMInput.Date), nameof(SLMInput.Unit), nameof(SLMInput.Id) };
            XAxisName = AxisNames[0];
            YAxisName = AxisNames[1];
            Inputs = ParseFile(filePath).OrderBy(inp => inp.Id).ToList();
            
        }

        public List<string> AxisNames { get; private set; }

        public string XAxisName { get; set; }

        public string YAxisName { get; set; }

        public List<SLMInput> Inputs { get; }


        public static string CreateFile(SoundLevelMeterFile soundLevelMeterFile, bool useAbsoluteValues)
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


        private List<SLMInput> ParseFile(string filePath)
        {
            var result = new List<SLMInput>();
            var allLinesInFile = File.ReadAllLines(filePath);
            foreach (var line in allLinesInFile)
            {
                Match match = fileLineExpression.Match(line);
                if (match.Success)
                {
                    result.Add(new SLMInput(
                        Convert.ToInt32(match.Groups["id"].Value),
                        match.Groups["unit"].Value,
                        Convert.ToDouble(match.Groups["value"].Value),
                        DateTime.Parse(match.Groups["date"].Value),
                        DateTime.Parse(match.Groups["timestamp"].Value)));
                }
            }

            return result;
        }
    }

    public class SLMInput
    {
        public SLMInput(int id, string unit, double value, DateTime date, DateTime timestamp)
        {
            Id = id;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Value = value;
            Date = date;
            Timestamp = timestamp;
        }

        public int Id { get; }
        public string Unit { get; }
        public double Value { get; }
        public DateTime Date { get; }
        public DateTime Timestamp { get; set; }

    }
}
