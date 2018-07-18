using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhatACCacheBinParser.Enums
{
    public enum PortalLinkType
    {
        Undef_PortalLinkType,
        LinkedLifestone_PortalLinkType,
        LinkedPortalOne_PortalLinkType,
        LinkedPortalTwo_PortalLinkType
    }

    public enum PortalRecallType
    {
        Undef_PortalRecallType,
        LastLifestone_PortalRecallType,
        LinkedLifestone_PortalRecallType,
        LastPortal_PortalRecallType,
        LinkedPortalOne_PortalRecallType,
        LinkedPortalTwo_PortalRecallType
    }

    public enum PortalSummonType
    {
        Undef_PortalSummonType,
        LinkedPortalOne_PortalSummonType,
        LinkedPortalTwo_PortalSummonType
    }

    [Flags]
    public enum PortalEnum
    {
        Undef_PortalEnum = 0,
        Not_Passable_PortalEnum = 0,
        Player_Passable_PortalEnum = 1,
        PK_Banned_PortalEnum = 2,
        PKLite_Banned_PortalEnum = 4,
        Player_NPK_Only_PortalEnum = 7,
        NPK_Banned_PortalEnum = 8,
        Player_PK_PKL_Only_PortalEnum = 9,
        Not_Summonable_PortalEnum = 16,
        Player_NotSummonable_PortalEnum = 17,
        Player_PK_PKL_Only_NotSummonable_PortalEnum = 25,
        Not_Recallable_Nor_Linkable_PortalEnum = 32,
        Player_NotRecallable_NotLinkable_PortalEnum = 33,
        Player_PK_PKL_Only_NotRecallable_NotLinkable_PortalEnum = 41,
        Player_NotRecallable_NotLinkable_NotSummonable_PortalEnum = 49,
        Player_PK_PKL_Only_NotSummonable_NotRecallable_NotLinkable_PortalEnum = 57,
        Only_Olthoi_PCs_PortalEnum = 64,
        No_Olthoi_PCs_PortalEnum = 128,
        No_Vitae_PortalEnum = 256,
        No_New_Accounts_PortalEnum = 512
    }
}
