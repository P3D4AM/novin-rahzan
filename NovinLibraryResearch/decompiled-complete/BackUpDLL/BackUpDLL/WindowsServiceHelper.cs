using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace BackUpDLL;

internal class WindowsServiceHelper
{
	[StructLayout(LayoutKind.Sequential)]
	private class SERVICE_STATUS
	{
		public int dwServiceType;

		public ServiceState dwCurrentState;

		public int dwControlsAccepted;

		public int dwWin32ExitCode;

		public int dwServiceSpecificExitCode;

		public int dwCheckPoint;

		public int dwWaitHint;
	}

	private const int STANDARD_RIGHTS_REQUIRED = 983040;

	private const int SERVICE_WIN32_OWN_PROCESS = 16;

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "OpenSCManagerW", ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr OpenSCManager(string machineName, string databaseName, ScmAccessRights dwDesiredAccess);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lp, string lpPassword);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool CloseServiceHandle(IntPtr hSCObject);

	[DllImport("advapi32.dll")]
	private static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool DeleteService(IntPtr hService);

	[DllImport("advapi32.dll")]
	private static extern int ControlService(IntPtr hService, ServiceControl dwControl, SERVICE_STATUS lpServiceStatus);

	[DllImport("advapi32.dll", SetLastError = true)]
	private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);

	public static void Uninstall(string serviceName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.AllAccess);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.AllAccess);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new ApplicationException("Service not installed.");
			}
			try
			{
				StopService(intPtr2);
				if (!DeleteService(intPtr2))
				{
					throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				CloseServiceHandle(intPtr2);
			}
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	public static bool ServiceIsInstalled(string serviceName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.Connect);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.QueryStatus);
			if (intPtr2 == IntPtr.Zero)
			{
				return false;
			}
			CloseServiceHandle(intPtr2);
			return true;
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	public static void InstallAndStart(string serviceName, string displayName, string fileName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.AllAccess);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.AllAccess);
			if (intPtr2 == IntPtr.Zero)
			{
				intPtr2 = CreateService(intPtr, serviceName, displayName, ServiceAccessRights.AllAccess, 16, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);
			}
			if (intPtr2 == IntPtr.Zero)
			{
				throw new ApplicationException("Failed to install service.");
			}
			try
			{
				StartService(intPtr2);
			}
			finally
			{
				CloseServiceHandle(intPtr2);
			}
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	public static void StartService(string serviceName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.Connect);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new ApplicationException("Could not open service.");
			}
			try
			{
				StartService(intPtr2);
			}
			finally
			{
				CloseServiceHandle(intPtr2);
			}
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	public static void StopService(string serviceName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.Connect);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
			if (intPtr2 == IntPtr.Zero)
			{
				throw new ApplicationException("Could not open service.");
			}
			try
			{
				StopService(intPtr2);
			}
			finally
			{
				CloseServiceHandle(intPtr2);
			}
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	private static void StartService(IntPtr service)
	{
		new SERVICE_STATUS();
		StartService(service, 0, 0);
		if (!WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running))
		{
			throw new ApplicationException("Unable to start service");
		}
	}

	private static void StopService(IntPtr service)
	{
		SERVICE_STATUS lpServiceStatus = new SERVICE_STATUS();
		ControlService(service, ServiceControl.Stop, lpServiceStatus);
		if (!WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped))
		{
			throw new ApplicationException("Unable to stop service");
		}
	}

	public static ServiceState GetServiceStatus(string serviceName)
	{
		IntPtr intPtr = OpenSCManager(ScmAccessRights.Connect);
		try
		{
			IntPtr intPtr2 = OpenService(intPtr, serviceName, ServiceAccessRights.QueryStatus);
			if (intPtr2 == IntPtr.Zero)
			{
				return ServiceState.NotFound;
			}
			try
			{
				return GetServiceStatus(intPtr2);
			}
			finally
			{
				CloseServiceHandle(intPtr2);
			}
		}
		finally
		{
			CloseServiceHandle(intPtr);
		}
	}

	private static ServiceState GetServiceStatus(IntPtr service)
	{
		SERVICE_STATUS sERVICE_STATUS = new SERVICE_STATUS();
		if (QueryServiceStatus(service, sERVICE_STATUS) == 0)
		{
			throw new ApplicationException("Failed to query service status.");
		}
		return sERVICE_STATUS.dwCurrentState;
	}

	private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
	{
		SERVICE_STATUS sERVICE_STATUS = new SERVICE_STATUS();
		QueryServiceStatus(service, sERVICE_STATUS);
		if (sERVICE_STATUS.dwCurrentState == desiredStatus)
		{
			return true;
		}
		int tickCount = Environment.TickCount;
		int dwCheckPoint = sERVICE_STATUS.dwCheckPoint;
		while (sERVICE_STATUS.dwCurrentState == waitStatus)
		{
			int num = sERVICE_STATUS.dwWaitHint / 10;
			if (num < 1000)
			{
				num = 1000;
			}
			else if (num > 10000)
			{
				num = 10000;
			}
			Thread.Sleep(num);
			if (QueryServiceStatus(service, sERVICE_STATUS) == 0)
			{
				break;
			}
			if (sERVICE_STATUS.dwCheckPoint > dwCheckPoint)
			{
				tickCount = Environment.TickCount;
				dwCheckPoint = sERVICE_STATUS.dwCheckPoint;
			}
			else if (Environment.TickCount - tickCount > sERVICE_STATUS.dwWaitHint)
			{
				break;
			}
		}
		return sERVICE_STATUS.dwCurrentState == desiredStatus;
	}

	private static IntPtr OpenSCManager(ScmAccessRights rights)
	{
		IntPtr intPtr = OpenSCManager(null, null, rights);
		if (intPtr == IntPtr.Zero)
		{
			throw new ApplicationException("Could not connect to service control manager.");
		}
		return intPtr;
	}
}
