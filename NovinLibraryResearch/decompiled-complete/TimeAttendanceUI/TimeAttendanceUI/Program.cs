using System;
using System.Threading;
using System.Windows.Forms;
using TimeAttendanceUI.Forms.User;

namespace TimeAttendanceUI;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		if ((int)((Form)new FrmEnter(default(Guid).ToString())).ShowDialog() != 1)
		{
			return;
		}
		while (true)
		{
			Thread.Sleep(1000);
			Application.DoEvents();
		}
	}
}
