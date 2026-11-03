using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Connection;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class NPSMS_WebServiceSettings
{
	public const string KeyName = "AccNPSMS_WebServiceSettings";

	private List<string> signalRUrlList;

	[DefaultValue(EncryptionType.None)]
	public EncryptionType EncryptionType { get; set; }

	public string NPACCUserName { get; set; }

	public string NPACCPassword { get; set; }

	[DefaultValue(20)]
	public int NPACC_GardeshCount { get; set; } = 20;


	[Obsolete("moved to WebUser", true)]
	public decimal AndroidMinPishFactorSum { get; set; }

	public bool SendProductToScaleImmediately { get; set; }

	[DefaultValue(false)]
	public bool SendUnlimitedGardesh { get; set; }

	public List<string> SignalRUrlList
	{
		get
		{
			return signalRUrlList;
		}
		set
		{
			signalRUrlList = value;
		}
	}

	[DefaultValue(true)]
	public bool ShowSignalRNotif { get; set; } = true;


	public Guid? DefaultBankGuid { get; set; }

	public int OperationEntitiesSendToServerSeconds { get; set; } = 20;


	public static NPSMS_WebServiceSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs"))
		{
			try
			{
				return data.FromJson<NPSMS_WebServiceSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs", 33);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "data:" + data, "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs", 37);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs", 50);
				throw;
			}
		}
	}

	public IEnumerable<ClientConfig> GetConnectionConfigs(ENSource source, Guid dbGuid, long ime, long lkSerial, string appVersion, Guid cashierGuid, string clientContextGuid, string cpuSerial = "", string hardSerial = "", ENCompressedMode compressedMode = ENCompressedMode.GZip, EncryptionType encryption = EncryptionType.None)
	{
		List<ClientConfig> list = new List<ClientConfig>();
		try
		{
			List<string> list2 = SignalRUrlList;
			if (list2 == null || !list2.Any())
			{
				SignalRUrlList = new List<string> { "http://www.NovinRank.ir:8040/hubCore" };
			}
			foreach (string signalRUrl in SignalRUrlList)
			{
				list.Add(new ClientConfig
				{
					AndroidIme = ime,
					CompressedMode = compressedMode,
					CpuSerial = cpuSerial,
					CustomerPassword = "",
					CustomerUsername = "",
					DbGuid = dbGuid,
					Encryption = encryption,
					LkSerial = lkSerial,
					Password = NPACCPassword,
					UserName = NPACCUserName,
					Version = appVersion,
					Url = signalRUrl,
					SyncRequested = true,
					Source = source,
					CashierGuid = cashierGuid,
					ClientContextGuid = clientContextGuid
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetConnectionConfigs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\NPSMS_WebServiceSettings.cs", 88);
		}
		return list;
	}
}
