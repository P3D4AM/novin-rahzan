using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

[ToolboxItem(false)]
public class SuperTooltipVisualEditor : UserControl
{
	private IContainer components;

	private PanelEx panelEx1;

	internal ButtonX buttonOK;

	internal ButtonX buttonCancel;

	private CheckBox footerVisible;

	private CheckBox headerVisible;

	private TextBox textFooter;

	private TextBox textBody;

	private TextBox textHeader;

	private PanelEx panelBodyImage;

	private PanelEx panelEx5;

	private ComboBox comboColors;

	private Label label1;

	private PanelEx panelFooterImage;

	private CheckBox checkCustomSize;

	private NumericUpDown customHeight;

	private NumericUpDown customWidth;

	private Label label2;

	private SuperTooltipInfo m_Info;

	private bool m_Canceled;

	private IWindowsFormsEditorService m_EditorService;

	private bool m_BodyImageSelected;

	private bool m_FooterImageSelected;

	private SuperTooltipControl m_Tooltip;

	private PanelEx resetBodyImage;

	private PanelEx resetFooterImage;

	private SuperTooltip superTooltip1;

	private CustomTypeEditorProvider m_EditorProvider;

	private Font m_DefaultFont;

	internal int MaximumWidth;

	private SuperTooltip m_ParentSuperTooltip;

	public CustomTypeEditorProvider EditorProvider
	{
		get
		{
			return m_EditorProvider;
		}
		set
		{
			m_EditorProvider = value;
		}
	}

	public SuperTooltipInfo SuperTooltipInfo
	{
		get
		{
			SuperTooltipInfo superTooltipInfo = new SuperTooltipInfo();
			UpdateSuperTooltipInfo(superTooltipInfo);
			return superTooltipInfo;
		}
		set
		{
			m_Info = value;
			OnSuperTooltipInfoChanged();
		}
	}

	public IWindowsFormsEditorService EditorService
	{
		get
		{
			return m_EditorService;
		}
		set
		{
			m_EditorService = value;
		}
	}

	public bool Canceled => m_Canceled;

	public SuperTooltip ParentSuperTooltip
	{
		get
		{
			return m_ParentSuperTooltip;
		}
		set
		{
			m_ParentSuperTooltip = value;
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		resetFooterImage = new PanelEx();
		resetBodyImage = new PanelEx();
		customWidth = new NumericUpDown();
		customHeight = new NumericUpDown();
		label2 = new Label();
		checkCustomSize = new CheckBox();
		panelFooterImage = new PanelEx();
		comboColors = new ComboBox();
		label1 = new Label();
		panelEx5 = new PanelEx();
		panelBodyImage = new PanelEx();
		textFooter = new TextBox();
		textBody = new TextBox();
		textHeader = new TextBox();
		footerVisible = new CheckBox();
		headerVisible = new CheckBox();
		buttonCancel = new ButtonX();
		buttonOK = new ButtonX();
		superTooltip1 = new SuperTooltip();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)customWidth).BeginInit();
		((ISupportInitialize)customHeight).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.AntiAlias = true;
		panelEx1.CanvasColor = SystemColors.Control;
		((Control)panelEx1).Controls.Add((Control)(object)resetFooterImage);
		((Control)panelEx1).Controls.Add((Control)(object)resetBodyImage);
		((Control)panelEx1).Controls.Add((Control)(object)customWidth);
		((Control)panelEx1).Controls.Add((Control)(object)customHeight);
		((Control)panelEx1).Controls.Add((Control)(object)label2);
		((Control)panelEx1).Controls.Add((Control)(object)checkCustomSize);
		((Control)panelEx1).Controls.Add((Control)(object)panelFooterImage);
		((Control)panelEx1).Controls.Add((Control)(object)comboColors);
		((Control)panelEx1).Controls.Add((Control)(object)label1);
		((Control)panelEx1).Controls.Add((Control)(object)panelEx5);
		((Control)panelEx1).Controls.Add((Control)(object)panelBodyImage);
		((Control)panelEx1).Controls.Add((Control)(object)textFooter);
		((Control)panelEx1).Controls.Add((Control)(object)textBody);
		((Control)panelEx1).Controls.Add((Control)(object)textHeader);
		((Control)panelEx1).Controls.Add((Control)(object)footerVisible);
		((Control)panelEx1).Controls.Add((Control)(object)headerVisible);
		((Control)panelEx1).Controls.Add((Control)(object)buttonCancel);
		((Control)panelEx1).Controls.Add((Control)(object)buttonOK);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(432, 320);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.WhiteSmoke;
		panelEx1.Style.BackColor2.Color = Color.White;
		panelEx1.Style.BackgroundImagePosition = eBackgroundImagePosition.Tile;
		panelEx1.Style.Border = eBorderType.SingleLine;
		panelEx1.Style.BorderColor.Color = Color.DimGray;
		panelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)resetFooterImage).Anchor = (AnchorStyles)6;
		resetFooterImage.AntiAlias = true;
		resetFooterImage.CanvasColor = SystemColors.Control;
		resetFooterImage.DialogResult = (DialogResult)1;
		((Control)resetFooterImage).Location = new Point(29, 230);
		((Control)resetFooterImage).Name = "resetFooterImage";
		((Control)resetFooterImage).Size = new Size(73, 20);
		resetFooterImage.Style.Alignment = (StringAlignment)1;
		resetFooterImage.Style.BackColor1.Color = Color.Gainsboro;
		resetFooterImage.Style.BackColor2.Color = Color.WhiteSmoke;
		resetFooterImage.Style.BackgroundImagePosition = eBackgroundImagePosition.Tile;
		resetFooterImage.Style.Border = eBorderType.SingleLine;
		resetFooterImage.Style.BorderColor.Color = Color.Gray;
		resetFooterImage.Style.CornerType = eCornerType.Rounded;
		resetFooterImage.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		resetFooterImage.Style.GradientAngle = 90;
		resetFooterImage.StyleMouseDown.Alignment = (StringAlignment)1;
		resetFooterImage.StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		resetFooterImage.StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		resetFooterImage.StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		resetFooterImage.StyleMouseDown.CornerType = eCornerType.Rounded;
		resetFooterImage.StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		resetFooterImage.StyleMouseOver.Alignment = (StringAlignment)1;
		resetFooterImage.StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		resetFooterImage.StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		resetFooterImage.StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		resetFooterImage.StyleMouseOver.CornerType = eCornerType.Rounded;
		resetFooterImage.StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		superTooltip1.SetSuperTooltip((IComponent)resetFooterImage, new SuperTooltipInfo("Reset footer image", "", "Click to reset footer image.", null, null, eTooltipColor.Lemon, headerVisible: true, footerVisible: false, new Size(0, 0)));
		((Control)resetFooterImage).TabIndex = 17;
		((Control)resetFooterImage).Text = "Reset Image";
		((Control)resetFooterImage).Click += resetFooterImage_Click;
		resetBodyImage.AntiAlias = true;
		resetBodyImage.CanvasColor = SystemColors.Control;
		resetBodyImage.DialogResult = (DialogResult)1;
		((Control)resetBodyImage).Location = new Point(7, 60);
		((Control)resetBodyImage).Name = "resetBodyImage";
		((Control)resetBodyImage).Size = new Size(95, 20);
		resetBodyImage.Style.Alignment = (StringAlignment)1;
		resetBodyImage.Style.BackColor1.Color = Color.Gainsboro;
		resetBodyImage.Style.BackColor2.Color = Color.WhiteSmoke;
		resetBodyImage.Style.BackgroundImagePosition = eBackgroundImagePosition.Tile;
		resetBodyImage.Style.Border = eBorderType.SingleLine;
		resetBodyImage.Style.BorderColor.Color = Color.Gray;
		resetBodyImage.Style.CornerType = eCornerType.Rounded;
		resetBodyImage.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		resetBodyImage.Style.GradientAngle = 90;
		resetBodyImage.StyleMouseDown.Alignment = (StringAlignment)1;
		resetBodyImage.StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		resetBodyImage.StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		resetBodyImage.StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		resetBodyImage.StyleMouseDown.CornerType = eCornerType.Rounded;
		resetBodyImage.StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		resetBodyImage.StyleMouseOver.Alignment = (StringAlignment)1;
		resetBodyImage.StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		resetBodyImage.StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		resetBodyImage.StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		resetBodyImage.StyleMouseOver.CornerType = eCornerType.Rounded;
		resetBodyImage.StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		superTooltip1.SetSuperTooltip((IComponent)resetBodyImage, new SuperTooltipInfo("Reset Body Image", "", "Click to reset body image.", null, null, eTooltipColor.Lemon, headerVisible: true, footerVisible: false, new Size(0, 0)));
		((Control)resetBodyImage).TabIndex = 16;
		((Control)resetBodyImage).Text = "Reset Image";
		((Control)resetBodyImage).Click += resetBodyImage_Click;
		((Control)customWidth).Anchor = (AnchorStyles)6;
		((Control)customWidth).Enabled = false;
		((Control)customWidth).Location = new Point(107, 256);
		customWidth.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		customWidth.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)customWidth).Name = "customWidth";
		((Control)customWidth).Size = new Size(52, 21);
		((Control)customWidth).TabIndex = 13;
		customWidth.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)customHeight).Anchor = (AnchorStyles)6;
		((Control)customHeight).Enabled = false;
		((Control)customHeight).Location = new Point(174, 256);
		customHeight.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		NumericUpDown obj = customHeight;
		int[] bits = new int[4];
		obj.Minimum = new decimal(bits);
		((Control)customHeight).Name = "customHeight";
		((Control)customHeight).Size = new Size(52, 21);
		((Control)customHeight).TabIndex = 14;
		customHeight.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)label2).Anchor = (AnchorStyles)6;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(160, 258);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(13, 13);
		((Control)label2).TabIndex = 15;
		((Control)label2).Text = "x";
		((Control)checkCustomSize).Anchor = (AnchorStyles)6;
		((Control)checkCustomSize).Location = new Point(7, 260);
		((Control)checkCustomSize).Name = "checkCustomSize";
		((Control)checkCustomSize).Size = new Size(83, 17);
		((Control)checkCustomSize).TabIndex = 12;
		((Control)checkCustomSize).Text = "Custom size";
		checkCustomSize.CheckedChanged += checkCustomSize_CheckedChanged;
		((Control)panelFooterImage).Anchor = (AnchorStyles)6;
		panelFooterImage.AntiAlias = true;
		panelFooterImage.CanvasColor = SystemColors.Control;
		((Control)panelFooterImage).Location = new Point(76, 203);
		((Control)panelFooterImage).Name = "panelFooterImage";
		((Control)panelFooterImage).Size = new Size(24, 24);
		panelFooterImage.Style.Alignment = (StringAlignment)1;
		panelFooterImage.Style.BackColor1.Color = Color.White;
		panelFooterImage.Style.BackColor2.Color = Color.White;
		panelFooterImage.Style.BackgroundImagePosition = eBackgroundImagePosition.Center;
		panelFooterImage.Style.Border = eBorderType.SingleLine;
		panelFooterImage.Style.BorderColor.Color = Color.DimGray;
		panelFooterImage.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		panelFooterImage.Style.GradientAngle = 90;
		superTooltip1.SetSuperTooltip((IComponent)panelFooterImage, new SuperTooltipInfo("Click to set footer image", "", "Allows you to choose footer image. Note that image displayed here is preview image. You can see tooltip preview below.", null, null, eTooltipColor.Lemon, headerVisible: true, footerVisible: false, new Size(280, 170)));
		((Control)panelFooterImage).TabIndex = 11;
		((Control)panelFooterImage).Click += panelFooterImage_Click;
		((Control)comboColors).Anchor = (AnchorStyles)13;
		comboColors.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboColors).Location = new Point(107, 6);
		((Control)comboColors).Name = "comboColors";
		((Control)comboColors).Size = new Size(316, 21);
		comboColors.Sorted = true;
		((Control)comboColors).TabIndex = 10;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(4, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(96, 13);
		((Control)label1).TabIndex = 9;
		((Control)label1).Text = "Predefined Colors:";
		((Control)panelEx5).Anchor = (AnchorStyles)14;
		panelEx5.AntiAlias = true;
		panelEx5.CanvasColor = SystemColors.Control;
		((Control)panelEx5).Location = new Point(7, 287);
		((Control)panelEx5).Name = "panelEx5";
		((Control)panelEx5).Size = new Size(283, 24);
		panelEx5.Style.Alignment = (StringAlignment)1;
		panelEx5.Style.BackColor1.Color = Color.LightGray;
		panelEx5.Style.BackColor2.Color = Color.DimGray;
		panelEx5.Style.Border = eBorderType.SingleLine;
		panelEx5.Style.BorderColor.Color = Color.DimGray;
		panelEx5.Style.CornerType = eCornerType.Rounded;
		panelEx5.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		panelEx5.Style.GradientAngle = 90;
		((Control)panelEx5).TabIndex = 8;
		((Control)panelEx5).Text = "Place mouse here to preview";
		((Control)panelEx5).MouseLeave += panelEx5_MouseLeave;
		((Control)panelEx5).MouseEnter += panelEx5_MouseEnter;
		((Control)panelBodyImage).Anchor = (AnchorStyles)7;
		panelBodyImage.AntiAlias = true;
		panelBodyImage.CanvasColor = SystemColors.Control;
		((Control)panelBodyImage).Location = new Point(7, 83);
		((Control)panelBodyImage).Name = "panelBodyImage";
		((Control)panelBodyImage).Size = new Size(94, 117);
		panelBodyImage.Style.Alignment = (StringAlignment)1;
		panelBodyImage.Style.BackColor1.Color = Color.White;
		panelBodyImage.Style.BackColor2.Color = Color.White;
		panelBodyImage.Style.BackgroundImagePosition = eBackgroundImagePosition.Center;
		panelBodyImage.Style.Border = eBorderType.SingleLine;
		panelBodyImage.Style.BorderColor.Color = Color.DimGray;
		panelBodyImage.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		panelBodyImage.Style.GradientAngle = 90;
		superTooltip1.SetSuperTooltip((IComponent)panelBodyImage, new SuperTooltipInfo("Click to set body image", "", "Allows you to choose body image. Note that image displayed here is preview image. You can see tooltip preview below", null, null, eTooltipColor.Lemon, headerVisible: true, footerVisible: false, Size.Empty));
		((Control)panelBodyImage).TabIndex = 6;
		((Control)panelBodyImage).Click += panelBodyImage_Click;
		((Control)textFooter).Anchor = (AnchorStyles)14;
		((Control)textFooter).Location = new Point(107, 206);
		((Control)textFooter).Name = "textFooter";
		((Control)textFooter).Size = new Size(316, 21);
		((Control)textFooter).TabIndex = 2;
		textBody.AcceptsReturn = true;
		((Control)textBody).Anchor = (AnchorStyles)15;
		((Control)textBody).Location = new Point(107, 60);
		((TextBoxBase)textBody).Multiline = true;
		((Control)textBody).Name = "textBody";
		textBody.ScrollBars = (ScrollBars)2;
		((Control)textBody).Size = new Size(316, 140);
		((Control)textBody).TabIndex = 1;
		((Control)textHeader).Anchor = (AnchorStyles)13;
		((Control)textHeader).Location = new Point(107, 33);
		((Control)textHeader).Name = "textHeader";
		((Control)textHeader).Size = new Size(316, 21);
		((Control)textHeader).TabIndex = 0;
		((Control)footerVisible).Anchor = (AnchorStyles)6;
		((Control)footerVisible).Location = new Point(7, 208);
		((Control)footerVisible).Name = "footerVisible";
		((Control)footerVisible).Size = new Size(58, 17);
		((Control)footerVisible).TabIndex = 7;
		((Control)footerVisible).Text = "Footer";
		((Control)headerVisible).Location = new Point(7, 39);
		((Control)headerVisible).Name = "headerVisible";
		((Control)headerVisible).Size = new Size(61, 17);
		((Control)headerVisible).TabIndex = 5;
		((Control)headerVisible).Text = "Header";
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.AntiAlias = true;
		buttonCancel.Style = eDotNetBarStyle.Office2007;
		buttonCancel.ColorTable = eButtonColor.Office2007WithBackground;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(364, 287);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(60, 25);
		((Control)buttonCancel).TabIndex = 4;
		((Control)buttonCancel).Text = "Cancel";
		((Control)buttonCancel).Click += buttonCancel_Click;
		((Control)buttonOK).Anchor = (AnchorStyles)10;
		buttonOK.AntiAlias = true;
		buttonOK.DialogResult = (DialogResult)1;
		((Control)buttonOK).Location = new Point(296, 287);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(62, 25);
		buttonOK.Style = eDotNetBarStyle.Office2007;
		buttonOK.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)buttonOK).TabIndex = 3;
		((Control)buttonOK).Text = "OK";
		((Control)buttonOK).Click += buttonOK_Click;
		((Control)this).BackColor = Color.WhiteSmoke;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this).Name = "SuperTooltipVisualEditor";
		((Control)this).Size = new Size(432, 320);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)customWidth).EndInit();
		((ISupportInitialize)customHeight).EndInit();
		((Control)this).ResumeLayout(false);
	}

	public SuperTooltipVisualEditor()
	{
		InitializeComponent();
		panelBodyImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
		panelFooterImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
		comboColors.Items.AddRange((object[])Enum.GetNames(typeof(eTooltipColor)));
	}

	private void panelBodyImage_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Image val = null;
		if (m_EditorProvider != null)
		{
			UITypeEditor val2 = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
			m_EditorProvider.SetInstance(m_Info, TypeDescriptor.GetProperties(m_Info)["BodyImage"]);
			object obj = val2.EditValue((ITypeDescriptorContext)m_EditorProvider, (IServiceProvider)m_EditorProvider, (object)m_Info.BodyImage);
			val = (Image)((obj is Image) ? obj : null);
		}
		if (m_Info.BodyImage != val)
		{
			m_Info.BodyImage = val;
			if (m_Info.BodyImage != null)
			{
				panelBodyImage.Style.BackgroundImage = m_Info.BodyImage;
			}
			else
			{
				panelBodyImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
			}
			m_BodyImageSelected = true;
		}
	}

	private void OnSuperTooltipInfoChanged()
	{
		if (m_Info != null)
		{
			((Control)textHeader).Text = m_Info.HeaderText;
			headerVisible.Checked = m_Info.HeaderVisible;
			((Control)textBody).Text = m_Info.BodyText;
			if (m_Info.BodyImage != null)
			{
				panelBodyImage.Style.BackgroundImage = m_Info.BodyImage;
				m_BodyImageSelected = true;
			}
			else
			{
				panelBodyImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
			}
			if (m_Info.FooterImage != null)
			{
				panelFooterImage.Style.BackgroundImage = m_Info.FooterImage;
				m_FooterImageSelected = true;
			}
			else
			{
				panelFooterImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
			}
			((Control)textFooter).Text = m_Info.FooterText;
			footerVisible.Checked = m_Info.FooterVisible;
			comboColors.SelectedItem = Enum.GetName(typeof(eTooltipColor), m_Info.Color);
			if (m_Info.CustomSize.IsEmpty)
			{
				checkCustomSize.Checked = false;
			}
			else
			{
				checkCustomSize.Checked = true;
				customWidth.Value = m_Info.CustomSize.Width;
				customHeight.Value = m_Info.CustomSize.Height;
			}
			((Control)customHeight).Enabled = checkCustomSize.Checked;
			((Control)customWidth).Enabled = checkCustomSize.Checked;
		}
	}

	private void UpdateSuperTooltipInfo(SuperTooltipInfo info)
	{
		if (info != null)
		{
			info.HeaderText = ((Control)textHeader).Text;
			info.HeaderVisible = headerVisible.Checked;
			info.BodyText = ((Control)textBody).Text;
			if (m_BodyImageSelected)
			{
				info.BodyImage = panelBodyImage.Style.BackgroundImage;
			}
			else
			{
				info.BodyImage = null;
			}
			if (m_FooterImageSelected)
			{
				info.FooterImage = panelFooterImage.Style.BackgroundImage;
			}
			else
			{
				info.FooterImage = null;
			}
			info.FooterText = ((Control)textFooter).Text;
			info.FooterVisible = footerVisible.Checked;
			info.Color = (eTooltipColor)Enum.Parse(typeof(eTooltipColor), comboColors.SelectedItem.ToString());
			if (checkCustomSize.Checked)
			{
				info.CustomSize = new Size((int)customWidth.Value, (int)customHeight.Value);
			}
			else
			{
				info.CustomSize = Size.Empty;
			}
		}
	}

	private void buttonCancel_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		DestroyTooltip();
		m_Canceled = true;
		((Form)((Control)this).Parent).Close();
	}

	private void buttonOK_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		DestroyTooltip();
		m_Canceled = false;
		((Form)((Control)this).Parent).Close();
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

	private void panelEx5_MouseEnter(object sender, EventArgs e)
	{
		DestroyTooltip();
		m_Tooltip = new SuperTooltipControl();
		if (m_DefaultFont != null)
		{
			((Control)m_Tooltip).Font = m_DefaultFont;
		}
		if (m_ParentSuperTooltip != null)
		{
			m_Tooltip.MaximumWidth = m_ParentSuperTooltip.MaximumWidth;
			m_Tooltip.MinimumTooltipSize = m_ParentSuperTooltip.MinimumTooltipSize;
			m_Tooltip.AntiAlias = m_ParentSuperTooltip.AntiAlias;
			if (m_ParentSuperTooltip.DefaultFont != null)
			{
				((Control)m_Tooltip).Font = m_ParentSuperTooltip.DefaultFont;
			}
		}
		else
		{
			m_Tooltip.MaximumWidth = MaximumWidth;
		}
		SuperTooltipInfo info = new SuperTooltipInfo();
		UpdateSuperTooltipInfo(info);
		Point point = ((Control)panelEx5).PointToScreen(new Point(0, ((Control)panelEx5).Height + 1));
		m_Tooltip.ShowTooltip(info, point.X, point.Y, enforceScreenPosition: true);
	}

	private void panelEx5_MouseLeave(object sender, EventArgs e)
	{
		DestroyTooltip();
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ScrollableControl)this).OnVisibleChanged(e);
		if (!((Control)this).Visible)
		{
			DestroyTooltip();
		}
	}

	private void panelFooterImage_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		Image val = null;
		if (m_EditorProvider != null)
		{
			UITypeEditor val2 = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
			m_EditorProvider.SetInstance(m_Info, TypeDescriptor.GetProperties(m_Info)["FooterImage"]);
			object obj = val2.EditValue((ITypeDescriptorContext)m_EditorProvider, (IServiceProvider)m_EditorProvider, (object)m_Info.FooterImage);
			val = (Image)((obj is Image) ? obj : null);
		}
		if (m_Info.FooterImage != val)
		{
			m_Info.FooterImage = val;
			if (m_Info.FooterImage != null)
			{
				panelFooterImage.Style.BackgroundImage = m_Info.FooterImage;
			}
			else
			{
				panelFooterImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
			}
			m_FooterImageSelected = true;
		}
	}

	private void checkCustomSize_CheckedChanged(object sender, EventArgs e)
	{
		((Control)customWidth).Enabled = checkCustomSize.Checked;
		((Control)customHeight).Enabled = checkCustomSize.Checked;
		if (checkCustomSize.Checked)
		{
			SuperTooltipInfo info = new SuperTooltipInfo();
			UpdateSuperTooltipInfo(info);
			SuperTooltipControl superTooltipControl = new SuperTooltipControl();
			superTooltipControl.UpdateWithSuperTooltipInfo(info);
			superTooltipControl.RecalcSize();
			customWidth.Value = ((Control)superTooltipControl).Width;
			customHeight.Value = ((Control)superTooltipControl).Height;
			((Component)(object)superTooltipControl).Dispose();
		}
	}

	private void resetBodyImage_Click(object sender, EventArgs e)
	{
		panelBodyImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
		m_Info.BodyImage = null;
		m_BodyImageSelected = false;
	}

	private void resetFooterImage_Click(object sender, EventArgs e)
	{
		panelFooterImage.Style.BackgroundImage = (Image)(object)Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
		m_Info.FooterImage = null;
		m_FooterImageSelected = false;
	}
}
