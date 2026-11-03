using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebReceptionNaghd : IReceptionNaghd, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public int IdSandogh { get; set; }

	public decimal Price { get; set; }

	public HesabType HType { get; }

	public int HTreeId { get; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	[Obsolete("", true)]
	public DateTime? DateMUtc { get; set; }

	public DateTime? DateUtc { get; set; }

	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	[JsonIgnore]
	public Guid MasterGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? DecimalGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	public string ShortDescription => "نقد";

	[Obsolete]
	public int Id { get; set; }

	public int Ordinal { get; set; }
}
