using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class ScaleSettings
{
	public const string KeyName = "Acc_ScaleSettings";

	private string baseHubAddress = "0.0.0.0";

	private int _baseHubPort = 5000;

	public int Version { get; set; } = 1;


	[DefaultValue(true)]
	public bool Network { get; set; } = true;


	[DefaultValue(false)]
	public bool WeightAccess { get; set; }

	[Obsolete("Use From CashierSettings", true)]
	[DefaultValue(false)]
	public bool ScaleListenEnable { get; set; }

	[DefaultValue("0.0.0.0")]
	public string BaseHubAddress
	{
		get
		{
			if (!(baseHubAddress?.ToLower() == "http://127.0.0.1"))
			{
				return baseHubAddress;
			}
			return "0.0.0.0";
		}
		set
		{
			baseHubAddress = value;
		}
	}

	public string SetupIp { get; set; }

	[DefaultValue(7401)]
	public int BaseHubPort
	{
		get
		{
			if (_baseHubPort != 5000)
			{
				return _baseHubPort;
			}
			return 7401;
		}
		set
		{
			_baseHubPort = value;
		}
	}

	public string SqlServerConnectionString { get; set; }

	[DefaultValue(true)]
	public bool ScaleNewFactorAfterSave { get; set; } = true;


	public int Interval { get; set; }

	public EnPriceType DefPrice { get; set; }

	[DefaultValue(true)]
	public bool ShowScaleNotification { get; set; } = true;


	public static ScaleSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new InvalidOperationException("data is null or empty! we cannot deserialize this data to ScaleSettings.");
				}
				return data.FromJson<ScaleSettings>(sendErrorIfException: true, "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs", 75);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs", 79);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\ScaleSettings.cs", 92);
				return "";
			}
		}
	}
}
