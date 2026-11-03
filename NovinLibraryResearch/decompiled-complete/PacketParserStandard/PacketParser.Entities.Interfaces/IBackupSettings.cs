namespace PacketParser.Entities.Interfaces;

public interface IBackupSettings
{
	int Id { get; set; }

	string Username { get; set; }

	string Password { get; set; }

	bool Startup { get; set; }

	bool Zip { get; set; }

	bool RememberMe { get; set; }
}
