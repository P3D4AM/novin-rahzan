using System.ComponentModel.DataAnnotations;

namespace PacketParser.DepartmentApiModel;

public class LkActivationCodeModel
{
	[Required]
	[MaxLength(10)]
	public string SerialSoft { get; set; }

	[Required]
	public long LkSerial { get; set; }

	[Required]
	[MaxLength(1)]
	public string UserName { get; set; }

	[Required]
	[MaxLength(1)]
	public string Password { get; set; }
}
