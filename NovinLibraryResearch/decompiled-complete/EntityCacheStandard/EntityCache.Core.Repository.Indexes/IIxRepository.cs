using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Indexes;

public interface IIxRepository : IDisposable
{
	WebContextHolder Context { get; }

	string ContextGuid { get; }

	Task<ReturnedSaveFuncInfo> SaveListAsync(Dictionary<Guid, HashSet<int>> lst, string tranName, CancellationToken token);
}
