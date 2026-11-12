using System;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCursorOption : SqlCodeObject
{
	private readonly SqlCursorOptionType type;

	internal override bool IsVisitable => true;

	public SqlCursorOptionType Type => type;

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

	public SqlCursorOption(StringToken token)
	{
		switch (token)
		{
		case StringToken.Dynamic:
			type = SqlCursorOptionType.Dynamic;
			break;
		case StringToken.FastForward:
			type = SqlCursorOptionType.FastForward;
			break;
		case StringToken.ForwardOnly:
			type = SqlCursorOptionType.ForwardOnly;
			break;
		case StringToken.Global:
			type = SqlCursorOptionType.Global;
			break;
		case StringToken.Insensitive:
			type = SqlCursorOptionType.Insensitive;
			break;
		case StringToken.Keyset:
			type = SqlCursorOptionType.Keyset;
			break;
		case StringToken.Local:
			type = SqlCursorOptionType.Local;
			break;
		case StringToken.Optimistic:
			type = SqlCursorOptionType.Optimistic;
			break;
		case StringToken.Read_Only:
			type = SqlCursorOptionType.ReadOnly;
			break;
		case StringToken.Scroll:
			type = SqlCursorOptionType.Scroll;
			break;
		case StringToken.ScrollLocks:
			type = SqlCursorOptionType.ScrollLocks;
			break;
		case StringToken.Static:
			type = SqlCursorOptionType.Static;
			break;
		case StringToken.TypeWarning:
			type = SqlCursorOptionType.TypeWarning;
			break;
		default:
			type = SqlCursorOptionType.Invalid;
			ReportParseError(SR.UnrecognizedCursorOption(base.Sql));
			break;
		}
	}
}
