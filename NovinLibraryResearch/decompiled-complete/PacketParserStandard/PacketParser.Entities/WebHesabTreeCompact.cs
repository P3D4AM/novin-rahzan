using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebHesabTreeCompact : Serializable<WebHesabTreeCompact>, IHesabTreeCompact, IReceiver, INpDisposable, IDisposable
{
	private string name;

	public Guid Guid { get; set; } = Guid.Empty;


	public string Name
	{
		get
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "نام حساب";
			}
			return name;
		}
		set
		{
			name = value;
		}
	}

	public decimal Account { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[DefaultValue(HesabType.All)]
	public HesabType HType { get; set; }

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebHesabTreeCompact.cs", 40);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebHesabTreeCompact.cs", 54);
			return $"Error({action})";
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}
}
