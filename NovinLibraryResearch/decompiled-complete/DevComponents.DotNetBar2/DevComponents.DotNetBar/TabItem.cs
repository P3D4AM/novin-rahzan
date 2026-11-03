using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.TabItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class TabItem : Component, IBlock, ISimpleTab
{
	private int դ = -1;

	private Image _0E66;

	private Icon ຈ;

	private string ө = "";

	private TabStrip ӭ;

	private bool Ӻ = true;

	private Rectangle ᕮ = Rectangle.Empty;

	private BaseItem ᕯ;

	private Control ᕰ;

	private object ᕱ;

	private Color ފ = Color.Empty;

	private Color ތ = Color.Empty;

	private int ގ = 90;

	private Color ᕲ = Color.Empty;

	private Color ᕳ = Color.Empty;

	private Color _07A8 = Color.Empty;

	private Color ޗ = Color.Empty;

	private string Ӿ = "";

	private eTabItemColor ᕴ;

	private object ӧ;

	private string ᕵ = "";

	private Rectangle ᕶ = Rectangle.Empty;

	private bool ᕷ;

	private bool ᕸ = true;

	private MouseEventHandler ग़;

	private MouseEventHandler ફ;

	private EventHandler ય;

	private EventHandler બ;

	private EventHandler ભ;

	private MouseEventHandler મ;

	private EventHandler દ;

	private TabItemAccessibleObject _ૠ;

	private Padding __0AE5 = new Padding(0);

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public virtual AccessibleObject AccessibleObject => CreateAccessibilityInstance();

	[Description("Indicates the tab image index")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			if (դ != value)
			{
				դ = value;
				if (ӭ != null)
				{
					ӭ.ᓧ = true;
					ӭ.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			if (ӭ != null)
			{
				return ӭ.ImageList;
			}
			return null;
		}
	}

	[Category("Appearance")]
	[Description("Indicates the tab image.")]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			if (_0E66 != value)
			{
				_0E66 = value;
				if (ӭ != null)
				{
					ӭ.ᓧ = true;
					ӭ.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates the tab icon. Icon has same functionality as Image except that it support Alpha blending.")]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Icon Icon
	{
		get
		{
			return ຈ;
		}
		set
		{
			if (ຈ != value)
			{
				ຈ = value;
				if (ӭ != null)
				{
					ӭ.ᓧ = true;
					ӭ.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates the text displayed on the tab.")]
	[DevCoBrowsable(true)]
	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			if (!(ө == value))
			{
				ө = value;
				if (ӭ != null)
				{
					ӭ.ᓧ = true;
				}
				Refresh();
				OnTextChanged();
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether the tab is visible.")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			if (Ӻ != value)
			{
				Ӻ = value;
				if (ӭ != null)
				{
					ӭ.ᕛ(this);
				}
				Refresh();
			}
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	public Rectangle DisplayRectangle => ᕮ;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle CloseButtonBounds
	{
		get
		{
			return ᕶ;
		}
		set
		{
			ᕶ = value;
		}
	}

	internal Rectangle _DisplayRectangle
	{
		get
		{
			return ᕮ;
		}
		set
		{
			ᕮ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public BaseItem AttachedItem
	{
		get
		{
			return ᕯ;
		}
		set
		{
			ᕯ = value;
		}
	}

	[Browsable(false)]
	internal bool CloseButtonMouseOver
	{
		get
		{
			return ᕷ;
		}
		set
		{
			ᕷ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates the control that is attached to this tab.")]
	[DefaultValue(null)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Control AttachedControl
	{
		get
		{
			return ᕰ;
		}
		set
		{
			ᕰ = value;
			if (ᕰ != null && ӭ != null && !ӭ.MdiTabbedDocuments)
			{
				if (ӭ.SelectedTab == this)
				{
					ᕰ.Visible = true;
				}
				else
				{
					ᕰ.Visible = false;
				}
			}
		}
	}

	public TabStrip Parent => ӭ;

	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Browsable(false)]
	public object ItemData
	{
		get
		{
			return ᕱ;
		}
		set
		{
			ᕱ = value;
		}
	}

	internal Size IconSize => new Size(Dpi.Width16, Dpi.Height16);

	[Description("Indicates the inactive tab background color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color BackColor
	{
		get
		{
			return ފ;
		}
		set
		{
			ފ = value;
			Refresh();
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	public Color BackColor2
	{
		get
		{
			return ތ;
		}
		set
		{
			ތ = value;
			Refresh();
		}
	}

	[DefaultValue(90)]
	[Browsable(true)]
	[Description("Indicates the gradient angle.")]
	[Category("Style")]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			ގ = value;
			Refresh();
		}
	}

	[Description("Indicates the inactive tab light border color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color LightBorderColor
	{
		get
		{
			return ᕲ;
		}
		set
		{
			ᕲ = value;
			Refresh();
		}
	}

	[Description("Indicates the inactive tab dark border color.")]
	[Category("Style")]
	[Browsable(true)]
	public Color DarkBorderColor
	{
		get
		{
			return ᕳ;
		}
		set
		{
			ᕳ = value;
			Refresh();
		}
	}

	[Description("Indicates the inactive tab border color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color BorderColor
	{
		get
		{
			return _07A8;
		}
		set
		{
			_07A8 = value;
			Refresh();
		}
	}

	[Description("Indicates the inactive tab text color.")]
	[Browsable(true)]
	[Category("Style")]
	public Color TextColor
	{
		get
		{
			return ޗ;
		}
		set
		{
			ޗ = value;
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Design")]
	[Browsable(true)]
	[Description("Indicates the name used to identify item.")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				Ӿ = Site.Name;
			}
			return Ӿ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[Description("Applies predefined color to tab.")]
	[DefaultValue(eTabItemColor.Default)]
	[Browsable(true)]
	[Category("Style")]
	public eTabItemColor PredefinedColor
	{
		get
		{
			return ᕴ;
		}
		set
		{
			ᕴ = value;
			TabColorScheme.ApplyPredefinedColor(this, ᕴ);
			if (base.DesignMode)
			{
				if (ӭ != null && ((Control)ӭ).Parent is TabControl)
				{
					((Control)ӭ).Parent.Invalidate(true);
					((TabControl)(object)((Control)ӭ).Parent).ApplyDefaultPanelStyle(AttachedControl as TabControlPanel);
					((Control)ӭ).Parent.Update();
				}
				else
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public object Tag
	{
		get
		{
			return ӧ;
		}
		set
		{
			ӧ = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the tab.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Localizable(true)]
	public string Tooltip
	{
		get
		{
			return ᕵ;
		}
		set
		{
			ᕵ = value;
		}
	}

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			if (ӭ != null)
			{
				return ӭ.SelectedTab == this;
			}
			return false;
		}
	}

	[Browsable(false)]
	public eTabStripAlignment TabAlignment => ӭ.TabAlignment;

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			if (ӭ != null)
			{
				return ӭ.ᓤ == this;
			}
			return false;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether Close button on the tab is visible when TabStrip.CloseButtonOnTabsVisible property is set to true.")]
	public bool CloseButtonVisible
	{
		get
		{
			return ᕸ;
		}
		set
		{
			if (ᕸ != value)
			{
				ᕸ = value;
				TabStrip tabStrip = ӭ;
				if (tabStrip != null && tabStrip.CloseButtonOnTabsVisible)
				{
					tabStrip.RecalcSize();
					((Control)tabStrip).Invalidate();
				}
			}
		}
	}

	Rectangle IBlock.Bounds
	{
		get
		{
			return _DisplayRectangle;
		}
		set
		{
			_DisplayRectangle = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	Padding IBlock.Margin
	{
		get
		{
			return __0AE5;
		}
		set
		{
			__0AE5 = value;
		}
	}

	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler MouseHover
	{
		add
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler MouseEnter
	{
		add
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler MouseLeave
	{
		add
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler Click
	{
		add
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public TabItem(IContainer container)
	{
		container.Add(this);
	}

	public TabItem()
	{
	}

	protected virtual AccessibleObject CreateAccessibilityInstance()
	{
		if (_ૠ == null)
		{
			_ૠ = new TabItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_ૠ;
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndex()
	{
		ImageIndex = -1;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImage()
	{
		Image = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public void ResetIcon()
	{
		Icon = null;
	}

	private void Refresh()
	{
		if (ӭ != null)
		{
			((Control)ӭ).Refresh();
		}
	}

	internal void SetParent(TabStrip tabstrip)
	{
		ӭ = tabstrip;
	}

	internal Image GetImage()
	{
		if (_0E66 != null)
		{
			return _0E66;
		}
		if (դ >= 0 && ӭ != null && ӭ.ImageList != null && դ < ӭ.ImageList.Images.Count)
		{
			return ӭ.ImageList.Images[դ];
		}
		return null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !ފ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBackColor2()
	{
		return !ތ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLightBorderColor()
	{
		return !ᕲ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetLightBorderColor()
	{
		LightBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDarkBorderColor()
	{
		return !ᕳ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDarkBorderColor()
	{
		DarkBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBorderColor()
	{
		return !_07A8.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTextColor()
	{
		return !ޗ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public override string ToString()
	{
		return ө;
	}

	public Font GetTabFont()
	{
		if (ӭ != null)
		{
			return ((Control)ӭ).Font;
		}
		return SystemFonts.DefaultFont;
	}

	private void OnImageChanged()
	{
		if (base.DesignMode && ӭ != null)
		{
			if (((Control)ӭ).Parent is TabControl)
			{
				((TabControl)(object)((Control)ӭ).Parent).SyncTabStripSize();
				((TabControl)(object)((Control)ӭ).Parent).RecalcLayout();
			}
			else
			{
				ӭ.RecalcSize();
				((Control)ӭ).Refresh();
			}
		}
	}

	private void OnTextChanged()
	{
		OnImageChanged();
	}

	internal void InvokeMouseDown(MouseEventArgs e)
	{
		if (ग़ != null)
		{
			ग़.Invoke((object)this, e);
		}
		if (ᕯ != null && ᕯ is DockContainerItem)
		{
			ᕯ.InternalMouseDown(e);
		}
	}

	internal void InvokeMouseUp(MouseEventArgs e)
	{
		if (ફ != null)
		{
			ફ.Invoke((object)this, e);
		}
		if (ᕯ != null && ᕯ is DockContainerItem)
		{
			ᕯ.InternalMouseUp(e);
		}
	}

	public void PerformClick()
	{
		InvokeClick(new EventArgs());
	}

	internal void InvokeClick(EventArgs e)
	{
		if (દ != null)
		{
			દ(this, e);
		}
	}

	internal void InvokeMouseEnter(EventArgs e)
	{
		if (બ != null)
		{
			બ(this, e);
		}
		if (ӭ != null)
		{
			ӭ.OnTabMouseEnter(this, e);
		}
	}

	internal void InvokeMouseLeave(EventArgs e)
	{
		if (ભ != null)
		{
			ભ(this, e);
		}
		if (ӭ != null)
		{
			ӭ.OnTabMouseLeave(this, e);
		}
	}

	internal void InvokeMouseHover(EventArgs e)
	{
		if (ય != null)
		{
			ય(this, e);
		}
		if (ӭ != null)
		{
			ӭ.OnTabMouseHover(this, e);
		}
	}

	internal void InvokeMouseMove(MouseEventArgs e)
	{
		if (મ != null)
		{
			મ.Invoke((object)this, e);
		}
		if (ӭ != null)
		{
			ӭ.OnTabMouseMove(this, e);
		}
	}
}
