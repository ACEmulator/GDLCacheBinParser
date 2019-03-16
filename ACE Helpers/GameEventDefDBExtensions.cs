using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class GameEventDefDBExtensions
    {
        public static List<Event> ConvertToACE(this SegB_GameEventDefDB.GameEventDefDB input)
        {
            var results = new List<Event>();

            foreach (var value in input.GameEventDefs)
            {
                var converted = value.ConvertToACE();

                results.Add(converted);
            }

            return results;
        }

        public static Event ConvertToACE(this SegB_GameEventDefDB.GameEventDef input)
        {
            var result = new Event
            {
                Name = input.Name,

                StartTime = input.StartTime,
                EndTime = input.EndTime,

                State = (int)input.GameEventState,

                LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
            };

            return result;
        }
    }
}
