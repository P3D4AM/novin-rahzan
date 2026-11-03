using System;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs.Connection;

public class ConnectionDataEventArgs : System.EventArgs
{
	[JsonIgnore]
	[XmlIgnore]
	public StringBuilder Sb { get; set; }

	public byte[] Data { get; set; }

	public string Ip { get; set; }

	public string ConnectionUniqueId { get; set; }

	public WebContextHolder context { get; set; }

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Connection\\ConnectionDataEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Connection\\ConnectionDataEventArgs.cs", 28);
				return null;
			}
		}
	}
}
