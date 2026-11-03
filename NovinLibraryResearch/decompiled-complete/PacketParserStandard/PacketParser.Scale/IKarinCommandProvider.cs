using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale.Entities;
using PacketParser.Scale.EventArguments;
using PacketParser.Scale.Provider;

namespace PacketParser.Scale;

public interface IKarinCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
	Task EnableAccessPassword(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task<int> FindFirstBlankMemory(int scaleNumber, int startIndex, CancellationToken token = default(CancellationToken));

	Task<string> ReadAccessPassword(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task<string> ReadBarcodeMemory(int scaleNumber, int memoryIndex, CancellationToken token = default(CancellationToken));

	Task<DateTime> ReadRtc(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task SetFetchStatus(int scaleNumber, int receiptIndex, FetchStatus status, CancellationToken token = default(CancellationToken));

	Task WriteAccessPassword(int scaleNumber, string password, CancellationToken token = default(CancellationToken));

	Task WriteBarcodeMemory(int scaleNumber, int memoryIndex, string barcode, CancellationToken token = default(CancellationToken));

	Task WriteRtc(int scaleNumber, DateTime miladi, CancellationToken token = default(CancellationToken));

	Task DisableAccessPassword(int scaleNumber, CancellationToken token = default(CancellationToken));

	new Task<ReturnedSaveFuncInfoWithValue<WeightEventArgs>> ReadWeightByRequestAsync(int scaleNumber, CancellationToken token = default(CancellationToken));
}
