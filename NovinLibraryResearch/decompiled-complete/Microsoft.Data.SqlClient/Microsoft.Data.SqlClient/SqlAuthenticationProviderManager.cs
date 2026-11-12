using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class SqlAuthenticationProviderManager
{
	private const string ActiveDirectoryPassword = "active directory password";

	private const string ActiveDirectoryIntegrated = "active directory integrated";

	private const string ActiveDirectoryInteractive = "active directory interactive";

	private const string ActiveDirectoryServicePrincipal = "active directory service principal";

	private const string ActiveDirectoryDeviceCodeFlow = "active directory device code flow";

	private const string ActiveDirectoryManagedIdentity = "active directory managed identity";

	private const string ActiveDirectoryMSI = "active directory msi";

	private const string ActiveDirectoryDefault = "active directory default";

	public static readonly SqlAuthenticationProviderManager Instance;

	private readonly string _typeName;

	private readonly SqlAuthenticationInitializer _initializer;

	private readonly IReadOnlyCollection<SqlAuthenticationMethod> _authenticationsWithAppSpecifiedProvider;

	private readonly ConcurrentDictionary<SqlAuthenticationMethod, SqlAuthenticationProvider> _providers;

	private readonly SqlClientLogger _sqlAuthLogger = new SqlClientLogger();

	private readonly string _applicationClientId = "2fd908ad-0664-4344-b9be-cd3e8b574c38";

	static SqlAuthenticationProviderManager()
	{
		//IL_001e: Expected O, but got Unknown
		SqlAuthenticationProviderConfigurationSection sqlAuthenticationProviderConfigurationSection = null;
		try
		{
			sqlAuthenticationProviderConfigurationSection = FetchConfigurationSection<SqlClientAuthenticationProviderConfigurationSection>("SqlClientAuthenticationProviders");
			if (sqlAuthenticationProviderConfigurationSection == null)
			{
				sqlAuthenticationProviderConfigurationSection = FetchConfigurationSection<SqlAuthenticationProviderConfigurationSection>("SqlAuthenticationProviders");
			}
		}
		catch (ConfigurationErrorsException val)
		{
			ConfigurationErrorsException args = val;
			SqlClientEventSource.Log.TryTraceEvent<ConfigurationErrorsException>("static SqlAuthenticationProviderManager: Unable to load custom SqlAuthenticationProviders or SqlClientAuthenticationProviders. ConfigurationManager failed to load due to configuration errors: {0}", args);
		}
		Instance = new SqlAuthenticationProviderManager(sqlAuthenticationProviderConfigurationSection);
		ActiveDirectoryAuthenticationProvider provider = new ActiveDirectoryAuthenticationProvider(Instance._applicationClientId);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryIntegrated, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryPassword, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryInteractive, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryServicePrincipal, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryMSI, provider);
		Instance.SetProvider(SqlAuthenticationMethod.ActiveDirectoryDefault, provider);
	}

	public SqlAuthenticationProviderManager(SqlAuthenticationProviderConfigurationSection configSection)
	{
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		_typeName = GetType().Name;
		string method = "Ctor";
		_providers = new ConcurrentDictionary<SqlAuthenticationMethod, SqlAuthenticationProvider>();
		HashSet<SqlAuthenticationMethod> hashSet = (HashSet<SqlAuthenticationMethod>)(_authenticationsWithAppSpecifiedProvider = new HashSet<SqlAuthenticationMethod>());
		if (configSection == null)
		{
			_sqlAuthLogger.LogInfo(_typeName, method, "Neither SqlClientAuthenticationProviders nor SqlAuthenticationProviders configuration section found.");
			return;
		}
		if (!string.IsNullOrEmpty(configSection.ApplicationClientId))
		{
			_applicationClientId = configSection.ApplicationClientId;
			_sqlAuthLogger.LogInfo(_typeName, method, "Received user-defined Application Client Id");
		}
		else
		{
			_sqlAuthLogger.LogInfo(_typeName, method, "No user-defined Application Client Id found.");
		}
		if (!string.IsNullOrEmpty(configSection.InitializerType))
		{
			try
			{
				Type type = Type.GetType(configSection.InitializerType, throwOnError: true);
				_initializer = (SqlAuthenticationInitializer)Activator.CreateInstance(type);
				_initializer.Initialize();
			}
			catch (Exception e)
			{
				throw SQL.CannotCreateSqlAuthInitializer(configSection.InitializerType, e);
			}
			_sqlAuthLogger.LogInfo(_typeName, method, "Created user-defined SqlAuthenticationInitializer.");
		}
		else
		{
			_sqlAuthLogger.LogInfo(_typeName, method, "No user-defined SqlAuthenticationInitializer found.");
		}
		if (configSection.Providers != null && ((ConfigurationElementCollection)configSection.Providers).Count > 0)
		{
			foreach (ProviderSettings item in (ConfigurationElementCollection)configSection.Providers)
			{
				ProviderSettings val = item;
				SqlAuthenticationMethod sqlAuthenticationMethod = AuthenticationEnumFromString(val.Name);
				SqlAuthenticationProvider sqlAuthenticationProvider;
				try
				{
					Type type2 = Type.GetType(val.Type, throwOnError: true);
					sqlAuthenticationProvider = (SqlAuthenticationProvider)Activator.CreateInstance(type2);
				}
				catch (Exception e2)
				{
					throw SQL.CannotCreateAuthProvider(sqlAuthenticationMethod.ToString(), val.Type, e2);
				}
				if (!sqlAuthenticationProvider.IsSupported(sqlAuthenticationMethod))
				{
					throw SQL.UnsupportedAuthenticationByProvider(sqlAuthenticationMethod.ToString(), val.Type);
				}
				_providers[sqlAuthenticationMethod] = sqlAuthenticationProvider;
				hashSet.Add(sqlAuthenticationMethod);
				_sqlAuthLogger.LogInfo(_typeName, method, $"Added user-defined auth provider: {((val != null) ? val.Type : null)} for authentication {sqlAuthenticationMethod}.");
			}
			return;
		}
		_sqlAuthLogger.LogInfo(_typeName, method, "No user-defined auth providers.");
	}

	public SqlAuthenticationProvider GetProvider(SqlAuthenticationMethod authenticationMethod)
	{
		if (!_providers.TryGetValue(authenticationMethod, out var value))
		{
			return null;
		}
		return value;
	}

	public bool SetProvider(SqlAuthenticationMethod authenticationMethod, SqlAuthenticationProvider provider)
	{
		if (!provider.IsSupported(authenticationMethod))
		{
			throw SQL.UnsupportedAuthenticationByProvider(authenticationMethod.ToString(), provider.GetType().Name);
		}
		string methodName = "SetProvider";
		if (_authenticationsWithAppSpecifiedProvider.Contains(authenticationMethod))
		{
			_sqlAuthLogger.LogError(_typeName, methodName, $"Failed to add provider {GetProviderType(provider)} because a user-defined provider with type {GetProviderType(_providers[authenticationMethod])} already existed for authentication {authenticationMethod}.");
			return false;
		}
		_providers.AddOrUpdate(authenticationMethod, provider, delegate(SqlAuthenticationMethod key, SqlAuthenticationProvider oldProvider)
		{
			oldProvider?.BeforeUnload(authenticationMethod);
			if (provider != null)
			{
				provider.BeforeLoad(authenticationMethod);
			}
			_sqlAuthLogger.LogInfo(_typeName, methodName, $"Added auth provider {GetProviderType(provider)}, overriding existed provider {GetProviderType(oldProvider)} for authentication {authenticationMethod}.");
			return provider;
		});
		return true;
	}

	private static T FetchConfigurationSection<T>(string name)
	{
		Type typeFromHandle = typeof(T);
		object section = ConfigurationManager.GetSection(name);
		if (section != null)
		{
			ConfigurationSection val = (ConfigurationSection)((section is ConfigurationSection) ? section : null);
			if (val != null && ((object)val).GetType() == typeFromHandle)
			{
				return (T)section;
			}
			SqlClientEventSource.Log.TraceEvent("Found a custom {0} configuration but it is not of type {1}.", name, typeFromHandle.FullName, "FetchConfigurationSection");
		}
		return default(T);
	}

	private static SqlAuthenticationMethod AuthenticationEnumFromString(string authentication)
	{
		return authentication.ToLowerInvariant() switch
		{
			"active directory integrated" => SqlAuthenticationMethod.ActiveDirectoryIntegrated, 
			"active directory password" => SqlAuthenticationMethod.ActiveDirectoryPassword, 
			"active directory interactive" => SqlAuthenticationMethod.ActiveDirectoryInteractive, 
			"active directory service principal" => SqlAuthenticationMethod.ActiveDirectoryServicePrincipal, 
			"active directory device code flow" => SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow, 
			"active directory managed identity" => SqlAuthenticationMethod.ActiveDirectoryManagedIdentity, 
			"active directory msi" => SqlAuthenticationMethod.ActiveDirectoryMSI, 
			"active directory default" => SqlAuthenticationMethod.ActiveDirectoryDefault, 
			_ => throw SQL.UnsupportedAuthentication(authentication), 
		};
	}

	private static string GetProviderType(SqlAuthenticationProvider provider)
	{
		if (provider == null)
		{
			return "null";
		}
		return provider.GetType().FullName;
	}
}
