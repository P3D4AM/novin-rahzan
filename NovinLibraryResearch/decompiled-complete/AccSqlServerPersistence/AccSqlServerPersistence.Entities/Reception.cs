using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Reception")]
public class Reception : IReception, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, IDeliverableEntity, IHasMetaData
{
	private DateTime _modified;

	private DateTime _date;

	private DateTime _dateSabt;

	[Key]
	public Guid Guid { get; set; }

	public bool? DeliveryStatus { get; set; }

	public bool IsChecked { get; set; }

	public ENSource Source { get; set; }

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

	public bool IsTemp { get; set; }

	public string? AndroidIme { get; set; }

	public string? CpuSerial { get; set; }

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
	public string Description { get; set; }

	public string? HddSerial { get; set; }

	public int Id2 { get; set; }

	[Required]
	public string FullName { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid UserGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public string? Ip { get; set; }

	public string? LkSerial { get; set; }

	public int SanadNo { get; set; }

	public short SanadSt { get; set; }

	public decimal Sum { get; set; }

	public decimal SumNaghd { get; set; }

	public decimal SumBank { get; set; }

	public decimal SumCheck { get; set; }

	public decimal SumEnteghal { get; set; }

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public bool Status { get; set; }

	public DependencyType DependencyType { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public DateTime? TransferDate { get; set; }

	public short StatusId { get; set; }

	public DateTime? DateMoaser { get; set; }

	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Guid? OrderGuid { get; set; }

	public Document? OrderType { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	[NotMapped]
	public List<ReceptionCheck> ListCheck { get; set; } = new List<ReceptionCheck>();


	[NotMapped]
	public List<ReceptionNaghd> ListNaghd { get; set; } = new List<ReceptionNaghd>();


	[NotMapped]
	public List<ReceptionHavale> ListBank { get; set; } = new List<ReceptionHavale>();


	[NotMapped]
	public List<Sanad_Det> ListEnteghal { get; set; } = new List<Sanad_Det>();


	public ICollection<AghsatM> AghsatAshkhas { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavaleItem { get; set; }

	public ICollection<ReceptionNaghd> ReceptionNaghdItem { get; set; }

	public ICollection<ReceptionCheck> ReceptionCheckMItem { get; set; }

	public ICollection<Resid> Resid { get; set; }

	public ICollection<BillMaster> BillMaster { get; set; }

	public ICollection<OpenInvoice> OpenInvoices { get; set; }

	public ICollection<Reservation> Reservation { get; set; }

	public Order OrderItem { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Tafsil TafsilItem2 { get; set; }

	public Tafsil TafsilItem3 { get; set; }

	public Moein MoeinItem { get; set; }

	public Cashier CashierItem { get; set; }
}
