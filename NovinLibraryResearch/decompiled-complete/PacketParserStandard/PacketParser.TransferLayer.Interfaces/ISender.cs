using System.Threading.Tasks;

namespace PacketParser.TransferLayer.Interfaces;

public interface ISender
{
	Task SendData(Packet Data, string guid);
}
