using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display;

internal class ស
{
	internal void ហ(ColumnHeaderRendererEventArgs ؿ, ElementStyleDisplayInfo ឡ)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		if (ؿ.Tree != null && ؿ.Tree.CellHorizontalSpacing > 0 && !ؿ.ColumnHeader._171C)
		{
			Rectangle bounds = ឡ.Bounds;
			ឡ.Bounds = new Rectangle(bounds.X - ؿ.Tree.CellHorizontalSpacing, bounds.Y, bounds.Width + ؿ.Tree.CellHorizontalSpacing, bounds.Height);
			ElementStyleDisplay.Paint(ឡ);
			ឡ.Bounds = bounds;
		}
		else
		{
			ElementStyleDisplay.Paint(ឡ);
		}
		ឡ.Bounds.Inflate(-1, -1);
		if (ឡ.Bounds.Width <= 1 || ឡ.Bounds.Height <= 1)
		{
			return;
		}
		if (ؿ.ColumnHeader._171C)
		{
			Rectangle bounds2 = ឡ.Bounds;
			bounds2.Width -= 3;
			bounds2.X += 3;
			ឡ.Bounds = bounds2;
		}
		if (ؿ.ColumnHeader.SortDirection != 0 && !ؿ.SortIndicatorColor.IsEmpty)
		{
			GraphicsPath val = ឱ.ឲ(new Point(ឡ.Bounds.Right - 11, ឡ.Bounds.Y + (ឡ.Bounds.Height - 5) / 2), 9, (ؿ.ColumnHeader.SortDirection == eSortDirection.Ascending) ? _17B6._074A : _17B6.ራ);
			try
			{
				SmoothingMode smoothingMode = ؿ.Graphics.SmoothingMode;
				ؿ.Graphics.SmoothingMode = (SmoothingMode)0;
				SolidBrush val2 = new SolidBrush(ؿ.SortIndicatorColor);
				try
				{
					ؿ.Graphics.FillPath((Brush)(object)val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				ؿ.Graphics.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			ឡ.Bounds.Width -= 12;
		}
		if (ؿ.ColumnHeader.Image != null)
		{
			Image image = ؿ.ColumnHeader.Image;
			Rectangle bounds3 = ឡ.Bounds;
			if (ؿ.ColumnHeader.ImageAlignment == eColumnImageAlignment.Left)
			{
				ؿ.Graphics.DrawImage(image, bounds3.X, bounds3.Y + (bounds3.Height - image.Height) / 2, image.Width, image.Height);
				bounds3.X += image.Width + 2;
				bounds3.Width -= image.Width + 2;
			}
			else if (ؿ.ColumnHeader.ImageAlignment == eColumnImageAlignment.Right)
			{
				ؿ.Graphics.DrawImage(image, bounds3.Right - image.Width, bounds3.Y + (bounds3.Height - image.Height) / 2, image.Width, image.Height);
				bounds3.Width -= image.Width + 2;
			}
			ឡ.Bounds = bounds3;
		}
		ElementStyleDisplay.PaintText(ឡ, ؿ.ColumnHeader.Text, ((Control)ؿ.Tree).Font);
	}

	internal static void អ(Graphics _٥, AdvTree ᝢ, int ឣ, ColumnHeaderCollection _064B)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		if (ឣ == -1)
		{
			throw new ArgumentException("columnMoveMarkerIndex must be grater or equal than 0");
		}
		if (_064B == null)
		{
			throw new ArgumentNullException("columns");
		}
		Color color = ColorScheme.GetColor("834DD5");
		Color color2 = ColorScheme.GetColor("CCCFF8");
		Size size = new Size(10, 14);
		ColumnHeader columnHeader = null;
		columnHeader = ((ឣ != _064B.Count) ? _064B[ឣ] : _064B.LastVisibleColumn);
		Rectangle ឥ = Rectangle.Empty;
		ឥ = ((ឣ == _064B.Count) ? new Rectangle(columnHeader.Bounds.Right - size.Width, columnHeader.Bounds.Bottom - size.Height, size.Width, size.Height) : ((_064B[ឣ] != _064B.FirstVisibleColumn) ? new Rectangle(columnHeader.Bounds.X - size.Width / 2 - ᝢ.Ԫ.GetCellLayout()._070F._073C, columnHeader.Bounds.Bottom - size.Height, size.Width, size.Height) : new Rectangle(columnHeader.Bounds.X, columnHeader.Bounds.Bottom - size.Height, size.Width, size.Height)));
		if (ᝢ.AutoScrollPosition.X != 0)
		{
			ឥ.Offset(ᝢ.AutoScrollPosition.X, 0);
		}
		GraphicsPath val = ឤ(ឥ);
		try
		{
			SolidBrush val2 = new SolidBrush(color2);
			try
			{
				_٥.FillPath((Brush)(object)val2, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				_٥.DrawPath(val3, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static GraphicsPath ឤ(Rectangle ឥ)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		ឥ.Height--;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(ឥ.X + ឥ.Width / 2, ឥ.Bottom, ឥ.X, ឥ.Bottom - ឥ.Width / 2);
		val.AddLine(ឥ.X, ឥ.Bottom - ឥ.Width / 2, ឥ.X + ឥ.Width / 3, ឥ.Bottom - ឥ.Width / 2);
		val.AddLine(ឥ.X + ឥ.Width / 3, ឥ.Bottom - ឥ.Width / 2, ឥ.X + ឥ.Width / 3, ឥ.Y);
		val.AddLine(ឥ.X + ឥ.Width / 3, ឥ.Y, ឥ.Right - ឥ.Width / 3, ឥ.Y);
		val.AddLine(ឥ.Right - ឥ.Width / 3, ឥ.Y, ឥ.Right - ឥ.Width / 3, ឥ.Bottom - ឥ.Width / 2);
		val.AddLine(ឥ.Right - ឥ.Width / 3, ឥ.Bottom - ឥ.Width / 2, ឥ.Right, ឥ.Bottom - ឥ.Width / 2);
		val.CloseAllFigures();
		return val;
	}
}
