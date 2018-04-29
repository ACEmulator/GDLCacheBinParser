using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class Recipe_Mod
	{
	    public List<Mod<int>> IntMods;
	    public List<Mod<uint>> DIDMods;
	    public List<Mod<uint>> IIDMods;
	    public List<Mod<double>> FloatMods;
	    public List<Mod<string>> StringMods;
	    public List<Mod<bool>> BoolMods;

        public int Unknown1;
	    public int Unknown2;
	    public int Unknown3;
	    public int Unknown4;
	    public int Unknown5;
	    public int Unknown6;

	    public bool Unknown7;
	    public int DataID;

	    public int Unknown9;
	    public int InstanceID;

        public bool Unpack(BinaryReader binaryReader)
        {
            int count;

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) IntMods = new List<Mod<int>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<int>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = binaryReader.ReadInt32();
                IntMods.Add(item);
            }

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) DIDMods = new List<Mod<uint>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<uint>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = binaryReader.ReadUInt32();
                DIDMods.Add(item);
            }

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) IIDMods = new List<Mod<uint>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<uint>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = binaryReader.ReadUInt32();
                IIDMods.Add(item);
            }

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) FloatMods = new List<Mod<double>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<double>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = binaryReader.ReadDouble();
                FloatMods.Add(item);
            }

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) StringMods = new List<Mod<string>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<string>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = Util.ReadString(binaryReader, true);
                StringMods.Add(item);
            }

            count = binaryReader.ReadUInt16();
            binaryReader.ReadUInt16();
            if (count > 0) BoolMods = new List<Mod<bool>>();
            for (int i = 0; i < count; i++)
            {
                var item = new Mod<bool>();
                item.Unknown1 = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = (binaryReader.ReadInt32() == 1);
                BoolMods.Add(item);
            }

            Unknown1 = binaryReader.ReadInt32();
            Unknown2 = binaryReader.ReadInt32();
            Unknown3 = binaryReader.ReadInt32();
            Unknown4 = binaryReader.ReadInt32();
            Unknown5 = binaryReader.ReadInt32();
            Unknown6 = binaryReader.ReadInt32();

            Unknown7 = (binaryReader.ReadInt32() == 1);
            DataID = binaryReader.ReadInt32();

            Unknown9 = binaryReader.ReadInt32();
            InstanceID = binaryReader.ReadInt32();

	        return true;
		}
    }
}
