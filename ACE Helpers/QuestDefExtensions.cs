
using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class QuestDefExtensions
    {
        public static Quest ConvertToACE(this Seg8_QuestDefDB.QuestDef input)
        {
            var result = new Quest
            {
                Name = input.Name,

                MinDelta = input.MinDelta,
                MaxSolves = input.MaxSolves,

                Message = input.Message
            };

            return result;
        }
    }
}
