using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

[JsonObject(MemberSerialization.OptIn)]
public class SqlObjectLocatorPattern : IPattern<ISqlObjectLocator>, IEquatable<SqlObjectLocatorPattern>
{
	internal const string VersionPropertyName = "version";

	internal const string PlatformPropertyName = "platform";

	internal const string TypePropertyName = "type";

	internal const string NamePropertyName = "name";

	internal const string EditionPropertyName = "engineEdition";

	internal const string ServerNamePropertyName = "serverName";

	internal const string VmTypePropertyName = "machineType";

	[JsonProperty("version")]
	public RangeList<Version> Version { get; }

	[JsonProperty("platform")]
	public IPattern<string> Platform { get; }

	[JsonProperty("type")]
	public SqlObjectType ObjectType { get; }

	[JsonProperty("name")]
	public IPattern<string> NamePattern { get; }

	[JsonProperty("serverName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public IPattern<string> ServerNamePattern { get; }

	[JsonProperty("machineType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public IPattern<string> MachineTypePattern { get; }

	[JsonProperty("engineEdition", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(StringEnumConverter))]
	public SqlEngineEdition EngineEdition { get; }

	[JsonConstructor]
	public SqlObjectLocatorPattern(RangeList<Version> version = null, IPattern<string> platform = null, SqlObjectType objectType = SqlObjectType.None, IPattern<string> namePattern = null, SqlEngineEdition engineEdition = SqlEngineEdition.None, IPattern<string> serverNamePattern = null, IPattern<string> machineTypePattern = null)
	{
		Version = version;
		Platform = platform;
		ObjectType = objectType;
		NamePattern = namePattern;
		EngineEdition = engineEdition;
		ServerNamePattern = serverNamePattern;
		MachineTypePattern = machineTypePattern;
	}

	public bool IsMatch(ISqlObjectLocator value)
	{
		if (value == null)
		{
			return false;
		}
		bool flag = ObjectType == SqlObjectType.None || (ObjectType & value.Type) == value.Type;
		bool flag2 = Platform == null || Platform.IsMatch(value.Platform ?? string.Empty);
		bool flag3 = NamePattern == null || NamePattern.IsMatch(value.Name ?? string.Empty);
		bool flag4 = Version == null || Version.IsMatch(value.Version);
		bool flag5 = EngineEdition == SqlEngineEdition.None || (EngineEdition & value.EngineEdition) == value.EngineEdition;
		bool flag6 = ServerNamePattern == null || ServerNamePattern.IsMatch(value.ServerName);
		bool flag7 = MachineTypePattern == null || MachineTypePattern.IsMatch(value.MachineType);
		return flag2 && flag3 && flag && flag4 && flag5 && flag6 && flag7;
	}

	public bool Equals(SqlObjectLocatorPattern other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(Platform, other.Platform) && object.Equals(Version, other.Version) && object.Equals(NamePattern, other.NamePattern) && ObjectType == other.ObjectType && EngineEdition == other.EngineEdition && object.Equals(ServerNamePattern, other.ServerNamePattern))
		{
			return object.Equals(MachineTypePattern, other.MachineTypePattern);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((SqlObjectLocatorPattern)obj);
	}

	public override int GetHashCode()
	{
		int num = ((Platform != null) ? Platform.GetHashCode() : 0);
		num = (num * 397) ^ ((Version != null) ? Version.GetHashCode() : 0);
		num = (num * 397) ^ ((NamePattern != null) ? NamePattern.GetHashCode() : 0);
		num = (num * 397) ^ (int)ObjectType;
		num = (num * 397) ^ (int)EngineEdition;
		num = (num * 397) ^ ((ServerNamePattern != null) ? ServerNamePattern.GetHashCode() : 0);
		return (num * 397) ^ ((MachineTypePattern != null) ? MachineTypePattern.GetHashCode() : 0);
	}
}
