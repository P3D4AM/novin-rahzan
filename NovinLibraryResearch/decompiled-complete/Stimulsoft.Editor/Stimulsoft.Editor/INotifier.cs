using System;

namespace Stimulsoft.Editor;

public interface INotifier
{
	void Notification(object sender, EventArgs e);
}
