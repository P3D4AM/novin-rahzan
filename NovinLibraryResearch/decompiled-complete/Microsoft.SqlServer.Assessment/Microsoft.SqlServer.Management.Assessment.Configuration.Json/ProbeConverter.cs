using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ProbeConverter : JsonConverter<IProbe>
{
	public const string TypePropertyName = "type";

	public const string ImplementationPropertyName = "implementation";

	public const string ClassPropertyName = "class";

	public const string AssemblyPropertyName = "assembly";

	public const string SqlProbeTypeSign = "SQL";

	public const string ClrProbeTypeSign = "CLR";

	public const string ExternalProbeTypeSign = "External";

	public const string CmdShellProbeTypeSign = "CmdShell";

	public const string PowerShellProbeTypeSign = "PowerShell";

	public const string WmiProbeTypeSign = "WMI";

	public const string KustoProbeTypeSign = "Kusto";

	public const string RegistryProbeTypeSign = "Registry";

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, IProbe value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "Probe"));
	}

	public override IProbe ReadJson(JsonReader reader, Type objectType, IProbe existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);
		if (!jObject.TryGetValue("type", StringComparison.OrdinalIgnoreCase, out JToken value))
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, "type");
		}
		if (value.Type != JTokenType.String)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongToken, value.Type, JTokenType.String);
		}
		return value.Value<string>() switch
		{
			"SQL" => ReadStandardProbe<SqlProbe>(jObject, reader, serializer), 
			"CLR" => ReadClrProbe(jObject, reader, serializer), 
			"External" => ReadExternalProbe(jObject, reader, serializer), 
			"CmdShell" => ReadStandardProbe<CmdShellProbe>(jObject, reader, serializer), 
			"PowerShell" => ReadStandardProbe<PowerShellProbe>(jObject, reader, serializer), 
			"WMI" => ReadStandardProbe<WmiProbe>(jObject, reader, serializer), 
			"Kusto" => ReadStandardProbe<KustoProbe>(jObject, reader, serializer), 
			"Registry" => ReadStandardProbe<RegistryProbe>(jObject, reader, serializer), 
			_ => throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongValue, value.Value<string>(), "probe type"), 
		};
	}

	private static SqlAssessmentJsonException CreateAssemblyLoadException(string assemblyString, JsonReader reader, Exception innerException)
	{
		return SqlAssessmentJsonException.Create(reader, Resources.ErrorLoadingAssembly, assemblyString, innerException.Message, innerException);
	}

	private static Assembly LoadAssembly(string assemblyString, JsonReader reader)
	{
		if (assemblyString == null)
		{
			throw new ArgumentNullException("assemblyString");
		}
		try
		{
			return Assembly.Load(assemblyString);
		}
		catch (FileNotFoundException)
		{
			return LoadAssemblyFrom(assemblyString, reader);
		}
		catch (FileLoadException)
		{
			return LoadAssemblyFrom(assemblyString, reader);
		}
		catch (ArgumentException innerException)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException);
		}
		catch (BadImageFormatException innerException2)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException2);
		}
	}

	private static Assembly LoadAssemblyFrom(string assemblyString, JsonReader reader)
	{
		try
		{
			return Assembly.LoadFrom(assemblyString);
		}
		catch (IOException innerException)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException);
		}
		catch (BadImageFormatException innerException2)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException2);
		}
		catch (SecurityException innerException3)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException3);
		}
		catch (ArgumentException innerException4)
		{
			throw CreateAssemblyLoadException(assemblyString, reader, innerException4);
		}
	}

	private static ExternalProbe ReadExternalProbe(JObject rep, JsonReader reader, JsonSerializer serializer)
	{
		JToken propertyObject = SerializationUtil.GetPropertyObject(rep, "implementation", JTokenType.Object, reader);
		string propertyValue = SerializationUtil.GetPropertyValue<string>(propertyObject, "class", reader, allowNull: false, new JTokenType[1] { JTokenType.String });
		string propertyValue2 = SerializationUtil.GetPropertyValue<string>(propertyObject, "assembly", reader, allowNull: true, new JTokenType[1] { JTokenType.String });
		Assembly assembly = (string.IsNullOrWhiteSpace(propertyValue2) ? Assembly.GetExecutingAssembly() : LoadAssembly(propertyValue2, reader));
		Type type = assembly.GetType(propertyValue, throwOnError: true, ignoreCase: true);
		ExternalProbe externalProbe = new ExternalProbe
		{
			ImplementationType = type,
			Configuration = propertyObject.ToString()
		};
		serializer.Populate(rep.CreateReader(), externalProbe);
		return externalProbe;
	}

	private static IProbe ReadClrProbe(JObject rep, JsonReader reader, JsonSerializer serializer)
	{
		JToken propertyObject = SerializationUtil.GetPropertyObject(rep, "implementation", JTokenType.Object, reader);
		string propertyValue = SerializationUtil.GetPropertyValue<string>(propertyObject, "class", reader, allowNull: false, new JTokenType[1] { JTokenType.String });
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		Type type = executingAssembly.GetType(propertyValue, throwOnError: true, ignoreCase: true);
		IProbe probe = Activator.CreateInstance(type) as IProbe;
		serializer.Populate(rep.CreateReader(), probe);
		serializer.Populate(propertyObject.CreateReader(), probe);
		return probe;
	}

	private static TProbe ReadStandardProbe<TProbe>(JObject rep, JsonReader reader, JsonSerializer serializer) where TProbe : class, IProbe, new()
	{
		TProbe val = new TProbe();
		serializer.Populate(rep.CreateReader(), val);
		JToken propertyObject = SerializationUtil.GetPropertyObject(rep, "implementation", JTokenType.Object, reader);
		serializer.Populate(propertyObject.CreateReader(), val);
		return val;
	}
}
