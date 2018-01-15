using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
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

		public bool Unpack(BinaryReader binaryReader)
		{
			Strength.Unpack(binaryReader);
			Endurance.Unpack(binaryReader);
			Quickness.Unpack(binaryReader);
			Coordination.Unpack(binaryReader);
			Focus.Unpack(binaryReader);
			Self.Unpack(binaryReader);

			Health.Unpack(binaryReader);
			Stamina.Unpack(binaryReader);
			Mana.Unpack(binaryReader);

			return true;
		}
	}
}
