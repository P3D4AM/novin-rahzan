using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class Send2FriendBusiness : Serializable<Send2FriendBusiness>, ISend2Friend, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _email;

	private string _family;

	private string _ip;

	private string _message;

	private string _mob;

	private string _name;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid CustomerGuid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Email
	{
		get
		{
			return _email;
		}
		set
		{
			_email = value.FixString();
		}
	}

	public string Family
	{
		get
		{
			return _family;
		}
		set
		{
			_family = value.FixString();
		}
	}

	public short Gender { get; set; }

	public string Ip
	{
		get
		{
			return _ip;
		}
		set
		{
			_ip = value.FixString();
		}
	}

	public string Message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value.FixString();
		}
	}

	public string Mob
	{
		get
		{
			return _mob;
		}
		set
		{
			_mob = value.FixString();
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value.FixString();
		}
	}

	public long SmsId { get; set; }

	public short SmsSt { get; set; }

	public int Type { get; set; }

	public string ContextGuid { get; set; }

	public Send2FriendBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
