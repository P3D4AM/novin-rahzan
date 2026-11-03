using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

internal class _᮱ : RichTextBox, IScrollBarOverrideSupport
{
	private EventHandler ᖤ;

	private ScrollValueChangedHandler ሁ;

	private EventHandler ᖥ;

	bool IScrollBarOverrideSupport.ᖛ => ((Component)this).DesignMode;

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

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 522)
		{
			((RichTextBox)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(ScrollbarControl._1BAB(((Message)(ref m)).Msg)));
		}
		else if (((Message)(ref m)).Msg == 131)
		{
			((RichTextBox)this).WndProc(ref m);
			OnNonClientSizeChanged(EventArgs.Empty);
		}
		else if (((Message)(ref m)).Msg == 206 || ((Message)(ref m)).Msg == 8270)
		{
			((RichTextBox)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(eScrollBarScrollChange.Vertical | eScrollBarScrollChange.Horizontal));
		}
		else if (((Message)(ref m)).Msg == 3)
		{
			((RichTextBox)this).WndProc(ref m);
			OnControlMoved(EventArgs.Empty);
		}
		else
		{
			((RichTextBox)this).WndProc(ref m);
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
