using System.Security.Principal;

namespace TimeAttendanceDAL.Utility.Settings;

public class IsRunAdministrator
{
	public static bool IsAdministrator()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole((WindowsBuiltInRole)544);
	}
}
