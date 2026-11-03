using System.Threading.Tasks;

namespace PacketParser.BusinessLayer.Interfaces;

public interface IOrderGridActionAggregator
{
	Task<ReturnedSaveFuncInfo> AddSync();

	Task<ReturnedSaveFuncInfo> UpdateSync();
}
