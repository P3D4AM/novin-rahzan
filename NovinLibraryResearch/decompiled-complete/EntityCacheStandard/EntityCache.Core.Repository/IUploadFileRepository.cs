using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IUploadFileRepository : IRepository<UploadFileBusiness>, INoGuidRepository<UploadFileBusiness>, IDisposable
{
	Task<UploadFileBusiness> GetAsync(long crc, CancellationToken token, string tranName);

	Task<UploadFileBusiness> GetByFileIdAsync(Guid fileid, CancellationToken token, string tranName);

	Task<bool> IsSentCompleteAsync(Guid fileId, CancellationToken token, string tranName);
}
