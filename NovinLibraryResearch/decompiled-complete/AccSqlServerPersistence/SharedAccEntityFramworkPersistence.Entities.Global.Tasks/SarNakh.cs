using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

[Table("Global_SarNakh")]
public class SarNakh : ISarNakh, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(500)]
	public string? Company { get; set; }

	[MaxLength(500)]
	public string? FullName { get; set; }

	public Guid CashierGuid { get; set; }

	public string? Description { get; set; }

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

	public string? MetaData { get; set; }

	public short StatusId { get; set; }

	public Guid? IntroductionGuid { get; set; }

	public DateTime CreationDate { get; set; }

	public IntroductionMethod Introduction { get; set; }

	public ICollection<SarNakhLocation> Locations { get; set; }

	public ICollection<SarNakhNumbers> Numbers { get; set; }
}
