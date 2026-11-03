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
using DevComponents.DotNetBar.Ribbon;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ComboBoxItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DefaultEvent("Click")]
public class ComboBoxItem : ImageItem, IPersonalizedMenuItem
{
	private ComboBoxEx ᚐ;

	private int ᚑ = 64;

	private bool ݡ;

	private bool ᚒ;

	private bool ᚓ;

	private eMenuVisibility _0E7F;

	private bool _0E80;

	internal bool _ᚔ;

	private bool ᚕ;

	private EventHandler ᚖ;

	private EventHandler _05CB;

	private Color _ᚗ = Color.Empty;

	private bool _ᚘ;

	private string _ᚙ = "";

	[Browsable(true)]
	[Description("Gets or sets the accessible role of the item.")]
	[Category("Accessibility")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.AccessibleRole;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.AccessibleRole = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether combo box generates the audible alert when Enter key is pressed.")]
	[DevCoBrowsable(true)]
	public bool PreventEnterBeep
	{
		get
		{
			return ᚕ;
		}
		set
		{
			ᚕ = value;
			if (ᚐ != null)
			{
				ᚐ.PreventEnterBeep = value;
			}
		}
	}

	[Description("Indicates color of combo box label.")]
	[Category("Columns")]
	public Color LabelForeColor
	{
		get
		{
			return _ᚗ;
		}
		set
		{
			_ᚗ = value;
			Refresh();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			if (base.Bounds != value)
			{
				bool flag = base.Bounds.Top != value.Top;
				bool flag2 = base.Bounds.Left != value.Left;
				base.Bounds = value;
				if ((flag || flag2) && (ContainerControl is ItemPanel || ContainerControl is ExplorerBar))
				{
					შ();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates width of the of the drop-down portion of a combo box.")]
	public int DropDownWidth
	{
		get
		{
			return ((ComboBox)ᚐ).DropDownWidth;
		}
		set
		{
			((ComboBox)ᚐ).DropDownWidth = value;
		}
	}

	[Description("Indicates height of the of the drop-down portion of a combo box.")]
	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Layout")]
	public int DropDownHeight
	{
		get
		{
			return ((ComboBox)ᚐ).DropDownHeight;
		}
		set
		{
			((ComboBox)ᚐ).DropDownHeight = value;
		}
	}

	[Category("Layout")]
	[Description("Indicates the Width of the combo box part of the item.")]
	[DefaultValue(64)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int ComboWidth
	{
		get
		{
			return ᚑ;
		}
		set
		{
			if (ᚑ != value)
			{
				ᚑ = value;
				if (Name != "" && GlobalItem)
				{
					BarFunctions.ॠ(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["ComboWidth"], ᚑ);
				}
				NeedRecalcSize = true;
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Description("Indicates whether control automatically releases its input focus when selection is made using keyboard.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool AutoReleaseFocus
	{
		get
		{
			return ᚐ.AutoReleaseFocus;
		}
		set
		{
			ᚐ.AutoReleaseFocus = value;
		}
	}

	[Browsable(false)]
	public ComboBoxEx ComboBoxEx => ᚐ;

	[Description("Indicates whether item caption is always shown.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool AlwaysShowCaption
	{
		get
		{
			return ᚒ;
		}
		set
		{
			if (ᚒ != value)
			{
				ᚒ = value;
			}
			NeedRecalcSize = true;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ComboItemsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Data")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public ObjectCollection Items => ᚐ.Items;

	[Description("Gets or sets a value specifying the style of the combo box.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)ᚐ).DropDownStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)ᚐ).DropDownStyle = value;
		}
	}

	[DefaultValue(0)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionStart
	{
		get
		{
			return ((ComboBox)ᚐ).SelectionStart;
		}
		set
		{
			((ComboBox)ᚐ).SelectionStart = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(0)]
	public int SelectionLength
	{
		get
		{
			return ((ComboBox)ᚐ).SelectionLength;
		}
		set
		{
			((ComboBox)ᚐ).SelectionLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	public string SelectedText
	{
		get
		{
			return ((ComboBox)ᚐ).SelectedText;
		}
		set
		{
			((ComboBox)ᚐ).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(null)]
	public object SelectedItem
	{
		get
		{
			return ((ComboBox)ᚐ).SelectedItem;
		}
		set
		{
			((ComboBox)ᚐ).SelectedItem = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(-1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			return ((ListControl)ᚐ).SelectedIndex;
		}
		set
		{
			((ListControl)ᚐ).SelectedIndex = value;
		}
	}

	[Description("Automatically loads all the fonts available into the combo box.")]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool FontCombo
	{
		get
		{
			return ᚓ;
		}
		set
		{
			if (ᚓ != value)
			{
				ᚓ = value;
				if (ᚓ)
				{
					ᚐ.LoadFonts();
					((ComboBox)ᚐ).DropDownStyle = (ComboBoxStyle)2;
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the height of an item in the combo box.")]
	[DefaultValue(15)]
	[Browsable(true)]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)ᚐ).ItemHeight;
		}
		set
		{
			((ComboBox)ᚐ).ItemHeight = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
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
				if (m_Name != "" && GlobalItem)
				{
					BarFunctions.ॠ(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["MenuVisibility"], _0E7F);
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
			return _0E80;
		}
		set
		{
			if (_0E80 != value)
			{
				_0E80 = value;
				if (m_Name != "" && GlobalItem)
				{
					BarFunctions.ॠ(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["RecentlyUsed"], _0E80);
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("Please use Text property instead to access ComboBox text.")]
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

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("The text contained in the underlining Control portion of the item.")]
	[Browsable(true)]
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

	[Description("Indicates the item Caption displayed next to the combo box.")]
	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue("")]
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
		}
	}

	[DefaultValue("")]
	[DevCoBrowsable(true)]
	[Description("Indicates string that specifies the property of the data source whose contents you want to display.")]
	[Browsable(true)]
	[Category("Data")]
	public virtual string DisplayMember
	{
		get
		{
			if (ᚐ != null)
			{
				return ((ListControl)ᚐ).DisplayMember;
			}
			return "";
		}
		set
		{
			if (ᚐ != null)
			{
				((ListControl)ᚐ).DisplayMember = value;
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies whether combo box is drawn using themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return ᚐ.ThemeAware;
		}
		set
		{
			ᚐ.ThemeAware = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

	[Description("Indicates watermark font.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return ᚐ.WatermarkFont;
		}
		set
		{
			ᚐ.WatermarkFont = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates watermark text color.")]
	[Category("Appearance")]
	public Color WatermarkColor
	{
		get
		{
			return ᚐ.WatermarkColor;
		}
		set
		{
			ᚐ.WatermarkColor = value;
		}
	}

	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[DefaultValue(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return ᚐ.WatermarkEnabled;
		}
		set
		{
			ᚐ.WatermarkEnabled = value;
		}
	}

	[DefaultValue("")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Category("Appearance")]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	public string WatermarkText
	{
		get
		{
			return ᚐ.WatermarkText;
		}
		set
		{
			ᚐ.WatermarkText = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return ᚐ.WatermarkBehavior;
		}
		set
		{
			ᚐ.WatermarkBehavior = value;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[ParenthesizePropertyName(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public new ControlBindingsCollection DataBindings => ((Control)ᚐ).DataBindings;

	[DefaultValue(false)]
	[Description("Indicates the appearance of the control.")]
	[Category("Appearance")]
	[Browsable(true)]
	public bool IsStandalone
	{
		get
		{
			return ᚐ.IsStandalone;
		}
		set
		{
			ᚐ.IsStandalone = value;
		}
	}

	public event EventHandler ComboBoxTextChanged
	{
		add
		{
			EventHandler eventHandler = ᚖ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚖ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᚖ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚖ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler SelectedIndexChanged
	{
		add
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ComboBoxItem()
		: this("", "")
	{
	}

	public ComboBoxItem(string sName)
		: this(sName, "")
	{
	}

	public ComboBoxItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		ᚚ();
		ᚑ = 64;
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
		IsAccessible = true;
		AccessibleRole = (AccessibleRole)46;
	}

	private void ᚚ()
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		if (ᚐ != null)
		{
			((Component)(object)ᚐ).Dispose();
			ᚐ = null;
		}
		ᚐ = new ComboBoxEx();
		ᚐ.IsStandalone = false;
		((Control)ᚐ).TabStop = false;
		((Control)ᚐ).TabIndex = 9999;
		ᚐ.Style = Style;
		((ComboBox)ᚐ).DrawMode = (DrawMode)1;
		((ComboBox)ᚐ).IntegralHeight = false;
		ᚐ.ThemeAware = false;
		((Control)ᚐ).Visible = false;
		((Control)ᚐ).Text = Text;
		((ComboBox)ᚐ).SelectionStart = 0;
		((ComboBox)ᚐ).DropDownStyle = (ComboBoxStyle)2;
		((Control)ᚐ).LostFocus += _169B;
		((Control)ᚐ).GotFocus += ᚠ;
		((Control)ᚐ).MouseHover += _169C;
		((Control)ᚐ).MouseEnter += _169D;
		((Control)ᚐ).MouseLeave += _169E;
		((Control)ᚐ).VisibleChanged += _169F;
		ᚐ.DropDownChange += ᚣ;
		((Control)ᚐ).TextChanged += ᚥ;
		((Control)ᚐ).KeyDown += new KeyEventHandler(ᚢ);
		((ComboBox)ᚐ).SelectedIndexChanged += ᚡ;
		ᚐ.PreventEnterBeep = ᚕ;
		ᚐ.ᛂ = this;
		if (ᚓ)
		{
			ᚐ.LoadFonts();
		}
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)ᚐ);
				((Control)ᚐ).Refresh();
			}
		}
		if (Displayed)
		{
			((Control)ᚐ).Visible = true;
		}
	}

	public override BaseItem Copy()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		ComboBoxItem comboBoxItem = new ComboBoxItem(Name);
		CopyToItem(comboBoxItem);
		comboBoxItem.DropDownStyle = DropDownStyle;
		comboBoxItem.AlwaysShowCaption = AlwaysShowCaption;
		comboBoxItem.FontCombo = FontCombo;
		comboBoxItem.ItemHeight = ItemHeight;
		comboBoxItem.Caption = Caption;
		return comboBoxItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		((ComboBoxItem)copy)._ᚔ = true;
		try
		{
			ComboBoxItem comboBoxItem = copy as ComboBoxItem;
			base.CopyToItem(comboBoxItem);
			comboBoxItem.ComboWidth = ᚑ;
			comboBoxItem.FontCombo = ᚓ;
			comboBoxItem.PreventEnterBeep = ᚕ;
			if (ᚐ == null)
			{
				return;
			}
			comboBoxItem.DisplayMember = ((ListControl)ᚐ).DisplayMember;
			ComboBoxEx comboBoxEx = comboBoxItem.ComboBoxEx;
			if (!ᚓ)
			{
				foreach (object item in ᚐ.Items)
				{
					comboBoxEx.Items.Add(item);
				}
			}
			((ListControl)comboBoxEx).SelectedIndex = ((ListControl)ᚐ).SelectedIndex;
		}
		finally
		{
			((ComboBoxItem)copy)._ᚔ = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return ((Control)ᚐ).AccessibilityObject;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected I4, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected I4, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected I4, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected I4, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected I4, but got Unknown
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("ComboWidth", XmlConvert.ToString(ᚑ));
		itemXmlElement.SetAttribute("FontCombo", XmlConvert.ToString(ᚓ));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)_0E7F));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(_0E80));
		itemXmlElement.SetAttribute("DropDownStyle", XmlConvert.ToString((int)((ComboBox)ᚐ).DropDownStyle));
		itemXmlElement.SetAttribute("ThemeAware", XmlConvert.ToString(ᚐ.ThemeAware));
		itemXmlElement.SetAttribute("AlwaysShowCaption", XmlConvert.ToString(ᚒ));
		itemXmlElement.SetAttribute("ItemHeight", XmlConvert.ToString(((ComboBox)ᚐ).ItemHeight));
		if (((ListControl)ᚐ).DisplayMember != "")
		{
			itemXmlElement.SetAttribute("DisplayMembers", ((ListControl)ᚐ).DisplayMember);
		}
		if (ᚕ)
		{
			itemXmlElement.SetAttribute("nobeep", XmlConvert.ToString(ᚕ));
		}
		if (ᚓ || ᚐ.Items.Count <= 0)
		{
			return;
		}
		XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("cbitems");
		itemXmlElement.AppendChild(xmlElement);
		foreach (object item in ᚐ.Items)
		{
			if (item is ComboItem comboItem)
			{
				XmlElement xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("ci");
				if (!comboItem.BackColor.IsEmpty)
				{
					xmlElement2.SetAttribute("bc", BarFunctions.ColorToString(comboItem.BackColor));
				}
				if (comboItem.FontName != "")
				{
					xmlElement2.SetAttribute("fn", comboItem.FontName);
				}
				if (comboItem.FontSize != 8f)
				{
					xmlElement2.SetAttribute("fs", XmlConvert.ToString(comboItem.FontSize));
				}
				xmlElement2.SetAttribute("fy", XmlConvert.ToString((int)comboItem.FontStyle));
				if (!comboItem.ForeColor.IsEmpty)
				{
					xmlElement2.SetAttribute("fc", BarFunctions.ColorToString(comboItem.ForeColor));
				}
				BarFunctions.SerializeImage(comboItem.Image, xmlElement2);
				if (comboItem.ImageIndex >= 0)
				{
					xmlElement2.SetAttribute("img", XmlConvert.ToString(comboItem.ImageIndex));
				}
				if ((int)comboItem.ImagePosition != 0)
				{
					xmlElement2.SetAttribute("ip", XmlConvert.ToString((int)comboItem.ImagePosition));
				}
				xmlElement2.SetAttribute("text", comboItem.Text);
				xmlElement2.SetAttribute("ta", XmlConvert.ToString((int)comboItem.TextAlignment));
				xmlElement2.SetAttribute("tla", XmlConvert.ToString((int)comboItem.TextLineAlignment));
				if (((ComboBox)ᚐ).SelectedItem == item)
				{
					xmlElement2.SetAttribute("selected", "1");
				}
				xmlElement.AppendChild(xmlElement2);
			}
			else
			{
				XmlElement xmlElement3 = itemXmlElement.OwnerDocument.CreateElement("co");
				xmlElement3.InnerText = item.ToString();
				xmlElement.AppendChild(xmlElement3);
				if (((ComboBox)ᚐ).SelectedItem == item)
				{
					xmlElement3.SetAttribute("selected", "1");
				}
			}
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ᚑ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("ComboWidth"));
		ᚓ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("FontCombo"));
		_0E7F = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		_0E80 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
		if (itemXmlElement.HasAttribute("DropDownStyle"))
		{
			((ComboBox)ᚐ).DropDownStyle = (ComboBoxStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("DropDownStyle"));
		}
		if (itemXmlElement.HasAttribute("CText"))
		{
			Text = itemXmlElement.GetAttribute("CText");
		}
		if (itemXmlElement.HasAttribute("ThemeAware"))
		{
			ᚐ.ThemeAware = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("ThemeAware"));
		}
		else
		{
			ᚐ.ThemeAware = true;
		}
		if (itemXmlElement.HasAttribute("AlwaysShowCaption"))
		{
			ᚒ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AlwaysShowCaption"));
		}
		if (itemXmlElement.HasAttribute("nobeep"))
		{
			PreventEnterBeep = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("nobeep"));
		}
		XmlNodeList elementsByTagName = itemXmlElement.GetElementsByTagName("cbitems");
		if (!ᚓ && elementsByTagName.Count > 0)
		{
			foreach (XmlElement childNode in elementsByTagName[0].ChildNodes)
			{
				if (childNode.Name == "ci")
				{
					ComboItem comboItem = new ComboItem();
					if (childNode.HasAttribute("bc"))
					{
						comboItem.BackColor = BarFunctions.ColorFromString(childNode.GetAttribute("bk"));
					}
					if (childNode.HasAttribute("fn"))
					{
						comboItem.FontName = childNode.GetAttribute("fn");
					}
					if (childNode.HasAttribute("fs"))
					{
						comboItem.FontSize = XmlConvert.ToSingle(childNode.GetAttribute("fs"));
					}
					if (childNode.HasAttribute("fy"))
					{
						comboItem.FontStyle = (FontStyle)XmlConvert.ToInt32(childNode.GetAttribute("fy"));
					}
					if (childNode.HasAttribute("fc"))
					{
						comboItem.ForeColor = BarFunctions.ColorFromString(childNode.GetAttribute("fc"));
					}
					comboItem.Image = BarFunctions.DeserializeImage(childNode);
					if (childNode.HasAttribute("img"))
					{
						comboItem.ImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("img"));
					}
					if (childNode.HasAttribute("ip"))
					{
						comboItem.ImagePosition = (HorizontalAlignment)XmlConvert.ToInt32(childNode.GetAttribute("ip"));
					}
					if (childNode.HasAttribute("ItemHeight"))
					{
						((ComboBox)ᚐ).ItemHeight = XmlConvert.ToInt32(childNode.GetAttribute("ItemHeight"));
					}
					comboItem.Text = childNode.GetAttribute("text");
					comboItem.TextAlignment = (StringAlignment)XmlConvert.ToInt32(childNode.GetAttribute("ta"));
					comboItem.TextLineAlignment = (StringAlignment)XmlConvert.ToInt32(childNode.GetAttribute("tla"));
					ᚐ.Items.Add((object)comboItem);
					if (childNode.HasAttribute("selected") && childNode.GetAttribute("selected") == "1")
					{
						((ComboBox)ᚐ).SelectedItem = comboItem;
					}
				}
				else if (childNode.Name == "co")
				{
					ᚐ.Items.Add((object)childNode.InnerText);
					if (childNode.HasAttribute("selected") && childNode.GetAttribute("selected") == "1")
					{
						((ComboBox)ᚐ).SelectedItem = ᚐ.Items[ᚐ.Items.Count - 1];
					}
				}
			}
		}
		if (ᚓ)
		{
			ᚐ.LoadFonts();
		}
		if (ᚐ != null)
		{
			((Control)ᚐ).Enabled = Enabled;
		}
		if (itemXmlElement.HasAttribute("DisplayMembers") && ᚐ != null)
		{
			((ListControl)ᚐ).DisplayMember = itemXmlElement.GetAttribute("DisplayMembers");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLabelForeColor()
	{
		return !_ᚗ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLabelForeColor()
	{
		LabelForeColor = Color.Empty;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		Size size = ო();
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		bool enabled = GetEnabled(pa.ContainerControl);
		Color color = (enabled ? SystemColors.ControlText : SystemColors.ControlDark);
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && pa.ໝ is Office2007Renderer && ((Office2007Renderer)pa.ໝ).ColorTable.ButtonItemColors.Count > 0)
		{
			color = (enabled ? ((Office2007Renderer)pa.ໝ).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText);
		}
		if (!_ᚗ.IsEmpty)
		{
			color = _ᚗ;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
			{
				size.Width += 7;
				displayRectangle.Width -= size.Width;
				displayRectangle.X += size.Width;
				if (base.IsOnCustomizeMenu)
				{
					size.Width += size.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
				if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
				{
					if (!pa.Colors.MenuUnusedSide2.IsEmpty)
					{
						LinearGradientBrush val = BarFunctions.ॴ(rectangle, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
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
				if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
					if (menu != null && !menu.SideBorder.IsEmpty)
					{
						if (pa.RightToLeft)
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorder, 1);
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
							DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
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
				if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
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
			if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)) && ݡ)
			{
				Rectangle displayRectangle2 = DisplayRectangle;
				displayRectangle2.Inflate(-1, 0);
				if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !(pa.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007ButtonItemStateColorTable mouseOver = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
					ᯊ.ᯓ(graphics, mouseOver, displayRectangle2, RoundRectangleShapeDescriptor.RoundCorner3);
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
			if (Caption != "" && (ᚒ || flag))
			{
				eTextFormat format = ಲ();
				Font font = GetFont();
				Rectangle rectangle2 = new Rectangle(displayRectangle.X + 8, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height);
				if (pa.GlassEnabled && Parent is ᱭ && !(ContainerControl is Ꮮ))
				{
					if (!pa.ໟ)
					{
						_21F8._21FC(graphics, Caption, font, rectangle2, TextDrawing.GetTextFormat(format), color, _21FE: true, _21FF: true, 7);
					}
				}
				else
				{
					TextDrawing.DrawString(graphics, Caption, font, color, rectangle2, format);
				}
				Size size2 = TextDrawing.MeasureString(graphics, Caption, font, 0, format);
				displayRectangle.X += size2.Width + 8;
				displayRectangle.Width -= size2.Width + 8;
			}
			if (ᚐ == null || base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
			{
				displayRectangle.Inflate(-3, -2);
				graphics.FillRectangle(SystemBrushes.Window, displayRectangle);
				_089F.र(graphics, SystemPens.ControlDark, displayRectangle);
				displayRectangle.X = displayRectangle.Right - (SystemInformation.HorizontalScrollBarThumbWidth - 2);
				displayRectangle.Width = SystemInformation.HorizontalScrollBarThumbWidth - 2;
				ControlPaint.DrawComboButton(graphics, displayRectangle, (ButtonState)16384);
			}
			else
			{
				int selectionLength = ((ComboBox)ᚐ).SelectionLength;
				displayRectangle.Inflate(-2, -2);
				if (((Control)ᚐ).Width != displayRectangle.Width)
				{
					((Control)ᚐ).Width = displayRectangle.Width;
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - ((Control)ᚐ).Width) / 2, (displayRectangle.Height - ((Control)ᚐ).Height) / 2);
				Control containerControl = pa.ContainerControl;
				ScrollableControl val4 = (ScrollableControl)(object)((containerControl is ScrollableControl) ? containerControl : null);
				if (val4 != null && val4.AutoScroll)
				{
					location.Offset(val4.AutoScrollPosition.X, val4.AutoScrollPosition.Y);
				}
				if (((Control)ᚐ).Location != location)
				{
					((Control)ᚐ).Location = location;
				}
				if (selectionLength > 0 && selectionLength < 1000 && ((ComboBox)ᚐ).SelectionLength != selectionLength && ((Control)ᚐ).Text.Length > selectionLength && (int)((ComboBox)ᚐ).DropDownStyle == 1)
				{
					((ComboBox)ᚐ).SelectionLength = selectionLength;
				}
			}
			if (base.IsOnCustomizeMenu && Visible)
			{
				Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					r.Inflate(-1, -1);
				}
				BarFunctions.DrawMenuCheckBox(pa, r, EffectiveStyle, ݡ);
			}
		}
		else
		{
			string text = Text;
			text = ((!(text == "")) ? (text + " »") : "...");
			if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
			{
				graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(graphics)), DisplayRectangle);
			}
			else
			{
				graphics.FillRectangle(SystemBrushes.Control, DisplayRectangle);
			}
			if (ݡ && !DesignMode)
			{
				if (EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					ControlPaint.DrawBorder3D(graphics, displayRectangle, (Border3DStyle)4, (Border3DSide)2063);
				}
				else if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					displayRectangle.Inflate(-1, -1);
					graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.HoverBackColor(graphics)), displayRectangle);
					_089F.र(graphics, SystemPens.Highlight, displayRectangle);
				}
			}
			displayRectangle = new Rectangle(m_Rect.Top, -m_Rect.Right, m_Rect.Height, m_Rect.Width);
			graphics.RotateTransform(90f);
			eTextFormat eTextFormat2 = ಲ();
			eTextFormat2 |= eTextFormat.HorizontalCenter;
			TextDrawing.DrawStringLegacy(graphics, text, GetFont(), color, displayRectangle, eTextFormat2);
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

	private void შ()
	{
		if (ᚐ != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-2, -2);
			((Control)ᚐ).Location = displayRectangle.Location;
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		bool isOnMenu = base.IsOnMenu;
		if (Orientation == eOrientation.Horizontal)
		{
			if (Parent != null && Parent is ImageItem)
			{
				m_Rect.Height = ((ImageItem)Parent).SubItemsImageSize.Height + Dpi.Height(4);
			}
			else
			{
				m_Rect.Height = SubItemsImageSize.Height + Dpi.Height(4);
			}
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				if (ᚐ != null && m_Rect.Height < ((Control)ᚐ).Height + 2)
				{
					m_Rect.Height = ((Control)ᚐ).Height + 2;
				}
			}
			else if (ᚐ != null && m_Rect.Height < ((Control)ᚐ).Height + 2)
			{
				m_Rect.Height = ((Control)ᚐ).Height + 2;
			}
			m_Rect.Width = Dpi.Width(ᚑ + 4);
			if (Caption != "" && (ᚒ || isOnMenu))
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null && BaseItem.IsHandleValid(val))
				{
					Graphics val2 = BarFunctions.CreateGraphics(val);
					try
					{
						Size empty = Size.Empty;
						empty = ((m_Orientation != eOrientation.Vertical || isOnMenu) ? TextDrawing.MeasureString(val2, Caption, GetFont(), 0, ಲ()) : TextDrawing.MeasureStringLegacy(val2, Caption, GetFont(), Size.Empty, ಲ()));
						if (empty.Height > SubItemsImageSize.Height && empty.Height > m_Rect.Height)
						{
							m_Rect.Height = empty.Height + 4;
						}
						m_Rect.Width = Dpi.Width(ᚑ + 4) + empty.Width + 8;
					}
					finally
					{
						val2.Dispose();
					}
				}
			}
			Size size = ო();
			if (base.IsOnMenu && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
			{
				m_Rect.Width += size.Width + 7;
			}
			if (base.IsOnCustomizeMenu)
			{
				m_Rect.Width += size.Height + 2;
			}
		}
		else
		{
			m_Rect.Width = Dpi.Width(SubItemsImageSize.Width + 4);
			string caption = Caption;
			caption = ((!(caption == "")) ? (caption + " »") : "...");
			object containerControl2 = ContainerControl;
			Control val3 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val3 != null && BaseItem.IsHandleValid(val3))
			{
				Graphics val4 = BarFunctions.CreateGraphics(val3);
				try
				{
					SizeF sizeF = TextDrawing.MeasureString(val4, caption, GetFont(), 0, ಲ());
					if (sizeF.Height > (float)SubItemsImageSize.Height)
					{
						m_Rect.Width = (int)sizeF.Height + 4;
					}
					m_Rect.Height = (int)sizeF.Width + 8;
				}
				finally
				{
					val4.Dispose();
				}
			}
		}
		base.RecalcSize();
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (ᚐ == null)
		{
			return;
		}
		if (((Control)ᚐ).Parent != null)
		{
			if (((Control)ᚐ).Visible)
			{
				((Control)ᚐ).Visible = false;
			}
			Control parent = ((Control)ᚐ).Parent;
			parent.Controls.Remove((Control)(object)ᚐ);
		}
		Control val = null;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)ᚐ);
				OnDisplayedChanged();
				((Control)ᚐ).Refresh();
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
		if (ᚐ != null && !newValue)
		{
			((Control)ᚐ).Visible = newValue;
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (ᚐ != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)ᚐ).Visible = Displayed;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (ᚐ != null && !((Control)ᚐ).Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)ᚐ).Focus();
		}
	}

	private void _169B(object _٠, EventArgs ؿ)
	{
		HideToolTip();
		Text = ((Control)ᚐ).Text;
		ReleaseFocus();
		if (ݡ)
		{
			ݡ = false;
			Refresh();
		}
	}

	private void _169C(object _٠, EventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!DesignMode && (int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
	}

	private void _169D(object _٠, EventArgs ؿ)
	{
		if (!ݡ)
		{
			ݡ = true;
			Refresh();
		}
	}

	private void _169E(object _٠, EventArgs ؿ)
	{
		HideToolTip();
		if (!((Control)ᚐ).Focused && ݡ)
		{
			ݡ = false;
			Refresh();
		}
	}

	private void _169F(object _٠, EventArgs ؿ)
	{
		HideToolTip();
	}

	private void ᚠ(object _٠, EventArgs ؿ)
	{
		HideToolTip();
		Focus();
		if (!GetEnabled() || DesignMode || _0E7F != eMenuVisibility.VisibleIfRecentlyUsed || _0E80 || !base.IsOnMenu)
		{
			return;
		}
		_0E80 = true;
		for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
		{
			if (parent is IPersonalizedMenuItem personalizedMenuItem)
			{
				personalizedMenuItem.RecentlyUsed = true;
			}
		}
	}

	private void ᚡ(object _٠, EventArgs ؿ)
	{
		if (!_ᚔ)
		{
			RaiseClick();
			if (_05CB != null)
			{
				_05CB(this, ؿ);
			}
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "SelectedIndex");
			}
			ExecuteCommand();
		}
	}

	private void ᚢ(object _٠, KeyEventArgs ؿ)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		HideToolTip();
		if ((int)ؿ.KeyCode == 13 && Parent is PopupItem)
		{
			((PopupItem)Parent).ClosePopup();
		}
	}

	private void ᚣ(object _٠, bool ᚤ)
	{
		Expanded = ᚤ;
		if (!ᚤ)
		{
			ReleaseFocus();
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
			((Control)ᚐ).Visible = false;
		}
		else
		{
			((Control)ᚐ).Visible = Displayed;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDropDownWidth()
	{
		return ((Control)ᚐ).Width != ((ComboBox)ᚐ).DropDownWidth;
	}

	public override void ReleaseFocus()
	{
		if (ᚐ != null && ((Control)ᚐ).Focused)
		{
			ᚐ.ReleaseFocus();
		}
		base.ReleaseFocus();
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

	private eTextFormat ಲ()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 |= eTextFormat.SingleLine;
		eTextFormat2 |= eTextFormat.EndEllipsis;
		return eTextFormat2 | eTextFormat.VerticalCenter;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!ݡ)
		{
			ݡ = true;
			Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (ݡ)
		{
			ݡ = false;
			Refresh();
		}
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool flag = base.IsAnyOnHandle(iHandle);
		if (!flag && ᚐ != null && ((ComboBox)ᚐ).DroppedDown && ᚐ.DropDownHandle != IntPtr.Zero && ᚐ.DropDownHandle == iHandle)
		{
			flag = true;
		}
		return flag;
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		ᚐ.Style = Style;
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if (ᚐ != null)
		{
			((Control)ᚐ).Enabled = Enabled;
		}
	}

	private void ᚥ(object _٠, EventArgs ؿ)
	{
		if (!_ᚘ)
		{
			Text = ((Control)ᚐ).Text;
		}
		OnComboBoxTextChanged(ؿ);
	}

	protected virtual void OnComboBoxTextChanged(EventArgs e)
	{
		if (ᚖ != null)
		{
			ᚖ(this, e);
		}
	}

	protected override void OnTextChanged()
	{
		if (ᚐ != null && ((Control)ᚐ).Text != Text)
		{
			try
			{
				_ᚘ = true;
				((Control)ᚐ).Text = Text;
			}
			finally
			{
				_ᚘ = false;
			}
		}
		base.OnTextChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return ᚐ.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	public override string ToString()
	{
		return m_Text;
	}
}
