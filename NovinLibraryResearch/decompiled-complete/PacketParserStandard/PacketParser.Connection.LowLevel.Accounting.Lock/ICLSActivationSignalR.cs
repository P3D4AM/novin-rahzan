using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface ICLSActivationSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<string>> GetClientNetNumberAsync(Package package);

	Task<ReturnedSaveFuncInfo> SetActiveDateAsync(DateTime value, Package package);

	Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> ApplicationTypeAsync(bool ignoreHttpExceptions);

	Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> SourceTypeAsync(string SoftwareSerial, string HardwareSerial, string ActivationCode);

	Task<ReturnedSaveFuncInfoWithValue<Level_Hozor>> ApplicationHozorTypeAsync(bool ignoreHttpExceptions);

	Task<ReturnedSaveFuncInfoWithValue<string>> RemoveScaleNoAndDaysAsync(string ActivationCode);

	Task<ReturnedSaveFuncInfoWithValue<string>> RemoveDaysAsync(string activationCode, Package package);

	Task<ReturnedSaveFuncInfoWithValue<long>> GetLKSerialAsync(Package package);

	Task<ReturnedSaveFuncInfo> SetLKSerialAsync(Package package, long value, bool ignoreHttpExceptions);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingActivationCodeAsync();

	Task<ReturnedSaveFuncInfo> SetAccountingActivationCodeAsync(string value, Package package);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetTolidActivationCodeAsync();

	Task<ReturnedSaveFuncInfo> SetTolidActivationCodeAsync(string value);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetBarCodeActivationCodeAsync();

	Task<ReturnedSaveFuncInfo> SetBarCodeActivationCodeAsync(string value);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetScaleActivationCodeAsync();

	Task<ReturnedSaveFuncInfo> SetScaleActivationCodeAsync(string value);

	Task<ReturnedSaveFuncInfo> SetCallerIDActivationCodeAsync(string value, Package package);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetSpecialActivationCodeAsync();

	Task<ReturnedSaveFuncInfo> SetSpecialActivationCodeAsync(string value);

	Task<ReturnedSaveFuncInfo> SetHoghoghActivationCodeAsync(string value, Package package);
}
