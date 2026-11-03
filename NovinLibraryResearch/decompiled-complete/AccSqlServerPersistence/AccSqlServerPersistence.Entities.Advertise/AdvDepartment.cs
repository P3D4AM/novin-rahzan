using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Settings.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_Department")]
public class AdvDepartment : IAdvDepartment, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[MaxLength(250)]
	public string? Title { get; set; }

	[JsonIgnore]
	[NotMapped]
	public AdvDepartmentChatList ChatList { get; set; }

	public string? ChatData { get; set; }

	public bool IsSendChat { get; set; }

	public bool Status { get; set; }

	public string? Description { get; set; }

	public int ChatCount { get; set; }

	[JsonIgnore]
	[NotMapped]
	public AdvDepartmentChatCity ChatCity { get; set; }

	public string? ChatCityData { get; set; }
}
