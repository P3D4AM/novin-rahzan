using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class MetadataProviderBase : IMetadataProvider
{
	private IBuiltInFunctionLookup m_builtInFunctionLookup;

	private ICollationLookup m_collationLookup;

	private ISystemDataTypeLookup m_systemDataTypeLookup;

	private IMetadataFactory m_metadataFactory;

	public abstract IServer Server { get; }

	public IBuiltInFunctionLookup BuiltInFunctionLookup
	{
		get
		{
			return m_builtInFunctionLookup;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_builtInFunctionLookup = value;
		}
	}

	public ICollationLookup CollationLookup
	{
		get
		{
			return m_collationLookup;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_collationLookup = value;
		}
	}

	public ISystemDataTypeLookup SystemDataTypeLookup
	{
		get
		{
			return m_systemDataTypeLookup;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_systemDataTypeLookup = value;
		}
	}

	public IMetadataFactory MetadataFactory
	{
		get
		{
			return m_metadataFactory;
		}
		protected set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_metadataFactory = value;
		}
	}

	public abstract MetadataProviderEventHandler BeforeBindHandler { get; }

	public abstract MetadataProviderEventHandler AfterBindHandler { get; }

	protected MetadataProviderBase()
		: this(null, null, null, null)
	{
	}

	protected MetadataProviderBase(IBuiltInFunctionLookup builtInFunctionLookup, ICollationLookup collationLookup, ISystemDataTypeLookup systemDataTypeLookup, IMetadataFactory metadataFactory)
	{
		m_builtInFunctionLookup = builtInFunctionLookup ?? Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals.BuiltInFunctionLookup.Instance;
		m_collationLookup = collationLookup ?? Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals.CollationLookup.Instance;
		m_systemDataTypeLookup = systemDataTypeLookup ?? Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals.SystemDataTypeLookup.Instance;
		m_metadataFactory = metadataFactory ?? new MetadataFactory();
	}
}
