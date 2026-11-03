using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale.Entities;
using PacketParser.Scale.EventArguments;

namespace PacketParser.Scale.Provider;

public interface IScaleCommandProvider : INpDisposable, IDisposable
{
	Task ClearMemory(int scaleNumber, uint index, CancellationToken token = default(CancellationToken));

	Task Ping(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task<FactorModel> ReadLastUnfetchedReciept(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task<ProductModel> ReadPluMemory(int scaleNumber, int memoryIndex, CancellationToken token = default(CancellationToken));

	Task<List<ProductModel>> ReadPluMemoryBatch(int scaleNumber, int memoryIndex, int length, CancellationToken token = default(CancellationToken));

	Task<FactorModel> ReadReceipt(int scaleNumber, int receiptIndex, CancellationToken token = default(CancellationToken));

	Task<List<FactorModel>> ReadReceiptBatch(int scaleNumber, int startIndex, int length, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> WritePluMemoryAsync(int scaleNumber, int memoryIndex, PluWritableMemory content, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> WritePluMemoryBatchAsync(int scaleNumber, IEnumerable<uint> memoryIndices, IEnumerable<PluWritableMemory> contents, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> ChangeReadWeightStatusAsync(int scaleNumber, EnReadWeightType type, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfoWithValue<WeightEventArgs>> ReadWeightByRequestAsync(int scaleNumber, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> TareAsync(MicroScaleInfo scaleInfo, CancellationToken token);
}
