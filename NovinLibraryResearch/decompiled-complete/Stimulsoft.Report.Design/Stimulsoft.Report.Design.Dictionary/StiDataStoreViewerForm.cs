using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataStoreViewerForm : StiFormUX
{
	private IContainer components;

	private StiDataControl dataControl;

	public StiDataStoreViewerForm(StiDictionary dictionary)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			dataControl.Build(dictionary.DataStore);
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "DataStoreViewerForm");
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		dataControl = new StiDataControl();
		((Control)this).SuspendLayout();
		((Control)dataControl).Anchor = (AnchorStyles)15;
		((Control)dataControl).Location = new Point(24, 24);
		((Control)dataControl).Margin = new Padding(6, 6, 6, 6);
		((Control)dataControl).Name = "dataControl";
		((Control)dataControl).Size = new Size(704, 733);
		((Control)dataControl).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(752, 779);
		((Control)this).Controls.Add((Control)(object)dataControl);
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataStoreViewerForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "DataStore";
		((Form)this).Closed += ThisClosed;
		((Control)this).ResumeLayout(false);
	}
}
