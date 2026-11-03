using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.DepartmentApiModel;

public class LkModel
{
	[Required]
	public List<WebLkInfo> Lk { get; set; }

	[Required]
	public string UserName { get; set; } = "";


	[Required]
	public string Password { get; set; } = "";


	public List<Guid> TafsilGuid { get; set; }

	[DataType(DataType.DateTime)]
	public DateTime? BeginExpireDate { get; set; }

	public DateTime? EndExpireDate { get; set; }

	public string LkSearial { get; set; } = "";


	public bool? IsOffline { get; set; }

	public bool IsAccounting { get; set; }

	public bool IsAttendance { get; set; }

	public ENCode LevelLkAccounting { get; set; }

	public TypeHozor LevelLkAttebdance { get; set; }

	public StatusLock StatusLock { get; set; }
}
