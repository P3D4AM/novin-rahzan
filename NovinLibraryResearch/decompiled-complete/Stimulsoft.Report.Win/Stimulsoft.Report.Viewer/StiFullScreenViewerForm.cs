using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Viewer;

public class StiFullScreenViewerForm : StiViewerForm
{
	private Button btClose;

	private Container components;

	public StiFullScreenViewerForm(StiReport report)
		: base(report)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Control)btClose).Visible = !StiOptions.Viewer.DisableCloseButtonInFullScreenMode;
			Localize();
			((Control)this).Controls.SetChildIndex((Control)(object)btClose, 0);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		btClose = new Button();
		((Control)this).SuspendLayout();
		((Control)btClose).AccessibleRole = (AccessibleRole)43;
		((Control)btClose).Anchor = (AnchorStyles)9;
		((Control)btClose).Location = new Point(507, 12);
		((Control)btClose).Name = "btClose";
		((Control)btClose).Size = new Size(192, 45);
		((Control)btClose).TabIndex = 1;
		((Control)btClose).Text = "Close";
		((Control)btClose).Click += ButtonClose_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((Form)this).ClientSize = new Size(712, 494);
		((Control)this).Controls.Add((Control)(object)btClose);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Location = new Point(0, 0);
		((Control)this).Name = "StiFullScreenViewerForm";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "StiFullScreenPreviewForm";
		((Form)this).WindowState = (FormWindowState)0;
		((Control)this).Controls.SetChildIndex((Control)(object)btClose, 0);
		((Control)this).ResumeLayout(false);
	}

	public void Localize()
	{
		((Control)btClose).Text = StiLocalization.Get("FormViewer", "Close");
	}

	private void ButtonClose_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		base.ViewerControl.Parameters.ShowDialogRequestFromUserVariables();
	}
}
