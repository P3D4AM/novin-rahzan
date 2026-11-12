using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal;

public class ClsUserInFormation : IEntity<WebUserInformation>
{
	public Task<List<WebUserInformation>> ApplyAccessAsync(IEnumerable<WebUserInformation> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebUserInformation>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}
}
