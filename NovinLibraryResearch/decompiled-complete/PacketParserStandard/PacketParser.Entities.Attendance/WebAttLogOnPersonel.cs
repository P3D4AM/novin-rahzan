using System;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttLogOnPersonel : ILogOnPersonel
{
	public int Id { get; set; }

	public string FileContents { get; }

	public DateTime DateTimeLogM { get; set; }

	public long TrafficId { get; set; }

	public int PersonNo { get; set; }

	public int PersonId { get; set; }

	public string Message { get; }

	public int Duration { get; set; }

	public SavedTaradodEnumType IsVirtual { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; }

	public TypeTaradodEnumType TypeTaradodVacationOrMission { get; set; }

	public string SmsRefrenceIDs { get; set; }

	public string ContextGuid { get; set; }

	public int DeviceId_ { get; set; }

	public string DeviceSerial_ { get; set; }

	Guid ILogOnPersonel.Guid { get; set; }
}
