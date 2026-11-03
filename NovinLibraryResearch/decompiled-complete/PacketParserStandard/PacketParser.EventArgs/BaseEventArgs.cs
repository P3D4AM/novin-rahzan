using System;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class BaseEventArgs
{
	[JsonIgnore]
	public int Part { get; set; }

	[JsonIgnore]
	public int TotalParts { get; set; }

	public Guid PacketReferenceId { get; set; }

	[JsonIgnore]
	public string ConnectionUniqueId { get; set; }

	[JsonIgnore]
	public ConnectionData Connection { get; set; }

	public override string ToString()
	{
		try
		{
			return Json.ToStringJson(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\BaseEventArgs.cs", 25);
			return "";
		}
	}

	public string UniqueKey()
	{
		return PacketReferenceId.ToString() + Part;
	}
}
