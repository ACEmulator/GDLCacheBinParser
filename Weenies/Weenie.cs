using System;
using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Weenies
{
	class Weenie : IParseableObject
	{
		// _cache_bin_parse_9_1
		public string Label;

		public uint Timestamp;

		public int WeenieType;

		public Dictionary<int, int> IntValues;

		public Dictionary<int, long> LongValues;

		public Dictionary<int, bool> BoolValues;

		public Dictionary<int, double> DoubleValues;

		public Dictionary<int, string> StringValues;

		public Dictionary<int, uint> DIDValues;

		public Dictionary<int, Position> PositionValues;

		public Dictionary<int, int> IIDValues;

		// _cache_bin_parse_9_8
		public uint WCID;

		// _cache_bin_parse_9_9
		public Attributes Attributes;

		public Dictionary<int, Skill> Skills;

		public Dictionary<int, BodyPart> BodyParts;

		public Dictionary<int, float> SpellCastingProbability;

		public List<int> EventFilters;

		public Dictionary<int, List<Emote>> Emotes;

		public List<Item> CreateList;

		public PagesData PagesData;

		public List<Generator> Generators;


		public Extra1 Extra1;

		public List<Extra2Item> Extra2Items;

		public List<Extra3Item> Extra3Items;


		// Unix timestamp is seconds past epoch
		//static DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public void Parse(BinaryReader binaryReader)
		{
			int count;


			// Skip the next 4 bytes. It is the wcid, but the code doesn't pull it from here.
			binaryReader.ReadInt32();


			// _cache_bin_parse_9_1

			// I believe this is a start of record identifier
			var unknown_header_1 = binaryReader.ReadInt32(); // 02000000, is it always this value?
			if (unknown_header_1 != 0x02)
				throw new Exception();

			Label = Util.ReadString(binaryReader, true);

			Timestamp = binaryReader.ReadUInt32();
			//TimestampAsString = UnixTimeStart.AddSeconds(Timestamp).ToString();

			var basicFlags = binaryReader.ReadInt32();
			WeenieType = binaryReader.ReadInt32();

			if ((basicFlags & 0x01) == 0x01)
			{
				IntValues = new Dictionary<int, int>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					IntValues.Add(binaryReader.ReadInt32(), binaryReader.ReadInt32());
			}

			if ((basicFlags & 0x80) == 0x80)
			{
				LongValues = new Dictionary<int, long>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					LongValues.Add(binaryReader.ReadInt32(), binaryReader.ReadInt64());
			}

			if ((basicFlags & 0x02) == 0x02)
			{
				BoolValues = new Dictionary<int, bool>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					BoolValues.Add(binaryReader.ReadInt32(), Convert.ToBoolean(binaryReader.ReadInt32()));
			}

			if ((basicFlags & 0x04) == 0x04)
			{
				DoubleValues = new Dictionary<int, double>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					DoubleValues.Add(binaryReader.ReadInt32(), binaryReader.ReadDouble());
			}

			if ((basicFlags & 0x08) == 0x08)
			{
				StringValues = new Dictionary<int, string>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					StringValues.Add(binaryReader.ReadInt32(), Util.ReadString(binaryReader, true));
			}

			if ((basicFlags & 0x10) == 0x10)
			{
				DIDValues = new Dictionary<int, uint>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					DIDValues.Add(binaryReader.ReadInt32(), binaryReader.ReadUInt32());
			}

			if ((basicFlags & 0x20) == 0x20)
			{
				PositionValues = new Dictionary<int, Position>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
				{
					var key = binaryReader.ReadInt32();

					var value = new Position();

					value.Parse(binaryReader);

					PositionValues.Add(key, value);
				}
			}

			if ((basicFlags & 0x40) == 0x40)
			{
				IIDValues = new Dictionary<int, int>();

				count = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // discard

				for (int i = 0; i < count; i++)
					IIDValues.Add(binaryReader.ReadInt32(), binaryReader.ReadInt32());
			}

			// _cache_bin_parse_9_8
			var extendedFlags = binaryReader.ReadInt32(); // 0000003D, 0000003F, 0000001F, 0000000F, ....., 0000012F
			WCID = binaryReader.ReadUInt32();

			// _cache_bin_parse_9_9
			if (extendedFlags != 0)
			{ 
				if ((extendedFlags & 0x01) == 0x01)
				{
					var attributeMask = binaryReader.ReadInt32(); // 000001FF
					if (attributeMask != 0x1FF)
						throw new NotImplementedException();

					Attributes = new Attributes();
					Attributes.Parse(binaryReader);
				}


				if ((extendedFlags & 0x02) == 0x02)
				{
					Skills = new Dictionary<int, Skill>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard
	
					for (int i = 0; i < count; i++)
					{
						var key = binaryReader.ReadInt32();

						var value = new Skill();
	
						value.Parse(binaryReader);
	
						Skills.Add(key, value);
					}
				}


				if ((extendedFlags & 0x04) == 0x04)
				{
					BodyParts = new Dictionary<int, BodyPart>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
					{
						var key = binaryReader.ReadInt32();

						// ?? Probably number of body.body_part_table.value objects, which should always be 1
						var unknown = binaryReader.ReadInt32();

						if (unknown != 1)
							throw new NotImplementedException();

						var value = new BodyPart();
						value.Parse(binaryReader);

						BodyParts.Add(key, value);
					}
				}


				if ((extendedFlags & 0x100) == 0x100)
				{
					SpellCastingProbability = new Dictionary<int, float>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
						SpellCastingProbability.Add(binaryReader.ReadInt32(), binaryReader.ReadSingle());
				}


				if ((extendedFlags & 0x08) == 0x08)
				{
					EventFilters = new List<int>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
						EventFilters.Add(binaryReader.ReadInt32());
				}


				if ((extendedFlags & 0x10) == 0x10)
				{
					Emotes = new Dictionary<int, List<Emote>>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
					{
						var key = binaryReader.ReadInt32();

						var value = new List<Emote>();

						var count2 = binaryReader.ReadUInt16();
						binaryReader.ReadUInt16(); // discard

						for (int j = 0; j < count2; j++)
						{
							var emote = new Emote();

							emote.Parse(binaryReader);

							value.Add(emote);
						}

						Emotes.Add(key, value);
					}
				}


				if ((extendedFlags & 0x20) == 0x20)
				{
					CreateList = new List<Item>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
					{
						var value = new Item();

						value.Parse(binaryReader);

						CreateList.Add(value);
					}
				}


				// I don't know if the order is correct for this data segment. I just assumed based on its mask.
				if ((extendedFlags & 0x40) == 0x40)
				{
					PagesData = new PagesData();
					PagesData.Parse(binaryReader);
				}

				// I don't know if the order is correct for this data segment. I just assumed based on its mask.
				if ((extendedFlags & 0x80) == 0x80)
				{
					Generators = new List<Generator>();

					count = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // discard

					for (int i = 0; i < count; i++)
					{
						var generator = new Generator();
						generator.Parse(binaryReader);
						Generators.Add(generator);
					}
				}
			}

			// _cache_bin_parse_9_19

			// I believe this is a record segment end/start identifier
			var unknown_footer_1 = binaryReader.ReadByte();    // 0x11
			if (unknown_footer_1 != 0x11)
				throw new Exception();

			var count_1 = binaryReader.ReadByte();
			var count_2 = binaryReader.ReadByte();
			var count_3 = binaryReader.ReadByte();

			if (count_1 != 0 || count_2 != 0 || count_3 != 0)
			{
				if (count_1 > 0)
				{
					Extra1 = new Extra1();

					Extra1.Parse(binaryReader, count_1);
				}

				if (count_2 > 0)
				{
					Extra2Items = new List<Extra2Item>();

					for (int i = 0; i < count_2; i++)
					{
						var item = new Extra2Item();
						item.Parse(binaryReader);
						Extra2Items.Add(item);
					}
				}

				if (count_3 > 0)
				{
					Extra3Items = new List<Extra3Item>();

					for (int i = 0; i < count_3; i++)
					{
						var item = new Extra3Item();
						item.Parse(binaryReader);
						Extra3Items.Add(item);
					}
				}

				// This is a bit of a hack. I don't know how the code determines how many bytes to increment the main address index by.
				// All segments end with 0x01, and the next segment starts with the WCID (little endian), and then 02 00 00 00 as a start of record identiifer.
				// We look for that byte sequence, and then subtract our position accordingly
				var startPosition = binaryReader.BaseStream.Position;

				for (long i = startPosition; i <= binaryReader.BaseStream.Length; i++)
				{
					binaryReader.BaseStream.Position = i;

					var bytes = binaryReader.ReadBytes(6);

					if (bytes[0] == 0x01 && bytes[3] == 0x00 && bytes[4] == 0x00 && bytes[5] == 0x02)
					{
						binaryReader.BaseStream.Position -= 6;
						break;
					}
				}
			}

			// _cache_bin_parse_9_1

			// I believe this is an end of record identifier
			var unknown_footer_9 = binaryReader.ReadByte();   // 0x01
			if (unknown_footer_9 != 0x01)
				throw new Exception();
		}
	}
}
