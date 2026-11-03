using System;
using System.Security.Principal;
using PacketParser.Entities;

namespace FormCustom.Assistance;

public class IsRunAdministrator
{
	public static bool IsAdministrator()
	{
		try
		{
			return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsAdministrator", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\IsRuncAdministrator.cs", 20);
			return false;
		}
	}
}
