using System;

namespace PhatACCacheBinParser.Enums
{
    public enum COMBAT_USE
    {
        COMBAT_USE_NONE,
        COMBAT_USE_MELEE,
        COMBAT_USE_MISSILE,
        COMBAT_USE_AMMO,
        COMBAT_USE_SHIELD,
        COMBAT_USE_TWO_HANDED
    }

    [Flags]
    public enum ITEM_USEABLE
    {
        USEABLE_UNDEF = 0,
        USEABLE_NO = (1 << 0),
        USEABLE_SELF = (1 << 1),
        USEABLE_WIELDED = (1 << 2),
        USEABLE_CONTAINED = (1 << 3),
        USEABLE_VIEWED = (1 << 4),
        USEABLE_REMOTE = (1 << 5),
        USEABLE_NEVER_WALK = (1 << 6),
        USEABLE_OBJSELF = (1 << 7),
        USEABLE_CONTAINED_VIEWED = 24,
        USEABLE_CONTAINED_VIEWED_REMOTE = 56,
        USEABLE_CONTAINED_VIEWED_REMOTE_NEVER_WALK = 120,
        USEABLE_VIEWED_REMOTE = 48,
        USEABLE_VIEWED_REMOTE_NEVER_WALK = 112,
        USEABLE_REMOTE_NEVER_WALK = 96,
        USEABLE_SOURCE_WIELDED_TARGET_WIELDED = 262148,
        USEABLE_SOURCE_WIELDED_TARGET_CONTAINED = 524292,
        USEABLE_SOURCE_WIELDED_TARGET_VIEWED = 1048580,
        USEABLE_SOURCE_WIELDED_TARGET_REMOTE = 2097156,
        USEABLE_SOURCE_WIELDED_TARGET_REMOTE_NEVER_WALK = 6291460,
        USEABLE_SOURCE_CONTAINED_TARGET_WIELDED = 262152,
        USEABLE_SOURCE_CONTAINED_TARGET_CONTAINED = 524296,
        USEABLE_SOURCE_CONTAINED_TARGET_OBJSELF_OR_CONTAINED = 8912904,
        USEABLE_SOURCE_CONTAINED_TARGET_SELF_OR_CONTAINED = 655368,
        USEABLE_SOURCE_CONTAINED_TARGET_VIEWED = 1048584,
        USEABLE_SOURCE_CONTAINED_TARGET_REMOTE = 2097160,
        USEABLE_SOURCE_CONTAINED_TARGET_REMOTE_NEVER_WALK = 6291464,
        USEABLE_SOURCE_CONTAINED_TARGET_REMOTE_OR_SELF = 2228232,
        USEABLE_SOURCE_VIEWED_TARGET_WIELDED = 262160,
        USEABLE_SOURCE_VIEWED_TARGET_CONTAINED = 524304,
        USEABLE_SOURCE_VIEWED_TARGET_VIEWED = 1048592,
        USEABLE_SOURCE_VIEWED_TARGET_REMOTE = 2097168,
        USEABLE_SOURCE_REMOTE_TARGET_WIELDED = 262176,
        USEABLE_SOURCE_REMOTE_TARGET_CONTAINED = 524320,
        USEABLE_SOURCE_REMOTE_TARGET_VIEWED = 1048608,
        USEABLE_SOURCE_REMOTE_TARGET_REMOTE = 2097184,
        USEABLE_SOURCE_REMOTE_TARGET_REMOTE_NEVER_WALK = 6291488,
        USEABLE_SOURCE_MASK = 65535,
        USEABLE_TARGET_MASK = -65536,
    }

    [Flags]
    public enum ImbuedEffectType
    {
        Undef_ImbuedEffectType = 0,
        CriticalStrike_ImbuedEffectType = (1 << 0),
        CripplingBlow_ImbuedEffectType = (1 << 1),
        ArmorRending_ImbuedEffectType = (1 << 2),
        SlashRending_ImbuedEffectType = (1 << 3),
        PierceRending_ImbuedEffectType = (1 << 4),
        BludgeonRending_ImbuedEffectType = (1 << 5),
        AcidRending_ImbuedEffectType = (1 << 6),
        ColdRending_ImbuedEffectType = (1 << 7),
        ElectricRending_ImbuedEffectType = (1 << 8),
        FireRending_ImbuedEffectType = (1 << 9),
        MeleeDefense_ImbuedEffectType = (1 << 10),
        MissileDefense_ImbuedEffectType = (1 << 11),
        MagicDefense_ImbuedEffectType = (1 << 12),
        Spellbook_ImbuedEffectType = (1 << 13),
        NetherRending_ImbuedEffectType = (1 << 14),
        IgnoreSomeMagicProjectileDamage_ImbuedEffectType = (1 << 29),
        AlwaysCritical_ImbuedEffectType = (1 << 30),
        IgnoreAllArmor_ImbuedEffectType = (1 << 31)
    }

    public enum WeaponType
    {
        Undef_WeaponType,
        Unarmed_WeaponType,
        Sword_WeaponType,
        Axe_WeaponType,
        Mace_WeaponType,
        Spear_WeaponType,
        Dagger_WeaponType,
        Staff_WeaponType,
        Bow_WeaponType,
        Crossbow_WeaponType,
        Thrown_WeaponType,
        TwoHanded_WeaponType,
        Magic_WeaponType
    }
    
    public enum AttunedStatusEnum
    {
        Normal_AttunedStatus,
        Attuned_AttunedStatus,
        Sticky_AttunedStatus
    }

    public enum BondedStatusEnum
    {
        Destroy_BondedStatus = -2,
        Slippery_BondedStatus = -1,
        Normal_BondedStatus = 0,
        Bonded_BondedStatus = 1,
        Sticky_BondedStatus = 2
    }

    public enum PALETTE_TEMPLATE
    {
        UNDEF_PALETTE_TEMPLATE,
        AQUABLUE_PALETTE_TEMPLATE,
        BLUE_PALETTE_TEMPLATE,
        BLUEPURPLE_PALETTE_TEMPLATE,
        BROWN_PALETTE_TEMPLATE,
        DARKBLUE_PALETTE_TEMPLATE,
        DEEPBROWN_PALETTE_TEMPLATE,
        DEEPGREEN_PALETTE_TEMPLATE,
        GREEN_PALETTE_TEMPLATE,
        GREY_PALETTE_TEMPLATE,
        LIGHTBLUE_PALETTE_TEMPLATE,
        MAROON_PALETTE_TEMPLATE,
        NAVY_PALETTE_TEMPLATE,
        PURPLE_PALETTE_TEMPLATE,
        RED_PALETTE_TEMPLATE,
        REDPURPLE_PALETTE_TEMPLATE,
        ROSE_PALETTE_TEMPLATE,
        YELLOW_PALETTE_TEMPLATE,
        YELLOWBROWN_PALETTE_TEMPLATE,
        COPPER_PALETTE_TEMPLATE,
        SILVER_PALETTE_TEMPLATE,
        GOLD_PALETTE_TEMPLATE,
        AQUA_PALETTE_TEMPLATE,
        DARKAQUAMETAL_PALETTE_TEMPLATE,
        DARKBLUEMETAL_PALETTE_TEMPLATE,
        DARKCOPPERMETAL_PALETTE_TEMPLATE,
        DARKGOLDMETAL_PALETTE_TEMPLATE,
        DARKGREENMETAL_PALETTE_TEMPLATE,
        DARKPURPLEMETAL_PALETTE_TEMPLATE,
        DARKREDMETAL_PALETTE_TEMPLATE,
        DARKSILVERMETAL_PALETTE_TEMPLATE,
        LIGHTAQUAMETAL_PALETTE_TEMPLATE,
        LIGHTBLUEMETAL_PALETTE_TEMPLATE,
        LIGHTCOPPERMETAL_PALETTE_TEMPLATE,
        LIGHTGOLDMETAL_PALETTE_TEMPLATE,
        LIGHTGREENMETAL_PALETTE_TEMPLATE,
        LIGHTPURPLEMETAL_PALETTE_TEMPLATE,
        LIGHTREDMETAL_PALETTE_TEMPLATE,
        LIGHTSILVERMETAL_PALETTE_TEMPLATE,
        BLACK_PALETTE_TEMPLATE,
        BRONZE_PALETTE_TEMPLATE,
        SANDYYELLOW_PALETTE_TEMPLATE,
        DARKBROWN_PALETTE_TEMPLATE,
        LIGHTBROWN_PALETTE_TEMPLATE,
        TANRED_PALETTE_TEMPLATE,
        PALEGREEN_PALETTE_TEMPLATE,
        TAN_PALETTE_TEMPLATE,
        PASTYYELLOW_PALETTE_TEMPLATE,
        SNOWYWHITE_PALETTE_TEMPLATE,
        RUDDYYELLOW_PALETTE_TEMPLATE,
        RUDDIERYELLOW_PALETTE_TEMPLATE,
        MIDGREY_PALETTE_TEMPLATE,
        DARKGREY_PALETTE_TEMPLATE,
        BLUEDULLSILVER_PALETTE_TEMPLATE,
        YELLOWPALESILVER_PALETTE_TEMPLATE,
        BROWNBLUEDARK_PALETTE_TEMPLATE,
        BROWNBLUEMED_PALETTE_TEMPLATE,
        GREENSILVER_PALETTE_TEMPLATE,
        BROWNGREEN_PALETTE_TEMPLATE,
        YELLOWGREEN_PALETTE_TEMPLATE,
        PALEPURPLE_PALETTE_TEMPLATE,
        WHITE_PALETTE_TEMPLATE,
        REDBROWN_PALETTE_TEMPLATE,
        GREENBROWN_PALETTE_TEMPLATE,
        ORANGEBROWN_PALETTE_TEMPLATE,
        PALEGREENBROWN_PALETTE_TEMPLATE,
        PALEORANGE_PALETTE_TEMPLATE,
        GREENSLIME_PALETTE_TEMPLATE,
        BLUESLIME_PALETTE_TEMPLATE,
        YELLOWSLIME_PALETTE_TEMPLATE,
        PURPLESLIME_PALETTE_TEMPLATE,
        DULLRED_PALETTE_TEMPLATE,
        GREYWHITE_PALETTE_TEMPLATE,
        MEDIUMGREY_PALETTE_TEMPLATE,
        DULLGREEN_PALETTE_TEMPLATE,
        OLIVEGREEN_PALETTE_TEMPLATE,
        ORANGE_PALETTE_TEMPLATE,
        BLUEGREEN_PALETTE_TEMPLATE,
        OLIVE_PALETTE_TEMPLATE,
        LEAD_PALETTE_TEMPLATE,
        IRON_PALETTE_TEMPLATE,
        LITEGREEN_PALETTE_TEMPLATE,
        PINKPURPLE_PALETTE_TEMPLATE,
        AMBER_PALETTE_TEMPLATE,
        DYEDARKGREEN_PALETTE_TEMPLATE,
        DYEDARKRED_PALETTE_TEMPLATE,
        DYEDARKYELLOW_PALETTE_TEMPLATE,
        DYEBOTCHED_PALETTE_TEMPLATE,
        DYEWINTERBLUE_PALETTE_TEMPLATE,
        DYEWINTERGREEN_PALETTE_TEMPLATE,
        DYEWINTERSILVER_PALETTE_TEMPLATE,
        DYESPRINGBLUE_PALETTE_TEMPLATE,
        DYESPRINGPURPLE_PALETTE_TEMPLATE,
        DYESPRINGBLACK_PALETTE_TEMPLATE
    }

    [Flags]
    public enum WieldRequirement
    {
        Invalid_WieldRequirement,
        WIELD_REQUIRES_SKILL_WieldRequirement,
        WIELD_REQUIRES_RAW_SKILL_WieldRequirement,
        WIELD_REQUIRES_ATTRIB_WieldRequirement,
        WIELD_REQUIRES_RAW_ATTRIB_WieldRequirement,
        WIELD_REQUIRES_SECONDARY_ATTRIB_WieldRequirement,
        WIELD_REQUIRES_RAW_SECONDARY_ATTRIB_WieldRequirement,
        WIELD_REQUIRES_LEVEL_WieldRequirement,
        WIELD_REQUIRES_TRAINING_WieldRequirement,
        WIELD_REQUIRES_INTSTAT_WieldRequirement,
        WIELD_REQUIRES_BOOLSTAT_WieldRequirement,
        WIELD_REQUIRES_CREATURE_TYPE_WieldRequirement,
        WIELD_REQUIRES_HERITAGE_TYPE_WieldRequirement
    }

    public enum ItemXpStyle
    {
        Undef,
        Fixed,
        ScalesWithLevel,
        // This appears to be another "scales with level type" that appears in the client but was never used
        Unknown
    }

    enum ActivationResponseEnum
    {
        Undef_ActivationResponse = 0,
        Use_ActivationResponse = 2,
        Animate_ActivationResponse = 4,
        Talk_ActivationResponse = 0x10,
        Unk800_ActivationResponse = 0x800,
        CastSpell_ActivationResponse = 0x1000,
        Generate_ActivationResponse = 0x10000
    };
}
