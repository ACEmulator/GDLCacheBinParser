using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class Recipe_Requirement
	{
	    public List<Requirement<int>> IntRequirements;
	    public List<Requirement<int>> DIDRequirements;
	    public List<Requirement<int>> IIDRequirements;
	    public List<Requirement<double>> FloatRequirements;
	    public List<Requirement<string>> StringRequirements;
	    public List<Requirement<bool>> BoolRequirements;

        public void Parse(BinaryReader binaryReader)
		{
            int count;

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) IntRequirements = new List<Requirement<int>>();
		    for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<int>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = binaryReader.ReadInt32();
		        item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
		        IntRequirements.Add(item);
		    }

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) DIDRequirements = new List<Requirement<int>>();
            for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<int>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = binaryReader.ReadInt32();
		        item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
                DIDRequirements.Add(item);
		    }

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) IIDRequirements = new List<Requirement<int>>();
            for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<int>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = binaryReader.ReadInt32();
		        item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
                IIDRequirements.Add(item);
		    }

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) FloatRequirements = new List<Requirement<double>>();
            for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<double>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = binaryReader.ReadSingle();
		        item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
                FloatRequirements.Add(item);
		    }

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) StringRequirements = new List<Requirement<string>>();
            for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<string>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = Util.ReadString(binaryReader, true);
                item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
                StringRequirements.Add(item);
		    }

		    count = binaryReader.ReadUInt16();
		    binaryReader.ReadUInt16();
		    if (count > 0) BoolRequirements = new List<Requirement<bool>>();
            for (int i = 0; i < count; i++)
		    {
		        var item = new Requirement<bool>();
		        item.Stat = binaryReader.ReadInt32();
		        item.Value = (binaryReader.ReadInt32() == 1);
		        item.Enum = binaryReader.ReadInt32();
		        item.Message = Util.ReadString(binaryReader, true);
                BoolRequirements.Add(item);
		    }
		}
	}
}
