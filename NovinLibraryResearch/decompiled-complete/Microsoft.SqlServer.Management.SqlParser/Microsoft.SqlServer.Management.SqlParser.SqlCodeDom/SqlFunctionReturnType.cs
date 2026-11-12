using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlFunctionReturnType : SqlCodeObject
{
	internal abstract SqlScalarFunctionReturnType AsSqlScalarFunctionReturnType { get; }

	internal abstract SqlTableFunctionReturnType AsSqlTableFunctionReturnType { get; }

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal abstract void Bind(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion);

	internal override void RunConsistencyChecks(string path, int depth)
	{
	}
}
