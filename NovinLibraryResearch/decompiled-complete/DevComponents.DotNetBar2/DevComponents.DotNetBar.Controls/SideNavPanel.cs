using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class SideNavPanel : Panel, IScrollBarOverrideSupport
{
	private ᗔ _ᖟ;

	private Color __0CD2 = Color.Empty;

	private EventHandler ᖤ;

	private ScrollValueChangedHandler ሁ;

	private EventHandler ᖥ;

	bool IScrollBarOverrideSupport.ᖛ => ((Component)this).DesignMode;

	[Browsable(false)]
	public event EventHandler NonClientSizeChanged
	{
		add
		{
			EventHandler eventHandler = ᖤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᖤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ScrollValueChangedHandler ScrollBarValueChanged
	{
		add
		{
			ScrollValueChangedHandler scrollValueChangedHandler = ሁ;
			ScrollValueChangedHandler scrollValueChangedHandler2;
			do
			{
				scrollValueChangedHandler2 = scrollValueChangedHandler;
				ScrollValueChangedHandler value2 = (ScrollValueChangedHandler)Delegate.Combine(scrollValueChangedHandler2, value);
				scrollValueChangedHandler = Interlocked.CompareExchange(ref ሁ, value2, scrollValueChangedHandler2);
			}
			while ((object)scrollValueChangedHandler != scrollValueChangedHandler2);
		}
		remove
		{
			ScrollValueChangedHandler scrollValueChangedHandler = ሁ;
			ScrollValueChangedHandler scrollValueChangedHandler2;
			do
			{
				scrollValueChangedHandler2 = scrollValueChangedHandler;
				ScrollValueChangedHandler value2 = (ScrollValueChangedHandler)Delegate.Remove(scrollValueChangedHandler2, value);
				scrollValueChangedHandler = Interlocked.CompareExchange(ref ሁ, value2, scrollValueChangedHandler2);
			}
			while ((object)scrollValueChangedHandler != scrollValueChangedHandler2);
		}
	}

	[Browsable(false)]
	public event EventHandler ControlMoved
	{
		add
		{
			EventHandler eventHandler = ᖥ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖥ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᖥ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖥ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SideNavPanel()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		_ᖟ = new ᗔ((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		_ᖟ.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		Color color = _1DD4();
		if (!color.IsEmpty)
		{
			SolidBrush val = new SolidBrush(color);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		PropertyInfo property = typeof(Control).GetProperty("RawBackColor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
		if ((object)property != null)
		{
			__0CD2 = (Color)property.GetValue(this, null);
		}
		((Control)this).OnBackColorChanged(e);
	}

	private Color _1DD4()
	{
		if (!__0CD2.IsEmpty)
		{
			return __0CD2;
		}
		return ጧ().PanelBackColor;
	}

	private SideNavColorTable ጧ()
	{
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.SideNav;
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 522)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(ScrollbarControl._1BAB(((Message)(ref m)).Msg)));
		}
		else if (((Message)(ref m)).Msg == 131)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnNonClientSizeChanged(EventArgs.Empty);
		}
		else if (((Message)(ref m)).Msg == 206 || ((Message)(ref m)).Msg == 8270)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(eScrollBarScrollChange.Vertical | eScrollBarScrollChange.Horizontal));
		}
		else if (((Message)(ref m)).Msg == 3)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnControlMoved(EventArgs.Empty);
		}
		else
		{
			((ScrollableControl)this).WndProc(ref m);
		}
	}

	protected virtual void OnNonClientSizeChanged(EventArgs e)
	{
		ᖤ?.Invoke(this, e);
	}

	protected virtual void OnScrollBarValueChanged(ScrollValueChangedEventArgs e)
	{
		ሁ?.Invoke(this, e);
	}

	protected virtual void OnControlMoved(EventArgs e)
	{
		ᖥ?.Invoke(this, e);
	}
}
