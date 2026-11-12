using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IAssessmentRequest : IDataRow, ITargetedObject
{
	IEnumerable<Check> Checks { get; }

	EvaluationContext<object> EvaluationContext { get; }
}
