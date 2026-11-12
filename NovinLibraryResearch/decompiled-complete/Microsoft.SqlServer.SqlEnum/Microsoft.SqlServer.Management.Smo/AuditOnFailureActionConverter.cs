namespace Microsoft.SqlServer.Management.Smo;

public class AuditOnFailureActionConverter : EnumToDisplayNameConverter
{
	public AuditOnFailureActionConverter()
		: base(typeof(OnFailureAction))
	{
	}
}
