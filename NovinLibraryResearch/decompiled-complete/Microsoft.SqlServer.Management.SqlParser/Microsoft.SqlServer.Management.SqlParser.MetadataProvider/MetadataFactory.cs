using System;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class MetadataFactory : IMetadataFactory
{
	private IAsymmetricKeyFactory asymmetricKey;

	private ICertificateFactory certificate;

	private IColumnFactory column;

	private IConstraintFactory constraint;

	private ICredentialFactory credential;

	private IDataTypeFactory dataType;

	private IDatabaseFactory database;

	private IExecutionContextFactory executionContext;

	private IIndexFactory index;

	private ILoginFactory login;

	private IParameterFactory parameter;

	private IPermissionFactory permission;

	private IResolvedSynonymFactory resolvedSynonym;

	private IRoleFactory role;

	private IScalarFactory scalar;

	private ISchemaFactory schema;

	private IServerFactory server;

	private IStoredProcedureFactory storedProcedure;

	private ISynonymFactory synonym;

	private ITabularFactory table;

	private ITriggerFactory trigger;

	private IUserFactory user;

	private IUserDefinedFunctionFactory userDefinedFunction;

	private IUserDefinedTypeFactory userDefinedType;

	private IVariableFactory variable;

	public IAsymmetricKeyFactory AsymmetricKey
	{
		get
		{
			return asymmetricKey;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			asymmetricKey = value;
		}
	}

	public ICertificateFactory Certificate
	{
		get
		{
			return certificate;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			certificate = value;
		}
	}

	public IColumnFactory Column
	{
		get
		{
			return column;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			column = value;
		}
	}

	public IConstraintFactory Constraint
	{
		get
		{
			return constraint;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			constraint = value;
		}
	}

	public ICredentialFactory Credential
	{
		get
		{
			return credential;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			credential = value;
		}
	}

	public IDatabaseFactory Database
	{
		get
		{
			return database;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			database = value;
		}
	}

	public IDataTypeFactory DataType
	{
		get
		{
			return dataType;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			dataType = value;
		}
	}

	public IExecutionContextFactory ExecutionContext
	{
		get
		{
			return executionContext;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			executionContext = value;
		}
	}

	public IIndexFactory Index
	{
		get
		{
			return index;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			index = value;
		}
	}

	public ILoginFactory Login
	{
		get
		{
			return login;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			login = value;
		}
	}

	public IParameterFactory Parameter
	{
		get
		{
			return parameter;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			parameter = value;
		}
	}

	public IPermissionFactory Permission
	{
		get
		{
			return permission;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			permission = value;
		}
	}

	public IResolvedSynonymFactory ResolvedSynonym
	{
		get
		{
			return resolvedSynonym;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			resolvedSynonym = value;
		}
	}

	public IRoleFactory Role
	{
		get
		{
			return role;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			role = value;
		}
	}

	public IScalarFactory Scalar
	{
		get
		{
			return scalar;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			scalar = value;
		}
	}

	public ISchemaFactory Schema
	{
		get
		{
			return schema;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			schema = value;
		}
	}

	public IServerFactory Server
	{
		get
		{
			return server;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			server = value;
		}
	}

	public IStoredProcedureFactory StoredProcedure
	{
		get
		{
			return storedProcedure;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			storedProcedure = value;
		}
	}

	public ISynonymFactory Synonym
	{
		get
		{
			return synonym;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			synonym = value;
		}
	}

	public ITabularFactory Tabular
	{
		get
		{
			return table;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			table = value;
		}
	}

	public ITriggerFactory Trigger
	{
		get
		{
			return trigger;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			trigger = value;
		}
	}

	public IUserFactory User
	{
		get
		{
			return user;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			user = value;
		}
	}

	public IUserDefinedFunctionFactory UserDefinedFunction
	{
		get
		{
			return userDefinedFunction;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			userDefinedFunction = value;
		}
	}

	public IUserDefinedTypeFactory UserDefinedType
	{
		get
		{
			return userDefinedType;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			userDefinedType = value;
		}
	}

	public IVariableFactory Variable
	{
		get
		{
			return variable;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			variable = value;
		}
	}

	public MetadataFactory()
	{
		asymmetricKey = ObjectFactoryBase<AsymmetricKeyFactory>.Instance;
		certificate = ObjectFactoryBase<CertificateFactory>.Instance;
		column = ObjectFactoryBase<ColumnFactory>.Instance;
		constraint = ObjectFactoryBase<ConstraintFactory>.Instance;
		credential = ObjectFactoryBase<CredentialFactory>.Instance;
		dataType = ObjectFactoryBase<DataTypeFactory>.Instance;
		database = ObjectFactoryBase<DatabaseFactory>.Instance;
		executionContext = ObjectFactoryBase<ExecutionContextFactory>.Instance;
		index = ObjectFactoryBase<IndexFactory>.Instance;
		login = ObjectFactoryBase<LoginFactory>.Instance;
		parameter = ObjectFactoryBase<ParameterFactory>.Instance;
		permission = ObjectFactoryBase<PermissionFactory>.Instance;
		resolvedSynonym = ObjectFactoryBase<ResolvedSynonymFactory>.Instance;
		role = ObjectFactoryBase<RoleFactory>.Instance;
		scalar = ObjectFactoryBase<ScalarFactory>.Instance;
		schema = ObjectFactoryBase<SchemaFactory>.Instance;
		server = ObjectFactoryBase<ServerFactory>.Instance;
		storedProcedure = ObjectFactoryBase<StoredProcedureFactory>.Instance;
		synonym = ObjectFactoryBase<SynonymFactory>.Instance;
		table = ObjectFactoryBase<TabularFactory>.Instance;
		trigger = ObjectFactoryBase<TriggerFactory>.Instance;
		user = ObjectFactoryBase<UserFactory>.Instance;
		userDefinedFunction = ObjectFactoryBase<UserDefinedFunctionFactory>.Instance;
		userDefinedType = ObjectFactoryBase<UserDefinedTypeFactory>.Instance;
		variable = ObjectFactoryBase<VariableFactory>.Instance;
	}
}
