using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal;

public class ClsDepartment : IEntity<WebDepartment>
{
	public Task<List<WebDepartment>> ApplyAccessAsync(IEnumerable<WebDepartment> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebDepartment>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}
}
