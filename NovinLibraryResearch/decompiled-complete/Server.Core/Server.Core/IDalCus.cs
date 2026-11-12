using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalCus
{
	Task<List<SaveResponse>> SaveAsync(string sender, ConnectionData connection, IEnumerable<WebCus> lst);

	Task<List<SaveResponse>> SaveAsync(string sender, short depId, long hashId, IEnumerable<WebCus> lst, ENSource source, CancellationToken token);

	Task<WebCus> LoadDataAsync(short depId, int id);

	Task<WebCus> LoadDataAsync(short depId, Guid guid);

	Task<WebCus> LoadDataAsync(short depId, string userName, string mobileNumber = "", string email = "", bool? status = null);

	Task<List<WebCus>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken);

	Task<List<WebCus>> GetAllAsync(short depId, long hashId, int partition, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfoWithValue<List<int>>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<CusTmpCmtRsp> IsUniqueAsync(string user, string mobile, string email, short depId);

	Task<int> CountAsync(short depId, CancellationToken cancellationToken);

	Task<WebCus> GetAsync(short depId, Guid guid);

	Task<List<WebCus>> SearchAsync(short depId, CancellationToken token, Dictionary<string, string> filters, Dictionary<string, string> orderBy);

	Task<List<WebCus>> GetAllAsync(short depId, string name, string family, CancellationToken token);
}
