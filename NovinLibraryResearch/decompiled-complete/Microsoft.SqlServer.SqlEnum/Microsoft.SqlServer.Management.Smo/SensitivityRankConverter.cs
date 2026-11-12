namespace Microsoft.SqlServer.Management.Smo;

public class SensitivityRankConverter : EnumToDisplayNameConverter
{
	public SensitivityRankConverter()
		: base(typeof(SensitivityRank))
	{
	}
}
