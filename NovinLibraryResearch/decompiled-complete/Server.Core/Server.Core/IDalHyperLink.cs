using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalHyperLink
{
	void Save(short depId, IEnumerable<WebHyperLink> lst);

	WebHyperLink LoadData(short depId, int id);

	Task<IEnumerable<WebHyperLink>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken);
}
