using System;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

[Serializable]
public sealed class SqlRetryLogicOption
{
	public int NumberOfTries { get; set; }

	public TimeSpan DeltaTime { get; set; }

	public TimeSpan MinTimeInterval { get; set; } = TimeSpan.Zero;


	public TimeSpan MaxTimeInterval { get; set; }

	public IEnumerable<int> TransientErrors { get; set; }

	public Predicate<string> AuthorizedSqlCondition { get; set; }
}
