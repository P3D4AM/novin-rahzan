using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class RibbonTabItemGroup : Component
{
	private ElementStyle m_Style;

	private string m_GroupTitle = "";

	private RibbonStrip m_ParentRibbonStrip;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ArrayList DisplayPositions = new ArrayList();

	private eRibbonTabGroupColor m_Color;

	private string m_Name = "";

	private string m_CustomColorName = "";

	[Browsable(true)]
	[DefaultValue(eRibbonTabGroupColor.Default)]
	[Category("Appearance")]
	[Description("Indicates predefined color of the group when Office 12 style is used.")]
	public eRibbonTabGroupColor Color
	{
		get
		{
			return m_Color;
		}
		set
		{
			if (m_Color != value)
			{
				m_Color = value;
				if (ParentRibbonStrip != null)
				{
					((Control)ParentRibbonStrip).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	public string CustomColorName
	{
		get
		{
			return m_CustomColorName;
		}
		set
		{
			m_CustomColorName = value;
			if (ParentRibbonStrip != null)
			{
				((Control)ParentRibbonStrip).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[Category("Background")]
	[Description("Gets the style for tab group.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle Style => m_Style;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	public string GroupTitle
	{
		get
		{
			return m_GroupTitle;
		}
		set
		{
			m_GroupTitle = value;
			if (base.DesignMode && m_ParentRibbonStrip != null)
			{
				m_ParentRibbonStrip.RecalcLayout();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal RibbonStrip ParentRibbonStrip
	{
		get
		{
			return m_ParentRibbonStrip;
		}
		set
		{
			m_ParentRibbonStrip = value;
			if (m_ParentRibbonStrip != null)
			{
				m_Style.SetColorScheme(m_ParentRibbonStrip.ColorScheme);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Visible
	{
		get
		{
			if (m_ParentRibbonStrip == null)
			{
				return true;
			}
			bool result = false;
			foreach (BaseItem item in m_ParentRibbonStrip.Items)
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
			if (m_ParentRibbonStrip == null)
			{
				return;
			}
			foreach (BaseItem item in m_ParentRibbonStrip.Items)
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
			m_ParentRibbonStrip.RecalcLayout();
		}
	}

	[Browsable(false)]
	public bool IsTabFromGroupSelected
	{
		get
		{
			if (m_ParentRibbonStrip == null)
			{
				return false;
			}
			foreach (BaseItem item in m_ParentRibbonStrip.Items)
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

	[Browsable(false)]
	[Category("Design")]
	[Description("Indicates the name used to identify the group.")]
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

	[Browsable(false)]
	public Rectangle[] TitleBounds => (Rectangle[])DisplayPositions.ToArray(typeof(Rectangle));

	public RibbonTabItemGroup()
	{
		m_Style = new ElementStyle();
		m_Style.StyleChanged += StyleChanged;
	}

	private void StyleChanged(object sender, EventArgs e)
	{
		if (m_ParentRibbonStrip != null)
		{
			((Control)m_ParentRibbonStrip).Refresh();
		}
	}

	public override string ToString()
	{
		if (m_GroupTitle.Length > 0)
		{
			return m_GroupTitle;
		}
		return base.ToString();
	}

	public void SelectFirstTab()
	{
		if (m_ParentRibbonStrip == null)
		{
			return;
		}
		foreach (BaseItem item in m_ParentRibbonStrip.Items)
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
