using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

[ToolboxItem(false)]
public class StiNoCultureControl : UserControl
{
	private StiGlobalizationEditorForm editor;

	private IContainer components;

	private PictureBox imageNoData;

	private Label hyperlinkClickHere;

	private Label label1;

	public StiNoCultureControl(StiGlobalizationEditorForm editor)
	{
		this.editor = editor;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			DoScaling();
			((Control)imageNoData).Left = (((Control)this).Width - ((Control)imageNoData).Width) / 2;
			Localize();
		}
	}

	public void Localize()
	{
		((Control)hyperlinkClickHere).Text = StiLocalization.Get("FormDictionaryDesigner", "ClickHere");
		((Control)label1).Text = StiLocalization.Get("FormGlobalizationEditor", "CreateNewCulture");
	}

	private void DoScaling()
	{
		string text = ((StiScaleUI.Factor >= 4.0) ? "_x4" : ((StiScaleUI.Factor >= 3.0) ? "_x3" : ((StiScaleUI.Factor >= 2.0) ? "_x2" : ((!(StiScaleUI.Factor >= 1.5)) ? "" : "_x1_5"))));
		imageNoData.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Images.Guid.NewCulture" + text + ".png");
	}

	private void HyperlinkClickHere_Click(object sender, EventArgs e)
	{
		editor.ProcessAddCultureClick();
	}

	private void HyperlinkClickHere_MouseLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = Color.FromArgb(255, 43, 86, 154);
		((Control)val).Invalidate();
	}

	private void HyperlinkClickHere_MouseEnter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = Color.Red;
		((Control)val).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		imageNoData = new PictureBox();
		hyperlinkClickHere = new Label();
		label1 = new Label();
		((ISupportInitialize)imageNoData).BeginInit();
		((Control)this).SuspendLayout();
		((Control)imageNoData).Anchor = (AnchorStyles)1;
		((Control)imageNoData).Location = new Point(70, 4);
		((Control)imageNoData).Name = "imageNoData";
		((Control)imageNoData).Size = new Size(60, 60);
		imageNoData.SizeMode = (PictureBoxSizeMode)2;
		imageNoData.TabIndex = 2;
		imageNoData.TabStop = false;
		((Control)hyperlinkClickHere).Anchor = (AnchorStyles)1;
		((Control)hyperlinkClickHere).Font = new Font("Microsoft Sans Serif", 9f);
		((Control)hyperlinkClickHere).ForeColor = Color.FromArgb(43, 86, 154);
		((Control)hyperlinkClickHere).Location = new Point(0, 68);
		((Control)hyperlinkClickHere).Name = "hyperlinkClickHere";
		((Control)hyperlinkClickHere).Size = new Size(200, 23);
		((Control)hyperlinkClickHere).TabIndex = 1;
		((Control)hyperlinkClickHere).Text = "Click here";
		hyperlinkClickHere.TextAlign = (ContentAlignment)32;
		((Control)hyperlinkClickHere).Click += HyperlinkClickHere_Click;
		((Control)hyperlinkClickHere).MouseEnter += HyperlinkClickHere_MouseEnter;
		((Control)hyperlinkClickHere).MouseLeave += HyperlinkClickHere_MouseLeave;
		((Control)label1).Anchor = (AnchorStyles)1;
		((Control)label1).Location = new Point(0, 89);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(200, 23);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "to create the new culture";
		label1.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)hyperlinkClickHere);
		((Control)this).Controls.Add((Control)(object)imageNoData);
		((Control)this).Name = "StiNoCultureControl";
		((Control)this).Size = new Size(200, 118);
		((ISupportInitialize)imageNoData).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
