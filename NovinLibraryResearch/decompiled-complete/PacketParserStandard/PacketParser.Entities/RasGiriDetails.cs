using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class RasGiriDetails : IRasGiriDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private decimal _enheraf;

	public int Id { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH => DateM.MiladiToCalture();

	public Guid TafsilGuid { get; set; }

	public decimal Price { get; set; }

	private decimal Enheraf
	{
		get
		{
			return _enheraf;
		}
		set
		{
			_enheraf = value;
		}
	}

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; }

	public string Description { get; set; }
}
