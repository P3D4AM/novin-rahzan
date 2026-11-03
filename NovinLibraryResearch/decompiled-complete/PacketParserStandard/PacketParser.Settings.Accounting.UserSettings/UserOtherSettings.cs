using System;
using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserOtherSettings
{
	private bool _x;

	[DefaultValue(3)]
	public double Bahre { get; set; } = 3.0;


	[DefaultValue(true)]
	public bool PardakhtDisplayDate { get; set; } = true;


	public bool PardakhtDisplayDescription { get; set; }

	public bool PardakhtDisplayHesab { get; set; }

	public bool PardakhtDisplayNumber { get; set; }

	public bool PardakhtDisplayPrice { get; set; }

	public bool PardakhtDisplayUser { get; set; }

	[DefaultValue(true)]
	public bool ReceptionDisplayDate { get; set; } = true;


	public bool ReceptionDisplayDescription { get; set; }

	public bool ReceptionDisplayHesab { get; set; }

	public bool ReceptionDisplayNumber { get; set; }

	public bool ReceptionDisplayPrice { get; set; }

	public bool ReceptionDisplayUser { get; set; }

	public int ReceptionBesHTreeID { get; set; }

	public Guid ReceptionBesHTreeGuid { get; set; }

	public bool ReceptionDisplayHesabGroup { get; set; }

	public bool ReceptionDisplaySource { get; set; }

	[DefaultValue(3)]
	public double VamZarib { get; set; } = 3.0;


	[DefaultValue(true)]
	public bool SanadDisplayDate { get; set; } = true;


	public bool SanadDisplayDescription { get; set; }

	public bool SanadDisplayNumber { get; set; }

	public bool SanadDisplayPrice { get; set; }

	public bool SanadDisplayUser { get; set; }

	public bool SanadDisplaySanadType { get; set; }

	public bool SanadDisplaySanadStatus { get; set; }

	[DefaultValue(true)]
	public bool ShowCalendar { get; set; } = true;


	[DefaultValue(true)]
	public bool AnalogClock { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowUser { get; set; } = true;


	[DefaultValue(true)]
	public bool MAliYearTitle { get; set; } = true;


	[DefaultValue(true)]
	public bool ShowStatuseBar { get; set; } = true;


	public string BackGroungImage { get; set; } = "";


	[DefaultValue(true)]
	public bool FastAccessKeys { get; set; } = true;


	[DefaultValue(false)]
	public bool NPACCEnable { get; set; }

	public bool ReceptionDisplayNaghd { get; set; }

	public bool ReceptionDisplayHavale { get; set; }

	public bool ReceptionDisplayCheque { get; set; }

	public bool ReceptionDisplayServerStatus { get; set; }

	public bool PardakhtDisplayNaghd { get; set; }

	public bool PardakhtDisplayHavale { get; set; }

	public bool PardakhtDisplayChequeSh { get; set; }

	public bool PardakhtDisplayChequeM { get; set; }

	public bool PardakhtDisplayHesabGroup { get; set; }

	public bool PardakhtDisplayServerStatus { get; set; }

	[DefaultValue(true)]
	public bool CallNotificationShow { get; set; } = true;


	public UserTarazHesabSettings Taraz { get; set; } = new UserTarazHesabSettings();


	public UserPlacementSettings Placement { get; set; } = new UserPlacementSettings();


	public UserReservationSettings Reservation { get; set; } = new UserReservationSettings();


	public UserEvaluationSettings Evaluation { get; set; } = new UserEvaluationSettings();


	public UserCouponSettings Coupon { get; set; } = new UserCouponSettings();


	public UserDepotReceiptSettings DepotReceipt { get; set; } = new UserDepotReceiptSettings();


	public UserDepotHavaleSettings DepotHavale { get; set; } = new UserDepotHavaleSettings();


	public UserTolidSettings Tolid { get; set; } = new UserTolidSettings();

}
