namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionPoolCountersNoCounters : DbConnectionPoolCounters
{
	public static readonly DbConnectionPoolCountersNoCounters SingletonInstance = new DbConnectionPoolCountersNoCounters();

	private DbConnectionPoolCountersNoCounters()
	{
	}
}
