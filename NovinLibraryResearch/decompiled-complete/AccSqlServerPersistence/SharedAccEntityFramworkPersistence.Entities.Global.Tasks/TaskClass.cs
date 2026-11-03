using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

[Table("Global_Task")]
public class TaskClass : ITask, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[Required]
	[MaxLength(500)]
	public string? Title { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public int StatusId { get; set; }

	public Guid? MasterGuid { get; set; }

	public int Id2 { get; set; }

	public string? Description { get; set; }

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

	public string? MetaData { get; set; }

	public bool Status { get; set; }

	public TaskClass Tasks { get; set; }

	public ICollection<TaskCashierRelation> TaskCashierRelation { get; set; }

	public ICollection<TaskClass> TaskClasses { get; set; }
}
