using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Services;

namespace FormCustom.ServiceManagement;

public interface IServiceManager : IHasContext
{
	ServiceStatus LastServiceStatus { get; }

	NssmServiceManager Manager { get; }

	event Func<object, ServiceStatusChangesEventArgument, Task> OnStatusChanges;

	Task<ReturnedSaveFuncInfo> InstallServiceAsync(string sqlConnectionString, string signalRHubBaseAddress, int signalRHubPortNumber);

	Task<ReturnedSaveFuncInfo> UnInstallServiceAsync();

	Task<ReturnedSaveFuncInfo> StartServiceAsync();

	Task<ReturnedSaveFuncInfo> StopServiceAsync();

	Task<ReturnedSaveFuncInfo> RestartServiceAsync();

	Task<ReturnedSaveFuncInfo> CreateScheduledTaskAsync(string taskName);

	ServiceStatus CheckService();
}
