using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("Global_Card")]
public class Card : ICard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public string? TextString1 { get; set; }

	public string? TextString2 { get; set; }

	public string? TextString3 { get; set; }

	public string? TextString4 { get; set; }

	public EnCardTheme Theme { get; set; }

	public string? ToolTipMessage { get; set; }

	public byte[]? Image { get; set; }

	public string? BackColor { get; set; }

	public EnCardType Type { get; set; }

	public bool Status { get; set; }

	public ICollection<CardTargetRelation> DetailItems { get; set; }
}
