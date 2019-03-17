using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class ValueAndChance : IUnpackable
	{
        /// <summary>
        /// Slot, WCID, Spell ID
        /// </summary>
		public uint Value;
		public float Chance;

		public bool Unpack(BinaryReader reader)
		{
			Value = reader.ReadUInt32();
			Chance = (float)reader.ReadDouble(); // These are floats stored as doubles.

			return true;
		}
	}
}
