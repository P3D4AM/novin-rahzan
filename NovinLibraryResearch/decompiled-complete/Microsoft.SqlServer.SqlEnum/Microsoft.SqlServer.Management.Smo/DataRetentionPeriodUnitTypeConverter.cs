namespace Microsoft.SqlServer.Management.Smo;

public class DataRetentionPeriodUnitTypeConverter : EnumToDisplayNameConverter
{
	public DataRetentionPeriodUnitTypeConverter()
		: base(typeof(DataRetentionPeriodUnit))
	{
	}
}
