using System.ComponentModel.DataAnnotations;

namespace PacketParser.DepartmentApiModel;

public class AdminLoginModel
{
	[Required]
	[DataType(DataType.Text)]
	public string Username { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[DataType(DataType.Text)]
	public string AdminUsername { get; set; }

	[DataType(DataType.Password)]
	public string AdminPassword { get; set; }
}
