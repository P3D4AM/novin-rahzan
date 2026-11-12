using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public interface IExecutionPolicy
{
	bool Expired { get; set; }

	bool ResumeExecution();

	TimeSpan BackoffInterval();
}
