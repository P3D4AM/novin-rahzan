using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal struct FunctionUnion<S, T, U> where S : class, IScalarValuedFunction, U where T : class, ITableValuedFunction, U where U : class, IUserDefinedFunction
{
	public readonly S ScalarValuedFunction;

	public readonly T TableValuedFunction;

	public bool IsNull
	{
		get
		{
			if (ScalarValuedFunction == null)
			{
				return TableValuedFunction == null;
			}
			return false;
		}
	}

	public bool IsScalarValuedFunction => ScalarValuedFunction != null;

	public bool IsTableValuedFunction => TableValuedFunction != null;

	public U AsUserDefinedFunction
	{
		get
		{
			if (ScalarValuedFunction != null)
			{
				return (U)ScalarValuedFunction;
			}
			return (U)TableValuedFunction;
		}
	}

	public FunctionUnion(S scalarValuedFunction)
	{
		ScalarValuedFunction = scalarValuedFunction;
		TableValuedFunction = null;
	}

	public FunctionUnion(T tableValuedFunction)
	{
		ScalarValuedFunction = null;
		TableValuedFunction = tableValuedFunction;
	}
}
