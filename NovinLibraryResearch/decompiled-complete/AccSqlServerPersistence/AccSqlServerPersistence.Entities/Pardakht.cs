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

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Pardakht")]
public class Pardakht : IPardakht, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, IDeliverableEntity, IHasMetaData
{
	private DateTime _modified;

	private DateTime _date;

	private DateTime _dateSabt;

	[Key]
	public Guid Guid { get; set; }

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

	public bool Status { get; set; } = true;


	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public bool IsTemp { get; set; }

	public string? AndroidIme { get; set; }

	[Required]
	public string FullName { get; set; }

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

	public Guid MoeinGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid UserGuid { get; set; }

	public string? Ip { get; set; }

	public bool IsRead { get; set; }

	public string? LkSerial { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public decimal Sum { get; set; }

	public decimal SumCheckM { get; set; }

	public decimal SumCheckSh { get; set; }

	public decimal SumEnteghal { get; set; }

	public decimal SumBank { get; set; }

	public decimal SumNaghd { get; set; }

	public DateTime? TransferDate { get; set; }

	[NotMapped]
	public List<PardakhtBank> Bank { get; set; } = new List<PardakhtBank>();


	[NotMapped]
	public List<PardakhtCheckM> CheckM { get; set; } = new List<PardakhtCheckM>();


	[NotMapped]
	public List<PardakhtCheckSh> CheckSh { get; set; } = new List<PardakhtCheckSh>();


	[NotMapped]
	public List<PardakhtNaghd> Naghd { get; set; } = new List<PardakhtNaghd>();


	[NotMapped]
	public List<Sanad_Det> Enteghal { get; set; } = new List<Sanad_Det>();


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public short StatusId { get; set; }

	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public DateTime? DateMoaser { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Order OrderItem { get; set; }

	public ICollection<PardakhtBank> PardakhtBankItem { get; set; }

	public ICollection<PardakhtNaghd> PardakhtNaghdItem { get; set; }

	public ICollection<PardakhtCheckM> PardakhtCheckMItem { get; set; }

	public ICollection<PardakhtCheckSh> PardakhtCheckShItem { get; set; }

	public ICollection<AghsatSh> AghsatShakhsi { get; set; }

	public ICollection<Lottery> Lotteries { get; set; }

	public ICollection<Vam> Vam { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Tafsil TafsilItem2 { get; set; }

	public Tafsil TafsilItem3 { get; set; }

	public Moein MoeinItem { get; set; }

	public Cashier CashierItem { get; set; }

	public ICollection<BillMaster> BillMaster { get; set; }

	public ICollection<OpenInvoice> OpenInvoices { get; set; }

	public string AutoTitle(ENAction act)
	{
		throw new NotImplementedException();
	}
}
