using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.Editors;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiFormattingToolbarService : StiToolbarService
{
	private ComboBoxItem comboBoxFont;

	private ComboBoxItem comboBoxFontSize;

	private ColorPickerDropDown colorPickerTextBrush;

	private ButtonItem buttonBold;

	private ButtonItem buttonItalic;

	private ButtonItem buttonUnderline;

	private ButtonItem buttonAlignLeft;

	private ButtonItem buttonAlignCenter;

	private ButtonItem buttonAlignRight;

	private ButtonItem buttonAlignWidth;

	private ButtonItem buttonAlignTop;

	private ButtonItem buttonAlignMiddle;

	private ButtonItem buttonAlignBottom;

	private ButtonItem buttonRotateText;

	private ButtonItem buttonConditions;

	private ButtonItem itemRotateAngle0;

	private ButtonItem itemRotateAngle45;

	private ButtonItem itemRotateAngle90;

	private ButtonItem itemRotateAngle180;

	private ButtonItem itemRotateAngle270;

	private bool locked;

	public override string ServiceName => StiLocalization.Get("Toolbars", "ToolbarFormatting");

	public override int Position => 4;

	public override int DefaultDockLine => 2;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFont
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowFont;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowFont = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFontSize
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowFontSize;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowFontSize = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTextBrush
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowTextBrush;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowTextBrush = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBold
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowBold;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowBold = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowItalic
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowItalic;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowItalic = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowUnderline
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowUnderline;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowUnderline = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignLeft
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignLeft;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignLeft = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignCenter
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignCenter;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignCenter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignRight
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignRight;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignRight = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignWidth
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignWidth;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignWidth = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignTop
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignTop;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignTop = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignMiddle
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignMiddle;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignMiddle = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignBottom
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignBottom;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowAlignBottom = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowRotateText
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowRotateText;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowRotateText = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowConditions
	{
		get
		{
			return StiOptions.Designer.Toolbars.FormattingToolbar.ShowConditions;
		}
		set
		{
			StiOptions.Designer.Toolbars.FormattingToolbar.ShowConditions = value;
		}
	}

	private void RefreshContentAlignment()
	{
		object obj = Report?.GetCurrentPage().GetVertAlignment();
		if (obj != null && !CheckPropertyEnabled(Report, "VertAlignment"))
		{
			obj = null;
		}
		if (obj == null)
		{
			if (buttonAlignTop != null && buttonAlignTop.Enabled)
			{
				buttonAlignTop.Enabled = false;
			}
			if (buttonAlignMiddle != null && buttonAlignMiddle.Enabled)
			{
				buttonAlignMiddle.Enabled = false;
			}
			if (buttonAlignBottom != null && buttonAlignBottom.Enabled)
			{
				buttonAlignBottom.Enabled = false;
			}
		}
		else
		{
			if (buttonAlignTop != null && !buttonAlignTop.Enabled)
			{
				buttonAlignTop.Enabled = true;
			}
			if (buttonAlignMiddle != null && !buttonAlignMiddle.Enabled)
			{
				buttonAlignMiddle.Enabled = true;
			}
			if (buttonAlignBottom != null && !buttonAlignBottom.Enabled)
			{
				buttonAlignBottom.Enabled = true;
			}
			StiVertAlignment stiVertAlignment = (StiVertAlignment)obj;
			if (buttonAlignTop != null)
			{
				buttonAlignTop.Checked = stiVertAlignment == StiVertAlignment.Top;
			}
			if (buttonAlignMiddle != null)
			{
				buttonAlignMiddle.Checked = stiVertAlignment == StiVertAlignment.Center;
			}
			if (buttonAlignBottom != null)
			{
				buttonAlignBottom.Checked = stiVertAlignment == StiVertAlignment.Bottom;
			}
		}
		object obj2 = Report?.GetCurrentPage().GetTextHorAlignment();
		if (obj2 != null && !CheckPropertyEnabled(Report, "HorAlignment"))
		{
			obj2 = null;
		}
		if (obj2 == null)
		{
			if (buttonAlignWidth != null && buttonAlignWidth.Enabled)
			{
				buttonAlignWidth.Enabled = false;
			}
			object obj3 = Report?.GetCurrentPage().GetHorAlignment();
			if (obj3 == null)
			{
				if (buttonAlignLeft != null && buttonAlignLeft.Enabled)
				{
					buttonAlignLeft.Enabled = false;
				}
				if (buttonAlignCenter != null && buttonAlignCenter.Enabled)
				{
					buttonAlignCenter.Enabled = false;
				}
				if (buttonAlignRight != null && buttonAlignRight.Enabled)
				{
					buttonAlignRight.Enabled = false;
				}
				return;
			}
			if (buttonAlignLeft != null && !buttonAlignLeft.Enabled)
			{
				buttonAlignLeft.Enabled = true;
			}
			if (buttonAlignCenter != null && !buttonAlignCenter.Enabled)
			{
				buttonAlignCenter.Enabled = true;
			}
			if (buttonAlignRight != null && !buttonAlignRight.Enabled)
			{
				buttonAlignRight.Enabled = true;
			}
			StiHorAlignment stiHorAlignment = (StiHorAlignment)obj3;
			if (buttonAlignLeft != null)
			{
				buttonAlignLeft.Checked = stiHorAlignment == StiHorAlignment.Left;
			}
			if (buttonAlignCenter != null)
			{
				buttonAlignCenter.Checked = stiHorAlignment == StiHorAlignment.Center;
			}
			if (buttonAlignRight != null)
			{
				buttonAlignRight.Checked = stiHorAlignment == StiHorAlignment.Right;
			}
		}
		else
		{
			if (buttonAlignLeft != null && !buttonAlignLeft.Enabled)
			{
				buttonAlignLeft.Enabled = true;
			}
			if (buttonAlignCenter != null && !buttonAlignCenter.Enabled)
			{
				buttonAlignCenter.Enabled = true;
			}
			if (buttonAlignRight != null && !buttonAlignRight.Enabled)
			{
				buttonAlignRight.Enabled = true;
			}
			if (buttonAlignWidth != null && !buttonAlignWidth.Enabled)
			{
				buttonAlignWidth.Enabled = true;
			}
			StiTextHorAlignment stiTextHorAlignment = (StiTextHorAlignment)obj2;
			if (buttonAlignLeft != null)
			{
				buttonAlignLeft.Checked = stiTextHorAlignment == StiTextHorAlignment.Left;
			}
			if (buttonAlignCenter != null)
			{
				buttonAlignCenter.Checked = stiTextHorAlignment == StiTextHorAlignment.Center;
			}
			if (buttonAlignRight != null)
			{
				buttonAlignRight.Checked = stiTextHorAlignment == StiTextHorAlignment.Right;
			}
			if (buttonAlignWidth != null)
			{
				buttonAlignWidth.Checked = stiTextHorAlignment == StiTextHorAlignment.Width;
			}
		}
	}

	private void RefreshTextBrush()
	{
		StiBrush stiBrush = ((Report != null) ? (Report.GetCurrentPage().GetTextBrush() as StiBrush) : null);
		if (stiBrush != null && !CheckPropertyEnabled(Report, "TextBrush"))
		{
			stiBrush = null;
		}
		if (stiBrush != null)
		{
			if (colorPickerTextBrush != null)
			{
				colorPickerTextBrush.SelectedColor = StiBrush.ToColor(stiBrush);
			}
			if (colorPickerTextBrush != null && !colorPickerTextBrush.Enabled)
			{
				colorPickerTextBrush.Enabled = true;
			}
		}
		else if (colorPickerTextBrush != null && colorPickerTextBrush.Enabled)
		{
			colorPickerTextBrush.Enabled = false;
		}
	}

	private void RefreshFont()
	{
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Invalid comparison between Unknown and I4
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Invalid comparison between Unknown and I4
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Invalid comparison between Unknown and I4
		object obj = Report?.GetCurrentPage().GetFont();
		if (obj != null && !CheckPropertyEnabled(Report, "Font"))
		{
			obj = null;
		}
		if (obj != null)
		{
			if (comboBoxFont != null && !comboBoxFont.Enabled)
			{
				comboBoxFont.Enabled = true;
			}
			if (comboBoxFontSize != null && !comboBoxFontSize.Enabled)
			{
				comboBoxFontSize.Enabled = true;
			}
			if (buttonBold != null && !buttonBold.Enabled)
			{
				buttonBold.Enabled = true;
			}
			if (buttonItalic != null && !buttonItalic.Enabled)
			{
				buttonItalic.Enabled = true;
			}
			if (buttonUnderline != null && !buttonUnderline.Enabled)
			{
				buttonUnderline.Enabled = true;
			}
			string text = ((comboBoxFont.SelectedItem != null) ? ((ComboItem)comboBoxFont.SelectedItem).FontName : string.Empty);
			while (comboBoxFont.Items.Count > 0 && ((ComboItem)comboBoxFont.Items[0]).IsResourceFontItem)
			{
				comboBoxFont.Items.RemoveAt(0);
			}
			ComboItem comboItem = null;
			List<StiResource> allFonts = StiFontResourceHelper.GetAllFonts(Designer.Report);
			foreach (StiResource item in allFonts.OrderByDescending((StiResource f) => f.Name))
			{
				string resourceFontName = Designer.Report.GetResourceFontName(item.Name);
				ComboItem comboItem2 = new ComboItem
				{
					IsResourceFontItem = true,
					FontName = resourceFontName,
					Text = resourceFontName
				};
				if (text.Equals(resourceFontName))
				{
					comboItem = comboItem2;
				}
				comboBoxFont.Items.Insert(0, (object)comboItem2);
			}
			if (comboItem != null)
			{
				comboBoxFont.SelectedItem = comboItem;
			}
			Font val = (Font)obj;
			if (comboBoxFont != null)
			{
				comboBoxFont.Text = val.FontFamily.Name;
			}
			if (comboBoxFontSize != null && comboBoxFontSize.Text != val.Size.ToString())
			{
				comboBoxFontSize.Text = val.Size.ToString();
			}
			bool @checked = (val.Style & 1) > 0;
			bool checked2 = (val.Style & 2) > 0;
			bool checked3 = (val.Style & 4) > 0;
			if (buttonBold != null)
			{
				buttonBold.Checked = @checked;
			}
			if (buttonItalic != null)
			{
				buttonItalic.Checked = checked2;
			}
			if (buttonUnderline != null)
			{
				buttonUnderline.Checked = checked3;
			}
		}
		else
		{
			if (comboBoxFont != null && comboBoxFont.Enabled)
			{
				comboBoxFont.Enabled = false;
			}
			if (comboBoxFontSize != null && comboBoxFontSize.Enabled)
			{
				comboBoxFontSize.Enabled = false;
			}
			if (buttonBold != null && buttonBold.Enabled)
			{
				buttonBold.Enabled = false;
			}
			if (buttonItalic != null && buttonItalic.Enabled)
			{
				buttonItalic.Enabled = false;
			}
			if (buttonUnderline != null && buttonUnderline.Enabled)
			{
				buttonUnderline.Enabled = false;
			}
		}
	}

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		if (!base.ToolBar.Visible)
		{
			return;
		}
		locked = true;
		base.ToolBar.SuspendLayout();
		RefreshContentAlignment();
		RefreshTextBrush();
		RefreshFont();
		bool flag = false;
		if (CheckPropertyEnabled(Report, "Angle"))
		{
			foreach (StiComponent item in Designer.SelectedComponentsOnPage)
			{
				if ((item is IStiTextOptions || item is IStiEnumAngle) && StiRestrictionsHelper.IsAllowChange(item))
				{
					flag = true;
					break;
				}
			}
		}
		if (buttonRotateText != null && buttonRotateText.Enabled != flag)
		{
			buttonRotateText.Enabled = flag;
		}
		if (buttonConditions != null)
		{
			bool flag2 = !(Designer.SelectedObject is StiComponent) || StiRestrictionsHelper.IsAllowChange((StiComponent)Designer.SelectedObject);
			if (flag2 && !CheckPropertyEnabled(Report, "Conditions"))
			{
				flag2 = false;
			}
			buttonConditions.Enabled = !(Designer.SelectedObject is StiReport) && Report != null && flag2;
		}
		base.ToolBar.ResumeLayout();
		locked = false;
	}

	private void ComboBoxFontSizeEnabledChanged(object sender, EventArgs e)
	{
		((ComboBox)comboBoxFontSize.ComboBoxEx).DropDownStyle = (ComboBoxStyle)(comboBoxFontSize.Enabled ? 1 : 2);
	}

	private void ButtonConditions_Click(object sender, EventArgs e)
	{
		StiActions.Style.InvokeConditions(Designer);
	}

	private void ButtonBold_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeBold(Designer, buttonBold.Checked);
	}

	private void ButtonItalic_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeItalic(Designer, buttonItalic.Checked);
	}

	private void ButtonUnderline_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeUnderline(Designer, buttonUnderline.Checked);
	}

	private void ButtonAlignLeft_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignLeft(Designer);
	}

	private void ButtonAlignCenter_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignCenter(Designer);
	}

	private void ButtonAlignRight_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignRight(Designer);
	}

	private void ButtonAlignWidth_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignWidth(Designer);
	}

	private void ButtonAlignTop_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignTop(Designer);
	}

	private void ButtonAlignMiddle_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignMiddle(Designer);
	}

	private void tbAlignBottom_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignBottom(Designer);
	}

	private void ButtonRotateTextPopupOpen(object sender, PopupOpenEventArgs e)
	{
		foreach (StiComponent item in Designer.SelectedComponentsOnPage)
		{
			if (item is IStiTextOptions)
			{
				itemRotateAngle45.Visible = true;
			}
			if (item is IStiEnumAngle)
			{
				itemRotateAngle45.Visible = false;
				break;
			}
		}
		object obj = null;
		bool flag = false;
		foreach (StiComponent item2 in Designer.SelectedComponentsOnPage)
		{
			if (item2 is IStiTextOptions && !flag)
			{
				float angle = ((IStiTextOptions)item2).TextOptions.Angle;
				if (obj == null)
				{
					obj = angle;
					continue;
				}
				if (angle == (float)obj)
				{
					continue;
				}
				flag = true;
				obj = null;
			}
			if (!(item2 is IStiEnumAngle))
			{
				continue;
			}
			itemRotateAngle45.Visible = false;
			if (!flag)
			{
				float num = (float)((IStiEnumAngle)item2).Angle;
				if (obj == null)
				{
					obj = num;
				}
				else if (num != (float)obj)
				{
					flag = true;
					obj = null;
				}
			}
		}
		itemRotateAngle0.Checked = obj != null && (float)obj == 0f;
		itemRotateAngle45.Checked = obj != null && (float)obj == 45f;
		itemRotateAngle90.Checked = obj != null && (float)obj == 90f;
		itemRotateAngle180.Checked = obj != null && (float)obj == 180f;
		itemRotateAngle270.Checked = obj != null && (float)obj == 270f;
	}

	private void ComboBoxFontSizeSelectedIndexChanged(object sender, EventArgs e)
	{
		if (locked)
		{
			return;
		}
		try
		{
			if (!(comboBoxFontSize.Text == ""))
			{
				float fontSize = float.Parse(comboBoxFontSize.Text);
				StiActions.Font.InvokeFontSize(Designer, fontSize);
			}
		}
		catch
		{
		}
	}

	private void ComboBoxFontSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode != 13 || locked)
		{
			return;
		}
		try
		{
			if (comboBoxFontSize.Text != "")
			{
				float fontSize = float.Parse(comboBoxFontSize.Text);
				StiActions.Font.InvokeFontSize(Designer, fontSize);
			}
		}
		catch
		{
		}
		e.Handled = true;
	}

	private void ComboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (locked || ((ComboBox)comboBoxFont.ComboBoxEx).SelectedItem == null)
		{
			return;
		}
		try
		{
			StiActions.Font.InvokeFontName(Designer, ((ComboItem)((ComboBox)comboBoxFont.ComboBoxEx).SelectedItem).FontName);
		}
		catch
		{
		}
	}

	private void ItemRotate0_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate0(Designer);
	}

	private void ItemRotate45_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate45(Designer);
	}

	private void ItemRotate90_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate90(Designer);
	}

	private void ItemRotate180_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate180(Designer);
	}

	private void ItemRotate270_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate270(Designer);
	}

	private void ButtonTextBrush_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			StiActions.Font.InvokeTextBrush(Designer, colorPickerTextBrush.SelectedColor);
		}
	}

	private void LocalizeSuperToolTip()
	{
		if (Designer == null)
		{
			return;
		}
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (comboBoxFont != null)
			{
				SetSuperToolTip(comboBoxFont, StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontName")), null, hotKey: false, "FontName", globalMeasureGraphics);
			}
			if (comboBoxFontSize != null)
			{
				SetSuperToolTip(comboBoxFontSize, StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontSize")), null, hotKey: false, "FontSize", globalMeasureGraphics);
			}
			if (buttonBold != null)
			{
				SetSuperToolTip(buttonBold, StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleBold")), " (Ctrl + B)", hotKey: true, "FontStyleBold", globalMeasureGraphics);
			}
			if (buttonItalic != null)
			{
				SetSuperToolTip(buttonItalic, StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleItalic")), " (Ctrl + I)", hotKey: true, "FontStyleItalic", globalMeasureGraphics);
			}
			if (buttonUnderline != null)
			{
				SetSuperToolTip(buttonUnderline, StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleUnderline")), " (Ctrl + U)", hotKey: true, "FontStyleUnderline", globalMeasureGraphics);
			}
			if (buttonAlignLeft != null)
			{
				SetSuperToolTip(buttonAlignLeft, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft")), " (Ctrl + L)", hotKey: true, "AlignLeft", globalMeasureGraphics);
			}
			if (buttonAlignCenter != null)
			{
				SetSuperToolTip(buttonAlignCenter, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter")), " (Ctrl + E)", hotKey: true, "AlignCenter", globalMeasureGraphics);
			}
			if (buttonAlignRight != null)
			{
				SetSuperToolTip(buttonAlignRight, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight")), " (Ctrl + R)", hotKey: true, "AlignRight", globalMeasureGraphics);
			}
			if (buttonAlignWidth != null)
			{
				SetSuperToolTip(buttonAlignWidth, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth")), " (Ctrl + J)", hotKey: true, "AlignWidth", globalMeasureGraphics);
			}
			if (buttonAlignTop != null)
			{
				SetSuperToolTip(buttonAlignTop, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop")), null, hotKey: false, "AlignTop", globalMeasureGraphics);
			}
			if (buttonAlignMiddle != null)
			{
				SetSuperToolTip(buttonAlignMiddle, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle")), null, hotKey: false, "AlignMiddle", globalMeasureGraphics);
			}
			if (buttonAlignBottom != null)
			{
				SetSuperToolTip(buttonAlignBottom, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom")), null, hotKey: false, "AlignBottom", globalMeasureGraphics);
			}
			if (buttonRotateText != null)
			{
				SetSuperToolTip(buttonRotateText, StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Angle")), null, hotKey: false, "Angle", globalMeasureGraphics);
			}
			if (colorPickerTextBrush != null)
			{
				SetSuperToolTip(colorPickerTextBrush, StiHint.CreateHint(StiLocalization.Get("PropertyMain", "TextColor")), null, hotKey: false, "TextColor", globalMeasureGraphics);
			}
			if (buttonConditions != null)
			{
				SetSuperToolTip(buttonConditions, StiHint.CreateHint(StiLocalization.Get("Toolbars", "Conditions")), null, hotKey: false, "biConditions", globalMeasureGraphics);
			}
		}
		else
		{
			if (comboBoxFont != null)
			{
				comboBoxFont.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontName"));
			}
			if (comboBoxFontSize != null)
			{
				comboBoxFontSize.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontSize"));
			}
			if (buttonBold != null)
			{
				buttonBold.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleBold")) + " (Ctrl + B)";
			}
			if (buttonItalic != null)
			{
				buttonItalic.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleItalic")) + " (Ctrl + I)";
			}
			if (buttonUnderline != null)
			{
				buttonUnderline.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleUnderline")) + " (Ctrl + U)";
			}
			if (buttonAlignLeft != null)
			{
				buttonAlignLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft")) + " (Ctrl + L)";
			}
			if (buttonAlignCenter != null)
			{
				buttonAlignCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter")) + " (Ctrl + E)";
			}
			if (buttonAlignRight != null)
			{
				buttonAlignRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight")) + " (Ctrl + R)";
			}
			if (buttonAlignWidth != null)
			{
				buttonAlignWidth.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth")) + " (Ctrl + J)";
			}
			if (buttonAlignTop != null)
			{
				buttonAlignTop.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
			}
			if (buttonAlignMiddle != null)
			{
				buttonAlignMiddle.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
			}
			if (buttonAlignBottom != null)
			{
				buttonAlignBottom.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
			}
			if (buttonRotateText != null)
			{
				buttonRotateText.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Angle"));
			}
			if (colorPickerTextBrush != null)
			{
				colorPickerTextBrush.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "TextColor"));
			}
			if (buttonConditions != null)
			{
				buttonConditions.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Conditions"));
			}
		}
	}

	private void SetSuperToolTip(BaseItem item, string header, string hotKeyText, bool hotKey, string localizeName, Graphics g)
	{
		string bodyText = StiLocalization.Get("HelpDesigner", localizeName);
		SuperTooltipInfo superTooltipInfo = (hotKey ? new SuperTooltipInfo(header + hotKeyText, null, bodyText, null, null, Designer.CurrentTooltipColor) : new SuperTooltipInfo(header, null, bodyText, null, null, Designer.CurrentTooltipColor));
		superTooltipInfo.FooterVisible = false;
		SizeF sizeF = Designer.SuperToolTip.MeasureHeaderText(g, header);
		SizeF sizeF2 = Designer.SuperToolTip.MeasureBodyText(g, bodyText);
		int width = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + 15) : ((int)Math.Round((double)sizeF2.Width + 0.5) + 25));
		superTooltipInfo.CustomSize = new Size(width, (int)Math.Round(sizeF2.Height + sizeF.Height) + 15);
		Designer.SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	public void InvokeConditions()
	{
		ButtonConditions_Click(null, EventArgs.Empty);
	}

	public void InvokeBold()
	{
		ButtonBold_Click(null, EventArgs.Empty);
	}

	public void InvokeItalic()
	{
		ButtonItalic_Click(null, EventArgs.Empty);
	}

	public void InvokeUnderline()
	{
		ButtonUnderline_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignLeft()
	{
		ButtonAlignLeft_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignCenter()
	{
		ButtonAlignCenter_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignRight()
	{
		ButtonAlignRight_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignWidth()
	{
		ButtonAlignWidth_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignTop()
	{
		ButtonAlignTop_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignMiddle()
	{
		ButtonAlignMiddle_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignBottom()
	{
		tbAlignBottom_Click(null, EventArgs.Empty);
	}

	[Obsolete("'InvokeRotateText' method is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InvokeRotateText()
	{
	}

	public static StiFormattingToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiFormattingToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiFormattingToolbarService)) as StiFormattingToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiFormattingToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarFormatting");
		}
		if (itemRotateAngle0 != null)
		{
			itemRotateAngle0.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle0");
		}
		if (itemRotateAngle45 != null)
		{
			itemRotateAngle45.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle45");
		}
		if (itemRotateAngle90 != null)
		{
			itemRotateAngle90.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle90");
		}
		if (itemRotateAngle180 != null)
		{
			itemRotateAngle180.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle180");
		}
		if (itemRotateAngle270 != null)
		{
			itemRotateAngle270.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle270");
		}
		if (comboBoxFont != null)
		{
			comboBoxFont.Caption = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontName"));
		}
		if (comboBoxFontSize != null)
		{
			comboBoxFontSize.Caption = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontSize"));
		}
		if (buttonBold != null)
		{
			buttonBold.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleBold"));
		}
		if (buttonItalic != null)
		{
			buttonItalic.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleItalic"));
		}
		if (buttonUnderline != null)
		{
			buttonUnderline.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleUnderline"));
		}
		if (buttonAlignLeft != null)
		{
			buttonAlignLeft.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		}
		if (buttonAlignCenter != null)
		{
			buttonAlignCenter.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		}
		if (buttonAlignRight != null)
		{
			buttonAlignRight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		}
		if (buttonAlignWidth != null)
		{
			buttonAlignWidth.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth"));
		}
		if (buttonAlignTop != null)
		{
			buttonAlignTop.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
		}
		if (buttonAlignMiddle != null)
		{
			buttonAlignMiddle.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
		}
		if (buttonAlignBottom != null)
		{
			buttonAlignBottom.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
		}
		if (buttonRotateText != null)
		{
			buttonRotateText.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Angle"));
		}
		if (colorPickerTextBrush != null)
		{
			colorPickerTextBrush.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "TextColor"));
		}
		if (buttonConditions != null)
		{
			buttonConditions.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Conditions"));
		}
		LocalizeSuperToolTip();
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarFormatting");
		if (ShowFont)
		{
			comboBoxFont = new ComboBoxItem
			{
				Caption = StiLocalization.Get("Toolbars", "FontName"),
				Name = "comboBoxFont",
				ItemHeight = StiScale.I15,
				FontCombo = true,
				ComboWidth = StiScale.I(130),
				DropDownHeight = StiScale.I(500)
			};
			((ComboBox)comboBoxFont.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
			((ComboBox)comboBoxFont.ComboBoxEx).DropDownWidth = StiScale.I(300);
			comboBoxFont.ComboBoxEx.AllowCustomFontDrawing = StiOptions.Designer.AllowCustomFontDrawing;
			((ComboBox)comboBoxFont.ComboBoxEx).SelectedIndexChanged += ComboBoxFont_SelectedIndexChanged;
			base.ToolBar.Items.Add(comboBoxFont);
		}
		AddToolButtonSeparator();
		if (ShowFontSize)
		{
			comboBoxFontSize = new ComboBoxItem
			{
				Caption = StiLocalization.Get("Toolbars", "FontSize"),
				Name = "comboBoxFontSize",
				ItemHeight = StiScale.I(15),
				DropDownHeight = StiScale.I(300),
				ComboWidth = StiScale.I(50)
			};
			comboBoxFontSize.EnabledChanged += ComboBoxFontSizeEnabledChanged;
			((ComboBox)comboBoxFontSize.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
			comboBoxFontSize.Items.Add((object)"5");
			comboBoxFontSize.Items.Add((object)"6");
			comboBoxFontSize.Items.Add((object)"7");
			comboBoxFontSize.Items.Add((object)"8");
			comboBoxFontSize.Items.Add((object)"9");
			comboBoxFontSize.Items.Add((object)"10");
			comboBoxFontSize.Items.Add((object)"11");
			comboBoxFontSize.Items.Add((object)"12");
			comboBoxFontSize.Items.Add((object)"14");
			comboBoxFontSize.Items.Add((object)"16");
			comboBoxFontSize.Items.Add((object)"18");
			comboBoxFontSize.Items.Add((object)"20");
			comboBoxFontSize.Items.Add((object)"22");
			comboBoxFontSize.Items.Add((object)"24");
			comboBoxFontSize.Items.Add((object)"26");
			comboBoxFontSize.Items.Add((object)"28");
			comboBoxFontSize.Items.Add((object)"36");
			comboBoxFontSize.Items.Add((object)"48");
			comboBoxFontSize.Items.Add((object)"72");
			comboBoxFontSize.SelectedIndexChanged += ComboBoxFontSizeSelectedIndexChanged;
			((Control)comboBoxFontSize.ComboBoxEx).KeyDown += new KeyEventHandler(ComboBoxFontSize_KeyDown);
			base.ToolBar.Items.Add(comboBoxFontSize);
		}
		AddToolButtonSeparator();
		if (ShowBold)
		{
			Add(ref buttonBold, StiReportDesignImages.Editor.FontStyleBold(), StiLocalization.Get("Toolbars", "FontStyleBold") + " (Ctrl + B)", ButtonBold_Click);
			buttonBold.AutoCheckOnClick = true;
			buttonBold.Name = "buttonBold";
		}
		if (ShowItalic)
		{
			Add(ref buttonItalic, StiReportDesignImages.Editor.FontStyleItalic(), StiLocalization.Get("Toolbars", "FontStyleItalic") + " (Ctrl + I)", ButtonItalic_Click);
			buttonItalic.AutoCheckOnClick = true;
			buttonItalic.Name = "buttonItalic";
		}
		if (ShowUnderline)
		{
			Add(ref buttonUnderline, StiReportDesignImages.Editor.FontStyleUnderline(), StiLocalization.Get("Toolbars", "FontStyleUnderline") + " (Ctrl + U)", ButtonUnderline_Click);
			buttonUnderline.AutoCheckOnClick = true;
			buttonUnderline.Name = "buttonUnderline";
		}
		AddToolButtonSeparator();
		if (ShowAlignLeft)
		{
			Add(ref buttonAlignLeft, StiReportDesignImages.Editor.AlignLeft(), StiLocalization.Get("Toolbars", "AlignLeft") + " (Ctrl + L)", ButtonAlignLeft_Click);
			buttonAlignLeft.Name = "buttonAlignLeft";
		}
		if (ShowAlignCenter)
		{
			Add(ref buttonAlignCenter, StiReportDesignImages.Editor.AlignCenter(), StiLocalization.Get("Toolbars", "AlignCenter") + " (Ctrl + E)", ButtonAlignCenter_Click);
			buttonAlignCenter.Name = "buttonAlignCenter";
		}
		if (ShowAlignRight)
		{
			Add(ref buttonAlignRight, StiReportDesignImages.Editor.AlignRight(), StiLocalization.Get("Toolbars", "AlignRight") + " (Ctrl + R)", ButtonAlignRight_Click);
			buttonAlignRight.Name = "buttonAlignRight";
		}
		if (ShowAlignWidth)
		{
			Add(ref buttonAlignWidth, StiReportDesignImages.Editor.AlignJustify(), StiLocalization.Get("Toolbars", "AlignWidth") + " (Ctrl + J)", ButtonAlignWidth_Click);
			buttonAlignWidth.Name = "buttonAlignWidth";
		}
		AddToolButtonSeparator();
		if (ShowAlignTop)
		{
			Add(ref buttonAlignTop, StiReportDesignImages.Editor.AlignTop(), StiLocalization.Get("Toolbars", "AlignTop"), ButtonAlignTop_Click);
			buttonAlignTop.Name = "buttonAlignTop";
		}
		if (ShowAlignMiddle)
		{
			Add(ref buttonAlignMiddle, StiReportDesignImages.Editor.AlignMiddle(), StiLocalization.Get("Toolbars", "AlignMiddle"), ButtonAlignMiddle_Click);
			buttonAlignMiddle.Name = "buttonAlignMiddle";
		}
		if (ShowAlignBottom)
		{
			Add(ref buttonAlignBottom, StiReportDesignImages.Editor.AlignBottom(), StiLocalization.Get("Toolbars", "AlignBottom"), tbAlignBottom_Click);
			buttonAlignBottom.Name = "buttonAlignBottom";
		}
		AddToolButtonSeparator();
		if (ShowRotateText)
		{
			Add(ref buttonRotateText, StiReportDesignImages.Editor.RotateText(), StiLocalization.Get("PropertyMain", "Angle"), null);
			buttonRotateText.Name = "buttonRotateText";
			buttonRotateText.AutoExpandOnClick = true;
			buttonRotateText.PopupOpen += ButtonRotateTextPopupOpen;
			itemRotateAngle0 = new ButtonItem();
			itemRotateAngle0.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle0");
			itemRotateAngle0.Click += ItemRotate0_Click;
			itemRotateAngle45 = new ButtonItem();
			itemRotateAngle45.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle45");
			itemRotateAngle45.Click += ItemRotate45_Click;
			itemRotateAngle90 = new ButtonItem();
			itemRotateAngle90.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle90");
			itemRotateAngle90.Click += ItemRotate90_Click;
			itemRotateAngle180 = new ButtonItem();
			itemRotateAngle180.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle180");
			itemRotateAngle180.Click += ItemRotate180_Click;
			itemRotateAngle270 = new ButtonItem();
			itemRotateAngle270.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle270");
			itemRotateAngle270.Click += ItemRotate270_Click;
			buttonRotateText.SubItems.AddRange(new BaseItem[5] { itemRotateAngle0, itemRotateAngle45, itemRotateAngle90, itemRotateAngle180, itemRotateAngle270 });
		}
		AddToolButtonSeparator();
		if (ShowTextBrush)
		{
			AddColorItem(ref colorPickerTextBrush, StiReportDesignImages.Editor.Brush(), StiHint.CreateHint(StiLocalization.Get("PropertyMain", "TextColor")), ButtonTextBrush_SelectedColorChanged);
			colorPickerTextBrush.Name = "colorPickerTextBrush";
			colorPickerTextBrush.SelectedColorImageRectangle = new Rectangle(StiScale.I1, StiScale.I14, StiScale.XXI(15.0), StiScale.I3);
		}
		if (ShowConditions)
		{
			Add(ref buttonConditions, StiReportDesignImages.Actions.Conditions(), StiLocalization.Get("Toolbars", "Conditions"), ButtonConditions_Click);
			buttonConditions.Name = "buttonConditions";
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		LocalizeSuperToolTip();
		designer.RefreshDesigner += Designer_RefreshDesigner;
	}

	public override void Dispose()
	{
		base.Dispose();
		comboBoxFont = null;
		comboBoxFontSize = null;
		colorPickerTextBrush = null;
		buttonBold = null;
		buttonItalic = null;
		buttonUnderline = null;
		buttonAlignLeft = null;
		buttonAlignCenter = null;
		comboBoxFontSize = null;
		buttonAlignRight = null;
		buttonAlignWidth = null;
		buttonAlignTop = null;
		buttonAlignMiddle = null;
		buttonAlignBottom = null;
		buttonRotateText = null;
		buttonConditions = null;
	}
}
