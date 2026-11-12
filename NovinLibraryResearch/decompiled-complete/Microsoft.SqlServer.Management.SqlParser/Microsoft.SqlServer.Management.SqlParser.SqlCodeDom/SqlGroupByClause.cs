using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlGroupByClause : SqlCodeObject
{
	private bool hasAll;

	private SqlGroupByItemCollection items;

	private SqlGroupByOptionType option;

	internal override bool IsVisitable => true;

	public bool HasAll => hasAll;

	public SqlGroupByItemCollection Items => items;

	public SqlGroupByOptionType Option => option;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlGroupByItem item in items)
			{
				yield return item;
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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.SelectScalar);
	}

	internal SqlGroupByClause(List<SqlGroupByItem> items, string option, bool all)
	{
		hasAll = all;
		this.items = new SqlGroupByItemCollection(items);
		if (option.Length > 0)
		{
			if (string.Equals(option, "WITH CUBE", StringComparison.OrdinalIgnoreCase))
			{
				this.option = SqlGroupByOptionType.Cube;
				return;
			}
			if (string.Equals(option, "WITH ROLLUP", StringComparison.OrdinalIgnoreCase))
			{
				this.option = SqlGroupByOptionType.Rollup;
				return;
			}
			this.option = SqlGroupByOptionType.None;
			ReportParseError(SR.InvalidGroupByOption(option));
		}
	}
}
