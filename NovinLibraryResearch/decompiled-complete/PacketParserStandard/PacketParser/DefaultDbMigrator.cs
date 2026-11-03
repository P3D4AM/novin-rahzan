namespace PacketParser;

public class DefaultDbMigrator : IDbMigrator
{
	public bool IsMigrationAllowed()
	{
		return true;
	}
}
