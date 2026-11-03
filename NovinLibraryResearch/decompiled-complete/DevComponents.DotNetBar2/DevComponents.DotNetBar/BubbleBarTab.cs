using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.BubbleBarTabDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class BubbleBarTab : Component, IBlock, ISimpleTab
{
	private string ө = "";

	private BubbleBar ӭ;

	private bool Ӻ = true;

	private Rectangle ᕮ = Rectangle.Empty;

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

	private BubbleButtonCollection _27C7;

	private bool _27C4;

	private Padding __0AE5 = new Padding(0);

	[Category("Buttons")]
	[Description("Returns collection of buttons on the tab.")]
	[Editor("DevComponents.DotNetBar.Design.BubbleButtonCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public BubbleButtonCollection Buttons => _27C7;

	[Localizable(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates the text displayed on the tab.")]
	[Category("Appearance")]
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
					ӭ._2812(this);
				}
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the tab is visible.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
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

	[Browsable(false)]
	public Rectangle DisplayRectangle => ᕮ;

	[Browsable(false)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("Indicates text that contains data about the cell.")]
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

	[Category("Data")]
	[Browsable(true)]
	[Description("Indicates text that contains data about the cell.")]
	[DefaultValue("")]
	public string TagString
	{
		get
		{
			if (ӧ == null)
			{
				return "";
			}
			return ӧ.ToString();
		}
		set
		{
			ӧ = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab background color.")]
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

	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	[Category("Style")]
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

	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(90)]
	[Description("Indicates the gradient angle.")]
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

	[Browsable(true)]
	[Description("Indicates the inactive tab light border color.")]
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

	[Browsable(true)]
	[Description("Indicates the inactive tab dark border color.")]
	[Category("Style")]
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

	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates the inactive tab text color.")]
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

	[Category("Design")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
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
	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(eTabItemColor.Default)]
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
			Refresh();
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
	public bool IsMouseOver
	{
		get
		{
			if (ӭ != null)
			{
				return ӭ._2813() == this;
			}
			return false;
		}
	}

	eTabStripAlignment ISimpleTab.TabAlignment
	{
		get
		{
			if (ӭ != null && ӭ.Alignment == eBubbleButtonAlignment.Top)
			{
				return eTabStripAlignment.Bottom;
			}
			return eTabStripAlignment.Top;
		}
	}

	[Browsable(false)]
	public BubbleBar Parent => ӭ;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Focus
	{
		get
		{
			return _27C4;
		}
		set
		{
			_27C4 = value;
		}
	}

	Rectangle IBlock.Bounds
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	public BubbleBarTab()
		: this(null)
	{
	}

	public BubbleBarTab(IContainer container)
	{
		container?.Add(this);
		_27C7 = new BubbleButtonCollection(this);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !ފ.IsEmpty;
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
		return !ތ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	internal void SetParent(BubbleBar parent)
	{
		ӭ = parent;
	}

	private void Refresh()
	{
		if (ӭ != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Width += 16;
			displayRectangle.X -= 8;
			((Control)ӭ).Invalidate(displayRectangle);
			((Control)ӭ).Update();
		}
	}

	internal void OnButtonInserted(BubbleButton button)
	{
		if (ӭ != null)
		{
			ӭ._2804(this, button);
		}
	}

	internal void OnButtonRemoved(BubbleButton button)
	{
		if (ӭ != null)
		{
			ӭ._2802(this, button);
		}
	}

	internal void OnButtonsCollectionClear()
	{
		if (ӭ != null)
		{
			ӭ._2801(this);
		}
	}
}
