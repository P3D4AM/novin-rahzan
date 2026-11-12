using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

internal class AssessmentRequest : IAssessmentRequest, IDataRow, ITargetedObject
{
	private readonly Check[] checks;

	public EvaluationContext<object> EvaluationContext { get; }

	public ISqlObjectLocator Target { get; }

	public IEnumerable<Check> Checks => checks;

	public AssessmentRequest(IEvaluationContext baseEvaluationContext, ISqlObjectLocator locator, Check[] checks)
	{
		Target = locator ?? throw new ArgumentNullException("locator");
		this.checks = checks ?? throw new ArgumentNullException("checks");
		EvaluationContext = new EvaluationContext<object>("attr", baseEvaluationContext);
	}

	public bool TryGetData(string column, out object value)
	{
		return EvaluationContext.TryGetData(column, out value);
	}
}
