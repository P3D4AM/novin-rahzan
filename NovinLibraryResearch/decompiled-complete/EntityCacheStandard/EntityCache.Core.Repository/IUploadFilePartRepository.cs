using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface IUploadFilePartRepository : IRepository<UploadFilePartBusiness>, INoGuidRepository<UploadFilePartBusiness>, IDisposable
{
	Task<IEnumerable<UploadFilePartBusiness>> GetLastFilePartAsync(int top, bool? status, CancellationToken token, string tranName);

	Task ResetSentAsync(Guid fileId, short tryCount, CancellationToken token, string tranName);

	Task<int> GetTotalPartCountAsync(Guid fileId, CancellationToken token, string tranName);

	Task<int> GetPartSentCountAsync(Guid fileId, CancellationToken token, string tranName);

	Task<List<string>> GetFileOrderAsync(Guid fileId, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid fileid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSentAsync(Guid filePart, short tryCount, CancellationToken token, string tranName);

	Task<Guid> GetFileIdAsync(Guid filePart, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(UploadFilePartBusiness item, byte[] bytes, short tryCount, CancellationToken token, string tranName);

	Task<byte[]> GetBytesAsync(Guid guid, CancellationToken token, string tranName);

	Task SetRetryAsync(DateTime addMinutes, Guid guid, CancellationToken token, string tranName);
}
