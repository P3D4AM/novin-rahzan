using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Connection;

public interface IServer
{
	WebContextHolder WebContextHolder { get; }

	Task<ReturnedSaveFuncInfo> SendBytesAsync(string connectionUniqueId, byte[] data, string sender);

	Task<ReturnedSaveFuncInfo> SendAsync(string connectionUniqueId, EnServerMessageInstruction instruction, string data, ClientConnectionSettings sett);

	bool ExistsConnection(string connectionUniqueId);
}
