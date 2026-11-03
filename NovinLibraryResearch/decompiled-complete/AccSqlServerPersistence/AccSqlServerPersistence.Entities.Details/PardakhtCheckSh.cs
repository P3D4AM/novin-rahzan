using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_PardakhtCheckSh")]
public class PardakhtCheckSh : IPardakhtCheckSh, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	private DateTime _modified;

	private DateTime _dateSarresid;

	private DateTime _pardakhtDate;

	[Key]
	public Guid Guid { get; set; }

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

	bool IHasStatus.Status { get; }

	public DateTime DateSaresid
	{
		get
		{
			return _dateSarresid;
		}
		set
		{
			DateTime dateSarresid = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_dateSarresid = dateSarresid;
		}
	}

	[Required]
	public string Description { get; set; }

	[Required]
	public string Number { get; set; }

	public decimal Price { get; set; }

	public ENCheckSHStatus Status { get; set; }

	public Guid BankTafsilGuid { get; set; }

	public Guid? MasterGuid { get; set; }

	public string? MetaData { get; set; }

	Guid IHasMasterGuid.MasterGuid { get; set; }

	public DateTime PardakhtDate
	{
		get
		{
			return _pardakhtDate;
		}
		set
		{
			DateTime pardakhtDate = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_pardakhtDate = pardakhtDate;
		}
	}

	public bool Aval2re { get; set; }

	public Guid? A2re_MoeinGuid { get; set; }

	public Guid? A2re_TafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public Guid? DebitA2reMoeinGuid { get; set; }

	public Guid? DebitA2reTafsiGuid { get; set; }

	[NotMapped]
	public string ShortDescription { get; set; }

	public int? SanadNo { get; set; }

	[MaxLength(100)]
	public string? SayadiNationalCode { get; set; }

	[Required]
	[MaxLength(100)]
	public string SayadiIdentity { get; set; }

	[Required]
	[MaxLength(100)]
	public string SayadiName { get; set; }

	public bool SayadiRegistered { get; set; }

	public int Ordinal { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Moein MoeinItem { get; set; }

	public Pardakht PardakhtItem { get; set; }

	public Tafsil BankTafsilItem { get; set; }

	public Cashier CashierItem { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein CreditMoein { get; set; }

	public Tafsil DebitA2reTafsil { get; set; }

	public Moein DebitA2reMoein { get; set; }

	public PardakhtChequeEntezami PardakhtChequeEntezami { get; set; }

	public ICollection<ChequeShOperation> ChequeShOperation { get; set; }
}
