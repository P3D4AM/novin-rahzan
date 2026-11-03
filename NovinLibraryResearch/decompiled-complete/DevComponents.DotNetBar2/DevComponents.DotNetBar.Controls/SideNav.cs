using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(SideNav), "Controls.SideNav.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.SideNavDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class SideNav : ContainerControl
{
	private SideNavStrip __20C7;

	private Bar __20C8;

	private LabelItem __20C9;

	private ButtonItem __2007;

	private ButtonItem __20CA;

	private ButtonItem __20CB;

	private Control __20CC;

	private bool __20CD = true;

	private bool __20CE = true;

	private bool __20CF = true;

	private bool _வ;

	private int __20D0;

	private bool __20D1 = true;

	private int __20D2;

	private SideNavItem __20D3;

	private bool __20D4 = true;

	private EventHandler _20D5;

	private bool __20D6;

	private int __20D7 = 250;

	private CancelSourceEventHandler _20D8;

	private CancelSourceEventHandler _20D9;

	private CancelSourceEventHandler _20DA;

	private CancelSourceEventHandler _20DB;

	private Point __20DC = Point.Empty;

	private EventHandler _20DD;

	[Description("Indicates whether splitter that is located on right hand side of open control is visible and enabled.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool EnableSplitter
	{
		get
		{
			return __20CD;
		}
		set
		{
			if (__20CD != value)
			{
				bool oldValue = __20CD;
				__20CD = value;
				OnEnableSplitterChanged(value, oldValue);
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether button which folds/closes the control is visible.")]
	[Category("Appearance")]
	public bool EnableClose
	{
		get
		{
			return __20CE;
		}
		set
		{
			if (__20CE != value)
			{
				bool oldValue = __20CE;
				__20CE = value;
				OnEnableCloseChanged(value, oldValue);
			}
		}
	}

	[Description("Indicates whether buttons which maximize and restore the control are visible.")]
	[DefaultValue(true)]
	[Category("Apeparance")]
	public bool EnableMaximize
	{
		get
		{
			return __20CF;
		}
		set
		{
			if (__20CF != value)
			{
				bool oldValue = __20CF;
				__20CF = value;
				OnEnableMaximizeChanged(value, oldValue);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => __20C7.Items;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsClosed
	{
		get
		{
			return !__20D1;
		}
		set
		{
			if (value)
			{
				if (__20D1)
				{
					Close(eEventSource.Code);
				}
			}
			else if (!__20D1)
			{
				SideNavItem sideNavItem = _20C6 ?? __20D3;
				if (sideNavItem != null)
				{
					Open(sideNavItem, eEventSource.Code);
				}
			}
		}
	}

	private SideNavItem _20C6
	{
		get
		{
			for (int i = 0; i < __20C7.Items.Count; i++)
			{
				if (__20C7.Items[i] is SideNavItem && __20C7.Items[i].Visible)
				{
					return (SideNavItem)__20C7.Items[i];
				}
			}
			return null;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether side menu is expanded, i.e. shows both image and text. When menu is collapsed only image is shown.")]
	[DefaultValue(true)]
	public bool IsMenuExpanded
	{
		get
		{
			return __20D4;
		}
		set
		{
			if (__20D4 != value)
			{
				bool oldValue = __20D4;
				__20D4 = value;
				OnIsMenuExpandedChanged(value, oldValue);
			}
		}
	}

	[Description("Indicates the animation time in milliseconds for operations that perform visual animation of transition. Set to zero to disable animation.")]
	[Category("Behavior")]
	[DefaultValue(250)]
	public int AnimationTime
	{
		get
		{
			return __20D7;
		}
		set
		{
			if (value != __20D7)
			{
				int oldValue = __20D7;
				__20D7 = value;
				OnAnimationTimeChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SideNavItem SelectedItem
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is SideNavItem && ((SideNavItem)item).Checked)
				{
					return (SideNavItem)item;
				}
			}
			return null;
		}
		set
		{
			if (value != null && value.Panel != null)
			{
				value.Checked = true;
			}
		}
	}

	[Browsable(false)]
	public SideNavStrip SideNavStrip => __20C7;

	[Description("Occurs when IsMenuExpanded property has changed.")]
	public event EventHandler IsMenuExpandedChanged
	{
		add
		{
			EventHandler eventHandler = _20D5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20D5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _20D5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20D5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before the control is maximized and allows you to cancel that.")]
	public event CancelSourceEventHandler BeforeMaximize
	{
		add
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20D8;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Combine(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20D8, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
		remove
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20D8;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Remove(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20D8, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
	}

	[Description("Occurs before the control is restored and allows you to cancel that.")]
	public event CancelSourceEventHandler BeforeRestore
	{
		add
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20D9;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Combine(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20D9, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
		remove
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20D9;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Remove(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20D9, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
	}

	[Description("Occurs before the control is opened and allows you to cancel that.")]
	public event CancelSourceEventHandler BeforeOpen
	{
		add
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20DA;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Combine(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20DA, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
		remove
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20DA;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Remove(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20DA, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
	}

	[Description("Occurs before the control is closed and allows you to cancel that.")]
	public event CancelSourceEventHandler BeforeClose
	{
		add
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20DB;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Combine(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20DB, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
		remove
		{
			CancelSourceEventHandler cancelSourceEventHandler = _20DB;
			CancelSourceEventHandler cancelSourceEventHandler2;
			do
			{
				cancelSourceEventHandler2 = cancelSourceEventHandler;
				CancelSourceEventHandler value2 = (CancelSourceEventHandler)Delegate.Remove(cancelSourceEventHandler2, value);
				cancelSourceEventHandler = Interlocked.CompareExchange(ref _20DB, value2, cancelSourceEventHandler2);
			}
			while ((object)cancelSourceEventHandler != cancelSourceEventHandler2);
		}
	}

	[Description("Occurs when SelectedItem changes.")]
	public event EventHandler SelectedItemChanged
	{
		add
		{
			EventHandler eventHandler = _20DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _20DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SideNav()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		((Control)this).Padding = new Padding(1);
		__20CC = new Control();
		__20CC.Width = 4;
		__20CC.Name = "splitter";
		__20CC.Cursor = Cursors.VSplit;
		__20CC.Dock = (DockStyle)4;
		__20CC.MouseMove += new MouseEventHandler(_109A);
		__20CC.MouseDown += new MouseEventHandler(_20E6);
		((Control)this).Controls.Add(__20CC);
		Bar bar = new Bar();
		bar.Name = "titleBar";
		bar.AntiAlias = true;
		bar.Name = "titleBar";
		bar.PaddingBottom = 7;
		bar.PaddingTop = 5;
		bar.PaddingLeft = 6;
		bar.RoundCorners = false;
		((Control)bar).Dock = (DockStyle)1;
		bar.Stretch = true;
		bar.Style = eDotNetBarStyle.StyleManagerControlled;
		bar.TabIndex = 0;
		bar.TabStop = false;
		bar.BarType = eBarType.StatusBar;
		bar.ItemsContainer._0DD8 = false;
		LabelItem labelItem = new LabelItem
		{
			Name = "titleLabel",
			Text = "Title",
			FontBold = true
		};
		__20C9 = labelItem;
		bar.Items.Add(labelItem);
		__20C8 = bar;
		__2007 = new ButtonItem("openCloseButton");
		__2007.Symbol = "\uf0d9";
		__2007.ImagePaddingHorizontal = 18;
		__2007.ItemAlignment = eItemAlignment.Far;
		__2007.SymbolSize = 12f;
		__2007.Click += _20DE;
		bar.Items.Add(__2007);
		__20CA = new ButtonItem("maximizeButton");
		__20CA.Symbol = "\uf090";
		__20CA.ItemAlignment = eItemAlignment.Far;
		__20CA.SymbolSize = 12f;
		__20CA.Click += _20DF;
		bar.Items.Add(__20CA);
		__20CB = new ButtonItem("restoreButton");
		__20CB.Symbol = "\uf100";
		__20CB.ItemAlignment = eItemAlignment.Far;
		__20CB.SymbolSize = 12f;
		__20CB.Click += _20E1;
		__20CB.Visible = false;
		bar.Items.Add(__20CB);
		((Control)this).Controls.Add((Control)(object)bar);
		__20C7 = new SideNavStrip();
		((Control)__20C7).Dock = (DockStyle)3;
		((Control)__20C7).AutoSize = true;
		__20C7._09B4 = eOrientation.Horizontal;
		__20C7.ButtonCheckedChanged += _20E2;
		((Control)this).Controls.Add((Control)(object)__20C7);
		((Control)__20C7).Width = 48;
		StyleManager.Register((Control)(object)this);
		UpdateColors();
	}

	protected override void Dispose(bool disposing)
	{
		if (_வ && disposing && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Resize -= _0A76;
		}
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateColors();
	}

	public void UpdateColors()
	{
		SideNavColorTable sideNavColorTable = ጧ();
		((Control)__20C8).BackColor = sideNavColorTable.TitleBackColor;
		__20C8._0AFA = sideNavColorTable.TitleBorderColors;
		((Control)this).Invalidate(true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor);
		SideNavColorTable sideNavColorTable = ጧ();
		DisplayHelp.DrawRoundedRectangle(graphics, ((Control)this).ClientRectangle, sideNavColorTable.BorderColors, 0);
	}

	private SideNavColorTable ጧ()
	{
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.SideNav;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		__20C7.SetDesignMode(mode: true);
	}

	protected virtual void OnEnableSplitterChanged(bool newValue, bool oldValue)
	{
		__20CC.Visible = newValue;
	}

	protected virtual void OnEnableCloseChanged(bool newValue, bool oldValue)
	{
		__2007.Visible = newValue;
		__20C8.RecalcLayout();
	}

	protected virtual void OnEnableMaximizeChanged(bool newValue, bool oldValue)
	{
		if (!newValue)
		{
			__20CA.Visible = false;
			__20CB.Visible = false;
		}
		else if (_வ)
		{
			__20CB.Visible = true;
		}
		else
		{
			__20CA.Visible = true;
		}
		__20C8.RecalcLayout();
	}

	private void _20DE(object _٠, EventArgs ؿ)
	{
		Close(eEventSource.Mouse);
	}

	private void _20DF(object _٠, EventArgs ؿ)
	{
		Maximize(eEventSource.Mouse);
	}

	public void Maximize(eEventSource source)
	{
		if (!_வ && ((Control)this).Parent != null && ((Control)this).Bounds.Right < ((Control)this).Parent.ClientRectangle.Right)
		{
			CancelSourceEventArgs cancelSourceEventArgs = new CancelSourceEventArgs(source);
			OnBeforeMaximize(cancelSourceEventArgs);
			if (!cancelSourceEventArgs.Cancel)
			{
				_வ = true;
				int width = _20E0();
				__20D0 = ((Control)this).Width;
				BarFunctions.AnimateControl((Control)(object)this, show: true, __20D7, ((Control)this).Bounds, new Rectangle(((Control)this).Location, new Size(width, ((Control)this).Height)));
				__20CB.Visible = true;
				__20CA.Visible = false;
				__20C8.RecalcLayout();
				__20CC.Visible = false;
				((Control)this).Invalidate();
				((Control)this).Parent.Resize += _0A76;
			}
		}
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		if (((Control)this).Parent.Width > 0)
		{
			((Control)this).Width = _20E0();
		}
	}

	private int _20E0()
	{
		int num = ((Control)this).Width + ((Control)this).Parent.ClientRectangle.Right - ((Control)this).Bounds.Right;
		if (((Control)this).Parent is MetroAppForm)
		{
			MetroAppForm metroAppForm = (MetroAppForm)(object)((Control)this).Parent;
			num = ((!(metroAppForm.BorderThickness.Right > 0.0)) ? (num - 3) : (num - (int)metroAppForm.BorderThickness.Right));
		}
		return num;
	}

	private void _20E1(object _٠, EventArgs ؿ)
	{
		Restore(eEventSource.Mouse);
	}

	public void Restore(eEventSource source)
	{
		if (!_வ || __20D0 == 0)
		{
			return;
		}
		_வ = false;
		CancelSourceEventArgs cancelSourceEventArgs = new CancelSourceEventArgs(source);
		OnBeforeRestore(cancelSourceEventArgs);
		if (!cancelSourceEventArgs.Cancel)
		{
			BarFunctions.AnimateControl((Control)(object)this, show: true, __20D7, ((Control)this).Bounds, new Rectangle(((Control)this).Location, new Size(__20D0, ((Control)this).Height)));
			__20D0 = 0;
			__20CB.Visible = false;
			__20CA.Visible = true;
			__20C8.RecalcLayout();
			__20CC.Visible = __20CD;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Resize -= _0A76;
			}
		}
	}

	private void _20E2(object _٠, EventArgs ؿ)
	{
		if (_٠ is SideNavItem && ((SideNavItem)_٠).Checked)
		{
			OnSelectedItemChanged(EventArgs.Empty);
			if (!__20D1)
			{
				Open((SideNavItem)_٠, eEventSource.Code);
			}
		}
	}

	public void Open(SideNavItem item, eEventSource source)
	{
		if (item == null)
		{
			throw new ArgumentException("item must be set to valid item to select");
		}
		if (__20D1)
		{
			return;
		}
		CancelSourceEventArgs cancelSourceEventArgs = new CancelSourceEventArgs(source, item);
		OnBeforeOpen(cancelSourceEventArgs);
		if (!cancelSourceEventArgs.Cancel)
		{
			if (!item.Checked)
			{
				item.Checked = true;
			}
			BarFunctions.AnimateControl((Control)(object)this, show: true, __20D7, ((Control)this).Bounds, new Rectangle(((Control)this).Location, new Size(__20D2, ((Control)this).Height)));
			__20D1 = true;
			__20CC.Visible = __20CD;
			((Control)this).Invalidate();
		}
	}

	public void Close(eEventSource source)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (!__20D1)
		{
			return;
		}
		CancelSourceEventArgs cancelSourceEventArgs = new CancelSourceEventArgs(source);
		OnBeforeClose(cancelSourceEventArgs);
		if (!cancelSourceEventArgs.Cancel)
		{
			__20D2 = ((Control)this).Width;
			int width = ((Control)__20C7).Width;
			Padding padding = ((Control)this).Padding;
			int width2 = width + ((Padding)(ref padding)).Horizontal;
			__20CC.Visible = false;
			BarFunctions.AnimateControl((Control)(object)this, show: true, __20D7, ((Control)this).Bounds, new Rectangle(((Control)this).Location, new Size(width2, ((Control)this).Height)));
			__20D1 = false;
			if (__20C7.SelectedItem != null)
			{
				__20D3 = __20C7.SelectedItem;
				__20C7.SelectedItem.Checked = false;
			}
			else
			{
				__20D3 = null;
			}
			((Control)this).Invalidate();
		}
	}

	protected virtual void OnIsMenuExpandedChanged(bool newValue, bool oldValue)
	{
		_20E3(newValue);
		OnIsMenuExpandedChanged(EventArgs.Empty);
	}

	protected virtual void OnIsMenuExpandedChanged(EventArgs e)
	{
		_20D5?.Invoke(this, e);
	}

	private void _20E3(bool _20E4)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if (!((Control)this).IsHandleCreated)
		{
			__20D6 = true;
			return;
		}
		for (int i = 0; i < __20C7.Items.Count; i++)
		{
			if (__20C7.Items[i] is ButtonItem buttonItem)
			{
				buttonItem.ButtonStyle = (_20E4 ? eButtonStyle.ImageAndText : eButtonStyle.Default);
			}
		}
		__20C7.RecalcLayout();
		if (!__20D1)
		{
			int width = ((Control)__20C7).Width;
			Padding padding = ((Control)this).Padding;
			((Control)this).Width = width + ((Padding)(ref padding)).Horizontal;
		}
	}

	protected virtual void OnAnimationTimeChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnBeforeMaximize(CancelSourceEventArgs e)
	{
		_20D8?.Invoke(this, e);
	}

	protected virtual void OnBeforeRestore(CancelSourceEventArgs e)
	{
		_20D9?.Invoke(this, e);
	}

	protected virtual void OnBeforeOpen(CancelSourceEventArgs e)
	{
		_20DA?.Invoke(this, e);
	}

	protected virtual void OnBeforeClose(CancelSourceEventArgs e)
	{
		_20DB?.Invoke(this, e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_20E5();
		UpdateColors();
		if (__20D6)
		{
			_20E3(__20D4);
			__20D6 = false;
		}
		((Control)this).OnHandleCreated(e);
	}

	internal void _20E5()
	{
		SideNavItem selectedItem = __20C7.SelectedItem;
		if (selectedItem != null)
		{
			if (!string.IsNullOrEmpty(selectedItem.Title))
			{
				__20C9.Text = selectedItem.Title;
			}
			else
			{
				__20C9.Text = selectedItem.Text;
			}
		}
	}

	private void _20E6(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button == 1048576)
		{
			__20DC = ؿ.Location;
		}
	}

	private void _109A(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button == 1048576)
		{
			int num = _20E0();
			int num2 = ((Control)__20C7).Width + 64;
			int num3 = ((Control)this).Width + (ؿ.X - __20DC.X);
			if (num3 > num)
			{
				num3 = num;
			}
			else if (num3 < num2)
			{
				num3 = num2;
			}
			((Control)this).Width = num3;
		}
	}

	protected virtual void OnSelectedItemChanged(EventArgs e)
	{
		_20DD?.Invoke(this, e);
	}
}
