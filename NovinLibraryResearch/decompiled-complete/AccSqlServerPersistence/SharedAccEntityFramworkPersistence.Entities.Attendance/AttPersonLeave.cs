using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance;

[Table("Att_PersonLeave")]
public class AttPersonLeave : IAttPersonLeave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid PersonGuid { get; set; }

	public DateTime RequestDate { get; set; }

	public string? Reason { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public EnPersonLeaveType Type { get; set; }

	public EnPersonLeaveStatus LeaveStatus { get; set; }

	public EnPersonLeaveKind Kind { get; set; }

	public string? ManagerMessage { get; set; }

	public DateTime? ManagerDate { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EnPersonLeaveQuiddity Quiddity { get; set; }

	[NotMapped]
	bool IHasStatus.Status { get; }
}
