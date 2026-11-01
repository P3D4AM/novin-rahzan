using System;
using System.Windows.Forms;
using ZDC2911.UI;

namespace ZDC2911;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run((Form)(object)new CMForm());
	}
}
