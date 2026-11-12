namespace Azure;

public class MatchConditions
{
	public ETag? IfMatch { get; set; }

	public ETag? IfNoneMatch { get; set; }
}
