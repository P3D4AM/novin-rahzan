using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;

namespace Server.Core;

public interface IDalDepartmentEntitySummery
{
	EntityPart Part { get; }

	int ServerCount { get; }

	int ClientCount { get; }

	bool IsSame { get; }

	Task<WebDepartmentEntitySummery> GetAsync(short depId);

	List<IDalDepartmentEntitySummery> Compare(WebDepartmentEntitySummery serverData, WebDepartmentEntitySummery clientData);

	string GetMessage();
}
