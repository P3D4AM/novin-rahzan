using System;
using System.ComponentModel;

namespace PacketParser.Entities;

public class SyncLog
{
	public DateTime Date { get; set; }

	public SyncInformation SyncInfo { get; set; }

	public SyncStatus SyncStatus { get; set; } = SyncStatus.Syncing;


	public bool IsConected { get; set; }

	public string Ip { get; set; } = "";


	public DateTime D
	{
		get
		{
			return Date;
		}
		set
		{
			Date = value;
		}
	}

	public SyncInformation SI
	{
		get
		{
			return SyncInfo;
		}
		set
		{
			SyncInfo = value;
		}
	}

	[DefaultValue(SyncStatus.Syncing)]
	public SyncStatus SC
	{
		get
		{
			return SyncStatus;
		}
		set
		{
			SyncStatus = value;
		}
	}

	public bool IC
	{
		get
		{
			return IsConected;
		}
		set
		{
			IsConected = value;
		}
	}
}
