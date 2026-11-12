using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableVariableRefExpression : SqlTableExpression
{
	private readonly string name;

	private readonly SqlIdentifier alias;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName((alias != null) ? alias.Value : name);

	public string Name => name;

	public SqlIdentifier Alias => alias;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (alias != null)
			{
				yield return alias;
			}
		}
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

	internal override void Bind(IBindingContext bindingContext)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		ITabular tabular = bindingContext.FindLocalTableVariable(name, lexicalScope);
		if (tabular != null)
		{
			if (!SqlIdentifier.IsNullOrEmpty(alias))
			{
				tabular = bindingContext.MetadataFactory.Tabular.CreateTableAlias(tabular, alias.Value);
				SetBoundTable(bindingContext, tabular, alias);
			}
			else
			{
				SetBoundTable(bindingContext, tabular, this);
			}
		}
		else
		{
			ReportBindError(SR.TableVariableRequired(name), ErrorFlags.UnresolvedReference);
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal override void BindDmlTarget(DmlSpecificationBindingContext bindingContext)
	{
		ITableVariable tableVariable = bindingContext.SetTableVariableTarget(name, GetLexicalScope());
		if (tableVariable == null)
		{
			base.BindingState |= BindingState.BindFailed;
			ReportBindError(SR.TableVariableRequired(name), ErrorFlags.UnresolvedReference);
		}
		else
		{
			base.BindingState |= BindingState.BindSucceeded;
			SetBoundDmlTarget(tableVariable, bindingContext.IsTargetTableAmbiguous);
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		FindChildContainingLocation(ref caretLocation);
		if (AtFirstToken(caretLocation))
		{
			result = RetrieveCompletionListItems(completionContext);
		}
		return result;
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		if (base.BoundTable != null && AtFirstToken(cursorLocation))
		{
			result = displayInfoProvider.GetDescription(base.BoundTable.Unaliased);
		}
		return result;
	}

	private bool AtFirstToken(LexLocation lexLocation)
	{
		if (lexLocation.startTokenNumber == base.Position.startTokenNumber)
		{
			return lexLocation.endTokenNumber == lexLocation.startTokenNumber + 1;
		}
		return false;
	}

	internal SqlTableVariableRefExpression(string name)
		: this(name, null)
	{
	}

	internal SqlTableVariableRefExpression(string name, SqlIdentifier alias)
	{
		this.name = name;
		this.alias = alias;
	}
}
