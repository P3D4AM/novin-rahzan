using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalHesabTree
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short depId, IEnumerable<WebHesabTree> lst, CancellationToken token, long hashId, ENSource source);

	Task<WebHesabTree> LoadDataAsync(short depId, int id, bool removeGardesh);

	List<WebHesabTree> GetAll(short depId, DateTime modified, CancellationToken cancellationToken, bool removeGardesh);

	Task<List<WebHesabTree>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken, bool removeGardesh);

	Task<List<WebHesabTreeCompact>> GetAllCompact(short depId, HesabType Htype, DateTime modified, CancellationToken cancellationToken);

	Task<List<WebHesabTree>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken, bool removeGardesh);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<WebHesabTree> LoadDataAsync(short depId, Guid guid, bool removeGardesh);

	Task<WebTafsil> LoadDataTafsilAsync(short depId, Guid guid, bool removeGardesh);

	Task<WebMoein> LoadDataMoeinAsync(short depId, Guid guid, bool removeGardesh);

	Task<WebKol> LoadDataKolAsync(short depId, Guid guid, bool removeGardesh);

	Task<WebHesabTree> LoadDataAsync(short depId, HesabType hType, int hId, bool removeGardesh);

	Task<bool> DeleteByDepIdAsync(short depId);

	void SendNotification(short departmentId, PacketType type, PacketAction action, List<int> ids, List<Guid> guids);

	Task DeserializeFirstSampleDataAsync();

	Task<WebHesabTree> GetAsync(short depId, string customerUserName, string customerPassword, bool removeGardesh);
}
