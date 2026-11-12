using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Report.Design.Components;

public class StiBarCodeEditorForm : Form
{
	private class StiMouseScrollableControl : StiScrollableControl
	{
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				base.VScrollBar.DoUp();
			}
			else
			{
				base.VScrollBar.DoDown();
			}
		}
	}

	private struct BarCodeInfo
	{
		public string Code { get; }

		public int Width { get; }

		public BarCodeInfo(string code, int width)
		{
			Code = code;
			Width = width;
		}
	}

	public struct StiSampleInfo
	{
		public Bitmap Bitmap { get; set; }

		public StiBarCodeTypeService BarCodeType { get; set; }

		public int ImageWidth { get; set; }

		public int ImageHeight { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public StiSampleInfo(Bitmap bitmap, StiBarCodeTypeService barCodeType, int x, int y, int width, int height, int imageWidth, int imageHeight)
		{
			Bitmap = bitmap;
			BarCodeType = barCodeType;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
		}
	}

	private StiBarCode barCode;

	private StiBarCode tempBarCode;

	private bool lockInit;

	private Bitmap sampleBitmap;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button btOk;

	private Button btCancel;

	internal StiReportPropertyGrid PropertyGrid;

	private Panel panel1;

	private Label labelCode;

	private Label labelAngle;

	private StiComboBox comboBoxAngle;

	private CheckBox checkBoxAutoScale;

	private Label labelForeColor;

	private ColorPickerButton colorBoxForeColor;

	private PictureBox imageSample;

	private CheckBox checkBoxShowQuietZones;

	private CheckBox checkBoxShowLabelText;

	private Label labelFont;

	private Button buttonFont;

	private Label labelBackColor;

	private ColorPickerButton colorBoxBackColor;

	private StiTypesButton buttonBarcodeTypes;

	private Label labelType;

	private StiButtonEdit textBoxCode;

	public StiBarCodeEditorForm(StiBarCode barCode)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.barCode = barCode;
			tempBarCode = (StiBarCode)this.barCode.Clone();
			tempBarCode.HorAlignment = StiHorAlignment.Center;
			tempBarCode.VertAlignment = StiVertAlignment.Center;
			((PropertyGrid)PropertyGrid).CommandsVisibleIfAvailable = true;
			((PropertyGrid)PropertyGrid).LargeButtons = false;
			PropertyGrid.SetStandardPropertyTabs();
			((PropertyGrid)PropertyGrid).SelectedObject = tempBarCode.BarCodeType;
			lockInit = true;
			((Control)textBoxCode).Text = tempBarCode.Code.Value;
			checkBoxAutoScale.Checked = tempBarCode.AutoScale;
			colorBoxForeColor.SelectedColor = tempBarCode.ForeColor;
			colorBoxBackColor.SelectedColor = tempBarCode.BackColor;
			checkBoxShowLabelText.Checked = tempBarCode.ShowLabelText;
			checkBoxShowQuietZones.Checked = tempBarCode.ShowQuietZones;
			lockInit = false;
			((Form)this).ShowIcon = false;
			Localize();
			DoScaling();
			Build();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiBarCode");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type") ?? "";
		((Control)labelCode).Text = StiLocalization.Get("PropertyMain", "Code") ?? "";
		((Control)labelAngle).Text = StiLocalization.Get("PropertyMain", "Angle") ?? "";
		((Control)labelFont).Text = StiLocalization.Get("PropertyMain", "Font") ?? "";
		((Control)labelForeColor).Text = StiLocalization.Get("PropertyMain", "ForeColor") ?? "";
		((Control)labelBackColor).Text = StiLocalization.Get("PropertyMain", "BackColor") ?? "";
		((Control)checkBoxAutoScale).Text = StiLocalization.Get("PropertyMain", "AutoScale") ?? "";
		((Control)checkBoxShowLabelText).Text = StiLocalization.Get("PropertyMain", "ShowLabelText") ?? "";
		((Control)checkBoxShowQuietZones).Text = StiLocalization.Get("PropertyMain", "ShowQuietZones") ?? "";
		lockInit = true;
		int num = ((ListControl)comboBoxAngle).SelectedIndex;
		if (num == -1)
		{
			num = (int)tempBarCode.Angle / 90;
		}
		((ComboBox)comboBoxAngle).Items.Clear();
		((ComboBox)comboBoxAngle).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiAngleAngle0"));
		((ComboBox)comboBoxAngle).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiAngleAngle90"));
		((ComboBox)comboBoxAngle).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiAngleAngle180"));
		((ComboBox)comboBoxAngle).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiAngleAngle270"));
		((ListControl)comboBoxAngle).SelectedIndex = num;
		lockInit = false;
	}

	private void DoScaling()
	{
		((Control)comboBoxAngle).Width = ((Control)buttonBarcodeTypes).Width;
		((Control)colorBoxForeColor).Width = ((Control)buttonBarcodeTypes).Width;
		((Control)colorBoxBackColor).Width = ((Control)buttonBarcodeTypes).Width;
		textBoxCode.Image = StiReportDesignImages.Actions.EditSmall();
		PropertyGrid.SetStandardPropertyTabs();
	}

	private void CheckState()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		bool flag = tempBarCode.BarCodeType is StiSSCC18BarCodeType;
		bool visible = ((Control)labelCode).Visible;
		foreach (Control item in (ArrangedElementCollection)((Control)panel1).Controls)
		{
			Control val = item;
			if (val != labelType && val != buttonBarcodeTypes && val != PropertyGrid)
			{
				if (flag && visible)
				{
					val.Location = new Point(val.Location.X, val.Location.Y - 26);
				}
				else if (!flag && !visible)
				{
					val.Location = new Point(val.Location.X, val.Location.Y + 26);
				}
			}
		}
		Label obj = labelCode;
		bool visible2 = (((Control)textBoxCode).Visible = !flag);
		((Control)obj).Visible = visible2;
	}

	private void RefreshButtonBarCodeType()
	{
		buttonBarcodeTypes.Image = StiImageResizer.GetImage(GetBarCodeImage(tempBarCode.BarCodeType));
		((Control)buttonBarcodeTypes).Text = tempBarCode.BarCodeType.ServiceName;
	}

	private void Build()
	{
		FillBarCodeItems(buttonBarcodeTypes);
		RefreshButtonBarCodeType();
	}

	private void FillBarCodeItems(ButtonX categoryButton)
	{
		StiInfographicsToolboxInfo[] barCodesToolboxItems = StiToolboxHelper.GetBarCodesToolboxItems();
		StiInfographicsToolboxInfo[] array = barCodesToolboxItems;
		foreach (StiInfographicsToolboxInfo stiInfographicsToolboxInfo in array)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Image = StiImageResizer.GetImage((Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode." + stiInfographicsToolboxInfo.Icon + ".png")),
				Text = stiInfographicsToolboxInfo.LocName,
				AutoExpandOnClick = true
			};
			categoryButton.SubItems.Add(buttonItem);
			if (!(stiInfographicsToolboxInfo.Tag is List<StiBarCodeTypeService> list))
			{
				continue;
			}
			foreach (StiBarCodeTypeService item in list)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					Image = StiImageResizer.GetImage(GetBarCodeImage(item)),
					Text = item.ServiceName,
					Tag = item,
					BeginGroup = stiInfographicsToolboxInfo.BeginGroup
				};
				buttonItem2.Click += ButtonBarCodeType_Click;
				buttonItem.SubItems.Add(buttonItem2);
			}
		}
	}

	private Image GetBarCodeImage(StiBarCodeTypeService barcodeType)
	{
		try
		{
			return (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode." + barcodeType.ComponentId.ToString() + ".png");
		}
		catch
		{
			return (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode.StiPharmacodeBarCodeType.png");
		}
	}

	private void UpdateSample()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		int width = ((Control)imageSample).Width;
		int height = ((Control)imageSample).Height;
		RectangleF rect = new RectangleF(StiScale.I5, StiScale.I5, width - StiScale.I10, height - StiScale.I10);
		imageSample.Image = null;
		if (sampleBitmap != null)
		{
			((Control)this).DoubleBuffered = true;
			((Image)sampleBitmap).Dispose();
			sampleBitmap = null;
		}
		sampleBitmap = new Bitmap(width, height);
		Graphics val = Graphics.FromImage((Image)(object)sampleBitmap);
		try
		{
			val.CompositingMode = (CompositingMode)0;
			val.CompositingQuality = (CompositingQuality)0;
			val.SmoothingMode = (SmoothingMode)4;
			val.TextRenderingHint = (TextRenderingHint)0;
			float height2 = tempBarCode.BarCodeType.Height;
			float module = tempBarCode.BarCodeType.Module;
			tempBarCode.BarCodeType.Height = ((tempBarCode.BarCodeType is StiEAN13BarCodeType) ? 0.6f : 1f);
			if (tempBarCode.BarCodeType is StiITF14BarCodeType)
			{
				tempBarCode.BarCodeType.Module = 16f;
			}
			if (tempBarCode.BarCodeType is StiCode11BarCodeType)
			{
				tempBarCode.BarCodeType.Module = 12f;
			}
			if (tempBarCode.BarCodeType is StiQRCodeBarCodeType)
			{
				tempBarCode.BarCodeType.Module = 35f;
			}
			tempBarCode.BarCodeType.Draw(val, tempBarCode, rect, (float)StiScale.Factor);
			imageSample.Image = (Image)(object)sampleBitmap;
			tempBarCode.BarCodeType.Height = height2;
			tempBarCode.BarCodeType.Module = module;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void Panels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val2.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(0, 0);
			Point point2 = new Point(((Control)val).Width, 0);
			e.Graphics.DrawLine(val2, point, point2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void ButtonFont_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(tempBarCode.Font, barCode.Report);
		try
		{
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				tempBarCode.Font = stiFontEditorForm.SelectedFont;
				UpdateSample();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void TextBoxCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			if (tempBarCode.Code.Value != ((Control)textBoxCode).Text)
			{
				tempBarCode.Code.Value = ((Control)textBoxCode).Text;
				UpdateSample();
			}
		}
	}

	private void TextBoxCode_Enter(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = null;
	}

	private void TextBoxCode_Leave(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		if (tempBarCode.Code.Value != ((Control)textBoxCode).Text)
		{
			tempBarCode.Code.Value = ((Control)textBoxCode).Text;
			UpdateSample();
		}
	}

	private void ComboBoxAngle_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			switch (((ListControl)comboBoxAngle).SelectedIndex)
			{
			case 0:
				tempBarCode.Angle = StiAngle.Angle0;
				break;
			case 1:
				tempBarCode.Angle = StiAngle.Angle90;
				break;
			case 2:
				tempBarCode.Angle = StiAngle.Angle180;
				break;
			case 3:
				tempBarCode.Angle = StiAngle.Angle270;
				break;
			}
			UpdateSample();
		}
	}

	private void CheckBoxAutoScale_Click(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			tempBarCode.AutoScale = checkBoxAutoScale.Checked;
			UpdateSample();
		}
	}

	private void ColorBoxForeColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			tempBarCode.ForeColor = colorBoxForeColor.SelectedColor;
			UpdateSample();
		}
	}

	private void ColorBoxBackColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			tempBarCode.BackColor = colorBoxBackColor.SelectedColor;
			UpdateSample();
		}
	}

	private void CheckBoxShowLabelText_Click(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			tempBarCode.ShowLabelText = checkBoxShowLabelText.Checked;
			UpdateSample();
		}
	}

	private void CheckBoxShowQuietZones_Click(object sender, EventArgs e)
	{
		if (!lockInit)
		{
			tempBarCode.ShowQuietZones = checkBoxShowQuietZones.Checked;
			UpdateSample();
		}
	}

	private void PropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		UpdateSample();
	}

	private void ButtonBarCodeType_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		bool flag = tempBarCode.Code.Value == tempBarCode.BarCodeType.DefaultCodeValue;
		tempBarCode.BarCodeType = buttonItem.Tag as StiBarCodeTypeService;
		((PropertyGrid)PropertyGrid).SelectedObject = tempBarCode.BarCodeType;
		if (flag)
		{
			((Control)textBoxCode).Text = tempBarCode.BarCodeType.DefaultCodeValue;
			tempBarCode.Code.Value = tempBarCode.BarCodeType.DefaultCodeValue;
		}
		RefreshButtonBarCodeType();
		UpdateSample();
		CheckState();
	}

	private void ButtonExpression_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxCode).Text;
		text = StiExpressionPacker.PackExpression(text, barCode.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(barCode.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxCode).Text = StiExpressionPacker.UnPackExpression(text, barCode.Report.Designer, useBraces: false);
		}
		((Control)textBoxCode).Focus();
		textBoxCode.Select(0, 0);
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		barCode.Report.Designer.UndoRedoSave(StiLocalization.Get("Buttons", "Duplicate"));
		barCode.Report.IsModified = true;
		barCode.BarCodeType = tempBarCode.BarCodeType;
		barCode.Code.Value = tempBarCode.Code.Value;
		barCode.Angle = tempBarCode.Angle;
		barCode.AutoScale = tempBarCode.AutoScale;
		barCode.ForeColor = tempBarCode.ForeColor;
		barCode.BackColor = tempBarCode.BackColor;
		barCode.ShowLabelText = tempBarCode.ShowLabelText;
		barCode.ShowQuietZones = tempBarCode.ShowQuietZones;
		barCode.Font = tempBarCode.Font;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Load(object sender, EventArgs e)
	{
		textBoxCode.Select(((Control)textBoxCode).Text.Length, 0);
		CheckState();
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		imageSample = new PictureBox();
		panel1 = new Panel();
		textBoxCode = new StiButtonEdit();
		PropertyGrid = new StiReportPropertyGrid();
		labelType = new Label();
		buttonBarcodeTypes = new StiTypesButton();
		checkBoxShowQuietZones = new CheckBox();
		checkBoxShowLabelText = new CheckBox();
		labelFont = new Label();
		buttonFont = new Button();
		labelBackColor = new Label();
		colorBoxBackColor = new ColorPickerButton();
		colorBoxForeColor = new ColorPickerButton();
		labelForeColor = new Label();
		checkBoxAutoScale = new CheckBox();
		comboBoxAngle = new StiComboBox();
		labelAngle = new Label();
		labelCode = new Label();
		stiGroupLine1 = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		((ISupportInitialize)imageSample).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)imageSample).BackColor = Color.White;
		((Control)imageSample).Dock = (DockStyle)5;
		((Control)imageSample).Location = new Point(0, 0);
		((Control)imageSample).Name = "imageSample";
		((Control)imageSample).Size = new Size(584, 179);
		imageSample.SizeMode = (PictureBoxSizeMode)3;
		imageSample.TabIndex = 1;
		imageSample.TabStop = false;
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)textBoxCode);
		((Control)panel1).Controls.Add((Control)(object)PropertyGrid);
		((Control)panel1).Controls.Add((Control)(object)labelType);
		((Control)panel1).Controls.Add((Control)(object)buttonBarcodeTypes);
		((Control)panel1).Controls.Add((Control)(object)checkBoxShowQuietZones);
		((Control)panel1).Controls.Add((Control)(object)checkBoxShowLabelText);
		((Control)panel1).Controls.Add((Control)(object)labelFont);
		((Control)panel1).Controls.Add((Control)(object)buttonFont);
		((Control)panel1).Controls.Add((Control)(object)labelBackColor);
		((Control)panel1).Controls.Add((Control)(object)colorBoxBackColor);
		((Control)panel1).Controls.Add((Control)(object)colorBoxForeColor);
		((Control)panel1).Controls.Add((Control)(object)labelForeColor);
		((Control)panel1).Controls.Add((Control)(object)checkBoxAutoScale);
		((Control)panel1).Controls.Add((Control)(object)comboBoxAngle);
		((Control)panel1).Controls.Add((Control)(object)labelAngle);
		((Control)panel1).Controls.Add((Control)(object)labelCode);
		((Control)panel1).Dock = (DockStyle)2;
		((Control)panel1).Location = new Point(0, 179);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(584, 327);
		((Control)panel1).TabIndex = 0;
		((Control)panel1).Paint += new PaintEventHandler(Panels_Paint);
		((Control)textBoxCode).AllowDrop = true;
		((Control)textBoxCode).Anchor = (AnchorStyles)13;
		((Control)textBoxCode).BackColor = SystemColors.Window;
		((Control)textBoxCode).Location = new Point(148, 47);
		((Control)textBoxCode).Margin = new Padding(2);
		((Control)textBoxCode).Name = "textBoxCode";
		((Control)textBoxCode).Size = new Size(163, 21);
		((Control)textBoxCode).TabIndex = 1;
		textBoxCode.ButtonClick += ButtonExpression_Click;
		((Control)textBoxCode).Enter += TextBoxCode_Enter;
		((Control)textBoxCode).KeyDown += new KeyEventHandler(TextBoxCode_KeyDown);
		((Control)textBoxCode).Leave += TextBoxCode_Leave;
		((Control)PropertyGrid).Anchor = (AnchorStyles)7;
		((Control)PropertyGrid).Cursor = Cursors.HSplit;
		((PropertyGrid)PropertyGrid).HelpBackColor = Color.White;
		((PropertyGrid)PropertyGrid).LineColor = SystemColors.Control;
		((Control)PropertyGrid).Location = new Point(324, 12);
		((Control)PropertyGrid).Name = "PropertyGrid";
		PropertyGrid.ScrollOffset = 0;
		PropertyGrid.ShowEventsTag = false;
		((Control)PropertyGrid).Size = new Size(246, 299);
		((Control)PropertyGrid).TabIndex = 9;
		((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		((PropertyGrid)PropertyGrid).SelectedObjectsChanged += PropertyGrid_SelectedObjectsChanged;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 12);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(129, 23);
		((Control)labelType).TabIndex = 48;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		((Control)buttonBarcodeTypes).AccessibleRole = (AccessibleRole)43;
		buttonBarcodeTypes.AutoExpandOnClick = true;
		buttonBarcodeTypes.ColorTable = eButtonColor.OrangeWithBackground;
		buttonBarcodeTypes.Is20Button = false;
		((Control)buttonBarcodeTypes).Location = new Point(148, 12);
		((Control)buttonBarcodeTypes).Margin = new Padding(4);
		((Control)buttonBarcodeTypes).Name = "buttonBarcodeTypes";
		buttonBarcodeTypes.PopupSide = ePopupSide.Right;
		buttonBarcodeTypes.ShowArrow = false;
		((Control)buttonBarcodeTypes).Size = new Size(163, 23);
		buttonBarcodeTypes.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonBarcodeTypes).TabIndex = 0;
		((Control)buttonBarcodeTypes).Text = "buttonX1";
		buttonBarcodeTypes.TextAlignment = eButtonTextAlignment.Left;
		((ButtonBase)checkBoxShowQuietZones).Image = null;
		((Control)checkBoxShowQuietZones).Location = new Point(148, 286);
		((Control)checkBoxShowQuietZones).Name = "checkBoxShowQuietZones";
		((Control)checkBoxShowQuietZones).Size = new Size(163, 24);
		((Control)checkBoxShowQuietZones).TabIndex = 8;
		((Control)checkBoxShowQuietZones).Text = "Show Quiet Zones";
		((ButtonBase)checkBoxShowQuietZones).UseVisualStyleBackColor = true;
		((Control)checkBoxShowQuietZones).Click += CheckBoxShowQuietZones_Click;
		((ButtonBase)checkBoxShowLabelText).Image = null;
		((Control)checkBoxShowLabelText).Location = new Point(148, 253);
		((Control)checkBoxShowLabelText).Name = "checkBoxShowLabelText";
		((Control)checkBoxShowLabelText).Size = new Size(164, 24);
		((Control)checkBoxShowLabelText).TabIndex = 7;
		((Control)checkBoxShowLabelText).Text = "Show Label Text";
		((ButtonBase)checkBoxShowLabelText).UseVisualStyleBackColor = true;
		((Control)checkBoxShowLabelText).Click += CheckBoxShowLabelText_Click;
		((Control)labelFont).ForeColor = Color.DimGray;
		((Control)labelFont).Location = new Point(12, 218);
		((Control)labelFont).Name = "labelFont";
		((Control)labelFont).Size = new Size(129, 23);
		((Control)labelFont).TabIndex = 42;
		((Control)labelFont).Text = "Font";
		labelFont.TextAlign = (ContentAlignment)16;
		((Control)buttonFont).Location = new Point(147, 218);
		((Control)buttonFont).Name = "buttonFont";
		((Control)buttonFont).Size = new Size(165, 23);
		((Control)buttonFont).TabIndex = 6;
		((Control)buttonFont).Text = "Font";
		((ButtonBase)buttonFont).UseVisualStyleBackColor = true;
		((Control)buttonFont).Click += ButtonFont_Click;
		((Control)labelBackColor).ForeColor = Color.DimGray;
		((Control)labelBackColor).Location = new Point(12, 182);
		((Control)labelBackColor).Name = "labelBackColor";
		((Control)labelBackColor).Size = new Size(129, 24);
		((Control)labelBackColor).TabIndex = 40;
		((Control)labelBackColor).Text = "Back Color";
		labelBackColor.TextAlign = (ContentAlignment)16;
		((Control)colorBoxBackColor).AccessibleRole = (AccessibleRole)43;
		colorBoxBackColor.AutoExpandOnClick = true;
		colorBoxBackColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxBackColor.ImagePosition = eImagePosition.Right;
		colorBoxBackColor.Is20Button = false;
		((Control)colorBoxBackColor).Location = new Point(148, 182);
		((Control)colorBoxBackColor).Name = "colorBoxBackColor";
		((Control)colorBoxBackColor).Size = new Size(163, 24);
		((Control)colorBoxBackColor).TabIndex = 5;
		((Control)colorBoxBackColor).TabStop = false;
		colorBoxBackColor.SelectedColorChanged += ColorBoxBackColor_SelectedColorChanged;
		((Control)colorBoxForeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxForeColor.AutoExpandOnClick = true;
		colorBoxForeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxForeColor.ImagePosition = eImagePosition.Right;
		colorBoxForeColor.Is20Button = false;
		((Control)colorBoxForeColor).Location = new Point(148, 146);
		((Control)colorBoxForeColor).Name = "colorBoxForeColor";
		((Control)colorBoxForeColor).Size = new Size(163, 24);
		((Control)colorBoxForeColor).TabIndex = 4;
		((Control)colorBoxForeColor).TabStop = false;
		colorBoxForeColor.SelectedColorChanged += ColorBoxForeColor_SelectedColorChanged;
		((Control)labelForeColor).ForeColor = Color.DimGray;
		((Control)labelForeColor).Location = new Point(12, 146);
		((Control)labelForeColor).Name = "labelForeColor";
		((Control)labelForeColor).Size = new Size(129, 24);
		((Control)labelForeColor).TabIndex = 37;
		((Control)labelForeColor).Text = "Fore Color";
		labelForeColor.TextAlign = (ContentAlignment)16;
		((ButtonBase)checkBoxAutoScale).Image = null;
		((Control)checkBoxAutoScale).Location = new Point(148, 113);
		((Control)checkBoxAutoScale).Name = "checkBoxAutoScale";
		((Control)checkBoxAutoScale).Size = new Size(165, 21);
		((Control)checkBoxAutoScale).TabIndex = 3;
		((Control)checkBoxAutoScale).Text = "Auto Scale";
		((ButtonBase)checkBoxAutoScale).UseVisualStyleBackColor = true;
		((Control)checkBoxAutoScale).Click += CheckBoxAutoScale_Click;
		((Control)comboBoxAngle).BackColor = SystemColors.Window;
		comboBoxAngle.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxAngle).ForeColor = SystemColors.ControlText;
		((Control)comboBoxAngle).Location = new Point(148, 80);
		((ComboBox)comboBoxAngle).MaxDropDownItems = 20;
		((Control)comboBoxAngle).Name = "comboBoxAngle";
		((Control)comboBoxAngle).Size = new Size(163, 21);
		((Control)comboBoxAngle).TabIndex = 2;
		((ComboBox)comboBoxAngle).SelectedIndexChanged += ComboBoxAngle_SelectedIndexChanged;
		((Control)labelAngle).ForeColor = Color.DimGray;
		((Control)labelAngle).Location = new Point(12, 80);
		((Control)labelAngle).Name = "labelAngle";
		((Control)labelAngle).Size = new Size(129, 21);
		((Control)labelAngle).TabIndex = 2;
		((Control)labelAngle).Text = "Angle";
		labelAngle.TextAlign = (ContentAlignment)16;
		((Control)labelCode).ForeColor = Color.DimGray;
		((Control)labelCode).Location = new Point(12, 48);
		((Control)labelCode).Name = "labelCode";
		((Control)labelCode).Size = new Size(129, 20);
		((Control)labelCode).TabIndex = 0;
		((Control)labelCode).Text = "Code";
		labelCode.TextAlign = (ContentAlignment)16;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 506);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(584, 46);
		((Control)stiGroupLine1).TabIndex = 0;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(415, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(496, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(584, 552);
		((Control)this).Controls.Add((Control)(object)imageSample);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiBarCodeEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "BarCode Editor";
		((Form)this).Load += This_Load;
		((ISupportInitialize)imageSample).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
