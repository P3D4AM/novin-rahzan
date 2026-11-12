using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiShapeElementEditorForm : StiElementEditorForm
{
	private StiPopup shapePopup;

	private IContainer components;

	private StiLabelUX labelType;

	private StiLabelUX labelSize;

	private StiLabelUX labelStroke;

	private StiLabelUX labelFill;

	private StiTypesButton buttonType;

	private StiTextBox textBoxSize;

	private ColorPickerButton colorBoxBorderColor;

	private StiBrushControl brushControl;

	internal StiShapeElement ShapeElement => base.Element as StiShapeElement;

	public override bool AllowResize => false;

	public StiShapeElementEditorForm()
		: this(null)
	{
	}

	public StiShapeElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		LocalizeInternal();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiShape");
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type");
		((Control)labelSize).Text = StiLocalization.Get("PropertyMain", "Size");
		((Control)labelStroke).Text = StiLocalization.Get("PropertyMain", "BorderColor");
		((Control)labelFill).Text = StiLocalization.Get("PropertyMain", "Fill");
	}

	private void DoScaling()
	{
		((Control)textBoxSize).AutoSize = false;
		((Control)textBoxSize).Size = new Size(((Control)textBoxSize).Width, StiScale.I(20));
		((Control)brushControl).Location = new Point(StiScale.XXI(10.0), StiScale.YYI(132.0));
		((Control)brushControl).Size = new Size(StiScale.XXI(241.0), StiScale.YYI(58.0));
		((Control)colorBoxBorderColor).Width = ((Control)buttonType).Width;
	}

	public override void ReadPropertiesFromElement()
	{
		base.LockRefresh = true;
		colorBoxBorderColor.SelectedColor = ShapeElement.Stroke;
		((Control)textBoxSize).Text = ShapeElement.Size.ToString();
		brushControl.Brush = ShapeElement.Fill;
		base.LockRefresh = false;
		RefreshButtonType();
	}

	private void RefreshButtonType()
	{
		StiArrowShapeType stiArrowShapeType = ShapeElement.ShapeType as StiArrowShapeType;
		string text = ShapeElement.ShapeType.GetType().Name;
		if (stiArrowShapeType != null && stiArrowShapeType.Direction != 0)
		{
			text += stiArrowShapeType.Direction;
		}
		Image image = buttonType.Image;
		buttonType.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + text + StiScale.StepName + ".png");
		if (image != null)
		{
			image.Dispose();
		}
		Size size = TextRenderer.MeasureText(ShapeElement.ShapeType.ServiceName, ((Control)buttonType).Font);
		if (size.Width > StiScale.XXI(100.0))
		{
			float num = (float)size.Width / (float)ShapeElement.ShapeType.ServiceName.Length;
			int length = (int)((float)StiScale.XXI(100.0) / num);
			((Control)buttonType).Text = ShapeElement.ShapeType.ServiceName.Substring(0, length) + "...";
		}
		else
		{
			((Control)buttonType).Text = ShapeElement.ShapeType.ServiceName;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		base.OnLoad(e);
		ReadPropertiesFromElement();
		StiDesignerControl designer = ShapeElement.Report.Designer.DesignerControl as StiDesignerControl;
		StiShapeToolboxControl stiShapeToolboxControl = new StiShapeToolboxControl(designer, new MouseEventHandler(ButtonShapeType_Click), null, showBaseShapes: false, showBorder: true);
		shapePopup = new StiPopup((Control)(object)stiShapeToolboxControl);
		stiShapeToolboxControl.Build();
	}

	private void ButtonType_Click(object sender, EventArgs e)
	{
		ButtonX buttonX = sender as ButtonX;
		((ToolStripDropDown)shapePopup).Show(new Point(((Control)buttonX).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonX).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonX).Height));
	}

	private void ButtonShapeType_Click(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			StiShape stiShape = val.Tag as StiShape;
			ShapeElement.ShapeType = stiShape.ShapeType.Clone() as StiShapeTypeService;
			RefreshElementView();
			RefreshButtonType();
		}
		((Control)shapePopup).Hide();
	}

	private void TextBoxSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			if (float.TryParse(((Control)textBoxSize).Text, out var result))
			{
				ShapeElement.Size = result;
				RefreshElementView();
			}
			else
			{
				((Control)textBoxSize).Text = ShapeElement.Size.ToString();
			}
		}
	}

	private void TextBoxSize_Leave(object sender, EventArgs e)
	{
		if (float.TryParse(((Control)textBoxSize).Text, out var result))
		{
			ShapeElement.Size = result;
			RefreshElementView();
		}
		else
		{
			((Control)textBoxSize).Text = ShapeElement.Size.ToString();
		}
	}

	private void ColorBoxBorderColor_SelectedColorChanged(object sender, EventArgs e)
	{
		ShapeElement.Stroke = colorBoxBorderColor.SelectedColor;
		RefreshElementView();
	}

	private void BrushControl_ClientSizeChanged(object sender, EventArgs e)
	{
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, ((Control)brushControl).Bottom + StiScale.I12);
	}

	private void BrushControl_BrushChanged(object sender, EventArgs e)
	{
		ShapeElement.Fill = brushControl.Brush;
		RefreshElementView();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_shape.htm");
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
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		textBoxSize = new StiTextBox();
		labelFill = new StiLabelUX();
		labelStroke = new StiLabelUX();
		labelSize = new StiLabelUX();
		labelType = new StiLabelUX();
		brushControl = new StiBrushControl();
		colorBoxBorderColor = new ColorPickerButton();
		buttonType = new StiTypesButton();
		((Control)this).SuspendLayout();
		((Control)textBoxSize).Location = new Point(106, 47);
		((Control)textBoxSize).Name = "textBoxSize";
		((Control)textBoxSize).Size = new Size(72, 21);
		((Control)textBoxSize).TabIndex = 1;
		((Control)textBoxSize).KeyDown += new KeyEventHandler(TextBoxSize_KeyDown);
		((Control)textBoxSize).Leave += TextBoxSize_Leave;
		((Control)labelFill).Location = new Point(12, 109);
		((Control)labelFill).Margin = new Padding(2, 0, 2, 0);
		((Control)labelFill).Name = "labelFill";
		((Control)labelFill).Size = new Size(82, 20);
		((Control)labelFill).TabIndex = 7;
		((Control)labelFill).Text = "Fill";
		((Label)labelFill).TextAlign = (ContentAlignment)16;
		((Control)labelStroke).Location = new Point(12, 79);
		((Control)labelStroke).Margin = new Padding(2, 0, 2, 0);
		((Control)labelStroke).Name = "labelStroke";
		((Control)labelStroke).Size = new Size(86, 20);
		((Control)labelStroke).TabIndex = 5;
		((Control)labelStroke).Text = "Stroke";
		((Label)labelStroke).TextAlign = (ContentAlignment)16;
		((Control)labelSize).Location = new Point(12, 47);
		((Control)labelSize).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(86, 21);
		((Control)labelSize).TabIndex = 4;
		((Control)labelSize).Text = "Size";
		((Label)labelSize).TextAlign = (ContentAlignment)16;
		((Control)labelType).Location = new Point(12, 14);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(86, 21);
		((Control)labelType).TabIndex = 3;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		brushControl.Brush = null;
		((Control)brushControl).Location = new Point(10, 132);
		((Control)brushControl).Margin = new Padding(6);
		((Control)brushControl).Name = "brushControl";
		((Control)brushControl).Padding = new Padding(1);
		((Control)brushControl).Size = new Size(241, 58);
		((Control)brushControl).TabIndex = 5;
		brushControl.BrushChanged += BrushControl_BrushChanged;
		((Control)brushControl).ClientSizeChanged += BrushControl_ClientSizeChanged;
		((Control)colorBoxBorderColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxBorderColor).Anchor = (AnchorStyles)13;
		colorBoxBorderColor.AutoExpandOnClick = true;
		colorBoxBorderColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxBorderColor.EnableMarkup = false;
		((Control)colorBoxBorderColor).Location = new Point(106, 79);
		colorBoxBorderColor.LockClosePopup = false;
		((Control)colorBoxBorderColor).Name = "colorBoxBorderColor";
		((Control)colorBoxBorderColor).Size = new Size(139, 22);
		((Control)colorBoxBorderColor).TabIndex = 3;
		((Control)colorBoxBorderColor).TabStop = false;
		colorBoxBorderColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxBorderColor.SelectedColorChanged += ColorBoxBorderColor_SelectedColorChanged;
		((Control)buttonType).AccessibleRole = (AccessibleRole)43;
		((Control)buttonType).Anchor = (AnchorStyles)13;
		buttonType.AutoExpandOnClick = true;
		buttonType.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonType).Location = new Point(106, 12);
		buttonType.LockClosePopup = false;
		((Control)buttonType).Margin = new Padding(4);
		((Control)buttonType).Name = "buttonType";
		buttonType.PopupSide = ePopupSide.Bottom;
		buttonType.ShowArrow = true;
		((Control)buttonType).Size = new Size(139, 23);
		buttonType.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonType).TabIndex = 0;
		((Control)buttonType).Text = "buttonX1";
		buttonType.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonType).Click += ButtonType_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(258, 195);
		((Control)this).Controls.Add((Control)(object)brushControl);
		((Control)this).Controls.Add((Control)(object)colorBoxBorderColor);
		((Control)this).Controls.Add((Control)(object)textBoxSize);
		((Control)this).Controls.Add((Control)(object)buttonType);
		((Control)this).Controls.Add((Control)(object)labelFill);
		((Control)this).Controls.Add((Control)(object)labelStroke);
		((Control)this).Controls.Add((Control)(object)labelSize);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Form)this).Margin = new Padding(3);
		((Control)this).Name = "StiShapeElementEditorForm";
		((Control)this).Padding = new Padding(12);
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).ResumeLayout(false);
	}
}
