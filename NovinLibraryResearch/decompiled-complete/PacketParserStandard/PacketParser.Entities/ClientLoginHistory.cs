using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class ClientLoginHistory : Serializable<ClientLoginHistory>, IClientLoginHistory
{
	public DateTime? LoginDateM { get; set; }

	public string LoginDateSh => LoginDateM.MiladiToCalture();

	public DateTime? DisconnectDateM { get; set; }

	public string DisconnectDateSh => DisconnectDateM.MiladiToCalture();

	public string DisconnectDescription { get; set; }

	public string Ip { get; set; } = "fillBill";


	public long HashId { get; set; }

	public string Username { get; set; }

	public string Pass { get; set; }

	public short DepId { get; set; }

	public string CusUserName { get; set; }

	public string CusPass { get; set; }

	public Guid CusGuid { get; set; }

	public ENSource Source { get; set; }

	public string Version { get; set; }

	public string ClientVersion { get; set; }

	public string CpuSerial { get; set; }

	public long AndroidIme { get; set; }

	public Guid DbGuid { get; set; }

	public string ConnectionUniqueId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string LoginTimeStr
	{
		get
		{
			if (LoginDateM.HasValue)
			{
				return LoginDateSh + "  " + LoginDateM.Value.ToString("HH:mm:ss");
			}
			return LoginDateSh;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public string DisconnectTimeStr
	{
		get
		{
			if (DisconnectDateM.HasValue)
			{
				return DisconnectDateSh + "  " + DisconnectDateM.Value.ToString("HH:mm:ss");
			}
			return DisconnectDateSh;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public string SourceStr => Source.GetDisplay();

	public void LoadFromLoginExtention(LoginExtention eConnectionData)
	{
		try
		{
			if (eConnectionData != null)
			{
				ClientVersion = eConnectionData?.ClientVersion.ToString() ?? "";
				Version = eConnectionData?.Version ?? "";
				CpuSerial = eConnectionData?.CpuSerial ?? "";
				Username = eConnectionData?.Username ?? "";
				Pass = eConnectionData.Password ?? "";
				CusUserName = eConnectionData.CustomerUsername ?? "";
				CusPass = eConnectionData.CustomerPassword ?? "";
				Source = eConnectionData?.Source ?? ENSource.None;
				DepId = eConnectionData.UserInfo?.DepId ?? 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadFromLoginExtention", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ClientLoginHistory.cs", 69);
		}
	}
}
