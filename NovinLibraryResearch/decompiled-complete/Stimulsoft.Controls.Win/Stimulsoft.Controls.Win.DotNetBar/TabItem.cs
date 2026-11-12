using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
[DesignTimeVisible(false)]
public class TabItem : Component, IBlock, ISimpleTab
{
	private int m_ImageIndex = -1;

	private Image m_Image;

	private Icon m_Icon;

	private string m_Text = "";

	private TabStrip m_Parent;

	private bool m_Visible = true;

	private Rectangle m_DisplayRectangle = Rectangle.Empty;

	private BaseItem m_DotNetBarItem;

	private Control m_BoundControl;

	private object m_ItemData;

	private Color m_BackColor = Color.Empty;

	private Color m_BackColor2 = Color.Empty;

	private int m_BackColorGradientAngle = 90;

	private Color m_LightBorderColor = Color.Empty;

	private Color m_DarkBorderColor = Color.Empty;

	private Color m_BorderColor = Color.Empty;

	private Color m_TextColor = Color.Empty;

	private string m_Name = "";

	private eTabItemColor m_PredefinedColor;

	private object m_Tag;

	private string m_Tooltip = "";

	private Rectangle m_CloseButtonBounds = Rectangle.Empty;

	private bool m_CloseButtonMouseOver;

	private bool m_CloseButtonVisible = true;

	[CompilerGenerated]
	private MouseEventHandler m_MouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_MouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_MouseMove;

	private Padding _Margin = new Padding(0);

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	[Category("Appearance")]
	[Description("Indicates the tab image index")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return m_ImageIndex;
		}
		set
		{
			if (m_ImageIndex != value)
			{
				m_ImageIndex = value;
				if (m_Parent != null)
				{
					m_Parent.NeedRecalcSize = true;
					m_Parent.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (m_Parent != null)
			{
				return m_Parent.ImageList;
			}
			return null;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates the tab image.")]
	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			if (m_Image != value)
			{
				m_Image = value;
				if (m_Parent != null)
				{
					m_Parent.NeedRecalcSize = true;
					m_Parent.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates the tab icon. Icon has same functionality as Image except that it support Alpha blending.")]
	public Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			if (m_Icon != value)
			{
				m_Icon = value;
				if (m_Parent != null)
				{
					m_Parent.NeedRecalcSize = true;
					m_Parent.ResetTabHeight();
				}
				Refresh();
				OnImageChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the text displayed on the tab.")]
	[Localizable(true)]
	public string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (!(m_Text == value))
			{
				m_Text = value;
				if (m_Parent != null)
				{
					m_Parent.NeedRecalcSize = true;
				}
				Refresh();
				OnTextChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the tab is visible.")]
	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			if (m_Visible != value)
			{
				m_Visible = value;
				if (m_Parent != null)
				{
					m_Parent.OnTabVisibleChanged(this);
				}
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public Rectangle DisplayRectangle => m_DisplayRectangle;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle CloseButtonBounds
	{
		get
		{
			return m_CloseButtonBounds;
		}
		set
		{
			m_CloseButtonBounds = value;
		}
	}

	internal Rectangle _DisplayRectangle
	{
		get
		{
			return m_DisplayRectangle;
		}
		set
		{
			m_DisplayRectangle = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem AttachedItem
	{
		get
		{
			return m_DotNetBarItem;
		}
		set
		{
			m_DotNetBarItem = value;
		}
	}

	[Browsable(false)]
	internal bool CloseButtonMouseOver
	{
		get
		{
			return m_CloseButtonMouseOver;
		}
		set
		{
			m_CloseButtonMouseOver = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates the control that is attached to this tab.")]
	public Control AttachedControl
	{
		get
		{
			return m_BoundControl;
		}
		set
		{
			m_BoundControl = value;
			if (m_BoundControl != null && m_Parent != null && !m_Parent.MdiTabbedDocuments)
			{
				if (m_Parent.SelectedTab == this)
				{
					m_BoundControl.Visible = true;
				}
				else
				{
					m_BoundControl.Visible = false;
				}
			}
		}
	}

	public TabStrip Parent => m_Parent;

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public object ItemData
	{
		get
		{
			return m_ItemData;
		}
		set
		{
			m_ItemData = value;
		}
	}

	internal Size IconSize
	{
		get
		{
			double factor = StiScale2.Factor;
			return new Size((int)Math.Ceiling(16.0 * factor), (int)Math.Ceiling(16.0 * factor));
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab background color.")]
	[Category("Style")]
	public Color BackColor
	{
		get
		{
			return m_BackColor;
		}
		set
		{
			m_BackColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return m_BackColor2;
		}
		set
		{
			m_BackColor2 = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the gradient angle.")]
	[Category("Style")]
	[DefaultValue(90)]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			m_BackColorGradientAngle = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab light border color.")]
	[Category("Style")]
	public Color LightBorderColor
	{
		get
		{
			return m_LightBorderColor;
		}
		set
		{
			m_LightBorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab dark border color.")]
	[Category("Style")]
	public Color DarkBorderColor
	{
		get
		{
			return m_DarkBorderColor;
		}
		set
		{
			m_DarkBorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab border color.")]
	[Category("Style")]
	public Color BorderColor
	{
		get
		{
			return m_BorderColor;
		}
		set
		{
			m_BorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab text color.")]
	[Category("Style")]
	public Color TextColor
	{
		get
		{
			return m_TextColor;
		}
		set
		{
			m_TextColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				m_Name = Site.Name;
			}
			return m_Name;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				m_Name = "";
			}
			else
			{
				m_Name = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabItemColor.Default)]
	[Category("Style")]
	[Description("Applies predefined color to tab.")]
	public eTabItemColor PredefinedColor
	{
		get
		{
			return m_PredefinedColor;
		}
		set
		{
			m_PredefinedColor = value;
			TabColorScheme.ApplyPredefinedColor(this, m_PredefinedColor);
			if (base.DesignMode)
			{
				if (m_Parent != null && ((Control)m_Parent).Parent is TabControl)
				{
					((Control)m_Parent).Parent.Invalidate(true);
					((TabControl)(object)((Control)m_Parent).Parent).ApplyDefaultPanelStyle(AttachedControl as TabControlPanel);
					((Control)m_Parent).Parent.Update();
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
			return m_Tag;
		}
		set
		{
			m_Tag = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the tab.")]
	[Localizable(true)]
	public string Tooltip
	{
		get
		{
			return m_Tooltip;
		}
		set
		{
			m_Tooltip = value;
		}
	}

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			if (m_Parent != null)
			{
				return m_Parent.SelectedTab == this;
			}
			return false;
		}
	}

	[Browsable(false)]
	public eTabStripAlignment TabAlignment => m_Parent.TabAlignment;

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			if (m_Parent != null)
			{
				return m_Parent.MouseOverTab == this;
			}
			return false;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether Close button on the tab is visible when TabStrip.CloseButtonOnTabsVisible property is set to true.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool CloseButtonVisible
	{
		get
		{
			return m_CloseButtonVisible;
		}
		set
		{
			if (m_CloseButtonVisible != value)
			{
				m_CloseButtonVisible = value;
				TabStrip parent = m_Parent;
				if (parent != null && parent.CloseButtonOnTabsVisible)
				{
					parent.RecalcSize();
					((Control)parent).Invalidate();
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	Padding IBlock.Margin
	{
		get
		{
			return _Margin;
		}
		set
		{
			_Margin = value;
		}
	}

	public event MouseEventHandler MouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler MouseUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler MouseHover;

	public event EventHandler MouseEnter;

	public event EventHandler MouseLeave;

	public event MouseEventHandler MouseMove
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler Click;

	public event EventHandler DoubleClick;

	public TabItem(IContainer container)
	{
		container.Add(this);
	}

	public TabItem()
	{
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIcon()
	{
		Icon = null;
	}

	private void Refresh()
	{
		if (m_Parent != null)
		{
			((Control)m_Parent).Refresh();
		}
	}

	internal void SetParent(TabStrip tabstrip)
	{
		m_Parent = tabstrip;
	}

	internal Image GetImage()
	{
		if (m_Image != null)
		{
			return m_Image;
		}
		if (m_ImageIndex >= 0 && m_Parent != null && m_Parent.ImageList != null && m_ImageIndex < m_Parent.ImageList.Images.Count)
		{
			return m_Parent.ImageList.Images[m_ImageIndex];
		}
		return null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !m_BackColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !m_BackColor2.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLightBorderColor()
	{
		return !m_LightBorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLightBorderColor()
	{
		LightBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDarkBorderColor()
	{
		return !m_DarkBorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDarkBorderColor()
	{
		DarkBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !m_BorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !m_TextColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public override string ToString()
	{
		return m_Text;
	}

	public Font GetTabFont()
	{
		if (m_Parent != null)
		{
			return ((Control)m_Parent).Font;
		}
		return SystemFonts.DefaultFont;
	}

	private void OnImageChanged()
	{
		if (base.DesignMode && m_Parent != null)
		{
			if (((Control)m_Parent).Parent is TabControl)
			{
				((TabControl)(object)((Control)m_Parent).Parent).SyncTabStripSize();
				((TabControl)(object)((Control)m_Parent).Parent).RecalcLayout();
			}
			else
			{
				m_Parent.RecalcSize();
				((Control)m_Parent).Refresh();
			}
		}
	}

	private void OnTextChanged()
	{
		OnImageChanged();
	}

	internal void InvokeMouseDown(MouseEventArgs e)
	{
		if (this.MouseDown != null)
		{
			this.MouseDown.Invoke((object)this, e);
		}
	}

	internal void InvokeMouseUp(MouseEventArgs e)
	{
		if (this.MouseUp != null)
		{
			this.MouseUp.Invoke((object)this, e);
		}
	}

	public void PerformClick()
	{
		InvokeClick(new EventArgs());
	}

	internal void InvokeClick(EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(this, e);
		}
	}

	internal void InvokeDoubleClick(EventArgs e)
	{
		if (this.DoubleClick != null)
		{
			this.DoubleClick(this, e);
		}
	}

	internal void InvokeMouseEnter(EventArgs e)
	{
		if (this.MouseEnter != null)
		{
			this.MouseEnter(this, e);
		}
	}

	internal void InvokeMouseLeave(EventArgs e)
	{
		if (this.MouseLeave != null)
		{
			this.MouseLeave(this, e);
		}
	}

	internal void InvokeMouseHover(EventArgs e)
	{
		if (this.MouseHover != null)
		{
			this.MouseHover(this, e);
		}
	}

	internal void InvokeMouseMove(MouseEventArgs e)
	{
		if (this.MouseMove != null)
		{
			this.MouseMove.Invoke((object)this, e);
		}
	}
}
