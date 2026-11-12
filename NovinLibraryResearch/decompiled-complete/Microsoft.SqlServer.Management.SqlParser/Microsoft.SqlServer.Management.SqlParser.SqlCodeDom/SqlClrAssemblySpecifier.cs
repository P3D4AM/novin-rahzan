using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlClrAssemblySpecifier : SqlCodeObject
{
	private readonly SqlIdentifier assemblyName;

	internal override bool IsVisitable => true;

	public SqlIdentifier AssemblyName => assemblyName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (assemblyName != null)
			{
				yield return assemblyName;
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

	internal SqlClrAssemblySpecifier(SqlIdentifier assemblyName)
	{
		this.assemblyName = assemblyName;
	}
}
