using System.Threading.Tasks;
using PacketParser.Assistance;

namespace TimeAttendanceDAL;

public static class MainFormOwner
{
	public static ILkUiFactory owner { get; set; }

	public static TaskCompletionSource<bool> TaskCompletion { get; set; }
}
