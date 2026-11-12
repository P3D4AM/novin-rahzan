using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserDefinedDataType : SchemaOwnedObject, IMutableUserDefinedDataType, IUserDefinedDataType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IScalarDataType, IMutableUserDefinedType, IMutableDataType, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableScalarDataType
{
	private bool m_isUnknown;

	private bool m_nullable;

	private ISystemDataType m_baseSystemDataType;

	public bool Nullable
	{
		get
		{
			return m_nullable;
		}
		set
		{
			m_nullable = value;
		}
	}

	public bool IsScalar => true;

	public bool IsTable => false;

	public bool IsCursor => false;

	public bool IsUnknown
	{
		get
		{
			return m_isUnknown;
		}
		set
		{
			m_isUnknown = value;
		}
	}

	public ISystemDataType BaseSystemDataType
	{
		get
		{
			return m_baseSystemDataType;
		}
		set
		{
			m_baseSystemDataType = value;
		}
	}

	public bool IsSystem => false;

	public bool IsClr => false;

	public bool IsXml => false;

	public bool IsVoid => false;

	public UserDefinedDataType(ISchema schema, string name)
		: base(schema, name)
	{
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
