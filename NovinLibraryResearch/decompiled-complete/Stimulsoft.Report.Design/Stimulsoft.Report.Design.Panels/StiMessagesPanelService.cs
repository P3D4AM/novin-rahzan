using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Panels;

[StiServiceBitmap(typeof(StiMessagesPanelService), "Stimulsoft.Report.Design.Images.Panels.Messages.png")]
[Obsolete("StiMessagesPanelService is not used more!")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class StiMessagesPanelService : StiPanelService
{
	public override string ServiceName => StiLocalization.Get("Panels", "Messages");

	public override eShortcut Shortcut => eShortcut.CtrlShiftM;

	public override DockStyle DockStyle => (DockStyle)2;

	protected override bool AutoHide => true;

	protected override Size DefaultFloatingSize => new Size(500, 300);

	public static StiMessagesPanelService GetService()
	{
		return GetService(null);
	}

	public static StiMessagesPanelService GetService(StiDesignerControl designer)
	{
		return null;
	}

	public void WriteMessage(string text)
	{
	}
}
