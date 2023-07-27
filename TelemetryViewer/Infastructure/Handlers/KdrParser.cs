using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TelemetryViewer.Models;

namespace TelemetryViewer.Infastructure.Handlers
{
    public class KdrParser
    {
        public static async Task<List<FrameModel>> ParseFileAsync(string filePath)
        {
            var frameModels = new List<FrameModel>();
            var fileContent = await File.ReadAllTextAsync(filePath);
            var frameStrings = fileContent.Split(new[] { "=KADR=     " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string frameString in frameStrings)
            {
                frameModels.Add(ParseFrameString(frameString));
            }

            return frameModels;
        }

        private static FrameModel ParseFrameString(string frameString)
        {
            var frameModel = new FrameModel();
            var words = frameString.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length != 543)
            {
                throw new ArgumentException("Неверный формат кадра");
            }

            var headers = new List<ushort>();
            var body = new List<ushort>();

            for (int i = 0; i < 31; i++)
            {
                if (ushort.TryParse(words[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort headerValue))
                {
                    headers.Add(headerValue);
                }
            }

            for (int i = 31; i < 543; i++)
            {
                if (ushort.TryParse(words[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort bodyValue))
                {
                    body.Add((ushort)((bodyValue & 0x01FE) >> 1));
                }
            }

            frameModel.Headers = headers.ToArray();
            frameModel.Body = body.ToArray();

            return frameModel;

        }
    }
}
