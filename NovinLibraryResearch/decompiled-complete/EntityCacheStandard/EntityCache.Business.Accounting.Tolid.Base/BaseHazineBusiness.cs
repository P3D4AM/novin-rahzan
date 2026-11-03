using System;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.Tolid.Base;

public abstract class BaseHazineBusiness : Serializable<BaseHazineBusiness>, IHasContext, IHasJustGuid
{
	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public string ContextGuid { get; set; }

	public bool Status { get; } = true;


	public Guid MasterGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[JsonIgnore]
	public long MoeinCode => Moein?.Code ?? 0;

	[JsonIgnore]
	public string MoeinName => Moein?.Name;

	[JsonIgnore]
	public Guid? TafsilGuid { get; set; }

	[JsonIgnore]
	public long TafsilCode => Tafsil?.Code ?? 0;

	[JsonIgnore]
	public string TafsilName => Tafsil?.Name;

	public decimal Price { get; set; }

	public string Description { get; set; }

	public HazineType Type { get; set; }

	public string TypeName => Type.GetDisplay().GetTranslate();

	[JsonIgnore]
	public MoeinBusiness Moein => MoeinBusiness.Get(ContextGuid, MoeinGuid);

	[JsonIgnore]
	public TafsilBusiness Tafsil
	{
		get
		{
			if (TafsilGuid.HasValue)
			{
				return TafsilBusiness.Get(ContextGuid, TafsilGuid.Value);
			}
			return null;
		}
	}

	public BaseHazineBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
