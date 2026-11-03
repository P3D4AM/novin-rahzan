using System.Collections.Generic;

namespace PacketParser.Entities.Accounting.DiscountCoupon;

public class CouponDiscountMetaData
{
	public List<CouponSmsSetting> CouponSmsSettingsList { get; set; }

	public List<WebCouponDiscountUsages> Usages { get; set; }
}
