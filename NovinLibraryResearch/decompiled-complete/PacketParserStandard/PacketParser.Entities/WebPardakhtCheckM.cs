using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebPardakhtCheckM : Serializable<WebPardakhtCheckM>, IPardakhtCheckM, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public int IdCheck { get; set; }

	public string Description { get; set; } = "";


	public decimal Price { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid GuidCheck { get; set; }

	public Guid? DecimalGuid { get; set; }

	[JsonIgnore]
	public Guid MasterGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiName { get; set; }

	public bool SayadiRegistered { get; set; }

	public int Ordinal { get; set; }
}
