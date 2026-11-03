using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebRasGiri : IRasGiri, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private SanadSt _sanadType;

	public int Id { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public int DayCounts => 0;

	public DateTime BaseDateM { get; set; }

	public string BaseDateSh => BaseDateM.MiladiToCalture();

	public int DayCount { get; set; }

	public int RecordCount { get; set; }

	public decimal Sood_Mounthly { get; set; }

	public decimal Sood_Total { get; set; }

	public decimal Sood_Percent { get; set; }

	public decimal Total_Sum { get; set; }

	public int RecordCounts => 0;

	public decimal SoodMonthly => 0m;

	public decimal SoodTotal => 0m;

	public float SoodPercent { get; set; }

	public decimal TotalSum => 0m;

	public int HId { get; set; }

	public short Htype { get; set; }

	public int SanadID { get; set; }

	public Guid SanadGuid { get; set; }

	SanadSt IRasGiri.SanadType
	{
		get
		{
			return _sanadType;
		}
		set
		{
			_sanadType = value;
		}
	}

	public SanadSt SanadType { get; set; }

	public List<WebRasGiriDet> Items { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;

}
