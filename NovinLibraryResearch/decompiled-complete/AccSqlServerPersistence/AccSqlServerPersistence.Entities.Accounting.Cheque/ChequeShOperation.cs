using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Cheque;

namespace AccSqlServerPersistence.Entities.Accounting.Cheque;

[Table("Acc_ChequeShOperation")]
public class ChequeShOperation : IChequeShOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public SanadSt SanadSt { get; set; }

	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid ChequeGuid { get; set; }

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

	public ENCheckSHStatus? OldState { get; set; }

	public ENCheckSHStatus NewState { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public short StatusId { get; set; }

	public string? MetaData { get; set; }

	public decimal Price { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Cashier Cashier { get; set; }

	public PardakhtCheckSh Cheque { get; set; }

	public Moein CreditMoein { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein DebitMoein { get; set; }

	public Tafsil DebitTafsil { get; set; }
}
