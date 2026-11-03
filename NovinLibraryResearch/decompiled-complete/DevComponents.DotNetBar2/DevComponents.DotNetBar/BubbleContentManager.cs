using System;
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

public class BubbleContentManager : SerialContentLayoutManager
{
	private Size _281C = Size.Empty;

	private float _281D;

	private float _281E;

	private float _281F;

	private float _2820;

	private int _2299 = -1;

	private int _2821 = -1;

	public Size BubbleSize
	{
		get
		{
			return _281C;
		}
		set
		{
			_281C = value;
		}
	}

	public float Factor1
	{
		get
		{
			return _281D;
		}
		set
		{
			_281D = value;
		}
	}

	public float Factor2
	{
		get
		{
			return _281E;
		}
		set
		{
			_281E = value;
		}
	}

	public float Factor3
	{
		get
		{
			return _281F;
		}
		set
		{
			_281F = value;
		}
	}

	public float Factor4
	{
		get
		{
			return _2820;
		}
		set
		{
			_2820 = value;
		}
	}

	public int MouseOverIndex
	{
		get
		{
			return _2299;
		}
		set
		{
			_2299 = value;
		}
	}

	public int MouseOverPosition
	{
		get
		{
			return _2821;
		}
		set
		{
			_2821 = value;
		}
	}

	public BubbleContentManager()
	{
		ContentAlignment = eContentAlignment.Center;
	}

	public override Rectangle Layout(Rectangle containerBounds, IBlock[] contentBlocks, BlockLayoutManager blockLayout)
	{
		if (contentBlocks.Length == 0)
		{
			return Rectangle.Empty;
		}
		if (_2299 == -1)
		{
			return base.Layout(containerBounds, contentBlocks, blockLayout);
		}
		BubbleButton[] array = new BubbleButton[contentBlocks.Length];
		contentBlocks.CopyTo(array, 0);
		int و = 0;
		int ى = 0;
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			array[_2299].SetMagnifiedDisplayRectangle(new Rectangle(_2821, _282A(containerBounds, _281C.Height), _281C.Width, _281C.Height));
			و = _2821;
		}
		else
		{
			array[_2299].SetMagnifiedDisplayRectangle(new Rectangle(_2829(containerBounds, _281C.Width), _2821, _281C.Width, _281C.Height));
			ى = _2821;
		}
		int _2823 = _281C.Width - contentBlocks[0].Bounds.Width;
		int _2824 = _281C.Height - contentBlocks[0].Bounds.Height;
		int num = _2828(array, _2299);
		if (num >= 0)
		{
			_2822(containerBounds, array[num], _2823, _2824, _281E, ref و, ref ى);
		}
		num = _2828(array, num);
		if (num >= 0)
		{
			_2822(containerBounds, array[num], _2823, _2824, _281D, ref و, ref ى);
		}
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			while (num >= 0)
			{
				num = _2828(array, num);
				if (num >= 0)
				{
					و -= array[num].DisplayRectangle.Width + BlockSpacing;
					array[num].SetMagnifiedDisplayRectangle(new Rectangle(و, array[num].DisplayRectangle.Y, array[num].DisplayRectangle.Width, array[num].DisplayRectangle.Height));
				}
			}
			و = _2821 + _281C.Width + BlockSpacing;
		}
		else
		{
			while (num >= 0)
			{
				num = _2828(array, num);
				if (num >= 0)
				{
					ى -= array[num].DisplayRectangle.Height + BlockSpacing;
					array[num].SetMagnifiedDisplayRectangle(new Rectangle(array[num].DisplayRectangle.X, ى, array[num].DisplayRectangle.Width, array[num].DisplayRectangle.Height));
				}
			}
			ى = _2821 + _281C.Height + BlockSpacing;
		}
		num = _2826(array, _2299);
		if (num >= 0)
		{
			_2825(containerBounds, array[num], _2823, _2824, _281F, ref و, ref ى);
		}
		if (num == -1)
		{
			num = _2299;
		}
		num = _2826(array, num);
		if (num >= 0)
		{
			_2825(containerBounds, array[num], _2823, _2824, _2820, ref و, ref ى);
		}
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			while (num >= 0)
			{
				num = _2826(array, num);
				if (num >= 0)
				{
					array[num].SetMagnifiedDisplayRectangle(new Rectangle(و, array[num].DisplayRectangle.Y, array[num].DisplayRectangle.Width, array[num].DisplayRectangle.Height));
					و += array[num].DisplayRectangle.Width + BlockSpacing;
				}
			}
		}
		else
		{
			while (num >= 0)
			{
				num = _2826(array, num);
				if (num >= 0)
				{
					array[num].SetMagnifiedDisplayRectangle(new Rectangle(array[num].DisplayRectangle.X, ى, array[num].DisplayRectangle.Width, array[num].DisplayRectangle.Height));
					ى += array[num].DisplayRectangle.Height + BlockSpacing;
				}
			}
		}
		if (array.Length == 1)
		{
			return array[0].MagnifiedDisplayRectangle;
		}
		return Rectangle.Union(array[0].MagnifiedDisplayRectangle, array[^1].MagnifiedDisplayRectangle);
	}

	private void _2822(Rectangle _೦, BubbleButton ᤉ, int _2823, int _2824, float _0659, ref int و, ref int ى)
	{
		int num = (int)((float)ᤉ.DisplayRectangle.Width + (float)_2823 * _0659);
		int num2 = (int)((float)ᤉ.DisplayRectangle.Height + (float)_2824 * _0659);
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			و -= num + BlockSpacing;
			ᤉ.SetMagnifiedDisplayRectangle(new Rectangle(و, _282A(_೦, num2), num, num2));
		}
		else
		{
			ى -= num2 + BlockSpacing;
			ᤉ.SetMagnifiedDisplayRectangle(new Rectangle(_2829(_೦, num), ى, num, num2));
		}
	}

	private void _2825(Rectangle _೦, BubbleButton ᤉ, int _2823, int _2824, float _0659, ref int و, ref int ى)
	{
		int num = (int)((float)ᤉ.DisplayRectangle.Width + (float)_2823 * _0659);
		int num2 = (int)((float)ᤉ.DisplayRectangle.Height + (float)_2824 * _0659);
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			ᤉ.SetMagnifiedDisplayRectangle(new Rectangle(و, _282A(_೦, num2), num, num2));
			و += num + BlockSpacing;
		}
		else
		{
			ᤉ.SetMagnifiedDisplayRectangle(new Rectangle(_2829(_೦, num), ى, num, num2));
			ى += num2 + BlockSpacing;
		}
	}

	private int _2826(BubbleButton[] _2827, int ԡ)
	{
		int result = -1;
		for (int i = ԡ + 1; i < _2827.Length; i++)
		{
			if (_2827[i].Visible)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private int _2828(BubbleButton[] _2827, int ԡ)
	{
		int result = -1;
		for (int num = ԡ - 1; num >= 0; num--)
		{
			if (_2827[num].Visible)
			{
				result = num;
				break;
			}
		}
		return result;
	}

	private int _2829(Rectangle _೦, int _९)
	{
		if (ContentOrientation == eContentOrientation.Vertical)
		{
			if (ContentVerticalAlignment == eContentVerticalAlignment.Top)
			{
				return _೦.Left;
			}
			if (ContentVerticalAlignment == eContentVerticalAlignment.Middle)
			{
				return _೦.Left + (_೦.Width - _९) / 2;
			}
			if (ContentVerticalAlignment == eContentVerticalAlignment.Bottom)
			{
				return _೦.Right - _९;
			}
			return -1;
		}
		throw new InvalidOperationException("Cannot use GetX method on eContentOrientation.Horizontal");
	}

	private int _282A(Rectangle _೦, int _0970)
	{
		if (ContentOrientation == eContentOrientation.Horizontal)
		{
			if (ContentVerticalAlignment == eContentVerticalAlignment.Top)
			{
				return _೦.Top;
			}
			if (ContentVerticalAlignment == eContentVerticalAlignment.Middle)
			{
				return _೦.Top + (_೦.Height - _0970) / 2;
			}
			if (ContentVerticalAlignment == eContentVerticalAlignment.Bottom)
			{
				return _೦.Bottom - _0970;
			}
			return -1;
		}
		throw new InvalidOperationException("Cannot use GetX method on eContentOrientation.Vertical");
	}
}
