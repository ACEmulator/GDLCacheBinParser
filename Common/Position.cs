using System.IO;

namespace PhatACCacheBinParser.Common
{
	class Position
	{
		public uint ObjCellID;

		public readonly Origin Origin = new Origin();

		public readonly Angles Angles = new Angles();

		public bool Unpack(BinaryReader binaryReader)
		{
			// This value (according to the PhatAC jsons) is stored in Big Endian order, not Little Endian like the rest of the file.
			//var data = binaryReader.ReadBytes(4);
			//ObjCellID = BitConverter.ToInt32(data, 0);
			// To avoid being an exact copy, for now we just copy it as is.
			ObjCellID = binaryReader.ReadUInt32();

			Origin.Unpack(binaryReader);

			Angles.Unpack(binaryReader);

			return true;
		}
	}
}
