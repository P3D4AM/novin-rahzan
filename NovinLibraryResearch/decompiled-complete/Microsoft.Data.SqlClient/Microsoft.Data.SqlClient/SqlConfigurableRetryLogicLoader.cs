using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConfigurableRetryLogicLoader
{
	private const string TypeName = "SqlConfigurableRetryLogicLoader";

	internal SqlRetryLogicBaseProvider ConnectionProvider { get; private set; }

	internal SqlRetryLogicBaseProvider CommandProvider { get; private set; }

	private void AssignProviders(SqlRetryLogicBaseProvider cnnProvider = null, SqlRetryLogicBaseProvider cmdProvider = null)
	{
		ConnectionProvider = cnnProvider ?? SqlConfigurableRetryFactory.CreateNoneRetryProvider();
		CommandProvider = cmdProvider ?? SqlConfigurableRetryFactory.CreateNoneRetryProvider();
	}

	public SqlConfigurableRetryLogicLoader(ISqlConfigurableRetryConnectionSection connectionRetryConfigs, ISqlConfigurableRetryCommandSection commandRetryConfigs, string cnnSectionName = "SqlConfigurableRetryLogicConnection", string cmdSectionName = "SqlConfigurableRetryLogicCommand")
	{
		AssignProviders((connectionRetryConfigs == null) ? null : CreateRetryLogicProvider(cnnSectionName, connectionRetryConfigs), (commandRetryConfigs == null) ? null : CreateRetryLogicProvider(cmdSectionName, commandRetryConfigs));
	}

	private static SqlRetryLogicBaseProvider CreateRetryLogicProvider(string sectionName, ISqlConfigurableRetryConnectionSection configSection)
	{
		string name = MethodBase.GetCurrentMethod().Name;
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Entry point.", "SqlConfigurableRetryLogicLoader", name);
		try
		{
			SqlRetryLogicOption sqlRetryLogicOption = new SqlRetryLogicOption
			{
				NumberOfTries = configSection.NumberOfTries,
				DeltaTime = configSection.DeltaTime,
				MinTimeInterval = configSection.MinTimeInterval,
				MaxTimeInterval = configSection.MaxTimeInterval
			};
			if (!string.IsNullOrEmpty(configSection.TransientErrors))
			{
				sqlRetryLogicOption.TransientErrors = (from x in configSection.TransientErrors.Split(new char[1] { ',' })
					select Convert.ToInt32(x)).ToList();
			}
			ISqlConfigurableRetryCommandSection cmdSection = configSection as ISqlConfigurableRetryCommandSection;
			if (cmdSection != null && !string.IsNullOrEmpty(cmdSection.AuthorizedSqlCondition))
			{
				sqlRetryLogicOption.AuthorizedSqlCondition = (string x) => Regex.IsMatch(x, cmdSection.AuthorizedSqlCondition);
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Successfully created a {2} object to use on creating a retry logic provider from the section '{3}'.", "SqlConfigurableRetryLogicLoader", name, "SqlRetryLogicOption", sectionName);
			SqlRetryLogicBaseProvider result = ResolveRetryLogicProvider(configSection.RetryLogicType, configSection.RetryMethod, sqlRetryLogicOption);
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Successfully created a {2} object from the section '{3}'.", "SqlConfigurableRetryLogicLoader", name, "SqlRetryLogicBaseProvider", sectionName);
			return result;
		}
		catch (Exception args)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> {2}", "SqlConfigurableRetryLogicLoader", name, args);
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Due to an exception, the default non-retriable logic will be applied.", "SqlConfigurableRetryLogicLoader", name);
		return SqlConfigurableRetryFactory.CreateNoneRetryProvider();
	}

	private static SqlRetryLogicBaseProvider ResolveRetryLogicProvider(string configurableRetryType, string retryMethod, SqlRetryLogicOption option)
	{
		string name = MethodBase.GetCurrentMethod().Name;
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Entry point.", "SqlConfigurableRetryLogicLoader", name);
		if (string.IsNullOrEmpty(retryMethod))
		{
			throw new ArgumentNullException("Failed to create SqlRetryLogicBaseProvider object because the retryMethod value is null or empty.");
		}
		Type type = null;
		try
		{
			type = LoadType(configurableRetryType);
		}
		catch (Exception args)
		{
			type = typeof(SqlConfigurableRetryFactory);
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Unable to load the '{2}' type; Trying to use the internal `{3}` type: {4}", "SqlConfigurableRetryLogicLoader", name, configurableRetryType, type.FullName, args);
		}
		try
		{
			object obj = CreateInstance(type, retryMethod, option);
			if (obj is SqlRetryLogicBaseProvider sqlRetryLogicBaseProvider)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The created instace is a {2} type.", "SqlConfigurableRetryLogicLoader", name, typeof(SqlRetryLogicBaseProvider).FullName);
				sqlRetryLogicBaseProvider.Retrying = (EventHandler<SqlRetryingEventArgs>)Delegate.Combine(sqlRetryLogicBaseProvider.Retrying, new EventHandler<SqlRetryingEventArgs>(OnRetryingEvent));
				return sqlRetryLogicBaseProvider;
			}
		}
		catch (Exception innerException)
		{
			throw new Exception("Exception occurred when running the `" + type.FullName + "." + retryMethod + "()` method.", innerException);
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Unable to resolve a valid provider; Returns `null`.", "SqlConfigurableRetryLogicLoader", name);
		return null;
	}

	private static object CreateInstance(Type type, string retryMethodName, SqlRetryLogicOption option)
	{
		string name = MethodBase.GetCurrentMethod().Name;
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Entry point.", "SqlConfigurableRetryLogicLoader", name);
		if (type == typeof(SqlConfigurableRetryFactory) || type == null)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The given type `{2}` infers as internal `{3}` type.", "SqlConfigurableRetryLogicLoader", name, type?.Name, typeof(SqlConfigurableRetryFactory).FullName);
			MethodInfo method = typeof(SqlConfigurableRetryFactory).GetMethod(retryMethodName);
			if (method == null)
			{
				throw new InvalidOperationException("Failed to resolve the '" + retryMethodName + "' method from `" + typeof(SqlConfigurableRetryFactory).FullName + "` type.");
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The `{2}.{3}()` method has been discovered as the `{4}` method name.", "SqlConfigurableRetryLogicLoader", name, method.ReflectedType.FullName, method.Name, retryMethodName);
			object[] parameters = PrepareParamValues(method.GetParameters(), option, retryMethodName);
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Parameters are prepared to invoke the `{2}.{3}()` method.", "SqlConfigurableRetryLogicLoader", name, method.ReflectedType.FullName, method.Name);
			return method.Invoke(null, parameters);
		}
		MethodInfo method2 = type.GetMethod(retryMethodName);
		if (method2 == null)
		{
			throw new InvalidOperationException("Failed to resolve the '" + retryMethodName + "' method from `" + type.FullName + "` type.");
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The `{2}` method metadata has been extracted from the `{3}` type by using the `{4}` method name.", "SqlConfigurableRetryLogicLoader", name, method2.Name, type.FullName, retryMethodName);
		if (!typeof(SqlRetryLogicBaseProvider).IsAssignableFrom(method2.ReturnType))
		{
			throw new InvalidCastException("Invalid return type; Return type must be of `" + typeof(SqlRetryLogicBaseProvider).FullName + "` type.");
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The return type of the `{2}.{3}()` method is valid.", "SqlConfigurableRetryLogicLoader", name, type.FullName, method2.Name);
		object[] parameters2 = PrepareParamValues(method2.GetParameters(), option, retryMethodName);
		if (method2.IsStatic)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Run the static `{2}` method without object creation of `{3}` type.", "SqlConfigurableRetryLogicLoader", name, method2.Name, type.FullName);
			return method2.Invoke(null, parameters2);
		}
		object obj = Activator.CreateInstance(type);
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> An instance of `{2}` type is created to invoke the `{3}` method.", "SqlConfigurableRetryLogicLoader", name, type.FullName, method2.Name);
		return method2.Invoke(obj, parameters2);
	}

	private static object[] PrepareParamValues(ParameterInfo[] parameterInfos, SqlRetryLogicOption option, string retryMethod)
	{
		if (parameterInfos.FirstOrDefault((ParameterInfo x) => x.ParameterType == typeof(SqlRetryLogicOption)) == null)
		{
			string message = "Failed to create SqlRetryLogicBaseProvider object because of invalid " + retryMethod + "'s parameters." + Environment.NewLine + "The function must have a paramter of type 'SqlRetryLogicOption'.";
			throw new InvalidOperationException(message);
		}
		object[] array = new object[parameterInfos.Length];
		for (int i = 0; i < parameterInfos.Length; i++)
		{
			ParameterInfo paramInfo = parameterInfos[i];
			if (paramInfo.HasDefaultValue && paramInfo.ParameterType != typeof(SqlRetryLogicOption))
			{
				array[i] = paramInfo.DefaultValue;
				continue;
			}
			if (paramInfo.ParameterType == typeof(SqlRetryLogicOption))
			{
				if (!paramInfo.HasDefaultValue || (paramInfo.HasDefaultValue && parameterInfos.FirstOrDefault((ParameterInfo x) => x != paramInfo && !x.HasDefaultValue && x.ParameterType == typeof(SqlRetryLogicOption)) == null))
				{
					array[i] = option;
				}
				else
				{
					array[i] = paramInfo.DefaultValue;
				}
				continue;
			}
			string message2 = "Failed to create SqlRetryLogicBaseProvider object because of invalid retryMethod's parameters." + Environment.NewLine + "Parameter '" + paramInfo.ParameterType.Name + " " + paramInfo.Name + "' doesn't have default value.";
			throw new InvalidOperationException(message2);
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Parameters are prepared to invoke the `{2}.{3}()` method.", "SqlConfigurableRetryLogicLoader", MethodBase.GetCurrentMethod().Name, typeof(SqlConfigurableRetryFactory).FullName, retryMethod);
		return array;
	}

	private static void OnRetryingEvent(object sender, SqlRetryingEventArgs args)
	{
		Exception ex = args.Exceptions[args.Exceptions.Count - 1];
		string args2 = string.Format("<sc.{0}.OnRetryingEvent|INFO>: Default configurable retry logic for {1} object. attempts count:{2}, upcoming delay:{3}", "SqlConfigurableRetryLogicLoader", sender.GetType().Name, args.RetryCount, args.Delay);
		SqlClientEventSource.Log.TryTraceEvent("{0}, Last exception:<{1}>", args2, ex.Message);
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<ADV>{0}, Last exception: {1}", args2, ex);
	}

	private static Type LoadType(string fullyQualifiedName)
	{
		string name = MethodBase.GetCurrentMethod().Name;
		Type type = Type.GetType(fullyQualifiedName);
		SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> The '{2}' type is resolved.", "SqlConfigurableRetryLogicLoader", name, type?.FullName);
		if (!(type != null))
		{
			return typeof(SqlConfigurableRetryFactory);
		}
		return type;
	}
}
