using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualItem
{
	[CompilerGenerated]
	private MouseEventHandler m_MouseClick;

	[CompilerGenerated]
	private MouseEventHandler m_MouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_MouseUp;

	private bool _Focusable;

	private bool _Enabled = true;

	private bool _Visible = true;

	private bool _IsRendered = true;

	private VisualGroup _Parent;

	private Size _ItemSize = Size.Empty;

	private Rectangle _RenderBounds = Rectangle.Empty;

	private Point _Location;

	private bool _IsRightToLeft;

	private bool _IsLayoutValid;

	private bool _IsFocused;

	private eItemAlignment _Alignment;

	private eSystemItemType _ItemType;

	private Size _CalculatedSize;

	public bool Focusable
	{
		get
		{
			return _Focusable;
		}
		set
		{
			if (_Focusable != value)
			{
				_Focusable = value;
				OnFocusableChanged();
			}
		}
	}

	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return _Enabled;
		}
		set
		{
			if (_Enabled != value)
			{
				_Enabled = value;
				OnRenderInvalid();
			}
		}
	}

	public bool Visible
	{
		get
		{
			return _Visible;
		}
		set
		{
			if (_Visible != value)
			{
				_Visible = value;
				OnVisibleChanged();
			}
		}
	}

	public virtual bool IsRendered
	{
		get
		{
			return _IsRendered;
		}
		set
		{
			_IsRendered = value;
		}
	}

	public VisualGroup Parent
	{
		get
		{
			return _Parent;
		}
		internal set
		{
			if (_Parent != value)
			{
				_Parent = value;
				OnParentChanged();
			}
		}
	}

	public Size Size
	{
		get
		{
			return _ItemSize;
		}
		internal set
		{
			_ItemSize = value;
		}
	}

	public Rectangle RenderBounds
	{
		get
		{
			return _RenderBounds;
		}
		internal set
		{
			_RenderBounds = value;
		}
	}

	public Point Location
	{
		get
		{
			return _Location;
		}
		internal set
		{
			_Location = value;
		}
	}

	public bool IsRightToLeft
	{
		get
		{
			return _IsRightToLeft;
		}
		set
		{
			if (_IsRightToLeft != value)
			{
				_IsRightToLeft = value;
				OnIsRightToLeftChanged();
			}
		}
	}

	public virtual bool IsLayoutValid => _IsLayoutValid;

	public bool IsFocused => _IsFocused;

	[DefaultValue(eItemAlignment.Left)]
	public eItemAlignment Alignment
	{
		get
		{
			return _Alignment;
		}
		set
		{
			if (_Alignment != value)
			{
				_Alignment = value;
				InvalidateArrange();
			}
		}
	}

	internal eSystemItemType ItemType
	{
		get
		{
			return _ItemType;
		}
		set
		{
			_ItemType = value;
		}
	}

	public Size CalculatedSize
	{
		get
		{
			return _CalculatedSize;
		}
		internal set
		{
			_CalculatedSize = value;
		}
	}

	public event EventHandler ArrangeInvalid;

	public event EventHandler RenderInvalid;

	public event EventHandler Click;

	public event MouseEventHandler MouseClick
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseClick;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseClick, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseClick;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseClick, value2, val2);
			}
			while (val != val2);
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

	internal virtual void ProcessMouseEnter()
	{
		if (GetIsEnabled())
		{
			OnMouseEnter();
		}
	}

	protected virtual void OnMouseEnter()
	{
	}

	internal virtual void ProcessMouseLeave()
	{
		if (GetIsEnabled())
		{
			OnMouseLeave();
		}
	}

	protected virtual void OnMouseLeave()
	{
	}

	internal virtual void ProcessMouseMove(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			OnMouseMove(e);
		}
	}

	protected virtual void OnMouseMove(MouseEventArgs e)
	{
	}

	internal virtual void ProcessMouseWheel(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			OnMouseWheel(e);
		}
	}

	protected virtual void OnMouseWheel(MouseEventArgs e)
	{
	}

	internal virtual void ProcessMouseDown(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			OnMouseDown(e);
		}
	}

	protected virtual void OnMouseDown(MouseEventArgs e)
	{
		if (this.MouseDown != null)
		{
			this.MouseDown.Invoke((object)this, e);
		}
	}

	internal virtual void ProcessMouseUp(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			OnMouseUp(e);
		}
	}

	protected virtual void OnMouseUp(MouseEventArgs e)
	{
		if (this.MouseUp != null)
		{
			this.MouseUp.Invoke((object)this, e);
		}
	}

	internal virtual void ProcessClick()
	{
		if (GetIsEnabled())
		{
			OnClick(EventArgs.Empty);
		}
	}

	protected virtual void OnClick(EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(this, e);
		}
	}

	internal virtual void ProcessMouseClick(MouseEventArgs e)
	{
		if (GetIsEnabled())
		{
			OnMouseClick(e);
		}
	}

	protected virtual void OnMouseClick(MouseEventArgs e)
	{
		if (this.MouseClick != null)
		{
			this.MouseClick.Invoke((object)this, e);
		}
	}

	internal virtual void ProcessKeyDown(KeyEventArgs e)
	{
		OnKeyDown(e);
	}

	protected virtual void OnKeyDown(KeyEventArgs e)
	{
	}

	internal virtual void ProcessKeyUp(KeyEventArgs e)
	{
		OnKeyUp(e);
	}

	protected virtual void OnKeyUp(KeyEventArgs e)
	{
	}

	internal virtual void ProcessKeyPress(KeyPressEventArgs e)
	{
		OnKeyPress(e);
	}

	protected virtual void OnKeyPress(KeyPressEventArgs e)
	{
	}

	internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return OnCmdKey(ref msg, keyData);
	}

	protected virtual bool OnCmdKey(ref Message msg, Keys keyData)
	{
		return false;
	}

	internal virtual void ProcessGotFocus()
	{
		OnGotFocus();
	}

	protected virtual void OnGotFocus()
	{
		_IsFocused = true;
		OnFocusChanged();
	}

	internal virtual void ProcessLostFocus()
	{
		OnLostFocus();
	}

	protected virtual void OnLostFocus()
	{
		_IsFocused = false;
		OnFocusChanged();
	}

	protected virtual void OnFocusChanged()
	{
	}

	internal virtual void ProcessPaint(PaintInfo p)
	{
		if (IsRendered)
		{
			OnPaint(p);
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
		if (!_Enabled)
		{
			return _Enabled;
		}
		for (VisualItem parent = Parent; parent != null; parent = parent.Parent)
		{
			if (!parent.Enabled)
			{
				return false;
			}
		}
		return _Enabled;
	}

	protected virtual bool GetIsEnabled(PaintInfo p)
	{
		if (p.ParentEnabled)
		{
			return _Enabled;
		}
		return false;
	}

	protected virtual void OnVisibleChanged()
	{
		if (_Parent != null)
		{
			_Parent.OnChildItemVisibleChanged(this);
		}
	}

	public virtual void InvalidateArrange()
	{
		_RenderBounds = Rectangle.Empty;
		_Location = Point.Empty;
		_IsLayoutValid = false;
		if (_Parent != null)
		{
			_Parent.InvalidateArrange();
		}
		OnArrangeInvalid();
	}

	public virtual void InvalidateRender()
	{
		if (_Parent != null)
		{
			_Parent.InvalidateRender();
		}
		OnRenderInvalid();
	}

	protected virtual void OnRenderInvalid()
	{
		if (this.RenderInvalid != null)
		{
			this.RenderInvalid(this, new EventArgs());
		}
	}

	protected virtual void OnArrangeInvalid()
	{
		if (this.ArrangeInvalid != null)
		{
			this.ArrangeInvalid(this, new EventArgs());
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
		_IsLayoutValid = true;
	}
}
