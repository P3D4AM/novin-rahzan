using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class Desc
{
	[XmlIgnore]
	public Dictionary<DescSection, List<string>> Data { get; set; }

	public void Add(DescSection section, string Message)
	{
		using (new JaegerMonitor("Add", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\Desc.cs"))
		{
			try
			{
				if (Data == null)
				{
					Data = new Dictionary<DescSection, List<string>>();
				}
				if (Data.ContainsKey(section))
				{
					Get(section).Add(Message.FixString());
					return;
				}
				Data.Add(section, new List<string> { Message.FixString() });
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Add", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\Desc.cs", 25);
			}
		}
	}

	public List<string> Get(DescSection section)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\Desc.cs"))
		{
			try
			{
				if (Data == null)
				{
					return null;
				}
				Data.TryGetValue(section, out var value);
				return value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\Desc.cs", 39);
				return null;
			}
		}
	}
}
