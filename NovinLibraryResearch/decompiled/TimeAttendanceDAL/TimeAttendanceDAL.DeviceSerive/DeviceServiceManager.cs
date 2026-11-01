using System;
using System.Threading.Tasks;
using FormCustom.ServiceManagement;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace TimeAttendanceDAL.DeviceSerive;

public class DeviceServiceManager : ServiceBaseManager
{
	public override string ServiceName => "NpCommunicationDeviceService";

	public override string ExeName => "NpCommunicationDeviceService";

	public override string HubName => "BioMetricServerCore";

	public DeviceServiceManager(string contextGuid)
		: base(contextGuid)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> InstallServiceAsync(string sqlConnectionString, string signalRHubBaseAddress, int signalRHubPortNumber)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo val = res;
			val.AddReturnedValue(await ((ServiceBaseManager)this).Manager.InstallServiceAsync(signalRHubBaseAddress, signalRHubPortNumber, sqlConnectionString), (ReturnedState)0, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 23);
			val = res;
			val.AddReturnedValue(await ((ServiceBaseManager)this).Manager.SetFailureConfigAsync(), (ReturnedState)0, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 24);
			if (!res.HasError)
			{
				EnvironmentManager<string>.SetValue(((ServiceBaseManager)this).Manager.ServiceName, ((ServiceBaseManager)this).GetServicePath());
				val = res;
				val.AddReturnedValue(await ((ServiceBaseManager)this).CreateScheduledTaskAsync("AutoRestartDeviceService"), (ReturnedState)0, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 28);
				res.AddInformation("سرویس ارتباط با دستگاه حضور و غیاب با موفقیت نصب شد.", "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 29);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 34);
			res.AddReturnedValue(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\DeviceSerive\\DeviceServiceManager.cs", 35);
		}
		return res;
	}
}
