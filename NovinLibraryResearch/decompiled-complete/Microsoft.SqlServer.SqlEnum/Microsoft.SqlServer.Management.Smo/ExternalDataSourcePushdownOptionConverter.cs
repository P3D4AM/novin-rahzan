namespace Microsoft.SqlServer.Management.Smo;

public class ExternalDataSourcePushdownOptionConverter : EnumToDisplayNameConverter
{
	public ExternalDataSourcePushdownOptionConverter()
		: base(typeof(ExternalDataSourcePushdownOption))
	{
	}
}
