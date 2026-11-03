using System;
using PacketParser.Entities;

namespace PacketParser.Assistance.Net;

public class TimeLogEventLog
{
	private int month1 = 1;

	private int _year = 2024;

	private int second1;

	private int minute1;

	private int hour1;

	private int day1 = 1;

	public int termId { get; set; }

	public string serialNumber { get; set; }

	public string DeviceName { get; set; }

	public string termType { get; set; }

	public string ClientIp { get; set; }

	public int ClientPort { get; set; }

	public long transId { get; set; }

	public int year
	{
		get
		{
			return _year;
		}
		set
		{
			if (value <= 2000 || value > 3000)
			{
				WebErrorLog.Instance.StartLog("received data from ebio with erro on year!\r\n" + $"year=>{value}", "", "year", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 55);
				_year = 2000;
			}
			else
			{
				_year = value;
			}
		}
	}

	public int month
	{
		get
		{
			return month1;
		}
		set
		{
			if (value <= 0 || value > 12)
			{
				throw new ArgumentOutOfRangeException($"month=>{value}");
			}
			month1 = value;
		}
	}

	public int day
	{
		get
		{
			return day1;
		}
		set
		{
			if (value < 0 || value > 31)
			{
				throw new ArgumentOutOfRangeException($"day=>{value}");
			}
			day1 = value;
		}
	}

	public int hour
	{
		get
		{
			return hour1;
		}
		set
		{
			if (value < 0 || value > 23)
			{
				throw new ArgumentOutOfRangeException($"hour=>{value}");
			}
			hour1 = value;
		}
	}

	public int minute
	{
		get
		{
			return minute1;
		}
		set
		{
			if (value < 0 || value > 59)
			{
				throw new ArgumentOutOfRangeException($"min=>{value}");
			}
			minute1 = value;
		}
	}

	public int second
	{
		get
		{
			return second1;
		}
		set
		{
			if (value < 0 || value > 59)
			{
				throw new ArgumentOutOfRangeException($"second=>{value}");
			}
			second1 = value;
		}
	}

	public long userID { get; set; }

	public int doorID { get; set; }

	public string attendStatus { get; set; }

	public string verifyMode { get; set; }

	public int jobCode { get; set; }

	public string antipassStatus { get; set; }

	public string ActionString { get; set; }

	public byte[] photo { get; set; }

	public DateTime TimeLog => new DateTime(year, month, day, hour, minute, second);
}
