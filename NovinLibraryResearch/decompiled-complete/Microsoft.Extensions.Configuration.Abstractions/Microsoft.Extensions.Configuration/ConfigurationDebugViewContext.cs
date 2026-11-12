namespace Microsoft.Extensions.Configuration;

public readonly struct ConfigurationDebugViewContext
{
	public string Path { get; }

	public string Key { get; }

	public string? Value { get; }

	public IConfigurationProvider ConfigurationProvider { get; }

	public ConfigurationDebugViewContext(string path, string key, string? value, IConfigurationProvider configurationProvider)
	{
		Path = path;
		Key = key;
		Value = value;
		ConfigurationProvider = configurationProvider;
	}
}
