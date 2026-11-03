using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebScript : Serializable<WebScript>, IScript, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver
{
	public int Id { get; set; }

	public string Script { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime? ExecTime { get; set; }

	public DateTime NextTime { get; set; } = DateTime.Now;


	public string Description { get; set; } = "";


	public string ExecDesc { get; set; } = "";


	public int ExecSt { get; set; }

	public DateTime Modified { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionName(short action)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionEnumName(short action)
	{
		throw new NotImplementedException();
	}
}
