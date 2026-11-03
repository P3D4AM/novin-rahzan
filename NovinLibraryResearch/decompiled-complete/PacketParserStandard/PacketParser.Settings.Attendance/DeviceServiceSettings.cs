using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.Settings.Attendance;

public class DeviceServiceSettings
{
	public const string KeyName = "Att_DeviceServiceSettings";

	public bool HandleService { get; set; }

	[DefaultValue("127.0.0.1")]
	public string BaseHubIp { get; set; } = "127.0.0.1";


	[DefaultValue("0.0.0.0")]
	public string SetupIp { get; set; } = "0.0.0.0";


	[DefaultValue(7594)]
	public int BaseHubPort { get; set; } = 7594;


	[JsonIgnore]
	public string Url => $"http://{BaseHubIp}:{BaseHubPort}/";

	public static DeviceServiceSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Attendance\\DeviceServiceSettings.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new InvalidOperationException("data is null or empty! we cannot deserialize this data to ScaleSettings.");
				}
				return data.FromJson<DeviceServiceSettings>(sendErrorIfException: true, "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Attendance\\DeviceServiceSettings.cs", 24);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Attendance\\DeviceServiceSettings.cs", 28);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Attendance\\DeviceServiceSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Attendance\\DeviceServiceSettings.cs", 41);
				return "";
			}
		}
	}
}
