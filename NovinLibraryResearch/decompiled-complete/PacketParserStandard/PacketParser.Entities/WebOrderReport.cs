using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebOrderReport : IOrder, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo
{
	public EnTaxResult TaxResult { get; set; }

	public string TaxRefrenceNumber { get; set; }

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public string TaxUid { get; set; }

	public string TaxOrderId { get; set; }

	public decimal SumService { get; set; }

	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }

	public string IntroductionMethodName { get; set; }

	public DateTime? DateUtc { get; set; }

	public Guid? AghsatGuid { get; set; }

	public string AndroidIme { get; set; }

	public decimal AfterAccount { get; set; }

	public long AndroidImeLong { get; set; }

	public decimal Avarez { get; set; }

	public decimal B1 { get; set; }

	public Guid? B1TafsilGuid { get; set; }

	public Guid? B2TafsilGuid { get; set; }

	public Guid? B3TafsilGuid { get; set; }

	public Guid? B4TafsilGuid { get; set; }

	public decimal B2 { get; set; }

	public decimal B3 { get; set; }

	public decimal B4 { get; set; }

	public string BarnameNo { get; set; }

	public decimal BeforeAccount { get; set; }

	public string BazaryabFullName { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public DateTime Date { get; set; }

	public Guid TafsilGuid { get; set; }

	public int Id2 { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public string HostRefrenced { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public string CustomerFullName { get; set; }

	public string DateSh { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public string Description { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountRows { get; set; }

	public Document FactorType { get; set; }

	public short DailyFactorNo { get; set; }

	public Guid? HazineTafsilGuid1 { get; set; }

	public Guid? HazineTafsilGuid2 { get; set; }

	public Guid? HazineTafsilGuid3 { get; set; }

	public Guid? HazineTafsilGuid4 { get; set; }

	public Guid? HazineTafsilGuid5 { get; set; }

	public Guid? HazineMoeinGuid1 { get; set; }

	public Guid? HazineMoeinGuid2 { get; set; }

	public Guid? HazineMoeinGuid3 { get; set; }

	public Guid? HazineMoeinGuid4 { get; set; }

	public Guid? HazineMoeinGuid5 { get; set; }

	public bool? DeliveryStatus { get; set; }

	public ENSource Source { get; set; }

	public string HddSerial { get; set; }

	public Guid MoeinGuid { get; set; }

	public bool IsTemp { get; set; }

	public string Ip { get; set; }

	public decimal Keraye { get; set; }

	public Guid? FareMoeinGuid { get; set; }

	public Guid? FareTafsilGuid { get; set; }

	public string LkSerial { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public short PrintCount { get; set; }

	public DateTime? PrintDate { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public SanadSt SanadSt { get; set; }

	public decimal Sum { get; set; }

	public decimal SumKala { get; set; }

	public int TasvieDay { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxImpact { get; set; }

	public decimal TotalBazaryab { get; set; }

	public decimal TotalDiscount { get; set; }

	public decimal TotalHazine { get; set; }

	public decimal TotalHazine1 { get; set; }

	public decimal TotalHazine2 { get; set; }

	public decimal TotalHazine3 { get; set; }

	public decimal TotalHazine4 { get; set; }

	public decimal TotalHazine5 { get; set; }

	public decimal TotalSum { get; set; }

	public Guid UserAccGuid { get; set; }

	public Guid? B1MoeinGuid { get; set; }

	public Guid? B2MoeinGuid { get; set; }

	public Guid? B3MoeinGuid { get; set; }

	public Guid? B4MoeinGuid { get; set; }

	public Guid? AvarezTafsilGuid { get; set; }

	public Guid? AvarezMoeinGuid { get; set; }

	public Guid? TaxTafsilGuid { get; set; }

	public Guid? TaxMoeinGuid { get; set; }

	public string CustName { get; set; }

	public string CustFamily { get; set; }

	public string CustMobile { get; set; }

	public string CustAddress { get; set; }

	public string CustZipCode { get; set; }

	public string CustTell { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public string SellerFactorNo { get; set; }

	public DateTime? TransferDate { get; set; }

	public Guid ProductMoeinGuid { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	public Guid DiscountMoeinGuid { get; set; }

	public Guid? DiscountTafsilGuid { get; set; }

	public int AttachmentNo { get; set; }

	public short StatusId { get; set; }

	public Guid? IntroductionMethodGuid { get; set; }

	public DateTime? TasvieDate { get; set; }

	public DateTime? TasvieDateMoaser { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public int SanadNo { get; set; }

	public Guid? CashBackCouponGuid { get; set; }

	public bool IsCashBackProgressed { get; set; }

	public Guid? ServiceMoeinGuid { get; set; }

	public string AutoTitle(ENAction act)
	{
		throw new NotImplementedException();
	}
}
