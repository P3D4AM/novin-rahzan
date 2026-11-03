using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_AbadDet")]
public class AbadDet : IAbadDet, IHasMasterGuid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public decimal X { get; set; }

	public decimal Y { get; set; }

	public decimal Z { get; set; }

	public decimal Count { get; set; }

	public Shape ShapeType { get; set; }

	public ShapeInfo ShapeInformation { get; set; }

	[NotMapped]
	public bool Status { get; }

	public OrderAbad Master { get; set; }
}
