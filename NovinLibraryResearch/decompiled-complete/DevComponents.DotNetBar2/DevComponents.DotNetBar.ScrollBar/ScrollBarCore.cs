using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.ScrollBar;

public class ScrollBarCore : IDisposable
{
	internal enum ᘱ
	{
		ᘲ,
		ᘳ,
		ᘴ,
		ᘵ,
		ᘶ
	}

	private Rectangle ᕮ = Rectangle.Empty;

	private eOrientation Ⴐ = eOrientation.Vertical;

	private Size ᘆ = new Size(Dpi.Width15, Dpi.Height17);

	private Size ᘇ = new Size(Dpi.Width17, Dpi.Height15);

	private Rectangle ᘈ = Rectangle.Empty;

	private Rectangle ᘉ = Rectangle.Empty;

	private Rectangle ᘊ = Rectangle.Empty;

	private ᘱ ᘋ = ᘱ.ᘶ;

	private Control ဿ;

	private bool ݠ;

	private Timer _৮;

	private int ᘌ;

	private Bitmap ᘍ;

	private bool ᘎ = true;

	private bool ᘏ;

	private bool ᘐ;

	private EventHandler ᗲ;

	private bool ᘑ;

	private bool ᘒ;

	private int ᘓ;

	private ScrollEventHandler ᗳ;

	private bool _ᘔ;

	private bool ᘕ = true;

	private bool ᘖ;

	private int ᘗ;

	private int ᘘ;

	private int ᘙ = 1;

	private int ᘚ = 16;

	private int ᘛ;

	private bool _ᘜ;

	private bool Ӻ = true;

	private bool ݕ = true;

	private bool ᘁ
	{
		get
		{
			if (ᘒ && ဿ.Focused)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsMouseDown => ݠ;

	private ScrollOrientation ᘂ
	{
		get
		{
			if (Orientation == eOrientation.Horizontal)
			{
				return (ScrollOrientation)0;
			}
			return (ScrollOrientation)1;
		}
	}

	internal bool ᘃ
	{
		get
		{
			return ᘕ;
		}
		set
		{
			ᘕ = value;
		}
	}

	internal bool ᘄ
	{
		get
		{
			return ᘖ;
		}
		set
		{
			ᘖ = value;
		}
	}

	public Rectangle ThumbDecreaseRectangle
	{
		get
		{
			return ᘈ;
		}
		set
		{
			ᘈ = value;
			ᘝ();
		}
	}

	public Rectangle ThumbIncreaseRectangle
	{
		get
		{
			return ᘉ;
		}
		set
		{
			ᘉ = value;
			ᘝ();
		}
	}

	public Rectangle TrackRectangle
	{
		get
		{
			return ᘊ;
		}
		set
		{
			ᘊ = value;
			ᘝ();
		}
	}

	public Rectangle DisplayRectangle
	{
		get
		{
			return ᕮ;
		}
		set
		{
			ᕮ = value;
			ᘝ();
			ᘥ();
		}
	}

	public eOrientation Orientation
	{
		get
		{
			return Ⴐ;
		}
		set
		{
			Ⴐ = value;
			ᘝ();
		}
	}

	public int Minimum
	{
		get
		{
			return ᘗ;
		}
		set
		{
			ᘗ = value;
			ᘝ();
			ᘥ();
		}
	}

	public int Maximum
	{
		get
		{
			return ᘘ;
		}
		set
		{
			ᘘ = value;
			ᘝ();
			ᘥ();
		}
	}

	public int SmallChange
	{
		get
		{
			return ᘙ;
		}
		set
		{
			ᘙ = value;
		}
	}

	public int LargeChange
	{
		get
		{
			return ᘚ;
		}
		set
		{
			ᘚ = value;
			ᘝ();
			ᘥ();
		}
	}

	public int Value
	{
		get
		{
			return ᘛ;
		}
		set
		{
			value = ᘪ(value);
			if (ᘛ == value && (!ᘑ || ᘒ))
			{
				return;
			}
			ᘛ = value;
			if (!ᘏ)
			{
				ᘝ();
				ᘥ();
				if (ᗲ != null)
				{
					ᗲ(this, new EventArgs());
				}
				ᘤ();
			}
		}
	}

	internal ᘱ ᘅ
	{
		get
		{
			return ᘋ;
		}
		set
		{
			ᘋ = value;
		}
	}

	public bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			Ӻ = value;
		}
	}

	public Control ParentControl
	{
		get
		{
			return ဿ;
		}
		set
		{
			ဿ = value;
			if (ဿ == null)
			{
				ᘡ();
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return ݕ;
		}
		set
		{
			if (ݕ != value)
			{
				ݕ = value;
				ᘤ();
			}
		}
	}

	public bool IsAppScrollBarStyle
	{
		get
		{
			return ᘐ;
		}
		set
		{
			ᘐ = value;
			ᘝ();
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ScrollEventHandler Scroll
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = ᗳ;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᗳ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = ᗳ;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᗳ, value2, val2);
			}
			while (val != val2);
		}
	}

	public ScrollBarCore()
		: this(null)
	{
	}

	public ScrollBarCore(Control parentControl)
		: this(parentControl, isPassive: false)
	{
	}

	public ScrollBarCore(Control parentControl, bool isPassive)
	{
		ဿ = parentControl;
		ᘑ = ဿ is ScrollBar || ဿ is ScrollBarAdv;
		ᘒ = ဿ is ScrollBarAdv;
		ᘏ = isPassive;
		ᘝ();
	}

	internal void ᘝ()
	{
		if (ᘍ != null)
		{
			((Image)ᘍ).Dispose();
			ᘍ = null;
		}
	}

	private ᘷ ᘞ()
	{
		ᘷ ᘷ2 = null;
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			ᘼ ᘼ = new ᘼ();
			ᘼ.ᘽ = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			ᘼ.ᘾ = ᘐ;
			return ᘼ;
		}
		ᚂ ᚂ2 = new ᚂ();
		ᚂ2.ᘽ = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		ᚂ2.ᘾ = ᘐ;
		return ᚂ2;
	}

	public void Paint(ItemPaintArgs p)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		if (_ᘔ)
		{
			return;
		}
		_ᘔ = true;
		try
		{
			if (ᕮ.IsEmpty || ᕮ.Width <= 0 || ᕮ.Height <= 0 || p.Graphics == null)
			{
				return;
			}
			ᘷ ᘷ2 = ᘞ();
			if (ᘍ == null)
			{
				try
				{
					ᘍ = new Bitmap(ᕮ.Width, ᕮ.Height, p.Graphics);
				}
				catch
				{
					return;
				}
				ᘍ.MakeTransparent();
				ᘎ = true;
			}
			if (!ᘎ)
			{
				Bitmap val = ᘍ;
				if (val != null)
				{
					p.Graphics.DrawImageUnscaled((Image)(object)val, ᕮ.Location);
				}
				return;
			}
			Graphics val2 = Graphics.FromImage((Image)(object)ᘍ);
			try
			{
				if (!ᕮ.Location.IsEmpty)
				{
					val2.TranslateTransform((float)(-ᕮ.X), (float)(-ᕮ.Y));
				}
				ᘹ state = ((!ݕ) ? ᘹ.ड़ : ᘹ.ཊ);
				if (ݕ && (ᘋ != ᘱ.ᘶ || ᘁ))
				{
					state = ᘹ.ᘻ;
				}
				bool rtl = false;
				if (ဿ != null)
				{
					rtl = (int)ဿ.RightToLeft == 1;
				}
				ᘷ2.PaintBackground(val2, ᕮ, state, Ⴐ == eOrientation.Horizontal, ᘖ, rtl);
				ᘸ position = ᘸ._074A;
				if (Ⴐ == eOrientation.Horizontal)
				{
					position = ᘸ._0749;
				}
				if (ᘋ == ᘱ.ᘳ && ݕ)
				{
					state = ((!ݠ) ? ᘹ.ᘺ : ᘹ._0F78);
				}
				ᘷ2.PaintThumb(val2, ᘈ, position, state);
				position = ᘸ.ራ;
				if (ᘋ == ᘱ.ᘲ && ݕ)
				{
					state = ((!ݠ) ? ᘹ.ᘺ : ᘹ._0F78);
				}
				else if (ᘋ != ᘱ.ᘶ && ݕ)
				{
					state = ᘹ.ᘻ;
				}
				if (Ⴐ == eOrientation.Horizontal)
				{
					position = ᘸ.ሪ;
				}
				ᘷ2.PaintThumb(val2, ᘉ, position, state);
				if (ᘋ == ᘱ.ᘴ && ݕ)
				{
					state = ((!ݠ) ? ᘹ.ᘺ : ᘹ._0F78);
				}
				else if ((ᘋ != ᘱ.ᘶ || ᘁ) && ݕ)
				{
					state = ᘹ.ᘻ;
				}
				if (Ⴐ == eOrientation.Horizontal)
				{
					ᘷ2.PaintTrackHorizontal(val2, ᘊ, state);
				}
				else
				{
					ᘷ2.PaintTrackVertical(val2, ᘊ, state);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			p.Graphics.DrawImageUnscaled((Image)(object)ᘍ, ᕮ.Location);
			ᘎ = false;
		}
		finally
		{
			_ᘔ = false;
		}
	}

	public void MouseMove(MouseEventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (!ݕ)
		{
			return;
		}
		Point point = new Point(e.X, e.Y);
		if (ݠ && ᘋ == ᘱ.ᘴ)
		{
			if (!ᘏ)
			{
				int ԝ = ᘟ(point);
				ᘩ(ԝ, (ScrollEventType)5);
			}
		}
		else
		{
			if ((int)e.Button != 0 && ᘋ != ᘱ.ᘶ)
			{
				return;
			}
			if (ᕮ.Contains(point))
			{
				ᘱ ᘱ = ᘱ.ᘵ;
				if (ᘈ.Contains(point))
				{
					ᘱ = ᘱ.ᘳ;
				}
				else if (ᘉ.Contains(point))
				{
					ᘱ = ᘱ.ᘲ;
				}
				else if (ᘊ.Contains(point))
				{
					ᘱ = ᘱ.ᘴ;
				}
				if (ᘋ != ᘱ)
				{
					ᘋ = ᘱ;
					ᘤ();
				}
			}
			else if (ᘋ != ᘱ.ᘶ)
			{
				ᘋ = ᘱ.ᘶ;
				ᘤ();
			}
		}
	}

	private int ᘟ(Point _065C)
	{
		if (Ⴐ == eOrientation.Vertical)
		{
			int val = _065C.Y - ᘌ;
			val = Math.Max(val, ᘈ.Bottom);
			val = Math.Min(val, ᘉ.Y - ᘦ());
			val -= ᘈ.Bottom;
			int num = ᘧ() - ᘦ();
			return (int)((float)(ᘬ() - Minimum) * ((float)val / (float)num));
		}
		int val2 = _065C.X - ᘌ;
		val2 = Math.Max(val2, ᘈ.Right);
		val2 = Math.Min(val2, ᘉ.X - ᘦ());
		val2 -= ᘈ.Right;
		int num2 = ᘧ() - ᘦ();
		return (int)((float)(ᘬ() - Minimum) * ((float)val2 / (float)num2));
	}

	public void MouseLeave()
	{
		if (ᘋ != ᘱ.ᘶ && (!ݠ || (ᘋ != ᘱ.ᘴ && (ᘋ == ᘱ.ᘶ || !ᘏ))))
		{
			ᘋ = ᘱ.ᘶ;
			ᘤ();
		}
	}

	public void MouseDown(MouseEventArgs e)
	{
		if (ᘋ == ᘱ.ᘶ)
		{
			return;
		}
		ݠ = true;
		if (ᘏ)
		{
			ᘎ = true;
			return;
		}
		if (Ⴐ == eOrientation.Vertical)
		{
			ᘌ = e.Y - ᘊ.Y;
		}
		else
		{
			ᘌ = e.X - ᘊ.X;
		}
		if (ᘋ == ᘱ.ᘳ)
		{
			ᘩ(Value - ᘙ, (ScrollEventType)0);
		}
		else if (ᘋ == ᘱ.ᘲ)
		{
			ᘩ(Value + ᘙ, (ScrollEventType)1);
		}
		else if (ᘋ == ᘱ.ᘴ)
		{
			ᘤ();
			if (ဿ != null)
			{
				ဿ.Capture = true;
			}
		}
		else if (ᘋ == ᘱ.ᘵ)
		{
			ᘠ(new Point(e.X, e.Y));
		}
		ᘢ();
	}

	private void ᘠ(Point _065C)
	{
		if ((Ⴐ != eOrientation.Vertical || (_065C.Y >= ᘊ.Y && _065C.Y <= ᘊ.Bottom)) && (Ⴐ != 0 || (_065C.X >= ᘊ.X && _065C.X <= ᘊ.Right)))
		{
			return;
		}
		if ((Ⴐ == eOrientation.Vertical && ᘊ.Y > _065C.Y) || (Ⴐ == eOrientation.Horizontal && ᘊ.X > _065C.X))
		{
			ᘩ(Value - LargeChange, (ScrollEventType)2);
		}
		else if (ᘑ)
		{
			if (Value + LargeChange > ᘬ())
			{
				ᘩ(ᘬ(), (ScrollEventType)3);
			}
			else
			{
				ᘩ(Value + LargeChange, (ScrollEventType)3);
			}
		}
		else
		{
			ᘩ(Value + LargeChange, (ScrollEventType)3);
		}
	}

	public void MouseUp(MouseEventArgs e)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (ဿ != null && ဿ.Capture)
		{
			ဿ.Capture = false;
		}
		if (ݠ)
		{
			if (ᘋ != ᘱ.ᘶ && _ᘜ)
			{
				if (ᘋ == ᘱ.ᘴ)
				{
					ᘫ(new ScrollEventArgs((ScrollEventType)4, Value, Value, ᘂ));
				}
				ᘫ(new ScrollEventArgs((ScrollEventType)8, Value, Value, ᘂ));
				_ᘜ = false;
			}
			ᘡ();
			ݠ = false;
			ᘤ();
		}
		if (!DisplayRectangle.Contains(e.X, e.Y))
		{
			MouseLeave();
		}
	}

	private void ᘡ()
	{
		Timer val = _৮;
		_৮ = null;
		if (val != null)
		{
			val.Enabled = false;
			val.Tick -= ᘣ;
			((Component)(object)val).Dispose();
		}
	}

	private void ᘢ()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (_৮ == null)
		{
			ᘓ = 0;
			_৮ = new Timer();
			_৮.Interval = 100;
			_৮.Tick += ᘣ;
			_৮.Enabled = true;
		}
	}

	private void ᘣ(object _٠, EventArgs ؿ)
	{
		Control val = ဿ;
		if (ݠ && ᘋ == ᘱ.ᘳ)
		{
			ᘩ(Value - ᘙ, (ScrollEventType)0);
		}
		else if (ݠ && ᘋ == ᘱ.ᘲ)
		{
			ᘩ(Value + ᘙ, (ScrollEventType)1);
		}
		else if (ݠ && ᘋ == ᘱ.ᘵ && val != null)
		{
			Point point = val.PointToClient(Control.MousePosition);
			if (ᘊ.Contains(point))
			{
				ᘡ();
				ݠ = false;
			}
			else
			{
				ᘠ(point);
			}
		}
		ᘓ++;
		if (ᘓ > 4 && _৮ != null && _৮.Interval > 20)
		{
			Timer obj = _৮;
			obj.Interval -= 10;
		}
	}

	private void ᘤ()
	{
		ᘎ = true;
		Control val = ဿ;
		if (val != null && !ᘏ)
		{
			val.Invalidate(ᕮ);
			val.Update();
		}
	}

	private void ᘥ()
	{
		if (!ᘏ)
		{
			Rectangle rectangle = ᕮ;
			if (ᘕ)
			{
				rectangle.Inflate(-1, -1);
			}
			if (Ⴐ == eOrientation.Vertical)
			{
				ᘈ = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, ᘆ.Height);
				ᘉ = new Rectangle(rectangle.X, rectangle.Bottom - ᘆ.Height, rectangle.Width, ᘆ.Height);
				int height = ᘦ();
				ᘊ = new Rectangle(rectangle.X, rectangle.Y + ᘨ(), rectangle.Width, height);
			}
			else
			{
				ᘈ = new Rectangle(rectangle.X, rectangle.Y, ᘇ.Width, rectangle.Height);
				ᘉ = new Rectangle(rectangle.Right - ᘇ.Width, rectangle.Y, ᘇ.Width, rectangle.Height);
				int width = ᘦ();
				ᘊ = new Rectangle(rectangle.X + ᘨ(), rectangle.Y, width, rectangle.Height);
			}
			if (!(ဿ is ScrollBar))
			{
				ᘤ();
			}
		}
	}

	public virtual void ScaleItem(SizeF factor)
	{
		ᘆ = new Size(Dpi.Width15, Dpi.Height17);
		ᘇ = new Size(Dpi.Width17, Dpi.Height15);
	}

	private int ᘦ()
	{
		int num = 0;
		int num2 = ᘧ();
		num = (int)((float)num2 * ((float)ᘚ / (float)(ᘘ - ᘗ + 1)));
		num = Math.Max(14, num);
		return Math.Min(num, num2);
	}

	private int ᘧ()
	{
		Rectangle rectangle = ᕮ;
		if (ᘕ)
		{
			rectangle.Inflate(-1, -1);
		}
		int height = rectangle.Height;
		height = ((Ⴐ != 0) ? (height - ᘆ.Height * 2) : (rectangle.Width - ᘇ.Width * 2));
		return Math.Max(height, 8);
	}

	private int ᘨ()
	{
		int num = ᘦ();
		int num2 = ᘧ() - num;
		int num3 = Math.Min(num2, Math.Max((int)((float)num2 * ((float)ᘛ / (float)ᘬ())), 0));
		if (Ⴐ == eOrientation.Vertical)
		{
			return num3 + ᘆ.Height;
		}
		return num3 + ᘇ.Width;
	}

	private void ᘩ(int ԝ, ScrollEventType ᗧ)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		ԝ = ᘪ(ԝ);
		ᘫ(new ScrollEventArgs(ᗧ, ᘛ, ԝ, ᘂ));
		Value = ԝ;
		_ᘜ = true;
	}

	private int ᘪ(int ԝ)
	{
		int num = (ᘒ ? ᘬ() : ᘘ);
		if (ԝ < ᘗ)
		{
			ԝ = ᘗ;
		}
		if (ԝ > num)
		{
			ԝ = num;
		}
		return ԝ;
	}

	private void ᘫ(ScrollEventArgs ؿ)
	{
		ScrollEventHandler val = ᗳ;
		if (val != null)
		{
			val.Invoke((object)this, ؿ);
		}
	}

	internal int ᘬ()
	{
		if (ᘑ)
		{
			return ᘘ - ᘚ + 1;
		}
		return ᘘ;
	}

	internal bool ᘭ(ref Message ᘮ, Keys ᘯ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		if ((int)ᘯ == 34)
		{
			ᘩ(Value + ᘚ, (ScrollEventType)3);
			return true;
		}
		if ((int)ᘯ == 33)
		{
			ᘩ(Value - ᘚ, (ScrollEventType)3);
			return true;
		}
		if ((int)ᘯ == 38 || (int)ᘯ == 37)
		{
			ᘩ(Value - ᘙ, (ScrollEventType)0);
			return true;
		}
		if ((int)ᘯ == 40 || (int)ᘯ == 39)
		{
			ᘩ(Value + ᘙ, (ScrollEventType)1);
			return true;
		}
		if ((int)ᘯ == 36)
		{
			ᘩ(ᘗ, (ScrollEventType)6);
			return true;
		}
		if ((int)ᘯ == 35)
		{
			ᘩ(ᘘ, (ScrollEventType)7);
			return true;
		}
		return false;
	}

	public void Dispose()
	{
		ᘡ();
		ᘝ();
		ဿ = null;
	}

	internal void ᘰ()
	{
		ᘝ();
		Control val = ဿ;
		if (val != null)
		{
			val.Invalidate();
		}
	}
}
