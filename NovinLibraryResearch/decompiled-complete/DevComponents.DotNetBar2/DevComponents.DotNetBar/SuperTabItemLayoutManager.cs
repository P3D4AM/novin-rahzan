using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

public class SuperTabItemLayoutManager : BlockLayoutManager
{
	private int __202D;

	private Size __202E = new Size(16, 16);

	private Size __1F5C = Size.Empty;

	private Size __202F = Size.Empty;

	private SuperTabStripItem _ὗ;

	private Dictionary<BaseItem, Size> __2030;

	public Size FixedTabSize
	{
		get
		{
			return __1F5C;
		}
		set
		{
			__1F5C = value;
		}
	}

	public Size TabLayoutOffset
	{
		get
		{
			return __202F;
		}
		set
		{
			__202F = value;
		}
	}

	internal ῲ έ => _ὗ.έ;

	public SuperTabItemLayoutManager()
	{
	}

	public SuperTabItemLayoutManager(SuperTabStripItem tabStripItem)
	{
		_ὗ = tabStripItem;
		__2030 = new Dictionary<BaseItem, Size>();
	}

	public override void Layout(IBlock block, Size availableSize)
	{
		if (block.Visible)
		{
			if (block is SuperTabItem)
			{
				_2031(block as SuperTabItem);
			}
			else if (block is BaseItem)
			{
				_2037(block as BaseItem);
			}
		}
	}

	private void _2031(SuperTabItem ᆰ)
	{
		Size size = default(Size);
		Rectangle empty = Rectangle.Empty;
		if (__1F5C.Width <= 0 || __1F5C.Height <= 0 || ᆰ.TextMarkupBody != null)
		{
			size = _2032(ᆰ);
		}
		if (ᆰ.FixedTabSize.Width > 0)
		{
			size.Width = ᆰ.FixedTabSize.Width;
		}
		else if (__1F5C.Width > 0)
		{
			size.Width = __1F5C.Width;
		}
		if (ᆰ.FixedTabSize.Height > 0)
		{
			size.Height = ᆰ.FixedTabSize.Height;
		}
		else if (__1F5C.Height > 0)
		{
			size.Height = __1F5C.Height;
		}
		Size ό = έ.Ό;
		if (size.Width < ό.Width)
		{
			size.Width = ό.Width;
		}
		if (size.Height < ό.Height)
		{
			size.Height = ό.Height;
		}
		empty.Size = (ᆰ.ᓙ ? new Size(size.Height, size.Width) : size);
		empty.Size = _ὗ.ᾯ(ᆰ, empty.Size, base.Graphics);
		ᆰ.Bounds = empty;
		ᆰ.RecalcSize();
	}

	private Size _2032(SuperTabItem ᆰ)
	{
		Size ߒ = new Size(0, 0);
		Font val = _2033(base.Graphics, ᆰ);
		Size size = ((ᆰ.TextMarkupBody != null) ? _2035(ᆰ, base.Graphics, val) : _2036(ᆰ, base.Graphics, val));
		size.Width += __202F.Width;
		size.Height += __202F.Height;
		bool flag = ᆰ.IsSelected || !_ὗ.DisplaySelectedTextOnly;
		if (!_ὗ.ᓙ || !_ὗ.HorizontalText)
		{
			if (flag)
			{
				ߒ.Width += size.Width + _ὗ.TabHorizontalSpacing;
			}
			if (size.Height > ߒ.Height)
			{
				ߒ.Height = size.Height;
			}
			ߒ.Width += έ._136F + έ._1FFF;
		}
		else
		{
			if (flag && size.Height > ߒ.Height)
			{
				ߒ.Height = size.Height;
			}
			ߒ.Height += έ._136F + έ._1FFF;
			ߒ.Width += size.Width + _ὗ.TabHorizontalSpacing + έ._1FFE;
		}
		ߒ = _2034(ᆰ, ߒ);
		if (έ.ῷ && ᆰ.CloseButtonVisible)
		{
			ߒ.Width += __202E.Width + _ὗ.TabHorizontalSpacing;
			if (ߒ.Height < __202E.Height + _ὗ.TabVerticalSpacing)
			{
				ߒ.Height = __202E.Height + _ὗ.TabVerticalSpacing;
			}
			if (ߒ.Height < __202E.Height + 4)
			{
				ߒ.Height = __202E.Height + 4;
			}
		}
		_ὗ.έ._2012(ref ߒ);
		return ߒ;
	}

	private Font _2033(Graphics _٥, SuperTabItem ᆰ)
	{
		if (ᆰ._1F47 == null)
		{
			Size size = Size.Empty;
			Font val = ᆰ._1F46;
			if (val != null)
			{
				size = ((ᆰ.TextMarkupBody != null) ? _2035(ᆰ, _٥, val) : _2036(ᆰ, _٥, val));
			}
			Size size2 = Size.Empty;
			Font ὂ = ᆰ.Ὂ;
			if (ὂ != null)
			{
				size2 = ((ᆰ.TextMarkupBody != null) ? _2035(ᆰ, _٥, ὂ) : _2036(ᆰ, _٥, ὂ));
			}
			ᆰ._1F47 = ((size.Width > size2.Width) ? val : ὂ);
		}
		return ᆰ._1F47;
	}

	private Size _2034(SuperTabItem ᆰ, Size ߒ)
	{
		Size size = Size.Empty;
		if (!string.IsNullOrEmpty(ᆰ.SymbolRealized))
		{
			size = ᆰ._᧗();
		}
		else
		{
			ᇎ ᇎ2 = ᆰ.ᕂ();
			if (ᇎ2 != null)
			{
				size = ᇎ2.ᇐ;
			}
		}
		if (!size.IsEmpty)
		{
			int num = _ὗ.TabVerticalSpacing + ᆰ.ImagePadding.Vertical;
			int num2 = _ὗ.TabHorizontalSpacing + ᆰ.ImagePadding.Horizontal;
			switch (ᆰ.ὅ)
			{
			case ImageAlignment.TopLeft:
			case ImageAlignment.TopCenter:
			case ImageAlignment.TopRight:
			case ImageAlignment.BottomLeft:
			case ImageAlignment.BottomCenter:
			case ImageAlignment.BottomRight:
				ߒ.Height += size.Height + num;
				break;
			case ImageAlignment.MiddleLeft:
			case ImageAlignment.MiddleRight:
				ߒ.Width += size.Width + num2;
				if (ߒ.Height < size.Height + num)
				{
					ߒ.Height = size.Height + num;
				}
				break;
			case ImageAlignment.MiddleCenter:
				if (ߒ.Width < size.Width + num2)
				{
					ߒ.Width = size.Width + num2;
				}
				if (ߒ.Height < size.Height + num)
				{
					ߒ.Height = size.Height + num;
				}
				break;
			}
			if (_ὗ.ᓙ && _ὗ.HorizontalText)
			{
				int num3 = size.Height + _ὗ.έ._136F + _ὗ.έ._1FFF + 4;
				if (ߒ.Height < num3)
				{
					ߒ.Height = num3;
				}
			}
		}
		return ߒ;
	}

	private Size _2035(SuperTabItem ᆰ, Graphics _٥, Font _0EE7)
	{
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, _0EE7, Color.Black, rightToLeft: false);
		Size availableSize = new Size(5000, 5000);
		ᆰ.TextMarkupBody._0CC4();
		ᆰ.TextMarkupBody.Measure(availableSize, markupDrawContext);
		markupDrawContext.RightToLeft = _ὗ.IsRightToLeft;
		ᆰ.TextMarkupBody._0CC6(new Rectangle(Point.Empty, ᆰ.TextMarkupBody.Bounds.Size), markupDrawContext);
		return ᆰ.TextMarkupBody.Bounds.Size;
	}

	private Size _2036(SuperTabItem ᆰ, Graphics _٥, Font _0EE7)
	{
		string text = (string.IsNullOrEmpty(ᆰ.Text) ? "M" : ᆰ.Text);
		return ᆰ.ᓙ ? TextDrawing.MeasureStringLegacy(_٥, text, _0EE7, Size.Empty, eTextFormat.Default) : TextDrawing.MeasureString(_٥, text, _0EE7, 0, eTextFormat.Default);
	}

	private void _2037(BaseItem _06EC)
	{
		Size size = default(Size);
		Rectangle empty = Rectangle.Empty;
		if (__1F5C.Width <= 0 || __1F5C.Height <= 0 || (_06EC is ButtonItem && ((ButtonItem)_06EC).FixedSize.IsEmpty))
		{
			_06EC.RecalcSize();
			size = _06EC.Bounds.Size;
			size.Width += __202F.Width;
			size.Height += __202F.Height;
			size.Width += _ὗ.ItemPadding.Horizontal;
			size.Height += _ὗ.ItemPadding.Vertical;
		}
		if (__1F5C.Width > 0)
		{
			size.Width = __1F5C.Width;
		}
		if (__1F5C.Height > 0)
		{
			size.Height = __1F5C.Height;
		}
		if (!(_06EC is ButtonItem) || ((ButtonItem)_06EC).FixedSize.IsEmpty)
		{
			empty.Size = size;
			_06EC.Bounds = empty;
		}
		if (!_06EC.Stretch)
		{
			__2030[_06EC] = size;
		}
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		if (lines.Count > 1)
		{
			int _2039 = _2044(lines);
			_2038(lines, _2039);
			_203D(containerBounds, lines);
			_204D(lines);
		}
		else
		{
			_203D(containerBounds, lines);
			_2043(containerBounds, lines);
			_2047(lines);
		}
		_ὗ.ὴ = lines.Count;
		__2030.Clear();
		return blocksBounds;
	}

	private void _2038(ArrayList _೨, int _2039)
	{
		__202D = 0;
		switch (_ὗ.TabAlignment)
		{
		case eTabStripAlignment.Left:
		case eTabStripAlignment.Top:
			_2039++;
			__202D = _೨.Count - _2039;
			break;
		default:
			__202D = _2039;
			break;
		}
		if (__202D > 0)
		{
			_203A(_೨, _2039);
		}
	}

	private void _203A(ArrayList _೨, int _0F0C)
	{
		Size[] array = _203B(_೨);
		Point ᓏ = default(Point);
		for (int i = 0; i < _೨.Count; i++)
		{
			int num = (i + _0F0C) % _೨.Count;
			if (_೨[num] is SerialContentLayoutManager._0CF0 obj)
			{
				_203C(obj, ᓏ);
				ᓏ.X += array[num].Width;
				ᓏ.Y += array[num].Height;
			}
		}
	}

	private Size[] _203B(ArrayList _೨)
	{
		Size[] array = new Size[_೨.Count];
		for (int i = 0; i < _೨.Count; i++)
		{
			if (_೨[i] is SerialContentLayoutManager._0CF0 obj && obj.ೱ[0] is IBlock block)
			{
				ref Size reference = ref array[i];
				reference = block.Bounds.Size;
			}
		}
		return array;
	}

	private void _203C(SerialContentLayoutManager._0CF0 _108F, Point ᓏ)
	{
		for (int i = 0; i < _108F.ೱ.Count; i++)
		{
			if (_108F.ೱ[i] is BaseItem baseItem)
			{
				switch (_ὗ.TabAlignment)
				{
				case eTabStripAlignment.Top:
				case eTabStripAlignment.Bottom:
					baseItem.TopInternal = ᓏ.Y;
					break;
				default:
					baseItem.LeftInternal = ᓏ.X;
					break;
				}
			}
		}
	}

	private void _203D(Rectangle _೦, ArrayList _೨)
	{
		for (int i = 0; i < _೨.Count; i++)
		{
			if (!(_೨[i] is SerialContentLayoutManager._0CF0 obj))
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < obj.ೱ.Count; j++)
			{
				if (obj.ೱ[j] is BaseItem { Visible: var displayed } baseItem)
				{
					if (obj.ೱ.Count > 1 && !_೦.Contains(baseItem.Bounds))
					{
						flag = false;
						displayed = false;
					}
					baseItem.Displayed = displayed;
					if (baseItem.Displayed)
					{
						_203E(baseItem);
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			Size ߒ;
			int num = _203F(obj, out ߒ);
			if (num > 0)
			{
				if (_ὗ.ᓙ)
				{
					_2040(obj, num, _೦.Height - ߒ.Height, ᔵ: true);
				}
				else
				{
					_2040(obj, num, _೦.Width - ߒ.Width, ᔵ: false);
				}
			}
		}
	}

	private void _203E(BaseItem _06EC)
	{
		Rectangle bounds = _06EC.Bounds;
		if (!_06EC.Stretch && __2030.TryGetValue(_06EC, out var value))
		{
			bounds.Size = value;
			bounds.Y = _06EC.Bounds.Y + (_06EC.Bounds.Height - bounds.Height) / 2;
			switch (_06EC.ItemAlignment)
			{
			case eItemAlignment.Center:
				bounds.X += (_06EC.Bounds.Width - bounds.Width) / 2;
				break;
			case eItemAlignment.Far:
				bounds.X = _06EC.Bounds.Right - bounds.Width;
				break;
			}
		}
		if (!(_06EC is SuperTabItem))
		{
			bounds.X += _ὗ.ItemPadding.Left;
			bounds.Y += _ὗ.ItemPadding.Top;
			bounds.Width -= _ὗ.ItemPadding.Horizontal;
			bounds.Height -= _ὗ.ItemPadding.Vertical;
		}
		_06EC.Bounds = bounds;
	}

	private int _203F(SerialContentLayoutManager._0CF0 _108F, out Size ߒ)
	{
		int num = 0;
		ߒ = Size.Empty;
		for (int i = 0; i < _108F.ೱ.Count; i++)
		{
			if (_108F.ೱ[i] is BaseItem { Displayed: not false } baseItem)
			{
				ߒ.Width = baseItem.Bounds.Right;
				ߒ.Height = baseItem.Bounds.Bottom;
				if (baseItem.BeginGroup)
				{
					num++;
				}
			}
		}
		return num;
	}

	private void _2040(SerialContentLayoutManager._0CF0 _108F, int _2041, int _2042, bool ᔵ)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = (float)_2042 / (float)_2041;
		for (int i = 0; i < _108F.ೱ.Count; i++)
		{
			if (!(_108F.ೱ[i] is BaseItem { Displayed: not false } baseItem))
			{
				continue;
			}
			if (baseItem.BeginGroup)
			{
				num2 = ((++num == _2041) ? ((float)_2042) : (num2 + num3));
			}
			if (num2 > 0f)
			{
				if (ᔵ)
				{
					baseItem.TopInternal += (int)num2;
				}
				else
				{
					baseItem.LeftInternal += (int)num2;
				}
			}
		}
	}

	private void _2043(Rectangle _೦, ArrayList _೨)
	{
		BaseItem ή = _ὗ.ή;
		if (ή == null || ή.Displayed || !(_೨[0] is SerialContentLayoutManager._0CF0 obj))
		{
			return;
		}
		for (int num = obj.ೱ.Count - 1; num >= 0; num--)
		{
			if (obj.ೱ[num] is BaseItem baseItem)
			{
				Rectangle rectangle = _ὗ.έ._2013(baseItem, ή);
				if (_೦.Contains(rectangle))
				{
					ή.Bounds = rectangle;
					ή.Displayed = true;
					break;
				}
				baseItem.Displayed = false;
				if (num == 0)
				{
					rectangle = baseItem.Bounds;
					rectangle.Size = ή.Size;
					ή.Bounds = rectangle;
					ή.Displayed = true;
				}
			}
		}
	}

	private int _2044(ArrayList _೨)
	{
		for (int i = 0; i < _೨.Count; i++)
		{
			if (_೨[i] is SerialContentLayoutManager._0CF0 obj)
			{
				int num = _2045(obj.ೱ);
				if (num >= 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int _2045(ArrayList _2046)
	{
		for (int i = 0; i < _2046.Count; i++)
		{
			if (_2046[i] is SuperTabItem superTabItem && superTabItem == _ὗ.SelectedTab)
			{
				return i;
			}
		}
		return -1;
	}

	private void _2047(ArrayList _೨)
	{
		if (_೨[0] is SerialContentLayoutManager._0CF0 obj)
		{
			switch (_ὗ.TabAlignment)
			{
			case eTabStripAlignment.Top:
				_2048(obj, 0, __202F.Height, 0, -__202F.Height);
				break;
			case eTabStripAlignment.Left:
				_2048(obj, __202F.Width, 0, -__202F.Width, 0);
				break;
			case eTabStripAlignment.Bottom:
				_204C(obj, 0, 2, 0, -__202F.Height);
				break;
			case eTabStripAlignment.Right:
				_204C(obj, 2, 0, -__202F.Width, 0);
				break;
			}
		}
	}

	private void _2048(SerialContentLayoutManager._0CF0 _108F, int _2049, int _204A, int _204B, int _0955)
	{
		for (int i = 0; i < _108F.ೱ.Count; i++)
		{
			if (_108F.ೱ[i] is BaseItem baseItem && !(_108F.ೱ[i] is SuperTabItem { IsSelected: not false }))
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.X += _2049;
				bounds.Y += _204A;
				bounds.Width += _204B;
				bounds.Height += _0955;
				baseItem.Bounds = bounds;
			}
		}
	}

	private void _204C(SerialContentLayoutManager._0CF0 _108F, int _2049, int _204A, int _204B, int _0955)
	{
		for (int i = 0; i < _108F.ೱ.Count; i++)
		{
			if (_108F.ೱ[i] is BaseItem { Bounds: var bounds } baseItem)
			{
				SuperTabItem superTabItem = _108F.ೱ[i] as SuperTabItem;
				bounds.X += _2049;
				bounds.Y += _204A;
				if (superTabItem == null || !superTabItem.IsSelected)
				{
					bounds.Width += _204B;
					bounds.Height += _0955;
				}
				baseItem.Bounds = bounds;
			}
		}
	}

	private void _204D(ArrayList _೨)
	{
		switch (_ὗ.TabAlignment)
		{
		case eTabStripAlignment.Top:
			_204E();
			break;
		case eTabStripAlignment.Left:
			_204F();
			break;
		case eTabStripAlignment.Bottom:
			_2050(_೨);
			break;
		case eTabStripAlignment.Right:
			_2051(_೨);
			break;
		}
	}

	private void _204E()
	{
		if (_ὗ.SelectedTab != null)
		{
			Rectangle bounds = _ὗ.SelectedTab.Bounds;
			bounds.Y -= __202F.Height;
			bounds.Height += __202F.Height;
			_ὗ.SelectedTab.Bounds = bounds;
		}
	}

	private void _204F()
	{
		if (_ὗ.SelectedTab != null)
		{
			Rectangle bounds = _ὗ.SelectedTab.Bounds;
			bounds.X -= __202F.Width;
			bounds.Width += __202F.Width;
			_ὗ.SelectedTab.Bounds = bounds;
		}
	}

	private void _2050(ArrayList _೨)
	{
		for (int i = 0; i < _೨.Count; i++)
		{
			if (!(_೨[i] is SerialContentLayoutManager._0CF0 obj))
			{
				continue;
			}
			for (int j = 0; j < obj.ೱ.Count; j++)
			{
				if (obj.ೱ[j] is BaseItem { Bounds: var bounds } baseItem)
				{
					bounds.Y += 2;
					if (obj.ೱ[j] is SuperTabItem { IsSelected: not false })
					{
						bounds.Height += __202F.Height;
					}
					baseItem.Bounds = bounds;
				}
			}
		}
	}

	private void _2051(ArrayList _೨)
	{
		for (int i = 0; i < _೨.Count; i++)
		{
			if (!(_೨[i] is SerialContentLayoutManager._0CF0 obj))
			{
				continue;
			}
			for (int j = 0; j < obj.ೱ.Count; j++)
			{
				if (obj.ೱ[j] is BaseItem { Bounds: var bounds } baseItem)
				{
					bounds.X += 2;
					if (obj.ೱ[j] is SuperTabItem { IsSelected: not false })
					{
						bounds.Width += __202F.Width;
					}
					baseItem.Bounds = bounds;
				}
			}
		}
	}
}
