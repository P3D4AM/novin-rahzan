using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.EventArgs;

namespace EntityCache;

public class TransferSqquestObject
{
	public TaskCompletionSource<bool> TaskStatus { get; } = new TaskCompletionSource<bool>();


	public Action<PercenteCompletedEventArgs> PercentageAction { get; set; }

	public ReturnedSaveFuncInfo Result { get; private set; } = new ReturnedSaveFuncInfo();

}
