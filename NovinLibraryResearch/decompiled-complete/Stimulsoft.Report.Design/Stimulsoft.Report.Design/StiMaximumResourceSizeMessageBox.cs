using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public static class StiMaximumResourceSizeMessageBox
{
	public static bool Check(Form owner, string file)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		if (new FileInfo(file).Length <= StiOptions.Engine.ReportResources.MaximumSize)
		{
			return true;
		}
		DialogResult val = MessageBox.Show((IWin32Window)(object)owner, Loc.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
		return (int)val != 7;
	}
}
