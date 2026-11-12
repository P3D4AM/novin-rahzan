using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateLoginFromWindowsStatement : SqlCreateLoginStatement
{
	private readonly SqlIdentifier defaultDatabase;

	private readonly SqlIdentifier defaultLanguage;

	internal override bool IsVisitable => true;

	internal sealed override LoginType MetadataLoginType => LoginType.Windows;

	public SqlIdentifier DefaultDatabase => defaultDatabase;

	public SqlIdentifier DefaultLanguage => defaultLanguage;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (defaultDatabase != null && defaultLanguage != null)
			{
				if (defaultDatabase.Position.CompareTo(defaultLanguage.Position) <= 0)
				{
					yield return defaultDatabase;
					yield return defaultLanguage;
				}
				else
				{
					yield return defaultLanguage;
					yield return defaultDatabase;
				}
				yield break;
			}
			if (defaultDatabase != null)
			{
				yield return defaultDatabase;
			}
			if (defaultLanguage != null)
			{
				yield return defaultLanguage;
			}
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

	internal override void BindLogin(BatchBindingContext bindingContext, IMutableLogin login, bool setBuildProperties)
	{
		if (DefaultDatabase != null)
		{
			login.DefaultDatabase = BindDatabase(bindingContext, DefaultDatabase);
		}
		if (DefaultLanguage != null)
		{
			login.Language = DefaultLanguage.Value;
		}
	}

	internal SqlCreateLoginFromWindowsStatement(SqlIdentifier loginName, List<SqlLoginOption> options)
		: base(loginName)
	{
		SqlLoginOptionType sqlLoginOptionType = SqlLoginOptionType.None;
		if (options == null)
		{
			return;
		}
		foreach (SqlLoginOption option in options)
		{
			SqlLoginOptionType type = option.Type;
			if ((sqlLoginOptionType & type) == type)
			{
				string message = SR.OptionSpecifiedMultipleTimes(option.TypeToString());
				option.ReportParseError(message);
				continue;
			}
			if ((type & SqlLoginOptionType.Invalid) == SqlLoginOptionType.Invalid)
			{
				option.AsInvalidOption().ReportError();
				continue;
			}
			switch (type)
			{
			case SqlLoginOptionType.DefaultDatabase:
				defaultDatabase = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			case SqlLoginOptionType.DefaultLanguage:
				defaultLanguage = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			}
			sqlLoginOptionType |= type;
		}
	}
}
