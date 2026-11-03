using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.InterFaces;

public interface IWebLogUsers : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long Id { get; set; }

	int UserId { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; set; }

	string Description { get; set; }

	string ActionTypeName { get; set; }

	string LogData { get; set; }

	string LogDataOld { get; set; }

	string LogDataNew { get; set; }

	string Refrence { get; set; }

	string UserName { get; set; }
}
