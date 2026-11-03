using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

public class PropertyNodeColor : PropertyNode
{
	private Image __2EB6;

	private Color __2EB7 = ColorScheme.GetColor(12961221);

	private Color __2E3E = Color.Black;

	private Color __2EB8 = Color.White;

	protected override bool IsTypeEditorPaintValueSupported => false;

	private Color _2EB4 => __2EB7;

	private Color ⸯ => __2E3E;

	private Color _2EB5 => __2EB8;

	public PropertyNodeColor(PropertyDescriptor property)
		: base(property)
	{
	}

	protected override void UpdateDisplayedValue(object propertyValue, bool refreshSubProperties)
	{
		base.UpdateDisplayedValue(propertyValue, refreshSubProperties);
		if (base.ம || base.ᗘ)
		{
			return;
		}
		Cell editCell = EditCell;
		if (propertyValue is Color color)
		{
			if (__2EB6 == null || !((Color)__2EB6.Tag).Equals((object?)color))
			{
				_2EBA();
				__2EB6 = _2EB9(color);
				editCell.Images.Image = __2EB6;
			}
		}
		else
		{
			_2EBA();
			editCell.Images.Image = null;
		}
	}

	private Image _2EB9(Color _065B)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(0, 0, 20, 13);
		Bitmap val = new Bitmap(rectangle.Width, rectangle.Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			DisplayHelp.DrawRectangle(val2, ⸯ, rectangle);
			rectangle.Inflate(-1, -1);
			if (_065B == Color.Empty)
			{
				HatchBrush val3 = new HatchBrush((HatchStyle)3, _2EB4, _2EB5);
				try
				{
					val2.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			else if (_065B == Color.Transparent || _065B.A < byte.MaxValue)
			{
				HatchBrush val4 = new HatchBrush((HatchStyle)50, _2EB4, _2EB5);
				try
				{
					val2.FillRectangle((Brush)(object)val4, rectangle);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				if (_065B != Color.Transparent)
				{
					SolidBrush val5 = new SolidBrush(_065B);
					try
					{
						val2.FillRectangle((Brush)(object)val5, rectangle);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val6 = new SolidBrush(_065B);
				try
				{
					val2.FillRectangle((Brush)(object)val6, rectangle);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		((Image)val).Tag = _065B;
		return (Image)(object)val;
	}

	private void _2EBA()
	{
		if (__2EB6 != null)
		{
			__2EB6.Dispose();
			__2EB6 = null;
			Cell editCell = EditCell;
			if (editCell != null)
			{
				editCell.Images.Image = null;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		_2EBA();
		base.Dispose(disposing);
	}
}
