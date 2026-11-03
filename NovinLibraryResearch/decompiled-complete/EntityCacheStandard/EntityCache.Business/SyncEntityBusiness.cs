using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class SyncEntityBusiness : IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public EntityPart Type { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime Modified { get; set; }

	public byte[] SerializedData { get; set; }

	public string ContextGuid { get; set; }

	public SyncEntityBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
