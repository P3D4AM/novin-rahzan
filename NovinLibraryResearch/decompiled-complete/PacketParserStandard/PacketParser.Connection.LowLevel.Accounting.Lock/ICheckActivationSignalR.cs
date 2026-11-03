using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface ICheckActivationSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfo> CheckLKWithWebServiceAsync(Package package, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> SetActivationCodeAsync(string value, Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationCodeAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> RemoveScaleNoAndDaysAsync(string activationCode, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> SetRemoveDaysAsync(string activationCode, Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<Level_Hozor>> GetHozorTypeAsync(string softwareSerial, string hardwareSerial, string activationCode, bool isDemo, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> GetApplicationTypeAsync(string softwareSerial, string hardwareSerial, string activationCode, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<DateTime>> GetActiveDateAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetClientSetAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<short>> GetNumberOfDaysAsync(Package package, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetScaleNoAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetLkSerialAsync(string version, Package package, long value, bool isDemo, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetHoghoghActivationCodeAsync(Package package, string value, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetActivateDateAsync(DateTime value, Package package, CancellationToken token);
}
