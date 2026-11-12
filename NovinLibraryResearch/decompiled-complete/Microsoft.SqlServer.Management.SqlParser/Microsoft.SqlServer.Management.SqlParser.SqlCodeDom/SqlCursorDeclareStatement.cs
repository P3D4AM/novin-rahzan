using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCursorDeclareStatement : SqlDeclareStatement
{
	private readonly SqlIdentifier name;

	private readonly List<SqlCursorOption> options;

	private readonly SqlSelectStatement selectStatement;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "DECLARE CURSOR";

	public SqlIdentifier Name => name;

	public SqlSelectStatement SelectStatement => selectStatement;

	public IEnumerable<SqlCursorOption> Options => options;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (options != null)
			{
				foreach (SqlCursorOption option in options)
				{
					yield return option;
				}
			}
			yield return selectStatement;
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

	public SqlCursorDeclareStatement(SqlIdentifier name, List<SqlCursorOption> options, CursorDefinitionInfo definitionInfo)
	{
		this.name = name;
		selectStatement = definitionInfo.SelectStatement;
		if (options.Count > 0)
		{
			if (definitionInfo.ExtendedCursorOptions.Count > 0)
			{
				ReportParseError(SR.MixingOldAndNewSyntaxForCursorOptionsNotAllowed);
			}
			foreach (SqlCursorOption option in options)
			{
				SqlCursorOptionType type = option.Type;
				if ((uint)type > 2u)
				{
					option.ReportParseError(SR.InvalidUsageOfCursorOption(option.Sql));
				}
			}
			this.options = options;
		}
		else if (definitionInfo.ExtendedCursorOptions.Count > 0)
		{
			this.options = definitionInfo.ExtendedCursorOptions;
		}
	}
}
