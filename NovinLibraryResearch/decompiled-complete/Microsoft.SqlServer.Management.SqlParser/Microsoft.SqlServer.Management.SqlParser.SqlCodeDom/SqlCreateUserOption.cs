using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserOption : SqlCodeObject
{
	private readonly SqlCreateUserOptionType type;

	private readonly SqlIdentifier value;

	internal override bool IsVisitable => true;

	public SqlCreateUserOptionType Type => type;

	public SqlIdentifier Value => value;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return value;
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

	public SqlCreateUserOption(StringToken stringToken, SqlIdentifier value)
		: this(stringToken)
	{
		this.value = value;
		this.value.Position = value.Position;
	}

	public SqlCreateUserOption(StringToken stringToken, SqlLiteralExpression literalExpression)
		: this(stringToken)
	{
		value = new SqlIdentifier(literalExpression.Value);
		value.Position = literalExpression.Position;
	}

	private SqlCreateUserOption(StringToken stringToken)
	{
		type = GetType(stringToken);
	}

	private SqlCreateUserOptionType GetType(StringToken stringToken)
	{
		return stringToken switch
		{
			StringToken.DefaultSchema => SqlCreateUserOptionType.DefaultSchema, 
			StringToken.DefaultLanguage => SqlCreateUserOptionType.DefaultLanguage, 
			StringToken.Password => SqlCreateUserOptionType.Password, 
			_ => SqlCreateUserOptionType.Invalid, 
		};
	}
}
