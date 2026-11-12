using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class ShapeEditorForm : Form
{
	private IShapeDescriptor _Shape;

	private IContainer components;

	private ItemPanel itemPanel1;

	private ButtonItem buttonRect;

	private ButtonItem buttonRound;

	private LabelItem labelItem1;

	private ItemContainer itemContainer1;

	private Button button1;

	private Button button2;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private TabItem tabDefault;

	private ButtonItem buttonDefault;

	private TabControlPanel tabControlPanel3;

	private TabItem tabRound;

	private TabControlPanel tabControlPanel2;

	private TabItem tabRect;

	private IntegerInput roundBottomRight;

	private IntegerInput roundTopRight;

	private IntegerInput roundBottomLeft;

	private IntegerInput roundTopLeft;

	private ButtonX buttonRoundPreview;

	private ButtonItem buttonEllipse;

	private TabControlPanel tabControlPanel4;

	private TabItem tabItem1;

	public IShapeDescriptor Value
	{
		get
		{
			return _Shape;
		}
		set
		{
			_Shape = value;
			OnShapeChanged();
		}
	}

	public ShapeEditorForm()
	{
		InitializeComponent();
	}

	private void itemPanel1_OptionGroupChanging(object sender, OptionGroupChangingEventArgs e)
	{
		BaseItem parent = e.NewChecked.Parent;
		tabControl1.SelectedTabIndex = parent.SubItems.IndexOf(e.NewChecked);
	}

	private void OnShapeChanged()
	{
		if (_Shape is RoundRectangleShapeDescriptor)
		{
			RoundRectangleShapeDescriptor roundRectangleShapeDescriptor = (RoundRectangleShapeDescriptor)_Shape;
			if (roundRectangleShapeDescriptor.IsEmpty)
			{
				buttonRect.Checked = true;
			}
			else
			{
				buttonRound.Checked = true;
			}
		}
		else if (_Shape is EllipticalShapeDescriptor)
		{
			buttonEllipse.Checked = true;
		}
		else
		{
			buttonDefault.Checked = true;
		}
	}

	private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		if (e.NewTab == tabRound)
		{
			RoundRectangleShapeDescriptor roundRectangleShapeDescriptor = _Shape as RoundRectangleShapeDescriptor;
			if (roundRectangleShapeDescriptor == null)
			{
				roundRectangleShapeDescriptor = new RoundRectangleShapeDescriptor(2);
			}
			roundTopLeft.Value = roundRectangleShapeDescriptor.TopLeft;
			roundTopRight.Value = roundRectangleShapeDescriptor.TopRight;
			roundBottomLeft.Value = roundRectangleShapeDescriptor.BottomLeft;
			roundBottomRight.Value = roundRectangleShapeDescriptor.BottomRight;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (buttonDefault.Checked)
		{
			_Shape = null;
		}
		else if (buttonRect.Checked)
		{
			_Shape = new RoundRectangleShapeDescriptor(0);
		}
		else if (buttonRound.Checked)
		{
			_Shape = new RoundRectangleShapeDescriptor(roundTopLeft.Value, roundTopRight.Value, roundBottomLeft.Value, roundBottomRight.Value);
		}
		else if (buttonEllipse.Checked)
		{
			_Shape = new EllipticalShapeDescriptor();
		}
	}

	private void RoundCorner_ValueChanged(object sender, EventArgs e)
	{
		RoundRectangleShapeDescriptor shape = new RoundRectangleShapeDescriptor(roundTopLeft.Value, roundTopRight.Value, roundBottomLeft.Value, roundBottomRight.Value);
		buttonRoundPreview.Shape = shape;
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d95: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ShapeEditorForm));
		itemPanel1 = new ItemPanel();
		labelItem1 = new LabelItem();
		itemContainer1 = new ItemContainer();
		buttonDefault = new ButtonItem();
		buttonRect = new ButtonItem();
		buttonRound = new ButtonItem();
		buttonEllipse = new ButtonItem();
		button1 = new Button();
		button2 = new Button();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		tabDefault = new TabItem(components);
		tabControlPanel4 = new TabControlPanel();
		tabItem1 = new TabItem(components);
		tabControlPanel2 = new TabControlPanel();
		tabRect = new TabItem(components);
		tabControlPanel3 = new TabControlPanel();
		roundBottomRight = new IntegerInput();
		roundTopRight = new IntegerInput();
		roundBottomLeft = new IntegerInput();
		roundTopLeft = new IntegerInput();
		buttonRoundPreview = new ButtonX();
		tabRound = new TabItem(components);
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel3).SuspendLayout();
		((ISupportInitialize)roundBottomRight).BeginInit();
		((ISupportInitialize)roundTopRight).BeginInit();
		((ISupportInitialize)roundBottomLeft).BeginInit();
		((ISupportInitialize)roundTopLeft).BeginInit();
		((Control)this).SuspendLayout();
		itemPanel1.BackgroundStyle.BackColor = Color.White;
		itemPanel1.BackgroundStyle.BorderBottom = eStyleBorderType.Solid;
		itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
		itemPanel1.BackgroundStyle.BorderColor = Color.FromArgb(127, 157, 185);
		itemPanel1.BackgroundStyle.BorderLeft = eStyleBorderType.Solid;
		itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
		itemPanel1.BackgroundStyle.BorderRight = eStyleBorderType.Solid;
		itemPanel1.BackgroundStyle.BorderRightWidth = 1;
		itemPanel1.BackgroundStyle.BorderTop = eStyleBorderType.Solid;
		itemPanel1.BackgroundStyle.BorderTopWidth = 1;
		itemPanel1.BackgroundStyle.PaddingBottom = 1;
		itemPanel1.BackgroundStyle.PaddingLeft = 1;
		itemPanel1.BackgroundStyle.PaddingRight = 1;
		itemPanel1.BackgroundStyle.PaddingTop = 1;
		((Control)itemPanel1).Dock = (DockStyle)1;
		itemPanel1.Items.AddRange(new BaseItem[2] { labelItem1, itemContainer1 });
		itemPanel1.ItemSpacing = 2;
		itemPanel1.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanel1).Location = new Point(0, 0);
		((Control)itemPanel1).Name = "itemPanel1";
		((Control)itemPanel1).Size = new Size(387, 69);
		((Control)itemPanel1).TabIndex = 0;
		((Control)itemPanel1).Text = "itemPanel1";
		itemPanel1.OptionGroupChanging += itemPanel1_OptionGroupChanging;
		labelItem1.BackColor = Color.FromArgb(224, 224, 224);
		labelItem1.BorderSide = eBorderSide.Bottom;
		labelItem1.BorderType = eBorderType.SingleLine;
		labelItem1.Name = "labelItem1";
		labelItem1.PaddingBottom = 2;
		labelItem1.PaddingLeft = 2;
		labelItem1.PaddingTop = 2;
		labelItem1.SingleLineColor = Color.DarkGray;
		labelItem1.Text = "<b>Choose the shape you want to use:</b>";
		itemContainer1.Name = "itemContainer1";
		itemContainer1.SubItems.AddRange(new BaseItem[4] { buttonDefault, buttonRect, buttonRound, buttonEllipse });
		buttonDefault.AutoCheckOnClick = true;
		buttonDefault.Image = (Image)componentResourceManager.GetObject("buttonDefault.Image");
		buttonDefault.ImagePaddingHorizontal = 8;
		buttonDefault.Name = "buttonDefault";
		buttonDefault.OptionGroup = "shape";
		buttonDefault.Text = "Default";
		buttonDefault.Tooltip = "Use Default Shape";
		buttonRect.AutoCheckOnClick = true;
		buttonRect.Image = (Image)componentResourceManager.GetObject("buttonRect.Image");
		buttonRect.ImagePaddingHorizontal = 8;
		buttonRect.Name = "buttonRect";
		buttonRect.OptionGroup = "shape";
		buttonRect.Text = "Rect";
		buttonRect.Tooltip = "Rectangle Shape";
		buttonRound.AutoCheckOnClick = true;
		buttonRound.Image = (Image)componentResourceManager.GetObject("buttonRound.Image");
		buttonRound.ImagePaddingHorizontal = 8;
		buttonRound.Name = "buttonRound";
		buttonRound.OptionGroup = "shape";
		buttonRound.Text = "Round";
		buttonRound.Tooltip = "Rounded Rectangle Shape";
		buttonEllipse.AutoCheckOnClick = true;
		buttonEllipse.Image = (Image)componentResourceManager.GetObject("buttonEllipse.Image");
		buttonEllipse.ImagePaddingHorizontal = 8;
		buttonEllipse.Name = "buttonEllipse";
		buttonEllipse.OptionGroup = "shape";
		buttonEllipse.Text = "Elliptical Shape";
		buttonEllipse.Tooltip = "Elliptical Shape";
		((Control)button1).Anchor = (AnchorStyles)10;
		button1.DialogResult = (DialogResult)1;
		((Control)button1).Location = new Point(228, 165);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(75, 24);
		((Control)button1).TabIndex = 1;
		((Control)button1).Text = "OK";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((Control)button1).Click += button1_Click;
		((Control)button2).Anchor = (AnchorStyles)10;
		button2.DialogResult = (DialogResult)2;
		((Control)button2).Location = new Point(309, 165);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(75, 24);
		((Control)button2).TabIndex = 2;
		((Control)button2).Text = "Cancel";
		((ButtonBase)button2).UseVisualStyleBackColor = true;
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel4);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel2);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel3);
		((Control)tabControl1).Location = new Point(0, 70);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(387, 89);
		tabControl1.Style = eTabStripStyle.Office2007Dock;
		((Control)tabControl1).TabIndex = 3;
		tabControl1.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		tabControl1.Tabs.Add(tabDefault);
		tabControl1.Tabs.Add(tabRect);
		tabControl1.Tabs.Add(tabRound);
		tabControl1.Tabs.Add(tabItem1);
		tabControl1.TabsVisible = false;
		((Control)tabControl1).Text = "tabControl1";
		tabControl1.SelectedTabChanged += tabControl1_SelectedTabChanged;
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 22);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(387, 67);
		tabControlPanel1.Style.Alignment = (StringAlignment)1;
		tabControlPanel1.Style.BackColor1.Color = Color.White;
		tabControlPanel1.Style.BackColor2.Color = Color.White;
		tabControlPanel1.Style.Border = eBorderType.SingleLine;
		tabControlPanel1.Style.BorderColor.Color = Color.FromArgb(127, 157, 185);
		tabControlPanel1.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		tabControlPanel1.Style.ForeColor.Color = Color.DimGray;
		tabControlPanel1.Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 1;
		tabControlPanel1.TabItem = tabDefault;
		((Control)tabControlPanel1).Text = "No properties available for this shape.";
		tabControlPanel1.UseCustomStyle = true;
		tabDefault.AttachedControl = (Control)(object)tabControlPanel1;
		tabDefault.Name = "tabDefault";
		tabDefault.Text = "Default";
		((Control)tabControlPanel4).Dock = (DockStyle)5;
		((Control)tabControlPanel4).Location = new Point(0, 22);
		((Control)tabControlPanel4).Name = "tabControlPanel4";
		((Control)tabControlPanel4).Padding = new Padding(1);
		((Control)tabControlPanel4).Size = new Size(387, 67);
		tabControlPanel4.Style.Alignment = (StringAlignment)1;
		tabControlPanel4.Style.BackColor1.Color = Color.White;
		tabControlPanel4.Style.BackColor2.Color = Color.White;
		tabControlPanel4.Style.Border = eBorderType.SingleLine;
		tabControlPanel4.Style.BorderColor.Color = Color.FromArgb(146, 165, 199);
		tabControlPanel4.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		tabControlPanel4.Style.ForeColor.Color = Color.DimGray;
		tabControlPanel4.Style.GradientAngle = 90;
		((Control)tabControlPanel4).TabIndex = 4;
		tabControlPanel4.TabItem = tabItem1;
		((Control)tabControlPanel4).Text = "No properties available for this shape.";
		tabControlPanel4.UseCustomStyle = true;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel4;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "Ellipse";
		((Control)tabControlPanel2).Dock = (DockStyle)5;
		((Control)tabControlPanel2).Location = new Point(0, 22);
		((Control)tabControlPanel2).Name = "tabControlPanel2";
		((Control)tabControlPanel2).Padding = new Padding(1);
		((Control)tabControlPanel2).Size = new Size(387, 67);
		tabControlPanel2.Style.Alignment = (StringAlignment)1;
		tabControlPanel2.Style.BackColor1.Color = Color.White;
		tabControlPanel2.Style.BackColor2.Color = Color.White;
		tabControlPanel2.Style.Border = eBorderType.SingleLine;
		tabControlPanel2.Style.BorderColor.Color = Color.FromArgb(127, 157, 185);
		tabControlPanel2.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		tabControlPanel2.Style.ForeColor.Color = Color.DimGray;
		tabControlPanel2.Style.GradientAngle = 90;
		((Control)tabControlPanel2).TabIndex = 2;
		tabControlPanel2.TabItem = tabRect;
		((Control)tabControlPanel2).Text = "No properties available for this shape.";
		tabControlPanel2.UseCustomStyle = true;
		tabRect.AttachedControl = (Control)(object)tabControlPanel2;
		tabRect.Name = "tabRect";
		tabRect.Text = "Rect";
		((Control)tabControlPanel3).Controls.Add((Control)(object)roundBottomRight);
		((Control)tabControlPanel3).Controls.Add((Control)(object)roundTopRight);
		((Control)tabControlPanel3).Controls.Add((Control)(object)roundBottomLeft);
		((Control)tabControlPanel3).Controls.Add((Control)(object)roundTopLeft);
		((Control)tabControlPanel3).Controls.Add((Control)(object)buttonRoundPreview);
		((Control)tabControlPanel3).Dock = (DockStyle)5;
		((Control)tabControlPanel3).Location = new Point(0, 22);
		((Control)tabControlPanel3).Name = "tabControlPanel3";
		((Control)tabControlPanel3).Padding = new Padding(1);
		((Control)tabControlPanel3).Size = new Size(387, 67);
		tabControlPanel3.Style.BackColor1.Color = Color.White;
		tabControlPanel3.Style.BackColor2.Color = Color.White;
		tabControlPanel3.Style.Border = eBorderType.SingleLine;
		tabControlPanel3.Style.BorderColor.Color = Color.FromArgb(127, 157, 185);
		tabControlPanel3.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		tabControlPanel3.Style.GradientAngle = 90;
		((Control)tabControlPanel3).TabIndex = 3;
		tabControlPanel3.TabItem = tabRound;
		tabControlPanel3.UseCustomStyle = true;
		roundBottomRight.AllowEmptyState = false;
		roundBottomRight.BackgroundStyle.Class = "DateTimeInputBackground";
		((Control)roundBottomRight).Location = new Point(231, 29);
		roundBottomRight.MaxValue = 999;
		roundBottomRight.MinValue = 0;
		((Control)roundBottomRight).Name = "roundBottomRight";
		roundBottomRight.ShowUpDown = true;
		((Control)roundBottomRight).Size = new Size(46, 20);
		((Control)roundBottomRight).TabIndex = 4;
		roundBottomRight.ValueChanged += RoundCorner_ValueChanged;
		roundTopRight.AllowEmptyState = false;
		roundTopRight.BackgroundStyle.Class = "DateTimeInputBackground";
		((Control)roundTopRight).Location = new Point(231, 4);
		roundTopRight.MaxValue = 999;
		roundTopRight.MinValue = 0;
		((Control)roundTopRight).Name = "roundTopRight";
		roundTopRight.ShowUpDown = true;
		((Control)roundTopRight).Size = new Size(46, 20);
		((Control)roundTopRight).TabIndex = 3;
		roundTopRight.ValueChanged += RoundCorner_ValueChanged;
		roundBottomLeft.AllowEmptyState = false;
		roundBottomLeft.BackgroundStyle.Class = "DateTimeInputBackground";
		((Control)roundBottomLeft).Location = new Point(96, 29);
		roundBottomLeft.MaxValue = 999;
		roundBottomLeft.MinValue = 0;
		((Control)roundBottomLeft).Name = "roundBottomLeft";
		roundBottomLeft.ShowUpDown = true;
		((Control)roundBottomLeft).Size = new Size(46, 20);
		((Control)roundBottomLeft).TabIndex = 2;
		roundBottomLeft.ValueChanged += RoundCorner_ValueChanged;
		roundTopLeft.AllowEmptyState = false;
		roundTopLeft.BackgroundStyle.Class = "DateTimeInputBackground";
		((Control)roundTopLeft).Location = new Point(96, 4);
		roundTopLeft.MaxValue = 999;
		roundTopLeft.MinValue = 0;
		((Control)roundTopLeft).Name = "roundTopLeft";
		roundTopLeft.ShowUpDown = true;
		((Control)roundTopLeft).Size = new Size(46, 20);
		((Control)roundTopLeft).TabIndex = 1;
		roundTopLeft.ValueChanged += RoundCorner_ValueChanged;
		((Control)buttonRoundPreview).AccessibleRole = (AccessibleRole)43;
		buttonRoundPreview.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRoundPreview).Location = new Point(148, 4);
		((Control)buttonRoundPreview).Name = "buttonRoundPreview";
		buttonRoundPreview.Shape = new RoundRectangleShapeDescriptor(2);
		((Control)buttonRoundPreview).Size = new Size(77, 45);
		((Control)buttonRoundPreview).TabIndex = 0;
		((Control)buttonRoundPreview).Text = "Preview";
		tabRound.AttachedControl = (Control)(object)tabControlPanel3;
		tabRound.Name = "tabRound";
		tabRound.Text = "Round";
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Form)this).ClientSize = new Size(387, 194);
		((Control)this).Controls.Add((Control)(object)tabControl1);
		((Control)this).Controls.Add((Control)(object)button2);
		((Control)this).Controls.Add((Control)(object)button1);
		((Control)this).Controls.Add((Control)(object)itemPanel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "ShapeEditorForm";
		((Control)this).Text = "Shape Editor";
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(performLayout: false);
		((Control)tabControlPanel3).ResumeLayout(false);
		((ISupportInitialize)roundBottomRight).EndInit();
		((ISupportInitialize)roundTopRight).EndInit();
		((ISupportInitialize)roundBottomLeft).EndInit();
		((ISupportInitialize)roundTopLeft).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
