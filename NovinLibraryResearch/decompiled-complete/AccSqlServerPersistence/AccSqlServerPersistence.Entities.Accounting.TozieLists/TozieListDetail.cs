using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace AccSqlServerPersistence.Entities.Accounting.TozieLists;

[Table("Acc_TozieListDetails")]
public class TozieListDetail : ITozieListDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

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

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public TozieList MasterItem { get; set; }

	public string? MetaData { get; set; }
}
