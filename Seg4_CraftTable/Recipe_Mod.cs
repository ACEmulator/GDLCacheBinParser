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

        public int Health;
	    public int Stamina; // Probably Stamina?
	    public int Mana;
	    public bool DoHealthMod;
	    public bool DoStaminaMod;
	    public bool DoManaMod;

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
                item.Source = binaryReader.ReadInt32();
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
                item.Source = binaryReader.ReadInt32();
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
                item.Source = binaryReader.ReadInt32();
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
                item.Source = binaryReader.ReadInt32();
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
                item.Source = binaryReader.ReadInt32();
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
                item.Source = binaryReader.ReadInt32();
                item.Enum = binaryReader.ReadInt32();
                item.Stat = binaryReader.ReadInt32();
                item.Value = (binaryReader.ReadInt32() == 1);
                BoolMods.Add(item);
            }

            Health = binaryReader.ReadInt32();
            Stamina = binaryReader.ReadInt32();
            Mana = binaryReader.ReadInt32();
            DoHealthMod = (binaryReader.ReadInt32() == 1);
            DoStaminaMod = (binaryReader.ReadInt32() == 1);
            DoManaMod = (binaryReader.ReadInt32() == 1);

            Unknown7 = (binaryReader.ReadInt32() == 1);
            DataID = binaryReader.ReadInt32();

            Unknown9 = binaryReader.ReadInt32();
            InstanceID = binaryReader.ReadInt32();

	        return true;
		}
    }
}
