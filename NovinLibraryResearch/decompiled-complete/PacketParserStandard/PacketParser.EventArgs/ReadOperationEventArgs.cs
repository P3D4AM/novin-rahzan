using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ReadOperationEventArgs : BaseEventArgs
{
	public bool ChangeReadST { get; set; }

	public short Top { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now.AddMonths(-1);


	public List<ReadStatus> Status { get; set; }

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ReadOperationEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ReadOperationEventArgs.cs", 24);
				throw;
			}
		}
	}
}
