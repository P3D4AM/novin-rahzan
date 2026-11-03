using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebBackupSetting : IBackupSettings
{
	public int Id { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public bool Startup { get; set; }

	public bool Zip { get; set; }

	public bool RememberMe { get; set; }
}
