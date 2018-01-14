using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class Attributes
	{
		public readonly Attribute Strength = new Attribute();
		public readonly Attribute Endurance = new Attribute();
		public readonly Attribute Quickness = new Attribute();
		public readonly Attribute Coordination = new Attribute();
		public readonly Attribute Focus = new Attribute();
		public readonly Attribute Self = new Attribute();

		public readonly Attribute2 Health = new Attribute2();
		public readonly Attribute2 Stamina = new Attribute2();
		public readonly Attribute2 Mana = new Attribute2();

		public void Parse(BinaryReader binaryReader)
		{
			Strength.Parse(binaryReader);
			Endurance.Parse(binaryReader);
			Quickness.Parse(binaryReader);
			Coordination.Parse(binaryReader);
			Focus.Parse(binaryReader);
			Self.Parse(binaryReader);

			Health.Parse(binaryReader);
			Stamina.Parse(binaryReader);
			Mana.Parse(binaryReader);
		}
	}
}
