using System;
using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Entities.Interfaces;

public interface IHasSanadStatusReport
{
	Document AsType { get; }

	short StatusId { get; set; }

	EntityChangeSt ChangeStatus { get; set; }

	Guid CashierGuid { get; }

	Task<int> NextId2Async(short? statusId, CancellationToken token, string tranName);

	void SetId2(int value, CancellationToken token);
}
