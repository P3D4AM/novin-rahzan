using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiDashboardEditorForm : StiElementEditorForm
{
	protected bool textLock;

	private StiDashboard dashboard;

	private string prevWidth;

	private string prevHeight;

	private IContainer components;

	private ToolTip toolTip;

	private StiTextBox textBoxHeight;

	private StiLabelUX labelHeight;

	private StiLabelUX labelWidth;

	private StiTextBox textBoxWidth;

	private StiCheckBoxUX checkBoxScaleContent;

	private StiOperationButton buttonMakeHorizontalSpacingEqual;

	private StiOperationButton buttonMakeVerticalSpacingEqual;

	private StiComboBox comboBoxContentAlignment;

	private StiLabelUX labelContentAlignment;

	private StiButtonPanelUX groupButtonsPanel;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiGroupPanelUX groupAlignmentPanel;

	internal override StiDesignerControl Designer => dashboard?.Report?.Designer?.DesignerControl as StiDesignerControl;

	public override bool AllowResize => false;

	private string LocAlignmentLeft => Loc.GetEnum("StiDashboardContentAlignmentLeft");

	private string LocAlignmentCenter => Loc.GetEnum("StiDashboardContentAlignmentCenter");

	private string LocAlignmentRight => Loc.GetEnum("StiDashboardContentAlignmentRight");

	private string LocAlignmentStretchX => Loc.GetEnum("StiDashboardContentAlignmentStretchX");

	private string LocAlignmentStretchXY => Loc.GetEnum("StiDashboardContentAlignmentStretchXY");

	public StiDashboardEditorForm(StiDashboard dashboard)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.dashboard = dashboard;
			LocalizeInternal();
			((Control)textBoxWidth).Text = dashboard.Width.ToString();
			((Control)textBoxHeight).Text = dashboard.Height.ToString();
			if (dashboard.ContentAlignment == StiDashboardContentAlignment.Left)
			{
				((ComboBox)comboBoxContentAlignment).SelectedItem = LocAlignmentLeft;
			}
			else if (dashboard.ContentAlignment == StiDashboardContentAlignment.Center)
			{
				((ComboBox)comboBoxContentAlignment).SelectedItem = LocAlignmentCenter;
			}
			else if (dashboard.ContentAlignment == StiDashboardContentAlignment.Right)
			{
				((ComboBox)comboBoxContentAlignment).SelectedItem = LocAlignmentRight;
			}
			else if (dashboard.ContentAlignment == StiDashboardContentAlignment.StretchX)
			{
				((ComboBox)comboBoxContentAlignment).SelectedItem = LocAlignmentStretchX;
			}
			else if (dashboard.ContentAlignment == StiDashboardContentAlignment.StretchXY)
			{
				((ComboBox)comboBoxContentAlignment).SelectedItem = LocAlignmentStretchXY;
			}
			DoScaling();
		}
	}

	private void DoScaling()
	{
		((Control)textBoxWidth).AutoSize = false;
		((Control)textBoxWidth).Size = new Size(((Control)textBoxWidth).Width, StiScale.I(20));
		((Control)textBoxHeight).AutoSize = false;
		((Control)textBoxHeight).Size = new Size(((Control)textBoxHeight).Width, StiScale.I(20));
		((ButtonBase)buttonMakeHorizontalSpacingEqual).Image = StiReportDesignImages.Layout.MakeHorizontalSpacingEqual();
		((ButtonBase)buttonMakeVerticalSpacingEqual).Image = StiReportDesignImages.Layout.MakeVerticalSpacingEqual();
	}

	internal static double Round(double value)
	{
		return Math.Round(value, MidpointRounding.AwayFromZero);
	}

	internal static RectangleD Round(RectangleD rect)
	{
		return new RectangleD(Round(rect.Left), Round(rect.Top), Round(rect.Width), Round(rect.Height));
	}

	private void ResizeDashboard(bool allowRescale)
	{
		bool flag = false;
		double width = dashboard.Width;
		double height = dashboard.Height;
		if (prevWidth != ((Control)textBoxWidth).Text && double.TryParse(((Control)textBoxWidth).Text, out var result))
		{
			result = Math.Max(50.0, result);
			result = Math.Min(10000.0, result);
			((Control)textBoxWidth).Text = result.ToString();
			dashboard.Width = result;
			prevWidth = ((Control)textBoxWidth).Text;
			flag = true;
		}
		if (prevHeight != ((Control)textBoxHeight).Text && double.TryParse(((Control)textBoxHeight).Text, out result))
		{
			result = Math.Max(50.0, result);
			result = Math.Min(10000.0, result);
			((Control)textBoxHeight).Text = result.ToString();
			dashboard.Height = result;
			prevHeight = ((Control)textBoxHeight).Text;
			flag = true;
		}
		if (flag)
		{
			Control val = FindFocusedControl((Control)(object)this);
			((StiDesignerControl)dashboard.Report.Designer.DesignerControl)?.Builder.BuildReport();
			if (((CheckBox)checkBoxScaleContent).Checked && allowRescale)
			{
				StiDashboardSizeHelper.Place(dashboard, width, height);
			}
			val.Focus();
		}
	}

	public static Control FindFocusedControl(Control control)
	{
		for (IContainerControl val = (IContainerControl)(object)((control is IContainerControl) ? control : null); val != null; val = (IContainerControl)(object)((control is IContainerControl) ? control : null))
		{
			control = val.ActiveControl;
		}
		return control;
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiDashboard");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelWidth).Text = Loc.GetMain("Width");
		((Control)labelHeight).Text = Loc.GetMain("Height");
		((Control)checkBoxScaleContent).Text = StiLocalization.Get("FormPageSetup", "ScaleContent");
		((Control)labelContentAlignment).Text = Loc.GetMain("ContentAlignment");
		toolTip.SetToolTip((Control)(object)buttonMakeHorizontalSpacingEqual, Loc.Get("Toolbars", "MakeHorizontalSpacingEqual"));
		toolTip.SetToolTip((Control)(object)buttonMakeVerticalSpacingEqual, Loc.Get("Toolbars", "MakeVerticalSpacingEqual"));
		((ComboBox)comboBoxContentAlignment).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxContentAlignment).SelectedIndex;
		((ComboBox)comboBoxContentAlignment).Items.Clear();
		((ComboBox)comboBoxContentAlignment).Items.Add((object)LocAlignmentLeft);
		((ComboBox)comboBoxContentAlignment).Items.Add((object)LocAlignmentCenter);
		((ComboBox)comboBoxContentAlignment).Items.Add((object)LocAlignmentRight);
		((ComboBox)comboBoxContentAlignment).Items.Add((object)LocAlignmentStretchX);
		((ComboBox)comboBoxContentAlignment).Items.Add((object)LocAlignmentStretchXY);
		((ListControl)comboBoxContentAlignment).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxContentAlignment).EndUpdate();
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		((Form)this).OnClosed(e);
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ScaleContent", ((CheckBox)checkBoxScaleContent).Checked);
		StiSettings.Save();
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
				else
				{
					ResizeDashboard(allowRescale: true);
				}
			}
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		StiSettings.Load();
		((CheckBox)checkBoxScaleContent).Checked = StiSettings.GetBool(((Control)this).Name, "ScaleContent", nullValue: true);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void TextBoxSize_Enter(object sender, EventArgs e)
	{
		prevWidth = ((Control)textBoxWidth).Text;
		prevHeight = ((Control)textBoxHeight).Text;
	}

	private void TextBoxSize_Leave(object sender, EventArgs e)
	{
		ResizeDashboard(allowRescale: true);
	}

	private void TextBoxSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 13)
		{
			ResizeDashboard(allowRescale: true);
			if (sender is TextBox)
			{
				((Form)this).Activate();
				((Control)(TextBox)sender).Focus();
			}
		}
	}

	private void ButtonMakeHorizontalSpacingEqual_Click(object sender, EventArgs e)
	{
		if (dashboard.Components.Count != 0)
		{
			IEnumerable<RectangleD> source = from c in dashboard.Components.ToList()
				select Round(c.GetPaintRectangle(convertToHInches: false, convertByZoom: false));
			double num = source.Min((RectangleD r) => r.Left);
			double num2 = source.Max((RectangleD r) => r.Right);
			if (num < 0.0)
			{
				num = 0.0;
			}
			num2 += num;
			((Control)this).SuspendLayout();
			((Control)textBoxWidth).Text = StiAlignHelper.AlignToGrid(num2, dashboard.Report.Info.GridSize, dashboard.Report.Info.AlignToGrid).ToString();
			ResizeDashboard(allowRescale: false);
			((Control)textBoxWidth).Focus();
			((Control)this).ResumeLayout();
		}
	}

	private void ButtonMakeVerticalSpacingEqual_Click(object sender, EventArgs e)
	{
		if (dashboard.Components.Count != 0)
		{
			IEnumerable<RectangleD> source = from c in dashboard.Components.ToList()
				select Round(c.GetPaintRectangle(convertToHInches: false, convertByZoom: false));
			double num = source.Min((RectangleD r) => r.Top);
			double num2 = source.Max((RectangleD r) => r.Bottom);
			if (num < 0.0)
			{
				num = 0.0;
			}
			num2 += num;
			((Control)this).SuspendLayout();
			((Control)textBoxHeight).Text = StiAlignHelper.AlignToGrid(num2, dashboard.Report.Info.GridSize, dashboard.Report.Info.AlignToGrid).ToString();
			ResizeDashboard(allowRescale: false);
			((Control)textBoxHeight).Focus();
			((Control)this).ResumeLayout();
		}
	}

	private void ComboBoxContentAlignment_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = ((ComboBox)comboBoxContentAlignment).SelectedItem as string;
		if (text == LocAlignmentLeft)
		{
			dashboard.ContentAlignment = StiDashboardContentAlignment.Left;
		}
		else if (text == LocAlignmentCenter)
		{
			dashboard.ContentAlignment = StiDashboardContentAlignment.Center;
		}
		else if (text == LocAlignmentRight)
		{
			dashboard.ContentAlignment = StiDashboardContentAlignment.Right;
		}
		else if (text == LocAlignmentStretchX)
		{
			dashboard.ContentAlignment = StiDashboardContentAlignment.StretchX;
		}
		else if (text == LocAlignmentStretchXY)
		{
			dashboard.ContentAlignment = StiDashboardContentAlignment.StretchXY;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDashboardEditorForm));
		toolTip = new ToolTip(components);
		groupAlignmentPanel = new StiGroupPanelUX();
		comboBoxContentAlignment = new StiComboBox();
		labelContentAlignment = new StiLabelUX();
		groupButtonsPanel = new StiButtonPanelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		buttonMakeVerticalSpacingEqual = new StiOperationButton();
		buttonMakeHorizontalSpacingEqual = new StiOperationButton();
		checkBoxScaleContent = new StiCheckBoxUX();
		textBoxHeight = new StiTextBox();
		labelHeight = new StiLabelUX();
		labelWidth = new StiLabelUX();
		textBoxWidth = new StiTextBox();
		((Control)groupAlignmentPanel).SuspendLayout();
		((Control)groupButtonsPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupAlignmentPanel).Controls.Add((Control)(object)comboBoxContentAlignment);
		((Control)groupAlignmentPanel).Controls.Add((Control)(object)labelContentAlignment);
		((Control)groupAlignmentPanel).Dock = (DockStyle)2;
		groupAlignmentPanel.LinePosition = StiLinePosition.Top;
		groupAlignmentPanel.LineStyle = StiLineStyle.Dot;
		((Control)groupAlignmentPanel).Location = new Point(0, 204);
		((Control)groupAlignmentPanel).Margin = new Padding(4);
		((Control)groupAlignmentPanel).Name = "groupAlignmentPanel";
		((Control)groupAlignmentPanel).Size = new Size(720, 96);
		((Control)groupAlignmentPanel).TabIndex = 5;
		((Control)comboBoxContentAlignment).Anchor = (AnchorStyles)13;
		comboBoxContentAlignment.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxContentAlignment).DropDownWidth = 200;
		((Control)comboBoxContentAlignment).Location = new Point(294, 23);
		((Control)comboBoxContentAlignment).Margin = new Padding(6);
		((Control)comboBoxContentAlignment).Name = "comboBoxContentAlignment";
		((Control)comboBoxContentAlignment).Size = new Size(292, 42);
		((Control)comboBoxContentAlignment).TabIndex = 0;
		((ComboBox)comboBoxContentAlignment).SelectedIndexChanged += ComboBoxContentAlignment_SelectedIndexChanged;
		((Control)labelContentAlignment).Location = new Point(24, 29);
		((Control)labelContentAlignment).Margin = new Padding(6, 0, 6, 0);
		((Control)labelContentAlignment).Name = "labelContentAlignment";
		((Control)labelContentAlignment).Size = new Size(258, 33);
		((Control)labelContentAlignment).TabIndex = 9;
		((Control)labelContentAlignment).Text = "Content Alignment";
		((Label)labelContentAlignment).TextAlign = (ContentAlignment)16;
		((Control)groupButtonsPanel).Controls.Add((Control)(object)buttonOk);
		((Control)groupButtonsPanel).Controls.Add((Control)(object)buttonCancel);
		((Control)groupButtonsPanel).Dock = (DockStyle)2;
		groupButtonsPanel.LinePosition = StiLinePosition.Top;
		groupButtonsPanel.LineStyle = StiLineStyle.Dot;
		((Control)groupButtonsPanel).Location = new Point(0, 300);
		((Control)groupButtonsPanel).Margin = new Padding(4);
		((Control)groupButtonsPanel).Name = "groupButtonsPanel";
		((Control)groupButtonsPanel).Size = new Size(720, 88);
		((Control)groupButtonsPanel).TabIndex = 6;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Button)buttonOk).DialogResult = (DialogResult)1;
		buttonOk.FlatStyle = (FlatStyle)0;
		((Control)buttonOk).Location = new Point(378, 23);
		((Control)buttonOk).Margin = new Padding(4);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		buttonCancel.FlatStyle = (FlatStyle)0;
		((Control)buttonCancel).Location = new Point(544, 23);
		((Control)buttonCancel).Margin = new Padding(4);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((ButtonBase)buttonMakeVerticalSpacingEqual).Image = (Image)componentResourceManager.GetObject("buttonMakeVerticalSpacingEqual.Image");
		((Control)buttonMakeVerticalSpacingEqual).Location = new Point(544, 85);
		((Control)buttonMakeVerticalSpacingEqual).Margin = new Padding(4);
		((Control)buttonMakeVerticalSpacingEqual).Name = "buttonMakeVerticalSpacingEqual";
		((Control)buttonMakeVerticalSpacingEqual).Size = new Size(40, 42);
		((Control)buttonMakeVerticalSpacingEqual).TabIndex = 3;
		((Control)buttonMakeVerticalSpacingEqual).TabStop = false;
		((ButtonBase)buttonMakeVerticalSpacingEqual).UseVisualStyleBackColor = true;
		((Control)buttonMakeVerticalSpacingEqual).Click += ButtonMakeVerticalSpacingEqual_Click;
		((ButtonBase)buttonMakeHorizontalSpacingEqual).Image = (Image)componentResourceManager.GetObject("buttonMakeHorizontalSpacingEqual.Image");
		((Control)buttonMakeHorizontalSpacingEqual).Location = new Point(544, 23);
		((Control)buttonMakeHorizontalSpacingEqual).Margin = new Padding(4);
		((Control)buttonMakeHorizontalSpacingEqual).Name = "buttonMakeHorizontalSpacingEqual";
		((Control)buttonMakeHorizontalSpacingEqual).Size = new Size(40, 42);
		((Control)buttonMakeHorizontalSpacingEqual).TabIndex = 1;
		((Control)buttonMakeHorizontalSpacingEqual).TabStop = false;
		((ButtonBase)buttonMakeHorizontalSpacingEqual).UseVisualStyleBackColor = true;
		((Control)buttonMakeHorizontalSpacingEqual).Click += ButtonMakeHorizontalSpacingEqual_Click;
		((Control)checkBoxScaleContent).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxScaleContent).AutoEllipsis = true;
		((CheckBox)checkBoxScaleContent).Checked = true;
		((CheckBox)checkBoxScaleContent).CheckState = (CheckState)1;
		((Control)checkBoxScaleContent).Location = new Point(294, 146);
		((Control)checkBoxScaleContent).Margin = new Padding(4);
		((Control)checkBoxScaleContent).Name = "checkBoxScaleContent";
		((Control)checkBoxScaleContent).Size = new Size(400, 35);
		((Control)checkBoxScaleContent).TabIndex = 4;
		((Control)checkBoxScaleContent).Text = "Scale Content";
		((ButtonBase)checkBoxScaleContent).UseVisualStyleBackColor = true;
		((Control)textBoxHeight).Location = new Point(294, 85);
		((Control)textBoxHeight).Margin = new Padding(6);
		((Control)textBoxHeight).Name = "textBoxHeight";
		((Control)textBoxHeight).Size = new Size(240, 42);
		((Control)textBoxHeight).TabIndex = 2;
		textBoxHeight.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxHeight).Enter += TextBoxSize_Enter;
		((Control)textBoxHeight).KeyDown += new KeyEventHandler(TextBoxSize_KeyDown);
		((Control)textBoxHeight).Leave += TextBoxSize_Leave;
		((Control)labelHeight).Location = new Point(24, 85);
		((Control)labelHeight).Margin = new Padding(6, 0, 6, 0);
		((Control)labelHeight).Name = "labelHeight";
		((Control)labelHeight).Size = new Size(196, 38);
		((Control)labelHeight).TabIndex = 4;
		((Control)labelHeight).Text = "Height";
		((Label)labelHeight).TextAlign = (ContentAlignment)16;
		((Control)labelWidth).Location = new Point(24, 23);
		((Control)labelWidth).Margin = new Padding(6, 0, 6, 0);
		((Control)labelWidth).Name = "labelWidth";
		((Control)labelWidth).Size = new Size(196, 38);
		((Control)labelWidth).TabIndex = 3;
		((Control)labelWidth).Text = "Width";
		((Label)labelWidth).TextAlign = (ContentAlignment)16;
		((Control)textBoxWidth).Location = new Point(294, 23);
		((Control)textBoxWidth).Margin = new Padding(6);
		((Control)textBoxWidth).Name = "textBoxWidth";
		((Control)textBoxWidth).Size = new Size(240, 42);
		((Control)textBoxWidth).TabIndex = 0;
		textBoxWidth.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxWidth).Enter += TextBoxSize_Enter;
		((Control)textBoxWidth).KeyDown += new KeyEventHandler(TextBoxSize_KeyDown);
		((Control)textBoxWidth).Leave += TextBoxSize_Leave;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(720, 388);
		((Control)this).Controls.Add((Control)(object)groupAlignmentPanel);
		((Control)this).Controls.Add((Control)(object)groupButtonsPanel);
		((Control)this).Controls.Add((Control)(object)buttonMakeVerticalSpacingEqual);
		((Control)this).Controls.Add((Control)(object)buttonMakeHorizontalSpacingEqual);
		((Control)this).Controls.Add((Control)(object)checkBoxScaleContent);
		((Control)this).Controls.Add((Control)(object)textBoxHeight);
		((Control)this).Controls.Add((Control)(object)labelHeight);
		((Control)this).Controls.Add((Control)(object)labelWidth);
		((Control)this).Controls.Add((Control)(object)textBoxWidth);
		((Form)this).Margin = new Padding(6);
		((Control)this).Name = "StiDashboardEditorForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Dashboard";
		((Control)groupAlignmentPanel).ResumeLayout(false);
		((Control)groupButtonsPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
