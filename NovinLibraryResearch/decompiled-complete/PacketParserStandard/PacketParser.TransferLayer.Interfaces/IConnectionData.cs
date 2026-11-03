using System.Text;

namespace PacketParser.TransferLayer.Interfaces;

public interface IConnectionData
{
	byte[] ProgressReceived(byte[] bytes, short part, short totalPart, string refrenceId, StringBuilder sb);
}
