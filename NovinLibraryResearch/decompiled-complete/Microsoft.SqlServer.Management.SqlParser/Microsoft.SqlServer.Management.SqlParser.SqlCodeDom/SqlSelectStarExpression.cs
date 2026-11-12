using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectStarExpression : SqlSelectExpression
{
	private ITabular boundTable;

	private IMetadataCollection<IColumn> boundColumns;

	private IMetadataCollection<ITabular> boundTables;

	private readonly SqlObjectIdentifier qualifier;

	internal override bool IsVisitable => true;

	public IMetadataCollection<IColumn> BoundColumns => boundColumns;

	public IMetadataCollection<ITabular> BoundTables => boundTables;

	internal override SelectExpressionType ExpressionType => SelectExpressionType.Star;

	public SqlObjectIdentifier Qualifier => qualifier;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (qualifier != null)
			{
				yield return qualifier;
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
		boundTables = bindingContext.LocalTableExpressions;
		if (qualifier != null)
		{
			boundTable = bindingContext.FindLocalTableExpression(qualifier.DatabaseName.Value, qualifier.SchemaName.Value, qualifier.ObjectName.Value);
			if (boundTable != null)
			{
				boundColumns = boundTable.Columns;
				base.BindingState = BindingState.BindSucceeded;
			}
			else
			{
				if (!HasAnyBindSkipped)
				{
					qualifier.ReportBindError(SR.ColumnPrefixMismatch(qualifier.ToString()));
				}
				base.BindingState = BindingState.BindFailed;
			}
		}
		else
		{
			foreach (ITabular localTableExpression in bindingContext.LocalTableExpressions)
			{
				boundColumns = Collection<IColumn>.Merge(allowDuplicates: true, boundColumns, localTableExpression.Columns);
			}
			base.BindingState = ((boundColumns != null) ? BindingState.BindSucceeded : BindingState.BindFailed);
		}
		if (boundColumns == null)
		{
			return;
		}
		foreach (IColumn boundColumn in boundColumns)
		{
			VerifyBoundColumn(boundColumn);
			bindingContext.AddLocalColumnExpression(boundColumn);
		}
	}

	internal override void Unbind()
	{
		boundColumns = null;
		boundTable = null;
		boundTables = null;
		base.Unbind();
	}

	private void VerifyBoundColumn(IColumn boundColumn)
	{
		if (boundColumn.DataType != null && boundColumn.DataType.IsXml && ((IXmlDataType)boundColumn.DataType).IsXmlNode)
		{
			ReportBindError(SR.InvalidColumnXmlNodeUse(boundColumn.Name));
		}
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		if (boundColumns != null)
		{
			result = ((qualifier == null) ? displayInfoProvider.CollectionToString(boundColumns, singleLine: false) : ((FindChildContainingLocation(ref cursorLocation) != qualifier) ? displayInfoProvider.CollectionToString(boundColumns, singleLine: false) : ((boundTable != null) ? qualifier.GetQuickInfoText(cursorLocation, displayInfoProvider, boundTable) : null)));
		}
		return result;
	}

	internal SqlSelectStarExpression()
	{
	}

	internal SqlSelectStarExpression(SqlObjectIdentifier qualifier)
	{
		this.qualifier = qualifier;
	}
}
