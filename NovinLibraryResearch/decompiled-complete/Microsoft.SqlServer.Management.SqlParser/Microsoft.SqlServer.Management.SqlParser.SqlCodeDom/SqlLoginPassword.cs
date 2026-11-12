using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlLoginPassword : SqlCodeObject
{
	private readonly SqlLiteralExpression password;

	private readonly SqlLoginOptionType optionFlags;

	internal override bool IsVisitable => true;

	public SqlLiteralExpression Password => password;

	public bool IsHashed => (optionFlags & SqlLoginOptionType.Hashed) == SqlLoginOptionType.Hashed;

	public bool MustChange => (optionFlags & SqlLoginOptionType.MustChange) == SqlLoginOptionType.MustChange;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return password;
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

	internal SqlLoginPassword(SqlLiteralExpression password, List<SqlLoginOption> options)
	{
		this.password = password;
		optionFlags = SqlLoginOptionType.None;
		if (options != null)
		{
			foreach (SqlLoginOption option in options)
			{
				SqlLoginOptionType type = option.Type;
				if ((optionFlags & type) == type)
				{
					string message = SR.OptionSpecifiedMultipleTimes(option.TypeToString());
					option.ReportParseError(message);
				}
				else if ((type & SqlLoginOptionType.Invalid) == SqlLoginOptionType.Invalid)
				{
					option.AsInvalidOption().ReportError();
				}
				else if (option.Type == SqlLoginOptionType.Hashed && optionFlags != 0)
				{
					string message2 = SR.IncorrectOptionOrder(option.TypeToString());
					option.ReportParseError(message2);
					optionFlags |= type;
				}
				else
				{
					optionFlags |= type;
				}
			}
		}
		if ((optionFlags & SqlLoginOptionType.Hashed) == SqlLoginOptionType.Hashed)
		{
			if (password.Type != 0)
			{
				password.ReportSyntaxError();
			}
		}
		else if (password.Type == LiteralValueType.Binary)
		{
			password.ReportSyntaxError();
		}
	}
}
