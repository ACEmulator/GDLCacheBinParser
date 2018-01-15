using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class EmoteAction
	{
		public int Type;
		public float Delay;
		public float Extent;

		public int? Motion;

		public string Message;
		public string TestString;
		public int? Min;
		public int? Max;
		public long? Min64;
		public long? Max64;
		public double? MinDbl;
		public double? MaxDbl;
		public int? Stat;
		public int? Display;

		public int? Amount;
		public long? Amount64;
		public long? HeroXP64;

		public double? Percent;

		public int? SpellID;

		public int? WealthRating;
		public int? TreasureClass;
		public int? TreasureType;

		public int? PScript;

		public int? Sound;

		public Item Item;
		public Frame Frame;
		public Position Position;

		public bool Unpack(BinaryReader binaryReader)
		{
			Type = binaryReader.ReadInt32();
			Delay = binaryReader.ReadSingle();
			Extent = binaryReader.ReadSingle();

			switch (Type)
			{
				case 1:
				case 8:
				case 10:
				//case 12:
				case 13:
				case 16:
				case 17:
				case 18:
				case 20:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
				case 26:
				case 31:
				case 51:
				case 58:
				case 60:
				case 61:
				case 64:
				case 65:
				case 67:
				case 68:
				case 79:
				case 80:
				case 81:
				case 83:
				case 88:
				case 121:
					Message = Util.ReadString(binaryReader, true);
					break;

				case 32:
				case 33:
				case 70:
				case 84:
				case 85:
				case 86:
				case 89:
				case 102:
				case 103:
				case 104:
				case 105:
				case 106:
				case 107:
				case 108:
				case 109:
					Message = Util.ReadString(binaryReader, true);
					Amount = binaryReader.ReadInt32();
					break;

				case 53:
				case 54:
				case 55:
				case 69:
					Stat = binaryReader.ReadInt32();
					Amount = binaryReader.ReadInt32();
					break;

				case 115:
					Stat = binaryReader.ReadInt32();
					Amount64 = binaryReader.ReadInt64();
					break;

				case 118:
					Stat = binaryReader.ReadInt32();
					Percent = binaryReader.ReadDouble();
					break;

				case 30:
				case 59:
				case 71:
				case 82:
					Message = Util.ReadString(binaryReader, true);
					Min = binaryReader.ReadInt32();
					Max = binaryReader.ReadInt32();
					break;

				case 2:
				case 62:
					Amount64 = binaryReader.ReadInt64();
					HeroXP64 = binaryReader.ReadInt64();
					break;

				case 112:
				case 113:
					Amount64 = binaryReader.ReadInt64();
					HeroXP64 = binaryReader.ReadInt64();
					break;

				case 34:
				case 47:
				case 48:
				case 90:
				case 119:
				case 120:
					Amount = binaryReader.ReadInt32();
					break;

				case 14:
				case 19:
				case 27:
				case 73:
					SpellID = binaryReader.ReadInt32();
					break;

				case 3:
				case 74:
					Item = new Item();
					Item.Unpack(binaryReader);
					break;

				case 76:
					Message = Util.ReadString(binaryReader, true);
					Item = new Item();
					Item.Unpack(binaryReader);
					break;

				case 56:
					WealthRating = binaryReader.ReadInt32();
					TreasureClass = binaryReader.ReadInt32();
					TreasureType = binaryReader.ReadInt32();
					break;

				case 5:
				case 52:
					Motion = binaryReader.ReadInt32();
					break;

				case 4:
				case 6:
				case 11:
				case 87:
					Frame = new Frame();
					Frame.Unpack(binaryReader);
					break;

				case 7:
					PScript = binaryReader.ReadInt32();
					break;

				case 9:
					Sound = binaryReader.ReadInt32();
					break;

				case 28:
				case 29:
					Amount = binaryReader.ReadInt32();
					Stat = binaryReader.ReadInt32();
					break;

				case 110:
					Stat = binaryReader.ReadInt32();
					break;

				case 111:
					Amount = binaryReader.ReadInt32();
					break;

				case 35:
				case 45:
				case 46:
					Message = Util.ReadString(binaryReader, true);
					Stat = binaryReader.ReadInt32();
					break;

				case 38:
				case 75:
					Message = Util.ReadString(binaryReader, true);
					TestString = Util.ReadString(binaryReader, true);
					Stat = binaryReader.ReadInt32();
					break;

				case 36:
				case 39:
				case 40:
				case 41:
				case 42:
				case 43:
				case 44:
					Message = Util.ReadString(binaryReader, true);
					Min = binaryReader.ReadInt32();
					Max = binaryReader.ReadInt32();
					Stat = binaryReader.ReadInt32();
					break;

				case 114:
					Message = Util.ReadString(binaryReader, true);
					Min64 = binaryReader.ReadInt64();
					Max64 = binaryReader.ReadInt64();
					Stat = binaryReader.ReadInt32();
					break;

				case 37:
					Message = Util.ReadString(binaryReader, true);
					MinDbl = binaryReader.ReadDouble();
					MaxDbl = binaryReader.ReadDouble();
					Stat = binaryReader.ReadInt32();
					break;

				case 49:
					Percent = binaryReader.ReadDouble();
					Min64 = binaryReader.ReadInt64();
					Max64 = binaryReader.ReadInt64();
					Display = binaryReader.ReadInt32();
					break;

				case 50:
					Stat = binaryReader.ReadInt32();
					Percent = binaryReader.ReadDouble();
					Min = binaryReader.ReadInt32();
					Max = binaryReader.ReadInt32();
					Display = binaryReader.ReadInt32();
					break;

				case 63:
					Position = new Position();
					Position.Unpack(binaryReader);
					break;

				case 99:
				case 100:
					Position = new Position();
					Position.Unpack(binaryReader);
					break;
			}

			return true;
		}
	}
}
