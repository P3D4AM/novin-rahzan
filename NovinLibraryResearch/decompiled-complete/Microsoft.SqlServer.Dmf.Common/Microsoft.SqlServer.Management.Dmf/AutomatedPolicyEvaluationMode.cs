using System;

namespace Microsoft.SqlServer.Management.Dmf;

[Flags]
public enum AutomatedPolicyEvaluationMode
{
	None = 0,
	Enforce = 1,
	CheckOnChanges = 2,
	CheckOnSchedule = 4
}
