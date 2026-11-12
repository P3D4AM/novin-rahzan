using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateAlterFunctionStatementBase : SqlDdlStatement
{
	private FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> boundFunction;

	private readonly SqlFunctionDefinition definition;

	private readonly bool isOrAlterStatement;

	internal sealed override bool IsModuleStatement => !definition.IsInlineTableFunction;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Function;

	public IUserDefinedFunction BoundFunction => boundFunction.AsUserDefinedFunction;

	public override IMetadataObject BoundObject => BoundFunction;

	public SqlFunctionDefinition Definition => definition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return definition;
		}
	}

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		string value = definition.Name.SchemaName.Value;
		string value2 = definition.Name.ObjectName.Value;
		LexicalScope execScope = GetLexicalScope();
		SchemaEx cDASchema = bindingContext.GetCDASchema(value, execScope);
		IUserDefinedFunctionFactory userDefinedFunction = bindingContext.MetadataFactory.UserDefinedFunction;
		boundFunction = definition.CreateFunction(userDefinedFunction, cDASchema.ProxyObject);
		FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction> existingFunction = FindFunctionMetadataObject(bindingContext, value, value2, execScope);
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && existingFunction.IsNull))
			{
				if (cDASchema.IsEmptySchema)
				{
					if (value != null)
					{
						definition.Name.SchemaName.ReportBindError(SR.SchemaNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
					}
					base.BindingState |= BindingState.BindFailed;
				}
				else if (!existingFunction.IsNull)
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(value2));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					ExecuteCreate(cDASchema, boundFunction, this, bindMode);
					base.BindingState |= BindingState.BindSucceeded;
				}
			}
			else
			{
				DoExecuteAlter(existingFunction, this, bindingContext, execScope);
			}
		}
		else if (!existingFunction.IsNull)
		{
			DoExecuteAlter(existingFunction, this, bindingContext, execScope);
		}
		else
		{
			if (BatchBindingContext.ObjectExistsInSchema(cDASchema, value2, execScope))
			{
				definition.Name.ReportBindError(SR.CannotPerformAlterOnObject(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			else
			{
				definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		SchemaEx schema = bindingContext.SchemaToSchemaEx(boundFunction.AsUserDefinedFunction.Schema);
		BatchBindingContext orCreateModuleBindingContext = bindingContext.GetOrCreateModuleBindingContext(schema);
		bool setTextProperties = bindMode == StatementBindMode.OutOfOrder;
		definition.Bind(orCreateModuleBindingContext, boundFunction, setTextProperties);
		base.BindingState |= definition.BindingState;
	}

	internal override void Unbind()
	{
		boundFunction = default(FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction>);
		base.Unbind();
	}

	private static FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction> FindFunctionMetadataObject(BatchBindingContext bindingContext, string schemaName, string functionName, LexicalScope lexicalScope)
	{
		IScalarValuedFunction scalarValuedFunction;
		if ((scalarValuedFunction = bindingContext.FindScalarValuedFunction(null, schemaName, functionName, lexicalScope)) != null)
		{
			return new FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction>(scalarValuedFunction);
		}
		ITableValuedFunction tableValuedFunction;
		if ((tableValuedFunction = bindingContext.FindTableValuedFunction(null, schemaName, functionName, lexicalScope)) != null)
		{
			return new FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction>(tableValuedFunction);
		}
		return default(FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction>);
	}

	private static void ExecuteCreate(SchemaEx schema, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> function, IBindFinalizer finalizer, StatementBindMode bindMode)
	{
		if (function.IsScalarValuedFunction)
		{
			schema.ScalarValuedFunctions.Create(function.ScalarValuedFunction, finalizer, bindMode);
		}
		else
		{
			schema.TableValuedFunctions.Create(function.TableValuedFunction, finalizer, bindMode);
		}
	}

	private static void ExecuteAlter(SchemaEx schema, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> function, LexicalScope lexicalScope)
	{
		if (function.IsScalarValuedFunction)
		{
			schema.ScalarValuedFunctions.Alter(function.ScalarValuedFunction, lexicalScope);
		}
		else
		{
			schema.TableValuedFunctions.Alter(function.TableValuedFunction, lexicalScope);
		}
	}

	private static bool CanExecuteAlter(FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction> existingFunction, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> newFunction)
	{
		bool flag = ((existingFunction.IsTableValuedFunction && newFunction.IsTableValuedFunction) ? (existingFunction.TableValuedFunction.IsInline == newFunction.TableValuedFunction.IsInline) : ((existingFunction.IsScalarValuedFunction && newFunction.IsScalarValuedFunction) ? true : false));
		if (flag)
		{
			flag = existingFunction.AsUserDefinedFunction.IsSqlClr == newFunction.AsUserDefinedFunction.IsSqlClr;
		}
		return flag;
	}

	private static void DoExecuteAlter(FunctionUnion<IScalarValuedFunction, ITableValuedFunction, IUserDefinedFunction> existingFunction, SqlCreateAlterFunctionStatementBase sqlCreateAlterFunctionStatementBase, BatchBindingContext bindingContext, LexicalScope execScope)
	{
		if (CanExecuteAlter(existingFunction, sqlCreateAlterFunctionStatementBase.boundFunction))
		{
			ExecuteAlter(bindingContext.SchemaToSchemaEx(existingFunction.AsUserDefinedFunction.Schema), sqlCreateAlterFunctionStatementBase.boundFunction, execScope);
			sqlCreateAlterFunctionStatementBase.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			sqlCreateAlterFunctionStatementBase.definition.Name.ReportBindError(SR.CannotPerformAlterOnObject(sqlCreateAlterFunctionStatementBase.definition.Name.ToString()));
			sqlCreateAlterFunctionStatementBase.BindingState |= BindingState.BindFailed;
		}
	}

	internal SqlCreateAlterFunctionStatementBase(SqlFunctionDefinition definition)
	{
		this.definition = definition;
	}

	internal SqlCreateAlterFunctionStatementBase(SqlFunctionDefinition definition, bool isOrAlterStatement)
	{
		this.definition = definition;
		this.isOrAlterStatement = isOrAlterStatement;
	}
}
