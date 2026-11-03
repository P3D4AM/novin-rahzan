using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class MdiWindowListItem : ImageItem
{
	private bool ṉ;

	private ArrayList Ṋ = new ArrayList();

	private int ṋ = 22;

	private bool Ṍ = true;

	private bool ᔊ = true;

	private bool ṍ;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[DevCoBrowsable(true)]
	[Description("Specifies whether the MDI Child Window Icons are displayed on items.")]
	[Category("Appearance")]
	[Browsable(true)]
	public bool ShowWindowIcons
	{
		get
		{
			return Ṍ;
		}
		set
		{
			Ṍ = value;
		}
	}

	[DefaultValue(22)]
	[Description("Indicates maximum form caption length that will be displayed on each item. If caption length exceeds given value ... characters are added.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public int MaxCaptionLength
	{
		get
		{
			return ṋ;
		}
		set
		{
			if (ṋ != value)
			{
				ṋ = value;
				Ṑ();
			}
		}
	}

	[Browsable(false)]
	public bool IsInitialized => ṉ;

	[Category("Mdi Support")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate.")]
	[Browsable(true)]
	public bool MdiNoFormActivateFlicker
	{
		get
		{
			return ᔊ;
		}
		set
		{
			if (ᔊ != value)
			{
				ᔊ = value;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Mdi Support")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether numbered access keys are created for MDI Child window menu items for first 9 items.")]
	public bool CreateMdiChildAccessKeys
	{
		get
		{
			return ṍ;
		}
		set
		{
			if (ṍ != value)
			{
				ṍ = value;
				Ṑ();
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
		if (Ṋ != null && Ṋ.Count > 0)
		{
			foreach (BaseItem item in Ṋ)
			{
				item.Tag = null;
				if (Parent != null)
				{
					Parent.SubItems.Remove(item);
				}
			}
			Ṋ.Clear();
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
				_1A6B._1A6C(graphics, rect, pa.Colors.ItemDesignTimeBorder);
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
		if (ContainerControl != null && !ṉ)
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
			Ṍ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("showicons"));
		}
		if (itemXmlElement.HasAttribute("mdiaccesskey"))
		{
			ṍ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("mdiaccesskey"));
		}
		if (!ṉ)
		{
			Initialize();
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (!Ṍ)
		{
			itemXmlElement.SetAttribute("showicons", XmlConvert.ToString(Ṍ));
		}
		if (ṍ)
		{
			itemXmlElement.SetAttribute("mdiaccesskey", XmlConvert.ToString(ṍ));
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
				mdiForm.Load += ᕤ;
				return;
			}
			((Control)mdiClient).ControlAdded += new ControlEventHandler(MdiFormAdded);
			((Control)mdiClient).ControlRemoved += new ControlEventHandler(MdiFormRemoved);
			mdiForm.MdiChildActivate += ᕩ;
			ṉ = true;
			Ṑ();
		}
	}

	public void Initialize()
	{
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner)._10FB)) || ṉ)
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

	private void ᕤ(object _٠, EventArgs ؿ)
	{
		if (GetOwner() is IOwner { ParentForm: not null } owner)
		{
			owner.ParentForm.Load -= ᕤ;
			if (!ṉ)
			{
				Initialize();
			}
		}
	}

	private void Ṏ()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (GetOwner() is IOwner { ParentForm: not null } owner && ṉ)
		{
			MdiClient mdiClient = owner.GetMdiClient(owner.ParentForm);
			if (mdiClient != null)
			{
				((Control)mdiClient).ControlAdded -= new ControlEventHandler(MdiFormAdded);
				((Control)mdiClient).ControlRemoved -= new ControlEventHandler(MdiFormRemoved);
				owner.ParentForm.MdiChildActivate -= ᕩ;
				ṉ = false;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormAdded(object sender, ControlEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner)._10FB)) || !ṉ)
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
			Ṑ();
			return;
		}
		int position = Parent.SubItems.IndexOf(this) + 1;
		if (Ṋ.Count > 0)
		{
			position = Parent.SubItems.IndexOf((BaseItem)Ṋ[Ṋ.Count - 1]) + 1;
		}
		string itemText = ᕦ(((Control)val2).Text, Ṋ.Count + 1);
		ButtonItem buttonItem = new ButtonItem("mdi-" + ((object)val2).GetHashCode(), itemText);
		buttonItem.ShouldSerialize = false;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		if (Ṋ.Count == 0 && BeginGroup)
		{
			buttonItem.BeginGroup = true;
		}
		buttonItem.Tag = val2;
		if (val.ActiveMdiChild == val2)
		{
			buttonItem.Checked = true;
		}
		buttonItem.Click += ṏ;
		buttonItem.SetSystemItem(b: true);
		if (Ṍ && val2.Icon != null && val2.ControlBox)
		{
			object obj = val2.Icon.Clone();
			buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		Ṋ.Add(buttonItem);
		Parent.SubItems.Add(buttonItem, position);
		buttonItem.Visible = ((Control)val2).Visible;
		if (ṍ)
		{
			Ṓ();
		}
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl).RecalcLayout();
		}
		else if (ContainerControl is ItemControl)
		{
			((ItemControl)ContainerControl).RecalcLayout();
		}
		((Control)val2).TextChanged += ᕪ;
		((Control)val2).VisibleChanged += ᕫ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormRemoved(object sender, ControlEventArgs e)
	{
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			Ṑ();
			return;
		}
		((Control)val).TextChanged -= ᕪ;
		((Control)val).VisibleChanged -= ᕫ;
		foreach (BaseItem item in Ṋ)
		{
			if (item.Tag == val)
			{
				Parent.SubItems.Remove(item);
				item.Click -= ṏ;
				item.Dispose();
				item.Tag = null;
				Ṋ.Remove(item);
				if (BeginGroup && Ṋ.Count > 0)
				{
					((BaseItem)Ṋ[0]).BeginGroup = true;
				}
				break;
			}
		}
		if (Ṋ.Count == 0)
		{
			if (ContainerControl is Bar)
			{
				((Bar)ContainerControl).RecalcLayout();
			}
		}
		else if (ṍ)
		{
			Ṓ();
		}
	}

	private void ᕩ(object _٠, EventArgs ؿ)
	{
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner)._10FB)) || !ṉ)
		{
			return;
		}
		Form val = (Form)((_٠ is Form) ? _٠ : null);
		Form activeMdiChild = val.ActiveMdiChild;
		foreach (ButtonItem item in Ṋ)
		{
			if (item.Tag == activeMdiChild)
			{
				item.Checked = true;
			}
			else
			{
				item.Checked = false;
			}
		}
	}

	private void ṏ(object _٠, EventArgs ؿ)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		ButtonItem buttonItem = _٠ as ButtonItem;
		if (buttonItem.Checked)
		{
			return;
		}
		object tag = buttonItem.Tag;
		Form val = (Form)((tag is Form) ? tag : null);
		IOwner owner = GetOwner() as IOwner;
		Form mdiParent = val.MdiParent;
		if (owner != null && mdiParent != null && ṉ)
		{
			Form activeMdiChild = owner.ActiveMdiChild;
			MdiClient mdiClient = owner.GetMdiClient(mdiParent);
			bool flag = false;
			if (mdiClient != null && ᔊ && ((int)val.WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
			{
				_089F.SendMessage(((Control)mdiClient).Handle, 11, 0, 0);
				flag = true;
			}
			val.Activate();
			if (flag)
			{
				_089F.SendMessage(((Control)mdiClient).Handle, 11, 1, 0);
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

	private void Ṑ()
	{
		if (Parent == null || Ṋ == null)
		{
			return;
		}
		Parent.SuspendLayout = true;
		if (Ṋ.Count > 0)
		{
			foreach (BaseItem item in Ṋ)
			{
				item.Tag = null;
				Parent.SubItems.Remove(item);
			}
			Ṋ.Clear();
		}
		if (!(GetOwner() is IOwner owner) || (owner.ParentForm == null && (!(owner is RibbonBar) || !((RibbonBar)owner)._10FB)) || !ṉ)
		{
			Parent.SuspendLayout = false;
			return;
		}
		Form val = owner.ParentForm;
		if (owner is RibbonBar && ((RibbonBar)owner)._10FB && val == null)
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
			string itemText = ᕦ(((Control)val2).Text, num2);
			ButtonItem buttonItem = new ButtonItem("mdi-" + ((Control)val2).Handle, itemText);
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
			buttonItem.Click += ṏ;
			buttonItem.SetSystemItem(b: true);
			if (Ṍ && val2.Icon != null && val2.ControlBox)
			{
				object obj = val2.Icon.Clone();
				buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
			}
			Ṋ.Add(buttonItem);
			Parent.SubItems.Add(buttonItem, num);
			buttonItem.Visible = ((Control)val2).Visible;
			num++;
			((Control)val2).TextChanged += ᕪ;
			num2++;
		}
		Parent.SuspendLayout = false;
		if (Ṋ.Count > 0 && ContainerControl is Bar)
		{
			((Bar)ContainerControl).RecalcLayout();
		}
	}

	private string ᕦ(string _06EA, int ԡ)
	{
		if (_06EA.Length > ṋ)
		{
			_06EA = _06EA.Substring(0, ṋ) + "...";
		}
		if (ṍ && ԡ < 10)
		{
			_06EA = "&" + ԡ + ". " + _06EA;
		}
		return _06EA;
	}

	public void RefreshButtonIcons()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (!Ṍ || Ṋ == null)
		{
			return;
		}
		foreach (ButtonItem item in Ṋ)
		{
			try
			{
				if (item.Tag is Form && ((Form)item.Tag).ControlBox)
				{
					object obj = ((Form)item.Tag).Icon.Clone();
					item.Icon = (Icon)((obj is Icon) ? obj : null);
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
			Parent.ExpandChange -= ṑ;
		}
		base.SetParent(o);
		if (Parent != null)
		{
			Parent.ExpandChange += ṑ;
		}
	}

	private void ṑ(object _٠, EventArgs ؿ)
	{
		if (Parent != null && Parent.Expanded)
		{
			RefreshButtonIcons();
		}
	}

	private void ᕪ(object _٠, EventArgs ؿ)
	{
		Form val = (Form)((_٠ is Form) ? _٠ : null);
		if (val == null)
		{
			return;
		}
		for (int i = 0; i < Ṋ.Count; i++)
		{
			BaseItem baseItem = Ṋ[i] as BaseItem;
			if (baseItem.Tag == val)
			{
				baseItem.Text = ᕦ(((Control)val).Text, i + 1);
				break;
			}
		}
	}

	private void Ṓ()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Ṋ.Count; i++)
		{
			BaseItem baseItem = Ṋ[i] as BaseItem;
			baseItem.Text = ᕦ(((Control)(Form)baseItem.Tag).Text, i + 1);
		}
	}

	private void ᕫ(object _٠, EventArgs ؿ)
	{
		Form val = (Form)((_٠ is Form) ? _٠ : null);
		if (val == null)
		{
			return;
		}
		if (ṍ)
		{
			Ṓ();
		}
		foreach (BaseItem item in Ṋ)
		{
			if (item.Tag != val)
			{
				continue;
			}
			item.Visible = ((Control)val).Visible;
			if (BeginGroup)
			{
				bool flag = true;
				foreach (BaseItem item2 in Ṋ)
				{
					if (flag && item2.Visible)
					{
						item2.BeginGroup = true;
						flag = false;
					}
					else
					{
						item2.BeginGroup = false;
					}
				}
			}
			if (ContainerControl is Bar && item.Visible)
			{
				((Bar)ContainerControl).RecalcLayout();
			}
			break;
		}
	}
}
