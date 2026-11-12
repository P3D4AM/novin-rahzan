using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class ExpressionByReferenceComparer : IEqualityComparer<Expression>
{
	public static ExpressionByReferenceComparer Instance { get; } = new ExpressionByReferenceComparer();


	public bool Equals(Expression x, Expression y)
	{
		return x == y;
	}

	public int GetHashCode(Expression obj)
	{
		return RuntimeHelpers.GetHashCode(obj);
	}
}
