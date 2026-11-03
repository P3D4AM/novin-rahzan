using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting;

[Table("Acc_ProductPriceUpdate")]
public class ProductPriceUpdate : IProductPriceUpdate, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

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

	public DateTime DateM { get; set; }

	public decimal RegularPrice { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Product Product { get; set; }
}
