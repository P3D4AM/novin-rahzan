using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ForeignKeyConstraint : Constraint, IMutableForeignKeyConstraint, IForeignKeyConstraint, IConstraint, IMetadataObject, IMutableConstraint, IMutableMetadataObject
{
	private readonly ForeignKeyColumnCollection m_columns;

	private ForeignKeyAction m_deleteAction;

	private ForeignKeyAction m_updateAction;

	private bool m_isEnabled;

	private bool m_isChecked;

	private bool m_isSystemNamed;

	private bool m_notForReplication;

	private ITable m_referencedTable;

	public override ConstraintType Type => ConstraintType.ForeignKey;

	public bool IsSystemNamed
	{
		get
		{
			return m_isSystemNamed;
		}
		set
		{
			m_isSystemNamed = value;
		}
	}

	public IMutableMetadataOrderedCollection<IForeignKeyColumn> Columns => m_columns;

	public ForeignKeyAction DeleteAction
	{
		get
		{
			return m_deleteAction;
		}
		set
		{
			m_deleteAction = value;
		}
	}

	public bool IsEnabled
	{
		get
		{
			return m_isEnabled;
		}
		set
		{
			m_isEnabled = value;
		}
	}

	public bool IsChecked
	{
		get
		{
			return m_isChecked;
		}
		set
		{
			m_isChecked = value;
		}
	}

	public bool NotForReplication
	{
		get
		{
			return m_notForReplication;
		}
		set
		{
			m_notForReplication = value;
		}
	}

	public ITable ReferencedTable
	{
		get
		{
			return m_referencedTable;
		}
		set
		{
			m_referencedTable = value;
		}
	}

	public ForeignKeyAction UpdateAction
	{
		get
		{
			return m_updateAction;
		}
		set
		{
			m_updateAction = value;
		}
	}

	IMetadataOrderedCollection<IForeignKeyColumn> IForeignKeyConstraint.Columns => m_columns;

	public ForeignKeyConstraint(ITable parent, string name)
		: base(parent, name)
	{
		CollationInfo collationInfo = parent.CollationInfo;
		m_columns = new ForeignKeyColumnCollection(collationInfo);
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
