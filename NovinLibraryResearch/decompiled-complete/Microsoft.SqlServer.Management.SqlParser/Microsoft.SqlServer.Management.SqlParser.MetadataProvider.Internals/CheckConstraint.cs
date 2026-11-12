using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class CheckConstraint : Constraint, IMutableCheckConstraint, ICheckConstraint, IConstraint, IMetadataObject, IMutableConstraint, IMutableMetadataObject
{
	private bool isEnabled;

	private bool isChecked;

	private bool isSystemNamed;

	private bool notForReplication;

	private string text;

	public override ConstraintType Type => ConstraintType.Check;

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

	public bool IsEnabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
		}
	}

	public bool IsChecked
	{
		get
		{
			return isChecked;
		}
		set
		{
			isChecked = value;
		}
	}

	public bool NotForReplication
	{
		get
		{
			return notForReplication;
		}
		set
		{
			notForReplication = value;
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

	public CheckConstraint(ITabular parent, string name)
		: base(parent, name)
	{
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
