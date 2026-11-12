using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class View : TableViewBase, IMutableView, IView, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IMutableTableViewBase, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	private bool m_hasCheckOption;

	private bool m_isEncrypted;

	private bool m_isSchemaBound;

	private bool m_hasColumnSpecification;

	private bool m_returnsViewMetadata;

	private string m_queryText;

	public override TabularType TabularType => TabularType.View;

	public bool HasCheckOption
	{
		get
		{
			return m_hasCheckOption;
		}
		set
		{
			m_hasCheckOption = value;
		}
	}

	public bool IsEncrypted
	{
		get
		{
			return m_isEncrypted;
		}
		set
		{
			m_isEncrypted = value;
		}
	}

	public bool IsSchemaBound
	{
		get
		{
			return m_isSchemaBound;
		}
		set
		{
			m_isSchemaBound = value;
		}
	}

	public string QueryText
	{
		get
		{
			return m_queryText;
		}
		set
		{
			m_queryText = value;
		}
	}

	public bool ReturnsViewMetadata
	{
		get
		{
			return m_returnsViewMetadata;
		}
		set
		{
			m_returnsViewMetadata = value;
		}
	}

	public bool HasColumnSpecification
	{
		get
		{
			return m_hasColumnSpecification;
		}
		set
		{
			m_hasColumnSpecification = value;
		}
	}

	public View(ISchema schema, string name)
		: base(schema, name)
	{
	}

	public View(IView view)
		: base(view)
	{
		m_isEncrypted = view.IsEncrypted;
		m_isSchemaBound = view.IsSchemaBound;
		m_returnsViewMetadata = view.ReturnsViewMetadata;
		m_queryText = view.QueryText;
		m_hasCheckOption = view.HasCheckOption;
		m_hasColumnSpecification = view.HasColumnSpecification;
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
