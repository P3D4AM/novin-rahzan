using System;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttLogOnPerson : ILogOnPersonel
{
	public DateTime DateTimeLogM { get; set; }

	public Guid TrafficGuid { get; set; }

	public Guid PersonGuid { get; set; }

	public int PersonNo { get; set; }

	public int DeviceId_ { get; set; }

	public string TrafficName { get; set; }

	public bool Calced { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; }

	public int Duration { get; set; }

	public DateTime DateInserted { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public long TrafficId { get; set; }

	public string SmsRefrenceIDs { get; set; }

	public string Message { get; }

	public int PersonId { get; set; }

	public int Id { get; set; }

	public string FileContents { get; }

	public string ContextGuid { get; set; }

	public SavedTaradodEnumType IsVirtual { get; set; }

	public TypeTaradodEnumType TypeTaradodVacationOrMission { get; set; }

	public string DeviceSerial_ { get; set; }
}
