using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

public class EvaluationContext<T> : DerivedEvaluationContext
{
	public IDictionary<string, T> Data { get; }

	public override ConcurrentSet<Expression> CallStack { get; } = new ConcurrentSet<Expression>(ReferenceComparer<Expression>.Instance);


	public EvaluationContext(string name)
		: base(name)
	{
		Data = InitData();
	}

	public EvaluationContext(string name, IDictionary<string, T> data)
		: base(name)
	{
		Data = data;
	}

	public EvaluationContext(string name, IEvaluationContext baseContext)
		: base(name, baseContext)
	{
		Data = InitData();
	}

	public EvaluationContext(string name, IEvaluationContext baseContext, IDictionary<string, T> data)
		: base(name, baseContext)
	{
		Data = data;
	}

	protected override bool TryGetDataCore(string column, out object value)
	{
		if (Data.TryGetValue(column, out var value2))
		{
			value = ToObject(value2);
			return true;
		}
		value = null;
		return false;
	}

	protected virtual object ToObject(T value)
	{
		return value;
	}

	private static Dictionary<string, T> InitData()
	{
		return new Dictionary<string, T>();
	}
}
