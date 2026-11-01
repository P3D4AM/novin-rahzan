using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;

namespace TimeAttendanceUI.Forms;

public static class FrmPrompt
{
	public class frmTest : CacheBaseForm
	{
		public frmTest(string contextGuid)
			: base(contextGuid)
		{
		}
	}

	public static async Task<string> ShowDialogAsync(IWin32Window owner, string contextGuid, string text, string caption)
	{
		frmTest frmTest = new frmTest(contextGuid);
		((Control)frmTest).Width = 500;
		((Control)frmTest).Height = 150;
		((Form)frmTest).FormBorderStyle = (FormBorderStyle)3;
		((Control)frmTest).Text = caption;
		((Form)frmTest).StartPosition = (FormStartPosition)4;
		frmTest prompt = frmTest;
		Label val = new Label
		{
			RightToLeft = (RightToLeft)1,
			Left = 350,
			Top = 20,
			Text = text
		};
		TextBox textBox = new TextBox
		{
			RightToLeft = (RightToLeft)1,
			Left = 50,
			Top = 40,
			Width = 400
		};
		Button val2 = new Button
		{
			Text = "ثبت",
			Left = 350,
			Width = 100,
			Top = 80,
			DialogResult = (DialogResult)1
		};
		((Control)val2).Click += delegate
		{
			((Form)prompt).Close();
		};
		((Control)prompt).Controls.Add((Control)(object)textBox);
		((Control)prompt).Controls.Add((Control)(object)val2);
		((Control)prompt).Controls.Add((Control)(object)val);
		((Form)prompt).AcceptButton = (IButtonControl)(object)val2;
		return ((int)(await ((CacheBaseForm)prompt).ShowAsync(owner)) == 1) ? ((Control)textBox).Text : "";
	}
}
