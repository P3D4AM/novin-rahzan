using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class DatabaseObject<P> : IMutableDatabaseObject, IDatabaseObject, IMetadataObject, IMutableMetadataObject where P : class, IDatabaseObject
{
	protected readonly P m_parent;

	private readonly string m_name;

	private bool m_isSystemObject;

	public bool IsSystemObject
	{
		get
		{
			return m_isSystemObject;
		}
		set
		{
			m_isSystemObject = value;
		}
	}

	public IDatabaseObject Parent => m_parent;

	public bool IsVolatile => false;

	public string Name => m_name;

	protected DatabaseObject(P parent, string name)
	{
		m_parent = parent;
		m_name = name;
	}

	protected DatabaseObject(P parent, IDatabaseObject dbObject)
	{
		m_parent = parent;
		m_name = dbObject.Name;
		m_isSystemObject = dbObject.IsSystemObject;
	}

	public abstract T Accept<T>(IDatabaseObjectVisitor<T> visitor);

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		return Accept((IDatabaseObjectVisitor<T>)visitor);
	}
}
