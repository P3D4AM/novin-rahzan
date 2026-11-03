using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebOrderAbad : IOrderAbad, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private List<WebAbad> _detList;

	public Document OrderType { get; set; }

	public Guid DetailGuid { get; set; }

	public Guid RefrencedGuid { get; set; }

	public ShapeInfo ShapeInfo { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Obsolete]
	public int RefrencedID { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public List<WebAbad> ListAbad
	{
		get
		{
			_detList = _detList?.Where((WebAbad p) => p != null)?.ToList() ?? new List<WebAbad>();
			return _detList;
		}
		set
		{
			_detList = value;
		}
	}
}
