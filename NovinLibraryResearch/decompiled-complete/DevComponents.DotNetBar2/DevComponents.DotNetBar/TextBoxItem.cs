using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.TextBoxItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class TextBoxItem : ImageItem, IPersonalizedMenuItem
{
	public delegate void TextChangedEventHandler(object sender);

	private TextChangedEventHandler ᵡ;

	private KeyEventHandler ᵢ;

	private KeyPressEventHandler ᵣ;

	private KeyEventHandler ᵤ;

	private EventHandler _181E;

	private EventHandler _181F;

	private TextBoxX ᵥ;

	private bool ݡ;

	private bool ᚒ;

	private int ᵦ;

	private eMenuVisibility _0E7F;

	private bool _0E80;

	private static RoundRectangleShapeDescriptor _ᵧ = new RoundRectangleShapeDescriptor(3);

	private int _ᵨ;

	private string _ᚙ = "";

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
					შ();
				}
			}
			else
			{
				base.Bounds = value;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether item caption is always shown.")]
	[Browsable(true)]
	public bool AlwaysShowCaption
	{
		get
		{
			return ᚒ;
		}
		set
		{
			ᚒ = value;
		}
	}

	[Description("Indicates the Width of the Text Box part of the item.")]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[DefaultValue(64)]
	[Browsable(true)]
	public int TextBoxWidth
	{
		get
		{
			return ᵦ;
		}
		set
		{
			if (ᵦ != value)
			{
				ᵦ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "TextBoxWidth");
				}
				OnExternalSizeChange();
				OnAppearanceChanged();
			}
		}
	}

	[Category("Layout")]
	[DefaultValue(0)]
	[Description("Indicates explicit height of TextBox.")]
	public int TextBoxHeight
	{
		get
		{
			return _ᵨ;
		}
		set
		{
			_ᵨ = value;
		}
	}

	[Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.")]
	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	public bool UseSystemPasswordChar
	{
		get
		{
			return ((TextBox)ᵥ).UseSystemPasswordChar;
		}
		set
		{
			((TextBox)ᵥ).UseSystemPasswordChar = value;
		}
	}

	[Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
	[Localizable(true)]
	[Category("Behavior")]
	[DefaultValue('\0')]
	[RefreshProperties(RefreshProperties.Repaint)]
	public char PasswordChar
	{
		get
		{
			return ((TextBox)ᵥ).PasswordChar;
		}
		set
		{
			((TextBox)ᵥ).PasswordChar = value;
		}
	}

	[Obsolete("Please use Text property instead to access TextBox text.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[Browsable(true)]
	[Description("The text contained in the text box that user can edit.")]
	[Category("Appearance")]
	[DefaultValue("")]
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
	[Description("Indicates the item Caption displayed next to the text input.")]
	[Localizable(true)]
	[Category("Appearance")]
	public string Caption
	{
		get
		{
			return _ᚙ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ᚙ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Browsable(false)]
	public TextBox TextBox => (TextBox)(object)ᵥ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			if (ᵥ != null)
			{
				return ((TextBoxBase)ᵥ).SelectionStart;
			}
			return 0;
		}
		set
		{
			if (ᵥ != null)
			{
				((TextBoxBase)ᵥ).SelectionStart = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionLength
	{
		get
		{
			if (ᵥ != null)
			{
				return ((TextBoxBase)ᵥ).SelectionLength;
			}
			return 0;
		}
		set
		{
			if (ᵥ != null)
			{
				((TextBoxBase)ᵥ).SelectionLength = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string SelectedText
	{
		get
		{
			if (ᵥ != null)
			{
				return ((TextBoxBase)ᵥ).SelectedText;
			}
			return "";
		}
		set
		{
			if (ᵥ != null)
			{
				((TextBoxBase)ᵥ).SelectedText = value;
			}
		}
	}

	[DefaultValue(32767)]
	[Description("Indicates maximum number of characters the user can type or paste into the text box control. ")]
	public int MaxLength
	{
		get
		{
			return ((TextBoxBase)ᵥ).MaxLength;
		}
		set
		{
			if (ᵥ != null)
			{
				((TextBoxBase)ᵥ).MaxLength = value;
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates item's visibility when on pop-up menu.")]
	[Browsable(true)]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[DevCoBrowsable(true)]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return _0E7F;
		}
		set
		{
			if (_0E7F != value)
			{
				_0E7F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "MenuVisibility");
				}
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool RecentlyUsed
	{
		get
		{
			return _0E80;
		}
		set
		{
			if (_0E80 != value)
			{
				_0E80 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "RecentlyUsed");
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
			return ᵥ.WatermarkEnabled;
		}
		set
		{
			ᵥ.WatermarkEnabled = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue(null)]
	public Image WatermarkImage
	{
		get
		{
			return ᵥ.WatermarkImage;
		}
		set
		{
			ᵥ.WatermarkImage = value;
		}
	}

	[Description("Indicates watermark image alignment.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public ContentAlignment WatermarkImageAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ᵥ.WatermarkImageAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			ᵥ.WatermarkImageAlignment = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Category("Appearance")]
	public string WatermarkText
	{
		get
		{
			return ᵥ.WatermarkText;
		}
		set
		{
			ᵥ.WatermarkText = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates watermark font.")]
	public Font WatermarkFont
	{
		get
		{
			return ᵥ.WatermarkFont;
		}
		set
		{
			ᵥ.WatermarkFont = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return ᵥ.WatermarkColor;
		}
		set
		{
			ᵥ.WatermarkColor = value;
		}
	}

	[Description("Indicates watermark hiding behaviour.")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return ᵥ.WatermarkBehavior;
		}
		set
		{
			ᵥ.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	[DefaultValue(false)]
	[Category("Appearance")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return ᵥ.FocusHighlightEnabled;
		}
		set
		{
			ᵥ.FocusHighlightEnabled = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	[Browsable(true)]
	public Color FocusHighlightColor
	{
		get
		{
			return ᵥ.FocusHighlightColor;
		}
		set
		{
			ᵥ.FocusHighlightColor = value;
		}
	}

	[ParenthesizePropertyName(true)]
	[RefreshProperties(RefreshProperties.All)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public new ControlBindingsCollection DataBindings => ((Control)ᵥ).DataBindings;

	[Localizable(true)]
	[DefaultValue(0)]
	[Description("Indicates text-alignment inside of the text box.")]
	[Category("Appearance")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)ᵥ).TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)ᵥ).TextAlign = value;
		}
	}

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom => ᵥ.ButtonCustom;

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom2 => ᵥ.ButtonCustom2;

	public event TextChangedEventHandler InputTextChanged
	{
		add
		{
			TextChangedEventHandler textChangedEventHandler = ᵡ;
			TextChangedEventHandler textChangedEventHandler2;
			do
			{
				textChangedEventHandler2 = textChangedEventHandler;
				TextChangedEventHandler value2 = (TextChangedEventHandler)Delegate.Combine(textChangedEventHandler2, value);
				textChangedEventHandler = Interlocked.CompareExchange(ref ᵡ, value2, textChangedEventHandler2);
			}
			while ((object)textChangedEventHandler != textChangedEventHandler2);
		}
		remove
		{
			TextChangedEventHandler textChangedEventHandler = ᵡ;
			TextChangedEventHandler textChangedEventHandler2;
			do
			{
				textChangedEventHandler2 = textChangedEventHandler;
				TextChangedEventHandler value2 = (TextChangedEventHandler)Delegate.Remove(textChangedEventHandler2, value);
				textChangedEventHandler = Interlocked.CompareExchange(ref ᵡ, value2, textChangedEventHandler2);
			}
			while ((object)textChangedEventHandler != textChangedEventHandler2);
		}
	}

	public event KeyEventHandler KeyDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᵢ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵢ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᵢ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵢ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event KeyPressEventHandler KeyPress
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = ᵣ;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵣ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = ᵣ;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵣ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event KeyEventHandler KeyUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᵤ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵤ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᵤ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᵤ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler ButtonCustomClick
	{
		add
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ButtonCustom2Click
	{
		add
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		ᵩ();
		ݡ = false;
		ᚒ = false;
		ᵦ = 64;
		ImageSize = new Size(16, 16);
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
	}

	private void ᵩ()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		TextBoxX textBoxX = null;
		if (ᵥ != null)
		{
			((Control)ᵥ).MouseEnter -= ᵭ;
			((Control)ᵥ).MouseLeave -= _16FC;
			((Control)ᵥ).MouseHover -= ᵮ;
			((Control)ᵥ).LostFocus -= ᵯ;
			((Control)ᵥ).GotFocus -= ᵰ;
			((Control)ᵥ).TextChanged -= ᵶ;
			((Control)ᵥ).KeyDown -= new KeyEventHandler(ᵱ);
			((Control)ᵥ).KeyPress -= new KeyPressEventHandler(ᵲ);
			((Control)ᵥ).KeyUp -= new KeyEventHandler(ᵳ);
			((Control)ᵥ).MouseDown -= new MouseEventHandler(ᵵ);
			((Control)ᵥ).MouseUp -= new MouseEventHandler(ᵴ);
			ᵥ.ButtonCustomClick -= ᵪ;
			ᵥ.ButtonCustom2Click -= ᵫ;
			textBoxX = ᵥ;
			ᵥ = null;
		}
		ᵥ = new TextBoxX(isTextBoxItem: true);
		ᵥ.BorderStyle = (BorderStyle)0;
		((Control)ᵥ).AutoSize = false;
		((Control)ᵥ).Visible = false;
		((Control)ᵥ).TabStop = false;
		if (textBoxX != null)
		{
			((TextBoxBase)ᵥ).MaxLength = ((TextBoxBase)textBoxX).MaxLength;
			((TextBox)ᵥ).UseSystemPasswordChar = ((TextBox)textBoxX).UseSystemPasswordChar;
			((TextBox)ᵥ).PasswordChar = ((TextBox)textBoxX).PasswordChar;
			ᵥ.WatermarkEnabled = textBoxX.WatermarkEnabled;
			ᵥ.WatermarkText = textBoxX.WatermarkText;
			ᵥ.WatermarkFont = textBoxX.WatermarkFont;
			ᵥ.WatermarkColor = textBoxX.WatermarkColor;
			ᵥ.WatermarkBehavior = textBoxX.WatermarkBehavior;
			ᵥ.FocusHighlightEnabled = textBoxX.FocusHighlightEnabled;
			ᵥ.FocusHighlightColor = textBoxX.FocusHighlightColor;
			((Component)(object)textBoxX).Dispose();
		}
		((Control)ᵥ).MouseEnter += ᵭ;
		((Control)ᵥ).MouseLeave += _16FC;
		((Control)ᵥ).MouseHover += ᵮ;
		((Control)ᵥ).LostFocus += ᵯ;
		((Control)ᵥ).GotFocus += ᵰ;
		((Control)ᵥ).TextChanged += ᵶ;
		((Control)ᵥ).KeyDown += new KeyEventHandler(ᵱ);
		((Control)ᵥ).KeyPress += new KeyPressEventHandler(ᵲ);
		((Control)ᵥ).KeyUp += new KeyEventHandler(ᵳ);
		((Control)ᵥ).MouseDown += new MouseEventHandler(ᵵ);
		((Control)ᵥ).MouseUp += new MouseEventHandler(ᵴ);
		ᵥ.ButtonCustomClick += ᵪ;
		ᵥ.ButtonCustom2Click += ᵫ;
		((Control)ᵥ).Text = Text;
		((TextBoxBase)ᵥ).SelectionStart = 0;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)ᵥ);
			}
		}
		if (Displayed)
		{
			((Control)ᵥ).Visible = true;
			Refresh();
		}
		ნ();
	}

	private void ᵪ(object _٠, EventArgs ؿ)
	{
		if (_181E != null)
		{
			_181E(this, ؿ);
		}
	}

	private void ᵫ(object _٠, EventArgs ؿ)
	{
		if (_181F != null)
		{
			_181F(this, ؿ);
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
		textBoxItem.TextBoxWidth = ᵦ;
	}

	protected override void Dispose(bool disposing)
	{
		_ = ᵥ;
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("Caption", _ᚙ);
		itemXmlElement.SetAttribute("TextBoxWidth", XmlConvert.ToString(ᵦ));
		itemXmlElement.SetAttribute("AlwaysShowCaption", XmlConvert.ToString(ᚒ));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)_0E7F));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(_0E80));
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("Caption"))
		{
			Caption = itemXmlElement.GetAttribute("Caption");
		}
		ᵦ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("TextBoxWidth"));
		ᚒ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AlwaysShowCaption"));
		_0E7F = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		_0E80 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Expected O, but got Unknown
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		bool enabled = GetEnabled(pa.ContainerControl);
		Size size = ო();
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
			{
				size.Width += 7;
				displayRectangle.Width -= size.Width;
				if (!pa.RightToLeft)
				{
					displayRectangle.X += size.Width;
				}
				if (base.IsOnCustomizeMenu)
				{
					size.Width += size.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
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
							LinearGradientBrush val = BarFunctions.ॴ(new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
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
						LinearGradientBrush val2 = BarFunctions.ॴ(rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
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
					displayRectangle.X += size.Height + 8;
					displayRectangle.Width -= size.Height + 8;
				}
				else
				{
					displayRectangle.X += size.Height + 4;
					displayRectangle.Width -= size.Height + 4;
				}
			}
			if (flag && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && enabled && (ݡ || Focused))
			{
				Rectangle displayRectangle2 = DisplayRectangle;
				displayRectangle2.Inflate(-1, 0);
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && !(pa.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007ButtonItemStateColorTable mouseOver = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
					ᯊ.ᯓ(graphics, mouseOver, displayRectangle2, _ᵧ);
				}
				else
				{
					if (!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						LinearGradientBrush val3 = BarFunctions.ॴ(displayRectangle2, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val3, displayRectangle2);
						((Brush)val3).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), displayRectangle2);
					}
					_089F.र(graphics, new Pen(pa.Colors.ItemHotBorder), displayRectangle2);
				}
			}
			if (_ᚙ != "" && (ᚒ || flag))
			{
				eTextFormat eTextFormat2 = ಲ();
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
					TextDrawing.DrawString(graphics, _ᚙ, font, color, bounds, eTextFormat2);
				}
				else
				{
					TextDrawing.DrawString(graphics, _ᚙ, font, color, bounds, eTextFormat2);
				}
				Size size2 = TextDrawing.MeasureString(graphics, _ᚙ, font, 0, eTextFormat2);
				if (!pa.RightToLeft)
				{
					displayRectangle.X += size2.Width + 8;
				}
				displayRectangle.Width -= size2.Width + 8;
			}
			int num = Math.Max(19, (_ᵨ > 0) ? _ᵨ : ((TextBoxBase)ᵥ).PreferredHeight);
			if (DesignMode && !flag)
			{
				Rectangle rectangle2 = displayRectangle;
				if (ᵥ != null)
				{
					rectangle2.Inflate(-5, -3);
					rectangle2.Y += (rectangle2.Height - (num + 3)) / 2;
					rectangle2.Height = num + 3;
				}
				graphics.FillRectangle(SystemBrushes.Window, rectangle2);
				if (Text != "")
				{
					rectangle2.Inflate(-1, -1);
					TextDrawing.DrawString(graphics, Text, GetFont(), enabled ? pa.Colors.ItemText : pa.Colors.ItemDisabledText, rectangle2, ಲ());
				}
			}
			if (ᵥ != null)
			{
				displayRectangle.Inflate(-4, -3);
				displayRectangle.X--;
				displayRectangle.Y += (displayRectangle.Height - num) / 2;
				displayRectangle.Height = num;
				((Control)ᵥ).Size = new Size(displayRectangle.Width, (_ᵨ > 0) ? _ᵨ : ((TextBoxBase)ᵥ).PreferredHeight);
				Point location = displayRectangle.Location;
				if (pa.RightToLeft)
				{
					location.Offset(0, (displayRectangle.Height - ((Control)ᵥ).Height) / 2);
				}
				else
				{
					location.Offset(2, (displayRectangle.Height - ((Control)ᵥ).Height) / 2);
				}
				((Control)ᵥ).Location = location;
				if (BarFunctions.IsOffice2007Style(effectiveStyle))
				{
					Color color2 = Color.Empty;
					ElementStyle elementStyle = ᵬ();
					if (!enabled)
					{
						color2 = SystemColors.ControlDark;
					}
					else if (elementStyle != null)
					{
						color2 = elementStyle.BorderColor;
					}
					if (!color2.IsEmpty)
					{
						displayRectangle = new Rectangle(displayRectangle.Location, new Size(((Control)ᵥ).Width + 2, num));
						DisplayHelp.FillRectangle(graphics, displayRectangle, ((Control)ᵥ).BackColor);
						displayRectangle.Inflate(1, 1);
						if (!enabled)
						{
							graphics.FillRectangle(SystemBrushes.Control, displayRectangle);
						}
						DisplayHelp.DrawRectangle(graphics, color2, displayRectangle);
					}
				}
				else if (enabled && (ݡ || Focused))
				{
					switch (effectiveStyle)
					{
					case eDotNetBarStyle.Office2000:
						displayRectangle = new Rectangle(((Control)ᵥ).Location, ((Control)ᵥ).Size);
						displayRectangle.Inflate(2, 2);
						ControlPaint.DrawBorder3D(graphics, displayRectangle, (Border3DStyle)2, (Border3DSide)2063);
						break;
					case eDotNetBarStyle.OfficeXP:
					case eDotNetBarStyle.Office2003:
					case eDotNetBarStyle.VS2005:
						displayRectangle = new Rectangle(((Control)ᵥ).Location, ((Control)ᵥ).Size);
						displayRectangle.Inflate(1, 1);
						_089F.र(graphics, SystemPens.Highlight, displayRectangle);
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
				BarFunctions.DrawMenuCheckBox(pa, r, effectiveStyle, ݡ);
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
			if (ݡ && !DesignMode)
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
						LinearGradientBrush val4 = BarFunctions.ॴ(displayRectangle, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val4, displayRectangle);
						((Brush)val4).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), displayRectangle);
					}
					_089F.र(graphics, new Pen(pa.Colors.ItemHotBorder), displayRectangle);
				}
			}
			displayRectangle = new Rectangle(m_Rect.Top, -m_Rect.Right, m_Rect.Height, m_Rect.Width);
			graphics.RotateTransform(90f);
			eTextFormat eTextFormat3 = ಲ();
			eTextFormat3 |= eTextFormat.HorizontalCenter;
			if (ݡ && enabled)
			{
				TextDrawing.DrawStringLegacy(graphics, text, GetFont(), pa.Colors.ItemHotText, displayRectangle, eTextFormat3);
			}
			else
			{
				TextDrawing.DrawStringLegacy(graphics, text, GetFont(), enabled ? pa.Colors.ItemText : pa.Colors.ItemDisabledText, displayRectangle, eTextFormat3);
			}
			graphics.ResetTransform();
		}
		if (Focused && DesignMode)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(graphics);
	}

	private ElementStyle ᵬ()
	{
		ElementStyle result = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			result = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.StyleClasses[ElementStyleClassKeys.TextBoxBorderKey] as ElementStyle;
		}
		return result;
	}

	private void შ()
	{
		if (ᵥ != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-5, -3);
			((Control)ᵥ).Location = displayRectangle.Location;
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
			m_Rect.Width = ᵦ + 4;
			if (_ᚙ != "" && (ᚒ || isOnMenu))
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null && BaseItem.IsHandleValid(val))
				{
					Graphics val2 = BarFunctions.CreateGraphics(val);
					try
					{
						Size size = TextDrawing.MeasureString(val2, _ᚙ, GetFont(), 0, ಲ());
						if (size.Height > SubItemsImageSize.Height)
						{
							m_Rect.Height = size.Height + 4;
						}
						m_Rect.Width = ᵦ + 4 + size.Width + 8;
					}
					finally
					{
						val2.Dispose();
					}
				}
			}
			if (ᵥ != null)
			{
				if (ᵥ.ButtonCustom.Visible && ᵥ.ButtonCustom.Image != null)
				{
					m_Rect.Height = Math.Max(m_Rect.Height, ᵥ.ButtonCustom.Image.Height + 6);
				}
				if (ᵥ.ButtonCustom2.Visible && ᵥ.ButtonCustom2.Image != null)
				{
					m_Rect.Height = Math.Max(m_Rect.Height, ᵥ.ButtonCustom2.Image.Height + 6);
				}
			}
			Size size2 = ო();
			if (ᵥ != null)
			{
				int num = Math.Max(19, (_ᵨ > 0) ? _ᵨ : ((TextBoxBase)ᵥ).PreferredHeight) + 2;
				if (m_Rect.Height < num)
				{
					m_Rect.Height = num;
				}
			}
			if (base.IsOnMenu && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
			{
				m_Rect.Width += size2.Width + 7;
			}
			if (base.IsOnCustomizeMenu)
			{
				m_Rect.Width += size2.Height + 2;
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
					Size size3 = TextDrawing.MeasureString(val4, text, GetFont(), 0, ಲ());
					if (size3.Height > SubItemsImageSize.Height)
					{
						m_Rect.Width = size3.Height + 4;
					}
					m_Rect.Height = size3.Width + 8;
				}
				finally
				{
					val4.Dispose();
				}
			}
		}
		base.RecalcSize();
	}

	private Size ო()
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
		if (ᵥ == null)
		{
			return;
		}
		if (((Control)ᵥ).Parent != null)
		{
			((Control)ᵥ).Parent.Controls.Remove((Control)(object)ᵥ);
		}
		if (objOldContainer is MenuPanel)
		{
			ᵩ();
		}
		Control val = null;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)ᵥ);
			}
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		ContainerControl = null;
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (ᵥ != null && !newValue)
		{
			((Control)ᵥ).Visible = newValue;
			ნ();
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (ᵥ != null)
		{
			((Control)ᵥ).Visible = Displayed;
			ნ();
		}
	}

	private void ᵭ(object _٠, EventArgs ؿ)
	{
		if (!ݡ)
		{
			ݡ = true;
			Refresh();
		}
	}

	private void _16FC(object _٠, EventArgs ؿ)
	{
		HideToolTip();
		if (ݡ)
		{
			ݡ = false;
			Refresh();
		}
	}

	private void ᵮ(object _٠, EventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!DesignMode && (int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
	}

	private void ᵯ(object _٠, EventArgs ؿ)
	{
		Text = ((Control)ᵥ).Text;
		ReleaseFocus();
		HideToolTip();
		Refresh();
		OnLostFocus(ؿ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnLostFocus()
	{
		if (ᵥ != null)
		{
			Text = ((Control)ᵥ).Text;
		}
		base.OnLostFocus();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (ᵥ != null && !((Control)ᵥ).Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)ᵥ).Focus();
		}
	}

	private void ᵰ(object _٠, EventArgs ؿ)
	{
		HideToolTip();
		Focus();
		if (GetEnabled() && !DesignMode && _0E7F == eMenuVisibility.VisibleIfRecentlyUsed && !_0E80 && base.IsOnMenu)
		{
			_0E80 = true;
			for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
			{
				if (parent is IPersonalizedMenuItem personalizedMenuItem)
				{
					personalizedMenuItem.RecentlyUsed = true;
				}
			}
		}
		OnGotFocus(ؿ);
	}

	private void ᵱ(object _٠, KeyEventArgs ؿ)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		HideToolTip();
		if (ᵢ != null)
		{
			ᵢ.Invoke((object)this, ؿ);
		}
		if ((int)ؿ.KeyCode == 13 && Parent is PopupItem)
		{
			RaiseClick();
		}
		if (!ؿ.Handled && (int)ؿ.KeyCode == 27)
		{
			ᵥ.ᶊ();
		}
	}

	private void ᵲ(object _٠, KeyPressEventArgs ؿ)
	{
		if (ᵣ != null)
		{
			ᵣ.Invoke((object)this, ؿ);
		}
	}

	private void ᵳ(object _٠, KeyEventArgs ؿ)
	{
		if (ᵤ != null)
		{
			ᵤ.Invoke((object)this, ؿ);
		}
	}

	private void ᵴ(object _٠, MouseEventArgs ؿ)
	{
		RaiseMouseUp(ؿ);
	}

	private void ᵵ(object _٠, MouseEventArgs ؿ)
	{
		RaiseMouseDown(ؿ);
	}

	private void ᵶ(object _٠, EventArgs ؿ)
	{
		Text = ((Control)ᵥ).Text;
		if (ᵡ != null)
		{
			ᵡ(this);
		}
		if (GetOwner() is DotNetBarManager dotNetBarManager)
		{
			dotNetBarManager.InvokeTextBoxItemTextChanged(this);
		}
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		base.OnIsOnCustomizeDialogChanged();
		ნ();
	}

	protected override void OnDesignModeChanged()
	{
		base.OnDesignModeChanged();
		ნ();
	}

	protected override void OnIsOnCustomizeMenuChanged()
	{
		base.OnIsOnCustomizeMenuChanged();
		ნ();
	}

	private void ნ()
	{
		if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
		{
			if (ᵥ != null)
			{
				((Control)ᵥ).Visible = false;
			}
		}
		else if (ᵥ == null)
		{
			ᵩ();
		}
		else if (!((Control)ᵥ).Visible)
		{
			((Control)ᵥ).Visible = Displayed && Visible;
		}
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!ݡ)
		{
			ݡ = true;
			Refresh();
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (ݡ)
		{
			ݡ = false;
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
			if (((Control)ᵥ).Focused)
			{
				((TextBoxBase)ᵥ).SelectionStart = 0;
				((Control)ᵥ).Text = Text;
			}
		}
		else if ((int)objArg.KeyCode == 13 && !((Control)ᵥ).Focused)
		{
			((Control)ᵥ).Focus();
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

	private eTextFormat ಲ()
	{
		return eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
	}

	protected override void OnTextChanged()
	{
		if (ᵥ != null && ((Control)ᵥ).Text != Text)
		{
			((Control)ᵥ).Text = Text;
		}
		base.OnTextChanged();
	}

	public override void ReleaseFocus()
	{
		if (ᵥ != null && ((Control)ᵥ).Focused)
		{
			ᵥ.ᶊ();
		}
		base.ReleaseFocus();
	}

	public void SelectAll()
	{
		if (ᵥ != null)
		{
			((TextBoxBase)ᵥ).SelectAll();
		}
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if (ᵥ != null)
		{
			((Control)ᵥ).Enabled = Enabled;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return ᵥ.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return ᵥ.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		ᵥ.ResetFocusHighlightColor();
	}
}
