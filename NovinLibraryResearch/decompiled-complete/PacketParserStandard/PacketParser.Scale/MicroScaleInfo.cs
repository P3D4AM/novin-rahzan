using System;
using System.ComponentModel;
using System.Net;
using PacketParser.Entities;

namespace PacketParser.Scale;

public struct MicroScaleInfo
{
	private int _id;

	private string ip;

	public int Port;

	public int ScaleNumber;

	public ConnectionType Type;

	public int Id
	{
		get
		{
			return _id;
		}
		set
		{
			if (value != _id || value != 0)
			{
				if (value <= 0)
				{
					throw new NotImplementedException("id set to zero or less than it!");
				}
				_id = value;
			}
		}
	}

	public string Ip
	{
		get
		{
			return ip;
		}
		set
		{
			ip = value?.ToLower();
		}
	}

	[DefaultValue(1)]
	public decimal WeightZarib { get; set; }

	public MicroScaleInfo()
	{
		_id = 0;
		ip = null;
		Port = 0;
		ScaleNumber = 0;
		Type = ConnectionType.None;
		WeightZarib = 1m;
	}

	public IPAddress GetIp()
	{
		return IPAddress.Parse(Ip);
	}
}
