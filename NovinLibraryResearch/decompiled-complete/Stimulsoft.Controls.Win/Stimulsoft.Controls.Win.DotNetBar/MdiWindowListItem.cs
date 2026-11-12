using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MdiWindowListItem : ImageItem
{
	private bool m_Initialized;

	private ArrayList m_MdiItems = new ArrayList();

	private int m_MaxCaptionLength = 22;

	private bool m_ShowWindowIcons = true;

	private bool m_MdiNoFormActivateFlicker = true;

	private bool m_CreateMdiChildAccessKeys;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Visible
	{
		get
		{
			if (DesignMode)
			{
				return Site != null;
			}
			return false;
		}
		set
		{
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether the MDI Child Window Icons are displayed on items.")]
	public bool ShowWindowIcons
	{
		get
		{
			return m_ShowWindowIcons;
		}
		set
		{
			m_ShowWindowIcons = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(22)]
	[Description("Indicates maximum form caption length that will be displayed on each item. If caption length exceeds given value ... characters are added.")]
	public int MaxCaptionLength
	{
		get
		{
			return m_MaxCaptionLength;
		}
		set
		{
			if (m_MaxCaptionLength != value)
			{
				m_MaxCaptionLength = value;
				RefreshMdiList();
			}
		}
	}

	[Browsable(false)]
	public bool IsInitialized => m_Initialized;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate.")]
	[Category("Mdi Support")]
	[DefaultValue(true)]
	public bool MdiNoFormActivateFlicker
	{
		get
		{
			return m_MdiNoFormActivateFlicker;
		}
		set
		{
			if (m_MdiNoFormActivateFlicker != value)
			{
				m_MdiNoFormActivateFlicker = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether numbered access keys are created for MDI Child window menu items for first 9 items.")]
	[Category("Mdi Support")]
	[DefaultValue(false)]
	public bool CreateMdiChildAccessKeys
	{
		get
		{
			return m_CreateMdiChildAccessKeys;
		}
		set
		{
			if (m_CreateMdiChildAccessKeys != value)
			{
				m_CreateMdiChildAccessKeys = value;
				RefreshMdiList();
			}
		}
	}

	public MdiWindowListItem()
		: this("", "")
	{
	}

	public MdiWindowListItem(string sName)
		: this(sName, "")
	{
	}

	public MdiWindowListItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		m_SystemItem = true;
		m_Visible = false;
		IsAccessible = false;
	}

	protected override void OnSiteChanged()
	{
		if (Site != null)
		{
			m_SystemItem = false;
			m_Visible = true;
		}
		else
		{
			m_SystemItem = true;
			m_Visible = false;
		}
		base.OnSiteChanged();
	}

	public override BaseItem Copy()
	{
		MdiWindowListItem mdiWindowListItem = new MdiWindowListItem(Name);
		CopyToItem(mdiWindowListItem);
		return mdiWindowListItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_MdiItems != null && m_MdiItems.Count > 0)
		{
			foreach (BaseItem mdiItem in m_MdiItems)
			{
				mdiItem.Tag = null;
				if (Parent != null)
				{
					Parent.SubItems.Remove(mdiItem);
				}
			}
			m_MdiItems.Clear();
		}
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (DesignMode)
		{
			Graphics graphics = pa.Graphics;
			TextDrawing.DrawString(graphics, (Name.Length > 0) ? Name : "MdiWindowListItem", pa.Font, pa.Colors.ItemText, m_Rect, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
			if (Focused)
			{
				Rectangle rect = m_Rect;
				rect.Inflate(-1, -1);
				DesignTime.DrawDesignTimeSelection(graphics, rect, pa.Colors.ItemDesignTimeBorder);
			}
			DrawInsertMarker(pa.Graphics);
		}
	}

	public override void RecalcSize()
	{
		if (DesignMode)
		{
			WidthInternal = 64;
			HeightInternal = 24;
		}
		base.RecalcSize();
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (ContainerControl != null && !m_Initialized)
		{
			Initialize();
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("showicons"))
		{
			m_ShowWindowIcons = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("showicons"));
		}
		if (itemXmlElement.HasAttribute("mdiaccesskey"))
		{
			m_CreateMdiChildAccessKeys = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("mdiaccesskey"));
		}
		if (!m_Initialized)
		{
			Initialize();
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (!m_ShowWindowIcons)
		{
			itemXmlElement.SetAttribute("showicons", XmlConvert.ToString(m_ShowWindowIcons));
		}
		if (m_CreateMdiChildAccessKeys)
		{
			itemXmlElement.SetAttribute("mdiaccesskey", XmlConvert.ToString(m_CreateMdiChildAccessKeys));
		}
	}

	public void Initialize(Form mdiForm)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (GetOwner() is IOwner owner)
		{
			MdiClient mdiClient = owner.GetMdiClient(mdiForm);
			if (mdiClient == null)
			{
				mdiForm.Load += ParentFormLoaded;
				return;
			}
			((Control)mdiClient).ControlAdded += new ControlEventHandler(MdiFormAdded);
			((Control)mdiClient).ControlRemoved += new ControlEventHandler(MdiFormRemoved);
			mdiForm.MdiChildActivate += MdiFormActivated;
			m_Initialized = true;
			RefreshMdiList();
		}
	}

	public void Initialize()
	{
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner).IsOverflowRibbon)) || m_Initialized)
		{
			return;
		}
		Form val = owner.ParentForm;
		if (val == null)
		{
			if (Form.ActiveForm != null && Form.ActiveForm.IsMdiChild)
			{
				val = Form.ActiveForm;
			}
			if (val == null)
			{
				return;
			}
		}
		Initialize(val);
	}

	private void ParentFormLoaded(object sender, EventArgs e)
	{
		if (GetOwner() is IOwner { ParentForm: not null } owner)
		{
			owner.ParentForm.Load -= ParentFormLoaded;
			if (!m_Initialized)
			{
				Initialize();
			}
		}
	}

	private void RemoveEvents()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (GetOwner() is IOwner { ParentForm: not null } owner && m_Initialized)
		{
			MdiClient mdiClient = owner.GetMdiClient(owner.ParentForm);
			if (mdiClient != null)
			{
				((Control)mdiClient).ControlAdded -= new ControlEventHandler(MdiFormAdded);
				((Control)mdiClient).ControlRemoved -= new ControlEventHandler(MdiFormRemoved);
				owner.ParentForm.MdiChildActivate -= MdiFormActivated;
				m_Initialized = false;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormAdded(object sender, ControlEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner).IsOverflowRibbon)) || !m_Initialized)
		{
			return;
		}
		Form val = owner.ParentForm;
		if (val == null)
		{
			Control parent = ((Control)sender).Parent;
			val = (Form)(object)((parent is Form) ? parent : null);
		}
		Control control = e.Control;
		Form val2 = (Form)(object)((control is Form) ? control : null);
		if (val2 == null)
		{
			RefreshMdiList();
			return;
		}
		int position = Parent.SubItems.IndexOf(this) + 1;
		if (m_MdiItems.Count > 0)
		{
			position = Parent.SubItems.IndexOf((BaseItem)m_MdiItems[m_MdiItems.Count - 1]) + 1;
		}
		string formText = GetFormText(((Control)val2).Text, m_MdiItems.Count + 1);
		ButtonItem buttonItem = new ButtonItem("mdi-" + ((object)val2).GetHashCode(), formText);
		buttonItem.ShouldSerialize = false;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		if (m_MdiItems.Count == 0 && BeginGroup)
		{
			buttonItem.BeginGroup = true;
		}
		buttonItem.Tag = val2;
		if (val.ActiveMdiChild == val2)
		{
			buttonItem.Checked = true;
		}
		buttonItem.Click += MdiWindowItemClick;
		buttonItem.SetSystemItem(b: true);
		if (m_ShowWindowIcons && val2.Icon != null && val2.ControlBox)
		{
			object obj = val2.Icon.Clone();
			buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		m_MdiItems.Add(buttonItem);
		Parent.SubItems.Add(buttonItem, position);
		buttonItem.Visible = ((Control)val2).Visible;
		if (m_CreateMdiChildAccessKeys)
		{
			RefreshMdiItemsText();
		}
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl).RecalcLayout();
		}
		else if (ContainerControl is ItemControl)
		{
			((ItemControl)ContainerControl).RecalcLayout();
		}
		((Control)val2).TextChanged += FormTextChanged;
		((Control)val2).VisibleChanged += FormVisibleChanged;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormRemoved(object sender, ControlEventArgs e)
	{
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			RefreshMdiList();
			return;
		}
		((Control)val).TextChanged -= FormTextChanged;
		((Control)val).VisibleChanged -= FormVisibleChanged;
		foreach (BaseItem mdiItem in m_MdiItems)
		{
			if (mdiItem.Tag == val)
			{
				Parent.SubItems.Remove(mdiItem);
				mdiItem.Click -= MdiWindowItemClick;
				mdiItem.Dispose();
				mdiItem.Tag = null;
				m_MdiItems.Remove(mdiItem);
				if (BeginGroup && m_MdiItems.Count > 0)
				{
					((BaseItem)m_MdiItems[0]).BeginGroup = true;
				}
				break;
			}
		}
		if (m_MdiItems.Count == 0)
		{
			if (ContainerControl is Bar)
			{
				((Bar)ContainerControl).RecalcLayout();
			}
		}
		else if (m_CreateMdiChildAccessKeys)
		{
			RefreshMdiItemsText();
		}
	}

	private void MdiFormActivated(object sender, EventArgs e)
	{
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner).IsOverflowRibbon)) || !m_Initialized)
		{
			return;
		}
		Form val = (Form)((sender is Form) ? sender : null);
		Form activeMdiChild = val.ActiveMdiChild;
		foreach (ButtonItem mdiItem in m_MdiItems)
		{
			if (mdiItem.Tag == activeMdiChild)
			{
				mdiItem.Checked = true;
			}
			else
			{
				mdiItem.Checked = false;
			}
		}
	}

	private void MdiWindowItemClick(object sender, EventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		ButtonItem buttonItem = sender as ButtonItem;
		if (buttonItem.Checked)
		{
			return;
		}
		object tag = buttonItem.Tag;
		Form val = (Form)((tag is Form) ? tag : null);
		IOwner owner = GetOwner() as IOwner;
		Form mdiParent = val.MdiParent;
		if (owner != null && mdiParent != null && m_Initialized)
		{
			Form activeMdiChild = owner.ActiveMdiChild;
			MdiClient mdiClient = owner.GetMdiClient(mdiParent);
			bool flag = false;
			if (mdiClient != null && m_MdiNoFormActivateFlicker && ((int)val.WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
			{
				NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 0, 0);
				flag = true;
			}
			val.Activate();
			if (flag)
			{
				NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 1, 0);
				((Control)mdiClient).Refresh();
			}
			if (activeMdiChild != null && (int)activeMdiChild.WindowState == 0 && !(ContainerControl is Bar) && BarFunctions.ThemedOS)
			{
				string text = ((Control)activeMdiChild).Text;
				((Control)activeMdiChild).Text = text + " ";
				((Control)activeMdiChild).Text = text;
			}
		}
	}

	private void RefreshMdiList()
	{
		if (Parent == null || m_MdiItems == null)
		{
			return;
		}
		Parent.SuspendLayout = true;
		if (m_MdiItems.Count > 0)
		{
			foreach (BaseItem mdiItem in m_MdiItems)
			{
				mdiItem.Tag = null;
				Parent.SubItems.Remove(mdiItem);
			}
			m_MdiItems.Clear();
		}
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner).IsOverflowRibbon)) || !m_Initialized)
		{
			Parent.SuspendLayout = false;
			return;
		}
		Form val = owner.ParentForm;
		if (owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon && val == null)
		{
			if (Form.ActiveForm != null && Form.ActiveForm.IsMdiChild)
			{
				val = Form.ActiveForm;
			}
			if (val == null)
			{
				return;
			}
		}
		MdiClient mdiClient = owner.GetMdiClient(val);
		if (mdiClient == null)
		{
			Parent.SuspendLayout = false;
			return;
		}
		int num = Parent.SubItems.IndexOf(this) + 1;
		bool flag = true;
		int num2 = 1;
		Form[] mdiChildren = mdiClient.MdiChildren;
		foreach (Form val2 in mdiChildren)
		{
			string formText = GetFormText(((Control)val2).Text, num2);
			ButtonItem buttonItem = new ButtonItem("mdi-" + ((Control)val2).Handle, formText);
			buttonItem.ShouldSerialize = false;
			buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
			if (flag && BeginGroup)
			{
				buttonItem.BeginGroup = true;
			}
			flag = false;
			buttonItem.Tag = val2;
			if (val.ActiveMdiChild == val2)
			{
				buttonItem.Checked = true;
			}
			buttonItem.Click += MdiWindowItemClick;
			buttonItem.SetSystemItem(b: true);
			if (m_ShowWindowIcons && val2.Icon != null && val2.ControlBox)
			{
				object obj = val2.Icon.Clone();
				buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
			}
			m_MdiItems.Add(buttonItem);
			Parent.SubItems.Add(buttonItem, num);
			buttonItem.Visible = ((Control)val2).Visible;
			num++;
			((Control)val2).TextChanged += FormTextChanged;
			num2++;
		}
		Parent.SuspendLayout = false;
		if (m_MdiItems.Count > 0 && ContainerControl is Bar)
		{
			((Bar)ContainerControl).RecalcLayout();
		}
	}

	private string GetFormText(string text, int index)
	{
		if (text.Length > m_MaxCaptionLength)
		{
			text = text.Substring(0, m_MaxCaptionLength) + "...";
		}
		if (m_CreateMdiChildAccessKeys && index < 10)
		{
			text = "&" + index + ". " + text;
		}
		return text;
	}

	public void RefreshButtonIcons()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (!m_ShowWindowIcons || m_MdiItems == null)
		{
			return;
		}
		foreach (ButtonItem mdiItem in m_MdiItems)
		{
			try
			{
				if (mdiItem.Tag is Form && ((Form)mdiItem.Tag).ControlBox)
				{
					object obj = ((Form)mdiItem.Tag).Icon.Clone();
					mdiItem.Icon = (Icon)((obj is Icon) ? obj : null);
				}
			}
			catch
			{
			}
		}
	}

	public override void SetParent(BaseItem o)
	{
		if (Parent != null)
		{
			Parent.ExpandChange -= ParentExpandedChanged;
		}
		base.SetParent(o);
		if (Parent != null)
		{
			Parent.ExpandChange += ParentExpandedChanged;
		}
	}

	private void ParentExpandedChanged(object sender, EventArgs e)
	{
		if (Parent != null && Parent.Expanded)
		{
			RefreshButtonIcons();
		}
	}

	private void FormTextChanged(object sender, EventArgs e)
	{
		Form val = (Form)((sender is Form) ? sender : null);
		if (val == null)
		{
			return;
		}
		for (int i = 0; i < m_MdiItems.Count; i++)
		{
			BaseItem baseItem = m_MdiItems[i] as BaseItem;
			if (baseItem.Tag == val)
			{
				baseItem.Text = GetFormText(((Control)val).Text, i + 1);
				break;
			}
		}
	}

	private void RefreshMdiItemsText()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < m_MdiItems.Count; i++)
		{
			BaseItem baseItem = m_MdiItems[i] as BaseItem;
			baseItem.Text = GetFormText(((Control)(Form)baseItem.Tag).Text, i + 1);
		}
	}

	private void FormVisibleChanged(object sender, EventArgs e)
	{
		Form val = (Form)((sender is Form) ? sender : null);
		if (val == null)
		{
			return;
		}
		if (m_CreateMdiChildAccessKeys)
		{
			RefreshMdiItemsText();
		}
		foreach (BaseItem mdiItem in m_MdiItems)
		{
			if (mdiItem.Tag != val)
			{
				continue;
			}
			mdiItem.Visible = ((Control)val).Visible;
			if (BeginGroup)
			{
				bool flag = true;
				foreach (BaseItem mdiItem2 in m_MdiItems)
				{
					if (flag && mdiItem2.Visible)
					{
						mdiItem2.BeginGroup = true;
						flag = false;
					}
					else
					{
						mdiItem2.BeginGroup = false;
					}
				}
			}
			if (ContainerControl is Bar && mdiItem.Visible)
			{
				((Bar)ContainerControl).RecalcLayout();
			}
			break;
		}
	}
}
