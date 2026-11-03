using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class DeliveryEventArgs : BaseEventArgs
{
	[JsonIgnore]
	public IEnumerable<WebDelivery> Deliveries { get; set; }

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\DeliveryEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\DeliveryEventArgs.cs", 24);
				throw;
			}
		}
	}
}
