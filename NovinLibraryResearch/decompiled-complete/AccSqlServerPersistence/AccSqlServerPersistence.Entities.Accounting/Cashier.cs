using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using AccSqlServerPersistence.Entities.Accounting.Note;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Accounting.TozieLists;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Cashier")]
public class Cashier : ICashier, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid UserGuid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	[Required]
	public string Name { get; set; }

	[Required]
	public string UserName { get; set; }

	public byte[]? HashedPassword { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	[NotMapped]
	public List<CashierTime> CashierTime { get; set; }

	public string? StrCashierTime { get; set; }

	public string? Setting { get; set; }

	public Guid? SandoghGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public Guid? VisitorGuid { get; set; }

	public Guid? BankGuid1 { get; set; }

	public Guid? BankGuid2 { get; set; }

	public Guid? BankGuid3 { get; set; }

	public Guid? BankGuid4 { get; set; }

	public Guid? CusGuid { get; set; }

	public Guid? DefaultCusGroup { get; set; }

	public bool AllVisitorAccess { get; set; }

	public bool AllSandoghsAccess { get; set; }

	public bool AllDepotsAccess { get; set; }

	public bool AllBanksAccess { get; set; }

	public short LoginAccess { get; set; }

	public Guid? ReceptionSmsCustomerGuid { get; set; }

	public Guid? PardakhtSmsCustomerGuid { get; set; }

	public Guid FareMoeinGuid { get; set; }

	public Guid? FareTafsilGuid { get; set; }

	public ICollection<CallingLine> CallingLines { get; set; }

	public ICollection<CashiersLog> CashiersLogs { get; set; }

	public ICollection<Lottery> Lotteries { get; set; }

	public ICollection<ShortCut> ShortCuts { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> Tolid { get; set; }

	public ICollection<TolidSummary> TolidSummary { get; set; }

	public ICollection<LotteryScore> LotteryScores { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Note.Note> Notes { get; set; }

	public ICollection<Order> CashierOrder { get; set; }

	public ICollection<PardakhtCheckSh> CashierPardakhtCheckSh { get; set; }

	public ICollection<ReceptionCheck> CashierReceptionCheck { get; set; }

	public ICollection<Pardakht> CashierPardakht { get; set; }

	public ICollection<AghsatSh> AghsatShs { get; set; }

	public ICollection<TafsilSelected> CusSelecteds { get; set; }

	public ICollection<DepEnteghal> DepEnteghals { get; set; }

	public ICollection<Kardex> Kardexs { get; set; }

	public ICollection<PrdAnbarGardani> PrdAnbarGardanis { get; set; }

	public ICollection<PrdSelected> PrdSelecteds { get; set; }

	public ICollection<Reception> Receptions { get; set; }

	public ICollection<Sanads> SanadItems { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperation { get; set; }

	public ICollection<SanadStatusLog> SanadStatusLog { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperation { get; set; }

	public ICollection<TozieList> TozieLists { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails { get; set; }

	public ICollection<ProductGroupSelected> ProductGroupSelected { get; set; }

	public ICollection<Resid> Resid { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover { get; set; }

	public ICollection<ReceptionChequeEntezami> ReceptionChequeEntezami { get; set; }

	public ICollection<PardakhtChequeEntezami> PardakhtChequeEntezami { get; set; }

	public ICollection<CouponDiscount> CouponDiscounts { get; set; }

	public ICollection<BillMaster> BillMaster { get; set; }

	public ICollection<CallingLogDetail> CallingLogDetail { get; set; }

	public ICollection<SanadStatusReportUpdates> SanadStatusReportUpdates { get; set; }

	public ICollection<DepotReceipt> DepotReceipts { get; set; }

	public ICollection<AghsatM_Group> AghsatGroup { get; set; }

	public ICollection<OpenInvoice> OpenInvoices { get; set; }

	public ICollection<Reservation> Reservation { get; set; }

	public ICollection<Evaluation> Evaluation { get; set; }

	public UserAcc UserItem { get; set; }

	public Tafsil Customer { get; set; }

	public Tafsil FareTafsilItem { get; set; }

	public Moein FareMoeinItem { get; set; }
}
