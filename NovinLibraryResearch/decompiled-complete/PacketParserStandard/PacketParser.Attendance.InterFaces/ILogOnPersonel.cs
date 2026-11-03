using System;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.InterFaces;

public interface ILogOnPersonel
{
	Guid Guid { get; set; }

	DateTime DateTimeLogM { get; set; }

	long TrafficId { get; set; }

	int PersonNo { get; set; }

	int DeviceId_ { get; set; }

	int Duration { get; set; }

	SavedTaradodEnumType IsVirtual { get; set; }

	TypeTaradodEnumType TypeTaradod { get; set; }

	TypeTaradodEnumType TypeTaradodVacationOrMission { get; set; }

	string SmsRefrenceIDs { get; }

	int PersonId { get; set; }

	int Id { get; set; }

	string DeviceSerial_ { get; set; }
}
