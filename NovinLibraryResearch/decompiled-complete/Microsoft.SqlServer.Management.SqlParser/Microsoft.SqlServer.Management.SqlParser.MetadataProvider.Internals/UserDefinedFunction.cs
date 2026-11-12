using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class UserDefinedFunction : SchemaOwnedObject, IMutableUserDefinedFunction, IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMutableFunction, IMutableFunctionModuleBase, IMutableMetadataObject, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	private bool m_isEncrypted;

	private bool m_isSchemaBound;

	private bool m_isSqlClr;

	private string m_bodyText;

	private IExecutionContext m_executionContext;

	private readonly ParameterCollection m_parameters;

	private bool m_isQuotedIdentifierOn;

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

	protected UserDefinedFunction(ISchema schema, string name)
		: base(schema, name)
	{
		CollationInfo collationInfo = schema.Database.CollationInfo;
		m_parameters = new ParameterCollection(collationInfo);
		m_isQuotedIdentifierOn = true;
	}
}
