using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Wizards;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Design.Controls;

[ToolboxItem(false)]
public class StiHtmlTextEditorControl : UserControl, IStiHtmlTextEditorControl
{
	private bool locked;

	private int storedSelectionStart;

	private int storedSelectionLength;

	private StiElement element;

	private IContainer components;

	private Bar barElements;

	private ComboBoxItem comboBoxFontName;

	private ComboBoxItem comboBoxFontSize;

	private ButtonItem buttonFontBold;

	private ButtonItem buttonFontItalic;

	private ButtonItem buttonFontUnderline;

	private ColorPickerDropDown buttonFontColor;

	private ButtonItem buttonAlignLeft;

	private ButtonItem buttonAlignCenter;

	private ButtonItem buttonAlignRight;

	private ButtonItem buttonAlignJustify;

	private ButtonItem buttonClearAllFormatting;

	private ButtonItem buttonInsertSymbol;

	internal StiRichBoxControl richTextBox;

	private StiGroupLine topLine;

	internal ButtonItem buttonInsertConstant;

	private ButtonItem buttonItemTemp;

	private ButtonItem buttonInsertLink;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string HtmlText
	{
		get
		{
			return GetHtmlText();
		}
		set
		{
			((RichTextBox)richTextBox).Rtf = string.Empty;
			SetHtmlText(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontNameBox
	{
		get
		{
			return comboBoxFontName.Visible;
		}
		set
		{
			comboBoxFontName.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontSizeBox
	{
		get
		{
			return comboBoxFontSize.Visible;
		}
		set
		{
			comboBoxFontSize.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontBold
	{
		get
		{
			return buttonFontBold.Visible;
		}
		set
		{
			buttonFontBold.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontItalic
	{
		get
		{
			return buttonFontItalic.Visible;
		}
		set
		{
			buttonFontItalic.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontUnderline
	{
		get
		{
			return buttonFontUnderline.Visible;
		}
		set
		{
			buttonFontUnderline.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFontColor
	{
		get
		{
			return buttonFontColor.Visible;
		}
		set
		{
			buttonFontColor.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowAlignLeft
	{
		get
		{
			return buttonAlignLeft.Visible;
		}
		set
		{
			buttonAlignLeft.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowAlignCenter
	{
		get
		{
			return buttonAlignCenter.Visible;
		}
		set
		{
			buttonAlignCenter.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowAlignRight
	{
		get
		{
			return buttonAlignRight.Visible;
		}
		set
		{
			buttonAlignRight.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowAlignJustify
	{
		get
		{
			return buttonAlignJustify.Visible;
		}
		set
		{
			buttonAlignJustify.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowClearAllFormatting
	{
		get
		{
			return buttonClearAllFormatting.Visible;
		}
		set
		{
			buttonClearAllFormatting.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<string> Idents { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowInsertContstantButton
	{
		get
		{
			return buttonInsertConstant.Visible;
		}
		set
		{
			buttonInsertConstant.Visible = value;
		}
	}

	internal StiDesignerControl Designer => Element?.Report?.Designer?.DesignerControl as StiDesignerControl;

	public StiElement Element
	{
		get
		{
			return element;
		}
		set
		{
			if (element != value)
			{
				element = value;
				if (IsBackgroundExists)
				{
					topLine.LinePosition = StiLinePosition.None;
				}
				Color backColor = StiDashboardStyleHelper.GetBackColor(element);
				if (backColor.A != byte.MaxValue)
				{
					backColor = Color.FromArgb(255, backColor.R, backColor.G, backColor.B);
				}
				((Control)richTextBox).BackColor = backColor;
			}
		}
	}

	internal bool IsBackgroundExists
	{
		get
		{
			if (!(StiDashboardExpressionHelper.GetBackColor(Element, Element.BackColor) != Color.Transparent))
			{
				return StiDashboardStyleHelper.IsDarkStyle(Element?.Dashboard);
			}
			return true;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public RichTextBox RichTextBox => (RichTextBox)(object)richTextBox;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StiGroupLine TopLine => topLine;

	public event EventHandler ElementChanged;

	public StiHtmlTextEditorControl()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			comboBoxFontName.ItemHeight = 19;
			comboBoxFontSize.ItemHeight = 19;
			((Control)comboBoxFontSize.ComboBoxEx).KeyDown += new KeyEventHandler(ComboBoxFontSize_KeyDown);
			((ComboBox)comboBoxFontName.ComboBoxEx).SelectedIndexChanged += ComboBoxFontName_SelectedIndexChanged;
			((Control)richTextBox).AllowDrop = true;
			((RichTextBox)richTextBox).EnableAutoDragDrop = true;
			((RichTextBox)richTextBox).DragDrop += new DragEventHandler(RichTextBox_DragDrop);
			((RichTextBox)richTextBox).DragEnter += new DragEventHandler(RichTextBox_DragEnter);
			((RichTextBox)richTextBox).DragOver += new DragEventHandler(RichTextBox_DragOver);
			if (!StiCharterMapRunner.AllowChartermapWindow())
			{
				buttonInsertSymbol.Visible = false;
			}
			else
			{
				buttonClearAllFormatting.BeginGroup = false;
			}
			DoScaling();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (Element?.Dashboard == null)
		{
			((Control)this).OnPaint(e);
		}
		else if (IsBackgroundExists)
		{
			Color color = StiDashboardStyleHelper.GetBackColor(Element);
			if (color.A != byte.MaxValue)
			{
				color = Color.FromArgb(255, color.R, color.G, color.B);
			}
			StiDrawing.FillRectangle(e.Graphics, color, 0.0, ((Control)TopLine).Bottom, ((Control)this).Width, ((Control)this).Height - ((Control)TopLine).Bottom);
		}
	}

	private void ButtonFontBold_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			richTextBox.SetSelectionBold(buttonFontBold.Checked);
		}
	}

	private void ButtonFontItalic_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			richTextBox.SetSelectionItalic(buttonFontItalic.Checked);
		}
	}

	private void ButtonFontUnderline_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			richTextBox.SetSelectionUnderlined(buttonFontUnderline.Checked);
		}
	}

	private void ButtonAlignLeft_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			if (Element is IStiTextHorAlignment stiTextHorAlignment)
			{
				stiTextHorAlignment.HorAlignment = StiTextHorAlignment.Left;
				int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
				richTextBox.BeginUpdate();
				((TextBoxBase)richTextBox).SelectAll();
				richTextBox.SelectionAlignment = StiRichTextAlignment.Left;
				((TextBoxBase)richTextBox).DeselectAll();
				((TextBoxBase)richTextBox).SelectionStart = selectionStart;
				richTextBox.EndUpdate();
				((Control)richTextBox).Invalidate();
			}
			else
			{
				richTextBox.SelectionAlignment = StiRichTextAlignment.Left;
			}
			RefreshElementsStatus();
			InvokeElementChanged();
		}
	}

	private void ButtonAlignCenter_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			if (Element is IStiTextHorAlignment stiTextHorAlignment)
			{
				stiTextHorAlignment.HorAlignment = StiTextHorAlignment.Center;
				int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
				richTextBox.BeginUpdate();
				((TextBoxBase)richTextBox).SelectAll();
				richTextBox.SelectionAlignment = StiRichTextAlignment.Center;
				((TextBoxBase)richTextBox).DeselectAll();
				((TextBoxBase)richTextBox).SelectionStart = selectionStart;
				richTextBox.EndUpdate();
				((Control)richTextBox).Invalidate();
			}
			else
			{
				richTextBox.SelectionAlignment = StiRichTextAlignment.Center;
			}
			RefreshElementsStatus();
			InvokeElementChanged();
		}
	}

	private void ButtonAlignRight_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			if (Element is IStiTextHorAlignment stiTextHorAlignment)
			{
				stiTextHorAlignment.HorAlignment = StiTextHorAlignment.Right;
				int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
				richTextBox.BeginUpdate();
				((TextBoxBase)richTextBox).SelectAll();
				richTextBox.SelectionAlignment = StiRichTextAlignment.Right;
				((TextBoxBase)richTextBox).DeselectAll();
				((TextBoxBase)richTextBox).SelectionStart = selectionStart;
				richTextBox.EndUpdate();
				((Control)richTextBox).Invalidate();
			}
			else
			{
				richTextBox.SelectionAlignment = StiRichTextAlignment.Right;
			}
			RefreshElementsStatus();
			InvokeElementChanged();
		}
	}

	private void ButtonAlignJustify_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			if (Element is IStiTextHorAlignment stiTextHorAlignment)
			{
				stiTextHorAlignment.HorAlignment = StiTextHorAlignment.Width;
				int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
				richTextBox.BeginUpdate();
				((TextBoxBase)richTextBox).SelectAll();
				richTextBox.SelectionAlignment = StiRichTextAlignment.Justify;
				((TextBoxBase)richTextBox).DeselectAll();
				((TextBoxBase)richTextBox).SelectionStart = selectionStart;
				richTextBox.EndUpdate();
				((Control)richTextBox).Invalidate();
			}
			else
			{
				richTextBox.SelectionAlignment = StiRichTextAlignment.Justify;
			}
			RefreshElementsStatus();
			InvokeElementChanged();
		}
	}

	private void ButtonInsertSymbol_Click(object sender, EventArgs e)
	{
		string text = comboBoxFontName.Text;
		IStiCharterMapWindow stiCharterMapWindow = StiCharterMapRunner.StiWindow(text);
		if (stiCharterMapWindow == null)
		{
			return;
		}
		IntPtr intPtr = ((Form.ActiveForm != null) ? ((Control)Form.ActiveForm).Handle : IntPtr.Zero);
		if (stiCharterMapWindow.ShowDialog(intPtr) != true)
		{
			return;
		}
		string result = stiCharterMapWindow.Result;
		if (!string.IsNullOrEmpty(result))
		{
			string text2 = Clipboard.GetText();
			Clipboard.SetText(result);
			((TextBoxBase)richTextBox).Paste();
			Clipboard.Clear();
			if (!string.IsNullOrEmpty(text2))
			{
				Clipboard.SetText(text2);
			}
		}
	}

	private void ButtonClearAllFormatting_Click(object sender, EventArgs e)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			new StiHtmlTextHelper().SetHtmlText(((Control)richTextBox).Text, stiRichBoxControl, StiDashboardStyleHelper.GetForeColor(Element));
			((RichTextBox)richTextBox).Rtf = ((RichTextBox)stiRichBoxControl).Rtf;
			if (Element is IStiTextHorAlignment stiTextHorAlignment)
			{
				stiTextHorAlignment.HorAlignment = StiTextHorAlignment.Center;
			}
			if (Element is IStiVertAlignment stiVertAlignment)
			{
				stiVertAlignment.VertAlignment = StiVertAlignment.Center;
			}
			RefreshElementsStatus();
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	private void RichTextBox_SelectionChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			RefreshElementsStatus();
		}
	}

	private void ButtonFontColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((RichTextBox)richTextBox).SelectionColor = buttonFontColor.SelectedColor;
			RefreshElementsStatus();
		}
	}

	private void ComboBoxFontSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			ComboBoxFontSize_SelectedIndexChanged(sender, EventArgs.Empty);
			InvokeElementChanged();
		}
	}

	private void ComboBoxFontSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked && int.TryParse(comboBoxFontSize.Text, out var result))
		{
			int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
			richTextBox.BeginUpdate();
			((TextBoxBase)richTextBox).SelectAll();
			richTextBox.SetSelectionSize(result);
			((TextBoxBase)richTextBox).DeselectAll();
			((TextBoxBase)richTextBox).SelectionStart = selectionStart;
			richTextBox.EndUpdate();
			((Control)richTextBox).Invalidate();
			InvokeElementChanged();
		}
	}

	private void ComboBoxFontName_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			string text = ((Control)comboBoxFontName.ComboBoxEx).Text;
			int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
			richTextBox.BeginUpdate();
			((TextBoxBase)richTextBox).SelectAll();
			richTextBox.SetSelectionFont(text);
			((TextBoxBase)richTextBox).DeselectAll();
			((TextBoxBase)richTextBox).SelectionStart = selectionStart;
			richTextBox.EndUpdate();
			((Control)richTextBox).Invalidate();
			InvokeElementChanged();
		}
	}

	private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Invalid comparison between Unknown and I4
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Invalid comparison between Unknown and I4
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Invalid comparison between Unknown and I4
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Invalid comparison between Unknown and I4
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 66)
		{
			richTextBox.SetSelectionBold(!((RichTextBox)richTextBox).SelectionFont.Bold);
			RefreshElementsStatus();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 73)
		{
			richTextBox.SetSelectionItalic(!((RichTextBox)richTextBox).SelectionFont.Italic);
			RefreshElementsStatus();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 85)
		{
			richTextBox.SetSelectionUnderlined(!((RichTextBox)richTextBox).SelectionFont.Underline);
			RefreshElementsStatus();
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 76)
		{
			ButtonAlignLeft_Click(sender, (EventArgs)(object)e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 69)
		{
			ButtonAlignCenter_Click(sender, (EventArgs)(object)e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 82)
		{
			ButtonAlignRight_Click(sender, (EventArgs)(object)e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		if ((int)e.Modifiers == 131072 && (int)e.KeyCode == 74)
		{
			ButtonAlignJustify_Click(sender, (EventArgs)(object)e);
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
	}

	private void ButtonInsertConstant_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		buttonInsertConstant.SubItems.Clear();
		foreach (string ident in Idents)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = ident,
				Tag = ident
			};
			buttonItem.Click += delegate(object sender2, EventArgs e2)
			{
				ButtonItem buttonItem2 = sender2 as ButtonItem;
				string text = buttonItem2.Tag as string;
				bool flag = true;
				int selectionStart = ((TextBoxBase)richTextBox).SelectionStart;
				if (selectionStart >= 1 && selectionStart < ((Control)richTextBox).Text.Length)
				{
					int num = selectionStart;
					while (num >= 0 && ((Control)richTextBox).Text[num - 1] != '}')
					{
						if (((Control)richTextBox).Text[num - 1] == '{')
						{
							flag = false;
							break;
						}
						num--;
					}
				}
				if (flag)
				{
					text = "{" + text + "}";
				}
				Clipboard.SetText(text);
				((TextBoxBase)richTextBox).Paste();
			};
			buttonInsertConstant.SubItems.Add(buttonItem);
		}
	}

	private void ButtonInsertLink_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		StiInsertLinkForm stiInsertLinkForm = new StiInsertLinkForm(((TextBoxBase)richTextBox).SelectedText);
		try
		{
			if ((int)((Form)stiInsertLinkForm).ShowDialog((IWin32Window)(object)((Control)this).FindForm()) == 1)
			{
				((TextBoxBase)richTextBox).SelectedText = stiInsertLinkForm.Link;
			}
		}
		finally
		{
			((IDisposable)stiInsertLinkForm)?.Dispose();
		}
	}

	private void RichTextBox_DragOver(object sender, DragEventArgs e)
	{
		((TextBoxBase)richTextBox).SelectionStart = ((TextBoxBase)richTextBox).GetCharIndexFromPosition(((Control)this).PointToClient(Cursor.Position));
		((TextBoxBase)richTextBox).SelectionLength = 1;
	}

	private void RichTextBox_DragEnter(object sender, DragEventArgs e)
	{
		IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
		if (draggedDataColumnOrVariable != null)
		{
			e.Effect = (DragDropEffects)2;
		}
		else if (e.Data.GetDataPresent(DataFormats.Text))
		{
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			e.Effect = (DragDropEffects)0;
		}
	}

	private void RichTextBox_DragDrop(object sender, DragEventArgs e)
	{
		string selectedText = null;
		IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
		if (draggedDataColumnOrVariable != null)
		{
			if (draggedDataColumnOrVariable is StiDataColumn stiDataColumn)
			{
				selectedText = "{" + stiDataColumn.DataSource.Name + "." + stiDataColumn.Name + "}";
			}
			if (draggedDataColumnOrVariable is StiVariable stiVariable)
			{
				selectedText = "{" + stiVariable.Name + "}";
			}
		}
		else if (e.Data.GetDataPresent(DataFormats.Text))
		{
			selectedText = e.Data.GetData(DataFormats.Text).ToString();
		}
		((TextBoxBase)richTextBox).SelectedText = selectedText;
	}

	public void Localize()
	{
		buttonFontBold.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontBold"));
		buttonFontItalic.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontItalic"));
		buttonFontUnderline.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontUnderline"));
		buttonAlignLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		buttonAlignCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		buttonAlignRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		buttonAlignJustify.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth"));
		buttonClearAllFormatting.Tooltip = StiHint.CreateHint(Loc.Get("Dashboard", "ClearAllFormatting"));
		buttonInsertSymbol.Tooltip = StiHint.CreateHint(StiLocalization.Get("Editor", "InsertSymbol"));
		buttonInsertLink.Tooltip = StiHint.CreateHint(StiLocalization.Get("Editor", "InsertLink"));
		buttonFontColor.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Color"));
		buttonInsertConstant.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormRichTextEditor", "Insert"));
	}

	internal void DoScaling()
	{
		buttonInsertConstant.Image = StiReportDesignImages.Editor.Function();
		buttonFontColor.Image = StiReportDesignImages.Editor.FontColor();
		buttonFontBold.Image = StiReportDesignImages.Editor.FontStyleBold();
		buttonFontItalic.Image = StiReportDesignImages.Editor.FontStyleItalic();
		buttonFontUnderline.Image = StiReportDesignImages.Editor.FontStyleUnderline();
		buttonAlignLeft.Image = StiReportDesignImages.Editor.AlignLeft();
		buttonAlignCenter.Image = StiReportDesignImages.Editor.AlignCenter();
		buttonAlignRight.Image = StiReportDesignImages.Editor.AlignRight();
		buttonAlignJustify.Image = StiReportDesignImages.Editor.AlignJustify();
		buttonInsertSymbol.Image = StiReportDesignImages.Editor.Symbol();
		buttonClearAllFormatting.Image = StiReportDesignImages.Editor.ClearContent();
		buttonInsertLink.Image = StiReportDesignImages.Layout.Link();
		((ComboBox)comboBoxFontName.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		comboBoxFontName.DropDownWidth = StiScale.XXI(400.0);
		comboBoxFontName.DropDownHeight = StiScale.YYI(500.0);
		comboBoxFontName.ComboWidth = StiScale.XXI(127.0);
		((ComboBox)comboBoxFontSize.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		comboBoxFontSize.ComboWidth = StiScale.XXI(50.0);
		comboBoxFontSize.DropDownHeight = StiScale.XXI(400.0);
		buttonFontBold.ImagePaddingHorizontal = StiScale.I12;
		buttonFontItalic.ImagePaddingHorizontal = StiScale.I12;
		buttonFontUnderline.ImagePaddingHorizontal = StiScale.I12;
	}

	private void SaveSelection()
	{
		storedSelectionStart = ((TextBoxBase)richTextBox).SelectionStart;
		storedSelectionLength = ((TextBoxBase)richTextBox).SelectionLength;
	}

	private void RestoreSelection()
	{
		((TextBoxBase)richTextBox).Select(storedSelectionStart, storedSelectionLength);
	}

	private void InvokeElementChanged()
	{
		this.ElementChanged?.Invoke(this, EventArgs.Empty);
	}

	private void RefreshElementsStatus()
	{
		locked = true;
		if (Element is IStiTextHorAlignment stiTextHorAlignment)
		{
			buttonAlignLeft.Checked = stiTextHorAlignment.HorAlignment == StiTextHorAlignment.Left;
			buttonAlignCenter.Checked = stiTextHorAlignment.HorAlignment == StiTextHorAlignment.Center;
			buttonAlignRight.Checked = stiTextHorAlignment.HorAlignment == StiTextHorAlignment.Right;
			buttonAlignJustify.Checked = stiTextHorAlignment.HorAlignment == StiTextHorAlignment.Width;
		}
		else
		{
			buttonAlignLeft.Checked = richTextBox.SelectionAlignment == StiRichTextAlignment.Left;
			buttonAlignCenter.Checked = richTextBox.SelectionAlignment == StiRichTextAlignment.Center;
			buttonAlignRight.Checked = richTextBox.SelectionAlignment == StiRichTextAlignment.Right;
			buttonAlignJustify.Checked = richTextBox.SelectionAlignment == StiRichTextAlignment.Justify;
		}
		if (Element is IStiFont stiFont)
		{
			comboBoxFontName.Text = stiFont.Font.FontFamily.Name;
			comboBoxFontSize.Text = ((int)stiFont.Font.Size).ToString();
		}
		else if (((RichTextBox)richTextBox).SelectionFont != null)
		{
			comboBoxFontName.Text = ((RichTextBox)richTextBox).SelectionFont.FontFamily.Name;
			comboBoxFontSize.Text = ((int)((RichTextBox)richTextBox).SelectionFont.Size).ToString();
		}
		if (((RichTextBox)richTextBox).SelectionFont != null)
		{
			buttonFontBold.Checked = ((RichTextBox)richTextBox).SelectionFont.Bold;
			buttonFontItalic.Checked = ((RichTextBox)richTextBox).SelectionFont.Italic;
			buttonFontUnderline.Checked = ((RichTextBox)richTextBox).SelectionFont.Underline;
		}
		buttonFontColor.SelectedColor = ((RichTextBox)richTextBox).SelectionColor;
		locked = false;
	}

	private void SetHtmlText(string htmlText)
	{
		locked = true;
		richTextBox.BeginUpdate();
		new StiHtmlTextHelper().SetHtmlText(htmlText, richTextBox, StiDashboardStyleHelper.GetForeColor(Element));
		richTextBox.EndUpdate();
		((Control)richTextBox).Invalidate();
		RefreshElementsStatus();
		locked = false;
	}

	private string GetHtmlText()
	{
		SaveSelection();
		locked = true;
		richTextBox.BeginUpdate();
		string htmlText = new StiHtmlTextHelper().GetHtmlText(richTextBox, StiDashboardStyleHelper.GetForeColor(Element));
		RestoreSelection();
		richTextBox.EndUpdate();
		((Control)richTextBox).Invalidate();
		locked = false;
		return htmlText;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		if (disposing && components != null)
		{
			try
			{
				((Control)comboBoxFontSize.ComboBoxEx).KeyDown -= new KeyEventHandler(ComboBoxFontSize_KeyDown);
				((ComboBox)comboBoxFontName.ComboBoxEx).SelectedIndexChanged -= ComboBoxFontName_SelectedIndexChanged;
				((RichTextBox)richTextBox).DragDrop -= new DragEventHandler(RichTextBox_DragDrop);
				((RichTextBox)richTextBox).DragEnter -= new DragEventHandler(RichTextBox_DragEnter);
				((RichTextBox)richTextBox).DragOver -= new DragEventHandler(RichTextBox_DragOver);
			}
			catch
			{
			}
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b8: Expected O, but got Unknown
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5f: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiHtmlTextEditorControl));
		barElements = new Bar();
		comboBoxFontName = new ComboBoxItem();
		comboBoxFontSize = new ComboBoxItem();
		buttonFontBold = new ButtonItem();
		buttonFontItalic = new ButtonItem();
		buttonFontUnderline = new ButtonItem();
		buttonFontColor = new ColorPickerDropDown();
		buttonAlignLeft = new ButtonItem();
		buttonAlignCenter = new ButtonItem();
		buttonAlignRight = new ButtonItem();
		buttonAlignJustify = new ButtonItem();
		buttonInsertSymbol = new ButtonItem();
		buttonClearAllFormatting = new ButtonItem();
		buttonInsertConstant = new ButtonItem();
		buttonItemTemp = new ButtonItem();
		richTextBox = new StiRichBoxControl();
		topLine = new StiGroupLine();
		buttonInsertLink = new ButtonItem();
		((ISupportInitialize)barElements).BeginInit();
		((Control)this).SuspendLayout();
		((Control)barElements).AccessibleDescription = "stiToolBar4 (barElements)";
		((Control)barElements).AccessibleName = "stiToolBar4";
		((Control)barElements).AccessibleRole = (AccessibleRole)22;
		barElements.BarType = eBarType.MenuBar;
		((Control)barElements).Dock = (DockStyle)1;
		barElements.DockTabStripHeight = 50;
		((Control)barElements).Font = new Font("Segoe UI", 9f);
		barElements.Items.AddRange(new BaseItem[14]
		{
			comboBoxFontName, comboBoxFontSize, buttonFontBold, buttonFontItalic, buttonFontUnderline, buttonFontColor, buttonAlignLeft, buttonAlignCenter, buttonAlignRight, buttonAlignJustify,
			buttonInsertSymbol, buttonClearAllFormatting, buttonInsertConstant, buttonInsertLink
		});
		barElements.Location = new Point(0, 0);
		barElements.Name = "barElements";
		barElements.RoundCorners = false;
		barElements.SingleLineColor = Color.Empty;
		barElements.Size = new Size(695, 31);
		barElements.Stretch = true;
		barElements.Style = eDotNetBarStyle.StyleManagerControlled;
		barElements.TabIndex = 10;
		barElements.TabStop = false;
		((Control)barElements).Text = "stiToolBar4";
		comboBoxFontName.BeginGroup = true;
		comboBoxFontName.ComboWidth = 130;
		comboBoxFontName.DropDownHeight = 500;
		comboBoxFontName.DropDownWidth = 242;
		comboBoxFontName.FontCombo = true;
		comboBoxFontName.ItemHeight = 16;
		comboBoxFontName.Name = "comboBoxFontName";
		comboBoxFontSize.DropDownHeight = 200;
		comboBoxFontSize.DropDownStyle = (ComboBoxStyle)1;
		comboBoxFontSize.ItemHeight = 17;
		comboBoxFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "76"
		});
		comboBoxFontSize.Name = "comboBoxFontSize";
		comboBoxFontSize.SelectedIndexChanged += ComboBoxFontSize_SelectedIndexChanged;
		buttonFontBold.AutoCheckOnClick = true;
		buttonFontBold.BeginGroup = true;
		buttonFontBold.Image = (Image)componentResourceManager.GetObject("buttonFontBold.Image");
		buttonFontBold.ImagePaddingHorizontal = 16;
		buttonFontBold.ImagePaddingVertical = 12;
		buttonFontBold.Name = "buttonFontBold";
		buttonFontBold.Click += ButtonFontBold_Click;
		buttonFontItalic.AutoCheckOnClick = true;
		buttonFontItalic.Image = (Image)componentResourceManager.GetObject("buttonFontItalic.Image");
		buttonFontItalic.ImagePaddingHorizontal = 16;
		buttonFontItalic.ImagePaddingVertical = 12;
		buttonFontItalic.Name = "buttonFontItalic";
		buttonFontItalic.Click += ButtonFontItalic_Click;
		buttonFontUnderline.AutoCheckOnClick = true;
		buttonFontUnderline.Image = (Image)componentResourceManager.GetObject("buttonFontUnderline.Image");
		buttonFontUnderline.ImagePaddingHorizontal = 16;
		buttonFontUnderline.ImagePaddingVertical = 12;
		buttonFontUnderline.Name = "buttonFontUnderline";
		buttonFontUnderline.Click += ButtonFontUnderline_Click;
		buttonFontColor.AutoExpandOnClick = true;
		buttonFontColor.BeginGroup = true;
		buttonFontColor.Image = (Image)componentResourceManager.GetObject("buttonFontColor.Image");
		buttonFontColor.ImagePaddingHorizontal = 16;
		buttonFontColor.ImagePaddingVertical = 12;
		buttonFontColor.Name = "buttonFontColor";
		buttonFontColor.SelectedColorChanged += ButtonFontColor_SelectedColorChanged;
		buttonAlignLeft.AutoCheckOnClick = true;
		buttonAlignLeft.BeginGroup = true;
		buttonAlignLeft.Image = (Image)componentResourceManager.GetObject("buttonAlignLeft.Image");
		buttonAlignLeft.ImagePaddingHorizontal = 16;
		buttonAlignLeft.ImagePaddingVertical = 12;
		buttonAlignLeft.Name = "buttonAlignLeft";
		buttonAlignLeft.Click += ButtonAlignLeft_Click;
		buttonAlignCenter.AutoCheckOnClick = true;
		buttonAlignCenter.Image = (Image)componentResourceManager.GetObject("buttonAlignCenter.Image");
		buttonAlignCenter.ImagePaddingHorizontal = 16;
		buttonAlignCenter.ImagePaddingVertical = 12;
		buttonAlignCenter.Name = "buttonAlignCenter";
		buttonAlignCenter.Click += ButtonAlignCenter_Click;
		buttonAlignRight.AutoCheckOnClick = true;
		buttonAlignRight.Image = (Image)componentResourceManager.GetObject("buttonAlignRight.Image");
		buttonAlignRight.ImagePaddingHorizontal = 16;
		buttonAlignRight.ImagePaddingVertical = 12;
		buttonAlignRight.Name = "buttonAlignRight";
		buttonAlignRight.Click += ButtonAlignRight_Click;
		buttonAlignJustify.AutoCheckOnClick = true;
		buttonAlignJustify.Image = (Image)componentResourceManager.GetObject("buttonAlignJustify.Image");
		buttonAlignJustify.ImagePaddingHorizontal = 16;
		buttonAlignJustify.ImagePaddingVertical = 12;
		buttonAlignJustify.Name = "buttonAlignJustify";
		buttonAlignJustify.Click += ButtonAlignJustify_Click;
		buttonInsertSymbol.BeginGroup = true;
		buttonInsertSymbol.ImagePaddingHorizontal = 16;
		buttonInsertSymbol.ImagePaddingVertical = 12;
		buttonInsertSymbol.Name = "buttonInsertSymbol";
		buttonInsertSymbol.Click += ButtonInsertSymbol_Click;
		buttonClearAllFormatting.BeginGroup = true;
		buttonClearAllFormatting.Image = (Image)componentResourceManager.GetObject("buttonClearAllFormatting.Image");
		buttonClearAllFormatting.ImagePaddingHorizontal = 16;
		buttonClearAllFormatting.ImagePaddingVertical = 12;
		buttonClearAllFormatting.Name = "buttonClearAllFormatting";
		buttonClearAllFormatting.Click += ButtonClearAllFormatting_Click;
		buttonInsertConstant.AutoExpandOnClick = true;
		buttonInsertConstant.BeginGroup = true;
		buttonInsertConstant.Image = (Image)componentResourceManager.GetObject("buttonInsertConstant.Image");
		buttonInsertConstant.ImagePaddingHorizontal = 16;
		buttonInsertConstant.ImagePaddingVertical = 12;
		buttonInsertConstant.Name = "buttonInsertConstant";
		buttonInsertConstant.SubItems.AddRange(new BaseItem[1] { buttonItemTemp });
		buttonInsertConstant.Visible = false;
		buttonInsertConstant.PopupOpen += ButtonInsertConstant_PopupOpen;
		buttonItemTemp.ImagePaddingHorizontal = 16;
		buttonItemTemp.ImagePaddingVertical = 12;
		buttonItemTemp.Name = "buttonItemTemp";
		buttonItemTemp.Text = "buttonItem1";
		((TextBoxBase)richTextBox).AcceptsTab = true;
		((TextBoxBase)richTextBox).BorderStyle = (BorderStyle)0;
		((Control)richTextBox).Dock = (DockStyle)5;
		((TextBoxBase)richTextBox).HideSelection = false;
		((Control)richTextBox).Location = new Point(0, 43);
		((Control)richTextBox).Margin = new Padding(0);
		((Control)richTextBox).Name = "richTextBox";
		richTextBox.SelectionAlignment = StiRichTextAlignment.Left;
		((Control)richTextBox).Size = new Size(695, 286);
		((Control)richTextBox).TabIndex = 17;
		((Control)richTextBox).Text = "";
		((RichTextBox)richTextBox).SelectionChanged += RichTextBox_SelectionChanged;
		((Control)richTextBox).KeyDown += new KeyEventHandler(RichTextBox_KeyDown);
		((Control)topLine).BackColor = Color.White;
		((Control)topLine).Dock = (DockStyle)1;
		topLine.LinePosition = StiLinePosition.Center;
		topLine.LineStyle = StiLineStyle.Dot;
		((Control)topLine).Location = new Point(0, 31);
		((Control)topLine).Margin = new Padding(2);
		((Control)topLine).Name = "topLine";
		((Control)topLine).Size = new Size(695, 12);
		((Control)topLine).TabIndex = 70;
		buttonInsertLink.Image = (Image)componentResourceManager.GetObject("buttonInsertHyperlink.Image");
		buttonInsertLink.Name = "buttonInsertHyperlink";
		buttonInsertLink.Text = "buttonInsertHyperlink";
		buttonInsertLink.Click += ButtonInsertLink_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)richTextBox);
		((Control)this).Controls.Add((Control)(object)topLine);
		((Control)this).Controls.Add((Control)(object)barElements);
		((Control)this).Name = "StiHtmlTextEditorControl";
		((Control)this).Size = new Size(695, 329);
		((ISupportInitialize)barElements).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
