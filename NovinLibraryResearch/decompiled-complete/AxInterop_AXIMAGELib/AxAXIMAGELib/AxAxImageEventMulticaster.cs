using System;
using System.Runtime.InteropServices;
using AXIMAGELib;

namespace AxAXIMAGELib;

[ClassInterface(ClassInterfaceType.None)]
public class AxAxImageEventMulticaster : _DAxImageEvents
{
	private AxAxImage parent;

	public AxAxImageEventMulticaster(AxAxImage parent)
	{
		this.parent = parent;
	}

	public virtual void Click()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnClickEvent(parent, e);
	}
}
