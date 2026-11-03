using System;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Waiting;

public interface IWaiterUi : INpDisposable, IDisposable
{
	event Func<object, EventArgs, Task> OnAutoClose;

	Task HideAsync();

	void Show();
}
