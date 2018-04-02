
namespace PhatACCacheBinParser.Common
{
    enum DamageType
    {
        Slash       = 1,
        Pierce      = 2,
        Bludgeon    = 4,
        Cold        = 8,
        Fire        = 0x10,
        Acid        = 0x20,
        Electric    = 0x40,
        Health      = 0x80,
        Stamina     = 0x100,
        Mana        = 0x200,
        Nether      = 0x400,

        //BASE_DAMAGE_TYPE = 0x10000000
    }
}
