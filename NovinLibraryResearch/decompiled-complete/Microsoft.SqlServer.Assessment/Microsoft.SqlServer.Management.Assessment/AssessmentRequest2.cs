using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

internal class AssessmentRequest2 : IAssessmentRequest, IDataRow, ITargetedObject
{
	public IEnumerable<Check> Checks { get; set; }

	public string[] CheckSelector { get; }

	public Func<ICheck, bool> Predicate { get; }

	public EvaluationContext<object> EvaluationContext { get; }

	public ISqlObjectLocator Target { get; }

	public bool IsDryRun { get; }

	public AssessmentRequest2(ISqlObjectLocator locator, EvaluationContext<object> baseContext, string[] checkSelector = null, Func<ICheck, bool> predicate = null, bool isDryRun = false)
	{
		Target = locator ?? throw new ArgumentNullException("locator");
		CheckSelector = checkSelector;
		Predicate = predicate;
		IsDryRun = isDryRun;
		EvaluationContext = new EvaluationContext<object>("attr", baseContext);
	}

	public bool TryGetData(string column, out object value)
	{
		return EvaluationContext.TryGetData(column, out value);
	}
}
