using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Dashboard.Design.Editors.OnlineMap;

public class StiHeatmapGradientForm : Form
{
	public List<StiHeatmapGradientRange> Ranges = new List<StiHeatmapGradientRange>();

	public int Raduis;

	private StiReport report;

	internal StiDesignerControl designer;

	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiGroupLine panelButtons;

	private Panel panelRangeOption;

	private StiPanelBox panelBoxRangeStartExpression;

	private HintTextBox textBoxRangeStartExpression;

	private StiOperationButton buttonRangeStartExpression;

	private ColorPickerButton colorBoxRangeColor;

	private StiHeatmapGradientListBox listBoxRanges;

	private Panel panelRangeUi;

	private Button buttonAddRange;

	private Label labelRadius;

	private StiNumericUpDown numericRadius;

	public bool LockRefresh { get; set; }

	internal StiHeatmapGradientRange HeatmapRange => listBoxRanges.SelectedRangeItem?.Range;

	protected int PaddingCorrection
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Padding padding = ((Control)this).Padding;
			int top = ((Padding)(ref padding)).Top;
			padding = ((Control)this).Padding;
			return top + ((Padding)(ref padding)).Bottom + StiScale.I4;
		}
	}

	public StiHeatmapGradientForm(StiReport report, StiElementEditorForm editor, StiDesignerControl designer, List<StiHeatmapGradientRange> ranges, int radius)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		this.report = report;
		this.designer = designer;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		numericRadius.Minimum = 1m;
		numericRadius.Maximum = 300m;
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = designer;
			item.Editor = editor;
			item.AllowMultipleElements = false;
			item.AllowImage = false;
		}
		listBoxRanges.Designer = designer;
		listBoxRanges.Editor = editor;
		listBoxRanges.AllowImage = false;
		StiFormSettings.Load((Form)(object)this, skipLocation: false, skipSize: true);
		Localize();
		Init(ranges, radius);
		DoScaling();
		((Form)this).ShowIcon = false;
	}

	private void Localize()
	{
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelRadius).Text = StiLocalization.Get("PropertyMain", "Radius");
		((Control)this).Text = Loc.Get("PropertyEnum", "StiOnlineMapHeatmapColorGradientTypeSteppedColors");
		((Control)buttonAddRange).Text = Loc.Get("PropertyEnum", "StiOnlineMapHeatmapColorGradientTypeAddRange");
	}

	private void DoScaling()
	{
		((Control)buttonAddRange).Left = ((Control)numericRadius).Left;
		((Control)buttonAddRange).Width = ((Control)panelRangeUi).Width - ((Control)buttonAddRange).Left;
		int num = ((Control)panelButtons).Width - StiScale.I1 - 1;
		num -= ((Control)buttonCancel).Width - 1;
		((Control)buttonCancel).Left = num;
		((ButtonBase)buttonRangeStartExpression).Image = StiReportDesignImages.Actions.Edit();
	}

	private void Init(List<StiHeatmapGradientRange> ranges, int radius)
	{
		numericRadius.Value = radius;
		Ranges.AddRange(ranges);
		ReadRangesPropertiesFromElement();
		listBoxRanges.SelectFirstItem();
		listBoxRanges.ElementsChanged += delegate
		{
			Ranges = (from t in listBoxRanges.ToItems()
				select t.Range).ToList();
			RefreshUiPanel();
		};
	}

	private Color GetPreferedColor()
	{
		if (Ranges.All((StiHeatmapGradientRange r) => r.Color != Color.Green))
		{
			return Color.Green;
		}
		if (Ranges.All((StiHeatmapGradientRange r) => r.Color != Color.Yellow))
		{
			return Color.Yellow;
		}
		if (Ranges.All((StiHeatmapGradientRange r) => r.Color != Color.Red))
		{
			return Color.Red;
		}
		return Color.Orange;
	}

	private void ReadRangesPropertiesFromElement()
	{
		((ListBox)listBoxRanges).BeginUpdate();
		((ListBox)listBoxRanges).Items.Clear();
		listBoxRanges.AddItems(Ranges);
		listBoxRanges.SelectFirstItem();
		((ListBox)listBoxRanges).EndUpdate();
	}

	internal void RefreshRange()
	{
		if (HeatmapRange != null)
		{
			LockRefresh = true;
			colorBoxRangeColor.SelectedColor = HeatmapRange.Color;
			((Control)textBoxRangeStartExpression).Text = HeatmapRange.StartExpression;
			LockRefresh = false;
			RefreshUiPanel();
			((Control)colorBoxRangeColor).Invalidate();
		}
	}

	protected virtual void SetListHeightFromCount(StiHeatmapGradientListBox list)
	{
		int num = Math.Min(10, ((ListBox)list).Items.Count);
		((Control)list).Height = Math.Min(num + 1, 7) * ((ListBox)list).ItemHeight;
	}

	public void RefreshUiPanel()
	{
		if (LockRefresh)
		{
			return;
		}
		LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			SetListHeightFromCount(listBoxRanges);
			CalculateWindowHeight();
		}
		finally
		{
			((Control)this).ResumeLayout();
			LockRefresh = false;
		}
	}

	protected void CalculateWindowHeight()
	{
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, PaddingCorrection + (from Control c in (IEnumerable)((Control)this).Controls
			select c.Height).Sum());
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Raduis = (int)numericRadius.Value;
		Ranges = (from t in listBoxRanges.ToItems()
			select t.Range).ToList();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void ButtonAddRange_Click(object sender, EventArgs e)
	{
		Ranges.Add(new StiHeatmapGradientRange
		{
			Color = GetPreferedColor()
		});
		ReadRangesPropertiesFromElement();
		listBoxRanges.SelectLastItem();
	}

	private void ListBoxRanges_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!LockRefresh)
		{
			RefreshRange();
		}
	}

	private void ListBoxRanges_Enter(object sender, EventArgs e)
	{
		RefreshRange();
	}

	private void ListBoxRanges_MouseDown(object sender, MouseEventArgs e)
	{
		RefreshRange();
	}

	private void ColorBoxRangeColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!LockRefresh && HeatmapRange != null)
		{
			HeatmapRange.Color = colorBoxRangeColor.SelectedColor;
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void TextBoxRangeStartExpression_TextChanged(object sender, EventArgs e)
	{
		if (!LockRefresh && HeatmapRange != null)
		{
			HeatmapRange.StartExpression = ((Control)textBoxRangeStartExpression).Text;
			listBoxRanges.InvalidateSelectedItem();
		}
	}

	private void ButtonRangeStartExpression_Click(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		HintTextBox hintTextBox = textBoxRangeStartExpression;
		string text = ((Control)hintTextBox).Text;
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorDashboardExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)hintTextBox).Text = text;
		}
		((Control)hintTextBox).Focus();
		((TextBoxBase)hintTextBox).Select(0, 0);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiHeatmapGradientForm));
		panelRangeOption = new Panel();
		colorBoxRangeColor = new ColorPickerButton();
		panelRangeUi = new Panel();
		labelRadius = new Label();
		buttonOk = new Button();
		buttonCancel = new Button();
		panelButtons = new StiGroupLine();
		numericRadius = new StiNumericUpDown();
		panelBoxRangeStartExpression = new StiPanelBox();
		textBoxRangeStartExpression = new HintTextBox();
		buttonRangeStartExpression = new StiOperationButton();
		listBoxRanges = new StiHeatmapGradientListBox();
		buttonAddRange = new Button();
		((Control)panelRangeOption).SuspendLayout();
		((Control)panelRangeUi).SuspendLayout();
		((Control)panelButtons).SuspendLayout();
		((ISupportInitialize)numericRadius).BeginInit();
		((Control)panelBoxRangeStartExpression).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelRangeOption).BackColor = Color.White;
		((Control)panelRangeOption).Controls.Add((Control)(object)panelBoxRangeStartExpression);
		((Control)panelRangeOption).Controls.Add((Control)(object)colorBoxRangeColor);
		((Control)panelRangeOption).Dock = (DockStyle)1;
		((Control)panelRangeOption).Location = new Point(12, 156);
		((Control)panelRangeOption).Name = "panelRangeOption";
		((Control)panelRangeOption).Size = new Size(265, 49);
		((Control)panelRangeOption).TabIndex = 30;
		((Control)colorBoxRangeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxRangeColor.AutoExpandOnClick = true;
		colorBoxRangeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxRangeColor.EnableMarkup = false;
		colorBoxRangeColor.Is20Button = false;
		((Control)colorBoxRangeColor).Location = new Point(0, 12);
		((Control)colorBoxRangeColor).Name = "colorBoxRangeColor";
		((Control)colorBoxRangeColor).Size = new Size(80, 25);
		((Control)colorBoxRangeColor).TabIndex = 60;
		((Control)colorBoxRangeColor).TabStop = false;
		colorBoxRangeColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxRangeColor.SelectedColorChanged += ColorBoxRangeColor_SelectedColorChanged;
		((Control)panelRangeUi).BackColor = Color.White;
		((Control)panelRangeUi).Controls.Add((Control)(object)numericRadius);
		((Control)panelRangeUi).Controls.Add((Control)(object)labelRadius);
		((Control)panelRangeUi).Controls.Add((Control)(object)buttonAddRange);
		((Control)panelRangeUi).Dock = (DockStyle)1;
		((Control)panelRangeUi).Location = new Point(12, 12);
		((Control)panelRangeUi).Margin = new Padding(2);
		((Control)panelRangeUi).Name = "panelRangeUi";
		((Control)panelRangeUi).Size = new Size(265, 72);
		((Control)panelRangeUi).TabIndex = 0;
		((Control)labelRadius).ForeColor = Color.DimGray;
		((Control)labelRadius).Location = new Point(0, 6);
		((Control)labelRadius).Margin = new Padding(2, 0, 2, 0);
		((Control)labelRadius).Name = "labelMinimum";
		((Control)labelRadius).Size = new Size(83, 17);
		((Control)labelRadius).TabIndex = 3;
		((Control)labelRadius).Text = "Radius";
		labelRadius.TextAlign = (ContentAlignment)16;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(92, 12);
		((Control)buttonOk).Name = "btOk";
		((Control)buttonOk).Size = new Size(79, 25);
		((Control)buttonOk).TabIndex = 70;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(180, 12);
		((Control)buttonCancel).Name = "btCancel";
		((Control)buttonCancel).Size = new Size(86, 25);
		((Control)buttonCancel).TabIndex = 80;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelButtons).BackColor = Color.White;
		((Control)panelButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)panelButtons).Controls.Add((Control)(object)buttonOk);
		((Control)panelButtons).Dock = (DockStyle)1;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(12, 205);
		((Control)panelButtons).Name = "stiGroupLine1";
		((Control)panelButtons).Size = new Size(265, 44);
		((Control)panelButtons).TabIndex = 10;
		((Control)numericRadius).BackColor = Color.White;
		((Control)numericRadius).Location = new Point(92, 6);
		((Control)numericRadius).Margin = new Padding(2);
		((Control)numericRadius).Name = "numericRadius";
		((Control)numericRadius).Size = new Size(173, 21);
		((Control)numericRadius).TabIndex = 5;
		((Control)panelBoxRangeStartExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxRangeStartExpression).BackColor = Color.White;
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)textBoxRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Controls.Add((Control)(object)buttonRangeStartExpression);
		((Control)panelBoxRangeStartExpression).Location = new Point(92, 12);
		((Control)panelBoxRangeStartExpression).Margin = new Padding(2);
		((Control)panelBoxRangeStartExpression).Name = "panelBoxRangeStartExpression";
		((Control)panelBoxRangeStartExpression).Size = new Size(173, 25);
		((Control)panelBoxRangeStartExpression).TabIndex = 35;
		((Control)textBoxRangeStartExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxRangeStartExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxRangeStartExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxRangeStartExpression).Location = new Point(4, 6);
		((Control)textBoxRangeStartExpression).Margin = new Padding(2);
		((Control)textBoxRangeStartExpression).Name = "textBoxRangeStartExpression";
		((Control)textBoxRangeStartExpression).Size = new Size(143, 13);
		((Control)textBoxRangeStartExpression).TabIndex = 40;
		((Control)textBoxRangeStartExpression).TextChanged += TextBoxRangeStartExpression_TextChanged;
		((Control)buttonRangeStartExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRangeStartExpression).Image = (Image)componentResourceManager.GetObject("buttonRangeStartExpression.Image");
		((Control)buttonRangeStartExpression).Location = new Point(149, 1);
		((Control)buttonRangeStartExpression).Margin = new Padding(2);
		((Control)buttonRangeStartExpression).Name = "buttonRangeStartExpression";
		((Control)buttonRangeStartExpression).Size = new Size(23, 23);
		((Control)buttonRangeStartExpression).TabIndex = 1;
		((Control)buttonRangeStartExpression).TabStop = false;
		((ButtonBase)buttonRangeStartExpression).UseVisualStyleBackColor = true;
		((Control)buttonRangeStartExpression).Click += ButtonRangeStartExpression_Click;
		((Control)listBoxRanges).AllowDrop = true;
		((Control)listBoxRanges).Dock = (DockStyle)1;
		((Control)listBoxRanges).Location = new Point(12, 84);
		((Control)listBoxRanges).Margin = new Padding(2);
		((Control)listBoxRanges).Name = "listBoxRanges";
		((Control)listBoxRanges).Size = new Size(265, 72);
		((Control)listBoxRanges).TabIndex = 20;
		((ListBox)listBoxRanges).SelectedIndexChanged += ListBoxRanges_SelectedIndexChanged;
		((Control)listBoxRanges).Enter += ListBoxRanges_Enter;
		((Control)listBoxRanges).MouseDown += new MouseEventHandler(ListBoxRanges_MouseDown);
		((ButtonBase)buttonAddRange).FlatAppearance.BorderSize = 0;
		((Control)buttonAddRange).Location = new Point(91, 38);
		((Control)buttonAddRange).Margin = new Padding(2);
		((Control)buttonAddRange).Name = "buttonAddRange";
		((Control)buttonAddRange).Size = new Size(175, 23);
		((Control)buttonAddRange).TabIndex = 10;
		((Control)buttonAddRange).Text = "Add Range";
		((ButtonBase)buttonAddRange).UseVisualStyleBackColor = true;
		((Control)buttonAddRange).Click += ButtonAddRange_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(289, 259);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Control)this).Controls.Add((Control)(object)panelRangeOption);
		((Control)this).Controls.Add((Control)(object)listBoxRanges);
		((Control)this).Controls.Add((Control)(object)panelRangeUi);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiHeatmapGradientForm";
		((Control)this).Padding = new Padding(12, 12, 12, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiMapCategoriesForm";
		((Form)this).Closed += This_Closed;
		((Control)panelRangeOption).ResumeLayout(false);
		((Control)panelRangeUi).ResumeLayout(false);
		((Control)panelButtons).ResumeLayout(false);
		((ISupportInitialize)numericRadius).EndInit();
		((Control)panelBoxRangeStartExpression).ResumeLayout(false);
		((Control)panelBoxRangeStartExpression).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
