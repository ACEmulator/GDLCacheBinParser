using System;

namespace PhatACCacheBinParser.Enums
{
    [Flags]
    public enum AttackType
    {
        Undef_AttackType = 0,
        Punch_AttackType = (1 << 0),
        Thrust_AttackType = (1 << 1),
        Slash_AttackType = (1 << 2),
        Kick_AttackType = (1 << 3),
        OffhandPunch_AttackType = (1 << 4),
        DoubleSlash_AttackType = (1 << 5),
        TripleSlash_AttackType = (1 << 6),
        DoubleThrust_AttackType = (1 << 7),
        TripleThrust_AttackType = (1 << 8),
        OffhandThrust_AttackType = (1 << 9),
        OffhandSlash_AttackType = (1 << 10),
        OffhandDoubleSlash_AttackType = (1 << 11),
        OffhandTripleSlash_AttackType = (1 << 12),
        OffhandDoubleThrust_AttackType = (1 << 13),
        OffhandTripleThrust_AttackType = (1 << 14),
        Unarmed_AttackType = Punch_AttackType | Kick_AttackType | OffhandPunch_AttackType, // 25
        MultiStrike_AttackType = DoubleSlash_AttackType | TripleSlash_AttackType | DoubleThrust_AttackType | TripleThrust_AttackType | OffhandDoubleSlash_AttackType | OffhandTripleSlash_AttackType | OffhandDoubleThrust_AttackType | OffhandTripleThrust_AttackType // 31200
    }


    public enum BodyPart
    {
        UNDEFINED = -1,
        HEAD = 0,
        CHEST = 1,
        ABDOMEN = 2,
        UPPER_ARM = 3,
        LOWER_ARM = 4,
        HAND = 5,
        UPPER_LEG = 6,
        LOWER_LEG = 7,
        FOOT = 8,
        HORN = 9,
        FRONT_LEG = 10,
        // Skip 11
        FRONT_FOOT = 12,
        REAR_LEG = 13,
        // Skip 14
        REAR_FOOT = 15,
        TORSO = 16,
        TAIL = 17,
        ARM = 18,
        LEG = 19,
        CLAW = 20,
        WINGS = 21,
        BREATH = 22,
        TENTACLE = 23,
        UPPER_TENTACLE = 24,
        LOWER_TENTACLE = 25,
        CLOAK = 26,
        NUM = 27
    }
}
