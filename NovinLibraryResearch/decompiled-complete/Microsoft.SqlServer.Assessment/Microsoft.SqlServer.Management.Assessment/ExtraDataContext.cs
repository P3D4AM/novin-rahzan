using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ExtraDataContext : DerivedEvaluationContext
{
	public IDataView ExtraData { get; }

	public int Index { get; }

	public override ConcurrentSet<Expression> CallStack => base.BaseContext?.CallStack;

	public ExtraDataContext(string name, IEvaluationContext baseContext, IDataView extraData, int index)
		: base(name, baseContext)
	{
		ExtraData = extraData;
		Index = index;
	}

	protected override bool TryGetDataCore(string column, out object value)
	{
		return ExtraData[Index].TryGetData(column, out value);
	}
}
