using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdAnbarGardaniDet")]
public class PrdAnbarGardaniDet : IPrdAnbarGardaniDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid PrdGuid { get; set; }

	public Guid DptGuid { get; set; }

	public decimal QtKasr { get; set; }

	public decimal QtEzafe { get; set; }

	public decimal UnitPrice { get; set; }

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

	public decimal KasrSum { get; set; }

	public decimal EzafeSum { get; set; }

	[Column("RealAmount")]
	public decimal? QtRealAmount { get; set; }

	public PrdAnbarGardani Master { get; set; }

	public Product Product { get; set; }

	public Depot Depot { get; set; }

	public string? MetaData { get; set; }
}
