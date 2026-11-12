using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class TextBoxItem : ImageItem, IPersonalizedMenuItem
{
	public delegate void TextChangedEventHandler(object sender);

	[CompilerGenerated]
	private KeyEventHandler m_KeyDown;

	[CompilerGenerated]
	private KeyPressEventHandler m_KeyPress;

	[CompilerGenerated]
	private KeyEventHandler m_KeyUp;

	private TextBoxX m_TextBox;

	private bool m_MouseOver;

	private bool m_AlwaysShowCaption;

	private int m_TextBoxWidth;

	private eMenuVisibility m_MenuVisibility;

	private bool m_RecentlyUsed;

	private static RoundRectangleShapeDescriptor _MenuShape = new RoundRectangleShapeDescriptor(3);

	private int _TextBoxHeight;

	private string _Caption = "";

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if ((val is ItemPanel || val is ExplorerBar || val is SideBar) && base.Bounds != value)
			{
				bool flag = base.Bounds.Top != value.Top;
				bool flag2 = base.Bounds.Left != value.Left;
				base.Bounds = value;
				if (flag || flag2)
				{
					UpdateControlLocation();
				}
			}
			else
			{
				base.Bounds = value;
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether item caption is always shown.")]
	public bool AlwaysShowCaption
	{
		get
		{
			return m_AlwaysShowCaption;
		}
		set
		{
			m_AlwaysShowCaption = value;
		}
	}

	[DefaultValue(64)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the Width of the Text Box part of the item.")]
	public int TextBoxWidth
	{
		get
		{
			return m_TextBoxWidth;
		}
		set
		{
			if (m_TextBoxWidth != value)
			{
				m_TextBoxWidth = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "TextBoxWidth");
				}
				OnExternalSizeChange();
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates explicit height of TextBox.")]
	public int TextBoxHeight
	{
		get
		{
			return _TextBoxHeight;
		}
		set
		{
			_TextBoxHeight = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return ((TextBox)m_TextBox).UseSystemPasswordChar;
		}
		set
		{
			((TextBox)m_TextBox).UseSystemPasswordChar = value;
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[Localizable(true)]
	[Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
	[Category("Behavior")]
	[DefaultValue('\0')]
	public char PasswordChar
	{
		get
		{
			return ((TextBox)m_TextBox).PasswordChar;
		}
		set
		{
			((TextBox)m_TextBox).PasswordChar = value;
		}
	}

	public virtual string ControlText
	{
		get
		{
			return Text;
		}
		set
		{
			Text = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("The text contained in the text box that user can edit.")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the item Caption displayed next to the text input.")]
	[Localizable(true)]
	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_Caption = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Browsable(false)]
	public TextBox TextBox => (TextBox)(object)m_TextBox;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			if (m_TextBox != null)
			{
				return ((TextBoxBase)m_TextBox).SelectionStart;
			}
			return 0;
		}
		set
		{
			if (m_TextBox != null)
			{
				((TextBoxBase)m_TextBox).SelectionStart = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionLength
	{
		get
		{
			if (m_TextBox != null)
			{
				return ((TextBoxBase)m_TextBox).SelectionLength;
			}
			return 0;
		}
		set
		{
			if (m_TextBox != null)
			{
				((TextBoxBase)m_TextBox).SelectionLength = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			if (m_TextBox != null)
			{
				return ((TextBoxBase)m_TextBox).SelectedText;
			}
			return "";
		}
		set
		{
			if (m_TextBox != null)
			{
				((TextBoxBase)m_TextBox).SelectedText = value;
			}
		}
	}

	[DefaultValue(32767)]
	[Description("Indicates maximum number of characters the user can type or paste into the text box control. ")]
	public int MaxLength
	{
		get
		{
			return ((TextBoxBase)m_TextBox).MaxLength;
		}
		set
		{
			if (m_TextBox != null)
			{
				((TextBoxBase)m_TextBox).MaxLength = value;
			}
		}
	}

	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visibility when on pop-up menu.")]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return m_MenuVisibility;
		}
		set
		{
			if (m_MenuVisibility != value)
			{
				m_MenuVisibility = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool RecentlyUsed
	{
		get
		{
			return m_RecentlyUsed;
		}
		set
		{
			if (m_RecentlyUsed != value)
			{
				m_RecentlyUsed = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "RecentlyUsed");
				}
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return m_TextBox.WatermarkEnabled;
		}
		set
		{
			m_TextBox.WatermarkEnabled = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	public Image WatermarkImage
	{
		get
		{
			return m_TextBox.WatermarkImage;
		}
		set
		{
			m_TextBox.WatermarkImage = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates watermark image alignment.")]
	public ContentAlignment WatermarkImageAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_TextBox.WatermarkImageAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_TextBox.WatermarkImageAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return m_TextBox.WatermarkText;
		}
		set
		{
			m_TextBox.WatermarkText = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return m_TextBox.WatermarkFont;
		}
		set
		{
			m_TextBox.WatermarkFont = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return m_TextBox.WatermarkColor;
		}
		set
		{
			m_TextBox.WatermarkColor = value;
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return m_TextBox.WatermarkBehavior;
		}
		set
		{
			m_TextBox.WatermarkBehavior = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return m_TextBox.FocusHighlightEnabled;
		}
		set
		{
			m_TextBox.FocusHighlightEnabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return m_TextBox.FocusHighlightColor;
		}
		set
		{
			m_TextBox.FocusHighlightColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[RefreshProperties(RefreshProperties.All)]
	[ParenthesizePropertyName(true)]
	public new ControlBindingsCollection DataBindings => ((Control)m_TextBox).DataBindings;

	[Description("Indicates text-alignment inside of the text box.")]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue(0)]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)m_TextBox).TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)m_TextBox).TextAlign = value;
		}
	}

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => m_TextBox.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => m_TextBox.ButtonCustom2;

	public event TextChangedEventHandler InputTextChanged;

	public event KeyEventHandler KeyDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_KeyDown;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_KeyDown;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyDown, value2, val2);
			}
			while (val != val2);
		}
	}

	public event KeyPressEventHandler KeyPress
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = this.m_KeyPress;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyPress, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = this.m_KeyPress;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyPress, value2, val2);
			}
			while (val != val2);
		}
	}

	public event KeyEventHandler KeyUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_KeyUp;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_KeyUp;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyUp, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler ButtonCustomClick;

	public event EventHandler ButtonCustom2Click;

	public TextBoxItem()
		: this("", "")
	{
	}

	public TextBoxItem(string sName)
		: this(sName, "")
	{
	}

	public TextBoxItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		CreateTextBox();
		m_MouseOver = false;
		m_AlwaysShowCaption = false;
		m_TextBoxWidth = 64;
		ImageSize = new Size(16, 16);
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
	}

	private void CreateTextBox()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		TextBoxX textBoxX = null;
		if (m_TextBox != null)
		{
			((Control)m_TextBox).MouseEnter -= TextBoxMouseEnter;
			((Control)m_TextBox).MouseLeave -= TextBoxMouseLeave;
			((Control)m_TextBox).MouseHover -= TextBoxMouseHover;
			((Control)m_TextBox).LostFocus -= TextBoxLostFocus;
			((Control)m_TextBox).GotFocus -= TextBoxGotFocus;
			((Control)m_TextBox).TextChanged -= TextBoxTextChanged;
			((Control)m_TextBox).KeyDown -= new KeyEventHandler(TextBoxKeyDown);
			((Control)m_TextBox).KeyPress -= new KeyPressEventHandler(TextBoxKeyPress);
			((Control)m_TextBox).KeyUp -= new KeyEventHandler(TextBoxKeyUp);
			m_TextBox.ButtonCustomClick -= TextBoxButtonCustomClick;
			m_TextBox.ButtonCustom2Click -= TextBoxButtonCustom2Click;
			textBoxX = m_TextBox;
			m_TextBox = null;
		}
		m_TextBox = new TextBoxX(isTextBoxItem: true);
		m_TextBox.BorderStyle = (BorderStyle)0;
		((Control)m_TextBox).AutoSize = false;
		((Control)m_TextBox).Visible = false;
		((Control)m_TextBox).TabStop = false;
		if (textBoxX != null)
		{
			((TextBoxBase)m_TextBox).MaxLength = ((TextBoxBase)textBoxX).MaxLength;
			((TextBox)m_TextBox).UseSystemPasswordChar = ((TextBox)textBoxX).UseSystemPasswordChar;
			((TextBox)m_TextBox).PasswordChar = ((TextBox)textBoxX).PasswordChar;
			m_TextBox.WatermarkEnabled = textBoxX.WatermarkEnabled;
			m_TextBox.WatermarkText = textBoxX.WatermarkText;
			m_TextBox.WatermarkFont = textBoxX.WatermarkFont;
			m_TextBox.WatermarkColor = textBoxX.WatermarkColor;
			m_TextBox.WatermarkBehavior = textBoxX.WatermarkBehavior;
			m_TextBox.FocusHighlightEnabled = textBoxX.FocusHighlightEnabled;
			m_TextBox.FocusHighlightColor = textBoxX.FocusHighlightColor;
			((Component)(object)textBoxX).Dispose();
		}
		((Control)m_TextBox).MouseEnter += TextBoxMouseEnter;
		((Control)m_TextBox).MouseLeave += TextBoxMouseLeave;
		((Control)m_TextBox).MouseHover += TextBoxMouseHover;
		((Control)m_TextBox).LostFocus += TextBoxLostFocus;
		((Control)m_TextBox).GotFocus += TextBoxGotFocus;
		((Control)m_TextBox).TextChanged += TextBoxTextChanged;
		((Control)m_TextBox).KeyDown += new KeyEventHandler(TextBoxKeyDown);
		((Control)m_TextBox).KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
		((Control)m_TextBox).KeyUp += new KeyEventHandler(TextBoxKeyUp);
		m_TextBox.ButtonCustomClick += TextBoxButtonCustomClick;
		m_TextBox.ButtonCustom2Click += TextBoxButtonCustom2Click;
		((Control)m_TextBox).Text = Text;
		((TextBoxBase)m_TextBox).SelectionStart = 0;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)m_TextBox);
			}
		}
		if (Displayed)
		{
			((Control)m_TextBox).Visible = true;
			Refresh();
		}
		CustomizeChanged();
	}

	private void TextBoxButtonCustomClick(object sender, EventArgs e)
	{
		if (this.ButtonCustomClick != null)
		{
			this.ButtonCustomClick(this, e);
		}
	}

	private void TextBoxButtonCustom2Click(object sender, EventArgs e)
	{
		if (this.ButtonCustom2Click != null)
		{
			this.ButtonCustom2Click(this, e);
		}
	}

	public override BaseItem Copy()
	{
		TextBoxItem textBoxItem = new TextBoxItem(Name);
		CopyToItem(textBoxItem);
		return textBoxItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		TextBoxItem textBoxItem = copy as TextBoxItem;
		base.CopyToItem(textBoxItem);
		textBoxItem.TextBoxWidth = m_TextBoxWidth;
	}

	protected override void Dispose(bool disposing)
	{
		_ = m_TextBox;
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("Caption", _Caption);
		itemXmlElement.SetAttribute("TextBoxWidth", XmlConvert.ToString(m_TextBoxWidth));
		itemXmlElement.SetAttribute("AlwaysShowCaption", XmlConvert.ToString(m_AlwaysShowCaption));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)m_MenuVisibility));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(m_RecentlyUsed));
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("Caption"))
		{
			Caption = itemXmlElement.GetAttribute("Caption");
		}
		m_TextBoxWidth = XmlConvert.ToInt32(itemXmlElement.GetAttribute("TextBoxWidth"));
		m_AlwaysShowCaption = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AlwaysShowCaption"));
		m_MenuVisibility = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		m_RecentlyUsed = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_0b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		bool enabled = GetEnabled(pa.ContainerControl);
		Size maxImageSize = GetMaxImageSize();
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
			{
				maxImageSize.Width += 7;
				displayRectangle.Width -= maxImageSize.Width;
				if (!pa.RightToLeft)
				{
					displayRectangle.X += maxImageSize.Width;
				}
				if (base.IsOnCustomizeMenu)
				{
					maxImageSize.Width += maxImageSize.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
				if (pa.RightToLeft)
				{
					rectangle.X = m_Rect.Right - rectangle.Width;
				}
				if (!BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
					{
						if (!pa.Colors.MenuUnusedSide2.IsEmpty)
						{
							LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
							graphics.FillRectangle((Brush)(object)val, rectangle);
							((Brush)val).Dispose();
						}
						else
						{
							graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuUnusedSide), rectangle);
						}
					}
					else if (!pa.Colors.MenuSide2.IsEmpty)
					{
						LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
						graphics.FillRectangle((Brush)(object)val2, rectangle);
						((Brush)val2).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuSide), rectangle);
					}
				}
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
					if (menu != null)
					{
						if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
						{
							DisplayHelp.FillRectangle(graphics, rectangle, menu.SideUnused);
						}
						else
						{
							DisplayHelp.FillRectangle(graphics, rectangle, menu.Side);
						}
					}
					if (menu != null && !menu.SideBorder.IsEmpty)
					{
						if (pa.RightToLeft)
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
						}
						else
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 2, rectangle.Y, rectangle.Right - 2, rectangle.Bottom - 1, menu.SideBorder, 1);
						}
					}
					if (menu != null && !menu.SideBorderLight.IsEmpty)
					{
						if (pa.RightToLeft)
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
						}
						else
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
						}
					}
				}
			}
			if (base.IsOnCustomizeMenu)
			{
				if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					displayRectangle.X += maxImageSize.Height + 8;
					displayRectangle.Width -= maxImageSize.Height + 8;
				}
				else
				{
					displayRectangle.X += maxImageSize.Height + 4;
					displayRectangle.Width -= maxImageSize.Height + 4;
				}
			}
			if (flag && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && enabled && (m_MouseOver || Focused))
			{
				Rectangle displayRectangle2 = DisplayRectangle;
				displayRectangle2.Inflate(-1, 0);
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && !(pa.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007ButtonItemStateColorTable mouseOver = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
					Office2007ButtonItemPainter.PaintBackground(graphics, mouseOver, displayRectangle2, _MenuShape);
				}
				else
				{
					if (!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(displayRectangle2, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val3, displayRectangle2);
						((Brush)val3).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), displayRectangle2);
					}
					NativeFunctions.DrawRectangle(graphics, new Pen(pa.Colors.ItemHotBorder), displayRectangle2);
				}
			}
			if (_Caption != "" && (m_AlwaysShowCaption || flag))
			{
				eTextFormat eTextFormat2 = GetStringFormat();
				Font font = GetFont();
				Rectangle bounds = new Rectangle(displayRectangle.X + 8, displayRectangle.Y, displayRectangle.Width - 8, displayRectangle.Height);
				if (pa.RightToLeft)
				{
					eTextFormat2 |= eTextFormat.Right;
					bounds.Width -= 9;
				}
				Color color = pa.Colors.ItemText;
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
				{
					color = (enabled ? ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText);
				}
				if (effectiveStyle == eDotNetBarStyle.Office2000)
				{
					TextDrawing.DrawString(graphics, _Caption, font, color, bounds, eTextFormat2);
				}
				else
				{
					TextDrawing.DrawString(graphics, _Caption, font, color, bounds, eTextFormat2);
				}
				Size size = TextDrawing.MeasureString(graphics, _Caption, font, 0, eTextFormat2);
				if (!pa.RightToLeft)
				{
					displayRectangle.X += size.Width + 8;
				}
				displayRectangle.Width -= size.Width + 8;
			}
			int num = Math.Max(19, (_TextBoxHeight > 0) ? _TextBoxHeight : ((TextBoxBase)m_TextBox).PreferredHeight);
			if (DesignMode && !flag)
			{
				Rectangle rectangle2 = displayRectangle;
				if (m_TextBox != null)
				{
					rectangle2.Inflate(-5, -3);
					rectangle2.Y += (rectangle2.Height - (num + 3)) / 2;
					rectangle2.Height = num + 3;
				}
				graphics.FillRectangle(SystemBrushes.Window, rectangle2);
				if (Text != "")
				{
					rectangle2.Inflate(-1, -1);
					TextDrawing.DrawString(graphics, Text, GetFont(), enabled ? pa.Colors.ItemText : pa.Colors.ItemDisabledText, rectangle2, GetStringFormat());
				}
			}
			if (m_TextBox != null)
			{
				displayRectangle.Inflate(-5, -3);
				displayRectangle.Y += (displayRectangle.Height - num) / 2;
				displayRectangle.Height = num;
				((Control)m_TextBox).Size = new Size(displayRectangle.Width, (_TextBoxHeight > 0) ? _TextBoxHeight : ((TextBoxBase)m_TextBox).PreferredHeight);
				Point location = displayRectangle.Location;
				if (pa.RightToLeft)
				{
					location.Offset(0, (displayRectangle.Height - ((Control)m_TextBox).Height) / 2);
				}
				else
				{
					location.Offset(2, (displayRectangle.Height - ((Control)m_TextBox).Height) / 2);
				}
				((Control)m_TextBox).Location = location;
				if (BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					Color color2 = Color.Empty;
					ElementStyle textBoxElementStyle = GetTextBoxElementStyle();
					if (!enabled)
					{
						color2 = SystemColors.ControlDark;
					}
					else if (textBoxElementStyle != null)
					{
						color2 = textBoxElementStyle.BorderColor;
					}
					if (!color2.IsEmpty)
					{
						displayRectangle = new Rectangle(displayRectangle.Location, new Size(((Control)m_TextBox).Width + 2, num));
						DisplayHelp.FillRectangle(graphics, displayRectangle, ((Control)m_TextBox).BackColor);
						displayRectangle.Inflate(1, 1);
						if (!enabled)
						{
							graphics.FillRectangle(SystemBrushes.Control, displayRectangle);
						}
						DisplayHelp.DrawRectangle(graphics, color2, displayRectangle);
					}
				}
				else if (enabled && (m_MouseOver || Focused))
				{
					switch (effectiveStyle)
					{
					case eDotNetBarStyle.Office2000:
						displayRectangle = new Rectangle(((Control)m_TextBox).Location, ((Control)m_TextBox).Size);
						displayRectangle.Inflate(2, 2);
						ControlPaint.DrawBorder3D(graphics, displayRectangle, (Border3DStyle)2, (Border3DSide)2063);
						break;
					case eDotNetBarStyle.OfficeXP:
					case eDotNetBarStyle.Office2003:
					case eDotNetBarStyle.VS2005:
						displayRectangle = new Rectangle(((Control)m_TextBox).Location, ((Control)m_TextBox).Size);
						displayRectangle.Inflate(1, 1);
						NativeFunctions.DrawRectangle(graphics, SystemPens.Highlight, displayRectangle);
						break;
					}
				}
			}
			else
			{
				displayRectangle.Inflate(-3, -3);
				graphics.FillRectangle(SystemBrushes.Window, displayRectangle);
			}
			if (base.IsOnCustomizeMenu && Visible)
			{
				Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					r.Inflate(-1, -1);
				}
				BarFunctions.DrawMenuCheckBox(pa, r, effectiveStyle, m_MouseOver);
			}
		}
		else
		{
			string text = Text;
			text = ((!(text == "")) ? (text + " »") : "...");
			if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.BarBackground), DisplayRectangle);
			}
			else
			{
				graphics.FillRectangle(SystemBrushes.Control, DisplayRectangle);
			}
			if (m_MouseOver && !DesignMode)
			{
				if (effectiveStyle == eDotNetBarStyle.Office2000)
				{
					ControlPaint.DrawBorder3D(graphics, displayRectangle, (Border3DStyle)4, (Border3DSide)2063);
				}
				else if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					displayRectangle.Inflate(-1, -1);
					if (!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(displayRectangle, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val4, displayRectangle);
						((Brush)val4).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), displayRectangle);
					}
					NativeFunctions.DrawRectangle(graphics, new Pen(pa.Colors.ItemHotBorder), displayRectangle);
				}
			}
			displayRectangle = new Rectangle(m_Rect.Top, -m_Rect.Right, m_Rect.Height, m_Rect.Width);
			graphics.RotateTransform(90f);
			eTextFormat stringFormat = GetStringFormat();
			stringFormat |= eTextFormat.HorizontalCenter;
			if (m_MouseOver && enabled)
			{
				TextDrawing.DrawStringLegacy(graphics, text, GetFont(), pa.Colors.ItemHotText, displayRectangle, stringFormat);
			}
			else
			{
				TextDrawing.DrawStringLegacy(graphics, text, GetFont(), enabled ? pa.Colors.ItemText : pa.Colors.ItemDisabledText, displayRectangle, stringFormat);
			}
			graphics.ResetTransform();
		}
		if (Focused && DesignMode)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(graphics);
	}

	private ElementStyle GetTextBoxElementStyle()
	{
		ElementStyle result = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			result = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.StyleClasses[ElementStyleClassKeys.TextBoxBorderKey] as ElementStyle;
		}
		return result;
	}

	private void UpdateControlLocation()
	{
		if (m_TextBox != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-5, -3);
			((Control)m_TextBox).Location = displayRectangle.Location;
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		bool isOnMenu = base.IsOnMenu;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (Orientation == eOrientation.Horizontal)
		{
			if (Parent != null && Parent is ImageItem)
			{
				m_Rect.Height = ((ImageItem)Parent).SubItemsImageSize.Height + 4;
			}
			else
			{
				m_Rect.Height = SubItemsImageSize.Height + 4;
			}
			m_Rect.Width = m_TextBoxWidth + 4;
			if (_Caption != "" && (m_AlwaysShowCaption || isOnMenu))
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null && BaseItem.IsHandleValid(val))
				{
					Graphics val2 = BarFunctions.CreateGraphics(val);
					try
					{
						Size size = TextDrawing.MeasureString(val2, _Caption, GetFont(), 0, GetStringFormat());
						if (size.Height > SubItemsImageSize.Height)
						{
							m_Rect.Height = size.Height + 4;
						}
						m_Rect.Width = m_TextBoxWidth + 4 + size.Width + 8;
					}
					finally
					{
						val2.Dispose();
					}
				}
			}
			if (m_TextBox != null)
			{
				if (m_TextBox.ButtonCustom.Visible && m_TextBox.ButtonCustom.Image != null)
				{
					m_Rect.Height = Math.Max(m_Rect.Height, m_TextBox.ButtonCustom.Image.Height + 6);
				}
				if (m_TextBox.ButtonCustom2.Visible && m_TextBox.ButtonCustom2.Image != null)
				{
					m_Rect.Height = Math.Max(m_Rect.Height, m_TextBox.ButtonCustom2.Image.Height + 6);
				}
			}
			Size maxImageSize = GetMaxImageSize();
			if (m_TextBox != null)
			{
				int num = Math.Max(19, (_TextBoxHeight > 0) ? _TextBoxHeight : ((TextBoxBase)m_TextBox).PreferredHeight) + 2;
				if (m_Rect.Height < num)
				{
					m_Rect.Height = num;
				}
			}
			if (base.IsOnMenu && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
			{
				m_Rect.Width += maxImageSize.Width + 7;
			}
			if (base.IsOnCustomizeMenu)
			{
				m_Rect.Width += maxImageSize.Height + 2;
			}
		}
		else
		{
			m_Rect.Width = SubItemsImageSize.Width + 4;
			string text = Text;
			text = ((!(text == "")) ? (text + " »") : "...");
			object containerControl2 = ContainerControl;
			Control val3 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val3 != null && BaseItem.IsHandleValid(val3))
			{
				Graphics val4 = BarFunctions.CreateGraphics(val3);
				try
				{
					Size size2 = TextDrawing.MeasureString(val4, text, GetFont(), 0, GetStringFormat());
					if (size2.Height > SubItemsImageSize.Height)
					{
						m_Rect.Width = size2.Height + 4;
					}
					m_Rect.Height = size2.Width + 8;
				}
				finally
				{
					val4.Dispose();
				}
			}
		}
		base.RecalcSize();
	}

	private Size GetMaxImageSize()
	{
		if (m_Parent != null)
		{
			if (m_Parent is ImageItem imageItem)
			{
				return imageItem.SubItemsImageSize;
			}
			return ImageSize;
		}
		return ImageSize;
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (m_TextBox == null)
		{
			return;
		}
		if (((Control)m_TextBox).Parent != null)
		{
			((Control)m_TextBox).Parent.Controls.Remove((Control)(object)m_TextBox);
		}
		if (objOldContainer is MenuPanel)
		{
			CreateTextBox();
		}
		Control val = null;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)m_TextBox);
			}
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnAfterItemRemoved(item);
		ContainerControl = null;
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (m_TextBox != null && !newValue)
		{
			((Control)m_TextBox).Visible = newValue;
			CustomizeChanged();
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (m_TextBox != null)
		{
			((Control)m_TextBox).Visible = Displayed;
			CustomizeChanged();
		}
	}

	private void TextBoxMouseEnter(object sender, EventArgs e)
	{
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			Refresh();
		}
	}

	private void TextBoxMouseLeave(object sender, EventArgs e)
	{
		HideToolTip();
		if (m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
	}

	private void TextBoxMouseHover(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!DesignMode && (int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
	}

	private void TextBoxLostFocus(object sender, EventArgs e)
	{
		Text = ((Control)m_TextBox).Text;
		ReleaseFocus();
		HideToolTip();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnLostFocus()
	{
		if (m_TextBox != null)
		{
			Text = ((Control)m_TextBox).Text;
		}
		base.OnLostFocus();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (m_TextBox != null && !((Control)m_TextBox).Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)m_TextBox).Focus();
		}
	}

	private void TextBoxGotFocus(object sender, EventArgs e)
	{
		HideToolTip();
		Focus();
		if (!GetEnabled() || DesignMode || m_MenuVisibility != eMenuVisibility.VisibleIfRecentlyUsed || m_RecentlyUsed || !base.IsOnMenu)
		{
			return;
		}
		m_RecentlyUsed = true;
		for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
		{
			if (parent is IPersonalizedMenuItem personalizedMenuItem)
			{
				personalizedMenuItem.RecentlyUsed = true;
			}
		}
	}

	private void TextBoxKeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		HideToolTip();
		if (this.KeyDown != null)
		{
			this.KeyDown.Invoke((object)this, e);
		}
		if ((int)e.KeyCode == 13 && Parent is PopupItem)
		{
			RaiseClick();
		}
		if ((int)e.KeyCode == 27)
		{
			m_TextBox.ReleaseFocus();
		}
	}

	private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
	{
		if (this.KeyPress != null)
		{
			this.KeyPress.Invoke((object)this, e);
		}
	}

	private void TextBoxKeyUp(object sender, KeyEventArgs e)
	{
		if (this.KeyUp != null)
		{
			this.KeyUp.Invoke((object)this, e);
		}
	}

	private void TextBoxTextChanged(object sender, EventArgs e)
	{
		Text = ((Control)m_TextBox).Text;
		if (this.InputTextChanged != null)
		{
			this.InputTextChanged(this);
		}
		if (GetOwner() is DotNetBarManager dotNetBarManager)
		{
			dotNetBarManager.InvokeTextBoxItemTextChanged(this);
		}
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		base.OnIsOnCustomizeDialogChanged();
		CustomizeChanged();
	}

	protected override void OnDesignModeChanged()
	{
		base.OnDesignModeChanged();
		CustomizeChanged();
	}

	protected override void OnIsOnCustomizeMenuChanged()
	{
		base.OnIsOnCustomizeMenuChanged();
		CustomizeChanged();
	}

	private void CustomizeChanged()
	{
		if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
		{
			if (m_TextBox != null)
			{
				((Control)m_TextBox).Visible = false;
			}
		}
		else if (m_TextBox == null)
		{
			CreateTextBox();
		}
		else if (!((Control)m_TextBox).Visible)
		{
			((Control)m_TextBox).Visible = Displayed && Visible;
		}
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			Refresh();
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		if ((int)objArg.KeyCode == 27)
		{
			if (((Control)m_TextBox).Focused)
			{
				((TextBoxBase)m_TextBox).SelectionStart = 0;
				((Control)m_TextBox).Text = Text;
			}
		}
		else if ((int)objArg.KeyCode == 13 && !((Control)m_TextBox).Focused)
		{
			((Control)m_TextBox).Focus();
			objArg.Handled = true;
			return;
		}
		base.InternalKeyDown(objArg);
	}

	protected virtual Font GetFont()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			return val.Font;
		}
		return SystemFonts.DefaultFont;
	}

	private eTextFormat GetStringFormat()
	{
		return eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
	}

	protected override void OnTextChanged()
	{
		if (m_TextBox != null && ((Control)m_TextBox).Text != Text)
		{
			((Control)m_TextBox).Text = Text;
		}
		base.OnTextChanged();
	}

	public override void ReleaseFocus()
	{
		if (m_TextBox != null && ((Control)m_TextBox).Focused)
		{
			m_TextBox.ReleaseFocus();
		}
		base.ReleaseFocus();
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if (m_TextBox != null)
		{
			((Control)m_TextBox).Enabled = Enabled;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return m_TextBox.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return m_TextBox.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		m_TextBox.ResetFocusHighlightColor();
	}
}
