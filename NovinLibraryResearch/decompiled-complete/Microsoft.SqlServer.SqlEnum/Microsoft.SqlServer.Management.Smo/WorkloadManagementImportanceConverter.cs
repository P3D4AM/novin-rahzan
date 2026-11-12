namespace Microsoft.SqlServer.Management.Smo;

public class WorkloadManagementImportanceConverter : EnumToDisplayNameConverter
{
	public WorkloadManagementImportanceConverter()
		: base(typeof(WorkloadManagementImportance))
	{
	}
}
