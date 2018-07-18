using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg6_LandBlockExtendedData
{
	class TerrainData
	{
        //public uint Id { get; set; }

        //public bool HasObjects { get; set; }

        public List<ushort> Terrain { get; } = new List<ushort>();

        //public List<byte> Height { get; } = new List<byte>();

        public bool Unpack(BinaryReader binaryReader)
		{

            //Id = binaryReader.ReadUInt32();

            //uint hasObjects = binaryReader.ReadUInt32();
            //if (hasObjects == 1)
            //    HasObjects = true;

            // Read in the terrain. 9x9 so 81 records.
            for (int i = 0; i < 81; i++)
            {
                var terrain = binaryReader.ReadUInt16();
                Terrain.Add(terrain);
            }

            // Read in the height. 9x9 so 81 records
            //for (int i = 0; i < 81; i++)
            //{
            //    var height = binaryReader.ReadByte();
            //    Height.Add(height);
            //}

            //binaryReader.AlignBoundary();

            return true;
		}
	}
}
