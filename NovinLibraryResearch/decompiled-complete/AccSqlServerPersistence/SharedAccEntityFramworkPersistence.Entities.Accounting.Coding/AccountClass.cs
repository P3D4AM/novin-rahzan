using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Coding;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

[Table("Acc_Accounts")]
public class AccountClass : IAccount, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	public Guid Guid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public decimal Account { get; set; }

	public string? MetaData { get; set; }

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

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public Moein Moein { get; set; }

	public Tafsil Tafsil { get; set; }
}
