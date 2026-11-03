using System;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace FormCustom.CallCenter;

public interface ICallCenterManager
{
	Task ShowChequeListAsync();

	Task ShowQestListAsync(Guid tafsilGuid);

	Task AddOrderAsync(Document factorType, Guid tafsilGuid);

	Task ShowOrderSellListAsync(Guid tafsilGuid);

	Task ShowGardeshAsync(Guid tafsilGuid);

	Task AddReceptionAsync(Guid tafsilGuid);

	Task AddPardakhtAsync(Guid tafsilGuid);

	Task AddQestAsync(Guid tafsilGuid);

	Task ShowCallingLogAsync();

	Task SaveAsync();

	Task AddNewCustomerAsync(string number);

	Task UpdateCustomerAsync(string number);
}
