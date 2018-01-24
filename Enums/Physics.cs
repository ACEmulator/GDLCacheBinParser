using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhatACCacheBinParser.Enums
{
    public enum PhysicsState
    {
        STATIC_PS = (1 << 0),
        UNUSED1_PS = (1 << 1),
        ETHEREAL_PS = (1 << 2),
        REPORT_COLLISIONS_PS = (1 << 3),
        IGNORE_COLLISIONS_PS = (1 << 4),
        NODRAW_PS = (1 << 5),
        MISSILE_PS = (1 << 6),
        PUSHABLE_PS = (1 << 7),
        ALIGNPATH_PS = (1 << 8),
        PATHCLIPPED_PS = (1 << 9),
        GRAVITY_PS = (1 << 10),
        LIGHTING_ON_PS = (1 << 11),
        PARTICLE_EMITTER_PS = (1 << 12),
        UNNUSED2_PS = (1 << 13),
        HIDDEN_PS = (1 << 14),
        SCRIPTED_COLLISION_PS = (1 << 15),
        HAS_PHYSICS_BSP_PS = (1 << 16),
        INELASTIC_PS = (1 << 17),
        HAS_DEFAULT_ANIM_PS = (1 << 18),
        HAS_DEFAULT_SCRIPT_PS = (1 << 19),
        CLOAKED_PS = (1 << 20),
        REPORT_COLLISIONS_AS_ENVIRONMENT_PS = (1 << 21),
        EDGE_SLIDE_PS = (1 << 22),
        SLEDDING_PS = (1 << 23),
        FROZEN_PS = (1 << 24)
    }

    namespace PhysicsObjHook
    {
        public enum HookType
        {
            SCALING,
            TRANSLUCENCY,
            PART_TRANSLUCENCY,
            LUMINOSITY,
            DIFFUSION,
            PART_LUMINOSITY,
            PART_DIFFUSION,
            CALL_PES
        }
    }

    public enum HookTypeEnum
    {
        Undef_HookTypeEnum = 0,
        Floor_HookTypeEnum = (1 << 0),
        Wall_HookTypeEnum = (1 << 1),
        Ceiling_HookTypeEnum = (1 << 2),
        Yard_HookTypeEnum = (1 << 3),
        Roof_HookTypeEnum = (1 << 4)
    }

    public enum PhysicsTimeStamp
    {
        POSITION_TS,
        MOVEMENT_TS,
        STATE_TS,
        VECTOR_TS,
        TELEPORT_TS,
        SERVER_CONTROLLED_MOVE_TS,
        FORCE_POSITION_TS,
        OBJDESC_TS,
        INSTANCE_TS,
        NUM_PHYSICS_TS
    }

    public enum SetPositionError
    {
        OK_SPE,
        GENERAL_FAILURE_SPE,
        NO_VALID_POSITION_SPE,
        NO_CELL_SPE,
        COLLIDED_SPE,
        INVALID_ARGUMENTS = 256
    }

    public enum SetPositionFlag
    {
        PLACEMENT_SPF = (1 << 0),
        TELEPORT_SPF = (1 << 1),
        RESTORE_SPF = (1 << 2),
        // NOTE: Skip 1
        SLIDE_SPF = (1 << 4),
        DONOTCREATECELLS_SPF = (1 << 5),
        // NOTE: Skip 2
        SCATTER_SPF = (1 << 8),
        RANDOMSCATTER_SPF = (1 << 9),
        LINE_SPF = (1 << 10),
        // NOTE: Skip 1
        SEND_POSITION_EVENT_SPF = (1 << 12)
    }

    public enum ObjCollisionProfile_Bitfield
    {
        Undef_ECPB = 0,
        Creature_OCPB = (1 << 0),
        Player_OCPB = (1 << 1),
        Attackable_OCPB = (1 << 2),
        Missile_OCPB = (1 << 3),
        Contact_OCPB = (1 << 4),
        MyContact_OCPB = (1 << 5),
        Door_OCPB = (1 << 6),
        Cloaked_OCPB = (1 << 7)
    }

    public enum EnvCollisionProfile_Bitfield
    {
        Undef_ECPB = 0,
        MyContact_ECPB = (1 << 0)
    }

    public enum TransientState
    {
        CONTACT_TS = (1 << 0),
        ON_WALKABLE_TS = (1 << 1),
        SLIDING_TS = (1 << 2),
        WATER_CONTACT_TS = (1 << 3),
        STATIONARY_FALL_TS = (1 << 4),
        STATIONARY_STOP_TS = (1 << 5),
        STATIONARY_STUCK_TS = (1 << 6),
        ACTIVE_TS = (1 << 7),
        CHECK_ETHEREAL_TS = (1 << 8)
    }

    public enum TransitionState
    {
        INVALID_TS,
        OK_TS,
        COLLIDED_TS,
        ADJUSTED_TS,
        SLID_TS
    }

    enum PhysicsDescInfo
    {
        CSetup = (1 << 0),
        MTABLE = (1 << 1),
        VELOCITY = (1 << 2),
        ACCELERATION = (1 << 3),
        OMEGA = (1 << 4),
        PARENT = (1 << 5),
        CHILDREN = (1 << 6),
        OBJSCALE = (1 << 7),
        FRICTION = (1 << 8),
        ELASTICITY = (1 << 9),
        TIMESTAMPS = (1 << 10),
        STABLE = (1 << 11),
        PETABLE = (1 << 12),
        DEFAULT_SCRIPT = (1 << 13),
        DEFAULT_SCRIPT_INTENSITY = (1 << 14),
        POSITION = (1 << 15),
        MOVEMENT = (1 << 16),
        ANIMFRAME_ID = (1 << 17),
        TRANSLUCENCY = (1 << 18)
    }

    public class PublicWeenieDesc
    {
        public enum PublicWeenieDescPackHeader
        {
            PWD_Packed_None = 0,
            PWD_Packed_PluralName = (1 << 0),
            PWD_Packed_ItemsCapacity = (1 << 1),
            PWD_Packed_ContainersCapacity = (1 << 2),
            PWD_Packed_Value = (1 << 3),
            PWD_Packed_Useability = (1 << 4),
            PWD_Packed_UseRadius = (1 << 5),
            PWD_Packed_Monarch = (1 << 6),
            PWD_Packed_UIEffects = (1 << 7),
            PWD_Packed_AmmoType = (1 << 8),
            PWD_Packed_CombatUse = (1 << 9),
            PWD_Packed_Structure = (1 << 10),
            PWD_Packed_MaxStructure = (1 << 11),
            PWD_Packed_StackSize = (1 << 12),
            PWD_Packed_MaxStackSize = (1 << 13),
            PWD_Packed_ContainerID = (1 << 14),
            PWD_Packed_WielderID = (1 << 15),
            PWD_Packed_ValidLocations = (1 << 16),
            PWD_Packed_Location = (1 << 17),
            PWD_Packed_Priority = (1 << 18),
            PWD_Packed_TargetType = (1 << 19),
            PWD_Packed_BlipColor = (1 << 20),
            PWD_Packed_Burden = (1 << 21),
            PWD_Packed_SpellID = (1 << 22),
            PWD_Packed_RadarEnum = (1 << 23),
            PWD_Packed_Workmanship = (1 << 24),
            PWD_Packed_HouseOwner = (1 << 25),
            PWD_Packed_HouseRestrictions = (1 << 26),
            PWD_Packed_PScript = (1 << 27),
            PWD_Packed_HookType = (1 << 28),
            PWD_Packed_HookItemTypes = (1 << 29),
            PWD_Packed_IconOverlay = (1 << 30),
            PWD_Packed_MaterialType = (1 << 31)
        }

        public enum PublicWeenieDescPackHeader2
        {
            PWD2_Packed_None = 0,
            PWD2_Packed_IconUnderlay = (1 << 0),
            PWD2_Packed_CooldownID = (1 << 1),
            PWD2_Packed_CooldownDuration = (1 << 2),
            PWD2_Packed_PetOwner = (1 << 3),
        }

        public enum BitfieldIndex
        {
            BF_OPENABLE = (1 << 0),
            BF_INSCRIBABLE = (1 << 1),
            BF_STUCK = (1 << 2),
            BF_PLAYER = (1 << 3),
            BF_ATTACKABLE = (1 << 4),
            BF_PLAYER_KILLER = (1 << 5),
            BF_HIDDEN_ADMIN = (1 << 6),
            BF_UI_HIDDEN = (1 << 7),
            BF_BOOK = (1 << 8),
            BF_VENDOR = (1 << 9),
            BF_PKSWITCH = (1 << 10),
            BF_NPKSWITCH = (1 << 11),
            BF_DOOR = (1 << 12),
            BF_CORPSE = (1 << 13),
            BF_LIFESTONE = (1 << 14),
            BF_FOOD = (1 << 15),
            BF_HEALER = (1 << 16),
            BF_LOCKPICK = (1 << 17),
            BF_PORTAL = (1 << 18),
            // NOTE: Skip 1
            BF_ADMIN = (1 << 20),
            BF_FREE_PKSTATUS = (1 << 21),
            BF_IMMUNE_CELL_RESTRICTIONS = (1 << 22),
            BF_REQUIRES_PACKSLOT = (1 << 23),
            BF_RETAINED = (1 << 24),
            BF_PKLITE_PKSTATUS = (1 << 25),
            BF_INCLUDES_SECOND_HEADER = (1 << 26),
            BF_BINDSTONE = (1 << 27),
            BF_VOLATILE_RARE = (1 << 28),
            BF_WIELD_ON_USE = (1 << 29),
            BF_WIELD_LEFT = (1 << 30),
        }
    }

    public class OldPublicWeenieDesc
    {
        public enum OldPublicWeenieDescPackHeader
        {
            PWD_Packed_None = 0,
            PWD_Packed_PluralName = (1 << 0),
            PWD_Packed_ItemsCapacity = (1 << 1),
            PWD_Packed_ContainersCapacity = (1 << 2),
            PWD_Packed_Value = (1 << 3),
            PWD_Packed_Useability = (1 << 4),
            PWD_Packed_UseRadius = (1 << 5),
            PWD_Packed_Monarch = (1 << 6),
            PWD_Packed_UIEffects = (1 << 7),
            PWD_Packed_AmmoType = (1 << 8),
            PWD_Packed_CombatUse = (1 << 9),
            PWD_Packed_Structure = (1 << 10),
            PWD_Packed_MaxStructure = (1 << 11),
            PWD_Packed_StackSize = (1 << 12),
            PWD_Packed_MaxStackSize = (1 << 13),
            PWD_Packed_ContainerID = (1 << 14),
            PWD_Packed_WielderID = (1 << 15),
            PWD_Packed_ValidLocations = (1 << 16),
            PWD_Packed_Location = (1 << 17),
            PWD_Packed_Priority = (1 << 18),
            PWD_Packed_TargetType = (1 << 19),
            PWD_Packed_BlipColor = (1 << 20),
            PWD_Packed_VendorClassID = (1 << 21),
            PWD_Packed_SpellID = (1 << 22),
            PWD_Packed_RadarEnum = (1 << 23),
            PWD_Packed_RadarDistance = (1 << 24),
            PWD_Packed_HouseOwner = (1 << 25),
            PWD_Packed_HouseRestrictions = (1 << 26),
            PWD_Packed_PScript = (1 << 27),
            PWD_Packed_HookType = (1 << 28),
            PWD_Packed_HookItemTypes = (1 << 29),
            PWD_Packed_IconOverlay = (1 << 30),
            PWD_Packed_MaterialType = (1 << 31)
        }

        public enum BitfieldIndex
        {
            BF_OPENABLE = (1 << 0),
            BF_INSCRIBABLE = (1 << 1),
            BF_STUCK = (1 << 2),
            BF_PLAYER = (1 << 3),
            BF_ATTACKABLE = (1 << 4),
            BF_PLAYER_KILLER = (1 << 5),
            BF_HIDDEN_ADMIN = (1 << 6),
            BF_UI_HIDDEN = (1 << 7),
            BF_BOOK = (1 << 8),
            BF_VENDOR = (1 << 9),
            BF_PKSWITCH = (1 << 10),
            BF_NPKSWITCH = (1 << 11),
            BF_DOOR = (1 << 12),
            BF_CORPSE = (1 << 13),
            BF_LIFESTONE = (1 << 14),
            BF_FOOD = (1 << 15),
            BF_HEALER = (1 << 16),
            BF_LOCKPICK = (1 << 17),
            BF_PORTAL = (1 << 18),
            // NOTE: Skip 1
            BF_ADMIN = (1 << 20),
            BF_FREE_PKSTATUS = (1 << 21),
            BF_IMMUNE_CELL_RESTRICTIONS = (1 << 22),
            BF_REQUIRES_PACKSLOT = (1 << 23),
            BF_RETAINED = (1 << 24),
            BF_PKLITE_PKSTATUS = (1 << 25),
            BF_INCLUDES_SECOND_HEADER = (1 << 26),
            BF_BINDSTONE = (1 << 27),
            BF_VOLATILE_RARE = (1 << 28),
            BF_WIELD_ON_USE = (1 << 29),
            BF_WIELD_LEFT = (1 << 30),
        }
    }
}
