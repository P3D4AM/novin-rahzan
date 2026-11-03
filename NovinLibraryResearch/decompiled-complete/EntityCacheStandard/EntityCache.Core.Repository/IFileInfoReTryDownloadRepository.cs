using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface IFileInfoReTryDownloadRepository : IRepository<FileInfoReTryDownloadBusiness>, INoGuidRepository<FileInfoReTryDownloadBusiness>, IDisposable
{
	Task<List<FileInfoReTryDownloadBusiness>> GetFilesByPartAsync(FileManagement_Parts part, CancellationToken token, string tranName);

	Task<FileInfoReTryDownloadBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranNme);

	Task<List<FileInfoReTryDownloadBusiness>> GetAllForDownloadAsync(short tryCount, CancellationToken token, string tranName);
}
