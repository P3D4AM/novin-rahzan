using System;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebGardeshTafsiliCompact : IGardeshTafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private decimal credit;

	public int RowIndex { get; set; }

	public DateTime Date { get; set; }

	public Guid? DepotGuid { get; set; }

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public short AsType { get; set; }

	public Document EnAsType { get; set; }

	public DocumentDet AsTypeEnum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit
	{
		get
		{
			return credit;
		}
		set
		{
			credit = value;
		}
	}

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public decimal Bazaryab { get; set; }

	public Guid? VGuid { get; set; }

	public Guid? PrdGuid { get; set; }

	public string SanadDesc { get; set; }

	public DateTime DateMoaser { get; set; }

	public decimal Rem { get; set; }

	public Guid? DecimalGuid { get; set; }

	public string RowDesc { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid KolGuid { get; set; }

	public Guid UGuid { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal DecimalQt { get; set; }

	public Guid DetailGuid { get; set; }

	[JsonIgnore]
	public DateTime Modified { get; set; }

	[JsonIgnore]
	public bool Status => true;

	[JsonIgnore]
	public Guid Guid { get; set; }
}
