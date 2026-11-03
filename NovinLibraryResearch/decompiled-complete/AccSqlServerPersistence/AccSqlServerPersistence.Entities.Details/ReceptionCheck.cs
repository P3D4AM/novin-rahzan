using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_ReceptionCheck")]
public class ReceptionCheck : IReceptionCheck, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	private DateTime _modified;

	private DateTime _date;

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

	public ServerStatusForClient ServerStatus { get; set; }

	bool IHasStatus.Status { get; }

	[Required]
	public string BankName { get; set; }

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

	public ENUMCheck Status { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public string HesabNum { get; set; }

	[Required]
	public string Number { get; set; }

	public bool Personal { get; set; }

	public long PoshtNomreh { get; set; }

	public decimal Price { get; set; }

	public Guid? MasterGuid { get; set; }

	Guid IHasMasterGuid.MasterGuid { get; set; }

	public Guid? A2re_PardazandeMoeinGuid { get; set; }

	public Guid? A2re_PardazandeTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid? CreditA2reMoeinGuid { get; set; }

	public Guid? CreditA2reTafsilGuid { get; set; }

	public DateTime? A2re_ReceptionDate { get; set; }

	public bool Aval2re { get; set; }

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public Guid? VisitorGuid { get; set; }

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

	public string? MetaData { get; set; }

	public bool SayadiRegistered { get; set; }

	public int Ordinal { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public ICollection<PardakhtCheckM> PardakhtCheckMItem { get; set; }

	public ICollection<ChequeMOperation> ChequeMOperation { get; set; }

	public ReceptionChequeEntezami ReceptionChequeEntezami { get; set; }

	public Tafsil A2re_TafsilItem { get; set; }

	public Moein A2re_MoeinItem { get; set; }

	public Tafsil DebitTafsil { get; set; }

	public Moein DebitMoein { get; set; }

	public Reception ReceptionItem { get; set; }

	public Cashier CashierItem { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein CreditMoein { get; set; }

	public Tafsil VisitorTafsil { get; set; }
}
