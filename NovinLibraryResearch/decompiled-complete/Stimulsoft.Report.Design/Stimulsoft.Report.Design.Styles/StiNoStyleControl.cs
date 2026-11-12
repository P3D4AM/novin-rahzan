using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design.Styles;

[ToolboxItem(false)]
public class StiNoStyleControl : UserControl
{
	private StiStyleDesigner designer;

	private IContainer components;

	private PictureBox imageNoData;

	private Label emptyDataHyperlink;

	private Label label1;

	public StiNoStyleControl(StiStyleDesigner designer)
	{
		this.designer = designer;
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
		((Control)emptyDataHyperlink).Text = StiLocalization.Get("FormDictionaryDesigner", "ClickHere");
		((Control)label1).Text = StiLocalization.Get("FormStyleDesigner", "CreateNewComponentStyle");
	}

	private void DoScaling()
	{
		string text = ((StiScaleUI.Factor >= 4.0) ? "_x4" : ((StiScaleUI.Factor >= 3.0) ? "_x3" : ((StiScaleUI.Factor >= 2.0) ? "_x2" : ((!(StiScaleUI.Factor >= 1.5)) ? "" : "_x1_5"))));
		imageNoData.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Images.Guid.NewStyle" + text + ".png");
	}

	private void OnEmptyDataHyperlinkClick(object sender, EventArgs e)
	{
		designer.ProcessNewComponentStyle();
	}

	private void OnHyperlinkMouseLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = Color.FromArgb(255, 43, 86, 154);
		((Control)val).Invalidate();
	}

	private void OnHyperlinkMouseEnter(object sender, EventArgs e)
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
		emptyDataHyperlink = new Label();
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
		((Control)emptyDataHyperlink).Anchor = (AnchorStyles)1;
		((Control)emptyDataHyperlink).Font = new Font("Microsoft Sans Serif", 9f);
		((Control)emptyDataHyperlink).ForeColor = Color.FromArgb(43, 86, 154);
		((Control)emptyDataHyperlink).Location = new Point(0, 68);
		((Control)emptyDataHyperlink).Name = "emptyDataHyperlink";
		((Control)emptyDataHyperlink).Size = new Size(200, 23);
		((Control)emptyDataHyperlink).TabIndex = 1;
		((Control)emptyDataHyperlink).Text = "Click here";
		emptyDataHyperlink.TextAlign = (ContentAlignment)32;
		((Control)emptyDataHyperlink).Click += OnEmptyDataHyperlinkClick;
		((Control)emptyDataHyperlink).MouseEnter += OnHyperlinkMouseEnter;
		((Control)emptyDataHyperlink).MouseLeave += OnHyperlinkMouseLeave;
		((Control)label1).Anchor = (AnchorStyles)1;
		((Control)label1).Location = new Point(0, 89);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(200, 23);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "to create the new component style";
		label1.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)emptyDataHyperlink);
		((Control)this).Controls.Add((Control)(object)imageNoData);
		((Control)this).Name = "StiNoStyleControl";
		((Control)this).Size = new Size(200, 118);
		((ISupportInitialize)imageNoData).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
