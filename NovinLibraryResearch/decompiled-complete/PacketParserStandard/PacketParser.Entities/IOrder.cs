using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IOrder : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo
{
	Guid? AghsatGuid { get; set; }

	string AndroidIme { get; set; }

	long AndroidImeLong { get; set; }

	decimal Avarez { get; }

	decimal B1 { get; }

	decimal B2 { get; }

	decimal B3 { get; }

	decimal B4 { get; }

	string BarnameNo { get; set; }

	string BazaryabFullName { get; }

	string CpuSerial { get; set; }

	DateTime DateSabt { get; }

	string Description { get; set; }

	decimal Discount { get; set; }

	decimal DiscountRows { get; }

	Document FactorType { get; set; }

	short DailyFactorNo { get; set; }

	Guid? HazineTafsilGuid1 { get; set; }

	Guid? HazineTafsilGuid2 { get; set; }

	Guid? HazineTafsilGuid3 { get; set; }

	Guid? HazineTafsilGuid4 { get; set; }

	Guid? HazineTafsilGuid5 { get; set; }

	Guid? HazineMoeinGuid1 { get; set; }

	Guid? HazineMoeinGuid2 { get; set; }

	Guid? HazineMoeinGuid3 { get; set; }

	Guid? HazineMoeinGuid4 { get; set; }

	Guid? HazineMoeinGuid5 { get; set; }

	bool? DeliveryStatus { get; set; }

	ENSource Source { get; set; }

	string HddSerial { get; set; }

	Guid MoeinGuid { get; set; }

	bool IsTemp { get; }

	string Ip { get; set; }

	decimal Keraye { get; }

	Guid? FareMoeinGuid { get; set; }

	Guid? FareTafsilGuid { get; set; }

	string LkSerial { get; set; }

	Guid? PardakhtGuid { get; }

	Guid? PriceTypeGuid { get; set; }

	short PrintCount { get; set; }

	DateTime? PrintDate { get; set; }

	Guid? ReceptionGuid { get; }

	SanadSt SanadSt { get; set; }

	decimal Sum { get; }

	decimal SumKala { get; }

	decimal SumService { get; }

	int TasvieDay { get; set; }

	decimal Tax { get; }

	decimal TaxImpact { get; }

	decimal TotalBazaryab { get; }

	decimal TotalDiscount { get; }

	decimal TotalHazine { get; }

	decimal TotalHazine1 { get; }

	decimal TotalHazine2 { get; }

	decimal TotalHazine3 { get; }

	decimal TotalHazine4 { get; }

	decimal TotalHazine5 { get; }

	decimal TotalSum { get; }

	Guid UserAccGuid { get; set; }

	Guid? B1MoeinGuid { get; set; }

	Guid? B2MoeinGuid { get; set; }

	Guid? B3MoeinGuid { get; set; }

	Guid? B4MoeinGuid { get; set; }

	Guid? AvarezTafsilGuid { get; set; }

	Guid? AvarezMoeinGuid { get; set; }

	Guid? TaxTafsilGuid { get; set; }

	Guid? TaxMoeinGuid { get; set; }

	string CustName { get; set; }

	string CustFamily { get; set; }

	string CustMobile { get; set; }

	string CustAddress { get; set; }

	string CustZipCode { get; set; }

	string CustTell { get; set; }

	PaymentMethod PaymentMethod { get; set; }

	string SellerFactorNo { get; set; }

	DateTime? TransferDate { get; set; }

	Guid ProductMoeinGuid { get; set; }

	Guid DiscountMoeinGuid { get; set; }

	Guid? DiscountTafsilGuid { get; set; }

	int AttachmentNo { get; set; }

	short StatusId { get; set; }

	Guid? IntroductionMethodGuid { get; set; }

	DateTime? TasvieDate { get; set; }

	DateTime? TasvieDateMoaser { get; set; }

	DateTime? DateUtc { get; set; }

	Guid? CashBackCouponGuid { get; set; }

	bool IsCashBackProgressed { get; set; }

	Guid? ServiceMoeinGuid { get; set; }

	string AutoTitle(ENAction act);
}
