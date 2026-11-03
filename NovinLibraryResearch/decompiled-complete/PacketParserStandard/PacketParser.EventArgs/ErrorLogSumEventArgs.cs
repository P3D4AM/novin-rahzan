using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ErrorLogSumEventArgs : BaseEventArgs
{
	public int Id { get; set; }

	public short State { get; set; }

	public DateTime Min { get; set; }

	public DateTime Max { get; set; }

	public DateTime? Modified { get; set; }

	public List<ENSource> Sources { get; set; }

	public string HashList { get; set; }

	public short Top { get; set; }

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ErrorLogSumEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ErrorLogSumEventArgs.cs", 66);
				throw;
			}
		}
	}
}
