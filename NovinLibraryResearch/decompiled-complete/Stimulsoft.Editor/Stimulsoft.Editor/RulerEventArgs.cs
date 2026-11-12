using System;

namespace Stimulsoft.Editor;

public class RulerEventArgs : EventArgs
{
	public object Object;

	public RulerEventArgs(object obj)
	{
		Object = obj;
	}
}
