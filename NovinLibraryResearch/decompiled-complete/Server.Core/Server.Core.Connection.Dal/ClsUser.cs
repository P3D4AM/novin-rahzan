using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal;

public class ClsUser : IEntity<WebUser>
{
	public Task<List<WebUser>> ApplyAccessAsync(IEnumerable<WebUser> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebUser>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}
}
