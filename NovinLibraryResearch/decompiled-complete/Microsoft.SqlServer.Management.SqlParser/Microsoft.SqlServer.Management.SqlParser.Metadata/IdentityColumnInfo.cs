namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class IdentityColumnInfo
{
	private readonly long seed;

	private readonly long increment;

	private readonly bool notForReplication;

	public long Increment => increment;

	public long Seed => seed;

	public bool NotForReplication => notForReplication;

	public IdentityColumnInfo(long seed, long increment)
		: this(seed, increment, notForReplication: false)
	{
	}

	public IdentityColumnInfo(long seed, long increment, bool notForReplication)
	{
		this.seed = seed;
		this.increment = increment;
		this.notForReplication = notForReplication;
	}
}
