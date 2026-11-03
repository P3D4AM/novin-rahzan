using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("Global_Menu")]
public class Menu : IMenu, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public int RowCount { get; set; }

	public int ColumnCount { get; set; }

	[Required]
	[MaxLength(500)]
	public string? Title { get; set; }

	public string? Description { get; set; }

	public bool Status { get; set; }

	public EnMenuType Type { get; set; }

	public Guid? MasterGuid { get; set; }

	public Menu Master { get; set; }

	public ICollection<MenuItem> DetailItem { get; set; }

	public ICollection<Menu> Details { get; set; }
}
