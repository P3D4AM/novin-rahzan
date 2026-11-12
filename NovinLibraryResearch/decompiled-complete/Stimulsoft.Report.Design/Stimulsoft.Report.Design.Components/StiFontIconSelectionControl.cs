using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components.FontIcons;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiFontIconSelectionControl : Control
{
	private StiFontIcons? selectedIcon;

	private string textFilter;

	private StiFontIcons? iconFocus;

	private bool customIconFocus;

	private bool showCustomIcon;

	private Rectangle customIconRectangle;

	private bool emptyIconFocus;

	private bool showEmptyIcon;

	private Rectangle emptyIconRectangle;

	private int iconWidth = StiScale.XXI(28.0);

	private int iconHeight = StiScale.YYI(28.0);

	private Font iconFont;

	private Hashtable iconsRectanglesHash = new Hashtable();

	public StiFontIcons? SelectedIcon
	{
		get
		{
			return selectedIcon;
		}
		set
		{
			selectedIcon = value;
			iconFocus = value;
			InvokeValueChanged();
		}
	}

	public string TextFilter
	{
		get
		{
			return textFilter;
		}
		set
		{
			if (textFilter != value)
			{
				textFilter = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Color IconColor { get; set; }

	private Color PreparedIconColor
	{
		get
		{
			Color color = IconColor.RemoveAlpha();
			if (color.R > 200 && color.G > 200 && color.B > 200)
			{
				return StiColorUtils.Dark(color, 50);
			}
			return color;
		}
	}

	public event EventHandler ValueChanged;

	public event EventHandler CustomIconClick;

	public StiFontIconSelectionControl(StiFontIcons? selectedIcon, Color iconColor, bool showCustomIcon = false, bool showEmptyIcon = false)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		((Control)this).ResizeRedraw = true;
		((Control)this).DoubleBuffered = true;
		iconFont = new Font(StiFontIconsHelper.GetFontFamilyIcons(), 13f);
		IconColor = iconColor;
		this.selectedIcon = selectedIcon;
		this.showCustomIcon = showCustomIcon;
		this.showEmptyIcon = showEmptyIcon;
	}

	protected void InvokeValueChanged()
	{
		this.ValueChanged?.Invoke(this, EventArgs.Empty);
	}

	protected void InvokeCustomIconClick()
	{
		this.CustomIconClick?.Invoke(this, EventArgs.Empty);
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		Font obj = iconFont;
		if (obj != null)
		{
			obj.Dispose();
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetBoundsCore(x, y, width, GetHeight(width), specified);
	}

	private void PaintGroupIcons(Graphics g, int x, int y)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		if (showEmptyIcon)
		{
			emptyIconRectangle = new Rectangle(x, y, iconWidth * 2, iconHeight);
			if (emptyIconFocus)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(230, 230, 230));
				try
				{
					g.FillRectangle((Brush)(object)val, emptyIconRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			SolidBrush val2 = new SolidBrush(PreparedIconColor);
			try
			{
				StringFormat val3 = new StringFormat
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)1
				};
				try
				{
					g.DrawString(StiLocalization.Get("ChartRibbon", "AxesTicksNone"), new Font("Arial", 10f), (Brush)(object)val2, (RectangleF)emptyIconRectangle, val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			x = StiScale.I2;
			y += iconHeight;
		}
		Array values = Enum.GetValues(typeof(StiFontIconGroup));
		for (int i = 0; i < values.Length; i++)
		{
			StiFontIconGroup iconGroup = (StiFontIconGroup)values.GetValue(i);
			Rectangle rectangle = new Rectangle(x + StiScale.I4, y, ((Control)this).Width, StiScale.I16);
			y += StiScale.I16;
			List<StiFontIcons> fontIcons = StiFontIconsHelper.GetFontIcons(iconGroup);
			int paintIconsCount = 0;
			PaintIcons(g, x, ref y, iconGroup, fontIcons, ref paintIconsCount);
			if (paintIconsCount > 0)
			{
				Pen val4 = new Pen((Brush)new SolidBrush(Color.FromArgb(230, 230, 230)));
				try
				{
					g.DrawLine(val4, rectangle.X, rectangle.Y + StiScale.I16, rectangle.X + ((Control)this).Width, rectangle.Y + StiScale.I16);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				SolidBrush val5 = new SolidBrush(Color.FromArgb(105, 105, 105));
				try
				{
					StringFormat val6 = new StringFormat
					{
						Alignment = (StringAlignment)0,
						LineAlignment = (StringAlignment)1
					};
					try
					{
						g.DrawString(StiFontIconsHelper.GetGroupIconsName(iconGroup), new Font("Microsoft Sans Serif", 8f), (Brush)(object)val5, (RectangleF)rectangle, val6);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			else
			{
				y -= StiScale.I16;
			}
			x = StiScale.I2;
		}
	}

	private void PaintIcons(Graphics g, int x, ref int y, StiFontIconGroup iconGroup, List<StiFontIcons> listIcons, ref int paintIconsCount)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		int num = y;
		for (int i = 0; i < listIcons.Count; i++)
		{
			StiFontIcons stiFontIcons = listIcons[i];
			if (!string.IsNullOrEmpty(TextFilter) && !stiFontIcons.ToString().ToLower().Contains(TextFilter))
			{
				continue;
			}
			Rectangle rectangle = new Rectangle(x, y, iconWidth, iconHeight);
			if (stiFontIcons == selectedIcon)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(210, 210, 210));
				try
				{
					g.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (stiFontIcons == iconFocus && !customIconFocus)
			{
				SolidBrush val2 = new SolidBrush(Color.FromArgb(230, 230, 230));
				try
				{
					g.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			SolidBrush val3 = new SolidBrush(PreparedIconColor);
			try
			{
				StringFormat val4 = new StringFormat
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)1
				};
				try
				{
					StiFontIconMetadata stiFontIconMetadata = new StiFontIconMetadata(iconGroup, stiFontIcons);
					if (!iconsRectanglesHash.Contains(stiFontIconMetadata))
					{
						iconsRectanglesHash[stiFontIconMetadata] = rectangle;
					}
					g.DrawString(StiFontIconsHelper.GetContent(stiFontIcons), iconFont, (Brush)(object)val3, (RectangleF)rectangle, val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			paintIconsCount++;
			x += iconWidth;
			if (x + StiScale.XXI(20.0) > ((Control)this).Width)
			{
				x = StiScale.I2;
				y += iconHeight;
			}
		}
		if (iconGroup == StiFontIconGroup.OtherIcons && showCustomIcon)
		{
			customIconRectangle = new Rectangle(x, y, iconWidth, iconHeight);
			if (customIconFocus)
			{
				SolidBrush val5 = new SolidBrush(Color.FromArgb(230, 230, 230));
				try
				{
					g.FillRectangle((Brush)(object)val5, customIconRectangle);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			SolidBrush val6 = new SolidBrush(PreparedIconColor);
			try
			{
				StringFormat val7 = new StringFormat
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)1
				};
				try
				{
					g.DrawString("...", iconFont, (Brush)(object)val6, (RectangleF)customIconRectangle, val7);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			paintIconsCount++;
		}
		int num2 = ((Control)this).Width / iconWidth;
		int num3 = (int)Math.Ceiling((double)paintIconsCount / (double)num2);
		y = num + num3 * iconHeight;
	}

	private int GetHeight(int width)
	{
		if (width <= 0)
		{
			return 0;
		}
		int num = 0;
		Array values = Enum.GetValues(typeof(StiFontIconGroup));
		for (int i = 0; i < values.Length; i++)
		{
			StiFontIconGroup stiFontIconGroup = (StiFontIconGroup)values.GetValue(i);
			List<StiFontIcons> fontIcons = StiFontIconsHelper.GetFontIcons(stiFontIconGroup);
			num += StiScale.I16;
			int num2 = fontIcons.Count;
			if (stiFontIconGroup == StiFontIconGroup.OtherIcons && showCustomIcon)
			{
				num2++;
			}
			if (stiFontIconGroup == StiFontIconGroup.OtherIcons && showEmptyIcon)
			{
				num2++;
			}
			num += GetHeight(width, num2);
		}
		return StiScale.I2 + num;
	}

	private int GetHeight(int width, int iconsCount)
	{
		if (width <= 0)
		{
			return 0;
		}
		int num = width / iconWidth;
		int num2 = (int)Math.Ceiling((double)iconsCount / (double)num);
		return num2 * iconHeight;
	}

	private void SetFocusIcon(int xPoin, int yPoint)
	{
		StiFontIcons? stiFontIcons = iconFocus;
		foreach (StiFontIconMetadata key in iconsRectanglesHash.Keys)
		{
			if (((Rectangle)iconsRectanglesHash[key]).Contains(xPoin, yPoint))
			{
				stiFontIcons = key.Icon;
				break;
			}
		}
		if (stiFontIcons != iconFocus)
		{
			iconFocus = stiFontIcons;
			((Control)this).Invalidate();
			return;
		}
		bool flag = customIconRectangle.Contains(xPoin, yPoint);
		if (customIconFocus != flag)
		{
			customIconFocus = flag;
			((Control)this).Invalidate();
			return;
		}
		bool flag2 = emptyIconRectangle.Contains(xPoin, yPoint);
		if (emptyIconFocus != flag2)
		{
			emptyIconFocus = flag2;
			((Control)this).Invalidate();
		}
	}

	private void SetSelectedIcon(int xPoin, int yPoint)
	{
		StiFontIcons? stiFontIcons = SelectedIcon;
		foreach (StiFontIconMetadata key in iconsRectanglesHash.Keys)
		{
			if (((Rectangle)iconsRectanglesHash[key]).Contains(xPoin, yPoint))
			{
				stiFontIcons = key.Icon;
				break;
			}
		}
		if (SelectedIcon != stiFontIcons)
		{
			SelectedIcon = stiFontIcons;
		}
		if (emptyIconRectangle.Contains(xPoin, yPoint))
		{
			SelectedIcon = null;
		}
		if (customIconRectangle.Contains(xPoin, yPoint))
		{
			InvokeCustomIconClick();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		SetFocusIcon(e.Location.X, e.Location.Y);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		iconFocus = null;
		((Control)this).Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		SetSelectedIcon(e.Location.X, e.Location.Y);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		int i = StiScale.I2;
		int i2 = StiScale.I2;
		PaintGroupIcons(graphics, i, i2);
	}
}
