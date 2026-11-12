using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateLoginFromCertificateStatement : SqlCreateLoginStatement
{
	private readonly SqlIdentifier certificateName;

	private readonly SqlIdentifier credential;

	internal override bool IsVisitable => true;

	internal sealed override LoginType MetadataLoginType => LoginType.Certificate;

	public SqlIdentifier CertificateName => certificateName;

	public SqlIdentifier Credential => credential;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			yield return certificateName;
			if (credential != null)
			{
				yield return credential;
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
		if (Credential != null)
		{
			login.Credential = BindCredential(bindingContext, Credential);
		}
		if (CertificateName != null)
		{
			login.Certificate = BindCertificate(bindingContext, CertificateName);
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == Credential)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.Credentials);
		}
		else if (sqlCodeObject == CertificateName)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.ServerCertificates);
		}
		return result;
	}

	internal SqlCreateLoginFromCertificateStatement(SqlIdentifier loginName, SqlIdentifier certificateName, List<SqlLoginOption> options)
		: base(loginName)
	{
		this.certificateName = certificateName;
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
			if (type == SqlLoginOptionType.Credential)
			{
				credential = option.AsObjectOption<SqlIdentifier>().Value;
			}
			sqlLoginOptionType |= type;
		}
	}
}
