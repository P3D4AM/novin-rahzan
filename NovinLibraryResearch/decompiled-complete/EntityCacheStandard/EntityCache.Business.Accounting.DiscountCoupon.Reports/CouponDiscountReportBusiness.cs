using System;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.DiscountCoupon.Reports;

public class CouponDiscountReportBusiness
{
	private int countCouponUse;

	private int countCouponUseForUser;

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public string Title { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public string DateFromSh => DateFrom.MiladiToCalture();

	public string DateToSh => DateTo.MiladiToCalture();

	public string UserName { get; set; }

	public int CountCouponUseForUser
	{
		get
		{
			if (Mode == EnCouponMode.Coupon)
			{
				return countCouponUseForUser;
			}
			return 0;
		}
		set
		{
			countCouponUseForUser = value;
		}
	}

	public int CountCouponUse
	{
		get
		{
			if (Mode == EnCouponMode.Coupon)
			{
				return countCouponUse;
			}
			return 0;
		}
		set
		{
			countCouponUse = value;
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public EnCouponType Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public EnCouponMode Mode { get; set; }

	public string ModeForDisplay => Mode.GetDisplay().GetTranslate();

	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public int UsedCount { get; set; }

	public int RemainCount
	{
		get
		{
			if (Mode == EnCouponMode.Coupon)
			{
				return checked(CountCouponUse - UsedCount);
			}
			return 0;
		}
	}
}
