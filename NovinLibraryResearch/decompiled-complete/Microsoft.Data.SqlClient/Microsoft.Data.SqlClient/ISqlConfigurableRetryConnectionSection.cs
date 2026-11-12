using System;

namespace Microsoft.Data.SqlClient;

internal interface ISqlConfigurableRetryConnectionSection
{
	TimeSpan DeltaTime { get; set; }

	TimeSpan MaxTimeInterval { get; set; }

	TimeSpan MinTimeInterval { get; set; }

	int NumberOfTries { get; set; }

	string RetryLogicType { get; set; }

	string RetryMethod { get; set; }

	string TransientErrors { get; set; }
}
