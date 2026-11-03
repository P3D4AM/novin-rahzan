using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Issabel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserSettings : Serializable<UserSettings>
{
	public UserCheckSettings Checks { get; set; } = new UserCheckSettings();


	public UserHesabyarSettings Hesabyar { get; set; } = new UserHesabyarSettings();


	public UserVisitingTour VisitingTour { get; set; } = new UserVisitingTour();


	public UserCustomerSettings CustomerS { get; set; } = new UserCustomerSettings();


	public UserPrintSettings Print { get; set; } = new UserPrintSettings();


	public UserProductSettings Products { get; set; } = new UserProductSettings();


	public UserSellSettings Sell { get; set; } = new UserSellSettings();


	public UserOrderSettings Order { get; set; } = new UserOrderSettings();


	public UserBarcodeSettings Barcode { get; set; } = new UserBarcodeSettings();


	public UserOtherSettings OtherSettings { get; set; } = new UserOtherSettings();


	public UserHesabSettings Hesabs { get; set; } = new UserHesabSettings();


	public UserResidSettings Resids { get; set; } = new UserResidSettings();


	public DateTime Modified { get; set; } = new DateTime(2000, 1, 1);


	public List<Guid> CusMotafaregheList { get; set; } = new List<Guid>();


	public List<Guid> OrderSellTemplateGuidList { get; set; } = new List<Guid>();


	public List<Guid> OrderBuyTemplateGuidList { get; set; } = new List<Guid>();


	public List<Guid> OrderBackSellTemplateGuidList { get; set; } = new List<Guid>();


	public List<Guid> OrderBackBuyTemplateGuidList { get; set; } = new List<Guid>();


	public List<Guid> PishFatorTemplateGuidList { get; set; } = new List<Guid>();


	public List<IsabelleNumber> NumberList { get; set; }

	public Guid DefaultMenuGuid { get; set; }

	public EnInvoiceOrderScaleDisplay InvoiceOrderScaleDisplay { get; set; }
}
