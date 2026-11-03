using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_TolidSummaryDetail")]
public class TolidSummaryDetail : ITolidSummaryDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
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

	public Guid MasterGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public Guid? TolidGuid { get; set; }

	public decimal RealAmount { get; set; }

	public decimal AvailableAmount { get; set; }

	public decimal ProducedAmount { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public TolidSummary MasterItem { get; set; }

	public Product ProductItem { get; set; }

	public Depot DepotItem { get; set; }

	public string? MetaData { get; set; }
}
