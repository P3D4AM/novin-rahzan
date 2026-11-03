using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebEntitySyncByHash : Serializable<WebEntitySync>, IDisposable, INpDisposable
{
	public EntityPart EntityType { get; set; }

	public Guid EntityGuid { get; set; }

	public DateTime SyncTime { get; set; }

	public string LastSyncResultMessage { get; set; }

	public short SyncTryCount { get; set; }

	public bool LastSyncResult { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }
}
