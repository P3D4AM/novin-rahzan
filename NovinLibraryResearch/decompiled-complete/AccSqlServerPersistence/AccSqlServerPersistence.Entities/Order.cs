using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Order")]
public class Order : Serializable<Order>, IOrder, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IDeliverableEntity, IHasMetaData
{
	private DateTime _date;

	private DateTime _dateSabt;

	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public decimal AfterAccount { get; set; }

	public Guid? AghsatGuid { get; set; }

	[NotMapped]
	public new bool IsDisposed => base.IsDisposed;

	public string? AndroidIme { get; set; }

	public long AndroidImeLong { get; set; }

	public Guid? B1TafsilGuid { get; set; }

	public Guid? B2TafsilGuid { get; set; }

	public Guid? B3TafsilGuid { get; set; }

	public Guid? B4TafsilGuid { get; set; }

	public Guid? B1MoeinGuid { get; set; }

	public Guid? B2MoeinGuid { get; set; }

	public Guid? B3MoeinGuid { get; set; }

	public Guid? B4MoeinGuid { get; set; }

	public decimal B1 { get; set; }

	public decimal B2 { get; set; }

	public decimal B3 { get; set; }

	public decimal B4 { get; set; }

	public string? BarnameNo { get; set; }

	public string? BazaryabFullName { get; set; }

	public decimal BeforeAccount { get; set; }

	public string? CpuSerial { get; set; }

	public string? CustomerFullName { get; set; }

	public DateTime Date
	{
		get
		{
			return _date;
		}
		set
		{
			DateTime date = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_date = date;
		}
	}

	public DateTime DateSabt
	{
		get
		{
			return _dateSabt;
		}
		set
		{
			DateTime dateSabt = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_dateSabt = dateSabt;
		}
	}

	[Required]
	public string? Description { get; set; }

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

	public decimal TotalHazine { get; set; }

	public decimal TotalHazine1 { get; set; }

	public decimal TotalHazine2 { get; set; }

	public decimal TotalHazine3 { get; set; }

	public decimal TotalHazine4 { get; set; }

	public decimal TotalHazine5 { get; set; }

	public string? HddSerial { get; set; }

	public int Id2 { get; set; }

	public string? Ip { get; set; }

	public decimal Keraye { get; set; }

	public Guid? FareMoeinGuid { get; set; }

	public Guid? FareTafsilGuid { get; set; }

	public string? LkSerial { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public decimal PardakhtSum { get; set; }

	public short PrintCount { get; set; }

	public DateTime? PrintDate { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public decimal ReceptionSum { get; set; }

	public int SanadNo { get; set; }

	[NotMapped]
	public int SanadNumber { get; }

	public SanadSt SanadSt { get; set; }

	public Guid ProductMoeinGuid { get; set; }

	public decimal Sum { get; set; }

	public decimal SumKala { get; set; }

	public decimal SumService { get; set; }

	public int TasvieDay { get; set; }

	public decimal Avarez { get; set; }

	public Guid? AvarezTafsilGuid { get; set; }

	public Guid? AvarezMoeinGuid { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxImpact { get; set; }

	public Guid? TaxTafsilGuid { get; set; }

	public Guid? TaxMoeinGuid { get; set; }

	public decimal TotalBazaryab { get; set; }

	public decimal TotalDiscount { get; set; }

	public decimal TotalSum { get; set; }

	public Guid UserAccGuid { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public Guid? DeliveryGuid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_modified = modified;
		}
	}

	public bool Status { get; set; } = true;


	public Guid? PriceTypeGuid { get; set; }

	public bool IsTemp { get; set; }

	public string? HostRefrenced { get; set; }

	public string? CustName { get; set; }

	public string? CustFamily { get; set; }

	public string? CustMobile { get; set; }

	public string? CustAddress { get; set; }

	public string? CustZipCode { get; set; }

	public string? CustTell { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public string? SellerFactorNo { get; set; }

	public bool? DeliveryStatus { get; set; }

	public ENSource Source { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? TransferDate { get; set; }

	public Guid DiscountMoeinGuid { get; set; }

	public Guid? DiscountTafsilGuid { get; set; }

	public int AttachmentNo { get; set; }

	public short StatusId { get; set; }

	public Guid? IntroductionMethodGuid { get; set; }

	public string? MetaData { get; set; }

	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }

	public DateTime? TasvieDate { get; set; }

	public DateTime? TasvieDateMoaser { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Guid? CashBackCouponGuid { get; set; }

	public bool IsCashBackProgressed { get; set; }

	public Guid? ServiceMoeinGuid { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Moein MoeinItem { get; set; }

	public Tafsil B1TafsilItem { get; set; }

	public Tafsil B2TafsilItem { get; set; }

	public Tafsil B3TafsilItem { get; set; }

	public Tafsil B4TafsilItem { get; set; }

	public Moein B1MoeinItem { get; set; }

	public Moein B2MoeinItem { get; set; }

	public Moein B3MoeinItem { get; set; }

	public Moein B4MoeinItem { get; set; }

	public Tafsil Hazine1TafsilItem { get; set; }

	public Tafsil Hazine2TafsilItem { get; set; }

	public Tafsil Hazine3TafsilItem { get; set; }

	public Tafsil Hazine4TafsilItem { get; set; }

	public Moein Hazine1MoeinItem { get; set; }

	public Moein Hazine2MoeinItem { get; set; }

	public Moein Hazine3MoeinItem { get; set; }

	public Moein Hazine4MoeinItem { get; set; }

	public Tafsil TaxTafsilItem { get; set; }

	public Moein TaxMoeinItem { get; set; }

	public Tafsil AvarezTafsilItem { get; set; }

	public Moein AvarezMoeinItem { get; set; }

	public Tafsil FareTafsilItem { get; set; }

	public Moein FareMoeinItem { get; set; }

	public Cashier CashierItem { get; set; }

	public Pardakht PardakhtItem { get; set; }

	public CouponDiscount CouponDiscount { get; set; }

	public Reception ReceptionItem { get; set; }

	public Moein ProductMoeinItem { get; set; }

	public Moein ServiceMoeinItem { get; set; }

	public Moein DiscountMoeinItem { get; set; }

	public Tafsil DiscountTafsilItem { get; set; }

	public ProductPriceLevel ProductPriceLevel { get; set; }

	public ICollection<SanadStatusLog> OrderStatusLog { get; set; }

	public ICollection<OrderDetail> OrderDetailItem { get; set; }

	public ICollection<LotteryScore> LotteryScores { get; set; }

	public ICollection<Receipt> Receipts { get; set; }

	public ICollection<TempBill> TempBills { get; set; }

	public ICollection<Resid> Resid { get; set; }

	public ICollection<BillDetail> BillDetail { get; set; }

	public ICollection<TaxOrderLog> TaxOrderLog { get; set; }

	public ICollection<OpenInvoiceOrderRelation> OrderRelation { get; set; }

	public ICollection<PlacementOrderRelation> PlacementOrderRelation { get; set; }

	public ICollection<Reservation> Reservation { get; set; }

	public ICollection<ReservationDetail> ReservationDetail { get; set; }

	public ICollection<DepotReceipt> DepotReceipts { get; set; }

	public string? AutoTitle(ENAction act)
	{
		throw new NotImplementedException();
	}
}
