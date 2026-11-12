using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalFileInfo
{
	Task<List<SaveResponse>> SaveAsync(short DepId, List<WebFileInfo> lst, ENSource? Source, CancellationToken token, short tryCount = 10);

	Task<ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>> DeleteFromServerAsync(List<WebFileInfo> lst, short DepId, short tryCount = 10);

	Task<List<WebFileInfo>> GetAllAsync(short DepId, ENSource Source, DateTime Modified, CancellationToken cancellationToken, bool loadFileContents);

	Task<ReturnedSaveFuncInfoWithValue<string>> FileCreateAsync(FileManagement_Parts Part, ENSource source, string FileName, byte[] Content, short DepId, short tryCount = 10);

	Task<WebFileInfo> CommitAsync(FileCommitRequestEventArgs e, short tryCount = 10);

	Task<List<WebFileInfo>> GetCustomerPicsAsync(short DepId, Guid CustomerTafsilGuid, DateTime Modified, CancellationToken cancellationToken, bool loadFileContents);

	Task<List<WebFileInfo>> GetProductPicsAsync(short DepId, Guid productGuid, CancellationToken cancellationToken, bool loadFileContents);

	Task<List<WebFileInfo>> GetProductGroupPicsAsync(short DepId, Guid productGuid, CancellationToken cancellationToken, bool loadFileContents);

	Task<string> GenerateNewLinkAsync(short depId, FileManagement_Parts Part, string FileName, short tryCount = 10);

	Task UpdateAllFilesCrcAsync();

	Task<ReturnedSaveFuncInfo> DeleteTempsAsync();

	Task<List<WebFileInfo>> GetAllAsync(short DepId, long HashId, int Partition, CancellationToken cancellationToken, bool loadFileContents);

	Task<List<int>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);

	Task FillUrlAsync();

	Task<string> SavePhysicalFileForNewClientsAsync(short depId, WebFileInfo item);
}
