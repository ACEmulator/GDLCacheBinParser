using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class QuestDefDBExtensions
    {
        public static List<Quest> ConvertToACE(this Seg8_QuestDefDB.QuestDefDB input)
        {
            var results = new List<Quest>();

            foreach (var value in input.QuestDefs)
            {
                var converted = value.ConvertToACE();

                results.Add(converted);
            }

            return results;
        }

        public static Quest ConvertToACE(this Seg8_QuestDefDB.QuestDef input)
        {
            var result = new Quest
            {
                Name = input.Name,

                MinDelta = input.MinDelta,
                MaxSolves = input.MaxSolves,

                Message = input.Message,

                LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
            };

            return result;
        }
    }
}
