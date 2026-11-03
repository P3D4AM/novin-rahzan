using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.Editors;

public class VisualItem
{
	private EventHandler ᡈ;

	private EventHandler ᡉ;

	private EventHandler ᡊ;

	private EventHandler દ;

	private MouseEventHandler ᡋ;

	private MouseEventHandler ग़;

	private MouseEventHandler ફ;

	private EventHandler ય;

	private bool _ᡌ;

	private bool _Ԋ = true;

	private bool _ᚉ = true;

	private bool __0AD7 = true;

	private VisualGroup _ᗚ;

	private Size _ᡍ = Size.Empty;

	private Rectangle _ᡎ = Rectangle.Empty;

	private Point _ᡏ;

	private bool _ᡐ;

	private bool _ᡑ;

	private bool _ᡒ;

	private eItemAlignment _ᡓ;

	private _1885 _ᡔ;

	private Size _ᡕ;

	public bool Focusable
	{
		get
		{
			return _ᡌ;
		}
		set
		{
			if (_ᡌ != value)
			{
				_ᡌ = value;
				OnFocusableChanged();
			}
		}
	}

	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return _Ԋ;
		}
		set
		{
			if (_Ԋ != value)
			{
				_Ԋ = value;
				OnRenderInvalid();
			}
		}
	}

	public bool Visible
	{
		get
		{
			return _ᚉ;
		}
		set
		{
			if (_ᚉ != value)
			{
				_ᚉ = value;
				OnVisibleChanged();
			}
		}
	}

	public virtual bool IsRendered
	{
		get
		{
			return __0AD7;
		}
		set
		{
			__0AD7 = value;
		}
	}

	public VisualGroup Parent
	{
		get
		{
			return _ᗚ;
		}
		internal set
		{
			if (_ᗚ != value)
			{
				_ᗚ = value;
				OnParentChanged();
			}
		}
	}

	public Size Size
	{
		get
		{
			return _ᡍ;
		}
		internal set
		{
			_ᡍ = value;
		}
	}

	public Rectangle RenderBounds
	{
		get
		{
			return _ᡎ;
		}
		internal set
		{
			_ᡎ = value;
		}
	}

	public Point Location
	{
		get
		{
			return _ᡏ;
		}
		internal set
		{
			_ᡏ = value;
		}
	}

	public bool IsRightToLeft
	{
		get
		{
			return _ᡐ;
		}
		set
		{
			if (_ᡐ != value)
			{
				_ᡐ = value;
				OnIsRightToLeftChanged();
			}
		}
	}

	public virtual bool IsLayoutValid => _ᡑ;

	public bool IsFocused => _ᡒ;

	[DefaultValue(eItemAlignment.Left)]
	public eItemAlignment Alignment
	{
		get
		{
			return _ᡓ;
		}
		set
		{
			if (_ᡓ != value)
			{
				_ᡓ = value;
				InvalidateArrange();
			}
		}
	}

	internal _1885 ᡇ
	{
		get
		{
			return _ᡔ;
		}
		set
		{
			_ᡔ = value;
		}
	}

	public Size CalculatedSize
	{
		get
		{
			return _ᡕ;
		}
		internal set
		{
			_ᡕ = value;
		}
	}

	public event EventHandler ArrangeInvalid
	{
		add
		{
			EventHandler eventHandler = ᡈ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡈ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᡈ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡈ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler RenderInvalid
	{
		add
		{
			EventHandler eventHandler = ᡉ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡉ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᡉ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡉ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ResetMouseHover
	{
		add
		{
			EventHandler eventHandler = ᡊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᡊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᡊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
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

	public event MouseEventHandler MouseClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᡋ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᡋ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᡋ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᡋ, value2, val2);
			}
			while (val != val2);
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

	internal virtual void ᡖ()
	{
		if (GetIsEnabled())
		{
			OnMouseEnter();
		}
	}

	protected virtual void OnMouseEnter()
	{
	}

	internal virtual void ᡗ()
	{
		if (GetIsEnabled())
		{
			OnMouseLeave();
		}
	}

	protected virtual void OnMouseLeave()
	{
	}

	internal virtual void ᡘ(MouseEventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseMove(ؿ);
		}
	}

	protected virtual void OnMouseMove(MouseEventArgs e)
	{
	}

	internal virtual void ᡙ(MouseEventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseWheel(ؿ);
		}
	}

	protected virtual void OnMouseWheel(MouseEventArgs e)
	{
	}

	internal virtual void ᡚ(MouseEventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseDown(ؿ);
		}
	}

	protected virtual void OnMouseDown(MouseEventArgs e)
	{
		if (ग़ != null)
		{
			ग़.Invoke((object)this, e);
		}
	}

	internal virtual void ᡛ(MouseEventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseUp(ؿ);
		}
	}

	protected virtual void OnMouseUp(MouseEventArgs e)
	{
		if (ફ != null)
		{
			ફ.Invoke((object)this, e);
		}
	}

	internal virtual void ᡜ(EventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseHover(ؿ);
		}
	}

	protected virtual void OnMouseHover(EventArgs e)
	{
		if (ય != null)
		{
			ય(this, e);
		}
	}

	internal virtual void ᡝ()
	{
		if (GetIsEnabled())
		{
			OnClick(EventArgs.Empty);
		}
	}

	protected virtual void OnClick(EventArgs e)
	{
		if (દ != null)
		{
			દ(this, e);
		}
	}

	internal virtual void ᡞ(MouseEventArgs ؿ)
	{
		if (GetIsEnabled())
		{
			OnMouseClick(ؿ);
		}
	}

	protected virtual void OnMouseClick(MouseEventArgs e)
	{
		if (ᡋ != null)
		{
			ᡋ.Invoke((object)this, e);
		}
	}

	internal virtual void ᡟ(KeyEventArgs ؿ)
	{
		OnKeyDown(ؿ);
	}

	protected virtual void OnKeyDown(KeyEventArgs e)
	{
	}

	internal virtual void ᡠ(KeyEventArgs ؿ)
	{
		OnKeyUp(ؿ);
	}

	protected virtual void OnKeyUp(KeyEventArgs e)
	{
	}

	internal virtual void ᡡ(KeyPressEventArgs ؿ)
	{
		OnKeyPress(ؿ);
	}

	protected virtual void OnKeyPress(KeyPressEventArgs e)
	{
	}

	internal virtual bool ᘭ(ref Message ᘮ, Keys ᘯ)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return OnCmdKey(ref ᘮ, ᘯ);
	}

	protected virtual bool OnCmdKey(ref Message msg, Keys keyData)
	{
		return false;
	}

	internal virtual void ᡢ()
	{
		OnGotFocus();
	}

	protected virtual void OnGotFocus()
	{
		_ᡒ = true;
		OnFocusChanged();
	}

	internal virtual void ᡣ()
	{
		OnLostFocus();
	}

	protected virtual void OnLostFocus()
	{
		_ᡒ = false;
		OnFocusChanged();
	}

	protected virtual void OnFocusChanged()
	{
	}

	internal virtual void ᡤ(PaintInfo _065C)
	{
		if (IsRendered)
		{
			OnPaint(_065C);
		}
	}

	protected virtual void OnPaint(PaintInfo p)
	{
	}

	protected virtual void OnFocusableChanged()
	{
	}

	protected virtual bool GetIsEnabled()
	{
		if (!_Ԋ)
		{
			return _Ԋ;
		}
		for (VisualItem parent = Parent; parent != null; parent = parent.Parent)
		{
			if (!parent.Enabled)
			{
				return false;
			}
		}
		return _Ԋ;
	}

	protected virtual bool GetIsEnabled(PaintInfo p)
	{
		if (p.ParentEnabled)
		{
			return _Ԋ;
		}
		return false;
	}

	protected virtual void OnVisibleChanged()
	{
		if (_ᗚ != null)
		{
			_ᗚ.ᡱ(this);
		}
	}

	public virtual void InvalidateArrange()
	{
		_ᡎ = Rectangle.Empty;
		_ᡏ = Point.Empty;
		_ᡑ = false;
		if (_ᗚ != null)
		{
			_ᗚ.InvalidateArrange();
		}
		OnArrangeInvalid();
	}

	public virtual void InvalidateRender()
	{
		if (_ᗚ != null)
		{
			_ᗚ.InvalidateRender();
		}
		OnRenderInvalid();
	}

	protected virtual void OnRenderInvalid()
	{
		if (ᡉ != null)
		{
			ᡉ(this, new EventArgs());
		}
	}

	protected virtual void OnArrangeInvalid()
	{
		if (ᡈ != null)
		{
			ᡈ(this, new EventArgs());
		}
	}

	protected virtual void OnParentChanged()
	{
	}

	protected virtual void OnIsRightToLeftChanged()
	{
		InvalidateArrange();
	}

	public virtual void PerformLayout(PaintInfo pi)
	{
		_ᡑ = true;
	}

	public virtual void ResetHover()
	{
		if (_ᗚ != null)
		{
			_ᗚ.ResetHover();
		}
		OnResetMouseHover();
	}

	protected virtual void OnResetMouseHover()
	{
		if (ᡊ != null)
		{
			ᡊ(this, new EventArgs());
		}
	}
}
