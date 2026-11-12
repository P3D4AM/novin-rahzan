using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateAlterViewStatementBase : SqlDdlStatement
{
	private IMutableView boundView;

	private readonly bool isOrAlterStatement;

	private SqlViewDefinition definition;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.View;

	public IView BoundView => boundView;

	public override IMetadataObject BoundObject => boundView;

	public SqlViewDefinition Definition => definition;

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
		boundView = bindingContext.MetadataFactory.Tabular.CreateView(cDASchema.ProxyObject, value2);
		IView view = BindUtils.Table.AsView(bindingContext.FindTableOrView(null, value, value2, execScope));
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && view == null))
			{
				if (cDASchema.IsEmptySchema)
				{
					if (value != null)
					{
						definition.Name.SchemaName.ReportBindError(SR.SchemaNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
					}
					base.BindingState |= BindingState.BindFailed;
				}
				else if (BatchBindingContext.ObjectExistsInSchema(cDASchema, value2, execScope))
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(value2));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					cDASchema.Views.Create(boundView, this, bindMode);
					base.BindingState |= BindingState.BindSucceeded;
				}
			}
			else
			{
				DoExecuteAlter(view, this, bindingContext, execScope);
			}
		}
		else if (view != null)
		{
			DoExecuteAlter(view, this, bindingContext, execScope);
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
		bool setBuildProperties = bindMode == StatementBindMode.OutOfOrder;
		definition.Bind(bindingContext, boundView, setBuildProperties);
		base.BindingState |= definition.BindingState;
	}

	internal override void Unbind()
	{
		boundView = null;
		base.Unbind();
	}

	private static void DoExecuteAlter(IView alterView, SqlCreateAlterViewStatementBase sqlCreateAlterViewStatementBase, BatchBindingContext bindingContext, LexicalScope execScope)
	{
		bindingContext.SchemaToSchemaEx(alterView.Schema).Views.Alter(sqlCreateAlterViewStatementBase.boundView, execScope);
		sqlCreateAlterViewStatementBase.BindingState |= BindingState.BindSucceeded;
	}

	internal SqlCreateAlterViewStatementBase(SqlViewDefinition definition)
	{
		this.definition = definition;
	}

	internal SqlCreateAlterViewStatementBase(SqlViewDefinition definition, bool isOrAlterStatement)
	{
		this.definition = definition;
		this.isOrAlterStatement = isOrAlterStatement;
	}
}
