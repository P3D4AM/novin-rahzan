using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTargetTableExpression : SqlCodeObject
{
	private SqlIdentifierCollection targetColumnList;

	private SqlObjectIdentifier name;

	private SqlScalarExpressionCollection arguments;

	private SqlObjectIdentifier openDataSourceObject;

	private string openDataSourceCommandString;

	private SqlHintCollection tableHints;

	internal override bool IsVisitable => true;

	public SqlIdentifierCollection TargetColumnList => targetColumnList;

	public SqlObjectIdentifier Name => name;

	public SqlScalarExpressionCollection Arguments => arguments;

	public SqlObjectIdentifier OpenDataSourceObject => openDataSourceObject;

	public string OpenDataSourceCommandString => openDataSourceCommandString;

	public SqlHintCollection TableHints => tableHints;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			foreach (SqlIdentifier targetColumn in targetColumnList)
			{
				yield return targetColumn;
			}
			if (tableHints != null)
			{
				foreach (SqlHint tableHint in tableHints)
				{
					yield return tableHint;
				}
			}
			if (arguments != null)
			{
				foreach (SqlScalarExpression argument in arguments)
				{
					yield return argument;
				}
			}
			if (openDataSourceObject != null)
			{
				yield return openDataSourceObject;
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

	internal SqlTargetTableExpression(SqlObjectIdentifier objectIdentifier, List<SqlIdentifier> columnList)
		: this(objectIdentifier, columnList, null, null, null, null)
	{
	}

	internal SqlTargetTableExpression(SqlObjectIdentifier objectIdentifier, List<SqlHint> tableHints, List<SqlIdentifier> columnList)
		: this(objectIdentifier, columnList, tableHints, null, null, null)
	{
	}

	internal SqlTargetTableExpression(SqlObjectIdentifier objectIdentifier, List<SqlScalarExpression> arguments, List<SqlIdentifier> columnList)
		: this(objectIdentifier, columnList, null, arguments, null, null)
	{
	}

	internal SqlTargetTableExpression(SqlObjectIdentifier objectIdentifier, List<SqlIdentifier> columnList, List<SqlHint> tableHints, List<SqlScalarExpression> arguments, SqlObjectIdentifier dataSource, string dataSourceCommandString)
	{
		name = objectIdentifier;
		if (columnList != null)
		{
			targetColumnList = new SqlIdentifierCollection(columnList);
		}
		if (tableHints != null)
		{
			this.tableHints = new SqlHintCollection(tableHints);
		}
		if (arguments != null)
		{
			this.arguments = new SqlScalarExpressionCollection(arguments);
		}
		openDataSourceObject = dataSource;
		openDataSourceCommandString = dataSourceCommandString;
	}
}
