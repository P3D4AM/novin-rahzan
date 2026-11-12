using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlFunctionBodyDefinition : SqlCodeObject
{
	internal abstract SqlClrFunctionBodyDefinition AsSqlClrFunctionBodyDefinition { get; }

	internal abstract SqlInlineFunctionBodyDefinition AsSqlInlineFunctionBodyDefinition { get; }

	internal abstract SqlMultistatementFunctionBodyDefinition AsSqlMultistatementFunctionBodyDefinition { get; }

	internal sealed override bool IsStatementLevelObject => true;

	internal sealed override void RegisterDatabaseContextChange(DatabaseContext databaseContext)
	{
	}

	internal sealed override DatabaseContext GetDatabaseContext(LexicalScope lexScope)
	{
		return null;
	}

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal abstract void Bind(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion);

	internal override void RunConsistencyChecks(string path, int depth)
	{
		int num = 0;
		if (AsSqlClrFunctionBodyDefinition != null)
		{
			num++;
		}
		if (AsSqlInlineFunctionBodyDefinition != null)
		{
			num++;
		}
		if (AsSqlMultistatementFunctionBodyDefinition != null)
		{
			num++;
		}
	}
}
