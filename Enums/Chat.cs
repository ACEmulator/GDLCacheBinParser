using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhatACCacheBinParser.Enums
{
    public enum eChatTypes
    {
        eTextTypeDefault,
        eTextTypeAllChannels,
        eTextTypeSpeech,
        eTextTypeSpeechDirect,
        eTextTypeSpeechDirectSend,
        eTextTypeSystemSvent,
        eTextTypeCombat,
        eTextTypeMagic,
        eTextTypeChannel,
        eTextTypeChannelCend,
        eTextTypeSocialChannel,
        eTextTypeSocialChannelSend,
        eTextTypeEmote,
        eTextTypeAdvancement,
        eTextTypeAbuseChannel,
        eTextTypeHelpChannel,
        eTextTypeAppraisalChannel,
        eTextTypeMagicCastingChannel,
        eTextTypeAllegienceChannel,
        eTextTypeFellowshipChannel,
        eTextTypeWorld_broadcast,
        eTextTypeCombatEnemy,
        eTextTypeCombatSelf,
        eTextTypeRecall,
        eTextTypeCraft,
        eTextTypeTotalNumChannels
    }

    public enum SquelchTypes
    {
        AllChannels = 1,
        Speech = 2,
        SpeechDirect = 3, // @tell
        Combat = 6,
        Magic = 7,
        Emote = 12,
        AppraisalChannel = 16,
        MagicCastingChannel = 17,
        AllegienceChannel = 18,
        FellowshipChannel = 19,
        CombatEnemy = 21,
        CombatSelf = 22,
        Recall = 23,
        Craft = 24,
        Salvaging = 25
    }

    public enum SquelchMasks
    {
        Speech = 0x00000004,
        SpeechDirect = 0x00000008, // @tell
        Combat = 0x00000040,
        Magic = 0x00000080,
        Emote = 0x00001000,
        AppraisalChannel = 0x00010000,
        MagicCastingChannel = 0x00020000,
        AllegienceChannel = 0x00040000,
        FellowshipChannel = 0x00080000,
        CombatEnemy = 0x00200000,
        CombatSelf = 0x00400000,
        Recall = 0x00800000,
        Craft = 0x01000000,
        Salvaging = 0x02000000,
        AllChannels = unchecked((int)0xFFFFFFFF)
    }

    public enum ChatTypeEnum
    {
        Undef_ChatTypeEnum,
        Allegiance_ChatTypeEnum,
        General_ChatTypeEnum,
        Trade_ChatTypeEnum,
        LFG_ChatTypeEnum,
        Roleplay_ChatTypeEnum,
        Society_ChatTypeEnum,
        SocietyCelHan_ChatTypeEnum,
        SocietyEldWeb_ChatTypeEnum,
        SocietyRadBlo_ChatTypeEnum,
        Olthoi_ChatTypeEnum
    }

    public enum GroupChatType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        /// Included for completeness since it is listed in the client
        Unknown = 0x00000000,

        /// <summary>
        /// @abuse - Abuse Channel
        /// </summary>
        TellAbuse = 0x00000001,

        /// <summary>
        /// @admin - Admin Channel (@ad)
        /// </summary>
        TellAdmin = 0x00000002,

        /// <summary>
        /// @audit - Audit Channel (@au)
        /// This channel was used to echo copies of enforcement commands (such as: ban, gag, boot) to all other online admins
        /// </summary>
        TellAudit = 0x00000004,

        /// <summary>
        /// @av1 - Advocate Channel (@advocate) (@advocate1)
        /// </summary>
        TellAdvocate = 0x00000008,

        /// <summary>
        /// @av2 - Advocate2 Channel (@advocate2)
        /// </summary>
        TellAdvocate2 = 0x00000010,

        /// <summary>
        /// @av3 - Advocate3 Channel (@advocate3)
        /// </summary>
        TellAdvocate3 = 0x000000020,

        /// <summary>
        /// @sent - Sentinel Channel (@sentinel)
        /// </summary>
        TellSentinel = 0x000000200,

        /// <summary>
        /// @[command name tbd] - Help Channel
        /// </summary>
        TellHelp = 0x000000400,

        /// <summary>
        /// @f - Tell Fellowship
        /// </summary>
        TellFellowship = 0x00000800,

        /// <summary>
        /// @v - Tell Vassals
        /// </summary>
        TellVassals = 0x00001000,

        /// <summary>
        /// @p - Tell Patron
        /// </summary>
        TellPatron = 0x00002000,

        /// <summary>
        /// @m - Tell Monarch
        /// </summary>
        TellMonarch = 0x00004000,

        /// <summary>
        /// @c - Tell Co-Vassals
        /// </summary>
        TellCoVassals = 0x01000000,

        /// <summary>
        /// @allegiance broadcast - Tell All Allegiance Members
        /// </summary>
        AllegianceBroadcast = 0x02000000,

        /// <summary>
        /// Player is now the leader of this fellowship.
        /// </summary>
        FellowshipBroadcast = 0x04000000,

        /// <summary>
        /// Celestial Hand Society
        /// </summary>
        CelestialHandBroadcast = 0x08000000,

        /// <summary>
        /// Eldrytch Web Society
        /// </summary>
        EldrytchWebBroadcast = 0x10000000,

        /// <summary>
        /// Radiant Blood Society
        /// </summary>
        RadiantBloodBroadcast = 0x20000000,

        /// <summary>
        /// Olthoi
        /// </summary>
        OlthoiBroadcast = 0x40000000
    }

    public enum LogTextType
    {
        LTT_DEFAULT = 0,
        LTT_ALL_CHANNELS,
        LTT_SPEECH,
        LTT_SPEECH_DIRECT,
        LTT_SPEECH_DIRECT_SEND,
        LTT_SYSTEM_EVENT,
        LTT_COMBAT,
        LTT_MAGIC,
        LTT_CHANNEL,
        LTT_CHANNEL_SEND,
        LTT_SOCIAL_CHANNEL,
        LTT_SOCIAL_CHANNEL_SEND,
        LTT_EMOTE,
        LTT_ADVANCEMENT,
        LTT_ABUSE_CHANNEL,
        LTT_HELP_CHANNEL,
        LTT_APPRAISAL_CHANNEL,
        LTT_MAGIC_CASTING_CHANNEL,
        LTT_ALLEGIENCE_CHANNEL,
        LTT_FELLOWSHIP_CHANNEL,
        LTT_WORLD_BROADCAST,
        LTT_COMBAT_ENEMY,
        LTT_COMBAT_SELF,
        LTT_RECALL,
        LTT_CRAFT,
        LTT_SALVAGING,
        LTT_ERROR,
        LTT_GENERAL_CHANNEL,
        LTT_TRADE_CHANNEL,
        LTT_LFG_CHANNEL,
        LTT_ROLEPLAY_CHANNEL,
        LTT_SPEECH_DIRECT_ADMIN,
        LTT_SOCIETY_CHANNEL,
        LTT_OLTHOI_CHANNEL,
        LTT_TOTAL_NUM_CHANNELS
    }

    [Flags]
    public enum ChannelID
    {
        Undef_ChannelID = 0,
        Abuse_ChannelID = 0x1,
        Admin_ChannelID = 0x2,
        Audit_ChannelID = 0x4,
        Advocate1_ChannelID = 0x8,
        Advocate2_ChannelID = 0x10,
        Advocate3_ChannelID = 0x20,
        QA1_ChannelID = 0x40,
        QA2_ChannelID = 0x80,
        Debug_ChannelID = 0x100,
        Sentinel_ChannelID = 0x200,
        Help_ChannelID = 0x400,
        AllBroadcast_ChannelID = 0x401,
        ValidChans_ChannelID = 0x73F,
        Fellow_ChannelID = 0x800,
        Vassals_ChannelID = 0x1000,
        Patron_ChannelID = 0x2000,
        Monarch_ChannelID = 0x4000,
        AlArqas_ChannelID = 0x8000,
        Holtburg_ChannelID = 0x10000,
        Lytelthorpe_ChannelID = 0x20000,
        Nanto_ChannelID = 0x40000,
        Rithwic_ChannelID = 0x80000,
        Samsur_ChannelID = 1048576,
        Shoushi_ChannelID = 2097152,
        Yanshi_ChannelID = 4194304,
        Yaraq_ChannelID = 8388608,
        Covassals_ChannelID = 16777216,
        TownChans_ChannelID = 16744448,
        AllegianceBroadcast_ChannelID = 33554432,
        FellowBroadcast_channelID = 67108864,
        GhostChans_ChannelID = 117471232,
        AllChans_ChannelID = 117473087
    }
}
