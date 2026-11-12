using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

[ToolboxItem(false)]
public class TextMarkupEditor : PanelEx
{
	public Button buttonCancel;

	public Button buttonOK;

	private Timer timer1;

	private PanelEx previewPanel;

	public TextBox inputText;

	private Label label2;

	private IContainer components;

	private Bar bar1;

	private ButtonItem buttonBold;

	private ButtonItem buttonItalic;

	private ButtonItem buttonUnderline;

	private ColorPickerDropDown buttonColor;

	private SuperTooltipControl m_Tooltip;

	public TextMarkupEditor()
	{
		InitializeComponent();
		base.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		ApplyLabelStyle();
		base.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		base.Style.Border = eBorderType.SingleLine;
		((Panel)this).TabStop = false;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		components = new Container();
		inputText = new TextBox();
		previewPanel = new PanelEx();
		buttonCancel = new Button();
		buttonOK = new Button();
		timer1 = new Timer(components);
		label2 = new Label();
		bar1 = new Bar();
		buttonBold = new ButtonItem();
		buttonItalic = new ButtonItem();
		buttonUnderline = new ButtonItem();
		buttonColor = new ColorPickerDropDown();
		((ISupportInitialize)bar1).BeginInit();
		((Control)this).SuspendLayout();
		inputText.AcceptsReturn = true;
		((Control)inputText).Anchor = (AnchorStyles)15;
		((Control)inputText).Location = new Point(8, 8);
		((TextBoxBase)inputText).Multiline = true;
		((Control)inputText).Name = "inputText";
		((Control)inputText).Size = new Size(332, 110);
		((Control)inputText).TabIndex = 0;
		((Control)inputText).TextChanged += inputText_TextChanged;
		((Control)previewPanel).Anchor = (AnchorStyles)14;
		((ScrollableControl)previewPanel).AutoScroll = true;
		previewPanel.CanvasColor = SystemColors.Control;
		((Control)previewPanel).Location = new Point(8, 152);
		((Control)previewPanel).Name = "previewPanel";
		((Control)previewPanel).Size = new Size(332, 75);
		previewPanel.Style.BackColor1.Color = Color.White;
		previewPanel.Style.BackgroundImagePosition = eBackgroundImagePosition.Tile;
		previewPanel.Style.Border = eBorderType.SingleLine;
		previewPanel.Style.BorderColor.Color = Color.DarkGray;
		previewPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		previewPanel.Style.GradientAngle = 90;
		previewPanel.Style.LineAlignment = (StringAlignment)0;
		((Control)previewPanel).TabIndex = 1;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((ButtonBase)buttonCancel).FlatStyle = (FlatStyle)3;
		((Control)buttonCancel).Location = new Point(269, 233);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(71, 24);
		((Control)buttonCancel).TabIndex = 4;
		((Control)buttonCancel).Text = "Cancel";
		((Control)buttonCancel).Click += buttonCancel_Click;
		((Control)buttonOK).Anchor = (AnchorStyles)10;
		buttonOK.DialogResult = (DialogResult)1;
		((ButtonBase)buttonOK).FlatStyle = (FlatStyle)3;
		((Control)buttonOK).Location = new Point(194, 233);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(71, 24);
		((Control)buttonOK).TabIndex = 3;
		((Control)buttonOK).Text = "OK";
		((Control)buttonOK).Click += buttonOK_Click;
		timer1.Interval = 1000;
		timer1.Tick += timer1_Tick;
		((Control)label2).Anchor = (AnchorStyles)10;
		((Control)label2).BackColor = Color.WhiteSmoke;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label2).Location = new Point(316, 133);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(24, 16);
		((Control)label2).TabIndex = 5;
		((Control)label2).Text = "?";
		label2.TextAlign = (ContentAlignment)32;
		((Control)label2).MouseLeave += label2_MouseLeave;
		((Control)label2).MouseEnter += label2_MouseEnter;
		bar1.Items.AddRange(new BaseItem[4] { buttonBold, buttonItalic, buttonUnderline, buttonColor });
		bar1.Location = new Point(8, 124);
		bar1.Name = "bar1";
		bar1.Size = new Size(174, 25);
		bar1.Style = eDotNetBarStyle.Office2007;
		bar1.TabIndex = 6;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		buttonBold.Name = "buttonBold";
		buttonBold.Shortcuts.Add(eShortcut.CtrlB);
		buttonBold.Text = "Bold";
		buttonBold.Click += buttonBold_Click;
		buttonItalic.Name = "buttonItalic";
		buttonItalic.Shortcuts.Add(eShortcut.CtrlI);
		buttonItalic.Text = "Italic";
		buttonItalic.Click += buttonItalic_Click;
		buttonUnderline.Name = "buttonUnderline";
		buttonUnderline.Shortcuts.Add(eShortcut.CtrlU);
		buttonUnderline.Text = "Underline";
		buttonUnderline.Click += buttonUnderline_Click;
		buttonColor.Name = "buttonColor";
		buttonColor.Tooltip = "Sets the text color";
		buttonColor.Image = (Image)(object)Helpers.LoadBitmap("SystemImages.ColorPickerButtonImage.png");
		buttonColor.SelectedColorImageRectangle = new Rectangle(2, 2, 12, 12);
		buttonColor.Click += buttonColor_SelectedColorChanged;
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)buttonOK);
		((Control)this).Controls.Add((Control)(object)buttonCancel);
		((Control)this).Controls.Add((Control)(object)previewPanel);
		((Control)this).Controls.Add((Control)(object)inputText);
		((Control)this).Name = "TextMarkupEditor";
		((Control)this).Size = new Size(348, 263);
		((ISupportInitialize)bar1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void buttonCancel_Click(object sender, EventArgs e)
	{
		DestroyTooltip();
		base.DialogResult = (DialogResult)2;
	}

	private void buttonOK_Click(object sender, EventArgs e)
	{
		DestroyTooltip();
		base.DialogResult = (DialogResult)1;
	}

	private void inputText_TextChanged(object sender, EventArgs e)
	{
		timer1.Stop();
		timer1.Start();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		timer1.Stop();
		UpdatePreview();
	}

	private void UpdatePreview()
	{
		((Control)previewPanel).Text = ((Control)inputText).Text;
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		((Control)inputText).Focus();
	}

	private void label2_MouseEnter(object sender, EventArgs e)
	{
		DestroyTooltip();
		m_Tooltip = new SuperTooltipControl();
		m_Tooltip.AntiAlias = false;
		SuperTooltipInfo superTooltipInfo = new SuperTooltipInfo();
		superTooltipInfo.HeaderText = "Markup tags available";
		superTooltipInfo.BodyText = "<b>b</b> - Bold <br/><b>i</b> - Italic <br/><b>u</b> - Underline <br/><b>p</b> - Paragraph container, <b>width</b> attribute indicates width of block element <br/><b>div</b> - Block-level container, <b>width</b> attribute indicates width of block element<br/><b>span</b> - Inline container, <b>width</b> attribute indicates width of block element. Can be used to create tables<br/><b>br</b> - Line break<br/><b>font</b> - Changes font, color, size. <b>size</b> attribute indicates absolute or relative font size. <b>color</b> attribute to change the colorYou can use relative sizing for example +1 to increase font size by one point or -1 to decrease by one point. <b>face</b> attribute can be employed to set the specific font name to use.<br/><b>h1</b> - Header markup. You can use h1, h2, h3, h4, h5, h6 to represent header sizes<br/><b>a</b> - Denotes hypertext link. <b>href</b> and <b>name</b> attributes are supported<br/><b>expand</b> - Displays the expand part of the button<br/>All tags must be lower case and they must be well formed i.e. each tag must be closed with end tag or be an empty tag.";
		superTooltipInfo.Color = eTooltipColor.Default;
		Point point = ((Control)label2).PointToScreen(new Point(0, ((Control)label2).Height + 1));
		m_Tooltip.ShowTooltip(superTooltipInfo, point.X, point.Y, enforceScreenPosition: true);
	}

	private void DestroyTooltip()
	{
		if (m_Tooltip != null)
		{
			((Control)m_Tooltip).Hide();
			((Component)(object)m_Tooltip).Dispose();
			m_Tooltip = null;
		}
	}

	private void label2_MouseLeave(object sender, EventArgs e)
	{
		DestroyTooltip();
	}

	private void buttonBold_Click(object sender, EventArgs e)
	{
		ApplyTag("<b>", "</b>");
	}

	private void buttonItalic_Click(object sender, EventArgs e)
	{
		ApplyTag("<i>", "</i>");
	}

	private void buttonUnderline_Click(object sender, EventArgs e)
	{
		ApplyTag("<u>", "</u>");
	}

	private void buttonColor_SelectedColorChanged(object sender, EventArgs e)
	{
		ApplyTag("<font color=\"" + GetHexColor(buttonColor.SelectedColor) + "\">", "</font>");
	}

	private void ApplyTag(string startTag, string endTag)
	{
		if (((TextBoxBase)inputText).SelectionLength == 0)
		{
			((TextBoxBase)inputText).SelectedText = startTag + endTag;
		}
		else
		{
			((TextBoxBase)inputText).SelectedText = startTag + ((TextBoxBase)inputText).SelectedText + endTag;
		}
		UpdatePreview();
	}

	private string GetHexColor(Color color)
	{
		return "#" + color.R.ToString("X02") + color.G.ToString("X02") + color.B.ToString("X02");
	}
}
