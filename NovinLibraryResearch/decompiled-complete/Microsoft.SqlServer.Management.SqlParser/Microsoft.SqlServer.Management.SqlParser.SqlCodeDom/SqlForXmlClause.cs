using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlForXmlClause : SqlForClause
{
	private sealed class SqlForXmlInvalidClause : SqlForXmlClause
	{
		internal override bool IsVisitable => true;

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

		internal SqlForXmlInvalidClause(List<SqlForXmlUnknownDirective> directives)
			: base(directives, SqlForXmlDirectivesType.None)
		{
		}
	}

	private readonly SqlForXmlDirectivesType directiveType;

	private SqlForXmlDirectiveCollection directives;

	public virtual SqlForXmlDirectiveCollection Directives => directives;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlForXmlDirective directive in directives)
			{
				yield return directive;
			}
		}
	}

	internal SqlForXmlClause(List<SqlForXmlUnknownDirective> directives, SqlForXmlDirectivesType directiveType)
	{
		this.directiveType = directiveType;
		base.Position = directives[0].Position;
		if (this.directiveType != 0)
		{
			List<SqlForXmlDirective> list = new List<SqlForXmlDirective>();
			for (int i = 1; i < directives.Count; i++)
			{
				SqlForXmlDirective item = new SqlForXmlDirective(directives[i], this.directiveType);
				list.Add(item);
			}
			this.directives = new SqlForXmlDirectiveCollection(list);
		}
		else
		{
			this.directives = new SqlForXmlDirectiveCollection(Convertor(directives));
		}
	}

	private IEnumerable<SqlForXmlDirective> Convertor(List<SqlForXmlUnknownDirective> directives)
	{
		foreach (SqlForXmlUnknownDirective directive in directives)
		{
			yield return directive;
		}
	}

	internal static SqlForXmlClause Create(List<SqlForXmlUnknownDirective> directives, StringToken token)
	{
		string name = directives[0].Name;
		StringToken stringToken = StringTokenLookup.Find(name);
		bool flag = token == StringToken.Json;
		if (flag)
		{
			switch (stringToken)
			{
			case StringToken.Auto:
				return new SqlForXmlAutoClause(directives, flag);
			case StringToken.Path:
				return new SqlForXmlPathClause(directives, flag);
			default:
				directives[0].ReportParseError(SR.IncorrectSyntaxNear(name));
				return new SqlForXmlInvalidClause(directives);
			}
		}
		switch (stringToken)
		{
		case StringToken.Auto:
			return new SqlForXmlAutoClause(directives, flag);
		case StringToken.Raw:
			return new SqlForXmlRawClause(directives);
		case StringToken.Path:
			return new SqlForXmlPathClause(directives, flag);
		case StringToken.Explicit:
			return new SqlForXmlExplicitClause(directives);
		default:
			directives[0].ReportParseError(SR.IncorrectSyntaxNear(name));
			return new SqlForXmlInvalidClause(directives);
		}
	}
}
