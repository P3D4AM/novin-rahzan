using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Cheque;

namespace AccSqlServerPersistence.Entities.Accounting.Cheque;

[Table("Acc_ChequeMOperation")]
public class ChequeMOperation : IChequeMOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public int SanadNO { get; set; }

	public SanadSt SanadSt { get; set; }

	public DateTime DateSabt { get; set; }

	public bool? Avale2re_Transfer { get; set; }

	public Guid GuidCheque { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public short Ordinal { get; set; }

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
	public bool Status { get; }

	public ENUMCheck? OldState { get; set; }

	public ENUMCheck NewState { get; set; }

	public bool? IsDebitTafsilAffective { get; set; }

	public bool? IsCreditTafsilAffective { get; set; }

	public string? Description { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public Document? AsType { get; set; }

	public Guid? AsGuid { get; set; }

	public short StatusId { get; set; }

	[DefaultValue(true)]
	public bool IsSanadAffective { get; set; } = true;


	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	public Cashier Cashier { get; set; }

	public ReceptionCheck Cheque { get; set; }

	public Moein CreditMoein { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein DebitMoein { get; set; }

	public Tafsil DebitTafsil { get; set; }
}
