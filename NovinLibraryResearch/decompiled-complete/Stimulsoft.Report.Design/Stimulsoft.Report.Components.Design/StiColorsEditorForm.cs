using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiColorsEditorForm : Form
{
	private IContainer components;

	private StiZoneContainerControl zoneContainerColors;

	private Button buttonCancel;

	private Button buttonOk;

	private StiGroupLine stiGroupLine1;

	private Button buttonAddColor;

	public Color[] Colors { get; set; }

	public StiColorsEditorForm(Color[] colors)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Colors = colors;
			Localize();
			BuildColors();
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Colors");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddColor).Text = StiLocalization.Get("Buttons", "Add");
	}

	public void FillColors()
	{
		List<Color> list = new List<Color>();
		for (int num = ((ArrangedElementCollection)((Control)zoneContainerColors).Controls).Count - 1; num >= 0; num--)
		{
			StiColorZoneControl stiColorZoneControl = ((Control)zoneContainerColors).Controls[num] as StiColorZoneControl;
			list.Add(stiColorZoneControl.SelectedColor);
		}
		Colors = list.ToArray();
	}

	public void BuildColors()
	{
		if (Colors != null)
		{
			Color[] colors = Colors;
			foreach (Color color in colors)
			{
				StiColorZoneControl stiColorZoneControl = new StiColorZoneControl(color);
				((Control)zoneContainerColors).Controls.Add((Control)(object)stiColorZoneControl);
				((Control)zoneContainerColors).Controls.SetChildIndex((Control)(object)stiColorZoneControl, 0);
			}
		}
	}

	private void ButtonAddColor_Click(object sender, EventArgs e)
	{
		StiColorZoneControl stiColorZoneControl = new StiColorZoneControl(Color.Green);
		((Control)zoneContainerColors).Controls.Add((Control)(object)stiColorZoneControl);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_conditional_formatting.htm");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		FillColors();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		buttonOk = new Button();
		buttonCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		zoneContainerColors = new StiZoneContainerControl();
		buttonAddColor = new Button();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(138, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 62;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(219, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 63;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 424);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(306, 46);
		((Control)stiGroupLine1).TabIndex = 67;
		((Control)zoneContainerColors).AllowDrop = true;
		((Control)zoneContainerColors).Anchor = (AnchorStyles)15;
		((ScrollableControl)zoneContainerColors).AutoScroll = true;
		((Control)zoneContainerColors).BackColor = Color.White;
		zoneContainerColors.EmptyImage = null;
		zoneContainerColors.EmptyText = null;
		((Control)zoneContainerColors).Location = new Point(12, 49);
		((Control)zoneContainerColors).Margin = new Padding(2, 2, 2, 2);
		((Control)zoneContainerColors).Name = "zoneContainerColors";
		((Control)zoneContainerColors).Padding = new Padding(2);
		zoneContainerColors.ShowBorder = true;
		((Control)zoneContainerColors).Size = new Size(281, 361);
		((Control)zoneContainerColors).TabIndex = 61;
		((Control)buttonAddColor).Location = new Point(12, 12);
		((Control)buttonAddColor).Name = "buttonAddColor";
		((Control)buttonAddColor).Size = new Size(113, 24);
		((Control)buttonAddColor).TabIndex = 75;
		((Control)buttonAddColor).Text = "Add Condition";
		((ButtonBase)buttonAddColor).UseVisualStyleBackColor = true;
		((Control)buttonAddColor).Click += ButtonAddColor_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(306, 470);
		((Control)this).Controls.Add((Control)(object)buttonAddColor);
		((Control)this).Controls.Add((Control)(object)zoneContainerColors);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(295, 391);
		((Control)this).Name = "StiColorsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Colors";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
