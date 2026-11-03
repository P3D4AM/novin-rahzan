using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[TypeConverter("DevComponents.DotNetBar.Design.RibbonTabItemGroupConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class RibbonTabItemGroup : Component
{
	private ElementStyle Ӷ;

	private string _0D5E = "";

	private RibbonStrip ൟ;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ArrayList DisplayPositions = new ArrayList();

	private eRibbonTabGroupColor ൠ;

	private string Ӿ = "";

	private string ൡ = "";

	[Category("Appearance")]
	[DefaultValue(eRibbonTabGroupColor.Default)]
	[Description("Indicates predefined color of the group when Office 12 style is used.")]
	[Browsable(true)]
	public eRibbonTabGroupColor Color
	{
		get
		{
			return ൠ;
		}
		set
		{
			if (ൠ != value)
			{
				ൠ = value;
				if (ParentRibbonStrip != null)
				{
					((Control)ParentRibbonStrip).Invalidate();
				}
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	[Browsable(true)]
	[DevCoBrowsable(false)]
	public string CustomColorName
	{
		get
		{
			return ൡ;
		}
		set
		{
			ൡ = value;
			if (ParentRibbonStrip != null)
			{
				((Control)ParentRibbonStrip).Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	[Category("Background")]
	[Description("Gets the style for tab group.")]
	public ElementStyle Style => Ӷ;

	[Localizable(true)]
	[DefaultValue("")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public string GroupTitle
	{
		get
		{
			return _0D5E;
		}
		set
		{
			_0D5E = value;
			if (base.DesignMode && ൟ != null)
			{
				ൟ.RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal RibbonStrip ParentRibbonStrip
	{
		get
		{
			return ൟ;
		}
		set
		{
			ൟ = value;
			if (ൟ != null)
			{
				Ӷ.SetColorScheme(ൟ.ColorScheme);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Visible
	{
		get
		{
			if (ൟ == null)
			{
				return true;
			}
			bool result = false;
			foreach (BaseItem item in ൟ.Items)
			{
				if (item is RibbonTabItem)
				{
					RibbonTabItem ribbonTabItem = item as RibbonTabItem;
					if (ribbonTabItem.Group == this && ribbonTabItem.Visible)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		set
		{
			if (ൟ == null)
			{
				return;
			}
			foreach (BaseItem item in ൟ.Items)
			{
				if (item is RibbonTabItem)
				{
					RibbonTabItem ribbonTabItem = item as RibbonTabItem;
					if (ribbonTabItem.Group == this)
					{
						ribbonTabItem.Visible = value;
					}
				}
			}
			ൟ.RecalcLayout();
		}
	}

	[Browsable(false)]
	public bool IsTabFromGroupSelected
	{
		get
		{
			if (ൟ == null)
			{
				return false;
			}
			foreach (BaseItem item in ൟ.Items)
			{
				if (item is RibbonTabItem)
				{
					RibbonTabItem ribbonTabItem = item as RibbonTabItem;
					if (ribbonTabItem.Group == this && ribbonTabItem.Checked)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	[Category("Design")]
	[Browsable(false)]
	[Description("Indicates the name used to identify the group.")]
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

	[Browsable(false)]
	public Rectangle[] TitleBounds => (Rectangle[])DisplayPositions.ToArray(typeof(Rectangle));

	public RibbonTabItemGroup()
	{
		Ӷ = new ElementStyle();
		Ӷ.StyleChanged += StyleChanged;
	}

	private void StyleChanged(object sender, EventArgs e)
	{
		if (ൟ != null)
		{
			((Control)ൟ).Refresh();
		}
	}

	public override string ToString()
	{
		if (_0D5E.Length > 0)
		{
			return _0D5E;
		}
		return base.ToString();
	}

	public void SelectFirstTab()
	{
		if (ൟ == null)
		{
			return;
		}
		foreach (BaseItem item in ൟ.Items)
		{
			if (item is RibbonTabItem)
			{
				RibbonTabItem ribbonTabItem = item as RibbonTabItem;
				if (ribbonTabItem.Group == this && ribbonTabItem.Visible)
				{
					ribbonTabItem.Checked = true;
					break;
				}
			}
		}
	}
}
