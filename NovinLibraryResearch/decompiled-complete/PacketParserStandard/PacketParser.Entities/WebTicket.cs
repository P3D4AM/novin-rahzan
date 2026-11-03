using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebTicket : Serializable<WebTicket>, ITicket, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public int Number { get; set; }

	public Guid SourceGuid { get; set; }

	public EnTargetType SourceType { get; set; }

	public string Title { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public EnTargetType TargetType { get; set; }

	public Guid TargetGuid { get; set; }

	public short StatusId { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Priority Priority { get; set; }

	public List<WebFileInfo> AddedFiles { get; set; }

	public List<WebFileInfo> RemovedFiles { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }
}
