using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Click")]
[DesignTimeVisible(false)]
public class ComboBoxItem : ImageItem, IPersonalizedMenuItem
{
	private ComboBoxEx m_ComboBox;

	private int m_ComboWidth = 64;

	private bool m_MouseOver;

	private bool m_AlwaysShowCaption;

	private bool m_FontCombo;

	private eMenuVisibility m_MenuVisibility;

	private bool m_RecentlyUsed;

	internal bool _CopyInProgress;

	private bool m_PreventEnterBeep;

	private Color _LabelForeColor = Color.Empty;

	private bool _SettingText;

	private string _Caption = "";

	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the accessible role of the item.")]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether combo box generates the audible alert when Enter key is pressed.")]
	[DefaultValue(false)]
	public bool PreventEnterBeep
	{
		get
		{
			return m_PreventEnterBeep;
		}
		set
		{
			m_PreventEnterBeep = value;
			if (m_ComboBox != null)
			{
				m_ComboBox.PreventEnterBeep = value;
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates color of combo box label.")]
	public Color LabelForeColor
	{
		get
		{
			return _LabelForeColor;
		}
		set
		{
			_LabelForeColor = value;
			Refresh();
		}
	}

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
			if (base.Bounds != value)
			{
				bool flag = base.Bounds.Top != value.Top;
				bool flag2 = base.Bounds.Left != value.Left;
				base.Bounds = value;
				if ((flag || flag2) && (ContainerControl is ItemPanel || ContainerControl is ExplorerBar))
				{
					UpdateControlLocation();
				}
			}
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates width of the of the drop-down portion of a combo box.")]
	public int DropDownWidth
	{
		get
		{
			return ((ComboBox)m_ComboBox).DropDownWidth;
		}
		set
		{
			((ComboBox)m_ComboBox).DropDownWidth = value;
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates height of the of the drop-down portion of a combo box.")]
	public int DropDownHeight
	{
		get
		{
			return ((ComboBox)m_ComboBox).DropDownHeight;
		}
		set
		{
			((ComboBox)m_ComboBox).DropDownHeight = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the Width of the combo box part of the item.")]
	[DefaultValue(64)]
	public int ComboWidth
	{
		get
		{
			return m_ComboWidth;
		}
		set
		{
			if (m_ComboWidth != value)
			{
				m_ComboWidth = value;
				if (Name != "" && GlobalItem)
				{
					BarFunctions.SetProperty(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["ComboWidth"], m_ComboWidth);
				}
				NeedRecalcSize = true;
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public ComboBoxEx ComboBoxEx => m_ComboBox;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether item caption is always shown.")]
	[DefaultValue(false)]
	public bool AlwaysShowCaption
	{
		get
		{
			return m_AlwaysShowCaption;
		}
		set
		{
			if (m_AlwaysShowCaption != value)
			{
				m_AlwaysShowCaption = value;
			}
			NeedRecalcSize = true;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ComboItemsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Data")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ObjectCollection Items => m_ComboBox.Items;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value specifying the style of the combo box.")]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)m_ComboBox).DropDownStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)m_ComboBox).DropDownStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	public int SelectionStart
	{
		get
		{
			return ((ComboBox)m_ComboBox).SelectionStart;
		}
		set
		{
			((ComboBox)m_ComboBox).SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	public int SelectionLength
	{
		get
		{
			return ((ComboBox)m_ComboBox).SelectionLength;
		}
		set
		{
			((ComboBox)m_ComboBox).SelectionLength = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			return ((ComboBox)m_ComboBox).SelectedText;
		}
		set
		{
			((ComboBox)m_ComboBox).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SelectedItem
	{
		get
		{
			return ((ComboBox)m_ComboBox).SelectedItem;
		}
		set
		{
			((ComboBox)m_ComboBox).SelectedItem = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(-1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			return ((ListControl)m_ComboBox).SelectedIndex;
		}
		set
		{
			((ListControl)m_ComboBox).SelectedIndex = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Automatically loads all the fonts available into the combo box.")]
	[DefaultValue(false)]
	public bool FontCombo
	{
		get
		{
			return m_FontCombo;
		}
		set
		{
			if (m_FontCombo != value)
			{
				m_FontCombo = value;
				if (m_FontCombo)
				{
					m_ComboBox.LoadFonts();
					((ComboBox)m_ComboBox).DropDownStyle = (ComboBoxStyle)2;
					((ComboBox)m_ComboBox).DrawMode = (DrawMode)2;
				}
			}
		}
	}

	[DefaultValue(15)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the height of an item in the combo box.")]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)m_ComboBox).ItemHeight;
		}
		set
		{
			((ComboBox)m_ComboBox).ItemHeight = value;
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
			return m_MenuVisibility;
		}
		set
		{
			if (m_MenuVisibility != value)
			{
				m_MenuVisibility = value;
				if (m_Name != "" && GlobalItem)
				{
					BarFunctions.SetProperty(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["MenuVisibility"], m_MenuVisibility);
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
				if (m_Name != "" && GlobalItem)
				{
					BarFunctions.SetProperty(GetOwner(), GetType(), m_Name, TypeDescriptor.GetProperties(this)["RecentlyUsed"], m_RecentlyUsed);
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Obsolete("Please use Text property instead to access ComboBox text.")]
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
	[Category("Appearance")]
	[Description("The text contained in the underlining Control portion of the item.")]
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
	[Category("Appearance")]
	[Description("Indicates the item Caption displayed next to the combo box.")]
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
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Data")]
	[Description("Indicates string that specifies the property of the data source whose contents you want to display.")]
	[DefaultValue("")]
	public virtual string DisplayMember
	{
		get
		{
			if (m_ComboBox != null)
			{
				return ((ListControl)m_ComboBox).DisplayMember;
			}
			return "";
		}
		set
		{
			if (m_ComboBox != null)
			{
				((ListControl)m_ComboBox).DisplayMember = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether combo box is drawn using themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return m_ComboBox.ThemeAware;
		}
		set
		{
			m_ComboBox.ThemeAware = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return m_ComboBox.WatermarkFont;
		}
		set
		{
			m_ComboBox.WatermarkFont = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return m_ComboBox.WatermarkColor;
		}
		set
		{
			m_ComboBox.WatermarkColor = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return m_ComboBox.WatermarkEnabled;
		}
		set
		{
			m_ComboBox.WatermarkEnabled = value;
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
			return m_ComboBox.WatermarkText;
		}
		set
		{
			m_ComboBox.WatermarkText = value;
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return m_ComboBox.WatermarkBehavior;
		}
		set
		{
			m_ComboBox.WatermarkBehavior = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[RefreshProperties(RefreshProperties.All)]
	[ParenthesizePropertyName(true)]
	public new ControlBindingsCollection DataBindings => ((Control)m_ComboBox).DataBindings;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates the appearance of the control.")]
	public bool IsStandalone
	{
		get
		{
			return m_ComboBox.IsStandalone;
		}
		set
		{
			m_ComboBox.IsStandalone = value;
		}
	}

	public event EventHandler ComboBoxTextChanged;

	public event EventHandler SelectedIndexChanged;

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
		CreateComboBox();
		m_ComboWidth = 64;
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
		IsAccessible = true;
		AccessibleRole = (AccessibleRole)46;
	}

	private void CreateComboBox()
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		if (m_ComboBox != null)
		{
			((Component)(object)m_ComboBox).Dispose();
			m_ComboBox = null;
		}
		m_ComboBox = new ComboBoxEx();
		m_ComboBox.IsStandalone = false;
		((Control)m_ComboBox).TabStop = false;
		((Control)m_ComboBox).TabIndex = 9999;
		m_ComboBox.Style = Style;
		((ComboBox)m_ComboBox).DrawMode = (DrawMode)1;
		((ComboBox)m_ComboBox).IntegralHeight = false;
		m_ComboBox.ThemeAware = false;
		((Control)m_ComboBox).Visible = false;
		((Control)m_ComboBox).Text = Text;
		((ComboBox)m_ComboBox).SelectionStart = 0;
		((ComboBox)m_ComboBox).DropDownStyle = (ComboBoxStyle)2;
		((Control)m_ComboBox).LostFocus += ComboLostFocus;
		((Control)m_ComboBox).GotFocus += ComboGotFocus;
		((Control)m_ComboBox).MouseHover += ComboMouseHover;
		((Control)m_ComboBox).MouseEnter += ComboMouseEnter;
		((Control)m_ComboBox).MouseLeave += ComboMouseLeave;
		((Control)m_ComboBox).VisibleChanged += ComboBoxVisibleChanged;
		m_ComboBox.DropDownChange += ComboDropDownChange;
		((Control)m_ComboBox).TextChanged += InternalComboTextChanged;
		((Control)m_ComboBox).KeyDown += new KeyEventHandler(ComboKeyDown);
		((ComboBox)m_ComboBox).SelectedIndexChanged += ComboSelChanged;
		m_ComboBox.PreventEnterBeep = m_PreventEnterBeep;
		m_ComboBox.ParentItem = this;
		if (m_FontCombo)
		{
			m_ComboBox.LoadFonts();
		}
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)m_ComboBox);
				((Control)m_ComboBox).Refresh();
			}
		}
		if (Displayed)
		{
			((Control)m_ComboBox).Visible = true;
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
		return comboBoxItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		((ComboBoxItem)copy)._CopyInProgress = true;
		try
		{
			ComboBoxItem comboBoxItem = copy as ComboBoxItem;
			base.CopyToItem(comboBoxItem);
			comboBoxItem.ComboWidth = m_ComboWidth;
			comboBoxItem.FontCombo = m_FontCombo;
			comboBoxItem.PreventEnterBeep = m_PreventEnterBeep;
			if (m_ComboBox == null)
			{
				return;
			}
			comboBoxItem.DisplayMember = ((ListControl)m_ComboBox).DisplayMember;
			ComboBoxEx comboBoxEx = comboBoxItem.ComboBoxEx;
			if (!m_FontCombo)
			{
				foreach (object item in m_ComboBox.Items)
				{
					comboBoxEx.Items.Add(item);
				}
			}
			((ListControl)comboBoxEx).SelectedIndex = ((ListControl)m_ComboBox).SelectedIndex;
		}
		finally
		{
			((ComboBoxItem)copy)._CopyInProgress = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return ((Control)m_ComboBox).AccessibilityObject;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected I4, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected I4, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected I4, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected I4, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected I4, but got Unknown
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("ComboWidth", XmlConvert.ToString(m_ComboWidth));
		itemXmlElement.SetAttribute("FontCombo", XmlConvert.ToString(m_FontCombo));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)m_MenuVisibility));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(m_RecentlyUsed));
		itemXmlElement.SetAttribute("DropDownStyle", XmlConvert.ToString((int)((ComboBox)m_ComboBox).DropDownStyle));
		itemXmlElement.SetAttribute("ThemeAware", XmlConvert.ToString(m_ComboBox.ThemeAware));
		itemXmlElement.SetAttribute("AlwaysShowCaption", XmlConvert.ToString(m_AlwaysShowCaption));
		itemXmlElement.SetAttribute("ItemHeight", XmlConvert.ToString(((ComboBox)m_ComboBox).ItemHeight));
		if (((ListControl)m_ComboBox).DisplayMember != "")
		{
			itemXmlElement.SetAttribute("DisplayMembers", ((ListControl)m_ComboBox).DisplayMember);
		}
		if (m_PreventEnterBeep)
		{
			itemXmlElement.SetAttribute("nobeep", XmlConvert.ToString(m_PreventEnterBeep));
		}
		if (m_FontCombo || m_ComboBox.Items.Count <= 0)
		{
			return;
		}
		XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("cbitems");
		itemXmlElement.AppendChild(xmlElement);
		foreach (object item in m_ComboBox.Items)
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
				if (((ComboBox)m_ComboBox).SelectedItem == item)
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
				if (((ComboBox)m_ComboBox).SelectedItem == item)
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
		m_ComboWidth = XmlConvert.ToInt32(itemXmlElement.GetAttribute("ComboWidth"));
		m_FontCombo = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("FontCombo"));
		m_MenuVisibility = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		m_RecentlyUsed = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
		if (itemXmlElement.HasAttribute("DropDownStyle"))
		{
			((ComboBox)m_ComboBox).DropDownStyle = (ComboBoxStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("DropDownStyle"));
		}
		if (itemXmlElement.HasAttribute("CText"))
		{
			Text = itemXmlElement.GetAttribute("CText");
		}
		if (itemXmlElement.HasAttribute("ThemeAware"))
		{
			m_ComboBox.ThemeAware = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("ThemeAware"));
		}
		else
		{
			m_ComboBox.ThemeAware = true;
		}
		if (itemXmlElement.HasAttribute("AlwaysShowCaption"))
		{
			m_AlwaysShowCaption = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AlwaysShowCaption"));
		}
		if (itemXmlElement.HasAttribute("nobeep"))
		{
			PreventEnterBeep = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("nobeep"));
		}
		XmlNodeList elementsByTagName = itemXmlElement.GetElementsByTagName("cbitems");
		if (!m_FontCombo && elementsByTagName.Count > 0)
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
						((ComboBox)m_ComboBox).ItemHeight = XmlConvert.ToInt32(childNode.GetAttribute("ItemHeight"));
					}
					comboItem.Text = childNode.GetAttribute("text");
					comboItem.TextAlignment = (StringAlignment)XmlConvert.ToInt32(childNode.GetAttribute("ta"));
					comboItem.TextLineAlignment = (StringAlignment)XmlConvert.ToInt32(childNode.GetAttribute("tla"));
					m_ComboBox.Items.Add((object)comboItem);
					if (childNode.HasAttribute("selected") && childNode.GetAttribute("selected") == "1")
					{
						((ComboBox)m_ComboBox).SelectedItem = comboItem;
					}
				}
				else if (childNode.Name == "co")
				{
					m_ComboBox.Items.Add((object)childNode.InnerText);
					if (childNode.HasAttribute("selected") && childNode.GetAttribute("selected") == "1")
					{
						((ComboBox)m_ComboBox).SelectedItem = m_ComboBox.Items[m_ComboBox.Items.Count - 1];
					}
				}
			}
		}
		if (m_FontCombo)
		{
			m_ComboBox.LoadFonts();
		}
		if (m_ComboBox != null)
		{
			((Control)m_ComboBox).Enabled = Enabled;
		}
		if (itemXmlElement.HasAttribute("DisplayMembers") && m_ComboBox != null)
		{
			((ListControl)m_ComboBox).DisplayMember = itemXmlElement.GetAttribute("DisplayMembers");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLabelForeColor()
	{
		return !_LabelForeColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLabelForeColor()
	{
		LabelForeColor = Color.Empty;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Expected O, but got Unknown
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		Size maxImageSize = GetMaxImageSize();
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		bool enabled = GetEnabled(pa.ContainerControl);
		Color color = (enabled ? SystemColors.ControlText : SystemColors.ControlDark);
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && pa.Renderer is Office2007Renderer && ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors.Count > 0)
		{
			color = (enabled ? ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText);
		}
		if (!_LabelForeColor.IsEmpty)
		{
			color = _LabelForeColor;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
			{
				maxImageSize.Width += 7;
				displayRectangle.Width -= maxImageSize.Width;
				displayRectangle.X += maxImageSize.Width;
				if (base.IsOnCustomizeMenu)
				{
					maxImageSize.Width += maxImageSize.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
				if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
				{
					if (!pa.Colors.MenuUnusedSide2.IsEmpty)
					{
						LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
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
					displayRectangle.X += maxImageSize.Height + 8;
					displayRectangle.Width -= maxImageSize.Height + 8;
				}
				else
				{
					displayRectangle.X += maxImageSize.Height + 4;
					displayRectangle.Width -= maxImageSize.Height + 4;
				}
			}
			if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)) && m_MouseOver)
			{
				Rectangle displayRectangle2 = DisplayRectangle;
				displayRectangle2.Inflate(-1, 0);
				if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !(pa.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007ButtonItemStateColorTable mouseOver = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
					Office2007ButtonItemPainter.PaintBackground(graphics, mouseOver, displayRectangle2, RoundRectangleShapeDescriptor.RoundCorner3);
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
			if (Caption != "" && (m_AlwaysShowCaption || flag))
			{
				eTextFormat stringFormat = GetStringFormat();
				Font font = GetFont();
				Rectangle bounds = new Rectangle(displayRectangle.X + 8, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height);
				if (EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					TextDrawing.DrawString(graphics, Caption, font, color, bounds, stringFormat);
				}
				else
				{
					TextDrawing.DrawString(graphics, Caption, font, color, bounds, stringFormat);
				}
				Size size = TextDrawing.MeasureString(graphics, Caption, font, 0, stringFormat);
				displayRectangle.X += size.Width + 8;
				displayRectangle.Width -= size.Width + 8;
			}
			if (m_ComboBox == null || base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
			{
				displayRectangle.Inflate(-3, -2);
				graphics.FillRectangle(SystemBrushes.Window, displayRectangle);
				NativeFunctions.DrawRectangle(graphics, SystemPens.ControlDark, displayRectangle);
				displayRectangle.X = displayRectangle.Right - (SystemInformation.HorizontalScrollBarThumbWidth - 2);
				displayRectangle.Width = SystemInformation.HorizontalScrollBarThumbWidth - 2;
				ControlPaint.DrawComboButton(graphics, displayRectangle, (ButtonState)16384);
			}
			else
			{
				int selectionLength = ((ComboBox)m_ComboBox).SelectionLength;
				displayRectangle.Inflate(-2, -2);
				if (((Control)m_ComboBox).Width != displayRectangle.Width)
				{
					((Control)m_ComboBox).Width = displayRectangle.Width;
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - ((Control)m_ComboBox).Width) / 2, (displayRectangle.Height - ((Control)m_ComboBox).Height) / 2);
				Control containerControl = pa.ContainerControl;
				ScrollableControl val4 = (ScrollableControl)(object)((containerControl is ScrollableControl) ? containerControl : null);
				if (val4 != null && val4.AutoScroll)
				{
					location.Offset(val4.AutoScrollPosition.X, val4.AutoScrollPosition.Y);
				}
				if (((Control)m_ComboBox).Location != location)
				{
					((Control)m_ComboBox).Location = location;
				}
				if (selectionLength > 0 && selectionLength < 1000 && ((ComboBox)m_ComboBox).SelectionLength != selectionLength && ((Control)m_ComboBox).Text.Length > selectionLength && (int)((ComboBox)m_ComboBox).DropDownStyle == 1)
				{
					((ComboBox)m_ComboBox).SelectionLength = selectionLength;
				}
			}
			if (base.IsOnCustomizeMenu && Visible)
			{
				Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					r.Inflate(-1, -1);
				}
				BarFunctions.DrawMenuCheckBox(pa, r, EffectiveStyle, m_MouseOver);
			}
		}
		else
		{
			string text = Text;
			text = ((!(text == "")) ? (text + " \ufffd") : "...");
			if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
			{
				graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(graphics)), DisplayRectangle);
			}
			else
			{
				graphics.FillRectangle(SystemBrushes.Control, DisplayRectangle);
			}
			if (m_MouseOver && !DesignMode)
			{
				if (EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					ControlPaint.DrawBorder3D(graphics, displayRectangle, (Border3DStyle)4, (Border3DSide)2063);
				}
				else if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					displayRectangle.Inflate(-1, -1);
					graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.HoverBackColor(graphics)), displayRectangle);
					NativeFunctions.DrawRectangle(graphics, SystemPens.Highlight, displayRectangle);
				}
			}
			displayRectangle = new Rectangle(m_Rect.Top, -m_Rect.Right, m_Rect.Height, m_Rect.Width);
			graphics.RotateTransform(90f);
			eTextFormat stringFormat2 = GetStringFormat();
			stringFormat2 |= eTextFormat.HorizontalCenter;
			TextDrawing.DrawStringLegacy(graphics, text, GetFont(), color, displayRectangle, stringFormat2);
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

	private void UpdateControlLocation()
	{
		if (m_ComboBox != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-2, -2);
			((Control)m_ComboBox).Location = displayRectangle.Location;
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
				m_Rect.Height = ((ImageItem)Parent).SubItemsImageSize.Height + 4;
			}
			else
			{
				m_Rect.Height = SubItemsImageSize.Height + 4;
			}
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				if (m_ComboBox != null && m_Rect.Height < ((Control)m_ComboBox).Height + 2)
				{
					m_Rect.Height = ((Control)m_ComboBox).Height + 2;
				}
			}
			else if (m_ComboBox != null && m_Rect.Height < ((Control)m_ComboBox).Height + 2)
			{
				m_Rect.Height = ((Control)m_ComboBox).Height + 2;
			}
			m_Rect.Width = m_ComboWidth + 4;
			if (Caption != "" && (m_AlwaysShowCaption || isOnMenu))
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null && BaseItem.IsHandleValid(val))
				{
					Graphics val2 = BarFunctions.CreateGraphics(val);
					try
					{
						Size empty = Size.Empty;
						empty = ((m_Orientation != eOrientation.Vertical || isOnMenu) ? TextDrawing.MeasureString(val2, Caption, GetFont(), 0, GetStringFormat()) : TextDrawing.MeasureStringLegacy(val2, Caption, GetFont(), Size.Empty, GetStringFormat()));
						if (empty.Height > SubItemsImageSize.Height && empty.Height > m_Rect.Height)
						{
							m_Rect.Height = empty.Height + 4;
						}
						m_Rect.Width = m_ComboWidth + 4 + empty.Width + 8;
					}
					finally
					{
						val2.Dispose();
					}
				}
			}
			Size maxImageSize = GetMaxImageSize();
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
			string caption = Caption;
			caption = ((!(caption == "")) ? (caption + " \ufffd") : "...");
			object containerControl2 = ContainerControl;
			Control val3 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val3 != null && BaseItem.IsHandleValid(val3))
			{
				Graphics val4 = BarFunctions.CreateGraphics(val3);
				try
				{
					SizeF sizeF = TextDrawing.MeasureString(val4, caption, GetFont(), 0, GetStringFormat());
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
		if (m_ComboBox == null)
		{
			return;
		}
		if (((Control)m_ComboBox).Parent != null)
		{
			if (((Control)m_ComboBox).Visible)
			{
				((Control)m_ComboBox).Visible = false;
			}
			Control parent = ((Control)m_ComboBox).Parent;
			parent.Controls.Remove((Control)(object)m_ComboBox);
		}
		Control val = null;
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Controls.Add((Control)(object)m_ComboBox);
				OnDisplayedChanged();
				((Control)m_ComboBox).Refresh();
			}
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnBeforeItemRemoved(item);
		ContainerControl = null;
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (m_ComboBox != null && !newValue)
		{
			((Control)m_ComboBox).Visible = newValue;
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (m_ComboBox != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)m_ComboBox).Visible = Displayed;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (m_ComboBox != null && !((Control)m_ComboBox).Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			((Control)m_ComboBox).Focus();
		}
	}

	private void ComboLostFocus(object sender, EventArgs e)
	{
		HideToolTip();
		Text = ((Control)m_ComboBox).Text;
		ReleaseFocus();
		if (m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
	}

	private void ComboMouseHover(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!DesignMode && (int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
	}

	private void ComboMouseEnter(object sender, EventArgs e)
	{
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			Refresh();
		}
	}

	private void ComboMouseLeave(object sender, EventArgs e)
	{
		HideToolTip();
		if (!((Control)m_ComboBox).Focused && m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
	}

	private void ComboBoxVisibleChanged(object sender, EventArgs e)
	{
		HideToolTip();
	}

	private void ComboGotFocus(object sender, EventArgs e)
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

	private void ComboSelChanged(object sender, EventArgs e)
	{
		if (!_CopyInProgress)
		{
			RaiseClick();
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, e);
			}
			if (ShouldSyncProperties)
			{
				BarFunctions.SyncProperty(this, "SelectedIndex");
			}
			ExecuteCommand();
		}
	}

	private void ComboKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		HideToolTip();
		if ((int)e.KeyCode == 13 && Parent is PopupItem)
		{
			((PopupItem)Parent).ClosePopup();
		}
	}

	private void ComboDropDownChange(object sender, bool bExpanded)
	{
		Expanded = bExpanded;
		if (!bExpanded)
		{
			ReleaseFocus();
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
			((Control)m_ComboBox).Visible = false;
		}
		else
		{
			((Control)m_ComboBox).Visible = Displayed;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDropDownWidth()
	{
		return ((Control)m_ComboBox).Width != ((ComboBox)m_ComboBox).DropDownWidth;
	}

	public override void ReleaseFocus()
	{
		if (m_ComboBox != null && ((Control)m_ComboBox).Focused)
		{
			m_ComboBox.ReleaseFocus();
		}
		base.ReleaseFocus();
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

	private eTextFormat GetStringFormat()
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool flag = base.IsAnyOnHandle(iHandle);
		if (!flag && m_ComboBox != null && ((ComboBox)m_ComboBox).DroppedDown && m_ComboBox.DropDownHandle != IntPtr.Zero && m_ComboBox.DropDownHandle == iHandle)
		{
			flag = true;
		}
		return flag;
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		m_ComboBox.Style = Style;
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if (m_ComboBox != null)
		{
			((Control)m_ComboBox).Enabled = Enabled;
		}
	}

	private void InternalComboTextChanged(object sender, EventArgs e)
	{
		if (!_SettingText)
		{
			Text = ((Control)m_ComboBox).Text;
		}
		OnComboBoxTextChanged(e);
	}

	protected virtual void OnComboBoxTextChanged(EventArgs e)
	{
		if (this.ComboBoxTextChanged != null)
		{
			this.ComboBoxTextChanged(this, e);
		}
	}

	protected override void OnTextChanged()
	{
		if (m_ComboBox != null && ((Control)m_ComboBox).Text != Text)
		{
			try
			{
				_SettingText = true;
				((Control)m_ComboBox).Text = Text;
			}
			finally
			{
				_SettingText = false;
			}
		}
		base.OnTextChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return m_ComboBox.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}
}
