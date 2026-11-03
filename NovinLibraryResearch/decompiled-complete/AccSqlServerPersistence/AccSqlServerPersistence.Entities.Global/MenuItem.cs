using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("Global_MenuItems")]
public class MenuItem : IMenuItem, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
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

	public EnMenuItemType Type { get; set; }

	public Guid TypeGuid { get; set; }

	public int Ordinal { get; set; }

	public bool Status { get; set; }

	public Guid MasterGuid { get; set; }

	public Menu MasterItem { get; set; }
}
