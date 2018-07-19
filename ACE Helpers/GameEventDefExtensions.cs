
using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class GameEventDefExtensions
    {
        public static Event ConvertToACE(this SegB_GameEventDefDB.GameEventDef input)
        {
            var result = new Event
            {
                Name = input.Name,

                StartTime = input.StartTime,
                EndTime = input.EndTime,

                State = (int)input.GameEventState
            };

            return result;
        }
    }
}
