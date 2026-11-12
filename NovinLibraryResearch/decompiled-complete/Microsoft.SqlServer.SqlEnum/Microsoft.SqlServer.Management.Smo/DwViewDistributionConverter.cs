namespace Microsoft.SqlServer.Management.Smo;

public class DwViewDistributionConverter : EnumToDisplayNameConverter
{
	public DwViewDistributionConverter()
		: base(typeof(DwViewDistributionType))
	{
	}
}
