using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebLog : Serializable<WebLog>, ILog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private const int ChunckSizeKb = 100;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime DateM { get; set; }

	public string Message { get; set; }

	public string UserDescription { get; set; }

	public LogPartEnum LogPart { get; set; }

	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture() + " " + DateM.ToString("t");
}
