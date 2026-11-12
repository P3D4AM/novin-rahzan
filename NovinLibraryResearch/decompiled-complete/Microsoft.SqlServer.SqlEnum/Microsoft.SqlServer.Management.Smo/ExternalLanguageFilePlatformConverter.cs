namespace Microsoft.SqlServer.Management.Smo;

public class ExternalLanguageFilePlatformConverter : EnumToDisplayNameConverter
{
	public ExternalLanguageFilePlatformConverter()
		: base(typeof(ExternalLanguageFilePlatform))
	{
	}
}
