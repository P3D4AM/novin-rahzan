using System;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.User;

public class WebLogUsers : IWebLogUsers, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public long Id { get; set; }

	public int UserId { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh { get; set; }

	public string Description { get; set; }

	public string ActionTypeName { get; set; }

	public string LogData { get; set; }

	public string LogDataOld { get; set; }

	public string LogDataNew { get; set; }

	public string Refrence { get; set; }

	public string UserName { get; set; }
}
