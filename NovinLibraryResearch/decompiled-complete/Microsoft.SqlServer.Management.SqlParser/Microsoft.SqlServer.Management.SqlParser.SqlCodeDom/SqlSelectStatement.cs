using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectStatement : SqlStatement, IBindFinalizer
{
	private IMutableTable boundTable;

	private readonly SqlQueryWithClause queryWithClause;

	private readonly SqlSelectSpecification selectSpecification;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "SELECT";

	public ITable BoundTable => boundTable;

	public override IMetadataObject BoundObject => boundTable;

	bool IBindFinalizer.IsFinalizing => false;

	bool IBindFinalizer.IsFinalized => true;

	LexicalScope IBindFinalizer.LexicalScope => GetLexicalScope();

	public SqlQueryWithClause QueryWithClause => queryWithClause;

	public SqlSelectSpecification SelectSpecification => selectSpecification;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (queryWithClause != null)
			{
				yield return queryWithClause;
			}
			yield return selectSpecification;
		}
	}

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		IBindingContext bindingContext2 = bindingContext;
		if (queryWithClause != null)
		{
			queryWithClause.Bind(bindingContext2);
			base.BindingState |= queryWithClause.BindingState;
			if (queryWithClause.CTEBoundContext != null)
			{
				bindingContext2 = queryWithClause.CTEBoundContext;
			}
		}
		selectSpecification.Bind(bindingContext2);
		base.BindingState |= selectSpecification.BindingState;
		SqlSelectIntoClause intoClause = GetIntoClause();
		if (intoClause != null)
		{
			BindIntoClause(bindingContext, intoClause, bindMode);
		}
	}

	internal SqlSelectIntoClause GetIntoClause()
	{
		return selectSpecification.QueryExpression.GetIntoClause();
	}

	private void BindIntoClause(BatchBindingContext bindingContext, SqlSelectIntoClause intoClause, StatementBindMode bindMode)
	{
		string value = intoClause.IntoTarget.DatabaseName.Value;
		string value2 = intoClause.IntoTarget.SchemaName.Value;
		string value3 = intoClause.IntoTarget.ObjectName.Value;
		LexicalScope lexicalScope = GetLexicalScope();
		SchemaEx cDASchema = bindingContext.GetCDASchema(value, value2, lexicalScope);
		if (cDASchema == null)
		{
			intoClause.IntoTarget.DatabaseName.ReportBindError(SR.DatabaseNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		boundTable = bindingContext.MetadataFactory.Tabular.CreateTable(cDASchema.ProxyObject, value3);
		if (cDASchema.IsEmptySchema)
		{
			if (value2 != null)
			{
				intoClause.IntoTarget.SchemaName.ReportBindError(SR.SchemaNotExist(value2), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			base.BindingState |= BindingState.BindFailed;
		}
		else if (BatchBindingContext.ObjectExistsInSchema(cDASchema, value3, lexicalScope))
		{
			intoClause.IntoTarget.ReportBindError(SR.DatabaseObjectExist(value3));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			cDASchema.Tables.Create(boundTable, this, bindMode);
			base.BindingState |= BindingState.BindSucceeded;
		}
		BindUtils.Table.PopulateTabularColumns(intoClause.IntoTarget.ObjectName, null, selectSpecification.QueryExpression, bindingContext, boundTable, BindUtils.Table.ColumnAliasOption.ScalarAlias);
	}

	internal override void Unbind()
	{
		boundTable = null;
		base.Unbind();
	}

	void IBindFinalizer.FinalizeBind()
	{
	}

	void IBindFinalizer.PostBindFinalize()
	{
	}

	internal SqlSelectStatement(SqlSelectSpecification selectSpecification)
		: this(null, selectSpecification)
	{
	}

	internal SqlSelectStatement(SqlQueryWithClause queryWithClause, SqlSelectSpecification selectSpecification)
	{
		this.selectSpecification = selectSpecification;
		this.queryWithClause = queryWithClause;
		this.selectSpecification.QueryExpression.Validate(QueryExpressionValidateOption.IntoClauseAllowLeftmost);
	}
}
