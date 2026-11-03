using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_OrderAbad")]
public class OrderAbad : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Document OrderType { get; set; }

	public Guid DetailGuid { get; set; }

	public Guid RefrencedGuid { get; set; }

	public ShapeInfo ShapeInfo { get; set; }

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

	public string? MetaData { get; set; }

	public ICollection<AbadDet> OrderAbads { get; set; }
}
