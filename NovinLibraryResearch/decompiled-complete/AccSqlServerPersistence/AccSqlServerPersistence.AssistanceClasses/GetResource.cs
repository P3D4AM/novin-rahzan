using AccSqlServerPersistence.Properties;

namespace AccSqlServerPersistence.AssistanceClasses;

public class GetResource
{
	public static string GetGlobalResource => Resources.Global_sp;

	public static string GetAccountingResource => Resources.Acc_sp;

	public static string GetAdvertiseResource => Resources.Advertise_sp;

	public static string GetAttendanceResource => Resources.Attendance_sp;
}
