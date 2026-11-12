using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableRefExpression : SqlTableExpression
{
	private SqlHintCollection hints;

	private SqlObjectIdentifier objectIdentifier;

	private SqlIdentifier alias;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName
	{
		get
		{
			if (alias == null)
			{
				return new TableExpressionName(objectIdentifier);
			}
			return new TableExpressionName(alias.Value);
		}
	}

	private IEnumerable<SqlIdentifier> Identifiers
	{
		get
		{
			if (!SqlIdentifier.IsNull(objectIdentifier.DatabaseName))
			{
				yield return objectIdentifier.DatabaseName;
			}
			if (!SqlIdentifier.IsNull(objectIdentifier.SchemaName))
			{
				yield return objectIdentifier.SchemaName;
			}
			yield return objectIdentifier.ObjectName;
		}
	}

	public SqlHintCollection Hints => hints;

	public SqlObjectIdentifier ObjectIdentifier => objectIdentifier;

	public SqlIdentifier Alias => alias;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (alias != null)
			{
				if (alias.Position.Before(objectIdentifier.Position))
				{
					yield return alias;
					yield return objectIdentifier;
				}
				else
				{
					yield return objectIdentifier;
					yield return alias;
				}
			}
			else
			{
				yield return objectIdentifier;
			}
			if (hints == null)
			{
				yield break;
			}
			foreach (SqlHint hint in hints)
			{
				yield return hint;
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
		BatchBindingContext batch = bindingContext.Batch;
		ITabular tabular = null;
		string value = objectIdentifier.DatabaseName.Value;
		string value2 = objectIdentifier.SchemaName.Value;
		string value3 = objectIdentifier.ObjectName.Value;
		LexicalScope lexicalScope = GetLexicalScope();
		bool flag = value2 == null && value == null;
		if (value3.Length == 0)
		{
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		if (flag)
		{
			CteBindMode bindMode;
			CteQueryFlags queryFlags;
			ITabular tabular2 = bindingContext.FindCommonTableExpression(value3, out bindMode, out queryFlags);
			tabular = tabular2;
			if (bindMode != 0)
			{
				if ((bindMode & CteBindMode.Recursive) == CteBindMode.Recursive)
				{
					if ((queryFlags & CteQueryFlags.HasSelfReference) == CteQueryFlags.HasSelfReference)
					{
						objectIdentifier.ReportBindError(SR.RecursiveCteMemberHasMultipleRefs(value3));
					}
					else
					{
						tabular = tabular2;
						bindingContext.OnBindingToSelfCte();
					}
				}
				else if ((queryFlags & CteQueryFlags.HasUnionAll) != CteQueryFlags.HasUnionAll)
				{
					objectIdentifier.ReportBindError(SR.RecursiveCteHasNoUnionAll(value3));
				}
				else
				{
					objectIdentifier.ReportBindError(SR.NoAnchorMemberForRecursiveQuery(value3));
				}
			}
			if (tabular == null)
			{
				tabular = batch.FindDmlTable(value3);
			}
		}
		if (tabular == null)
		{
			tabular = batch.FindTableOrView(value, value2, value3, lexicalScope);
		}
		if (tabular != null)
		{
			if (!SqlIdentifier.IsNullOrEmpty(alias))
			{
				tabular = bindingContext.MetadataFactory.Tabular.CreateTableAlias(tabular, alias.Value);
				SetBoundTable(bindingContext, tabular, alias);
			}
			else
			{
				SetBoundTable(bindingContext, tabular, objectIdentifier);
			}
		}
		else
		{
			string message = ((batch.FindTableValuedFunction(value, value2, value3, lexicalScope) == null) ? SR.InvalidObjectName(objectIdentifier.ToString()) : SR.ParametersNotSuppliedForFunction(objectIdentifier.ToString()));
			objectIdentifier.ReportBindError(message, ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal override void BindDmlTarget(DmlSpecificationBindingContext bindingContext)
	{
		string value = objectIdentifier.DatabaseName.Value;
		string value2 = objectIdentifier.SchemaName.Value;
		string value3 = objectIdentifier.ObjectName.Value;
		ITabular tabular = bindingContext.SetTableTarget(value, value2, value3, GetLexicalScope());
		if (tabular == null)
		{
			base.BindingState |= BindingState.BindFailed;
			objectIdentifier.ReportBindError(SR.InvalidObjectName(objectIdentifier.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			return;
		}
		base.BindingState |= BindingState.BindSucceeded;
		if (BindUtils.Table.IsTableValuedFunction(tabular) && !bindingContext.IsTargetTableLocal)
		{
			ReportBindError(SR.ParametersNotSuppliedForFunction(objectIdentifier.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
		SetBoundDmlTarget(tabular, bindingContext.IsTargetTableAmbiguous);
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		if (FindChildContainingLocation(ref caretLocation) == objectIdentifier)
		{
			result = objectIdentifier.GetCompletionList(caretLocation, completionContext);
		}
		return result;
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject == objectIdentifier)
		{
			if (base.BoundTable != null)
			{
				result = objectIdentifier.GetQuickInfoText(cursorLocation, displayInfoProvider, base.BoundTable.Unaliased);
			}
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider);
		}
		return result;
	}

	internal SqlTableRefExpression(SqlObjectIdentifier objectIdentifier)
		: this(objectIdentifier, null, null)
	{
	}

	internal SqlTableRefExpression(SqlObjectIdentifier objectIdentifier, SqlIdentifier alias)
		: this(objectIdentifier, alias, null)
	{
	}

	internal SqlTableRefExpression(SqlObjectIdentifier objectIdentifier, List<SqlHint> hints)
		: this(objectIdentifier, null, hints)
	{
	}

	internal SqlTableRefExpression(SqlObjectIdentifier objectIdentifier, SqlIdentifier alias, List<SqlHint> hints)
	{
		this.objectIdentifier = objectIdentifier;
		this.alias = alias;
		if (hints != null)
		{
			this.hints = new SqlHintCollection(hints);
		}
	}
}
