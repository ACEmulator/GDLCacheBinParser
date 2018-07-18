using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser
{
	static class UnpackableExtensions
	{
		public static bool Unpack<T>(this List<T> value, BinaryReader binaryReader) where T : IUnpackable, new()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var item = new T();
				item.Unpack(binaryReader);
				value.Add(item);
			}

			return true;
		}

		public static bool Unpack<T>(this List<T> value, BinaryReader binaryReader, int fixedQuantity) where T : IUnpackable, new()
		{
			for (int i = 0; i < fixedQuantity; i++)
			{
				var item = new T();
				item.Unpack(binaryReader);
				value.Add(item);
			}

			return true;
		}

		public static bool Unpack<T>(this List<List<T>> value, BinaryReader binaryReader) where T : IUnpackable, new()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var items = new List<T>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new T();
					item.Unpack(binaryReader);
					items.Add(item);
				}

				value.Add(items);
			}

			return true;
		}

		public static bool Unpack<T>(this List<List<T>> value, BinaryReader binaryReader, int fixedQuantity) where T : IUnpackable, new()
		{
			for (int i = 0; i < fixedQuantity; i++)
			{
				var items = new List<T>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new T();
					item.Unpack(binaryReader);
					items.Add(item);
				}

				value.Add(items);
			}

			return true;
		}

		public static bool Unpack<T>(this List<Dictionary<uint, List<T>>> value, BinaryReader binaryReader, int fixedQuantity) where T : IUnpackable, new()
		{
			for (int i = 0; i < fixedQuantity; i++)
			{
				var items = new Dictionary<uint, List<T>>();

				var totalObjects = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // Discard

				for (int j = 0; j < totalObjects; j++)
				{
					var key = binaryReader.ReadUInt32();

					var items2 = new List<T>();

					var count = binaryReader.ReadUInt32();
					for (int k = 0; k < count; k++)
					{
						var item = new T();
						item.Unpack(binaryReader);
						items2.Add(item);
					}

					items.Add(key, items2);
				}

				value.Add(items);
			}

			return true;
		}


		public static bool Unpack<T>(this Dictionary<uint, T> value, BinaryReader binaryReader) where T : IUnpackable, new ()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();

				var item = new T();
				item.Unpack(binaryReader);
				value.Add(key, item);
			}

			return true;
		}

		public static bool Unpack<T>(this Dictionary<uint, List<T>> value, BinaryReader binaryReader) where T : IUnpackable, new()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();

				var items = new List<T>();

				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new T();
					item.Unpack(binaryReader);
					items.Add(item);
				}

				value.Add(key, items);
			}

			return true;
		}

		public static bool Unpack<T>(this Dictionary<uint, Dictionary<uint, List<T>>> value, BinaryReader binaryReader) where T : IUnpackable, new()
		{
			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();

				var items = new Dictionary<uint, List<T>>();

				var totalObjects2 = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // Discard

				for (int j = 0; j < totalObjects2; j++)
				{
					var key2 = binaryReader.ReadUInt32();

					var items2 = new List<T>();

					var count = binaryReader.ReadUInt32();
					for (int k = 0; k < count; k++)
					{
						var item = new T();
						item.Unpack(binaryReader);
						items2.Add(item);
					}

					items.Add(key2, items2);
				}

				value.Add(key, items);
			}

			return true;
		}
	}
}
