using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting;

[Table("Acc_Evaluation")]
public class Evaluation : IEvaluation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public int Id2 { get; set; }

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

	public DateTime DateM { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid CashierGuid { get; set; }

	public string? Description { get; set; }

	public Guid AnbarGardaniEzafeGuid { get; set; }

	public Guid AnbarGardaniKasrGuid { get; set; }

	public string? MetaData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public Cashier CashierItem { get; set; }

	public PrdAnbarGardani AnbarGardaniEzafeItem { get; set; }

	public PrdAnbarGardani AnbarGardaniKasrItem { get; set; }
}
