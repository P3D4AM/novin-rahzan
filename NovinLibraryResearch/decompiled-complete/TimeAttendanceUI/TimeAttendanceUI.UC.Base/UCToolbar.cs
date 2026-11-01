using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace TimeAttendanceUI.UC.Base;

public class UCToolbar : UserControl
{
	private IContainer components;

	private PanelEx panelEx1;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnPrintA4;

	private ToolStripButton BtnPreviewA4;

	private ToolStripButton BtnDesignA4;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnPrintA5;

	private ToolStripButton BtnPreviewA5;

	private ToolStripButton BtnDesignA5;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton BtnPrint8;

	private ToolStripButton BtnPreview8;

	private ToolStripButton Btndesign8;

	public UCToolbar()
	{
		InitializeComponent();
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Expected O, but got Unknown
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UCToolbar));
		panelEx1 = new PanelEx();
		toolStrip1 = new ToolStrip();
		BtnPrintA4 = new ToolStripButton();
		BtnPreviewA4 = new ToolStripButton();
		BtnDesignA4 = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		BtnPrintA5 = new ToolStripButton();
		BtnPreviewA5 = new ToolStripButton();
		BtnDesignA5 = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		BtnPrint8 = new ToolStripButton();
		BtnPreview8 = new ToolStripButton();
		Btndesign8 = new ToolStripButton();
		((Control)panelEx1).SuspendLayout();
		((Control)toolStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)toolStrip1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(778, 34);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)toolStrip1).Dock = (DockStyle)5;
		toolStrip1.Items.AddRange((ToolStripItem[])(object)new ToolStripItem[11]
		{
			(ToolStripItem)BtnPrintA4,
			(ToolStripItem)BtnPreviewA4,
			(ToolStripItem)BtnDesignA4,
			(ToolStripItem)toolStripSeparator1,
			(ToolStripItem)BtnPrintA5,
			(ToolStripItem)BtnPreviewA5,
			(ToolStripItem)BtnDesignA5,
			(ToolStripItem)toolStripSeparator2,
			(ToolStripItem)BtnPrint8,
			(ToolStripItem)BtnPreview8,
			(ToolStripItem)Btndesign8
		});
		((Control)toolStrip1).Location = new Point(0, 0);
		((Control)toolStrip1).Name = "toolStrip1";
		((Control)toolStrip1).Size = new Size(778, 34);
		((Control)toolStrip1).TabIndex = 0;
		((Control)toolStrip1).Text = "toolStrip1";
		((ToolStripItem)BtnPrintA4).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPrintA4).Image = (Image)componentResourceManager.GetObject("BtnPrintA4.Image");
		((ToolStripItem)BtnPrintA4).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPrintA4).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPrintA4).Name = "BtnPrintA4";
		((ToolStripItem)BtnPrintA4).Size = new Size(52, 31);
		((ToolStripItem)BtnPrintA4).Text = "toolStripButton1";
		((ToolStripItem)BtnPrintA4).ToolTipText = "پرینت A4";
		((ToolStripItem)BtnPreviewA4).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPreviewA4).Image = (Image)componentResourceManager.GetObject("BtnPreviewA4.Image");
		((ToolStripItem)BtnPreviewA4).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPreviewA4).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPreviewA4).Name = "BtnPreviewA4";
		((ToolStripItem)BtnPreviewA4).Size = new Size(52, 31);
		((ToolStripItem)BtnPreviewA4).Text = "toolStripButton2";
		((ToolStripItem)BtnPreviewA4).ToolTipText = "پیش نمایش A4";
		((ToolStripItem)BtnDesignA4).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnDesignA4).Image = (Image)componentResourceManager.GetObject("BtnDesignA4.Image");
		((ToolStripItem)BtnDesignA4).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnDesignA4).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnDesignA4).Name = "BtnDesignA4";
		((ToolStripItem)BtnDesignA4).Size = new Size(52, 31);
		((ToolStripItem)BtnDesignA4).Text = "toolStripButton3";
		((ToolStripItem)BtnDesignA4).ToolTipText = "طراحی A4\r\n";
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(6, 34);
		((ToolStripItem)BtnPrintA5).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPrintA5).Image = (Image)componentResourceManager.GetObject("BtnPrintA5.Image");
		((ToolStripItem)BtnPrintA5).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPrintA5).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPrintA5).Name = "BtnPrintA5";
		((ToolStripItem)BtnPrintA5).Size = new Size(52, 31);
		((ToolStripItem)BtnPrintA5).Text = "toolStripButton4";
		((ToolStripItem)BtnPrintA5).ToolTipText = "پرینت A5";
		((ToolStripItem)BtnPreviewA5).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPreviewA5).Image = (Image)componentResourceManager.GetObject("BtnPreviewA5.Image");
		((ToolStripItem)BtnPreviewA5).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPreviewA5).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPreviewA5).Name = "BtnPreviewA5";
		((ToolStripItem)BtnPreviewA5).Size = new Size(52, 31);
		((ToolStripItem)BtnPreviewA5).Text = "toolStripButton5";
		((ToolStripItem)BtnPreviewA5).ToolTipText = "پیش نمایش A5";
		((ToolStripItem)BtnDesignA5).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnDesignA5).Image = (Image)componentResourceManager.GetObject("BtnDesignA5.Image");
		((ToolStripItem)BtnDesignA5).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnDesignA5).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnDesignA5).Name = "BtnDesignA5";
		((ToolStripItem)BtnDesignA5).Size = new Size(52, 31);
		((ToolStripItem)BtnDesignA5).Text = "طراحی A5";
		((ToolStripItem)toolStripSeparator2).Name = "toolStripSeparator2";
		((ToolStripItem)toolStripSeparator2).Size = new Size(6, 34);
		((ToolStripItem)BtnPrint8).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPrint8).Image = (Image)componentResourceManager.GetObject("BtnPrint8.Image");
		((ToolStripItem)BtnPrint8).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPrint8).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPrint8).Name = "BtnPrint8";
		((ToolStripItem)BtnPrint8).Size = new Size(52, 31);
		((ToolStripItem)BtnPrint8).Text = "پرینت 8 سانت";
		((ToolStripItem)BtnPreview8).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)BtnPreview8).Image = (Image)componentResourceManager.GetObject("BtnPreview8.Image");
		((ToolStripItem)BtnPreview8).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)BtnPreview8).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)BtnPreview8).Name = "BtnPreview8";
		((ToolStripItem)BtnPreview8).Size = new Size(52, 31);
		((ToolStripItem)BtnPreview8).Text = "پیش نمایش 8 سانت";
		((ToolStripItem)Btndesign8).DisplayStyle = (ToolStripItemDisplayStyle)2;
		((ToolStripItem)Btndesign8).Image = (Image)componentResourceManager.GetObject("Btndesign8.Image");
		((ToolStripItem)Btndesign8).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)Btndesign8).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)Btndesign8).Name = "Btndesign8";
		((ToolStripItem)Btndesign8).Size = new Size(52, 31);
		((ToolStripItem)Btndesign8).Text = "طراحی 8 سانت";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Name = "UCToolbar";
		((Control)this).Size = new Size(778, 34);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)toolStrip1).ResumeLayout(false);
		((Control)toolStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
