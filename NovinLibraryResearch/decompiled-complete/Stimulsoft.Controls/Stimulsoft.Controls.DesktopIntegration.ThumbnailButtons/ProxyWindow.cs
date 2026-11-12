using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

internal sealed class ProxyWindow : Form
{
	private CustomWindowsManager windowsManager;

	private IntPtr _proxyingFor;

	public CustomWindowsManager WindowsManager
	{
		private get
		{
			return windowsManager;
		}
		set
		{
			windowsManager = value;
		}
	}

	public IntPtr RealWindow => _proxyingFor;

	protected override void WndProc(ref Message m)
	{
		if (WindowsManager != null)
		{
			WindowsManager.DispatchMessage(ref m);
		}
		((Form)this).WndProc(ref m);
	}

	public ProxyWindow(IntPtr proxyingFor)
	{
		_proxyingFor = proxyingFor;
		((Form)this).Size = new Size(1, 1);
		StringBuilder stringBuilder = new StringBuilder(256);
		UnsafeNativeMethods.GetWindowText(_proxyingFor, stringBuilder, stringBuilder.Capacity);
		((Control)this).Text = stringBuilder.ToString();
	}
}
