using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebPrdAnbarRelation : Serializable<IPrdAnbarRelation>, IPrdAnbarRelation, IHasModified, IReceiver
{
	[Obsolete("", true)]
	public short IdAnbar { get; set; }

	public decimal Amount { get; set; }

	public decimal Min { get; set; }

	public decimal Max { get; set; }

	public decimal Price { get; set; }

	public Guid PrdGuid { get; set; }

	[JsonIgnore]
	public DateTime Modified { get; set; }

	public Guid AnbarGuid { get; set; }

	public string GetPacketActionEnumName(short action)
	{
		PrdAnbarRelationActions prdAnbarRelationActions = (PrdAnbarRelationActions)action;
		return prdAnbarRelationActions.ToString();
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionName(short action)
	{
		PrdAnbarRelationActions prdAnbarRelationActions = (PrdAnbarRelationActions)action;
		return prdAnbarRelationActions.ToString();
	}
}
