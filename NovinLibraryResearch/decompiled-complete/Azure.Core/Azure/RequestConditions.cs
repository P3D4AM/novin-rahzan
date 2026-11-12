using System;

namespace Azure;

public class RequestConditions : MatchConditions
{
	public DateTimeOffset? IfModifiedSince { get; set; }

	public DateTimeOffset? IfUnmodifiedSince { get; set; }
}
