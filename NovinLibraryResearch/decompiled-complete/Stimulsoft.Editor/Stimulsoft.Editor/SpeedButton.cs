using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class SpeedButton : Control
{
	private const int DefaultButtonSize = 18;

	private IPainter painter;

	private ImageList images;

	private bool hot;

	private int imageIndex = -1;

	private EditBorderStyle borderStyle;

	protected bool Hot
	{
		get
		{
			return hot;
		}
		set
		{
			if (hot != value)
			{
				hot = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected bool Pressed
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			if (hot)
			{
				return (int)Control.MouseButtons == 1048576;
			}
			return false;
		}
	}

	public ImageList ImageList
	{
		get
		{
			return images;
		}
		set
		{
			if (images != value)
			{
				images = value;
				((Control)this).Invalidate();
			}
		}
	}

	public int ImageIndex
	{
		get
		{
			return imageIndex;
		}
		set
		{
			if (imageIndex != value)
			{
				imageIndex = value;
				((Control)this).Invalidate();
			}
		}
	}

	public EditBorderStyle BorderStyle
	{
		get
		{
			return borderStyle;
		}
		set
		{
			if (borderStyle != value)
			{
				borderStyle = value;
				((Control)this).Invalidate();
			}
		}
	}

	public SpeedButton(IContainer container)
		: this()
	{
		container?.Add((IComponent?)this);
	}

	public SpeedButton()
	{
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).Width = 18;
		((Control)this).Height = 18;
		painter = new GdiPainter();
		((Control)this).SetStyle((ControlStyles)14082, true);
		((Control)this).SetStyle((ControlStyles)4, false);
		((Control)this).Cursor = Cursors.Default;
		((Control)this).BackColor = SystemColors.Control;
	}

	private Color GetBkColor()
	{
		EditBorderStyle editBorderStyle = borderStyle;
		if (editBorderStyle == EditBorderStyle.None || editBorderStyle == EditBorderStyle.FixedSingle)
		{
			if (Pressed)
			{
				return Consts.DefaultPressedBackColor;
			}
			if (Hot)
			{
				return Consts.DefaultHotTrackBackColor;
			}
		}
		return ((Control)this).BackColor;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = ((Control)this).ClientRectangle;
		painter.BeginPaint(e.Graphics);
		try
		{
			switch (borderStyle)
			{
			case EditBorderStyle.None:
				if (hot)
				{
					painter.BackColor = Consts.DefaultHotTrackColor;
					painter.DrawRectangle(new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1));
					rect.Inflate(-1, -1);
				}
				break;
			case EditBorderStyle.FixedSingle:
				painter.BackColor = Consts.DefaultWindowFrameColor;
				painter.DrawRectangle(new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1));
				rect.Inflate(-1, -1);
				break;
			case EditBorderStyle.Fixed3D:
			case EditBorderStyle.System:
				if (borderStyle == EditBorderStyle.System && XPThemes.CurrentTheme != 0)
				{
					IntPtr rgn = painter.SaveClip(rect);
					try
					{
						painter.ExcludeClipRect(2, 2, ((Control)this).Width - 2, ((Control)this).Height - 2);
						XPThemes.DrawEditBorder(painter, rect);
					}
					finally
					{
						painter.RestoreClip(rgn);
					}
					rect.Inflate(-2, -2);
				}
				else
				{
					Rectangle rectangle = rect;
					painter.DrawEdge(ref rect, (Border3DStyle)(Pressed ? 10 : 5), (Border3DSide)2063);
					rect = rectangle;
					rect.Inflate(-2, -2);
				}
				break;
			}
			painter.BackColor = GetBkColor();
			painter.FillRectangle(rect);
			if (images != null)
			{
				Rectangle rect2 = new Rectangle(Point.Empty, images.ImageSize);
				int x = ((Control)this).ClientRectangle.Left + (rect.Width - rect2.Width) / 2;
				int y = ((Control)this).ClientRectangle.Top + (rect.Height - rect2.Height) / 2;
				rect2.Offset(x, y);
				rect2.Intersect(rect);
				painter.DrawImage(images, imageIndex, rect2);
			}
		}
		finally
		{
			painter.EndPaint();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		Hot = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		Hot = false;
	}
}
