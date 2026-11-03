using System;
using System.Threading.Tasks;

namespace PacketParser.Scale;

public class ScaleClientEventHandler
{
	public static ScaleClientEventHandler Instance { get; } = new ScaleClientEventHandler();


	public event Func<object, int, Task> OnMarkedAsRead;

	public Task OnMarkedAsReadAsync(int orderId)
	{
		return Utilities.RaiseEventOnNewThread(this.OnMarkedAsRead, this, orderId);
	}
}
