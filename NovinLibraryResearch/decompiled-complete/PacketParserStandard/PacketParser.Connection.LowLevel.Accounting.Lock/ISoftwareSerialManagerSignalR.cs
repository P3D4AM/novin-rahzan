using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace PacketParser.Connection.LowLevel.Accounting.Lock;

public interface ISoftwareSerialManagerSignalR : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerial_HozorAsync(string version, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerial_AccountingAsync(string version, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetSoftwareSerial_HozorAsync(string value, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<AccountingPackageClass>> SetAccountingSoftwareSerial(string softwareSerial, string activationCode, List<PackageContainer> packages, CancellationToken token);
}
