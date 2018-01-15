using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PhatACCacheBinParser
{
	abstract class Segment
	{
		protected static JsonSerializer Serializer = new JsonSerializer();

		static Segment()
		{
			Serializer.Converters.Add(new JavaScriptDateTimeConverter());
			Serializer.NullValueHandling = NullValueHandling.Ignore;
		}


		private bool parsed;

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public virtual bool Parse(BinaryReader binaryReader)
		{
			if (parsed)
				throw new InvalidOperationException();

			parsed = true;

			return false;
		}

		public virtual bool WriteJSONOutput(string outputFolder)
		{
			if (!parsed)
				throw new InvalidOperationException();

			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			return false;
		}
	}
}
