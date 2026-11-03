using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_DepEnteghalDet")]
public class DepEnteghalDet : IDepEnteghalDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
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
	public bool Status { get; }

	public Guid MasterGuid { get; set; }

	public Guid DptGuidSrc { get; set; }

	public Guid DptGuidDst { get; set; }

	public short Ordinal { get; set; }

	public Guid PrdGuid { get; set; }

	public decimal Quantity { get; set; }

	public Guid? VahedGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V2Price { get; set; }

	public DepEnteghal DepEnteghal { get; set; }

	public Depot Depot { get; set; }

	public Product Product { get; set; }

	public ProductVahed ProductVahed { get; set; }

	public string? MetaData { get; set; }
}
