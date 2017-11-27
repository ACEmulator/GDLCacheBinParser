using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class ArmorValues
	{
		public int BaseArmor;
		public int ArmorVsSlash;
		public int ArmorVsPierce;
		public int ArmorVsBludgeon;
		public int ArmorVsCold;
		public int ArmorVsFire;
		public int ArmorVsAcid;
		public int ArmorVsElectric;
		public int ArmorVsNether;

		public void Parse(BinaryReader binaryReader)
		{
			BaseArmor = binaryReader.ReadInt32();
			ArmorVsSlash = binaryReader.ReadInt32();
			ArmorVsPierce = binaryReader.ReadInt32();
			ArmorVsBludgeon = binaryReader.ReadInt32();
			ArmorVsCold = binaryReader.ReadInt32();
			ArmorVsFire = binaryReader.ReadInt32();
			ArmorVsAcid = binaryReader.ReadInt32();
			ArmorVsElectric = binaryReader.ReadInt32();
			ArmorVsNether = binaryReader.ReadInt32();
		}
	}
}
