using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IEvaluationContext : IDataRow
{
	string Name { get; }

	ConcurrentSet<Expression> CallStack { get; }
}
