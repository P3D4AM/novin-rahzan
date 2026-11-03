using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_UserInformation")]
public class UserInformation
{
	[Key]
	public string Username { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	public string UserData { get; set; }
}
