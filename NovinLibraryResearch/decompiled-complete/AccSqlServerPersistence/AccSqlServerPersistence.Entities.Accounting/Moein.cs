using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Moein")]
public class Moein : IMoein, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Name { get; set; }

	public Guid KolGuid { get; set; }

	public long Code { get; set; }

	public bool Status { get; set; }

	[Required]
	public string Description { get; set; }

	public ENMahiat Mahiat { get; set; }

	public bool System { get; set; }

	public bool Peygiri { get; set; }

	public bool Tedadi { get; set; }

	public decimal Account { get; set; }

	public DateTime DateM { get; set; }

	public byte[]? GardeshHesab { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public ICollection<MoeinTafsilGroupRelations> MoeinTafsilLevelRelation { get; set; }

	public ICollection<Order> MoeinOrder { get; set; }

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

	public ICollection<Order> ProductOrder { get; set; }

	public ICollection<Order> ServiceOrder { get; set; }

	public ICollection<Order> DiscountOrder { get; set; }

	public ICollection<Pardakht> MoeinPardakht { get; set; }

	public ICollection<PardakhtBank> PardakhtBank { get; set; }

	public ICollection<PardakhtBank> PardakhtBankWage { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavaleWage { get; set; }

	public ICollection<PardakhtNaghd> PardakhtNaghd { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShA2re { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShDebitA2re { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShCredit { get; set; }

	public ICollection<PardakhtCheckM> PardakhtCheckMCredit { get; set; }

	public ICollection<Reception> MoeinReception { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavale { get; set; }

	public ICollection<ReceptionNaghd> ReceptionNaghd { get; set; }

	public ICollection<ReceptionCheck> ReceptionCheck { get; set; }

	public ICollection<ReceptionCheck> DebitReceptionCheck { get; set; }

	public ICollection<ReceptionCheck> CreditReceptionCheck { get; set; }

	public ICollection<AghsatSh> AghsatShakhsi { get; set; }

	public ICollection<FareOrderCredit> FareOrderCredits { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<Kardex> Kardexes { get; set; }

	public ICollection<TolidFormulaHazine> TolidFormulaHaizne { get; set; }

	public ICollection<Sanad_Det> SanadDet { get; set; }

	public ICollection<PrdAnbarGardani> KasrDebit { get; set; }

	public ICollection<PrdAnbarGardani> KasrCredit { get; set; }

	public ICollection<PrdAnbarGardani> EzafeDebit { get; set; }

	public ICollection<PrdAnbarGardani> EzafeCredit { get; set; }

	public ICollection<TolidHazine> TolidHazine { get; set; }

	public ICollection<Vam> VamGirandeMoein { get; set; }

	public ICollection<Vam> VamSepordeMoein { get; set; }

	public ICollection<CustomerGroup> CustomerGroups { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> Mavad { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> Mahsol { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperationDebit { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperationCredit { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperationDebit { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperationCredit { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails { get; set; }

	public ICollection<ViewSanadDetailCover> DebitViewSanadDetailCover { get; set; }

	public ICollection<ViewSanadDetailCover> CreditViewSanadDetailCover { get; set; }

	public ICollection<ReceptionChequeEntezami> ReceptionChequeEntezamiDebit { get; set; }

	public ICollection<ReceptionChequeEntezami> ReceptionChequeEntezamiCredit { get; set; }

	public ICollection<PardakhtChequeEntezami> PardakhtChequeEntezamiDebit { get; set; }

	public ICollection<PardakhtChequeEntezami> PardakhtChequeEntezamiCredit { get; set; }

	public ICollection<AccountClass> Accounts { get; set; }

	public ICollection<AccountLog> AccountLogs { get; set; }

	public ICollection<AghsatM_Group> AghsatGroup { get; set; }

	public ICollection<AghsatM_Group> CreditAghsatGroup { get; set; }

	public ICollection<AghsatM> CreditAghsat { get; set; }

	public ICollection<DepEnteghal> CreditDepEnteghal { get; set; }

	public ICollection<DepEnteghal> DebitDepEnteghal { get; set; }

	public Kol Kol { get; set; }
}
