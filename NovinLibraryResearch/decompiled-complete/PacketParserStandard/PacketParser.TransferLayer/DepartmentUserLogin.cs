using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.TransferLayer;

internal class DepartmentUserLogin
{
	private TaskCompletionSource<EnLoginResult> TaskCompletionTmpSave = new TaskCompletionSource<EnLoginResult>();
}
