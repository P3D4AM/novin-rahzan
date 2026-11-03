using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface IActivationCodeSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<string>> ReturnActiveCodeAsync(string activationCode, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingSadeActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingMotevasetActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingPishrafteActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingJameActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingSpecialActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetTamdidActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetAttendanceActivationCodeAsync(short level, string softwareSerial, string lkSerial, HozoorPackage pack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel1(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel2(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel3(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel4(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationAttendanceLevel5(string softwareSerial, string lkSerial, HozoorPackage package, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token);
}
