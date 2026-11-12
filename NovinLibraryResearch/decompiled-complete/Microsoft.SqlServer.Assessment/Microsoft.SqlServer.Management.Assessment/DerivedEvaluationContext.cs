using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

public abstract class DerivedEvaluationContext : IEvaluationContext, IDataRow
{
	public IEvaluationContext BaseContext { get; }

	public string Name { get; }

	public abstract ConcurrentSet<Expression> CallStack { get; }

	protected DerivedEvaluationContext(string name, IEvaluationContext baseContext)
		: this(name)
	{
		BaseContext = baseContext;
	}

	protected DerivedEvaluationContext(string name)
	{
		Name = name;
	}

	public bool TryGetData(string column, out object value)
	{
		if (TryGetOwnData(column, out value))
		{
			return true;
		}
		if (BaseContext != null)
		{
			return BaseContext.TryGetData(column, out value);
		}
		value = null;
		return false;
	}

	protected virtual bool TryGetOwnData(string column, out object value)
	{
		value = null;
		if (column != null && Names.Split(Name, column, out var tail))
		{
			return TryGetDataCore(tail, out value);
		}
		return false;
	}

	protected abstract bool TryGetDataCore(string column, out object value);
}
