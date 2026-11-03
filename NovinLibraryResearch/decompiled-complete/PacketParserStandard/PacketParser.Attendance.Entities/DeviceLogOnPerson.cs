using System;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.Entities;

public class DeviceLogOnPerson : IHasJustGuid, IHasContext
{
	private DateTime? _fixedDateM;

	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public string DeviceIp { get; set; }

	public string DeviceSerial { get; set; }

	public string DeviceModel { get; set; }

	public string DevicePassword { get; set; }

	public string InOut { get; set; }

	public string VerifyMode { get; set; }

	public string LogPhoto { get; set; }

	public int DeviceNo { get; set; }

	public int DevicePort { get; set; }

	public int IoMode { get; set; }

	public DeviceModelEnumType TerminalType { get; set; }

	public DateTime DateTimeLogM { get; set; }

	public long IdTraffic { get; set; }

	public int PersonNo { get; set; }

	public DateTime FixedDateM
	{
		get
		{
			if (!_fixedDateM.HasValue)
			{
				_fixedDateM = DateTime.Parse(DateTimeLogM.ToString("yyyy/MM/dd HH:mm"));
			}
			return _fixedDateM.Value;
		}
	}

	public DeviceLogOnPerson(string contextGuid)
	{
		ContextGuid = contextGuid;
		Guid = Guid.NewGuid();
	}
}
