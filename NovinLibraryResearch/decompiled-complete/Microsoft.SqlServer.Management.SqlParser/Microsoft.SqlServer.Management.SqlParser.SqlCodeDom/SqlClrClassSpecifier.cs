using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlClrClassSpecifier : SqlClrAssemblySpecifier
{
	private readonly SqlIdentifier className;

	internal override bool IsVisitable => true;

	public SqlIdentifier ClassName => className;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.AssemblyName != null)
			{
				yield return base.AssemblyName;
			}
			if (className != null)
			{
				yield return className;
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

	internal SqlClrClassSpecifier(SqlIdentifier assemblyName)
		: base(assemblyName)
	{
	}

	internal SqlClrClassSpecifier(SqlIdentifier assemblyName, SqlIdentifier className)
		: base(assemblyName)
	{
		this.className = className;
	}
}
