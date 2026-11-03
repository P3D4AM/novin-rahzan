using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebPardakhtNaghd : IPardakhtNaghd, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public int IdSandogh { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public int TafsilId { get; }

	public int MoeinId { get; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[Obsolete]
	public int ID { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[JsonIgnore]
	public Guid MasterGuid { get; set; }

	public string ShortDescription => "نقد";

	public int Ordinal { get; set; }

	public DateTime DateM { get; set; }

	[Obsolete("", true)]
	public DateTime? DateMUtc { get; set; }

	public DateTime? DateUtc { get; set; }
}
