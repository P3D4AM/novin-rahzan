using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Dictionary.Design;

[ToolboxItem(false)]
public class StiNoDataControl : UserControl
{
	private StiDictionaryPanelService service;

	private IContainer components;

	private PictureBox imageNoData;

	private Label emptyDataHyperlink;

	private StiLabelUX label1;

	private StiLabelUX labelOr;

	private StiLabelUX label2;

	private Panel panelLine;

	internal Label ButtonShowMore { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return Color.White;
			}
			return StiUX.BackgroundDark;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	public StiNoDataControl(StiDictionaryPanelService service)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (!StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)labelOr).Visible = false;
				((Control)label2).Visible = false;
				((Control)panelLine).Visible = false;
			}
			DoScaling();
			((Control)imageNoData).Left = (((Control)this).Width - ((Control)imageNoData).Width) / 2;
			this.service = service;
			ButtonShowMore = new Label
			{
				Anchor = (AnchorStyles)2,
				AutoSize = true,
				Location = new Point(10, 10),
				Font = ((Control)emptyDataHyperlink).Font
			};
			((Control)ButtonShowMore).MouseEnter += Hyperlink_MouseEnter;
			((Control)ButtonShowMore).MouseLeave += Hyperlink_MouseLeave;
			((Control)ButtonShowMore).SizeChanged += ButtonShowMore_SizeChanged;
			((Control)emptyDataHyperlink).Click += EmptyDataHyperlink_Click;
			((Control)emptyDataHyperlink).MouseEnter += Hyperlink_MouseEnter;
			((Control)emptyDataHyperlink).MouseLeave += Hyperlink_MouseLeave;
			DoTheming();
			Localize();
			((Control)this).SizeChanged += This_SizeChanged;
		}
	}

	public void Localize()
	{
		((Control)emptyDataHyperlink).Text = StiLocalization.Get("FormDictionaryDesigner", "ClickHere");
		((Control)label1).Text = StiLocalization.Get("FormDictionaryDesigner", "CreateNewDataSource");
		((Control)labelOr).Text = StiLocalization.Get("PropertyEnum", "StiFilterModeOr").ToLowerInvariant();
		((Control)label2).Text = StiLocalization.Get("FormDictionaryDesigner", "DragNewDataSource");
		((Control)ButtonShowMore).Text = StiLocalization.Get("Buttons", "ShowMore");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)this).BackColor = StiUX.BackgroundDark;
			((Control)panelLine).BackColor = StiUX.Separator;
		}
		((Control)emptyDataHyperlink).ForeColor = StiUX.Hyperlink;
		((Control)ButtonShowMore).ForeColor = StiUX.Hyperlink;
		((Control)ButtonShowMore).BackColor = ((Control)this).BackColor;
	}

	private void DoScaling()
	{
		string text = ((StiScaleUI.Factor >= 4.0) ? "_x4" : ((StiScaleUI.Factor >= 3.0) ? "_x3" : ((StiScaleUI.Factor >= 2.0) ? "_x2" : ((!(StiScaleUI.Factor >= 1.5)) ? "" : "_x1_5"))));
		imageNoData.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Images.Guid.NewData" + text + ".png");
	}

	private void This_SizeChanged(object sender, EventArgs e)
	{
		Control parent = ((Control)this).Parent;
		UserControl val = (UserControl)(object)((parent is UserControl) ? parent : null);
		if (val != null)
		{
			((Control)this).Location = new Point(0, ((Control)val).Height / 2 - ((Control)this).Height / 2);
			((Control)this).Width = ((Control)val).Width;
		}
	}

	private void ButtonShowMore_SizeChanged(object sender, EventArgs e)
	{
		Control parent = ((Control)ButtonShowMore).Parent;
		UserControl val = (UserControl)(object)((parent is UserControl) ? parent : null);
		if (val != null)
		{
			((Control)ButtonShowMore).Location = new Point((((Control)val).Width - ((Control)ButtonShowMore).Width) / 2, ((Control)val).Height - ((Control)ButtonShowMore).Height - 12);
		}
	}

	private async void EmptyDataHyperlink_Click(object sender, EventArgs e)
	{
		await service.InvokeConnectionNewAsync();
	}

	private void Hyperlink_MouseLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = StiUX.Hyperlink;
		((Control)val).Invalidate();
	}

	private void Hyperlink_MouseEnter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Label val = (Label)sender;
		((Control)val).ForeColor = StiUX.HyperlinkHover;
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		imageNoData = new PictureBox();
		emptyDataHyperlink = new Label();
		label1 = new StiLabelUX();
		labelOr = new StiLabelUX();
		label2 = new StiLabelUX();
		panelLine = new Panel();
		((ISupportInitialize)imageNoData).BeginInit();
		((Control)this).SuspendLayout();
		((Control)imageNoData).Anchor = (AnchorStyles)1;
		((Control)imageNoData).Location = new Point(70, 4);
		((Control)imageNoData).Name = "imageNoData";
		((Control)imageNoData).Size = new Size(60, 60);
		imageNoData.SizeMode = (PictureBoxSizeMode)2;
		imageNoData.TabIndex = 4;
		imageNoData.TabStop = false;
		((Control)emptyDataHyperlink).Anchor = (AnchorStyles)1;
		((Control)emptyDataHyperlink).Font = new Font("Microsoft Sans Serif", 9f);
		((Control)emptyDataHyperlink).ForeColor = Color.FromArgb(43, 86, 154);
		((Control)emptyDataHyperlink).Location = new Point(0, 68);
		((Control)emptyDataHyperlink).Name = "emptyDataHyperlink";
		((Control)emptyDataHyperlink).Size = new Size(200, 23);
		((Control)emptyDataHyperlink).TabIndex = 3;
		((Control)emptyDataHyperlink).Text = "Click here";
		emptyDataHyperlink.TextAlign = (ContentAlignment)32;
		((Control)label1).Anchor = (AnchorStyles)1;
		((Control)label1).Location = new Point(0, 89);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(200, 23);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "to create the new data source";
		((Label)label1).TextAlign = (ContentAlignment)32;
		((Control)labelOr).Anchor = (AnchorStyles)1;
		((Control)labelOr).AutoSize = true;
		((Control)labelOr).Location = new Point(92, 108);
		((Control)labelOr).Name = "labelOr";
		((Control)labelOr).Size = new Size(16, 13);
		((Control)labelOr).TabIndex = 1;
		((Control)labelOr).Text = "or";
		((Control)label2).Anchor = (AnchorStyles)1;
		((Control)label2).Location = new Point(0, 122);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(200, 23);
		((Control)label2).TabIndex = 0;
		((Control)label2).Text = "drag your data directly to this panel";
		((Label)label2).TextAlign = (ContentAlignment)32;
		((Control)panelLine).Anchor = (AnchorStyles)1;
		((Control)panelLine).BackColor = Color.FromArgb(43, 86, 154);
		((Control)panelLine).Location = new Point(0, 115);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(200, 1);
		((Control)panelLine).TabIndex = 5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)labelOr);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)emptyDataHyperlink);
		((Control)this).Controls.Add((Control)(object)imageNoData);
		((Control)this).Controls.Add((Control)(object)panelLine);
		((Control)this).Name = "StiNoDataControl";
		((Control)this).Size = new Size(200, 150);
		((ISupportInitialize)imageNoData).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
