using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhatACCacheBinParser.Enums
{
    public enum ShopMode
    {
        SHOP_MODE_UNDEF,
        SHOP_MODE_NONE,
        SHOP_MODE_BUY,
        SHOP_MODE_SELL
    }

    public enum ShopEvent
    {
        SE_BUY,
        SE_SELL
    }

    public enum VendorTypeEnum
    {
        Undef_VendorTypeEmote = 0,
        Open_VendorTypeEmote = 1,
        Close_VendorTypeEmote = 2,
        Sell_VendorTypeEmote = 3,
        Buy_VendorTypeEmote = 4,
        Heartbeat_VendorTypeEmote = 5
    }
}
