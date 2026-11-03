using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

[ToolboxItem(false)]
public class StiPagesRangeControl : UserControl
{
	private IContainer components;

	private StiGroupBox groupBoxPageRange;

	internal StiRadioButtonInfo radioButtonAll;

	internal StiRadioButtonInfo radioButtonCurrentPage;

	internal StiRadioButtonInfo radioButtonPages;

	internal StiTextBox tbPagesRange;

	private Label labelInfo;

	public int CurrentPage { get; set; }

	public StiPagesRangeControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			LocalizeSuperToolTip();
			((Control)labelInfo).Anchor = (AnchorStyles)15;
		}
	}

	private void TextBoxPagesRange_TextChanged(object sender, EventArgs e)
	{
		((RadioButton)radioButtonPages).Checked = true;
	}

	private void GroupBoxPageRange_SizeChanged(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)groupBoxPageRange).Size;
		if (StiScale.Factor > 1.0)
		{
			((Control)labelInfo).Height = ((Control)groupBoxPageRange).Height - ((Control)labelInfo).Top - StiScale.YYI(16.0);
		}
		((Control)tbPagesRange).Width = ((Control)groupBoxPageRange).Width - ((Control)tbPagesRange).Left - StiScaleUI.I18;
	}

	public void Localize()
	{
		((Control)groupBoxPageRange).Text = StiLocalization.Get("Report", "RangePage");
		((Control)radioButtonAll).Text = StiLocalization.Get("Report", "RangeAll");
		((Control)radioButtonCurrentPage).Text = StiLocalization.Get("Report", "RangeCurrentPage");
		((Control)radioButtonPages).Text = Loc.GetCleaned("Report", "RangePages");
		((Control)labelInfo).Text = StiLocalization.Get("Report", "RangeInfo");
	}

	private void LocalizeSuperToolTip()
	{
		radioButtonAll.SetToolTip(StiLocalization.Get("HelpViewer", "PageAll"));
		radioButtonCurrentPage.SetToolTip(StiLocalization.Get("HelpViewer", "CurrentPage"));
		radioButtonPages.SetToolTip(StiLocalization.Get("HelpViewer", "RangePages"));
	}

	public StiPagesRange GetPagesRange()
	{
		StiRangeType rangeType = (((RadioButton)radioButtonAll).Checked ? StiRangeType.All : ((!((RadioButton)radioButtonCurrentPage).Checked) ? StiRangeType.Pages : StiRangeType.CurrentPage));
		return new StiPagesRange(rangeType, ((Control)tbPagesRange).Text, CurrentPage);
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		groupBoxPageRange = new StiGroupBox();
		tbPagesRange = new StiTextBox();
		radioButtonAll = new StiRadioButtonInfo();
		labelInfo = new Label();
		radioButtonPages = new StiRadioButtonInfo();
		radioButtonCurrentPage = new StiRadioButtonInfo();
		((Control)groupBoxPageRange).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBoxPageRange).Controls.Add((Control)(object)tbPagesRange);
		((Control)groupBoxPageRange).Controls.Add((Control)(object)radioButtonAll);
		((Control)groupBoxPageRange).Controls.Add((Control)(object)labelInfo);
		((Control)groupBoxPageRange).Controls.Add((Control)(object)radioButtonPages);
		((Control)groupBoxPageRange).Controls.Add((Control)(object)radioButtonCurrentPage);
		((Control)groupBoxPageRange).Dock = (DockStyle)5;
		((Control)groupBoxPageRange).Location = new Point(0, 0);
		((Control)groupBoxPageRange).Name = "groupBoxPageRange";
		groupBoxPageRange.ResHeight = 0;
		((Control)groupBoxPageRange).Size = new Size(304, 155);
		((Control)groupBoxPageRange).TabIndex = 0;
		((GroupBox)groupBoxPageRange).TabStop = false;
		((Control)groupBoxPageRange).Text = "PageRange";
		((Control)groupBoxPageRange).SizeChanged += GroupBoxPageRange_SizeChanged;
		((Control)tbPagesRange).Anchor = (AnchorStyles)13;
		((Control)tbPagesRange).BackColor = Color.White;
		((Control)tbPagesRange).ForeColor = SystemColors.ControlText;
		((Control)tbPagesRange).Location = new Point(128, 78);
		((Control)tbPagesRange).Name = "tbPagesRange";
		((Control)tbPagesRange).Size = new Size(162, 21);
		((Control)tbPagesRange).TabIndex = 3;
		((Control)tbPagesRange).TextChanged += TextBoxPagesRange_TextChanged;
		((Control)radioButtonAll).AutoSize = true;
		((RadioButton)radioButtonAll).Checked = true;
		((Control)radioButtonAll).ForeColor = SystemColors.ControlText;
		((Control)radioButtonAll).Location = new Point(12, 20);
		((Control)radioButtonAll).Name = "radioButtonAll";
		((Control)radioButtonAll).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonAll).Size = new Size(48, 17);
		((Control)radioButtonAll).TabIndex = 0;
		((RadioButton)radioButtonAll).TabStop = true;
		((Control)radioButtonAll).Text = "All";
		((Control)labelInfo).ForeColor = Color.DimGray;
		((Control)labelInfo).Location = new Point(12, 110);
		((Control)labelInfo).Name = "labelInfo";
		((Control)labelInfo).Size = new Size(278, 31);
		((Control)labelInfo).TabIndex = 4;
		((Control)labelInfo).Text = "Info";
		labelInfo.TextAlign = (ContentAlignment)16;
		((Control)radioButtonPages).AutoSize = true;
		((Control)radioButtonPages).ForeColor = SystemColors.ControlText;
		((Control)radioButtonPages).Location = new Point(12, 78);
		((Control)radioButtonPages).Name = "radioButtonPages";
		((Control)radioButtonPages).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonPages).Size = new Size(67, 17);
		((Control)radioButtonPages).TabIndex = 2;
		((Control)radioButtonPages).Text = "Pages";
		((Control)radioButtonCurrentPage).AutoSize = true;
		((Control)radioButtonCurrentPage).ForeColor = SystemColors.ControlText;
		((Control)radioButtonCurrentPage).Location = new Point(12, 49);
		((Control)radioButtonCurrentPage).Name = "radioButtonCurrentPage";
		((Control)radioButtonCurrentPage).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonCurrentPage).Size = new Size(96, 17);
		((Control)radioButtonCurrentPage).TabIndex = 1;
		((Control)radioButtonCurrentPage).Text = "CurrentPage";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)groupBoxPageRange);
		((Control)this).Name = "StiPagesRangeControl";
		((Control)this).Size = new Size(304, 155);
		((Control)groupBoxPageRange).ResumeLayout(false);
		((Control)groupBoxPageRange).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
