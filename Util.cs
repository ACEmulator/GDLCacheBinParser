using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using PhatACCacheBinParser.Seg1_RegionDesc;

namespace PhatACCacheBinParser
{
	static class Util
	{
		public static readonly Regex IllegalInFileName = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Compiled);


		public static string ReadString(BinaryReader binaryReader, bool alignTo4Bytes)
		{
			var len = binaryReader.ReadUInt16();

			var bytes = binaryReader.ReadBytes(len);

			if (alignTo4Bytes)
			{
				// Make sure our position is a multiple of 4
				if (binaryReader.BaseStream.Position % 4 != 0)
					binaryReader.BaseStream.Position += 4 - (binaryReader.BaseStream.Position % 4);
			}

			return Encoding.ASCII.GetString(bytes);
		}

		/// <summary>
		/// Each byte of the string has it's upper and lower nibble swapped.
		/// </summary>
		public static string ReadEncryptedString1(BinaryReader binaryReader, bool alignTo4Bytes)
		{
			var len = binaryReader.ReadUInt16();

			var bytes = binaryReader.ReadBytes(len);

			for (int i = 0; i < bytes.Length; i++)
			{
				var b = bytes[i];
				bytes[i] = (byte)((b >> 4) | (b << 4));
			}

			if (alignTo4Bytes)
			{
				// Make sure our position is a multiple of 4
				if (binaryReader.BaseStream.Position % 4 != 0)
					binaryReader.BaseStream.Position += 4 - (binaryReader.BaseStream.Position % 4);
			}

			return Encoding.ASCII.GetString(bytes);
		}

	    public static int ReadPackedKnownType(BinaryReader binaryReader, int knownType)
	    {
	        int result = binaryReader.ReadUInt16();

	        if ((result & 0x8000) == 0x8000)
	        {
	            var lower = binaryReader.ReadUInt16();
                result = ((result & 0x3FFF) << 16) | lower; // Should this be masked with 0x7FFF instead?
	        }

            return knownType + result;
	    }


		public static List<T> GetParsedObjects<T>(ParserControl parserControl, BinaryReader binaryReader) where T : IParseableObject, new()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			// For Segment 1, the first dword appears to simply be an is present flag
			// The value is 256, which is probably tied to the number of landblocks or landblock width or something.
			if (typeof(T).IsAssignableFrom(typeof(RegionDesc)) && totalObjects > 0)
				totalObjects = 1;

			// ReSharper disable once NotAccessedVariable
			T lastParsed; // Used for debugging

			var parsedObjects = new List<T>();

			while (parsedObjects.Count < totalObjects)
			{
				var parsedObject = new T();

				parsedObject.Parse(binaryReader);

				parsedObjects.Add(parsedObject);

				// ReSharper disable once RedundantAssignment
				lastParsed = parsedObject; // Used for debugging

				if ((parsedObjects.Count % 100) == 0)
					parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = (int)(((double)parsedObjects.Count / totalObjects) * 100)));
			}

			parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = (int)(((double)parsedObjects.Count / totalObjects) * 100)));

			return parsedObjects;
		}

		public static void WriteJSONOutput<T>(ParserControl parserControl, List<T> parsedObjects, string outputFolder, Func<T, string> fileNameFormatter) where T : IParseableObject, new()
		{
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			JsonSerializer serializer = new JsonSerializer();
			serializer.Converters.Add(new JavaScriptDateTimeConverter());
			serializer.NullValueHandling = NullValueHandling.Ignore;

			int processedCounter = 0;

			Parallel.For(0, parsedObjects.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + fileNameFormatter(parsedObjects[i]) + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					serializer.Serialize(writer, parsedObjects[i]);

					var counter = Interlocked.Increment(ref processedCounter);

					if ((counter % 1000) == 0)
						parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = (int)(((double)counter / parsedObjects.Count) * 100)));
				}
			});

			parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
		}
	}
}
