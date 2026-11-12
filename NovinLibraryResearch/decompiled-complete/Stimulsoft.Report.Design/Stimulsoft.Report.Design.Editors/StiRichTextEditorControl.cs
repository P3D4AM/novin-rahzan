using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Wizards;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Editors;

[ToolboxItem(false)]
public class StiRichTextEditorControl : UserControl
{
	private bool locked;

	private StiReport report;

	private IContainer components;

	private Bar tlbBorder;

	internal ButtonItem tbOpen;

	internal ButtonItem tbSave;

	internal ButtonItem tbInsert;

	private ControlContainerItem controlContainerItem1;

	private StiDictionaryControl dictionaryControl;

	private ButtonItem tbUndo;

	private ButtonItem tbRedo;

	private ButtonItem tbFontSelect;

	private ComboBoxItem fbFont;

	private ComboBoxItem cbFontSize;

	private ButtonItem tbBold;

	private ButtonItem tbItalic;

	private ButtonItem tbUnderline;

	private ButtonItem tbSubscript;

	private ButtonItem tbSuperscript;

	private ColorPickerDropDown tbColor;

	private ButtonItem tbLeft;

	private ButtonItem tbCenter;

	private ButtonItem tbRight;

	private ButtonItem tbJustify;

	private ButtonItem tbInsertSymbol;

	private ButtonItem tbBullets;

	internal StiRichTextBox rtbCodeEditor;

	private StiGroupPanelUX stiGroupLine1;

	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			report = value;
			if (report != null)
			{
				dictionaryControl.NodesMemory = report.Designer.NodesMemory;
				((TreeView)dictionaryControl.tvData).BorderStyle = (BorderStyle)0;
				dictionaryControl.Build(report);
				((Control)dictionaryControl.tvData).DoubleClick += DictControl_DoubleClick;
			}
			else
			{
				((Control)dictionaryControl).Visible = false;
			}
		}
	}

	public string TextValue
	{
		get
		{
			return ((RichTextBox)rtbCodeEditor).Rtf;
		}
		set
		{
			try
			{
				if (StiRtfHelper.IsRtfText(value))
				{
					((RichTextBox)rtbCodeEditor).Rtf = value;
				}
				else
				{
					((Control)rtbCodeEditor).Text = value;
				}
			}
			catch
			{
			}
			RefreshParam();
		}
	}

	public Bar Toolbar => tlbBorder;

	public StiRichTextEditorControl()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			DoTheming();
			tbInsertSymbol.Visible = false;
			((Control)cbFontSize.ComboBoxEx).KeyDown += new KeyEventHandler(ComboBoxFontSize_KeyDown);
			((ComboBox)fbFont.ComboBoxEx).SelectedIndexChanged += FontBoxFont_SelectedIndexChanged;
			tbColor.SelectedColorImageRectangle = new Rectangle(0, 13, 15, 3);
		}
	}

	private void DictControl_DoubleClick(object sender, EventArgs e)
	{
		string textFromDictionaryTree = StiCodeEditor.GetTextFromDictionaryTree(dictionaryControl, null);
		if (textFromDictionaryTree != null)
		{
			((TextBoxBase)rtbCodeEditor).SelectedText = "{" + textFromDictionaryTree + "}";
		}
		tbInsert.ClosePopup();
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "RtfFiles");
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("Buttons", "Open"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
				{
					((RichTextBox)rtbCodeEditor).LoadFile(((FileDialog)val).FileName);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "RtfFiles");
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("Buttons", "Save"));
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).DefaultExt = "rtf";
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
				{
					((RichTextBox)rtbCodeEditor).SaveFile(((FileDialog)val).FileName);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonBold_Click(object sender, EventArgs e)
	{
		rtbCodeEditor.SetSelectionBold(tbBold.Checked);
	}

	private void RichTextBoxCodeEditor_SelectionChanged(object sender, EventArgs e)
	{
		RefreshParam();
	}

	private void ButtonLeft_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SelectionAlignment = StiRtfSelectionAlign.Left;
			RefreshParam();
		}
	}

	private void ButtonCenter_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SelectionAlignment = StiRtfSelectionAlign.Center;
			RefreshParam();
		}
	}

	private void ButtonRight_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SelectionAlignment = StiRtfSelectionAlign.Right;
			RefreshParam();
		}
	}

	private void ButtonJustify_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SelectionAlignment = StiRtfSelectionAlign.Justify;
			RefreshParam();
		}
	}

	private void ButtonItalic_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SetSelectionItalic(tbItalic.Checked);
		}
	}

	private void ButtonUnderline_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SetSelectionUnderlined(tbUnderline.Checked);
		}
	}

	private void ButtonFontSelect_Click(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(((RichTextBox)rtbCodeEditor).SelectionFont, Report);
		try
		{
			if (((RichTextBox)rtbCodeEditor).SelectionFont != null)
			{
				((Control)stiFontEditorForm).Font = ((RichTextBox)rtbCodeEditor).SelectionFont;
			}
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				((RichTextBox)rtbCodeEditor).SelectionFont = stiFontEditorForm.SelectedFont;
				RefreshParam();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void ButtonUndo_Click(object sender, EventArgs e)
	{
		((TextBoxBase)rtbCodeEditor).Undo();
	}

	private void ButtonInsertSymbol_Click(object sender, EventArgs e)
	{
		string fontFamily = (string.IsNullOrEmpty(((Control)fbFont.ComboBoxEx).Text) ? ((Control)fbFont.ComboBoxEx).Text : "Arial");
		IStiCharterMapWindow stiCharterMapWindow = StiCharterMapRunner.StiWindow(fontFamily);
		if (stiCharterMapWindow == null)
		{
			return;
		}
		IntPtr intPtr = ((Form.ActiveForm != null) ? ((Control)Form.ActiveForm).Handle : IntPtr.Zero);
		if (stiCharterMapWindow.ShowDialog(intPtr) == true)
		{
			string result = stiCharterMapWindow.Result;
			if (!string.IsNullOrEmpty(result))
			{
				Clipboard.SetText(result);
				((TextBoxBase)rtbCodeEditor).Paste();
			}
		}
	}

	private void ButtonBullets_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			((RichTextBox)rtbCodeEditor).SelectionBullet = true;
			RefreshParam();
		}
	}

	private void ButtonRedo_Click(object sender, EventArgs e)
	{
		((RichTextBox)rtbCodeEditor).Redo();
	}

	private void ButtonColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((RichTextBox)rtbCodeEditor).SelectionColor = tbColor.SelectedColor;
			RefreshParam();
		}
	}

	private void ButtonSubscript_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			((RichTextBox)rtbCodeEditor).SelectionCharOffset = (tbSubscript.Checked ? (-4) : 0);
			RefreshParam();
		}
	}

	private void ButtonSuperscript_Click(object sender, EventArgs e)
	{
		if (!locked)
		{
			((RichTextBox)rtbCodeEditor).SelectionCharOffset = (tbSuperscript.Checked ? 4 : 0);
			RefreshParam();
		}
	}

	private void ComboBoxFontSize_EnabledChanged(object sender, EventArgs e)
	{
		((ComboBox)cbFontSize.ComboBoxEx).DropDownStyle = (ComboBoxStyle)(cbFontSize.Enabled ? 1 : 2);
	}

	private void ComboBoxFontSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			ComboBoxFontSize_SelectedIndexChanged(sender, EventArgs.Empty);
		}
	}

	private void ComboBoxFontSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SetSelectionSize(int.Parse(cbFontSize.Text));
		}
	}

	private void FontBoxFont_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			rtbCodeEditor.SetSelectionFont(((Control)fbFont.ComboBoxEx).Text);
		}
	}

	private void Localize()
	{
		tbOpen.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Open"));
		tbOpen.Text = StiLocalization.Get("Buttons", "Open");
		tbSave.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Save"));
		tbSave.Text = StiLocalization.Get("Buttons", "Save");
		tbBold.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontBold"));
		tbItalic.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontItalic"));
		tbUnderline.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontUnderline"));
		tbSubscript.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontSubscript"));
		tbSuperscript.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "FontSuperscript"));
		tbUndo.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditUndo"));
		tbRedo.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditRedo"));
		tbLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		tbCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		tbRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		tbJustify.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth"));
		tbInsertSymbol.Tooltip = StiHint.CreateHint(StiLocalization.Get("Editor", "InsertSymbol"));
		tbBullets.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormRichTextEditor", "Bullets"));
		tbFontSelect.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Font"));
		tbInsert.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Insert"));
		tbColor.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Color"));
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)rtbCodeEditor).BackColor = StiColor.Get("#888888");
			stiGroupLine1.LinePosition = StiLinePosition.None;
		}
	}

	internal void DoScaling()
	{
		((Control)dictionaryControl).Width = StiScale.XXI(300.0);
		((Control)dictionaryControl).Height = StiScale.YYI(500.0);
		tbOpen.Image = StiReportDesignImages.Actions.OpenFile();
		tbSave.Image = StiReportDesignImages.Actions.SaveFile();
		tbInsert.Image = StiReportDesignImages.Editor.Function();
		tbUndo.Image = StiReportDesignImages.Editor.Undo();
		tbRedo.Image = StiReportDesignImages.Editor.Redo();
		tbFontSelect.Image = StiReportDesignImages.Editor.Font();
		tbBold.Image = StiReportDesignImages.Editor.FontStyleBold();
		tbItalic.Image = StiReportDesignImages.Editor.FontStyleItalic();
		tbUnderline.Image = StiReportDesignImages.Editor.FontStyleUnderline();
		tbSubscript.Image = StiReportDesignImages.Editor.FontSubscript();
		tbSuperscript.Image = StiReportDesignImages.Editor.FontSuperscript();
		tbInsertSymbol.Image = StiReportDesignImages.Editor.Symbol();
		tbBullets.Image = StiReportDesignImages.Editor.Bullets();
		tbLeft.Image = StiReportDesignImages.Editor.AlignLeft();
		tbCenter.Image = StiReportDesignImages.Editor.AlignCenter();
		tbRight.Image = StiReportDesignImages.Editor.AlignRight();
		tbJustify.Image = StiReportDesignImages.Editor.AlignJustify();
		tbColor.Image = StiReportDesignImages.Editor.FontColor();
		((ComboBox)fbFont.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		fbFont.DropDownWidth = StiScale.XXI(400.0);
		fbFont.DropDownHeight = StiScale.YYI(500.0);
		fbFont.ComboWidth = StiScale.XXI(127.0);
		((ComboBox)cbFontSize.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		cbFontSize.ComboWidth = StiScale.XXI(50.0);
		cbFontSize.DropDownHeight = StiScale.XXI(400.0);
		tbColor.SelectedColorImageRectangle = new Rectangle(StiScale.I1, StiScale.I14, StiScale.XXI(15.0), StiScale.I3);
		tbUndo.ImagePaddingHorizontal = StiScale.I12;
		tbRedo.ImagePaddingHorizontal = StiScale.I12;
		tbBold.ImagePaddingHorizontal = StiScale.I12;
		tbItalic.ImagePaddingHorizontal = StiScale.I12;
		tbUnderline.ImagePaddingHorizontal = StiScale.I12;
		tbSuperscript.ImagePaddingHorizontal = StiScale.I12;
		tbSubscript.ImagePaddingHorizontal = StiScale.I12;
	}

	private void RefreshParam()
	{
		locked = true;
		tbLeft.Checked = rtbCodeEditor.SelectionAlignment == StiRtfSelectionAlign.Left;
		tbCenter.Checked = rtbCodeEditor.SelectionAlignment == StiRtfSelectionAlign.Center;
		tbRight.Checked = rtbCodeEditor.SelectionAlignment == StiRtfSelectionAlign.Right;
		tbJustify.Checked = rtbCodeEditor.SelectionAlignment == StiRtfSelectionAlign.Justify;
		tbSubscript.Checked = ((RichTextBox)rtbCodeEditor).SelectionCharOffset < 0;
		tbSuperscript.Checked = ((RichTextBox)rtbCodeEditor).SelectionCharOffset > 0;
		if (((RichTextBox)rtbCodeEditor).SelectionFont != null)
		{
			tbBold.Checked = ((RichTextBox)rtbCodeEditor).SelectionFont.Bold;
			tbItalic.Checked = ((RichTextBox)rtbCodeEditor).SelectionFont.Italic;
			tbUnderline.Checked = ((RichTextBox)rtbCodeEditor).SelectionFont.Underline;
			fbFont.Text = ((RichTextBox)rtbCodeEditor).SelectionFont.FontFamily.Name;
			cbFontSize.Text = ((int)((RichTextBox)rtbCodeEditor).SelectionFont.Size).ToString();
		}
		tbColor.SelectedColor = ((RichTextBox)rtbCodeEditor).SelectionColor;
		tbBullets.Checked = ((RichTextBox)rtbCodeEditor).SelectionBullet;
		locked = false;
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
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRichTextEditorControl));
		tlbBorder = new Bar();
		tbOpen = new ButtonItem();
		tbSave = new ButtonItem();
		tbInsert = new ButtonItem();
		controlContainerItem1 = new ControlContainerItem();
		dictionaryControl = new StiDictionaryControl();
		tbUndo = new ButtonItem();
		tbRedo = new ButtonItem();
		tbFontSelect = new ButtonItem();
		fbFont = new ComboBoxItem();
		cbFontSize = new ComboBoxItem();
		tbBold = new ButtonItem();
		tbItalic = new ButtonItem();
		tbUnderline = new ButtonItem();
		tbSubscript = new ButtonItem();
		tbSuperscript = new ButtonItem();
		tbColor = new ColorPickerDropDown();
		tbLeft = new ButtonItem();
		tbCenter = new ButtonItem();
		tbRight = new ButtonItem();
		tbJustify = new ButtonItem();
		tbInsertSymbol = new ButtonItem();
		tbBullets = new ButtonItem();
		rtbCodeEditor = new StiRichTextBox();
		stiGroupLine1 = new StiGroupPanelUX();
		((ISupportInitialize)tlbBorder).BeginInit();
		((Control)this).SuspendLayout();
		((Control)tlbBorder).AccessibleDescription = "stiToolBar4 (tlbBorder)";
		((Control)tlbBorder).AccessibleName = "stiToolBar4";
		((Control)tlbBorder).AccessibleRole = (AccessibleRole)22;
		tlbBorder.BarType = eBarType.MenuBar;
		((Control)tlbBorder).Dock = (DockStyle)1;
		tlbBorder.DockTabStripHeight = 32;
		((Control)tlbBorder).Font = new Font("Segoe UI", 9f);
		tlbBorder.Items.AddRange(new BaseItem[20]
		{
			tbOpen, tbSave, tbInsert, tbUndo, tbRedo, tbFontSelect, fbFont, cbFontSize, tbBold, tbItalic,
			tbUnderline, tbSubscript, tbSuperscript, tbColor, tbLeft, tbCenter, tbRight, tbJustify, tbInsertSymbol, tbBullets
		});
		tlbBorder.Location = new Point(0, 0);
		((Control)tlbBorder).MinimumSize = new Size(0, 30);
		tlbBorder.Name = "tlbBorder";
		tlbBorder.PaddingLeft = 5;
		tlbBorder.PaddingTop = 5;
		tlbBorder.RoundCorners = false;
		tlbBorder.SingleLineColor = Color.Empty;
		tlbBorder.Size = new Size(713, 32);
		tlbBorder.Stretch = true;
		tlbBorder.Style = eDotNetBarStyle.StyleManagerControlled;
		tlbBorder.TabIndex = 10;
		tlbBorder.TabStop = false;
		((Control)tlbBorder).Text = "stiToolBar4";
		tbOpen.ButtonStyle = eButtonStyle.ImageAndText;
		tbOpen.ForeColor = SystemColors.ControlText;
		tbOpen.Image = (Image)componentResourceManager.GetObject("tbOpen.Image");
		tbOpen.ImagePaddingHorizontal = 10;
		tbOpen.ImagePaddingVertical = 8;
		tbOpen.Name = "tbOpen";
		tbOpen.Click += ButtonOpen_Click;
		tbSave.ButtonStyle = eButtonStyle.ImageAndText;
		tbSave.ForeColor = SystemColors.ControlText;
		tbSave.Image = (Image)componentResourceManager.GetObject("tbSave.Image");
		tbSave.ImagePaddingHorizontal = 10;
		tbSave.ImagePaddingVertical = 8;
		tbSave.Name = "tbSave";
		tbSave.Click += ButtonSave_Click;
		tbInsert.AutoExpandOnClick = true;
		tbInsert.BeginGroup = true;
		tbInsert.Image = (Image)componentResourceManager.GetObject("tbInsert.Image");
		tbInsert.ImagePaddingHorizontal = 10;
		tbInsert.ImagePaddingVertical = 8;
		tbInsert.Name = "tbInsert";
		tbInsert.SubItems.AddRange(new BaseItem[1] { controlContainerItem1 });
		controlContainerItem1.AllowItemResize = true;
		controlContainerItem1.Control = (Control)(object)dictionaryControl;
		controlContainerItem1.MenuVisibility = eMenuVisibility.VisibleAlways;
		controlContainerItem1.Name = "controlContainerItem1";
		controlContainerItem1.Text = "controlContainerItem1";
		dictionaryControl.AddParentheses = true;
		dictionaryControl.CanBuildDatabase = false;
		dictionaryControl.CanBuildFormats = true;
		dictionaryControl.CanBuildHtmlTags = false;
		dictionaryControl.CanBuildParameters = false;
		dictionaryControl.CanBuildTotals = true;
		dictionaryControl.CanDragDrop = false;
		dictionaryControl.CanDragDropBusinessObjectColumn = true;
		dictionaryControl.CanDragDropDataColumn = true;
		dictionaryControl.CanDragDropDataParameter = true;
		dictionaryControl.CanDragDropDataSource = true;
		dictionaryControl.CanDragDropFunction = true;
		dictionaryControl.CanDragDropHtmlTags = true;
		dictionaryControl.CanDragDropProperty = true;
		dictionaryControl.CanDragDropResource = true;
		dictionaryControl.CanDragDropTextFormat = true;
		dictionaryControl.CanDragDropTotals = true;
		dictionaryControl.CanDragDropVariable = true;
		dictionaryControl.IsDataExpression = false;
		((Control)dictionaryControl).Location = new Point(281, 107);
		((Control)dictionaryControl).Margin = new Padding(4);
		((Control)dictionaryControl).Name = "dictionaryControl";
		dictionaryControl.SimpleDragDrop = false;
		((Control)dictionaryControl).Size = new Size(250, 400);
		((Control)dictionaryControl).TabIndex = 17;
		tbUndo.Image = (Image)componentResourceManager.GetObject("tbUndo.Image");
		tbUndo.ImagePaddingHorizontal = 10;
		tbUndo.ImagePaddingVertical = 8;
		tbUndo.Name = "tbUndo";
		tbUndo.Click += ButtonUndo_Click;
		tbRedo.Image = (Image)componentResourceManager.GetObject("tbRedo.Image");
		tbRedo.ImagePaddingHorizontal = 10;
		tbRedo.ImagePaddingVertical = 8;
		tbRedo.Name = "tbRedo";
		tbRedo.Click += ButtonRedo_Click;
		tbFontSelect.ImagePaddingHorizontal = 10;
		tbFontSelect.ImagePaddingVertical = 8;
		tbFontSelect.Name = "tbFontSelect";
		tbFontSelect.Click += ButtonFontSelect_Click;
		fbFont.BeginGroup = true;
		fbFont.ComboWidth = 130;
		fbFont.DropDownHeight = 500;
		fbFont.DropDownWidth = 242;
		fbFont.FontCombo = true;
		fbFont.ItemHeight = 16;
		fbFont.Name = "fbFont";
		cbFontSize.DropDownHeight = 200;
		cbFontSize.ItemHeight = 17;
		cbFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "76"
		});
		cbFontSize.Name = "cbFontSize";
		cbFontSize.SelectedIndexChanged += ComboBoxFontSize_SelectedIndexChanged;
		cbFontSize.EnabledChanged += ComboBoxFontSize_EnabledChanged;
		tbBold.AutoCheckOnClick = true;
		tbBold.BeginGroup = true;
		tbBold.Image = (Image)componentResourceManager.GetObject("tbBold.Image");
		tbBold.ImagePaddingHorizontal = 10;
		tbBold.ImagePaddingVertical = 8;
		tbBold.Name = "tbBold";
		tbBold.Click += ButtonBold_Click;
		tbItalic.AutoCheckOnClick = true;
		tbItalic.Image = (Image)componentResourceManager.GetObject("tbItalic.Image");
		tbItalic.ImagePaddingHorizontal = 10;
		tbItalic.ImagePaddingVertical = 8;
		tbItalic.Name = "tbItalic";
		tbItalic.Click += ButtonItalic_Click;
		tbUnderline.AutoCheckOnClick = true;
		tbUnderline.Image = (Image)componentResourceManager.GetObject("tbUnderline.Image");
		tbUnderline.ImagePaddingHorizontal = 10;
		tbUnderline.ImagePaddingVertical = 8;
		tbUnderline.Name = "tbUnderline";
		tbUnderline.Click += ButtonUnderline_Click;
		tbSubscript.AutoCheckOnClick = true;
		tbSubscript.ImagePaddingHorizontal = 10;
		tbSubscript.ImagePaddingVertical = 8;
		tbSubscript.Name = "tbSubscript";
		tbSubscript.Click += ButtonSubscript_Click;
		tbSuperscript.AutoCheckOnClick = true;
		tbSuperscript.ImagePaddingHorizontal = 10;
		tbSuperscript.ImagePaddingVertical = 8;
		tbSuperscript.Name = "tbSuperscript";
		tbSuperscript.Click += ButtonSuperscript_Click;
		tbColor.AutoExpandOnClick = true;
		tbColor.BeginGroup = true;
		tbColor.ImagePaddingHorizontal = 10;
		tbColor.ImagePaddingVertical = 8;
		tbColor.Name = "tbColor";
		tbColor.SelectedColorChanged += ButtonColor_SelectedColorChanged;
		tbLeft.AutoCheckOnClick = true;
		tbLeft.BeginGroup = true;
		tbLeft.ImagePaddingHorizontal = 10;
		tbLeft.ImagePaddingVertical = 8;
		tbLeft.Name = "tbLeft";
		tbLeft.Click += ButtonLeft_Click;
		tbCenter.AutoCheckOnClick = true;
		tbCenter.ImagePaddingHorizontal = 10;
		tbCenter.ImagePaddingVertical = 8;
		tbCenter.Name = "tbCenter";
		tbCenter.Click += ButtonCenter_Click;
		tbRight.AutoCheckOnClick = true;
		tbRight.ImagePaddingHorizontal = 10;
		tbRight.ImagePaddingVertical = 8;
		tbRight.Name = "tbRight";
		tbRight.Click += ButtonRight_Click;
		tbJustify.AutoCheckOnClick = true;
		tbJustify.ImagePaddingHorizontal = 10;
		tbJustify.ImagePaddingVertical = 8;
		tbJustify.Name = "tbJustify";
		tbJustify.Click += ButtonJustify_Click;
		tbInsertSymbol.BeginGroup = true;
		tbInsertSymbol.ImagePaddingHorizontal = 10;
		tbInsertSymbol.ImagePaddingVertical = 8;
		tbInsertSymbol.Name = "tbInsertSymbol";
		tbInsertSymbol.Click += ButtonInsertSymbol_Click;
		tbBullets.BeginGroup = true;
		tbBullets.ImagePaddingHorizontal = 10;
		tbBullets.ImagePaddingVertical = 8;
		tbBullets.Name = "tbBullets";
		tbBullets.Click += ButtonBullets_Click;
		((TextBoxBase)rtbCodeEditor).AcceptsTab = true;
		((TextBoxBase)rtbCodeEditor).BorderStyle = (BorderStyle)0;
		((Control)rtbCodeEditor).Dock = (DockStyle)5;
		((TextBoxBase)rtbCodeEditor).HideSelection = false;
		((Control)rtbCodeEditor).Location = new Point(0, 38);
		((Control)rtbCodeEditor).Name = "rtbCodeEditor";
		rtbCodeEditor.SelectionAlignment = StiRtfSelectionAlign.Left;
		((Control)rtbCodeEditor).Size = new Size(713, 312);
		((Control)rtbCodeEditor).TabIndex = 17;
		((Control)rtbCodeEditor).Text = "";
		((RichTextBox)rtbCodeEditor).SelectionChanged += RichTextBoxCodeEditor_SelectionChanged;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Dock = (DockStyle)1;
		stiGroupLine1.LinePosition = StiLinePosition.Center;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 32);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(713, 6);
		((Control)stiGroupLine1).TabIndex = 21;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)rtbCodeEditor);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)tlbBorder);
		((Control)this).Name = "StiRichTextEditorControl";
		((Control)this).Size = new Size(713, 350);
		((ISupportInitialize)tlbBorder).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
