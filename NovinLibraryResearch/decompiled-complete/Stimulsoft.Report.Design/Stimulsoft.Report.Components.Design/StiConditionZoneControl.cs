using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiConditionZoneControl : StiZoneControl
{
	internal bool tbFontStyleStrikeOutPushed;

	private StiReport report;

	private StiComponent component;

	private bool allowAssingExpression;

	internal bool blockUpdateSample;

	private Font selectedFont;

	private Color selectedTextColor;

	private Color selectedBackColor;

	private IContainer components;

	private Panel panel1;

	private Label lbAaBbCc;

	private StiToolButton stiToolButton3;

	private ImageList ilButtons;

	public StiToolButton tbFontStyleBold;

	public StiToolButton tbFontStyleItalic;

	public StiColorButtonXF tbBackColor;

	public StiToolButton tbFontStyleUnderline;

	public StiColorButtonXF tbTextColor;

	public CheckBox cbEnabled;

	private StiToolButton stiToolButton1;

	public StiToolButton tbFont;

	private FontDialog fontDialog1;

	private ToolTip toolTip1;

	private StiStyleBox styleBoxSelect;

	public Panel panelConditions;

	public StiToolButton toolButtonAddLevel;

	public RadioButton radioButtonOr;

	public RadioButton radioButtonAnd;

	public StiButtonEdit textBoxAssignExpression;

	private ButtonX buttonBorders;

	public ButtonItem buttonBordersAll;

	public ButtonItem buttonBordersTop;

	public ButtonItem buttonBordersLeft;

	public ButtonItem buttonBordersBottom;

	public ButtonItem buttonBordersRight;

	public ButtonItem buttonBordersNone;

	public StiToolBar tbTool;

	private Panel panel3;

	private StiToolButton stiToolButton4;

	private StiToolButton stiToolButton5;

	private ButtonX buttonPermissions;

	internal ButtonItem buttonItemAllowFontName;

	internal ButtonItem buttonItemAllowFontSize;

	internal ButtonItem buttonItemAllowFontBold;

	private StiToolButton stiToolButton2;

	internal ButtonItem buttonItemAllowFontItalic;

	internal ButtonItem buttonItemAllowFontUnderline;

	internal ButtonItem buttonItemAllowTextColor;

	internal ButtonItem buttonItemAllowBackColor;

	internal ButtonItem buttonItemAllowBorders;

	internal ButtonItem buttonItemAllowFontStrikeout;

	public CheckBox checkBoxCanAssignExpression;

	public StiCheckBoxInfo checkBoxBreakIfTrue;

	private Panel panelBreakIfTrue;

	public StiBaseStyle SelectedStyle
	{
		get
		{
			return styleBoxSelect.SelectedStyle;
		}
		set
		{
			styleBoxSelect.SelectedStyle = value;
		}
	}

	public Font SelectedFont
	{
		get
		{
			return selectedFont;
		}
		set
		{
			selectedFont = value;
		}
	}

	public Color SelectedTextColor
	{
		get
		{
			return selectedTextColor;
		}
		set
		{
			selectedTextColor = value;
		}
	}

	public Color SelectedBackColor
	{
		get
		{
			return selectedBackColor;
		}
		set
		{
			selectedBackColor = value;
		}
	}

	public StiConditionZoneControl(StiReport report, bool allowAssignExpression, StiComponent component)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoScaling();
			this.report = report;
			this.component = component;
			allowAssingExpression = allowAssignExpression;
			Localize();
			LocalizeSuperToolTip();
			styleBoxSelect.Designer = report.Designer;
			styleBoxSelect.ShowNone = true;
			styleBoxSelect.ShowStyle = true;
			styleBoxSelect.ShowPredefinedStyles = true;
			styleBoxSelect.ShowEditStyles = true;
			styleBoxSelect.ShowChartStyles = false;
			styleBoxSelect.ShowCrossTabStyles = false;
			((Control)panelConditions).ControlAdded += new ControlEventHandler(OnControlsChanged);
			((Control)panelConditions).ControlRemoved += new ControlEventHandler(OnControlsChanged);
			((Control)checkBoxCanAssignExpression).Visible = allowAssignExpression;
			((Control)textBoxAssignExpression).Visible = allowAssignExpression;
			UpdateStates();
			tbTool.AllowMove = false;
			if (!VisualStyleInformation.IsEnabledByUser)
			{
				buttonBorders.ThemeAware = false;
				buttonBorders.Style = eDotNetBarStyle.OfficeXP;
				buttonPermissions.ThemeAware = false;
				buttonPermissions.Style = eDotNetBarStyle.OfficeXP;
			}
		}
	}

	public override void SetBackColor()
	{
		((Control)this).BackColor = Color.White;
		if (base.IsFocused)
		{
			Panel obj = panelBreakIfTrue;
			StiCheckBoxInfo stiCheckBoxInfo = checkBoxBreakIfTrue;
			RadioButton obj2 = radioButtonAnd;
			Panel obj3 = panelBreakIfTrue;
			Color color2 = (((Control)radioButtonOr).BackColor = Color.FromArgb(255, 245, 245, 245));
			Color color4 = (((Control)obj3).BackColor = color2);
			Color color6 = (((Control)obj2).BackColor = color4);
			Color backColor = (((Control)stiCheckBoxInfo).BackColor = color6);
			((Control)obj).BackColor = backColor;
		}
		else
		{
			Panel obj4 = panelBreakIfTrue;
			StiCheckBoxInfo stiCheckBoxInfo2 = checkBoxBreakIfTrue;
			RadioButton obj5 = radioButtonAnd;
			Panel obj6 = panelBreakIfTrue;
			Color color2 = (((Control)radioButtonOr).BackColor = Color.White);
			Color color4 = (((Control)obj6).BackColor = color2);
			Color color6 = (((Control)obj5).BackColor = color4);
			Color backColor = (((Control)stiCheckBoxInfo2).BackColor = color6);
			((Control)obj4).BackColor = backColor;
		}
	}

	private void DoScaling()
	{
		((ButtonBase)tbFontStyleBold).Image = StiReportDesignImages.Editor.FontStyleBold();
		((ButtonBase)tbFontStyleItalic).Image = StiReportDesignImages.Editor.FontStyleItalic();
		((ButtonBase)tbFontStyleUnderline).Image = StiReportDesignImages.Editor.FontStyleUnderline();
		((ButtonBase)tbTextColor).Image = StiReportDesignImages.Editor.FontColor();
		((ButtonBase)tbBackColor).Image = StiReportDesignImages.Editor.Fill();
		buttonPermissions.Image = StiReportDesignImages.Actions.SelectPermissions();
		textBoxAssignExpression.Image = StiReportDesignImages.Actions.EditSmall();
		StiResourcesHelper.SetImage(buttonBordersAll, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderAll.png", scale: true);
		StiResourcesHelper.SetImage(buttonBordersNone, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderNone.png", scale: true);
		StiResourcesHelper.SetImage(buttonBordersTop, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderTop.png", scale: true);
		StiResourcesHelper.SetImage(buttonBordersLeft, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderLeft.png", scale: true);
		StiResourcesHelper.SetImage(buttonBordersBottom, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderBottom.png", scale: true);
		StiResourcesHelper.SetImage(buttonBordersRight, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Home.Borders.BorderRight.png", scale: true);
	}

	private FontStyle GetFontStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (tbFontStyleBold.Pushed)
		{
			val = (FontStyle)(val | 1);
		}
		if (tbFontStyleItalic.Pushed)
		{
			val = (FontStyle)(val | 2);
		}
		if (tbFontStyleUnderline.Pushed)
		{
			val = (FontStyle)(val | 4);
		}
		if (tbFontStyleStrikeOutPushed)
		{
			val = (FontStyle)(val | 8);
		}
		return val;
	}

	private void CheckAllows()
	{
		bool flag = styleBoxSelect.SelectedStyle != null;
		((Control)tbFont).Enabled = (buttonItemAllowFontName.Checked || buttonItemAllowFontSize.Checked) && !flag;
		((Control)tbTextColor).Enabled = buttonItemAllowTextColor.Checked && !flag;
		((Control)tbBackColor).Enabled = buttonItemAllowBackColor.Checked && !flag;
		((Control)tbFontStyleBold).Enabled = buttonItemAllowFontBold.Checked && !flag;
		((Control)tbFontStyleItalic).Enabled = buttonItemAllowFontItalic.Checked && !flag;
		((Control)tbFontStyleUnderline).Enabled = buttonItemAllowFontUnderline.Checked && !flag;
		((Control)buttonBorders).Enabled = buttonItemAllowBorders.Checked && !flag;
		((Control)buttonPermissions).Enabled = !flag;
	}

	private void Check()
	{
		buttonBordersAll.Checked = buttonBordersLeft.Checked && buttonBordersRight.Checked && buttonBordersTop.Checked && buttonBordersBottom.Checked;
		buttonBordersNone.Checked = !buttonBordersLeft.Checked && !buttonBordersRight.Checked && !buttonBordersTop.Checked && !buttonBordersBottom.Checked;
	}

	internal void UpdateSample()
	{
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		if (blockUpdateSample)
		{
			return;
		}
		Font val = null;
		Color? color = null;
		Color? color2 = null;
		StiBorderSides? stiBorderSides = null;
		if (component is IStiFont stiFont)
		{
			val = stiFont.Font;
		}
		if (component is IStiTextBrush stiTextBrush)
		{
			color = StiBrush.ToColor(stiTextBrush.TextBrush);
		}
		if (component is IStiBrush stiBrush)
		{
			color2 = StiBrush.ToColor(stiBrush.Brush);
		}
		if (component is IStiBorder stiBorder)
		{
			stiBorderSides = stiBorder.Border.Side;
		}
		if (!color.HasValue)
		{
			color = Color.Black;
		}
		if (!color2.HasValue)
		{
			color2 = Color.White;
		}
		if (!stiBorderSides.HasValue)
		{
			stiBorderSides = StiBorderSides.All;
		}
		if (styleBoxSelect.SelectedStyle != null)
		{
			StiStyle stiStyle = styleBoxSelect.SelectedStyle as StiStyle;
			if (stiStyle.AllowUseFont)
			{
				((Control)lbAaBbCc).Font = stiStyle.Font;
			}
			else
			{
				((Control)lbAaBbCc).Font = val;
			}
			if (((Control)lbAaBbCc).Font == null)
			{
				((Control)lbAaBbCc).Font = new Font("Arial", 8f);
			}
			if (stiStyle.AllowUseTextBrush)
			{
				((Control)lbAaBbCc).ForeColor = StiBrush.ToColor(stiStyle.TextBrush);
			}
			else
			{
				((Control)lbAaBbCc).ForeColor = color.GetValueOrDefault();
			}
			if (stiStyle.AllowUseBrush)
			{
				((Control)lbAaBbCc).BackColor = StiBrush.ToColor(stiStyle.Brush);
			}
			else
			{
				((Control)lbAaBbCc).BackColor = color2.GetValueOrDefault();
			}
			return;
		}
		FontStyle val2 = (FontStyle)0;
		if (buttonItemAllowFontBold.Checked && tbFontStyleBold.Pushed)
		{
			val2 = (FontStyle)(val2 | 1);
		}
		if (buttonItemAllowFontItalic.Checked && tbFontStyleItalic.Pushed)
		{
			val2 = (FontStyle)(val2 | 2);
		}
		if (buttonItemAllowFontUnderline.Checked && tbFontStyleUnderline.Pushed)
		{
			val2 = (FontStyle)(val2 | 4);
		}
		if (buttonItemAllowFontStrikeout.Checked && tbFontStyleStrikeOutPushed)
		{
			val2 = (FontStyle)(val2 | 8);
		}
		string text = null;
		if (buttonItemAllowFontName.Checked && SelectedFont != null)
		{
			text = SelectedFont.Name;
		}
		if (text == null)
		{
			text = ((val == null) ? "Arial" : val.Name);
		}
		float? num = null;
		if (buttonItemAllowFontSize.Checked && SelectedFont != null)
		{
			num = SelectedFont.Size;
		}
		if (!num.HasValue)
		{
			num = ((val == null) ? new float?(8f) : new float?(val.Size));
		}
		if (buttonItemAllowFontName.Checked)
		{
			((Control)lbAaBbCc).Font = new Font(SelectedFont.FontFamily, num.GetValueOrDefault(), val2);
		}
		else
		{
			((Control)lbAaBbCc).Font = new Font(text, num.GetValueOrDefault(), val2);
		}
		((Control)lbAaBbCc).ForeColor = (buttonItemAllowTextColor.Checked ? SelectedTextColor : color.GetValueOrDefault());
		((Control)lbAaBbCc).BackColor = (buttonItemAllowBackColor.Checked ? SelectedBackColor : color2.GetValueOrDefault());
	}

	private void LocalizeSuperToolTip()
	{
		checkBoxBreakIfTrue.SetToolTip(StiLocalization.Get("FormConditions", "BreakIfTrueToolTip"));
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)cbEnabled).Text = StiLocalization.Get("FormConditions", "ComponentIsEnabled");
		((Control)tbFont).Text = Loc.GetCleaned("FormConditions", "ChangeFont");
		((Control)styleBoxSelect).Text = StiLocalization.Get("FormConditions", "SelectStyle");
		((Control)checkBoxCanAssignExpression).Text = StiLocalization.Get("FormConditions", "AssignExpression");
		((Control)checkBoxBreakIfTrue).Text = StiLocalization.Get("FormConditions", "BreakIfTrue");
		((Control)radioButtonAnd).Text = StiLocalization.Get("Report", "FilterAnd");
		((Control)radioButtonOr).Text = StiLocalization.Get("Report", "FilterOr");
		((Control)toolButtonAddLevel).Text = StiLocalization.Get("FormConditions", "AddLevel");
		toolTip1.SetToolTip((Control)(object)tbFontStyleBold, StiLocalization.Get("Toolbars", "FontStyleBold"));
		toolTip1.SetToolTip((Control)(object)tbFontStyleItalic, StiLocalization.Get("Toolbars", "FontStyleItalic"));
		toolTip1.SetToolTip((Control)(object)tbFontStyleUnderline, StiLocalization.Get("Toolbars", "FontStyleUnderline"));
		toolTip1.SetToolTip((Control)(object)tbTextColor, StiLocalization.Get("PropertyMain", "TextColor"));
		toolTip1.SetToolTip((Control)(object)tbBackColor, StiLocalization.Get("PropertyMain", "BackColor"));
		toolTip1.SetToolTip((Control)(object)styleBoxSelect, StiLocalization.Get("FormConditions", "SelectStyle"));
		((Control)buttonBorders).Text = StiLocalization.Get("PropertyMain", "Border");
		buttonBordersAll.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesAll");
		buttonBordersNone.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesNone");
		buttonBordersLeft.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft");
		buttonBordersRight.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesRight");
		buttonBordersTop.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesTop");
		buttonBordersBottom.Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom");
		buttonItemAllowFontName.Text = StiLocalization.Get("PropertyMain", "FontName");
		buttonItemAllowFontSize.Text = StiLocalization.Get("PropertyMain", "FontSize");
		buttonItemAllowFontBold.Text = StiLocalization.Get("PropertyMain", "FontBold");
		buttonItemAllowFontItalic.Text = StiLocalization.Get("PropertyMain", "FontItalic");
		buttonItemAllowFontUnderline.Text = StiLocalization.Get("PropertyMain", "FontUnderline");
		buttonItemAllowFontStrikeout.Text = StiLocalization.Get("PropertyMain", "FontStrikeout");
		buttonItemAllowTextColor.Text = StiLocalization.Get("PropertyMain", "TextColor");
		buttonItemAllowBackColor.Text = StiLocalization.Get("PropertyMain", "BackColor");
		buttonItemAllowBorders.Text = StiLocalization.Get("PropertyMain", "Border");
	}

	private void tbFontStyleBold_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		UpdateSample();
		SelectedFont = new Font(SelectedFont, GetFontStyle());
	}

	private void tbFontStyleItalic_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		UpdateSample();
		SelectedFont = new Font(SelectedFont, GetFontStyle());
	}

	private void tbFontStyleUnderline_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		UpdateSample();
		SelectedFont = new Font(SelectedFont, GetFontStyle());
	}

	private void tbBackColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		SelectedBackColor = tbBackColor.SelectedColor;
		UpdateSample();
	}

	private void tbTextColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		SelectedTextColor = tbTextColor.SelectedColor;
		UpdateSample();
	}

	private void tbFont_Click(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(SelectedFont, report);
		try
		{
			if (report != null && report.Designer != null && report.Designer.DesignerControl != null)
			{
				report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiFontEditorForm);
			}
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				SelectedFont = stiFontEditorForm.SelectedFont;
				tbFontStyleBold.Pushed = (SelectedFont.Style & 1) > 0;
				tbFontStyleItalic.Pushed = (SelectedFont.Style & 2) > 0;
				tbFontStyleUnderline.Pushed = (SelectedFont.Style & 4) > 0;
				tbFontStyleStrikeOutPushed = (SelectedFont.Style & 8) > 0;
				UpdateSample();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panel1).Width - 1, ((Control)panel1).Height - 1);
	}

	private void styleBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckAllows();
		UpdateSample();
	}

	private void OnControlsChanged(object sender, ControlEventArgs e)
	{
		UpdateStates();
	}

	public void UpdateStates()
	{
		RadioButton obj = radioButtonAnd;
		bool visible = (((Control)radioButtonOr).Visible = ((ArrangedElementCollection)((Control)panelConditions).Controls).Count > 1);
		((Control)obj).Visible = visible;
		foreach (StiConditionItemZoneControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			item.ShowClose = ((ArrangedElementCollection)((Control)panelConditions).Controls).Count > 1;
		}
	}

	private void textBoxAssignExpression_ButtonClick(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
		string text = ((Control)stiButtonEdit).Text;
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorCode);
		if ((int)val == 1)
		{
			((Control)stiButtonEdit).Text = text;
		}
	}

	private void buttonBordersNotAssigned_Click(object sender, EventArgs e)
	{
		buttonBordersAll.Checked = false;
		buttonBordersNone.Checked = false;
		buttonBordersLeft.Checked = false;
		buttonBordersRight.Checked = false;
		buttonBordersTop.Checked = false;
		buttonBordersBottom.Checked = false;
	}

	private void buttonBordersAll_Click(object sender, EventArgs e)
	{
		buttonBordersAll.Checked = true;
		buttonBordersNone.Checked = false;
		buttonBordersLeft.Checked = true;
		buttonBordersRight.Checked = true;
		buttonBordersTop.Checked = true;
		buttonBordersBottom.Checked = true;
	}

	private void buttonBordersNone_Click(object sender, EventArgs e)
	{
		buttonBordersAll.Checked = false;
		buttonBordersNone.Checked = true;
		buttonBordersLeft.Checked = false;
		buttonBordersRight.Checked = false;
		buttonBordersTop.Checked = false;
		buttonBordersBottom.Checked = false;
	}

	private void buttonBordersTop_Click(object sender, EventArgs e)
	{
		Check();
	}

	private void buttonBordersLeft_Click(object sender, EventArgs e)
	{
		Check();
	}

	private void buttonBordersBottom_Click(object sender, EventArgs e)
	{
		Check();
	}

	private void buttonBordersRight_Click(object sender, EventArgs e)
	{
		Check();
	}

	private void checkBoxCanAssignExpression_CheckedChanged(object sender, EventArgs e)
	{
		((Control)textBoxAssignExpression).Enabled = checkBoxCanAssignExpression.Checked;
	}

	private void buttonItemAllow_CheckedChanged(object sender, EventArgs e)
	{
		CheckAllows();
		UpdateSample();
	}

	private void styleBoxSelect_EditStylesInvoked(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void StiConditionZoneControl_Load(object sender, EventArgs e)
	{
		SetBackColor();
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Expected O, but got Unknown
		//IL_1374: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_1402: Unknown result type (might be due to invalid IL or missing references)
		//IL_140c: Expected O, but got Unknown
		//IL_1484: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Expected O, but got Unknown
		//IL_1506: Unknown result type (might be due to invalid IL or missing references)
		//IL_1510: Expected O, but got Unknown
		//IL_1588: Unknown result type (might be due to invalid IL or missing references)
		//IL_1592: Expected O, but got Unknown
		//IL_1caf: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiConditionZoneControl));
		ilButtons = new ImageList(components);
		fontDialog1 = new FontDialog();
		toolTip1 = new ToolTip(components);
		panel3 = new Panel();
		checkBoxCanAssignExpression = new CheckBox();
		textBoxAssignExpression = new StiButtonEdit();
		cbEnabled = new CheckBox();
		radioButtonOr = new RadioButton();
		radioButtonAnd = new RadioButton();
		panelConditions = new Panel();
		panel1 = new Panel();
		lbAaBbCc = new Label();
		tbTool = new StiToolBar();
		styleBoxSelect = new StiStyleBox();
		stiToolButton5 = new StiToolButton();
		buttonPermissions = new ButtonX();
		buttonItemAllowFontName = new ButtonItem();
		buttonItemAllowFontSize = new ButtonItem();
		buttonItemAllowFontBold = new ButtonItem();
		buttonItemAllowFontItalic = new ButtonItem();
		buttonItemAllowFontUnderline = new ButtonItem();
		buttonItemAllowFontStrikeout = new ButtonItem();
		buttonItemAllowTextColor = new ButtonItem();
		buttonItemAllowBackColor = new ButtonItem();
		buttonItemAllowBorders = new ButtonItem();
		stiToolButton4 = new StiToolButton();
		buttonBorders = new ButtonX();
		buttonBordersAll = new ButtonItem();
		buttonBordersNone = new ButtonItem();
		buttonBordersTop = new ButtonItem();
		buttonBordersLeft = new ButtonItem();
		buttonBordersBottom = new ButtonItem();
		buttonBordersRight = new ButtonItem();
		stiToolButton1 = new StiToolButton();
		tbBackColor = new StiColorButtonXF();
		tbTextColor = new StiColorButtonXF();
		stiToolButton3 = new StiToolButton();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		stiToolButton2 = new StiToolButton();
		tbFont = new StiToolButton();
		toolButtonAddLevel = new StiToolButton();
		checkBoxBreakIfTrue = new StiCheckBoxInfo();
		panelBreakIfTrue = new Panel();
		((Control)panel3).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)tbTool).SuspendLayout();
		((Control)panelBreakIfTrue).SuspendLayout();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		ilButtons.Images.SetKeyName(3, "");
		ilButtons.Images.SetKeyName(4, "");
		((Control)panel3).Anchor = (AnchorStyles)14;
		((Control)panel3).BackColor = Color.Transparent;
		((Control)panel3).Controls.Add((Control)(object)checkBoxCanAssignExpression);
		((Control)panel3).Controls.Add((Control)(object)textBoxAssignExpression);
		((Control)panel3).Controls.Add((Control)(object)cbEnabled);
		((Control)panel3).Location = new Point(124, 73);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(596, 22);
		((Control)panel3).TabIndex = 6;
		((Control)checkBoxCanAssignExpression).AutoSize = true;
		((Control)checkBoxCanAssignExpression).BackColor = Color.Transparent;
		checkBoxCanAssignExpression.Checked = true;
		checkBoxCanAssignExpression.CheckState = (CheckState)1;
		((Control)checkBoxCanAssignExpression).Dock = (DockStyle)4;
		((Control)checkBoxCanAssignExpression).Location = new Point(268, 0);
		((Control)checkBoxCanAssignExpression).Name = "checkBoxCanAssignExpression";
		((Control)checkBoxCanAssignExpression).Size = new Size(111, 22);
		((Control)checkBoxCanAssignExpression).TabIndex = 15;
		((Control)checkBoxCanAssignExpression).Text = "Assign Expression";
		((ButtonBase)checkBoxCanAssignExpression).UseVisualStyleBackColor = false;
		((Control)checkBoxCanAssignExpression).Visible = false;
		checkBoxCanAssignExpression.CheckedChanged += checkBoxCanAssignExpression_CheckedChanged;
		((Control)textBoxAssignExpression).BackColor = SystemColors.Window;
		((Control)textBoxAssignExpression).Dock = (DockStyle)4;
		textBoxAssignExpression.Flat = false;
		textBoxAssignExpression.Image = (Image)componentResourceManager.GetObject("textBoxAssignExpression.Image");
		((Control)textBoxAssignExpression).Location = new Point(379, 0);
		((Control)textBoxAssignExpression).Name = "textBoxAssignExpression";
		((Control)textBoxAssignExpression).Size = new Size(217, 21);
		((Control)textBoxAssignExpression).TabIndex = 16;
		((Control)textBoxAssignExpression).Visible = false;
		textBoxAssignExpression.ButtonClick += textBoxAssignExpression_ButtonClick;
		((Control)cbEnabled).AutoSize = true;
		((Control)cbEnabled).BackColor = Color.Transparent;
		((Control)cbEnabled).Dock = (DockStyle)3;
		((Control)cbEnabled).ForeColor = SystemColors.ControlText;
		((Control)cbEnabled).Location = new Point(0, 0);
		((Control)cbEnabled).Name = "cbEnabled";
		((Control)cbEnabled).Size = new Size(132, 22);
		((Control)cbEnabled).TabIndex = 14;
		((Control)cbEnabled).Text = "Component is Enabled";
		((ButtonBase)cbEnabled).UseVisualStyleBackColor = false;
		((Control)radioButtonOr).BackColor = Color.White;
		((Control)radioButtonOr).Location = new Point(220, 8);
		((Control)radioButtonOr).Name = "radioButtonOr";
		((Control)radioButtonOr).Size = new Size(64, 16);
		((Control)radioButtonOr).TabIndex = 2;
		((Control)radioButtonOr).Text = "Or";
		((ButtonBase)radioButtonOr).UseVisualStyleBackColor = false;
		((Control)radioButtonOr).Visible = false;
		((Control)radioButtonAnd).BackColor = Color.White;
		radioButtonAnd.Checked = true;
		((Control)radioButtonAnd).Location = new Point(148, 8);
		((Control)radioButtonAnd).Name = "radioButtonAnd";
		((Control)radioButtonAnd).Size = new Size(64, 16);
		((Control)radioButtonAnd).TabIndex = 1;
		radioButtonAnd.TabStop = true;
		((Control)radioButtonAnd).Text = "And";
		((ButtonBase)radioButtonAnd).UseVisualStyleBackColor = false;
		((Control)radioButtonAnd).Visible = false;
		((Control)panelConditions).Anchor = (AnchorStyles)15;
		((Control)panelConditions).AutoSize = true;
		((Control)panelConditions).Location = new Point(5, 32);
		((Control)panelConditions).Name = "panelConditions";
		((Control)panelConditions).Size = new Size(719, 4);
		((Control)panelConditions).TabIndex = 4;
		((Control)panel1).Anchor = (AnchorStyles)6;
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)lbAaBbCc);
		((Control)panel1).Location = new Point(8, 42);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Padding = new Padding(1);
		((Control)panel1).Size = new Size(110, 53);
		((Control)panel1).TabIndex = 4;
		((Control)panel1).Paint += new PaintEventHandler(panel1_Paint);
		((Control)lbAaBbCc).Dock = (DockStyle)5;
		((Control)lbAaBbCc).Location = new Point(1, 1);
		((Control)lbAaBbCc).Name = "lbAaBbCc";
		((Control)lbAaBbCc).Size = new Size(108, 51);
		((Control)lbAaBbCc).TabIndex = 4;
		((Control)lbAaBbCc).Text = "AaBbCcYyZz";
		lbAaBbCc.TextAlign = (ContentAlignment)32;
		((Control)tbTool).Anchor = (AnchorStyles)14;
		((Control)tbTool).Controls.Add((Control)(object)styleBoxSelect);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton5);
		((Control)tbTool).Controls.Add((Control)(object)buttonPermissions);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton4);
		((Control)tbTool).Controls.Add((Control)(object)buttonBorders);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton1);
		((Control)tbTool).Controls.Add((Control)(object)tbBackColor);
		((Control)tbTool).Controls.Add((Control)(object)tbTextColor);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton3);
		((Control)tbTool).Controls.Add((Control)(object)tbFontStyleUnderline);
		((Control)tbTool).Controls.Add((Control)(object)tbFontStyleItalic);
		((Control)tbTool).Controls.Add((Control)(object)tbFontStyleBold);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton2);
		((Control)tbTool).Controls.Add((Control)(object)tbFont);
		tbTool.ControlStyle = StiControlStyle.Office2010;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.All;
		((Control)tbTool).Location = new Point(124, 42);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(6, 3, 3, 4);
		((Control)tbTool).Size = new Size(596, 28);
		((Control)tbTool).TabIndex = 5;
		((Control)tbTool).Text = "stiToolBar1";
		styleBoxSelect.CustomColors = null;
		styleBoxSelect.Designer = null;
		((Control)styleBoxSelect).Dock = (DockStyle)5;
		styleBoxSelect.Draw3DButton = false;
		styleBoxSelect.HotDragDrop = false;
		styleBoxSelect.IsCrossTabStyle = false;
		((Control)styleBoxSelect).Location = new Point(383, 3);
		((Control)styleBoxSelect).Name = "styleBoxSelect";
		styleBoxSelect.SelectedIndex = 0;
		styleBoxSelect.SelectedStyle = null;
		styleBoxSelect.ShowChartStyles = true;
		styleBoxSelect.ShowCrossTabStyles = true;
		styleBoxSelect.ShowCustomStyles = false;
		styleBoxSelect.ShowEditStyles = false;
		styleBoxSelect.ShowNone = true;
		styleBoxSelect.ShowPredefinedStyles = false;
		styleBoxSelect.ShowReportStyles = true;
		styleBoxSelect.ShowStyle = true;
		((Control)styleBoxSelect).Size = new Size(210, 21);
		styleBoxSelect.Style = StiToolBarButtonStyle.PushButton;
		((Control)styleBoxSelect).TabIndex = 13;
		((Control)styleBoxSelect).Text = "Select Style";
		((ButtonBase)styleBoxSelect).UseVisualStyleBackColor = false;
		styleBoxSelect.SelectedIndexChanged += styleBoxSelect_SelectedIndexChanged;
		styleBoxSelect.EditStylesInvoked += styleBoxSelect_EditStylesInvoked;
		((Control)stiToolButton5).Dock = (DockStyle)3;
		stiToolButton5.Draw3DButton = false;
		stiToolButton5.HotDragDrop = false;
		((ButtonBase)stiToolButton5).ImageIndex = 11;
		((Control)stiToolButton5).Location = new Point(377, 3);
		((Control)stiToolButton5).Name = "stiToolButton5";
		((Control)stiToolButton5).Size = new Size(6, 21);
		stiToolButton5.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton5).TabIndex = 36;
		((ButtonBase)stiToolButton5).UseVisualStyleBackColor = false;
		((Control)buttonPermissions).AccessibleRole = (AccessibleRole)43;
		buttonPermissions.AutoExpandOnClick = true;
		buttonPermissions.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonPermissions).Dock = (DockStyle)3;
		buttonPermissions.Image = (Image)componentResourceManager.GetObject("buttonPermissions.Image");
		buttonPermissions.Is20Button = false;
		((Control)buttonPermissions).Location = new Point(336, 3);
		buttonPermissions.LockClosePopup = false;
		((Control)buttonPermissions).Name = "buttonPermissions";
		((Control)buttonPermissions).Size = new Size(41, 21);
		buttonPermissions.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonPermissions.SubItems.AddRange(new BaseItem[9] { buttonItemAllowFontName, buttonItemAllowFontSize, buttonItemAllowFontBold, buttonItemAllowFontItalic, buttonItemAllowFontUnderline, buttonItemAllowFontStrikeout, buttonItemAllowTextColor, buttonItemAllowBackColor, buttonItemAllowBorders });
		((Control)buttonPermissions).TabIndex = 12;
		buttonPermissions.ThemeAware = true;
		buttonItemAllowFontName.AutoCheckOnClick = true;
		buttonItemAllowFontName.AutoCollapseOnClick = false;
		buttonItemAllowFontName.Checked = true;
		buttonItemAllowFontName.GlobalItem = false;
		buttonItemAllowFontName.Name = "buttonItemAllowFontName";
		buttonItemAllowFontName.Text = "Font Name";
		buttonItemAllowFontName.ThemeAware = true;
		buttonItemAllowFontName.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowFontSize.AutoCheckOnClick = true;
		buttonItemAllowFontSize.AutoCollapseOnClick = false;
		buttonItemAllowFontSize.Checked = true;
		buttonItemAllowFontSize.GlobalItem = false;
		buttonItemAllowFontSize.Name = "buttonItemAllowFontSize";
		buttonItemAllowFontSize.Text = "Font Size";
		buttonItemAllowFontSize.ThemeAware = true;
		buttonItemAllowFontSize.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowFontBold.AutoCheckOnClick = true;
		buttonItemAllowFontBold.AutoCollapseOnClick = false;
		buttonItemAllowFontBold.BeginGroup = true;
		buttonItemAllowFontBold.Checked = true;
		buttonItemAllowFontBold.GlobalItem = false;
		buttonItemAllowFontBold.Name = "buttonItemAllowFontBold";
		buttonItemAllowFontBold.Text = "Font Bold";
		buttonItemAllowFontBold.ThemeAware = true;
		buttonItemAllowFontBold.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowFontItalic.AutoCheckOnClick = true;
		buttonItemAllowFontItalic.AutoCollapseOnClick = false;
		buttonItemAllowFontItalic.Checked = true;
		buttonItemAllowFontItalic.GlobalItem = false;
		buttonItemAllowFontItalic.Name = "buttonItemAllowFontItalic";
		buttonItemAllowFontItalic.Text = "Font Italic";
		buttonItemAllowFontItalic.ThemeAware = true;
		buttonItemAllowFontItalic.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowFontUnderline.AutoCheckOnClick = true;
		buttonItemAllowFontUnderline.AutoCollapseOnClick = false;
		buttonItemAllowFontUnderline.Checked = true;
		buttonItemAllowFontUnderline.GlobalItem = false;
		buttonItemAllowFontUnderline.Name = "buttonItemAllowFontUnderline";
		buttonItemAllowFontUnderline.Text = "Font Underline";
		buttonItemAllowFontUnderline.ThemeAware = true;
		buttonItemAllowFontUnderline.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowFontStrikeout.AutoCheckOnClick = true;
		buttonItemAllowFontStrikeout.AutoCollapseOnClick = false;
		buttonItemAllowFontStrikeout.Checked = true;
		buttonItemAllowFontStrikeout.GlobalItem = false;
		buttonItemAllowFontStrikeout.Name = "buttonItemAllowFontStrikeout";
		buttonItemAllowFontStrikeout.Text = "Font Strikeout";
		buttonItemAllowFontStrikeout.ThemeAware = true;
		buttonItemAllowFontStrikeout.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowTextColor.AutoCheckOnClick = true;
		buttonItemAllowTextColor.AutoCollapseOnClick = false;
		buttonItemAllowTextColor.BeginGroup = true;
		buttonItemAllowTextColor.Checked = true;
		buttonItemAllowTextColor.GlobalItem = false;
		buttonItemAllowTextColor.Name = "buttonItemAllowTextColor";
		buttonItemAllowTextColor.Text = "Text Color";
		buttonItemAllowTextColor.ThemeAware = true;
		buttonItemAllowTextColor.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowBackColor.AutoCheckOnClick = true;
		buttonItemAllowBackColor.AutoCollapseOnClick = false;
		buttonItemAllowBackColor.Checked = true;
		buttonItemAllowBackColor.GlobalItem = false;
		buttonItemAllowBackColor.Name = "buttonItemAllowBackColor";
		buttonItemAllowBackColor.Text = "Back Color";
		buttonItemAllowBackColor.ThemeAware = true;
		buttonItemAllowBackColor.CheckedChanged += buttonItemAllow_CheckedChanged;
		buttonItemAllowBorders.AutoCheckOnClick = true;
		buttonItemAllowBorders.AutoCollapseOnClick = false;
		buttonItemAllowBorders.BeginGroup = true;
		buttonItemAllowBorders.Checked = true;
		buttonItemAllowBorders.GlobalItem = false;
		buttonItemAllowBorders.Name = "buttonItemAllowBorders";
		buttonItemAllowBorders.Text = "Borders";
		buttonItemAllowBorders.ThemeAware = true;
		buttonItemAllowBorders.CheckedChanged += buttonItemAllow_CheckedChanged;
		((Control)stiToolButton4).Dock = (DockStyle)3;
		stiToolButton4.Draw3DButton = false;
		stiToolButton4.HotDragDrop = false;
		((ButtonBase)stiToolButton4).ImageIndex = 11;
		((Control)stiToolButton4).Location = new Point(330, 3);
		((Control)stiToolButton4).Name = "stiToolButton4";
		((Control)stiToolButton4).Size = new Size(6, 21);
		stiToolButton4.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton4).TabIndex = 34;
		((ButtonBase)stiToolButton4).UseVisualStyleBackColor = false;
		((Control)buttonBorders).AccessibleRole = (AccessibleRole)43;
		buttonBorders.AutoExpandOnClick = true;
		buttonBorders.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonBorders).Dock = (DockStyle)3;
		buttonBorders.Is20Button = false;
		((Control)buttonBorders).Location = new Point(236, 3);
		buttonBorders.LockClosePopup = false;
		((Control)buttonBorders).Name = "buttonBorders";
		((Control)buttonBorders).Size = new Size(94, 21);
		buttonBorders.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonBorders.SubItems.AddRange(new BaseItem[6] { buttonBordersAll, buttonBordersNone, buttonBordersTop, buttonBordersLeft, buttonBordersBottom, buttonBordersRight });
		((Control)buttonBorders).TabIndex = 11;
		((Control)buttonBorders).Text = "Borders";
		buttonBorders.ThemeAware = true;
		buttonBordersAll.AutoCheckOnClick = true;
		buttonBordersAll.AutoCollapseOnClick = false;
		buttonBordersAll.BeginGroup = true;
		buttonBordersAll.GlobalItem = false;
		buttonBordersAll.Image = (Image)componentResourceManager.GetObject("buttonBordersAll.Image");
		buttonBordersAll.Name = "buttonBordersAll";
		buttonBordersAll.Text = "All";
		buttonBordersAll.ThemeAware = true;
		buttonBordersAll.Click += buttonBordersAll_Click;
		buttonBordersNone.AutoCheckOnClick = true;
		buttonBordersNone.AutoCollapseOnClick = false;
		buttonBordersNone.GlobalItem = false;
		buttonBordersNone.Image = (Image)componentResourceManager.GetObject("buttonBordersNone.Image");
		buttonBordersNone.Name = "buttonBordersNone";
		buttonBordersNone.Text = "None";
		buttonBordersNone.ThemeAware = true;
		buttonBordersNone.Click += buttonBordersNone_Click;
		buttonBordersTop.AutoCheckOnClick = true;
		buttonBordersTop.AutoCollapseOnClick = false;
		buttonBordersTop.BeginGroup = true;
		buttonBordersTop.GlobalItem = false;
		buttonBordersTop.Image = (Image)componentResourceManager.GetObject("buttonBordersTop.Image");
		buttonBordersTop.Name = "buttonBordersTop";
		buttonBordersTop.Text = "Top";
		buttonBordersTop.ThemeAware = true;
		buttonBordersTop.Click += buttonBordersTop_Click;
		buttonBordersLeft.AutoCheckOnClick = true;
		buttonBordersLeft.AutoCollapseOnClick = false;
		buttonBordersLeft.GlobalItem = false;
		buttonBordersLeft.Image = (Image)componentResourceManager.GetObject("buttonBordersLeft.Image");
		buttonBordersLeft.Name = "buttonBordersLeft";
		buttonBordersLeft.Text = "Left";
		buttonBordersLeft.ThemeAware = true;
		buttonBordersLeft.Click += buttonBordersLeft_Click;
		buttonBordersBottom.AutoCheckOnClick = true;
		buttonBordersBottom.AutoCollapseOnClick = false;
		buttonBordersBottom.GlobalItem = false;
		buttonBordersBottom.Image = (Image)componentResourceManager.GetObject("buttonBordersBottom.Image");
		buttonBordersBottom.Name = "buttonBordersBottom";
		buttonBordersBottom.Text = "Bottom";
		buttonBordersBottom.ThemeAware = true;
		buttonBordersBottom.Click += buttonBordersBottom_Click;
		buttonBordersRight.AutoCheckOnClick = true;
		buttonBordersRight.AutoCollapseOnClick = false;
		buttonBordersRight.GlobalItem = false;
		buttonBordersRight.Image = (Image)componentResourceManager.GetObject("buttonBordersRight.Image");
		buttonBordersRight.Name = "buttonBordersRight";
		buttonBordersRight.Text = "Right";
		buttonBordersRight.ThemeAware = true;
		buttonBordersRight.Click += buttonBordersRight_Click;
		((Control)stiToolButton1).Dock = (DockStyle)3;
		stiToolButton1.Draw3DButton = false;
		stiToolButton1.HotDragDrop = false;
		((ButtonBase)stiToolButton1).ImageIndex = 11;
		((Control)stiToolButton1).Location = new Point(230, 3);
		((Control)stiToolButton1).Name = "stiToolButton1";
		((Control)stiToolButton1).Size = new Size(6, 21);
		stiToolButton1.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton1).TabIndex = 33;
		((ButtonBase)stiToolButton1).UseVisualStyleBackColor = false;
		((Control)tbBackColor).Dock = (DockStyle)3;
		tbBackColor.Draw3DButton = false;
		((Control)tbBackColor).ForeColor = Color.Transparent;
		tbBackColor.HotDragDrop = false;
		((ButtonBase)tbBackColor).ImageIndex = 3;
		((ButtonBase)tbBackColor).ImageList = ilButtons;
		((Control)tbBackColor).Location = new Point(198, 3);
		((Control)tbBackColor).Name = "tbBackColor";
		tbBackColor.SelectedColor = Color.Transparent;
		((Control)tbBackColor).Size = new Size(32, 21);
		tbBackColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbBackColor).TabIndex = 10;
		((ButtonBase)tbBackColor).UseVisualStyleBackColor = false;
		tbBackColor.SelectedColorChanged += tbBackColor_SelectedIndexChanged;
		((Control)tbTextColor).Dock = (DockStyle)3;
		tbTextColor.Draw3DButton = false;
		((Control)tbTextColor).ForeColor = Color.Red;
		tbTextColor.HotDragDrop = false;
		((ButtonBase)tbTextColor).ImageIndex = 4;
		((ButtonBase)tbTextColor).ImageList = ilButtons;
		((Control)tbTextColor).Location = new Point(166, 3);
		((Control)tbTextColor).Name = "tbTextColor";
		tbTextColor.SelectedColor = Color.Transparent;
		((Control)tbTextColor).Size = new Size(32, 21);
		tbTextColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbTextColor).TabIndex = 8;
		((ButtonBase)tbTextColor).UseVisualStyleBackColor = false;
		tbTextColor.SelectedColorChanged += tbTextColor_SelectedIndexChanged;
		((Control)stiToolButton3).Dock = (DockStyle)3;
		stiToolButton3.Draw3DButton = false;
		stiToolButton3.HotDragDrop = false;
		((ButtonBase)stiToolButton3).ImageIndex = 11;
		((Control)stiToolButton3).Location = new Point(160, 3);
		((Control)stiToolButton3).Name = "stiToolButton3";
		((Control)stiToolButton3).Size = new Size(6, 21);
		stiToolButton3.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton3).TabIndex = 8;
		((ButtonBase)stiToolButton3).UseVisualStyleBackColor = false;
		((Control)tbFontStyleUnderline).Dock = (DockStyle)3;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).ImageIndex = 2;
		((ButtonBase)tbFontStyleUnderline).ImageList = ilButtons;
		((Control)tbFontStyleUnderline).Location = new Point(138, 3);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(22, 21);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 8;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		((Control)tbFontStyleUnderline).Click += tbFontStyleUnderline_Click;
		((Control)tbFontStyleItalic).Dock = (DockStyle)3;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).ImageIndex = 1;
		((ButtonBase)tbFontStyleItalic).ImageList = ilButtons;
		((Control)tbFontStyleItalic).Location = new Point(116, 3);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(22, 21);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 7;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		((Control)tbFontStyleItalic).Click += tbFontStyleItalic_Click;
		((Control)tbFontStyleBold).Dock = (DockStyle)3;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).ImageIndex = 0;
		((ButtonBase)tbFontStyleBold).ImageList = ilButtons;
		((Control)tbFontStyleBold).Location = new Point(94, 3);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(22, 21);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 6;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)tbFontStyleBold).Click += tbFontStyleBold_Click;
		((Control)stiToolButton2).Dock = (DockStyle)3;
		stiToolButton2.Draw3DButton = false;
		stiToolButton2.HotDragDrop = false;
		((ButtonBase)stiToolButton2).ImageIndex = 11;
		((Control)stiToolButton2).Location = new Point(88, 3);
		((Control)stiToolButton2).Name = "stiToolButton2";
		((Control)stiToolButton2).Size = new Size(6, 21);
		stiToolButton2.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton2).TabIndex = 14;
		((ButtonBase)stiToolButton2).UseVisualStyleBackColor = false;
		((Control)tbFont).AutoSize = true;
		((Control)tbFont).Dock = (DockStyle)3;
		tbFont.Draw3DButton = false;
		tbFont.HotDragDrop = false;
		((Control)tbFont).Location = new Point(6, 3);
		((Control)tbFont).Name = "tbFont";
		((Control)tbFont).Size = new Size(82, 21);
		((Control)tbFont).TabIndex = 5;
		((Control)tbFont).Text = "Change Font";
		((ButtonBase)tbFont).UseVisualStyleBackColor = false;
		((Control)tbFont).Click += tbFont_Click;
		toolButtonAddLevel.Draw3DButton = false;
		toolButtonAddLevel.HotDragDrop = false;
		((Control)toolButtonAddLevel).Location = new Point(8, 4);
		((Control)toolButtonAddLevel).Name = "toolButtonAddLevel";
		((Control)toolButtonAddLevel).Size = new Size(131, 23);
		((Control)toolButtonAddLevel).TabIndex = 0;
		((Control)toolButtonAddLevel).Text = "Add Level";
		((ButtonBase)toolButtonAddLevel).UseVisualStyleBackColor = false;
		((Control)checkBoxBreakIfTrue).Anchor = (AnchorStyles)9;
		((Control)checkBoxBreakIfTrue).AutoSize = true;
		((Control)checkBoxBreakIfTrue).Location = new Point(130, 2);
		((Control)checkBoxBreakIfTrue).Name = "checkBoxBreakIfTrue";
		((Control)checkBoxBreakIfTrue).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxBreakIfTrue).Size = new Size(95, 17);
		((Control)checkBoxBreakIfTrue).TabIndex = 3;
		((Control)checkBoxBreakIfTrue).Text = "Break if true";
		((ButtonBase)checkBoxBreakIfTrue).UseVisualStyleBackColor = true;
		((Control)panelBreakIfTrue).Anchor = (AnchorStyles)9;
		((Control)panelBreakIfTrue).BackColor = Color.White;
		((Control)panelBreakIfTrue).Controls.Add((Control)(object)checkBoxBreakIfTrue);
		((Control)panelBreakIfTrue).Location = new Point(454, 8);
		((Control)panelBreakIfTrue).Name = "panelBreakIfTrue";
		((Control)panelBreakIfTrue).Size = new Size(228, 21);
		((Control)panelBreakIfTrue).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).AutoSize = true;
		((UserControl)this).AutoSizeMode = (AutoSizeMode)0;
		((Control)this).Controls.Add((Control)(object)panelBreakIfTrue);
		((Control)this).Controls.Add((Control)(object)panel3);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Controls.Add((Control)(object)radioButtonOr);
		((Control)this).Controls.Add((Control)(object)radioButtonAnd);
		((Control)this).Controls.Add((Control)(object)toolButtonAddLevel);
		((Control)this).Controls.Add((Control)(object)panelConditions);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "StiConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(727, 103);
		((UserControl)this).Load += StiConditionZoneControl_Load;
		((Control)panel3).ResumeLayout(false);
		((Control)panel3).PerformLayout();
		((Control)panel1).ResumeLayout(false);
		((Control)tbTool).ResumeLayout(false);
		((Control)tbTool).PerformLayout();
		((Control)panelBreakIfTrue).ResumeLayout(false);
		((Control)panelBreakIfTrue).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
