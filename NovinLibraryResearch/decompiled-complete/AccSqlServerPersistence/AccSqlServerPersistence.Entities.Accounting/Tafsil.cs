using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Accounting.Note;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Entities.Global;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Tafsil")]
public class Tafsil : ITafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Name { get; set; }

	public long Code { get; set; }

	public bool Status { get; set; }

	[Required]
	public string Description { get; set; }

	public bool System { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public HesabType HType { get; set; }

	public DateTime DateM { get; set; }

	public string? SerializedData { get; set; }

	public DateTime Modified { get; set; }

	public decimal SumAghsat { get; set; }

	public int Version { get; set; }

	public bool IsSpecial { get; set; }

	public ICollection<Order> TafsilOrder { get; set; }

	public ICollection<AccountLog> AccountLogs { get; set; }

	public ICollection<Order> B1Order { get; set; }

	public ICollection<Order> B2Order { get; set; }

	public ICollection<Order> B3Order { get; set; }

	public ICollection<Order> B4Order { get; set; }

	public ICollection<Order> Hazine1Order { get; set; }

	public ICollection<Order> Hazine2Order { get; set; }

	public ICollection<Order> Hazine3Order { get; set; }

	public ICollection<Order> Hazine4Order { get; set; }

	public ICollection<Order> TaxOrder { get; set; }

	public ICollection<Order> AvarezOrder { get; set; }

	public ICollection<Order> FareOrder { get; set; }

	public ICollection<Cashier> DefaultFareOrder { get; set; }

	public ICollection<OrderDetail> ProductOrder { get; set; }

	public ICollection<Order> DiscountOrder { get; set; }

	public ICollection<TafsilNumber> TafsilNumber { get; set; }

	public ICollection<Pardakht> TafsilPardakht { get; set; }

	public ICollection<Pardakht> TafsilPardakht2 { get; set; }

	public ICollection<Pardakht> TafsilPardakht3 { get; set; }

	public ICollection<PardakhtBank> PardakhtBank { get; set; }

	public ICollection<PardakhtNaghd> PardakhtNaghd { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShA2re { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShDebitA2re { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShBank { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShCredit { get; set; }

	public ICollection<PardakhtCheckM> PardakhtCheckMCredit { get; set; }

	public ICollection<Reception> TafsilReception { get; set; }

	public ICollection<Reception> TafsilReception2 { get; set; }

	public ICollection<Reception> TafsilReception3 { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavale { get; set; }

	public ICollection<ReceptionNaghd> ReceptionNaghd { get; set; }

	public ICollection<ReceptionCheck> ReceptionCheck { get; set; }

	public ICollection<ReceptionCheck> DebitReceptionCheck { get; set; }

	public ICollection<ReceptionCheck> CreditReceptionCheck { get; set; }

	public ICollection<ReceptionCheck> VisitorReceptionCheck { get; set; }

	public ICollection<AghsatSh> AghsatShakhsi { get; set; }

	public ICollection<ChequeBook> ChequeBooks { get; set; }

	public ICollection<FareOrderCredit> FareOrderCredits { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<Kardex> Kardexes { get; set; }

	public ICollection<Kardex> KardexProductSide { get; set; }

	public ICollection<Lottery> Lotteries { get; set; }

	public ICollection<TolidFormulaHazine> TolidFormulaHaizne { get; set; }

	public ICollection<Resid> Resid { get; set; }

	public ICollection<Sanad_Det> SanadDet { get; set; }

	public ICollection<Sanad_Det> SanadDet2 { get; set; }

	public ICollection<Sanad_Det> SanadDet3 { get; set; }

	public ICollection<Sanad_Det> SanadDet4 { get; set; }

	public ICollection<LotteryScore> LotteryScores { get; set; }

	public ICollection<PrdAnbarGardani> KasrDebit { get; set; }

	public ICollection<PrdAnbarGardani> KasrCredit { get; set; }

	public ICollection<PrdAnbarGardani> EzafeDebit { get; set; }

	public ICollection<PrdAnbarGardani> EzafeCredit { get; set; }

	public ICollection<TolidHazine> TolidHazine { get; set; }

	public ICollection<Vam> VamSepordeTafsil { get; set; }

	public ICollection<Vam> VamGirandeTafsil { get; set; }

	public ICollection<Vam> VamZamen1 { get; set; }

	public ICollection<Vam> VamZamen2 { get; set; }

	public ICollection<Vam> VamZamen3 { get; set; }

	public ICollection<Vam> VamZamen4 { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> Mavad { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> Mahsol { get; set; }

	public ICollection<Cashier> Cashiers { get; set; }

	public ICollection<ViewSanadDetailCover> DebitViewSanadDetailCover { get; set; }

	public ICollection<ViewSanadDetailCover> CreditViewSanadDetailCover { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover2 { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover3 { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover4 { get; set; }

	public ICollection<PardakhtBank> PardakhtBankWage { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavaleWage { get; set; }

	public ICollection<ReceptionChequeEntezami> ReceptionChequeEntezamiDebit { get; set; }

	public ICollection<ReceptionChequeEntezami> ReceptionChequeEntezamiCredit { get; set; }

	public ICollection<PardakhtChequeEntezami> PardakhtChequeEntezamiDebit { get; set; }

	public ICollection<PardakhtChequeEntezami> PardakhtChequeEntezamiCredit { get; set; }

	public ICollection<AccountClass> Accounts { get; set; }

	public ICollection<Receipt> Receipts { get; set; }

	public ICollection<Receipt> ReceiptsPresentor { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperationDebit { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperationCredit { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperationDebit { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperationCredit { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails2 { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails3 { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails4 { get; set; }

	public ICollection<LastPrice> LastPrice { get; set; }

	public Customer Customer { get; set; }

	public TafsilGroup TafsilGroupItems { get; set; }

	public ICollection<DepotReceipt> DepotReceipts { get; set; }

	public ICollection<AghsatM_Group> AghsatGroup { get; set; }

	public ICollection<AghsatM_Group> CreditAghsatGroup { get; set; }

	public ICollection<AghsatM> CreditAghsat { get; set; }

	public ICollection<OpenInvoice> OpenInvoices { get; set; }

	public ICollection<Reservation> Reservation { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Note.Note> Notes { get; set; }

	public ICollection<TempBarcodePrint> TempBarcodePrint { get; set; }

	public ICollection<DepEnteghal> CreditDepEnteghal { get; set; }

	public ICollection<DepEnteghal> DebitDepEnteghal { get; set; }
}
