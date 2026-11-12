using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInlineTableVariableDeclareStatement : SqlDeclareStatement
{
	private readonly SqlInlineTableVariableDeclaration declaration;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DECLARE TABLE";

	public SqlInlineTableVariableDeclaration Declaration => declaration;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return declaration;
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
		declaration.Bind(bindingContext);
		base.BindingState = declaration.BindingState;
		if (declaration.BoundTable != null)
		{
			LexicalScope lexicalScope = GetLexicalScope();
			if (!bindingContext.AddLocalTableVariable(declaration.BoundTable, lexicalScope))
			{
				declaration.ReportBindError(SR.VariableNameNotUnique(declaration.Name));
			}
		}
	}

	internal SqlInlineTableVariableDeclareStatement(SqlInlineTableVariableDeclaration declaration)
	{
		this.declaration = declaration;
	}
}
