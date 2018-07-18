using System;

namespace PhatACCacheBinParser.Enums
{
    [Flags]
    public enum HookTypeEnum
    {
        Undef_HookTypeEnum = 0,
        Floor_HookTypeEnum = (1 << 0),
        Wall_HookTypeEnum = (1 << 1),
        Ceiling_HookTypeEnum = (1 << 2),
        Yard_HookTypeEnum = (1 << 3),
        Roof_HookTypeEnum = (1 << 4)
    }
}
