using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(RibbonBarMergeContainer), "Ribbon.RibbonControl.ico")]
[ToolboxItem(false)]
public class RibbonBarMergeContainer : RibbonPanel
{
	private bool m_AutoActivateTab = true;

	private string m_RibbonTabText = "";

	private string m_MergeIntoRibbonTabItemName = "";

	private ArrayList m_MergedRibbonBarsList = new ArrayList();

	private bool m_RibbonTabItemCreated;

	private bool m_PreMergedVisibleState;

	private bool m_AllowMerge = true;

	private string m_MergeRibbonGroupName = "";

	private int m_MergeRibbonTabItemIndex = -1;

	private eRibbonTabColor m_RibbonTabColorTable;

	private string _RibbonTabKeyTips = "";

	public bool IsMerged => m_MergedRibbonBarsList.Count > 0;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behaviour")]
	[Description("Indicates whether RibbonTab item the RibbonBar controls are added to when merged is automatically activated (selected) after controls are merged.")]
	public bool AutoActivateTab
	{
		get
		{
			return m_AutoActivateTab;
		}
		set
		{
			m_AutoActivateTab = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behaviour")]
	[Description("Indicates whether merge functionality is enabled for the container.")]
	public bool AllowMerge
	{
		get
		{
			return m_AllowMerge;
		}
		set
		{
			m_AllowMerge = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Data")]
	[Description("Indicates the Ribbon Tab text for the tab that will be created when ribbon bar objects from this container are merged into the ribbon.")]
	public string RibbonTabText
	{
		get
		{
			return m_RibbonTabText;
		}
		set
		{
			m_RibbonTabText = value;
			if (IsMerged && base.RibbonTabItem != null)
			{
				base.RibbonTabItem.Text = value;
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the Ribbon Tab.")]
	public virtual string RibbonTabKeyTips
	{
		get
		{
			return _RibbonTabKeyTips;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_RibbonTabKeyTips = value.ToUpper();
			if (IsMerged && base.RibbonTabItem != null)
			{
				base.RibbonTabItem.KeyTips = _RibbonTabKeyTips;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eRibbonTabColor.Default)]
	[Category("Appearance")]
	[Description("Indicates predefined color for the ribbon tab that is created when ribbon bar controls are merged into the ribbon.")]
	public eRibbonTabColor RibbonTabColorTable
	{
		get
		{
			return m_RibbonTabColorTable;
		}
		set
		{
			m_RibbonTabColorTable = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates the name of RibbonTabItem object that already exists on Ribbon control into which the RibbonBar controls are merged.")]
	public string MergeIntoRibbonTabItemName
	{
		get
		{
			return m_MergeIntoRibbonTabItemName;
		}
		set
		{
			m_MergeIntoRibbonTabItemName = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates the name of the RibbonTabItemGroup the new Ribbon Tab Item that is created will be added to.")]
	public string MergeRibbonGroupName
	{
		get
		{
			return m_MergeRibbonGroupName;
		}
		set
		{
			m_MergeRibbonGroupName = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(-1)]
	[Category("Data")]
	[Description("Indicates the insertion index for the ribbon tab item that is created when ribbon bars are merged into the ribbon control.")]
	public int MergeRibbonTabItemIndex
	{
		get
		{
			return m_MergeRibbonTabItemIndex;
		}
		set
		{
			m_MergeRibbonTabItemIndex = value;
		}
	}

	public event EventHandler BeforeRibbonMerge;

	public event EventHandler AfterRibbonMerge;

	public event EventHandler BeforeRibbonUnmerge;

	public event EventHandler AfterRibbonUnmerge;

	public virtual void RemoveMergedRibbonBars(RibbonControl ribbon)
	{
		OnBeforeRibbonUnmerge(new EventArgs());
		Control val = null;
		foreach (RibbonBar mergedRibbonBars in m_MergedRibbonBarsList)
		{
			if (((Control)mergedRibbonBars).Parent != null)
			{
				if (val == null)
				{
					val = ((Control)mergedRibbonBars).Parent;
					val.SuspendLayout();
				}
				((Control)mergedRibbonBars).Parent.Controls.Remove((Control)(object)mergedRibbonBars);
			}
			((Control)this).Controls.Add((Control)(object)mergedRibbonBars);
			((Control)mergedRibbonBars).Enabled = false;
		}
		m_MergedRibbonBarsList.Clear();
		if (val != null)
		{
			val.ResumeLayout();
		}
		if (m_RibbonTabItemCreated)
		{
			base.RibbonTabItem.Parent.SubItems.Remove(base.RibbonTabItem);
			if (base.RibbonTabItem.Panel != null && ((Control)base.RibbonTabItem.Panel).Parent != null)
			{
				((Control)base.RibbonTabItem.Panel).Parent.Controls.Remove((Control)(object)base.RibbonTabItem.Panel);
			}
			((Component)(object)base.RibbonTabItem.Panel).Dispose();
			base.RibbonTabItem.Panel = null;
			m_RibbonTabItemCreated = false;
		}
		base.RibbonTabItem = null;
		if (base.Visible != m_PreMergedVisibleState)
		{
			base.Visible = m_PreMergedVisibleState;
		}
		OnAfterRibbonUnmerge(new EventArgs());
	}

	public virtual void MergeRibbonBars(RibbonControl ribbon)
	{
		OnBeforeRibbonMerge(new EventArgs());
		m_PreMergedVisibleState = base.Visible;
		if (base.Visible)
		{
			base.Visible = false;
		}
		RibbonTabItem createRibbonTabItem = GetCreateRibbonTabItem(ribbon);
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		int num = ((Control)createRibbonTabItem.Panel).Width + 1;
		((Control)createRibbonTabItem.Panel).SuspendLayout();
		Control[] array2 = array;
		foreach (Control val in array2)
		{
			if (val is RibbonBar)
			{
				((Control)this).Controls.Remove(val);
				if (m_MergeIntoRibbonTabItemName.Length > 0 && !createRibbonTabItem.Panel.DefaultLayout)
				{
					val.Left = num;
					num += val.Width;
				}
				((Control)createRibbonTabItem.Panel).Controls.Add(val);
				val.Enabled = true;
				m_MergedRibbonBarsList.Add(val);
			}
		}
		((Control)createRibbonTabItem.Panel).ResumeLayout();
		OnAfterRibbonMerge(new EventArgs());
	}

	private RibbonTabItem GetCreateRibbonTabItem(RibbonControl ribbon)
	{
		if (m_MergeIntoRibbonTabItemName != "")
		{
			if (ribbon.Items.IndexOf(m_MergeIntoRibbonTabItemName) < 0)
			{
				throw new NullReferenceException("MergeIntoRibbonTabItemName specified (" + m_MergeIntoRibbonTabItemName + ") cannot be found in RibbonControl.Items collection");
			}
			base.RibbonTabItem = ribbon.Items[m_MergeIntoRibbonTabItemName] as RibbonTabItem;
			return base.RibbonTabItem;
		}
		string text = m_RibbonTabText;
		if (text.Length == 0)
		{
			text = ((Control)this).Name;
		}
		string name = ((((Control)this).Parent != null) ? (((Control)this).Parent.Name + ".") : "") + ((Control)this).Name;
		base.RibbonTabItem = ribbon.CreateRibbonTab(text, name, m_MergeRibbonTabItemIndex);
		if (!string.IsNullOrEmpty(_RibbonTabKeyTips))
		{
			base.RibbonTabItem.KeyTips = _RibbonTabKeyTips;
		}
		base.RibbonTabItem.ColorTable = m_RibbonTabColorTable;
		m_RibbonTabItemCreated = true;
		if (m_MergeRibbonGroupName.Length > 0)
		{
			RibbonTabItemGroup ribbonTabItemGroup = ribbon.TabGroups[m_MergeRibbonGroupName];
			if (ribbonTabItemGroup != null)
			{
				int num = -1;
				for (int i = 0; i < ribbon.Items.Count; i++)
				{
					if (ribbon.Items[i] is RibbonTabItem && ((RibbonTabItem)ribbon.Items[i]).Group == ribbonTabItemGroup)
					{
						num = i;
					}
				}
				base.RibbonTabItem.Group = ribbonTabItemGroup;
				if (num >= 0)
				{
					ribbon.Items.Remove(base.RibbonTabItem);
					ribbon.Items.Insert(num + 1, base.RibbonTabItem);
				}
			}
		}
		return base.RibbonTabItem;
	}

	protected virtual void OnBeforeRibbonMerge(EventArgs e)
	{
		if (this.BeforeRibbonMerge != null)
		{
			this.BeforeRibbonMerge(this, e);
		}
	}

	protected virtual void OnAfterRibbonMerge(EventArgs e)
	{
		if (this.AfterRibbonMerge != null)
		{
			this.AfterRibbonMerge(this, e);
		}
	}

	protected virtual void OnBeforeRibbonUnmerge(EventArgs e)
	{
		if (this.BeforeRibbonUnmerge != null)
		{
			this.BeforeRibbonUnmerge(this, e);
		}
	}

	protected virtual void OnAfterRibbonUnmerge(EventArgs e)
	{
		if (this.AfterRibbonUnmerge != null)
		{
			this.AfterRibbonUnmerge(this, e);
		}
	}
}
