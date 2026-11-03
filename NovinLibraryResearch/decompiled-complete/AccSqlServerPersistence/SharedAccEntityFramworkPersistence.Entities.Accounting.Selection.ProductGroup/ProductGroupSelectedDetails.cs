using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

[Table("Acc_PrdGroupSelectedDetails")]
public class ProductGroupSelectedDetails : IPrdGroupSelectedDetList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid GroupGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public Guid MasterGuid { get; set; }

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

	[NotMapped]
	public bool Status { get; set; }

	public ProductGroupSelected Master { get; set; }

	public AccSqlServerPersistence.Entities.ProductGroup GroupItem { get; set; }
}
