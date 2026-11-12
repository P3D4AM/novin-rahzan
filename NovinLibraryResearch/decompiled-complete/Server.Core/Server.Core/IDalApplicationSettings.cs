using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalApplicationSettings
{
	Task<bool> SaveAsync(short DepId, List<WebApplicationSetting> lst, CancellationToken token);

	Task<WebApplicationSetting> LoadDataAsync(short DepId, string Name);

	Task<List<WebApplicationSetting>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken = default(CancellationToken));
}
