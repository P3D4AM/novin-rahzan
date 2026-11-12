using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserDefinedClrType : SchemaOwnedObject, IMutableUserDefinedClrType, IUserDefinedClrType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IClrDataType, IScalarDataType, IMutableUserDefinedType, IMutableDataType, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableClrDataType, IMutableScalarDataType
{
	private string m_assemblyName;

	private string m_className;

	private bool m_isBinaryOrdered;

	private bool m_isComVisible;

	private bool m_isNullable;

	private bool m_isUnknown;

	private readonly UdtMethodCollection m_methods;

	private readonly UdtDataMemberCollection m_dataMembers;

	public IMetadataCollection<IUdtMethod> Methods => m_methods;

	public IMetadataCollection<IUdtDataMember> DataMembers => m_dataMembers;

	public string AssemblyName
	{
		get
		{
			return m_assemblyName;
		}
		set
		{
			m_assemblyName = value;
		}
	}

	public string ClassName
	{
		get
		{
			return m_className;
		}
		set
		{
			m_className = value;
		}
	}

	public bool IsBinaryOrdered
	{
		get
		{
			return m_isBinaryOrdered;
		}
		set
		{
			m_isBinaryOrdered = value;
		}
	}

	public bool IsComVisible
	{
		get
		{
			return m_isComVisible;
		}
		set
		{
			m_isComVisible = value;
		}
	}

	public bool IsNullable
	{
		get
		{
			return m_isNullable;
		}
		set
		{
			m_isNullable = value;
		}
	}

	IMutableMetadataCollection<IUdtMethod> IMutableClrDataType.Methods => m_methods;

	IMutableMetadataCollection<IUdtDataMember> IMutableClrDataType.DataMembers => m_dataMembers;

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
			m_isUnknown = true;
		}
	}

	public ISystemDataType BaseSystemDataType
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsSystem => false;

	public bool IsClr => true;

	public bool IsXml => false;

	public bool IsVoid => false;

	public UserDefinedClrType(ISchema schema, string name)
		: base(schema, name)
	{
		m_methods = new UdtMethodCollection();
		m_dataMembers = new UdtDataMemberCollection();
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
