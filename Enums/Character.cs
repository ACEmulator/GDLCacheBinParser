
namespace PhatACCacheBinParser.Enums
{
    public enum AetheriaBitfield
    {
        NoSlotsUnlocked,
        OneSlotUnlocked,
        // Skip 1
        TwoSlotsUnlocked = 3,
        // Skip 3
        ThreeSlotsUnlocked = 7
    }

    // Gleaned from client. See gmCharacterInfoUI::UpdateAugmentations()
    public enum SummoningMastery
    {
        Undef,
        Primalist,
        Necromancer,
        Naturalist
    }
}
