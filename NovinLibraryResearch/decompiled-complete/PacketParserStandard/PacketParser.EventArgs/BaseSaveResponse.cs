using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class BaseSaveResponse
{
	public string HostRefrenced { get; set; } = "";


	public int PartitionId { get; set; }

	public DateTime Date { get; set; } = DateTime.Now;


	public string Shortmessage { get; set; } = "";


	public string Message { get; set; } = "";


	[DefaultValue(true)]
	public bool SaveResultStatus { get; set; } = true;


	[DefaultValue(-1)]
	public int RequestPart { get; set; } = -1;


	public Guid RequestRefrencedId { get; set; } = Guid.Empty;


	public override string ToString()
	{
		try
		{
			return Json.ToStringJson(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 87);
			throw;
		}
	}
}
