using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DefaultConstraint : MetadataObjectBase, IMutableDefaultConstraint, IDefaultConstraint, IMetadataObject, IMutableMetadataObject
{
	private readonly IColumn parent;

	private bool isSystemNamed;

	private string text;

	public bool IsSystemNamed
	{
		get
		{
			return isSystemNamed;
		}
		set
		{
			isSystemNamed = value;
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
		}
	}

	public IColumn Parent => parent;

	public DefaultConstraint(IColumn parent, string name)
		: base(name)
	{
		this.parent = parent;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
