using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Interfaces;

public interface IFileAttachment : IHasJustGuid
{
	FileManagement_Parts Part { get; }

	IReadOnlyCollection<FileInfoBusiness> AddedFiles { get; }

	IReadOnlyCollection<FileInfoBusiness> RemovedFiles { get; }

	Task AddFileAsync(FileInfoBusiness file);

	Task RemoveFileAsync(FileInfoBusiness file);
}
