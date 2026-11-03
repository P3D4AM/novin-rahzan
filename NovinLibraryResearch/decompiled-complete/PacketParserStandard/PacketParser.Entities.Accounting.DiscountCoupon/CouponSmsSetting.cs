using System;

namespace PacketParser.Entities.Accounting.DiscountCoupon;

public class CouponSmsSetting
{
	public Guid Guid { get; set; }

	public bool SendSmsWhenAddCoupon { get; set; }

	public bool SendSmsWhenEditCoupon { get; set; }

	public bool CalculationWithEndDate { get; set; }

	public int DayCount { get; set; }

	public string SmsText { get; set; }

	public PayamakSendStatus Status { get; set; }
}
