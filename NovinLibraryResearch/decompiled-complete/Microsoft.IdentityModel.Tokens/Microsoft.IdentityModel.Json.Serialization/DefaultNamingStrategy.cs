namespace Microsoft.IdentityModel.Json.Serialization;

internal class DefaultNamingStrategy : NamingStrategy
{
	protected override string ResolvePropertyName(string name)
	{
		return name;
	}
}
