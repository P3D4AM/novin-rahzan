using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace PacketParser.DalInterfaces;

public interface ISettingRepo : IHasContext
{
	Task<string> GetValueAsync(string connectionString, string key, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveSettingAsync(string connectionString, string key, string value, CancellationToken token);
}
