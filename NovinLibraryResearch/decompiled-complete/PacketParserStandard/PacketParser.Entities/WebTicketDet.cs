using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;

namespace PacketParser.Entities;

public class WebTicketDet : Serializable<WebTicketDet>, ITicketDet, IHasMasterGuid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable
{
	public Guid Guid { get; set; } = Guid.NewGuid();


	public Guid MasterGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public bool IsSeen { get; set; }

	public DateTime? SeenTime { get; set; }

	public Guid SourceGuid { get; set; }

	public EnTargetType SourceType { get; set; }

	public string Description { get; set; }

	public string Title { get; set; }

	public DateTime DateM { get; set; }

	public List<WebFileInfo> AddedFiles { get; set; }

	public List<WebFileInfo> RemovedFiles { get; set; }

	public TicketMetaData MetaData { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }
}
