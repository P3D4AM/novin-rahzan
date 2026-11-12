using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCollation : SqlCodeObject
{
	private ICollation boundCollation;

	private readonly SqlIdentifier name;

	internal override bool IsVisitable => true;

	public ICollation BoundCollation => boundCollation;

	public override IMetadataObject BoundObject => boundCollation;

	public SqlIdentifier Name => name;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		if (!SqlIdentifier.IsNullOrEmpty(name))
		{
			if (StringTokenLookup.Find(name.Value) == StringToken.DatabaseDefault)
			{
				boundCollation = bindingContext.Batch.DatabaseCollation;
			}
			else
			{
				boundCollation = bindingContext.Batch.FindCollation(name.Value);
			}
			if (boundCollation != null)
			{
				base.BindingState |= BindingState.BindSucceeded;
				return;
			}
			name.ReportBindError(SR.InvalidCollation(name.Value));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundCollation = null;
		base.Unbind();
	}

	internal SqlCollation(SqlIdentifier name)
	{
		this.name = name;
	}
}
