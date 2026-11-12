using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Base.Drawing.Design;

[ToolboxItem(false)]
public class StiSimpleBorderEditorForm : Form
{
	private IContainer components;

	private ToolTip toolTip1;

	private ItemPanel itemPanel1;

	private ButtonItem biNone;

	private ButtonItem biSolid;

	private ButtonItem biDash;

	private ButtonItem biDashDot;

	private ButtonItem biDashDotDot;

	private ButtonItem biDot;

	private ButtonItem biDouble;

	private ButtonX buttonNone;

	private ButtonX buttonAll;

	private Label labelColor;

	private Label labelSize;

	private ColorPickerButton colorPickerColor;

	private ButtonX biSize;

	private ButtonItem biSize1;

	private ButtonItem biSize2;

	private ButtonItem biSize3;

	private ButtonItem biSize4;

	private ButtonItem biSize5;

	private ImageList imageListPenStyle;

	private ImageList imageListPenStyleRotated;

	private ButtonX buttonLeft;

	private ButtonX buttonRight;

	private ButtonX buttonBottom;

	private ButtonX buttonTop;

	private Panel panel2;

	private ImageList imageListShadowSizes;

	private Button btCancel;

	private Button btOk;

	private StiOffice2013GroupBox groupSides;

	private StiOffice2013GroupBox groupPenStyle;

	private StiGroupLine stiGroupLine1;

	private StiOffice2013GroupBox gbBorderColor;

	public StiSimpleBorder Border { get; private set; }

	public StiSimpleBorderEditorForm(StiSimpleBorder border)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Border = border;
			Localize();
			DoScaling();
			biSize1.Tag = 1.0;
			biSize2.Tag = 2.0;
			biSize3.Tag = 3.0;
			biSize4.Tag = 4.0;
			biSize5.Tag = 5.0;
			SetBorder();
		}
	}

	private void Localize()
	{
		biSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		biDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		biDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		biDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		biDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		biDouble.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDouble");
		biNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		toolTip1.SetToolTip((Control)(object)buttonAll, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesAll")));
		toolTip1.SetToolTip((Control)(object)buttonNone, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesNone")));
		toolTip1.SetToolTip((Control)(object)buttonLeft, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft")));
		toolTip1.SetToolTip((Control)(object)buttonTop, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesTop")));
		toolTip1.SetToolTip((Control)(object)buttonRight, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesRight")));
		toolTip1.SetToolTip((Control)(object)buttonBottom, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom")));
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)gbBorderColor).Text = StiLocalization.Get("PropertyMain", "Border");
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Border");
		((Control)groupSides).Text = StiLocalization.Get("PropertyMain", "Sides");
		((Control)groupPenStyle).Text = StiLocalization.Get("PropertyMain", "Style");
		((Control)labelSize).Text = StiLocalization.Get("PropertyMain", "Size");
		((Control)labelColor).Text = StiLocalization.Get("PropertyMain", "Color");
	}

	private void DoScaling()
	{
		buttonAll.Image = StiImageResizer.GetImage(buttonAll.Image);
		buttonNone.Image = StiImageResizer.GetImage(buttonNone.Image);
		biSolid.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false);
		biDash.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false);
		biDashDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false);
		biDashDotDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false);
		biDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false);
		biDouble.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Double.png"], allowDispose: false);
		biNone.Image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
		biSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		biSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		biSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		biSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		biSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
	}

	private static int GetSizeIndex(double size)
	{
		if (size == 1.0)
		{
			return 0;
		}
		if (size == 2.0)
		{
			return 1;
		}
		if (size == 3.0)
		{
			return 2;
		}
		if (size == 4.0)
		{
			return 3;
		}
		if (size == 5.0)
		{
			return 4;
		}
		return -1;
	}

	private StiSimpleBorder GetBorder()
	{
		StiSimpleBorder stiSimpleBorder = new StiSimpleBorder();
		stiSimpleBorder.Color = colorPickerColor.SelectedColor;
		if (((Control)biSize).Tag is double)
		{
			stiSimpleBorder.Size = (double)((Control)biSize).Tag;
		}
		if (biSolid.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.Solid;
		}
		if (biDash.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.Dash;
		}
		if (biDashDot.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.DashDot;
		}
		if (biDashDotDot.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.DashDotDot;
		}
		if (biDot.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.Dot;
		}
		if (biDouble.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.Double;
		}
		if (biNone.Checked)
		{
			stiSimpleBorder.Style = StiPenStyle.None;
		}
		StiBorderSides stiBorderSides = StiBorderSides.None;
		if (buttonTop.Checked)
		{
			stiBorderSides |= StiBorderSides.Top;
		}
		if (buttonBottom.Checked)
		{
			stiBorderSides |= StiBorderSides.Bottom;
		}
		if (buttonLeft.Checked)
		{
			stiBorderSides |= StiBorderSides.Left;
		}
		if (buttonRight.Checked)
		{
			stiBorderSides |= StiBorderSides.Right;
		}
		stiSimpleBorder.Side = stiBorderSides;
		return stiSimpleBorder;
	}

	public void SetBorder()
	{
		if (Border != null)
		{
			buttonTop.Checked = (Border.Side & StiBorderSides.Top) > StiBorderSides.None;
			buttonBottom.Checked = (Border.Side & StiBorderSides.Bottom) > StiBorderSides.None;
			buttonLeft.Checked = (Border.Side & StiBorderSides.Left) > StiBorderSides.None;
			buttonRight.Checked = (Border.Side & StiBorderSides.Right) > StiBorderSides.None;
			biSolid.Checked = Border.Style == StiPenStyle.Solid;
			biNone.Checked = Border.Style == StiPenStyle.None;
			biDash.Checked = Border.Style == StiPenStyle.Dash;
			biDashDot.Checked = Border.Style == StiPenStyle.DashDot;
			biDashDotDot.Checked = Border.Style == StiPenStyle.DashDotDot;
			biDot.Checked = Border.Style == StiPenStyle.Dot;
			biDouble.Checked = Border.Style == StiPenStyle.Double;
			int sizeIndex = GetSizeIndex(Border.Size);
			if (sizeIndex != -1)
			{
				biSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
			}
			((Control)biSize).Tag = Border.Size;
			colorPickerColor.SelectedColor = Border.Color;
		}
	}

	private void ResetPenStyle()
	{
		biSolid.Checked = false;
		biDash.Checked = false;
		biDashDot.Checked = false;
		biDashDotDot.Checked = false;
		biDot.Checked = false;
		biDouble.Checked = false;
		biNone.Checked = false;
	}

	private void ButtonTop_Click(object sender, EventArgs e)
	{
		((Control)panel2).Invalidate();
	}

	private void ButtonBottom_Click(object sender, EventArgs e)
	{
		((Control)panel2).Invalidate();
	}

	private void ButtonLeft_Click(object sender, EventArgs e)
	{
		((Control)panel2).Invalidate();
	}

	private void ButtonRight_Click(object sender, EventArgs e)
	{
		((Control)panel2).Invalidate();
	}

	private void ButtonAll_Click(object sender, EventArgs e)
	{
		buttonTop.Checked = true;
		buttonBottom.Checked = true;
		buttonLeft.Checked = true;
		buttonRight.Checked = true;
		((Control)panel2).Invalidate();
	}

	private void ButtonNone_Click(object sender, EventArgs e)
	{
		buttonTop.Checked = false;
		buttonBottom.Checked = false;
		buttonLeft.Checked = false;
		buttonRight.Checked = false;
		((Control)panel2).Invalidate();
	}

	private void ButtonSolid_Click(object sender, EventArgs e)
	{
		ResetPenStyle();
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.Checked = true;
		((Control)panel2).Invalidate();
	}

	private void Panel2_Paint(object sender, PaintEventArgs e)
	{
		Rectangle clientRectangle = ((Control)panel2).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		e.Graphics.DrawRectangle(Pens.LightGray, clientRectangle);
		StiSimpleBorder border = GetBorder();
		RectangleD rect = RectangleD.CreateFromRectangle(((Control)panel2).ClientRectangle);
		rect.Inflate(-10.0, -10.0);
		border.Draw(e.Graphics, rect, 1.0);
	}

	private void ButtonSize_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)biSize).Tag is double)
		{
			biSize1.Checked = (double)((Control)biSize).Tag == 1.0;
			biSize2.Checked = (double)((Control)biSize).Tag == 2.0;
			biSize3.Checked = (double)((Control)biSize).Tag == 3.0;
			biSize4.Checked = (double)((Control)biSize).Tag == 4.0;
			biSize5.Checked = (double)((Control)biSize).Tag == 5.0;
		}
	}

	private void ButtonSize_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		((Control)biSize).Tag = (double)buttonItem.Tag;
		double size = (double)((Control)biSize).Tag;
		int sizeIndex = GetSizeIndex(size);
		if (sizeIndex != -1)
		{
			biSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
		}
		((Control)panel2).Invalidate();
	}

	private void ColorPickerColor_SelectedColorChanged(object sender, EventArgs e)
	{
		((Control)panel2).Invalidate();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Border = GetBorder();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_appearance_borders_simple_borders.htm");
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091f: Expected O, but got Unknown
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Expected O, but got Unknown
		//IL_1333: Unknown result type (might be due to invalid IL or missing references)
		//IL_133d: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSimpleBorderEditorForm));
		imageListPenStyle = new ImageList(components);
		toolTip1 = new ToolTip(components);
		imageListPenStyleRotated = new ImageList(components);
		imageListShadowSizes = new ImageList(components);
		btCancel = new Button();
		btOk = new Button();
		groupSides = new StiOffice2013GroupBox();
		panel2 = new Panel();
		buttonRight = new ButtonX();
		buttonBottom = new ButtonX();
		buttonTop = new ButtonX();
		buttonLeft = new ButtonX();
		buttonAll = new ButtonX();
		buttonNone = new ButtonX();
		groupPenStyle = new StiOffice2013GroupBox();
		itemPanel1 = new ItemPanel();
		biSolid = new ButtonItem();
		biDash = new ButtonItem();
		biDashDot = new ButtonItem();
		biDashDotDot = new ButtonItem();
		biDot = new ButtonItem();
		biDouble = new ButtonItem();
		biNone = new ButtonItem();
		gbBorderColor = new StiOffice2013GroupBox();
		biSize = new ButtonX();
		biSize1 = new ButtonItem();
		biSize2 = new ButtonItem();
		biSize3 = new ButtonItem();
		biSize4 = new ButtonItem();
		biSize5 = new ButtonItem();
		colorPickerColor = new ColorPickerButton();
		labelSize = new Label();
		labelColor = new Label();
		stiGroupLine1 = new StiGroupLine();
		((Control)groupSides).SuspendLayout();
		((Control)groupPenStyle).SuspendLayout();
		((Control)gbBorderColor).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		imageListPenStyle.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyle.ImageStream");
		imageListPenStyle.TransparentColor = Color.Transparent;
		imageListPenStyle.Images.SetKeyName(0, "Dash.png");
		imageListPenStyle.Images.SetKeyName(1, "DashDot.png");
		imageListPenStyle.Images.SetKeyName(2, "DashDotDot.png");
		imageListPenStyle.Images.SetKeyName(3, "Dot.png");
		imageListPenStyle.Images.SetKeyName(4, "Double.png");
		imageListPenStyle.Images.SetKeyName(5, "None.png");
		imageListPenStyle.Images.SetKeyName(6, "Solid.png");
		imageListPenStyleRotated.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyleRotated.ImageStream");
		imageListPenStyleRotated.TransparentColor = Color.Transparent;
		imageListPenStyleRotated.Images.SetKeyName(0, "Solid_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(1, "Dash_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(2, "DashDot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(3, "DashDotDot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(4, "Dot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(5, "Double_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(6, "None_Rotated.png");
		imageListShadowSizes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListShadowSizes.ImageStream");
		imageListShadowSizes.TransparentColor = Color.Transparent;
		imageListShadowSizes.Images.SetKeyName(0, "Size1.png");
		imageListShadowSizes.Images.SetKeyName(1, "Size2.png");
		imageListShadowSizes.Images.SetKeyName(2, "Size3.png");
		imageListShadowSizes.Images.SetKeyName(3, "Size4.png");
		imageListShadowSizes.Images.SetKeyName(4, "Size5.png");
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(299, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 25);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(219, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 25);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)groupSides).Anchor = (AnchorStyles)7;
		((Control)groupSides).Controls.Add((Control)(object)panel2);
		((Control)groupSides).Controls.Add((Control)(object)buttonRight);
		((Control)groupSides).Controls.Add((Control)(object)buttonBottom);
		((Control)groupSides).Controls.Add((Control)(object)buttonTop);
		((Control)groupSides).Controls.Add((Control)(object)buttonLeft);
		((Control)groupSides).Controls.Add((Control)(object)buttonAll);
		((Control)groupSides).Controls.Add((Control)(object)buttonNone);
		groupSides.HeaderHeight = 24;
		((Control)groupSides).Location = new Point(12, 12);
		((Control)groupSides).Name = "groupSides";
		((Control)groupSides).Size = new Size(175, 226);
		((Control)groupSides).TabIndex = 0;
		((Control)groupSides).TabStop = false;
		((Control)groupSides).Text = "Sides";
		((Control)panel2).BackColor = Color.WhiteSmoke;
		((Control)panel2).Location = new Point(40, 65);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(94, 91);
		((Control)panel2).TabIndex = 3;
		((Control)panel2).Paint += new PaintEventHandler(Panel2_Paint);
		((Control)buttonRight).AccessibleRole = (AccessibleRole)43;
		buttonRight.AutoCheckOnClick = true;
		buttonRight.ColorTable = eButtonColor.OrangeWithBackground;
		buttonRight.Is20Button = false;
		((Control)buttonRight).Location = new Point(140, 65);
		((Control)buttonRight).Name = "buttonRight";
		((Control)buttonRight).Size = new Size(22, 91);
		((Control)buttonRight).TabIndex = 4;
		((Control)buttonRight).TabStop = false;
		((Control)buttonRight).Click += ButtonRight_Click;
		((Control)buttonBottom).AccessibleRole = (AccessibleRole)43;
		buttonBottom.AutoCheckOnClick = true;
		buttonBottom.ColorTable = eButtonColor.OrangeWithBackground;
		buttonBottom.Is20Button = false;
		((Control)buttonBottom).Location = new Point(40, 162);
		((Control)buttonBottom).Name = "buttonBottom";
		((Control)buttonBottom).Size = new Size(94, 22);
		((Control)buttonBottom).TabIndex = 5;
		((Control)buttonBottom).TabStop = false;
		((Control)buttonBottom).Click += ButtonBottom_Click;
		((Control)buttonTop).AccessibleRole = (AccessibleRole)43;
		buttonTop.AutoCheckOnClick = true;
		buttonTop.ColorTable = eButtonColor.OrangeWithBackground;
		buttonTop.Is20Button = false;
		((Control)buttonTop).Location = new Point(40, 37);
		((Control)buttonTop).Name = "buttonTop";
		((Control)buttonTop).Size = new Size(94, 22);
		((Control)buttonTop).TabIndex = 1;
		((Control)buttonTop).TabStop = false;
		((Control)buttonTop).Click += ButtonTop_Click;
		((Control)buttonLeft).AccessibleRole = (AccessibleRole)43;
		buttonLeft.AutoCheckOnClick = true;
		buttonLeft.ColorTable = eButtonColor.OrangeWithBackground;
		buttonLeft.Is20Button = false;
		((Control)buttonLeft).Location = new Point(12, 65);
		((Control)buttonLeft).Name = "buttonLeft";
		((Control)buttonLeft).Size = new Size(22, 91);
		((Control)buttonLeft).TabIndex = 2;
		((Control)buttonLeft).TabStop = false;
		((Control)buttonLeft).Click += ButtonLeft_Click;
		((Control)buttonAll).AccessibleRole = (AccessibleRole)43;
		buttonAll.ColorTable = eButtonColor.OrangeWithBackground;
		buttonAll.Image = (Image)componentResourceManager.GetObject("buttonAll.Image");
		buttonAll.Is20Button = false;
		((Control)buttonAll).Location = new Point(12, 37);
		((Control)buttonAll).Name = "buttonAll";
		((Control)buttonAll).Size = new Size(22, 22);
		((Control)buttonAll).TabIndex = 0;
		((Control)buttonAll).TabStop = false;
		((Control)buttonAll).Click += ButtonAll_Click;
		((Control)buttonNone).AccessibleRole = (AccessibleRole)43;
		buttonNone.ColorTable = eButtonColor.OrangeWithBackground;
		buttonNone.Image = (Image)componentResourceManager.GetObject("buttonNone.Image");
		buttonNone.Is20Button = false;
		((Control)buttonNone).Location = new Point(140, 162);
		((Control)buttonNone).Name = "buttonNone";
		((Control)buttonNone).Size = new Size(22, 22);
		((Control)buttonNone).TabIndex = 6;
		((Control)buttonNone).TabStop = false;
		((Control)buttonNone).Click += ButtonNone_Click;
		((Control)groupPenStyle).Anchor = (AnchorStyles)15;
		((Control)groupPenStyle).Controls.Add((Control)(object)itemPanel1);
		groupPenStyle.HeaderHeight = 24;
		((Control)groupPenStyle).Location = new Point(200, 12);
		((Control)groupPenStyle).Name = "groupPenStyle";
		((Control)groupPenStyle).Size = new Size(175, 226);
		((Control)groupPenStyle).TabIndex = 1;
		((Control)groupPenStyle).TabStop = false;
		((Control)groupPenStyle).Text = "Pen Style";
		((Control)itemPanel1).Anchor = (AnchorStyles)15;
		itemPanel1.BackgroundStyle.BackColor = Color.White;
		itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
		itemPanel1.BackgroundStyle.BorderColor = Color.FromArgb(127, 157, 185);
		itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
		itemPanel1.BackgroundStyle.BorderRightWidth = 1;
		itemPanel1.BackgroundStyle.BorderTopWidth = 1;
		itemPanel1.BackgroundStyle.CornerType = eCornerType.Square;
		itemPanel1.BackgroundStyle.PaddingBottom = 1;
		itemPanel1.BackgroundStyle.PaddingLeft = 1;
		itemPanel1.BackgroundStyle.PaddingRight = 1;
		itemPanel1.BackgroundStyle.PaddingTop = 1;
		itemPanel1.ContainerControlProcessDialogKey = true;
		itemPanel1.Images = imageListPenStyle;
		itemPanel1.Items.AddRange(new BaseItem[7] { biSolid, biDash, biDashDot, biDashDotDot, biDot, biDouble, biNone });
		itemPanel1.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanel1).Location = new Point(0, 37);
		((Control)itemPanel1).Name = "itemPanel1";
		((Control)itemPanel1).Size = new Size(175, 192);
		((Control)itemPanel1).TabIndex = 0;
		((Control)itemPanel1).Text = "itemPanel1";
		biSolid.ButtonStyle = eButtonStyle.ImageAndText;
		biSolid.Checked = true;
		biSolid.ImageIndex = 6;
		biSolid.ImagePaddingHorizontal = 16;
		biSolid.ImagePaddingVertical = 12;
		biSolid.Name = "biSolid";
		biSolid.Text = "Solid";
		biSolid.Click += ButtonSolid_Click;
		biDash.ButtonStyle = eButtonStyle.ImageAndText;
		biDash.ImageIndex = 0;
		biDash.ImagePaddingHorizontal = 16;
		biDash.ImagePaddingVertical = 12;
		biDash.Name = "biDash";
		biDash.Text = "Dash";
		biDash.Click += ButtonSolid_Click;
		biDashDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDashDot.ImageIndex = 1;
		biDashDot.ImagePaddingHorizontal = 16;
		biDashDot.ImagePaddingVertical = 12;
		biDashDot.Name = "biDashDot";
		biDashDot.Text = "Dash Dot";
		biDashDot.Click += ButtonSolid_Click;
		biDashDotDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDashDotDot.ImageIndex = 2;
		biDashDotDot.ImagePaddingHorizontal = 16;
		biDashDotDot.ImagePaddingVertical = 12;
		biDashDotDot.Name = "biDashDotDot";
		biDashDotDot.Text = "Dash Dot Dot";
		biDashDotDot.Click += ButtonSolid_Click;
		biDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDot.ImageIndex = 3;
		biDot.ImagePaddingHorizontal = 16;
		biDot.ImagePaddingVertical = 12;
		biDot.Name = "biDot";
		biDot.Text = "Dot";
		biDot.Click += ButtonSolid_Click;
		biDouble.ButtonStyle = eButtonStyle.ImageAndText;
		biDouble.ImageIndex = 4;
		biDouble.ImagePaddingHorizontal = 16;
		biDouble.ImagePaddingVertical = 12;
		biDouble.Name = "biDouble";
		biDouble.Text = "Double";
		biDouble.Click += ButtonSolid_Click;
		biNone.ButtonStyle = eButtonStyle.ImageAndText;
		biNone.ImageIndex = 5;
		biNone.ImagePaddingHorizontal = 16;
		biNone.ImagePaddingVertical = 12;
		biNone.Name = "biNone";
		biNone.Text = "None";
		biNone.Click += ButtonSolid_Click;
		((Control)gbBorderColor).Anchor = (AnchorStyles)14;
		((Control)gbBorderColor).Controls.Add((Control)(object)biSize);
		((Control)gbBorderColor).Controls.Add((Control)(object)colorPickerColor);
		((Control)gbBorderColor).Controls.Add((Control)(object)labelSize);
		((Control)gbBorderColor).Controls.Add((Control)(object)labelColor);
		gbBorderColor.HeaderHeight = 24;
		((Control)gbBorderColor).Location = new Point(12, 245);
		((Control)gbBorderColor).Name = "gbBorderColor";
		((Control)gbBorderColor).Size = new Size(363, 105);
		((Control)gbBorderColor).TabIndex = 2;
		((Control)gbBorderColor).TabStop = false;
		((Control)gbBorderColor).Text = "Border";
		((Control)biSize).AccessibleRole = (AccessibleRole)43;
		biSize.AutoExpandOnClick = true;
		biSize.ColorTable = eButtonColor.OrangeWithBackground;
		biSize.Is20Button = false;
		((Control)biSize).Location = new Point(154, 38);
		((Control)biSize).Name = "biSize";
		((Control)biSize).Size = new Size(45, 24);
		biSize.SubItems.AddRange(new BaseItem[5] { biSize1, biSize2, biSize3, biSize4, biSize5 });
		((Control)biSize).TabIndex = 0;
		((Control)biSize).TabStop = false;
		biSize.PopupOpen += ButtonSize_PopupOpen;
		biSize1.GlobalItem = false;
		biSize1.ImagePaddingHorizontal = 16;
		biSize1.ImagePaddingVertical = 12;
		biSize1.Name = "biSize1";
		biSize1.Text = "1";
		biSize1.Click += ButtonSize_Click;
		biSize2.GlobalItem = false;
		biSize2.ImagePaddingHorizontal = 16;
		biSize2.ImagePaddingVertical = 12;
		biSize2.Name = "biSize2";
		biSize2.Text = "2";
		biSize2.Click += ButtonSize_Click;
		biSize3.GlobalItem = false;
		biSize3.ImagePaddingHorizontal = 16;
		biSize3.ImagePaddingVertical = 12;
		biSize3.Name = "biSize3";
		biSize3.Text = "3";
		biSize3.Click += ButtonSize_Click;
		biSize4.GlobalItem = false;
		biSize4.ImagePaddingHorizontal = 16;
		biSize4.ImagePaddingVertical = 12;
		biSize4.Name = "biSize4";
		biSize4.Text = "4";
		biSize4.Click += ButtonSize_Click;
		biSize5.GlobalItem = false;
		biSize5.ImagePaddingHorizontal = 16;
		biSize5.ImagePaddingVertical = 12;
		biSize5.Name = "biSize5";
		biSize5.Text = "5";
		biSize5.Click += ButtonSize_Click;
		((Control)colorPickerColor).AccessibleRole = (AccessibleRole)43;
		colorPickerColor.AutoExpandOnClick = true;
		colorPickerColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerColor.Image = (Image)componentResourceManager.GetObject("colorPickerColor.Image");
		colorPickerColor.ImagePosition = eImagePosition.Right;
		colorPickerColor.Is20Button = false;
		((Control)colorPickerColor).Location = new Point(154, 74);
		((Control)colorPickerColor).Name = "colorPickerColor";
		colorPickerColor.SelectedColorImageRectangle = new Rectangle(2, 2, 12, 12);
		((Control)colorPickerColor).Size = new Size(45, 24);
		((Control)colorPickerColor).TabIndex = 1;
		((Control)colorPickerColor).TabStop = false;
		colorPickerColor.SelectedColorChanged += ColorPickerColor_SelectedColorChanged;
		((Control)labelSize).ForeColor = Color.DimGray;
		((Control)labelSize).Location = new Point(12, 38);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(131, 24);
		((Control)labelSize).TabIndex = 2;
		((Control)labelSize).Text = "Size";
		labelSize.TextAlign = (ContentAlignment)16;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(12, 74);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(128, 24);
		((Control)labelColor).TabIndex = 3;
		((Control)labelColor).Text = "Color";
		labelColor.TextAlign = (ContentAlignment)16;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 358);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(386, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(386, 404);
		((Control)this).Controls.Add((Control)(object)groupSides);
		((Control)this).Controls.Add((Control)(object)groupPenStyle);
		((Control)this).Controls.Add((Control)(object)gbBorderColor);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSimpleBorderEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Border";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)groupSides).ResumeLayout(false);
		((Control)groupPenStyle).ResumeLayout(false);
		((Control)gbBorderColor).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
