using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlTableExpression : SqlCodeObject
{
	internal struct TableExpressionName
	{
		private readonly SqlObjectIdentifier objectIdentifier;

		private readonly string name;

		public string Database
		{
			get
			{
				if (objectIdentifier == null)
				{
					return null;
				}
				return objectIdentifier.DatabaseName.Value;
			}
		}

		public string Schema
		{
			get
			{
				if (objectIdentifier == null)
				{
					return null;
				}
				return objectIdentifier.SchemaName.Value;
			}
		}

		public string Table
		{
			get
			{
				if (objectIdentifier == null)
				{
					return name;
				}
				return objectIdentifier.ObjectName.Value;
			}
		}

		public TableExpressionName(SqlObjectIdentifier objectIdentifier)
		{
			this.objectIdentifier = objectIdentifier;
			name = null;
		}

		public TableExpressionName(string name)
		{
			objectIdentifier = null;
			this.name = name;
		}

		public override string ToString()
		{
			if (objectIdentifier == null)
			{
				return name;
			}
			return objectIdentifier.ToString();
		}
	}

	private ITabular boundTable;

	public ITabular BoundTable => boundTable;

	public override IMetadataObject BoundObject => boundTable;

	internal abstract TableExpressionName ExpressionName { get; }

	internal virtual void BindDmlTarget(DmlSpecificationBindingContext bindingContext)
	{
	}

	internal override void Unbind()
	{
		boundTable = null;
		base.Unbind();
	}

	internal void SetBoundTable(IBindingContext bindingContext, ITabular table, SqlCodeObject errorObject)
	{
		ITabular tabular = bindingContext.AddLocalTableExpression(table);
		if (tabular == null)
		{
			boundTable = table;
			base.BindingState = BindingState.BindSucceeded;
			return;
		}
		bool num = !(BindUtils.Table.IsDatabaseTable(table) | BindUtils.Table.IsTableVariable(table));
		bool flag = !(BindUtils.Table.IsDatabaseTable(tabular) | BindUtils.Table.IsTableVariable(tabular));
		string text = ExpressionName.ToString();
		string message;
		if (num)
		{
			if (flag)
			{
				message = SR.CorrelationNameNotUnique(text);
			}
			else
			{
				SqlTableExpression sqlTableExpression = FindSiblingTableExpressionBoundTo(this, tabular);
				message = SR.InvalidCorrelationNameWithTable(text, sqlTableExpression.ExpressionName.ToString());
			}
		}
		else if (flag)
		{
			message = SR.InvalidCorrelationNameWithTable(tabular.Name, text);
		}
		else
		{
			SqlTableExpression sqlTableExpression2 = base.Parent.FindSiblingTableExpressionBoundTo(this, tabular);
			message = SR.InvalidCorrelationNamesInFrom(text, sqlTableExpression2.ExpressionName.ToString());
		}
		errorObject.ReportBindError(message);
		base.BindingState = BindingState.BindFailed;
	}

	internal void SetBoundCte(ITabular cte)
	{
		boundTable = cte;
	}

	internal void SetBoundDmlTarget(ITabular table, bool isAmbiguous)
	{
		boundTable = table;
		if (isAmbiguous)
		{
			ReportBindError(SR.TableIsAmbiguous(ExpressionName.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal virtual SqlTableExpression FindTableExpressionBoundTo(ITabular boundTable)
	{
		if (this.boundTable != boundTable)
		{
			return null;
		}
		return this;
	}
}
