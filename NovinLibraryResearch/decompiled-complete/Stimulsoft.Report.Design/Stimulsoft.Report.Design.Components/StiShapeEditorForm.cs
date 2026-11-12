using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.Components;

public class StiShapeEditorForm : StiFormUX
{
	private StiShape shape;

	private StiShape tempShape;

	private Bitmap sampleBitmap;

	private StiPopup shapePopup;

	private IContainer components;

	private PictureBox imageSample;

	private StiButtonPanelUX footerPanel;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiPanelUX panelData;

	private StiLabelUX labelType;

	private StiLabelUX labelDirection;

	private StiTypesButton buttonDirection;

	private StiTypesButton buttonType;

	private ColorPickerButton colorBoxBorderColor;

	private StiLabelUX labelBorderColor;

	private StiNumericUpDown textBoxSize;

	private StiLabelUX labelSize;

	private StiLabelUX labelStyle;

	private StiLabelUX labelForeColor;

	private StiLabelUX labelBackgroundColor;

	private StiLabelUX labelFont;

	private StiLabelUX labelText;

	private ColorPickerButton colorBoxForeColor;

	private ColorPickerButton colorBoxBackgroundColor;

	private StiButtonUX buttonFont;

	private StiTypesButton buttonBorderStyle;

	private ImageList imageListPenStyle;

	private ButtonItem buttonSolid;

	private ButtonItem buttonDash;

	private ButtonItem buttonDashDot;

	private ButtonItem buttonDashDotDot;

	private ButtonItem buttonDot;

	private ButtonItem buttonNone;

	private ButtonItem buttonDirectionUp;

	private ButtonItem buttonDirectionDown;

	private ButtonItem buttonDirectionLeft;

	private ButtonItem buttonDirectionRight;

	private StiBrushControl brushControl;

	private StiLabelUX labelBrush;

	private StiPanelUX panelType;

	private StiPanelUX panelDirection;

	private StiPanelUX panelSize;

	private StiPanelUX panelBorderColor;

	private StiPanelUX panelBorderStyle;

	private StiPanelUX panelText;

	private StiPanelUX panelForeColor;

	private StiPanelUX panelBackgroundColor;

	private StiPanelUX panelFont;

	private StiPanelUX panelBrush;

	private StiPanelBox panelExpression;

	private HintTextBox textBoxExpression;

	private StiButtonUX buttonExpression;

	public StiShapeEditorForm(StiShape shape)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			buttonSolid.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false);
			buttonDash.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false);
			buttonDashDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false);
			buttonDashDotDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false);
			buttonDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false);
			buttonNone.Image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
			((ButtonBase)buttonExpression).Image = StiReportDesignImages.Actions.Edit();
			this.shape = shape;
			tempShape = (StiShape)this.shape.Clone();
			((Control)textBoxExpression).Text = tempShape.Text;
			colorBoxBackgroundColor.SelectedColor = tempShape.BackgroundColor;
			colorBoxForeColor.SelectedColor = tempShape.ForeColor;
			colorBoxBorderColor.SelectedColor = tempShape.BorderColor;
			textBoxSize.Value = (decimal)tempShape.Size;
			brushControl.Brush = tempShape.Brush;
			((Form)this).ShowIcon = false;
			((Control)buttonFont).Click += ButtonFont_Click;
			UpdateDirectionState();
			Localize();
			Build();
			CalculateThisHeight();
		}
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiShape");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		buttonSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		buttonDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		buttonDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		buttonDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		buttonDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		buttonNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		buttonDirectionUp.Text = StiLocalization.Get("PropertyEnum", "StiShapeDirectionUp");
		buttonDirectionDown.Text = StiLocalization.Get("PropertyEnum", "StiShapeDirectionDown");
		buttonDirectionLeft.Text = StiLocalization.Get("PropertyEnum", "StiShapeDirectionLeft");
		buttonDirectionRight.Text = StiLocalization.Get("PropertyEnum", "StiShapeDirectionRight");
		((Control)buttonFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type");
		((Control)labelDirection).Text = StiLocalization.Get("PropertyMain", "Direction");
		((Control)labelStyle).Text = StiLocalization.Get("PropertyMain", "Style");
		((Control)labelSize).Text = StiLocalization.Get("PropertyMain", "Size");
		((Control)labelBorderColor).Text = StiLocalization.Get("PropertyMain", "BorderColor");
		((Control)labelBrush).Text = StiLocalization.Get("PropertyMain", "Brush");
		((Control)labelText).Text = StiLocalization.Get("PropertyMain", "Text");
		((Control)labelForeColor).Text = StiLocalization.Get("PropertyMain", "ForeColor");
		((Control)labelFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)labelBackgroundColor).Text = StiLocalization.Get("PropertyMain", "BackgroundColor");
	}

	private void Build()
	{
		RefreshButtonType();
		RefreshButtonBorderStyle();
		UpdateSample();
	}

	private void RefreshButtonType()
	{
		StiArrowShapeType stiArrowShapeType = tempShape.ShapeType as StiArrowShapeType;
		string text = tempShape.ShapeType.GetType().Name;
		if (stiArrowShapeType != null && stiArrowShapeType.Direction != 0)
		{
			text += stiArrowShapeType.Direction;
		}
		buttonType.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + text + StiScale.StepName + ".png");
		Size size = TextRenderer.MeasureText(tempShape.ShapeType.ServiceName, ((Control)buttonType).Font);
		if (size.Width > StiScaleUI.I(120))
		{
			float num = (float)size.Width / (float)tempShape.ShapeType.ServiceName.Length;
			int length = (int)((float)StiScaleUI.I(120) / num);
			((Control)buttonType).Text = tempShape.ShapeType.ServiceName.Substring(0, length) + "...";
		}
		else
		{
			((Control)buttonType).Text = tempShape.ShapeType.ServiceName;
		}
	}

	private void RefreshButtonBorderStyle()
	{
		Image image = null;
		string text = string.Empty;
		switch (tempShape.Style)
		{
		case StiPenStyle.Dash:
			image = imageListPenStyle.Images["Dash.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
			break;
		case StiPenStyle.DashDot:
			image = imageListPenStyle.Images["DashDot.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
			break;
		case StiPenStyle.DashDotDot:
			image = imageListPenStyle.Images["DashDotDot.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
			break;
		case StiPenStyle.Dot:
			image = imageListPenStyle.Images["Dot.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
			break;
		case StiPenStyle.None:
			image = imageListPenStyle.Images["None.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
			break;
		case StiPenStyle.Solid:
			image = imageListPenStyle.Images["Solid.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
			break;
		case StiPenStyle.Double:
			image = imageListPenStyle.Images["None.png"];
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
			break;
		}
		Size size = TextRenderer.MeasureText(text, ((Control)buttonBorderStyle).Font);
		if (size.Width > StiScaleUI.I(100))
		{
			float num = (float)size.Width / (float)text.Length;
			int length = (int)((float)StiScaleUI.I(100) / num);
			((Control)buttonBorderStyle).Text = text.Substring(0, length) + "...";
		}
		else
		{
			((Control)buttonBorderStyle).Text = text;
		}
		buttonBorderStyle.Image = StiImageResizer.GetImage(image);
	}

	private void UpdateSample()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		imageSample.Image = null;
		if (sampleBitmap != null)
		{
			((Image)sampleBitmap).Dispose();
			sampleBitmap = null;
		}
		int num = (int)((double)Math.Min(((Control)imageSample).Width, ((Control)imageSample).Height) * StiScale.Factor);
		sampleBitmap = new Bitmap(num, num);
		Graphics val = Graphics.FromImage((Image)(object)sampleBitmap);
		try
		{
			val.CompositingQuality = (CompositingQuality)2;
			val.SmoothingMode = (SmoothingMode)2;
			val.TextRenderingHint = (TextRenderingHint)3;
			int num2 = StiScale.XXI(12.0);
			RectangleD rect = new RectangleD(num2, num2, ((Image)sampleBitmap).Width - num2 * 2, ((Image)sampleBitmap).Height - num2 * 2);
			tempShape.ShapeType.Paint(val, tempShape, rect.ToRectangleF(), 1f);
			StiShapeGdiPainter stiShapeGdiPainter = (StiShapeGdiPainter)StiPainter.GetPainter(typeof(StiShape), StiGuiMode.Gdi);
			stiShapeGdiPainter.DrawText(val, tempShape, rect);
			imageSample.Image = (Image)(object)sampleBitmap;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void UpdateDirectionState()
	{
		PropertyInfo property = tempShape.ShapeType.GetType().GetProperty("Direction");
		if (property != null)
		{
			StiShapeDirection stiShapeDirection = (StiShapeDirection)property.GetValue(tempShape.ShapeType, null);
			((Control)buttonDirection).Text = StiLocalization.Get("PropertyEnum", $"StiShapeDirection{stiShapeDirection}");
			((Control)panelDirection).Visible = true;
		}
		else
		{
			((Control)panelDirection).Visible = false;
		}
		CalculateThisHeight();
	}

	private void CalculateThisHeight()
	{
		((Control)panelBrush).Height = ((Control)brushControl).Height + StiScale.XXI(12.0);
		((Control)panelData).Size = new Size(StiScale.XXI(394.0), ((Control)panelType).Height + ((Control)panelDirection).Height + ((Control)panelSize).Height + ((Control)panelBorderColor).Height + ((Control)panelBorderStyle).Height + ((Control)panelBrush).Height + ((Control)panelText).Height + ((Control)panelForeColor).Height + ((Control)panelFont).Height + ((Control)panelBackgroundColor).Height);
		int height = ((Control)imageSample).Height + ((Control)panelData).Height + ((Control)footerPanel).Height + StiScale.YYI(48.0);
		((Control)this).Height = height;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		shape.Report.Designer.UndoRedoSave(StiLocalization.Get("Buttons", "Duplicate"));
		shape.Report.IsModified = true;
		shape.BorderColor = tempShape.BorderColor;
		shape.Size = tempShape.Size;
		shape.Style = tempShape.Style;
		shape.ShapeType = tempShape.ShapeType;
		shape.Brush = tempShape.Brush;
		shape.Text = tempShape.Text;
		shape.ForeColor = tempShape.ForeColor;
		shape.Font = tempShape.Font;
		shape.BackgroundColor = tempShape.BackgroundColor;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (shape.Report.Designer.DesignerControl is StiDesignerControl designer)
		{
			StiShapeToolboxControl stiShapeToolboxControl = new StiShapeToolboxControl(designer, new MouseEventHandler(ButtonItemType_Click), null, showBaseShapes: false, showBorder: true);
			shapePopup = new StiPopup((Control)(object)stiShapeToolboxControl);
			stiShapeToolboxControl.Build();
		}
		Timer timer = new Timer
		{
			Interval = 70
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)textBoxSize).Focus();
			((Control)textBoxSize).Select();
			((Component)(object)timer).Dispose();
		};
		timer.Start();
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

	private void ButtonTypeSelected_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		if (buttonItem == buttonSolid)
		{
			tempShape.Style = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonDash)
		{
			tempShape.Style = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonDashDot)
		{
			tempShape.Style = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonDashDotDot)
		{
			tempShape.Style = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonDot)
		{
			tempShape.Style = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonNone)
		{
			tempShape.Style = StiPenStyle.None;
		}
		UpdateSample();
		RefreshButtonBorderStyle();
	}

	private void ButtonDirectionSelected_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		((Control)buttonDirection).Text = buttonItem.Text;
		StiShapeDirection stiShapeDirection = StiShapeDirection.Up;
		if (buttonItem == buttonDirectionUp)
		{
			stiShapeDirection = StiShapeDirection.Up;
		}
		else if (buttonItem == buttonDirectionDown)
		{
			stiShapeDirection = StiShapeDirection.Down;
		}
		else if (buttonItem == buttonDirectionLeft)
		{
			stiShapeDirection = StiShapeDirection.Left;
		}
		else if (buttonItem == buttonDirectionRight)
		{
			stiShapeDirection = StiShapeDirection.Right;
		}
		PropertyInfo property = tempShape.ShapeType.GetType().GetProperty("Direction");
		if (property != null)
		{
			property.SetValue(tempShape.ShapeType, stiShapeDirection, null);
		}
		UpdateSample();
	}

	private void ButtonItemType_Click(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			StiShape stiShape = val.Tag as StiShape;
			tempShape.ShapeType = stiShape.ShapeType.Clone() as StiShapeTypeService;
			UpdateDirectionState();
			UpdateSample();
			RefreshButtonType();
		}
		((Control)shapePopup).Hide();
		BrushControl_ClientSizeChanged(sender, EventArgs.Empty);
	}

	private void TextBoxSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			tempShape.Size = (float)textBoxSize.Value;
			UpdateSample();
		}
	}

	private void TextBoxText_TextChanged(object sender, EventArgs e)
	{
		tempShape.Text = ((Control)textBoxExpression).Text;
		UpdateSample();
	}

	private void TextBoxSize_Enter(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = null;
	}

	private void TextBoxSize_Leave(object sender, EventArgs e)
	{
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		tempShape.Size = (float)textBoxSize.Value;
		UpdateSample();
	}

	private void ButtonFont_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(tempShape.Font, shape.Report);
		try
		{
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				tempShape.Font = stiFontEditorForm.SelectedFont;
				UpdateSample();
			}
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	private void ColorBoxBorderColor_SelectedColorChanged(object sender, EventArgs e)
	{
		tempShape.BorderColor = colorBoxBorderColor.SelectedColor;
		UpdateSample();
	}

	private void ColorBoxForeColor_SelectedColorChanged(object sender, EventArgs e)
	{
		tempShape.ForeColor = colorBoxForeColor.SelectedColor;
		UpdateSample();
	}

	private void ColorBoxBackgroundColor_SelectedColorChanged(object sender, EventArgs e)
	{
		tempShape.BackgroundColor = colorBoxBackgroundColor.SelectedColor;
		UpdateSample();
	}

	private void BrushControl_ClientSizeChanged(object sender, EventArgs e)
	{
		CalculateThisHeight();
	}

	private void BrushControl_BrushChanged(object sender, EventArgs e)
	{
		tempShape.Brush = brushControl.Brush;
		UpdateSample();
	}

	private void ButtonType_Click(object sender, EventArgs e)
	{
		ButtonX buttonX = sender as ButtonX;
		((ToolStripDropDown)shapePopup).Show(new Point(((Control)buttonX).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonX).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonX).Height));
	}

	private void ButtonExpression_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxExpression).Text;
		text = StiExpressionPacker.PackExpression(text, shape.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(shape.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxExpression).Text = StiExpressionPacker.UnPackExpression(text, shape.Report.Designer, useBraces: false);
		}
		((Control)textBoxExpression).Focus();
		((TextBoxBase)textBoxExpression).Select(0, 0);
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f7: Expected O, but got Unknown
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1952: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a01: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiShapeEditorForm));
		imageSample = new PictureBox();
		panelData = new StiPanelUX();
		panelBrush = new StiPanelUX();
		labelBrush = new StiLabelUX();
		brushControl = new StiBrushControl();
		panelFont = new StiPanelUX();
		labelFont = new StiLabelUX();
		buttonFont = new StiButtonUX();
		panelBackgroundColor = new StiPanelUX();
		labelBackgroundColor = new StiLabelUX();
		colorBoxBackgroundColor = new ColorPickerButton();
		panelForeColor = new StiPanelUX();
		labelForeColor = new StiLabelUX();
		colorBoxForeColor = new ColorPickerButton();
		panelText = new StiPanelUX();
		panelExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpression = new StiButtonUX();
		labelText = new StiLabelUX();
		panelBorderStyle = new StiPanelUX();
		labelStyle = new StiLabelUX();
		buttonBorderStyle = new StiTypesButton();
		buttonSolid = new ButtonItem();
		buttonDash = new ButtonItem();
		buttonDashDot = new ButtonItem();
		buttonDashDotDot = new ButtonItem();
		buttonDot = new ButtonItem();
		buttonNone = new ButtonItem();
		panelBorderColor = new StiPanelUX();
		labelBorderColor = new StiLabelUX();
		colorBoxBorderColor = new ColorPickerButton();
		panelSize = new StiPanelUX();
		labelSize = new StiLabelUX();
		textBoxSize = new StiNumericUpDown();
		panelDirection = new StiPanelUX();
		labelDirection = new StiLabelUX();
		buttonDirection = new StiTypesButton();
		buttonDirectionUp = new ButtonItem();
		buttonDirectionDown = new ButtonItem();
		buttonDirectionLeft = new ButtonItem();
		buttonDirectionRight = new ButtonItem();
		panelType = new StiPanelUX();
		labelType = new StiLabelUX();
		buttonType = new StiTypesButton();
		imageListPenStyle = new ImageList(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		footerPanel = new StiButtonPanelUX();
		((ISupportInitialize)imageSample).BeginInit();
		((Control)panelData).SuspendLayout();
		((Control)panelBrush).SuspendLayout();
		((Control)panelFont).SuspendLayout();
		((Control)panelBackgroundColor).SuspendLayout();
		((Control)panelForeColor).SuspendLayout();
		((Control)panelText).SuspendLayout();
		((Control)panelExpression).SuspendLayout();
		((Control)panelBorderStyle).SuspendLayout();
		((Control)panelBorderColor).SuspendLayout();
		((Control)panelSize).SuspendLayout();
		((ISupportInitialize)textBoxSize).BeginInit();
		((Control)panelDirection).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)footerPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)imageSample).BackColor = Color.White;
		((Control)imageSample).Dock = (DockStyle)1;
		((Control)imageSample).Location = new Point(0, 0);
		((Control)imageSample).Name = "imageSample";
		((Control)imageSample).Size = new Size(394, 166);
		imageSample.SizeMode = (PictureBoxSizeMode)4;
		imageSample.TabIndex = 0;
		imageSample.TabStop = false;
		((Control)panelData).BackColor = Color.White;
		((Control)panelData).Controls.Add((Control)(object)panelBrush);
		((Control)panelData).Controls.Add((Control)(object)panelFont);
		((Control)panelData).Controls.Add((Control)(object)panelBackgroundColor);
		((Control)panelData).Controls.Add((Control)(object)panelForeColor);
		((Control)panelData).Controls.Add((Control)(object)panelText);
		((Control)panelData).Controls.Add((Control)(object)panelBorderStyle);
		((Control)panelData).Controls.Add((Control)(object)panelBorderColor);
		((Control)panelData).Controls.Add((Control)(object)panelSize);
		((Control)panelData).Controls.Add((Control)(object)panelDirection);
		((Control)panelData).Controls.Add((Control)(object)panelType);
		((Control)panelData).Dock = (DockStyle)1;
		((Control)panelData).Location = new Point(0, 166);
		((Control)panelData).Name = "panelData";
		((Control)panelData).Size = new Size(394, 234);
		((Control)panelData).TabIndex = 0;
		((Control)panelData).Paint += new PaintEventHandler(Panels_Paint);
		((Control)panelBrush).Controls.Add((Control)(object)labelBrush);
		((Control)panelBrush).Controls.Add((Control)(object)brushControl);
		((Control)panelBrush).Dock = (DockStyle)1;
		((Control)panelBrush).Location = new Point(0, 324);
		((Control)panelBrush).Name = "panelBrush";
		((Control)panelBrush).Size = new Size(394, 90);
		((Control)panelBrush).TabIndex = 12;
		((Control)labelBrush).ForeColor = Color.DimGray;
		((Control)labelBrush).Location = new Point(12, 6);
		((Control)labelBrush).Name = "labelBrush";
		((Control)labelBrush).Size = new Size(130, 20);
		((Control)labelBrush).TabIndex = 10;
		((Control)labelBrush).Text = "Brush";
		((Label)labelBrush).TextAlign = (ContentAlignment)16;
		((Control)brushControl).BackColor = Color.White;
		brushControl.Brush = null;
		((Control)brushControl).Location = new Point(148, 6);
		((Control)brushControl).Margin = new Padding(6);
		((Control)brushControl).Name = "brushControl";
		((Control)brushControl).Padding = new Padding(1);
		((Control)brushControl).Size = new Size(241, 78);
		((Control)brushControl).TabIndex = 11;
		brushControl.BrushChanged += BrushControl_BrushChanged;
		((Control)brushControl).ClientSizeChanged += BrushControl_ClientSizeChanged;
		((Control)panelFont).Controls.Add((Control)(object)labelFont);
		((Control)panelFont).Controls.Add((Control)(object)buttonFont);
		((Control)panelFont).Dock = (DockStyle)1;
		((Control)panelFont).Location = new Point(0, 289);
		((Control)panelFont).Name = "panelFont";
		((Control)panelFont).Size = new Size(394, 35);
		((Control)panelFont).TabIndex = 12;
		((Control)labelFont).ForeColor = Color.DimGray;
		((Control)labelFont).Location = new Point(12, 6);
		((Control)labelFont).Name = "labelFont";
		((Control)labelFont).Size = new Size(130, 23);
		((Control)labelFont).TabIndex = 8;
		((Control)labelFont).Text = "Font";
		((Label)labelFont).TextAlign = (ContentAlignment)16;
		((Control)buttonFont).Location = new Point(149, 6);
		((Control)buttonFont).Name = "buttonFont";
		((Control)buttonFont).Size = new Size(163, 24);
		((Control)buttonFont).TabIndex = 7;
		((Control)buttonFont).TabStop = false;
		((Control)buttonFont).Text = "Font";
		((Control)panelBackgroundColor).Controls.Add((Control)(object)labelBackgroundColor);
		((Control)panelBackgroundColor).Controls.Add((Control)(object)colorBoxBackgroundColor);
		((Control)panelBackgroundColor).Dock = (DockStyle)1;
		((Control)panelBackgroundColor).Location = new Point(0, 254);
		((Control)panelBackgroundColor).Name = "panelBackgroundColor";
		((Control)panelBackgroundColor).Size = new Size(394, 35);
		((Control)panelBackgroundColor).TabIndex = 12;
		((Control)labelBackgroundColor).ForeColor = Color.DimGray;
		((Control)labelBackgroundColor).Location = new Point(12, 6);
		((Control)labelBackgroundColor).Name = "labelBackgroundColor";
		((Control)labelBackgroundColor).Size = new Size(130, 23);
		((Control)labelBackgroundColor).TabIndex = 8;
		((Control)labelBackgroundColor).Text = "Backgroud Color";
		((Label)labelBackgroundColor).TextAlign = (ContentAlignment)16;
		((Control)colorBoxBackgroundColor).AccessibleRole = (AccessibleRole)43;
		colorBoxBackgroundColor.AutoExpandOnClick = true;
		colorBoxBackgroundColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxBackgroundColor.ImagePosition = eImagePosition.Right;
		((Control)colorBoxBackgroundColor).Location = new Point(149, 6);
		colorBoxBackgroundColor.LockClosePopup = false;
		((Control)colorBoxBackgroundColor).Name = "colorBoxBackgroundColor";
		((Control)colorBoxBackgroundColor).Size = new Size(163, 24);
		((Control)colorBoxBackgroundColor).TabIndex = 7;
		((Control)colorBoxBackgroundColor).TabStop = false;
		colorBoxBackgroundColor.SelectedColorChanged += ColorBoxBackgroundColor_SelectedColorChanged;
		((Control)panelForeColor).Controls.Add((Control)(object)labelForeColor);
		((Control)panelForeColor).Controls.Add((Control)(object)colorBoxForeColor);
		((Control)panelForeColor).Dock = (DockStyle)1;
		((Control)panelForeColor).Location = new Point(0, 219);
		((Control)panelForeColor).Name = "panelForeColor";
		((Control)panelForeColor).Size = new Size(394, 35);
		((Control)panelForeColor).TabIndex = 12;
		((Control)labelForeColor).ForeColor = Color.DimGray;
		((Control)labelForeColor).Location = new Point(12, 6);
		((Control)labelForeColor).Name = "labelForeColor";
		((Control)labelForeColor).Size = new Size(130, 23);
		((Control)labelForeColor).TabIndex = 8;
		((Control)labelForeColor).Text = "Text Color";
		((Label)labelForeColor).TextAlign = (ContentAlignment)16;
		((Control)colorBoxForeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxForeColor.AutoExpandOnClick = true;
		colorBoxForeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxForeColor.ImagePosition = eImagePosition.Right;
		((Control)colorBoxForeColor).Location = new Point(149, 6);
		colorBoxForeColor.LockClosePopup = false;
		((Control)colorBoxForeColor).Name = "colorBoxForeColor";
		((Control)colorBoxForeColor).Size = new Size(163, 24);
		((Control)colorBoxForeColor).TabIndex = 7;
		((Control)colorBoxForeColor).TabStop = false;
		colorBoxForeColor.SelectedColorChanged += ColorBoxForeColor_SelectedColorChanged;
		((Control)panelText).Controls.Add((Control)(object)panelExpression);
		((Control)panelText).Controls.Add((Control)(object)labelText);
		((Control)panelText).Dock = (DockStyle)1;
		((Control)panelText).Location = new Point(0, 181);
		((Control)panelText).Name = "panelText";
		((Control)panelText).Size = new Size(394, 38);
		((Control)panelText).TabIndex = 12;
		((Control)panelExpression).Anchor = (AnchorStyles)13;
		((Control)panelExpression).BackColor = Color.White;
		((Control)panelExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelExpression).Controls.Add((Control)(object)buttonExpression);
		((Control)panelExpression).Location = new Point(149, 6);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Size = new Size(163, 26);
		((Control)panelExpression).TabIndex = 9;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(131, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxText_TextChanged;
		((Control)buttonExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(138, 1);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(24, 24);
		((Control)buttonExpression).TabIndex = 1;
		((Control)buttonExpression).TabStop = false;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Click += ButtonExpression_Click;
		((Control)labelText).ForeColor = Color.DimGray;
		((Control)labelText).Location = new Point(12, 8);
		((Control)labelText).Name = "labelText";
		((Control)labelText).Size = new Size(130, 23);
		((Control)labelText).TabIndex = 8;
		((Control)labelText).Text = "Text";
		((Label)labelText).TextAlign = (ContentAlignment)16;
		((Control)panelBorderStyle).Controls.Add((Control)(object)labelStyle);
		((Control)panelBorderStyle).Controls.Add((Control)(object)buttonBorderStyle);
		((Control)panelBorderStyle).Dock = (DockStyle)1;
		((Control)panelBorderStyle).Location = new Point(0, 146);
		((Control)panelBorderStyle).Name = "panelBorderStyle";
		((Control)panelBorderStyle).Size = new Size(394, 35);
		((Control)panelBorderStyle).TabIndex = 12;
		((Control)labelStyle).ForeColor = Color.DimGray;
		((Control)labelStyle).Location = new Point(12, 6);
		((Control)labelStyle).Name = "labelStyle";
		((Control)labelStyle).Size = new Size(130, 23);
		((Control)labelStyle).TabIndex = 8;
		((Control)labelStyle).Text = "Style";
		((Label)labelStyle).TextAlign = (ContentAlignment)16;
		((Control)buttonBorderStyle).AccessibleRole = (AccessibleRole)43;
		buttonBorderStyle.AutoExpandOnClick = true;
		buttonBorderStyle.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonBorderStyle).Location = new Point(149, 6);
		buttonBorderStyle.LockClosePopup = false;
		((Control)buttonBorderStyle).Margin = new Padding(4);
		((Control)buttonBorderStyle).Name = "buttonBorderStyle";
		buttonBorderStyle.PopupSide = ePopupSide.Bottom;
		buttonBorderStyle.ShowArrow = false;
		((Control)buttonBorderStyle).Size = new Size(163, 23);
		buttonBorderStyle.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonBorderStyle.SubItems.AddRange(new BaseItem[6] { buttonSolid, buttonDash, buttonDashDot, buttonDashDotDot, buttonDot, buttonNone });
		((Control)buttonBorderStyle).TabIndex = 9;
		((Control)buttonBorderStyle).Text = "buttonX1";
		buttonBorderStyle.TextAlignment = eButtonTextAlignment.Left;
		buttonSolid.GlobalItem = false;
		buttonSolid.ImagePaddingHorizontal = 16;
		buttonSolid.ImagePaddingVertical = 12;
		buttonSolid.Name = "buttonSolid";
		buttonSolid.Text = "Solid";
		buttonSolid.Click += ButtonTypeSelected_Click;
		buttonDash.GlobalItem = false;
		buttonDash.ImagePaddingHorizontal = 16;
		buttonDash.ImagePaddingVertical = 12;
		buttonDash.Name = "buttonDash";
		buttonDash.Text = "Dash";
		buttonDash.Click += ButtonTypeSelected_Click;
		buttonDashDot.GlobalItem = false;
		buttonDashDot.ImagePaddingHorizontal = 16;
		buttonDashDot.ImagePaddingVertical = 12;
		buttonDashDot.Name = "buttonDashDot";
		buttonDashDot.Text = "Dash Dot";
		buttonDashDot.Click += ButtonTypeSelected_Click;
		buttonDashDotDot.GlobalItem = false;
		buttonDashDotDot.ImagePaddingHorizontal = 16;
		buttonDashDotDot.ImagePaddingVertical = 12;
		buttonDashDotDot.Name = "buttonDashDotDot";
		buttonDashDotDot.Text = "Dash Dot Dot";
		buttonDashDotDot.Click += ButtonTypeSelected_Click;
		buttonDot.GlobalItem = false;
		buttonDot.ImagePaddingHorizontal = 16;
		buttonDot.ImagePaddingVertical = 12;
		buttonDot.Name = "buttonDot";
		buttonDot.Text = "Dot";
		buttonDot.Click += ButtonTypeSelected_Click;
		buttonNone.GlobalItem = false;
		buttonNone.ImagePaddingHorizontal = 16;
		buttonNone.ImagePaddingVertical = 12;
		buttonNone.Name = "buttonNone";
		buttonNone.Text = "None";
		buttonNone.Click += ButtonTypeSelected_Click;
		((Control)panelBorderColor).Controls.Add((Control)(object)labelBorderColor);
		((Control)panelBorderColor).Controls.Add((Control)(object)colorBoxBorderColor);
		((Control)panelBorderColor).Dock = (DockStyle)1;
		((Control)panelBorderColor).Location = new Point(0, 111);
		((Control)panelBorderColor).Name = "panelBorderColor";
		((Control)panelBorderColor).Size = new Size(394, 35);
		((Control)panelBorderColor).TabIndex = 12;
		((Control)labelBorderColor).ForeColor = Color.DimGray;
		((Control)labelBorderColor).Location = new Point(12, 6);
		((Control)labelBorderColor).Name = "labelBorderColor";
		((Control)labelBorderColor).Size = new Size(130, 24);
		((Control)labelBorderColor).TabIndex = 6;
		((Control)labelBorderColor).Text = "Border Color";
		((Label)labelBorderColor).TextAlign = (ContentAlignment)16;
		((Control)colorBoxBorderColor).AccessibleRole = (AccessibleRole)43;
		colorBoxBorderColor.AutoExpandOnClick = true;
		colorBoxBorderColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxBorderColor.ImagePosition = eImagePosition.Right;
		((Control)colorBoxBorderColor).Location = new Point(149, 6);
		colorBoxBorderColor.LockClosePopup = false;
		((Control)colorBoxBorderColor).Name = "colorBoxBorderColor";
		((Control)colorBoxBorderColor).Size = new Size(163, 24);
		((Control)colorBoxBorderColor).TabIndex = 7;
		((Control)colorBoxBorderColor).TabStop = false;
		colorBoxBorderColor.SelectedColorChanged += ColorBoxBorderColor_SelectedColorChanged;
		((Control)panelSize).Controls.Add((Control)(object)labelSize);
		((Control)panelSize).Controls.Add((Control)(object)textBoxSize);
		((Control)panelSize).Dock = (DockStyle)1;
		((Control)panelSize).Location = new Point(0, 76);
		((Control)panelSize).Name = "panelSize";
		((Control)panelSize).Size = new Size(394, 35);
		((Control)panelSize).TabIndex = 12;
		((Control)labelSize).ForeColor = Color.DimGray;
		((Control)labelSize).Location = new Point(12, 6);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(130, 20);
		((Control)labelSize).TabIndex = 4;
		((Control)labelSize).Text = "Size";
		((Label)labelSize).TextAlign = (ContentAlignment)16;
		((Control)textBoxSize).BackColor = Color.White;
		textBoxSize.DecimalPlaces = 0;
		((Control)textBoxSize).Location = new Point(149, 6);
		((Control)textBoxSize).Name = "textBoxSize";
		((Control)textBoxSize).Size = new Size(57, 21);
		((Control)textBoxSize).TabIndex = 5;
		((Control)textBoxSize).Enter += TextBoxSize_Enter;
		((Control)textBoxSize).KeyDown += new KeyEventHandler(TextBoxSize_KeyDown);
		((Control)textBoxSize).Leave += TextBoxSize_Leave;
		((Control)panelDirection).Controls.Add((Control)(object)labelDirection);
		((Control)panelDirection).Controls.Add((Control)(object)buttonDirection);
		((Control)panelDirection).Dock = (DockStyle)1;
		((Control)panelDirection).Location = new Point(0, 41);
		((Control)panelDirection).Name = "panelDirection";
		((Control)panelDirection).Size = new Size(394, 35);
		((Control)panelDirection).TabIndex = 12;
		((Control)labelDirection).ForeColor = Color.DimGray;
		((Control)labelDirection).Location = new Point(12, 6);
		((Control)labelDirection).Name = "labelDirection";
		((Control)labelDirection).Size = new Size(130, 23);
		((Control)labelDirection).TabIndex = 2;
		((Control)labelDirection).Text = "Direction";
		((Label)labelDirection).TextAlign = (ContentAlignment)16;
		((Control)buttonDirection).AccessibleRole = (AccessibleRole)43;
		buttonDirection.AutoExpandOnClick = true;
		buttonDirection.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonDirection).Location = new Point(149, 6);
		buttonDirection.LockClosePopup = false;
		((Control)buttonDirection).Margin = new Padding(4);
		((Control)buttonDirection).Name = "buttonDirection";
		buttonDirection.PopupSide = ePopupSide.Bottom;
		buttonDirection.ShowArrow = false;
		((Control)buttonDirection).Size = new Size(163, 23);
		buttonDirection.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonDirection.SubItems.AddRange(new BaseItem[4] { buttonDirectionUp, buttonDirectionDown, buttonDirectionLeft, buttonDirectionRight });
		((Control)buttonDirection).TabIndex = 3;
		((Control)buttonDirection).Text = "buttonX1";
		buttonDirection.TextAlignment = eButtonTextAlignment.Left;
		buttonDirectionUp.GlobalItem = false;
		buttonDirectionUp.Name = "buttonDirectionUp";
		buttonDirectionUp.Text = "Up";
		buttonDirectionUp.Click += ButtonDirectionSelected_Click;
		buttonDirectionDown.GlobalItem = false;
		buttonDirectionDown.Name = "buttonDirectionDown";
		buttonDirectionDown.Text = "Down";
		buttonDirectionDown.Click += ButtonDirectionSelected_Click;
		buttonDirectionLeft.GlobalItem = false;
		buttonDirectionLeft.Name = "buttonDirectionLeft";
		buttonDirectionLeft.Text = "Left";
		buttonDirectionLeft.Click += ButtonDirectionSelected_Click;
		buttonDirectionRight.GlobalItem = false;
		buttonDirectionRight.Name = "buttonDirectionRight";
		buttonDirectionRight.Text = "Right";
		buttonDirectionRight.Click += ButtonDirectionSelected_Click;
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)buttonType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(0, 0);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(394, 41);
		((Control)panelType).TabIndex = 12;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 12);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(130, 23);
		((Control)labelType).TabIndex = 0;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)buttonType).AccessibleRole = (AccessibleRole)43;
		buttonType.AutoExpandOnClick = true;
		buttonType.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonType).Location = new Point(149, 12);
		buttonType.LockClosePopup = false;
		((Control)buttonType).Margin = new Padding(4);
		((Control)buttonType).Name = "buttonType";
		buttonType.PopupSide = ePopupSide.Bottom;
		buttonType.ShowArrow = true;
		((Control)buttonType).Size = new Size(163, 23);
		buttonType.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonType).TabIndex = 1;
		((Control)buttonType).Text = "buttonX1";
		buttonType.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonType).Click += ButtonType_Click;
		imageListPenStyle.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyle.ImageStream");
		imageListPenStyle.TransparentColor = Color.Transparent;
		imageListPenStyle.Images.SetKeyName(0, "Dash.png");
		imageListPenStyle.Images.SetKeyName(1, "DashDot.png");
		imageListPenStyle.Images.SetKeyName(2, "DashDotDot.png");
		imageListPenStyle.Images.SetKeyName(3, "Dot.png");
		imageListPenStyle.Images.SetKeyName(4, "Double.png");
		imageListPenStyle.Images.SetKeyName(5, "None.png");
		imageListPenStyle.Images.SetKeyName(6, "Solid.png");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(226, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(307, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)footerPanel).BackColor = Color.White;
		((Control)footerPanel).Controls.Add((Control)(object)btOk);
		((Control)footerPanel).Controls.Add((Control)(object)btCancel);
		((Control)footerPanel).Dock = (DockStyle)2;
		footerPanel.LinePosition = StiLinePosition.Top;
		footerPanel.LineStyle = StiLineStyle.Dot;
		((Control)footerPanel).Location = new Point(0, 454);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(394, 46);
		((Control)footerPanel).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(394, 500);
		((Control)this).Controls.Add((Control)(object)panelData);
		((Control)this).Controls.Add((Control)(object)footerPanel);
		((Control)this).Controls.Add((Control)(object)imageSample);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiShapeEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiShapeEditorForm";
		((Form)this).Load += This_Load;
		((ISupportInitialize)imageSample).EndInit();
		((Control)panelData).ResumeLayout(false);
		((Control)panelBrush).ResumeLayout(false);
		((Control)panelFont).ResumeLayout(false);
		((Control)panelBackgroundColor).ResumeLayout(false);
		((Control)panelForeColor).ResumeLayout(false);
		((Control)panelText).ResumeLayout(false);
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelExpression).PerformLayout();
		((Control)panelBorderStyle).ResumeLayout(false);
		((Control)panelBorderColor).ResumeLayout(false);
		((Control)panelSize).ResumeLayout(false);
		((ISupportInitialize)textBoxSize).EndInit();
		((Control)panelDirection).ResumeLayout(false);
		((Control)panelType).ResumeLayout(false);
		((Control)footerPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
