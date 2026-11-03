using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(RibbonBarMergeContainer), "Ribbon.RibbonControl.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.RibbonBarMergeContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class RibbonBarMergeContainer : RibbonPanel
{
	private bool _244E = true;

	private string _244F = "";

	private string _2450 = "";

	private ArrayList _2451 = new ArrayList();

	private bool _2452;

	private bool _2453;

	private bool _1C30 = true;

	private string _2454 = "";

	private int _2455 = -1;

	private eRibbonTabColor _2456;

	private EventHandler _2457;

	private EventHandler _2458;

	private EventHandler _2459;

	private EventHandler _245A;

	private string __245B = "";

	public bool IsMerged => _2451.Count > 0;

	[DefaultValue(true)]
	[Category("Behaviour")]
	[Description("Indicates whether RibbonTab item the RibbonBar controls are added to when merged is automatically activated (selected) after controls are merged.")]
	[Browsable(true)]
	public bool AutoActivateTab
	{
		get
		{
			return _244E;
		}
		set
		{
			_244E = value;
		}
	}

	[Category("Behaviour")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether merge functionality is enabled for the container.")]
	public bool AllowMerge
	{
		get
		{
			return _1C30;
		}
		set
		{
			_1C30 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Description("Indicates the Ribbon Tab text for the tab that will be created when ribbon bar objects from this container are merged into the ribbon.")]
	[Category("Data")]
	public string RibbonTabText
	{
		get
		{
			return _244F;
		}
		set
		{
			_244F = value;
			if (IsMerged && base.RibbonTabItem != null)
			{
				base.RibbonTabItem.Text = value;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the Ribbon Tab.")]
	[Browsable(true)]
	public virtual string RibbonTabKeyTips
	{
		get
		{
			return __245B;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			__245B = value.ToUpper();
			if (IsMerged && base.RibbonTabItem != null)
			{
				base.RibbonTabItem.KeyTips = __245B;
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates predefined color for the ribbon tab that is created when ribbon bar controls are merged into the ribbon.")]
	[Browsable(true)]
	[DefaultValue(eRibbonTabColor.Default)]
	public eRibbonTabColor RibbonTabColorTable
	{
		get
		{
			return _2456;
		}
		set
		{
			_2456 = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the name of RibbonTabItem object that already exists on Ribbon control into which the RibbonBar controls are merged.")]
	[DefaultValue("")]
	[Category("Data")]
	public string MergeIntoRibbonTabItemName
	{
		get
		{
			return _2450;
		}
		set
		{
			_2450 = value;
		}
	}

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates the name of the RibbonTabItemGroup the new Ribbon Tab Item that is created will be added to.")]
	[Browsable(true)]
	public string MergeRibbonGroupName
	{
		get
		{
			return _2454;
		}
		set
		{
			_2454 = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(-1)]
	[Description("Indicates the insertion index for the ribbon tab item that is created when ribbon bars are merged into the ribbon control.")]
	public int MergeRibbonTabItemIndex
	{
		get
		{
			return _2455;
		}
		set
		{
			_2455 = value;
		}
	}

	public event EventHandler BeforeRibbonMerge
	{
		add
		{
			EventHandler eventHandler = _2457;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2457, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2457;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2457, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler AfterRibbonMerge
	{
		add
		{
			EventHandler eventHandler = _2458;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2458, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2458;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2458, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler BeforeRibbonUnmerge
	{
		add
		{
			EventHandler eventHandler = _2459;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2459, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2459;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2459, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler AfterRibbonUnmerge
	{
		add
		{
			EventHandler eventHandler = _245A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _245A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _245A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _245A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public virtual void RemoveMergedRibbonBars(RibbonControl ribbon)
	{
		OnBeforeRibbonUnmerge(new EventArgs());
		Control val = null;
		foreach (RibbonBar item in _2451)
		{
			if (((Control)item).Parent != null)
			{
				if (val == null)
				{
					val = ((Control)item).Parent;
					val.SuspendLayout();
				}
				((Control)item).Parent.Controls.Remove((Control)(object)item);
			}
			((Control)this).Controls.Add((Control)(object)item);
			((Control)item).Enabled = false;
		}
		_2451.Clear();
		if (val != null)
		{
			val.ResumeLayout();
		}
		if (_2452)
		{
			base.RibbonTabItem.Parent.SubItems.Remove(base.RibbonTabItem);
			if (base.RibbonTabItem.Panel != null && ((Control)base.RibbonTabItem.Panel).Parent != null)
			{
				((Control)base.RibbonTabItem.Panel).Parent.Controls.Remove((Control)(object)base.RibbonTabItem.Panel);
			}
			((Component)(object)base.RibbonTabItem.Panel).Dispose();
			base.RibbonTabItem.Panel = null;
			_2452 = false;
		}
		base.RibbonTabItem = null;
		if (base.Visible != _2453)
		{
			base.Visible = _2453;
		}
		OnAfterRibbonUnmerge(new EventArgs());
	}

	public virtual void MergeRibbonBars(RibbonControl ribbon)
	{
		OnBeforeRibbonMerge(new EventArgs());
		_2453 = base.Visible;
		if (base.Visible)
		{
			base.Visible = false;
		}
		RibbonTabItem ribbonTabItem = _245C(ribbon);
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		int num = ((Control)ribbonTabItem.Panel).Width + 1;
		((Control)ribbonTabItem.Panel).SuspendLayout();
		Control[] array2 = array;
		foreach (Control val in array2)
		{
			if (val is RibbonBar)
			{
				((Control)this).Controls.Remove(val);
				if (_2450.Length > 0 && !ribbonTabItem.Panel.DefaultLayout)
				{
					val.Left = num;
					num += val.Width;
				}
				((Control)ribbonTabItem.Panel).Controls.Add(val);
				val.Enabled = true;
				_2451.Add(val);
			}
		}
		((Control)ribbonTabItem.Panel).ResumeLayout();
		OnAfterRibbonMerge(new EventArgs());
	}

	private RibbonTabItem _245C(RibbonControl ᤓ)
	{
		if (_2450 != "")
		{
			if (ᤓ.Items.IndexOf(_2450) < 0)
			{
				throw new NullReferenceException("MergeIntoRibbonTabItemName specified (" + _2450 + ") cannot be found in RibbonControl.Items collection");
			}
			base.RibbonTabItem = ᤓ.Items[_2450] as RibbonTabItem;
			return base.RibbonTabItem;
		}
		string text = _244F;
		if (text.Length == 0)
		{
			text = ((Control)this).Name;
		}
		string name = ((((Control)this).Parent != null) ? (((Control)this).Parent.Name + ".") : "") + ((Control)this).Name;
		base.RibbonTabItem = ᤓ.CreateRibbonTab(text, name, _2455);
		if (!string.IsNullOrEmpty(__245B))
		{
			base.RibbonTabItem.KeyTips = __245B;
		}
		base.RibbonTabItem.ColorTable = _2456;
		_2452 = true;
		if (_2454.Length > 0)
		{
			RibbonTabItemGroup ribbonTabItemGroup = ᤓ.TabGroups[_2454];
			if (ribbonTabItemGroup != null)
			{
				int num = -1;
				for (int i = 0; i < ᤓ.Items.Count; i++)
				{
					if (ᤓ.Items[i] is RibbonTabItem && ((RibbonTabItem)ᤓ.Items[i]).Group == ribbonTabItemGroup)
					{
						num = i;
					}
				}
				base.RibbonTabItem.Group = ribbonTabItemGroup;
				if (num >= 0)
				{
					ᤓ.Items.Remove(base.RibbonTabItem);
					ᤓ.Items.Insert(num + 1, base.RibbonTabItem);
				}
			}
		}
		return base.RibbonTabItem;
	}

	protected virtual void OnBeforeRibbonMerge(EventArgs e)
	{
		if (_2457 != null)
		{
			_2457(this, e);
		}
	}

	protected virtual void OnAfterRibbonMerge(EventArgs e)
	{
		if (_2458 != null)
		{
			_2458(this, e);
		}
	}

	protected virtual void OnBeforeRibbonUnmerge(EventArgs e)
	{
		if (_2459 != null)
		{
			_2459(this, e);
		}
	}

	protected virtual void OnAfterRibbonUnmerge(EventArgs e)
	{
		if (_245A != null)
		{
			_245A(this, e);
		}
	}
}
