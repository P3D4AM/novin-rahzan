using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebProductAttribute : Serializable<WebProductAttribute>, IProductAttribute, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Slug { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public List<WebProductAttributeValue> DetList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }
}
