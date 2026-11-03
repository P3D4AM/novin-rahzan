using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebVam : IVam, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public int Id2 { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string DateSh { get; }

	public decimal Zarib { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public Guid VamMoeinGuid { get; set; }

	public Guid VamTafsilGuid { get; set; }

	public Guid SepordeMoeinGuid { get; set; }

	public Guid SepordeTafsilGuid { get; set; }

	public Guid? AghsatGuid { get; set; }

	public decimal Sum { get; set; }

	public Guid? ZamenGuid1 { get; set; }

	public Guid? ZamenGuid2 { get; set; }

	public Guid? ZamenGuid3 { get; set; }

	public Guid? ZamenGuid4 { get; set; }

	public DateTime Date1 { get; set; }

	public DateTime Date2 { get; set; }

	public short Average { get; set; }

	public ENMoadel AverageType { get; set; }

	public decimal AverageSum { get; set; }

	public decimal ProfitPercent { get; set; }
}
