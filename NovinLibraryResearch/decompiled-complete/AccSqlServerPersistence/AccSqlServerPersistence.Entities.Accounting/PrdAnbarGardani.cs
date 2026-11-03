using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdAnbarGardani")]
public class PrdAnbarGardani : IPrdAnbarGardani, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

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
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	[NotMapped]
	public bool Status { get; set; }

	public DateTime DateM { get; set; }

	public DateTime DateSabt { get; set; }

	public int Number { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid? KasrDebitTafsilGuid { get; set; }

	public Guid? KasrCreditTafsilGuid { get; set; }

	public Guid EzafeDebitMoeinGuid { get; set; }

	public Guid EzafeCreditMoeinGuid { get; set; }

	public Guid KasrDebitMoeinGuid { get; set; }

	public Guid KasrCreditMoeinGuid { get; set; }

	public Guid? EzafeDebitTafsilGuid { get; set; }

	public Guid? EzafeCreditTafsilGuid { get; set; }

	public decimal KasrSum { get; set; }

	public decimal EzafeSum { get; set; }

	public Guid UserGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public short StatusId { get; set; }

	public string? MetaData { get; set; }

	public SanadType Type { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Tafsil KasrDebitTafsil { get; set; }

	public Moein KasrDebitMoein { get; set; }

	public Tafsil KasrCreditTafsil { get; set; }

	public Moein KasrCreditMoein { get; set; }

	public Tafsil EzafeDebitTafsil { get; set; }

	public Moein EzafeDebitMoein { get; set; }

	public Tafsil EzafeCreditTafsil { get; set; }

	public Moein EzafeCreditMoein { get; set; }

	public Cashier CashierItem { get; set; }

	public bool IsTemp { get; set; }

	public ICollection<PrdAnbarGardaniDet> Details { get; set; }

	public ICollection<Evaluation> EvaluationKasr { get; set; }

	public ICollection<Evaluation> EvaluationEzafe { get; set; }
}
