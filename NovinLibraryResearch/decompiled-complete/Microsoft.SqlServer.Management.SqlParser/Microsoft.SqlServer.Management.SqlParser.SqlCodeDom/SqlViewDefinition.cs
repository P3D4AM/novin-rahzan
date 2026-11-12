using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlViewDefinition : SqlCodeObject
{
	private readonly SqlObjectIdentifier name;

	private readonly SqlIdentifierCollection columnList;

	private readonly SqlCollection<SqlModuleOption> options;

	private readonly SqlQueryWithClause queryWithClause;

	private readonly SqlQueryExpression queryExpression;

	private readonly bool hasCheckOption;

	internal override bool IsVisitable => true;

	public SqlObjectIdentifier Name => name;

	public SqlIdentifierCollection ColumnList => columnList;

	public SqlQueryExpression QueryExpression => queryExpression;

	public SqlQueryWithClause QueryWithClause => queryWithClause;

	public bool HasCheckOption => hasCheckOption;

	public SqlCollection<SqlModuleOption> Options => options;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (columnList != null)
			{
				foreach (SqlIdentifier column in columnList)
				{
					yield return column;
				}
			}
			if (options != null)
			{
				foreach (SqlModuleOption option in options)
				{
					yield return option;
				}
			}
			if (queryWithClause != null)
			{
				yield return queryWithClause;
			}
			yield return queryExpression;
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

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal void Bind(BatchBindingContext bindingContext, IMutableView view, bool setBuildProperties)
	{
		BatchBindingContext bindingContext2 = bindingContext;
		if (queryWithClause != null)
		{
			queryWithClause.Bind(bindingContext2);
			base.BindingState |= queryWithClause.BindingState;
			if (queryWithClause.CTEBoundContext != null)
			{
				bindingContext2 = queryWithClause.CTEBoundContext;
			}
		}
		queryExpression.Bind(bindingContext2);
		base.BindingState = queryExpression.BindingState;
		BindUtils.Table.PopulateTabularColumns(Name.ObjectName, ColumnList, queryExpression, bindingContext, view, BindUtils.Table.ColumnAliasOption.ScalarAlias);
		if (options != null)
		{
			SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
			foreach (SqlModuleOption option in options)
			{
				option.Bind(bindingContext);
				base.BindingState |= option.BindingState;
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) != SqlModuleOptionMask.Invalid && (sqlModuleOptionMask & typeMask) != typeMask)
				{
					switch (option.Type)
					{
					case SqlModuleOptionType.Encryption:
						view.IsEncrypted = true;
						break;
					case SqlModuleOptionType.SchemaBinding:
						view.IsSchemaBound = true;
						break;
					case SqlModuleOptionType.ViewMetadata:
						view.ReturnsViewMetadata = true;
						break;
					}
					sqlModuleOptionMask |= typeMask;
				}
			}
		}
		view.HasCheckOption = HasCheckOption;
		view.HasColumnSpecification = ColumnList != null;
		if (setBuildProperties)
		{
			LexLocation lexLocation = queryExpression.Position;
			if (queryWithClause != null)
			{
				lexLocation = lexLocation.Merge(queryWithClause.Position);
			}
			view.QueryText = GetSql(lexLocation);
		}
	}

	internal override bool MayChildHaveCompletionList(SqlCodeObject child)
	{
		if (child != queryWithClause)
		{
			return child == queryExpression;
		}
		return true;
	}

	internal SqlViewDefinition(SqlObjectIdentifier name, List<SqlIdentifier> columnList, List<SqlModuleOption> options, SqlQueryWithClause queryWithClause, SqlQueryExpression queryExpression, bool hasCheckOption)
	{
		this.name = name;
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
		if (!SqlIdentifier.IsNullOrEmpty(this.name.DatabaseName))
		{
			this.name.DatabaseName.ReportParseError(SR.DatabaseNameAsPrefixInCreateView);
		}
		SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
		if (options != null)
		{
			this.options = new SqlCollection<SqlModuleOption>(options);
			foreach (SqlModuleOption option in options)
			{
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid)
				{
					string message = SR.OptionNotRecognized(option.ToString());
					option.ReportParseError(message);
				}
				else if ((typeMask | SqlModuleOptionMask.CreateView) != SqlModuleOptionMask.CreateView)
				{
					string invalidOptionInCreateView = SR.InvalidOptionInCreateView;
					option.ReportParseError(invalidOptionInCreateView);
				}
				else if ((sqlModuleOptionMask & typeMask) == typeMask)
				{
					string message2 = SR.OptionSpecifiedMultipleTimes(option.ToString());
					option.ReportParseError(message2);
				}
				sqlModuleOptionMask |= typeMask;
			}
		}
		this.queryWithClause = queryWithClause;
		this.queryExpression = queryExpression;
		this.hasCheckOption = hasCheckOption;
	}
}
