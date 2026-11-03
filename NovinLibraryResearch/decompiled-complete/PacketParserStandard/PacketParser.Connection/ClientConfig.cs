using System;
using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;

namespace PacketParser.Connection;

public class ClientConfig : Serializable<ClientConfig>
{
	private string _customerUsername = "";

	private string _customerPassword = "";

	private string url;

	public string UserName { get; set; }

	public string Password { get; set; }

	public string CustomerUsername
	{
		get
		{
			return _customerUsername ?? "";
		}
		set
		{
			_customerUsername = value;
		}
	}

	public string CustomerPassword
	{
		get
		{
			return _customerPassword ?? "";
		}
		set
		{
			_customerPassword = value;
		}
	}

	public short ClientVersion { get; set; } = 25;


	public string Url
	{
		get
		{
			return url;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && value.Contains("//hub"))
			{
				throw new ArgumentException("url is not valid!!");
			}
			url = value;
		}
	}

	public int ReconnectInterval { get; set; } = 60;


	public long AndroidIme { get; set; }

	public ENSource Source { get; set; }

	public string CpuSerial { get; set; } = "";


	[Obsolete("لطفا از CpuSerial استفاده کنید", true)]
	public string HardSerial { get; set; } = "";


	public Guid DbGuid { get; set; } = Guid.Empty;


	public bool SyncRequested { get; set; } = true;


	public string Version { get; set; }

	public long LkSerial { get; set; }

	public ENCompressedMode CompressedMode { get; set; } = ENCompressedMode.GZip;


	[DefaultValue(EncryptionType.Custom)]
	public EncryptionType Encryption { get; set; } = EncryptionType.Custom;


	public long MilisecondsDelayBetweenSend { get; set; } = 100L;


	public string ClientContextGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public WebDepartmentEntitySummery DepartmentEntitySummery { get; set; }

	public ClientConfig Copy()
	{
		return new ClientConfig
		{
			AndroidIme = AndroidIme,
			CpuSerial = CpuSerial,
			CustomerPassword = CustomerPassword,
			CustomerUsername = CustomerUsername,
			DbGuid = DbGuid,
			Password = Password,
			ReconnectInterval = ReconnectInterval,
			Source = Source,
			Url = Url,
			UserName = UserName,
			SyncRequested = SyncRequested,
			LkSerial = LkSerial,
			Version = Version,
			CompressedMode = CompressedMode,
			Encryption = Encryption,
			ClientVersion = ClientVersion,
			DepartmentEntitySummery = DepartmentEntitySummery,
			ClientContextGuid = ClientContextGuid,
			CashierGuid = CashierGuid
		};
	}

	public LoginExtention ToLoginExtention()
	{
		return new LoginExtention
		{
			Source = Source,
			CustomerPassword = CustomerPassword,
			CustomerUsername = CustomerUsername,
			DbGuid = DbGuid,
			Password = Password,
			AndroidIme = AndroidIme,
			LkSerial = LkSerial,
			CpuSerial = CpuSerial,
			SyncRequested = SyncRequested,
			Username = UserName,
			Version = Version,
			CompressedMode = CompressedMode,
			ClientVersion = ClientVersion,
			DepartmentEntitySummery = DepartmentEntitySummery,
			ClientContextGuid = ClientContextGuid,
			CashierGuid = CashierGuid,
			Encryption = Encryption
		};
	}
}
