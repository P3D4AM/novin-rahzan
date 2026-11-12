using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalNews
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short DepId, List<WebNews> lst);

	Task<WebNews> LoadDataAsync(short DepId, int Id);

	Task<List<WebNews>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);
}
