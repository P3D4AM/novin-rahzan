using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class NullLiteralExpression : Expression
{
	public static NullLiteralExpression Instance { get; } = new NullLiteralExpression();


	private NullLiteralExpression()
	{
	}

	public override bool Equals(object obj)
	{
		return obj is NullLiteralExpression;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		return null;
	}

	internal override IEnumerable<string> GetNames()
	{
		yield break;
	}
}
