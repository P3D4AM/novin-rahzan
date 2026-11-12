using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiWebBrowser : AxHost
{
	private object ocx;

	public virtual string LocationURL
	{
		get
		{
			try
			{
				Type type = ocx.GetType();
				return type.InvokeMember("LocationURL", BindingFlags.GetProperty, null, ocx, new object[0]) as string;
			}
			catch
			{
				return string.Empty;
			}
		}
	}

	public void DoBack()
	{
		try
		{
			if (ocx != null)
			{
				Type type = ocx.GetType();
				type.InvokeMember("GoBack", BindingFlags.InvokeMethod, null, ocx, new object[0]);
			}
		}
		catch
		{
		}
	}

	public void DoForward()
	{
		try
		{
			if (ocx != null)
			{
				Type type = ocx.GetType();
				type.InvokeMember("GoForward", BindingFlags.InvokeMethod, null, ocx, new object[0]);
			}
		}
		catch
		{
		}
	}

	public void DoRefresh()
	{
		try
		{
			if (ocx != null)
			{
				Type type = ocx.GetType();
				type.InvokeMember("Refresh", BindingFlags.InvokeMethod, null, ocx, new object[0]);
			}
		}
		catch
		{
		}
	}

	public void DoStop()
	{
		try
		{
			if (ocx != null)
			{
				Type type = ocx.GetType();
				type.InvokeMember("Stop", BindingFlags.InvokeMethod, null, ocx, new object[0]);
			}
		}
		catch
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = ((AxHost)this).GetOcx();
		}
		catch
		{
		}
	}

	public void DoNavigate(string url)
	{
		if (ocx != null)
		{
			object obj = null;
			ocx.GetType().InvokeMember("Navigate2", BindingFlags.InvokeMethod, null, ocx, new object[5] { url, obj, obj, obj, obj });
		}
	}

	public StiWebBrowser()
		: base("8856f961-340a-11d0-a96b-00c04fd705a2")
	{
	}
}
