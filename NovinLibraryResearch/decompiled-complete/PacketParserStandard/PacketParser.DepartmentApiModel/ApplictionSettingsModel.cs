using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class ApplictionSettingsModel
{
	[Required]
	public string UserName { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	public List<WebApplicationSetting> List { get; set; }
}
