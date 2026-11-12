using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class MatchExpression : BinaryExpression
{
	public RegexOptions Options { get; }

	public MatchExpression()
	{
	}

	public MatchExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	public MatchExpression(params Expression[] source)
		: this((IEnumerable<Expression>)source)
	{
	}

	public MatchExpression(RegexOptions options)
	{
		Options = options;
	}

	public MatchExpression(RegexOptions options, IEnumerable<Expression> source)
		: base(source)
	{
		Options = options;
	}

	public MatchExpression(RegexOptions options, params Expression[] source)
		: this(options, (IEnumerable<Expression>)source)
	{
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		if (arg1 == null || arg2 == null)
		{
			return false;
		}
		return Regex.Match(arg1.Normalize<string>(), arg2.Normalize<string>(), Options).Success;
	}

	protected override CompositeExpression Copy()
	{
		return new MatchExpression(Options);
	}
}
