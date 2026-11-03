using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_SettingBusiness : IScale_Setting, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Val { get; set; }

	public Guid UserGuid { get; set; }

	public DateTime Modified { get; set; }

	public string ContextGuid { get; set; }

	public bool Status { get; set; }

	public Scale_SettingBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
