using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class StoredProcedure : SchemaOwnedObject, IMutableStoredProcedure, IStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase, IMutableCallableModule, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableMetadataObject, IMutableFunctionModuleBase
{
	private bool m_forReplication;

	private bool m_isEncrypted;

	private bool m_isSqlClr;

	private bool m_isRecompiled;

	private bool m_startup;

	private string m_bodyText;

	private IExecutionContext m_executionContext;

	private IScalarDataType m_returnType;

	private readonly ParameterCollection m_parameters;

	private bool m_isQuotedIdentifierOn;

	public CallableModuleType ModuleType => CallableModuleType.StoredProcedure;

	public IScalarDataType ReturnType
	{
		get
		{
			return m_returnType;
		}
		set
		{
			m_returnType = value;
		}
	}

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => m_parameters;

	public IMutableMetadataOrderedCollection<IParameter> Parameters => m_parameters;

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

	public IExecutionContext ExecutionContext
	{
		get
		{
			return m_executionContext;
		}
		set
		{
			m_executionContext = value;
		}
	}

	public string BodyText
	{
		get
		{
			return m_bodyText;
		}
		set
		{
			m_bodyText = value;
		}
	}

	public bool ForReplication
	{
		get
		{
			return m_forReplication;
		}
		set
		{
			m_forReplication = value;
		}
	}

	public bool IsSqlClr
	{
		get
		{
			return m_isSqlClr;
		}
		set
		{
			m_isSqlClr = value;
		}
	}

	public bool IsRecompiled
	{
		get
		{
			return m_isRecompiled;
		}
		set
		{
			m_isRecompiled = value;
		}
	}

	public bool Startup
	{
		get
		{
			return m_startup;
		}
		set
		{
			m_startup = value;
		}
	}

	public bool IsQuotedIdentifierOn
	{
		get
		{
			return m_isQuotedIdentifierOn;
		}
		set
		{
			m_isQuotedIdentifierOn = true;
		}
	}

	public StoredProcedure(ISchema schema, string name)
		: base(schema, name)
	{
		CollationInfo collationInfo = schema.Database.CollationInfo;
		m_parameters = new ParameterCollection(collationInfo);
		m_isQuotedIdentifierOn = true;
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
