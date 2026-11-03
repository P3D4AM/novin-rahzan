using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface IFileInfoRepository : ISyncRepository<FileInfoBusiness>, IEditableDelivery, IRepository<FileInfoBusiness>, INoGuidRepository<FileInfoBusiness>, IDisposable
{
	Task<FileInfoBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranName);

	Task<List<FileInfoBusiness>> GetAsync(Guid entityGuid, FileManagement_Parts part, bool justFromCache, CancellationToken token, string tranName);

	Task<List<FileInfoBusiness>> GetAllCompressedImageAsync(CancellationToken token, string tranName);

	Task<List<FileInfoBusiness>> GetWrongExtentionsAsync(CancellationToken token, string tranName);

	Task<FileInfoBusiness> GetNoImageAsync(FileManagement_Parts part, CancellationToken token, string tranName);

	Task<List<Guid>> GetAllGuidListAsync(FileInfoFilter filter, CancellationToken token, string tranName);
}
