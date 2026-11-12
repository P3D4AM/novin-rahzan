using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlAlterLoginStatement : SqlDdlStatement
{
	private readonly SqlIdentifier m_LoginName;

	private readonly LoginOptions m_Options;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "ALTER LOGIN";

	internal sealed override bool IsCreateStatement => false;

	internal sealed override bool IsAlterStatement => true;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Login;

	public SqlIdentifier LoginName => m_LoginName;

	public SqlLoginPassword Password => m_Options.Password;

	public SqlLoginPassword OldPassword => m_Options.OldPassword;

	public SqlIdentifier DefaultDatabase => m_Options.DefaultDatabase;

	public SqlIdentifier DefaultLanguage => m_Options.DefaultLanguage;

	public SqlIdentifier Credential => m_Options.Credential;

	public SqlOnOffValue CheckExpiration => m_Options.CheckExpiration;

	public SqlOnOffValue CheckPolicy => m_Options.CheckPolicy;

	public SqlIdentifier Name => m_Options.Name;

	public bool NoCredential => m_Options.NoCredential;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return m_LoginName;
			foreach (SqlCodeObject child in m_Options.Children)
			{
				yield return child;
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

	public SqlAlterLoginStatement(SqlIdentifier loginName)
		: this(loginName, new List<SqlLoginOption>())
	{
	}

	internal SqlAlterLoginStatement(SqlIdentifier loginName, List<SqlLoginOption> alterLoginOptions)
	{
		m_LoginName = loginName;
		m_Options = new LoginOptions(alterLoginOptions);
	}
}
