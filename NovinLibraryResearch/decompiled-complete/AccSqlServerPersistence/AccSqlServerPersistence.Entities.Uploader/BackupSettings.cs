using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_BackupSettings")]
public class BackupSettings : IBackupSettings
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Username { get; set; }

	[Required]
	public string Password { get; set; }

	public bool Startup { get; set; }

	public bool Zip { get; set; }

	public bool RememberMe { get; set; }
}
