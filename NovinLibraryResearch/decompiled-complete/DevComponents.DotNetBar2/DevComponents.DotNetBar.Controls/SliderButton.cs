using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class SliderButton : Control
{
	private SlidePanel __23BC;

	private Point __23BD = Point.Empty;

	private Point _ᐨ = Point.Empty;

	private Point __23BE = Point.Empty;

	private bool __23BF;

	private int _23C0 = SystemInformation.FrameBorderSize.Width * 2 - 2;

	private static readonly Point _23C1 = new Point(4, 4);

	private static readonly Point _23C2 = new Point(96, 96);

	private Timer __23C3;

	private bool __23C4 = true;

	internal static readonly Size _23C5 = new Size(16, 36);

	private Size __23C6 = _23C5;

	internal static readonly Size _23C7 = new Size(24, 36);

	private Size __23C8 = _23C7;

	private bool _Ꮄ;

	private bool __171A;

	private ElementStyle _ᎌ;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ = 10f;

	[Description("Indicates whether slider button automatically checks whether its top-most control on the form, i.e. visible at all times on top of other controls")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool AutoTopMostEnabled
	{
		get
		{
			return __23C4;
		}
		set
		{
			if (value != __23C4)
			{
				bool oldValue = __23C4;
				__23C4 = value;
				OnAutoTopMostEnabledChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates slider size in default state. Notice that size specified here applies to Left and Right SlidePanel positions. For Top and Bottom positions the Width and Height are interchaged.")]
	[Category("Appearance")]
	public Size SliderSize
	{
		get
		{
			return __23C6;
		}
		set
		{
			if (value != __23C6)
			{
				Size oldValue = __23C6;
				__23C6 = value;
				OnSliderSizeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates slider size in active state. Notice that size specified here applies to Left and Right SlidePanel positions. For Top and Bottom positions the Width and Height are interchaged.")]
	[Category("Appearance")]
	public Size ActiveSliderSize
	{
		get
		{
			return __23C8;
		}
		set
		{
			if (value != __23C8)
			{
				Size oldValue = __23C8;
				__23C8 = value;
				OnActiveSliderSizeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates  whether slider button is in active state.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool IsActive
	{
		get
		{
			return _Ꮄ;
		}
		set
		{
			if (value != _Ꮄ)
			{
				bool ꮄ = _Ꮄ;
				_Ꮄ = value;
				OnIsActiveChanged(ꮄ, value);
			}
		}
	}

	protected override Size DefaultSize => _23CD();

	internal bool _206D
	{
		get
		{
			return __171A;
		}
		set
		{
			if (value != __171A)
			{
				bool oldValue = __171A;
				__171A = value;
				OnIsMouseDownChanged(oldValue, value);
			}
		}
	}

	public ElementStyle Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			if (_ᎌ != null)
			{
				_ᎌ.StyleChanged -= _065F;
			}
			_ᎌ = value;
			if (_ᎌ != null)
			{
				_ᎌ.StyleChanged += _065F;
			}
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the Symbol.")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[DefaultValue(10f)]
	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	public SliderButton(SlidePanel slidePanel)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		__23BC = slidePanel;
		((Control)this).Margin = default(Padding);
	}

	protected override void Dispose(bool disposing)
	{
		if (_ᎌ != null)
		{
			_ᎌ.StyleChanged -= _065F;
		}
		_23CC();
		((Control)this).Dispose(disposing);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (((Control)this).Parent != null)
		{
			_23C9();
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (!__23BF)
		{
			__23BC.IsOpen = true;
		}
		((Control)this).OnClick(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			_ᐨ = Control.MousePosition;
			_206D = true;
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_206D && !__23BF)
		{
			eSlideSide slideSide = __23BC.SlideSide;
			if (slideSide == eSlideSide.Left)
			{
				int num = Control.MousePosition.X - _ᐨ.X;
				Point point = _23C1;
				if (num > point.X)
				{
					__23BF = true;
					goto IL_00fd;
				}
			}
			if (slideSide == eSlideSide.Right)
			{
				int num2 = _ᐨ.X - Control.MousePosition.X;
				Point point2 = _23C1;
				if (num2 > point2.X)
				{
					__23BF = true;
					goto IL_00fd;
				}
			}
			if (slideSide == eSlideSide.Top)
			{
				int num3 = Control.MousePosition.Y - _ᐨ.Y;
				Point point3 = _23C1;
				if (num3 > point3.Y)
				{
					__23BF = true;
					goto IL_00fd;
				}
			}
			if (slideSide == eSlideSide.Bottom)
			{
				int num4 = _ᐨ.Y - Control.MousePosition.Y;
				Point point4 = _23C1;
				if (num4 > point4.Y)
				{
					__23BF = true;
				}
			}
		}
		goto IL_00fd;
		IL_00fd:
		if (_206D && __23BF)
		{
			if (!((Control)this).Capture)
			{
				((Control)this).Capture = true;
			}
			Point mousePosition = Control.MousePosition;
			eSlideSide slideSide2 = __23BC.SlideSide;
			switch (slideSide2)
			{
			case eSlideSide.Left:
				__23BD = new Point(Math.Max(0, mousePosition.X - _ᐨ.X), 0);
				break;
			case eSlideSide.Right:
				__23BD = new Point(Math.Max(0, _ᐨ.X - mousePosition.X), 0);
				break;
			case eSlideSide.Top:
				__23BD = new Point(0, Math.Max(0, mousePosition.Y - _ᐨ.Y));
				break;
			case eSlideSide.Bottom:
				__23BD = new Point(0, Math.Max(0, _ᐨ.Y - mousePosition.Y));
				break;
			}
			if (!__23BD.IsEmpty)
			{
				_23C9();
				if (!((Control)__23BC).Visible)
				{
					((Control)__23BC).Visible = true;
					__23BE = ((Control)__23BC).Location;
				}
				switch (slideSide2)
				{
				case eSlideSide.Left:
					((Control)__23BC).Left = __23BE.X + __23BD.X;
					break;
				case eSlideSide.Right:
					((Control)__23BC).Left = __23BE.X - __23BD.X;
					break;
				case eSlideSide.Top:
					((Control)__23BC).Top = __23BE.Y + __23BD.Y;
					break;
				case eSlideSide.Bottom:
					((Control)__23BC).Top = __23BE.Y - __23BD.Y;
					break;
				}
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_206D)
		{
			Point mousePosition = Control.MousePosition;
			if (((Control)this).Capture)
			{
				((Control)this).Capture = false;
			}
			eSlideSide slideSide = __23BC.SlideSide;
			if (slideSide == eSlideSide.Left)
			{
				int num = mousePosition.X - _ᐨ.X;
				Point point = _23C2;
				if (num > point.X)
				{
					__23BC.IsOpen = true;
					goto IL_0128;
				}
			}
			if (slideSide == eSlideSide.Right)
			{
				int num2 = _ᐨ.X - mousePosition.X;
				Point point2 = _23C2;
				if (num2 > point2.X)
				{
					__23BC.IsOpen = true;
					goto IL_0128;
				}
			}
			if (slideSide == eSlideSide.Top)
			{
				int num3 = mousePosition.Y - _ᐨ.Y;
				Point point3 = _23C2;
				if (num3 > point3.Y)
				{
					__23BC.IsOpen = true;
					goto IL_0128;
				}
			}
			if (slideSide == eSlideSide.Bottom)
			{
				int num4 = _ᐨ.Y - mousePosition.Y;
				Point point4 = _23C2;
				if (num4 > point4.Y)
				{
					__23BC.IsOpen = true;
					goto IL_0128;
				}
			}
			if (__23BF)
			{
				((Control)__23BC).Location = __23BE;
				((Control)__23BC).Visible = false;
			}
			goto IL_0128;
		}
		goto IL_0135;
		IL_0135:
		((Control)this).OnMouseUp(e);
		return;
		IL_0128:
		_206D = false;
		_23C9();
		goto IL_0135;
	}

	protected override void OnMouseCaptureChanged(EventArgs e)
	{
		if (_206D)
		{
			_ = ((Control)this).Capture;
		}
		((Control)this).OnMouseCaptureChanged(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		Size activeSliderSize = ActiveSliderSize;
		if (activeSliderSize != ((Control)this).Size)
		{
			((Control)this).Size = activeSliderSize;
			_23C9();
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		Size sliderSize = SliderSize;
		if (sliderSize != ((Control)this).Size)
		{
			((Control)this).Size = sliderSize;
			_23C9();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).Invalidate();
		((Control)this).OnResize(e);
	}

	internal void _23C9()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		Control parent = ((Control)this).Parent;
		if (parent != null)
		{
			eSlideSide slideSide = __23BC.SlideSide;
			Point location = default(Point);
			switch (slideSide)
			{
			case eSlideSide.Left:
			{
				Padding margin7 = ((Control)this).Margin;
				location.X = ((Padding)(ref margin7)).Left + __23BD.X;
				int num4 = (parent.ClientRectangle.Height - ((Control)this).Height) / 2;
				Padding margin8 = ((Control)this).Margin;
				location.Y = num4 + ((Padding)(ref margin8)).Top;
				break;
			}
			case eSlideSide.Right:
			{
				int width = parent.ClientRectangle.Width;
				Padding margin5 = ((Control)this).Margin;
				location.X = width - ((Padding)(ref margin5)).Right - ((Control)this).Width - __23BD.X;
				int num3 = (parent.ClientRectangle.Height - ((Control)this).Height) / 2;
				Padding margin6 = ((Control)this).Margin;
				location.Y = num3 + ((Padding)(ref margin6)).Top;
				break;
			}
			case eSlideSide.Top:
			{
				int num2 = (parent.ClientRectangle.Width - ((Control)this).Width) / 2;
				Padding margin3 = ((Control)this).Margin;
				location.X = num2 + ((Padding)(ref margin3)).Top;
				Padding margin4 = ((Control)this).Margin;
				location.Y = ((Padding)(ref margin4)).Top + __23BD.Y;
				break;
			}
			case eSlideSide.Bottom:
			{
				int num = (parent.ClientRectangle.Width - ((Control)this).Width) / 2;
				Padding margin = ((Control)this).Margin;
				location.X = num + ((Padding)(ref margin)).Top;
				int height = parent.ClientRectangle.Height;
				Padding margin2 = ((Control)this).Margin;
				location.Y = height - ((Padding)(ref margin2)).Bottom - ((Control)this).Height - __23BD.Y;
				break;
			}
			}
			((Control)this).Location = location;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		if (_ᎌ != null)
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(_ᎌ, graphics, ((Control)this).ClientRectangle);
			ElementStyleDisplay.Paint(e2);
		}
		if (!string.IsNullOrEmpty(__0EA6))
		{
			Font font = Symbols.GetFont(_ຨ, _ວ);
			Color color = ((Control)this).ForeColor;
			bool ല = false;
			ElementStyle elementStyle = null;
			if (_ᎌ != null)
			{
				elementStyle = ElementStyleDisplay.റ(_ᎌ, out ല);
			}
			if (elementStyle != null && !elementStyle.TextColor.IsEmpty)
			{
				color = elementStyle.TextColor;
			}
			if (ല)
			{
				elementStyle.Dispose();
			}
			TextDrawing.DrawStringLegacy(graphics, __0EA6, font, __0EA4.IsEmpty ? color : __0EA4, ((Control)this).ClientRectangle, eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.VerticalCenter);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_23CA();
		((Control)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_23CC();
		((Control)this).OnHandleDestroyed(e);
	}

	private void _23CA()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (__23C3 == null && __23C4)
		{
			__23C3 = new Timer();
			__23C3.Interval = 800;
			__23C3.Tick += _23CB;
			__23C3.Start();
		}
	}

	private void _23CB(object _٠, EventArgs ؿ)
	{
		if (((Control)this).Visible && ((Control)this).Parent != null)
		{
			Control parent = ((Control)this).Parent;
			if (parent.Controls.IndexOf((Control)(object)this) != 0)
			{
				parent.Controls.SetChildIndex((Control)(object)this, 0);
			}
		}
	}

	private void _23CC()
	{
		Timer val = __23C3;
		__23C3 = null;
		if (val != null)
		{
			val.Stop();
			((Component)(object)val).Dispose();
		}
	}

	protected virtual void OnAutoTopMostEnabledChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			_23CC();
		}
		else if (((Control)this).IsHandleCreated)
		{
			_23CA();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSliderSize()
	{
		return __23C6 != _23C5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSliderSize()
	{
		SliderSize = _23C5;
	}

	protected virtual void OnSliderSizeChanged(Size oldValue, Size newValue)
	{
		((Control)this).Size = newValue;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeActiveSliderSize()
	{
		return __23C8 != _23C7;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetActiveSliderSize()
	{
		ActiveSliderSize = _23C7;
	}

	protected virtual void OnActiveSliderSizeChanged(Size oldValue, Size newValue)
	{
	}

	protected virtual void OnIsActiveChanged(bool oldValue, bool newValue)
	{
	}

	private Size _23CD()
	{
		Size result = (IsActive ? ActiveSliderSize : SliderSize);
		if (__23BC != null)
		{
			eSlideSide slideSide = __23BC.SlideSide;
			if (slideSide == eSlideSide.Left || slideSide == eSlideSide.Right)
			{
				return result;
			}
			return new Size(result.Height, result.Width);
		}
		return result;
	}

	protected virtual void OnIsMouseDownChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			__23BF = false;
			__23BD = Point.Empty;
		}
		((Control)this).Invalidate();
	}

	private void _065F(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		((Control)this).Invalidate();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		((Control)this).Invalidate();
		((Control)this).Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		((Control)this).Invalidate();
	}
}
