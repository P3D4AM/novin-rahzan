using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiListBox : ListBox
{
	private bool isFormBackground;

	private bool useFirstImage;

	private bool hotTrack;

	private ImageList imageList;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override DrawMode DrawMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ListBox)this).DrawMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((ListBox)this).DrawMode = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!IsFormBackground)
			{
				return StiUX.InputBackground;
			}
			return StiUX.Background;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).BackColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFormBackground
	{
		get
		{
			return isFormBackground;
		}
		set
		{
			isFormBackground = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool UseFirstImage
	{
		get
		{
			return useFirstImage;
		}
		set
		{
			useFirstImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool HotTrack
	{
		get
		{
			return hotTrack;
		}
		set
		{
			hotTrack = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(null)]
	public ImageList ImageList
	{
		get
		{
			return imageList;
		}
		set
		{
			imageList = value;
			((Control)this).Invalidate();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		FieldInfo field = typeof(ListBox).GetField("EVENT_DRAWITEM", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
		DrawItemEventHandler val = (DrawItemEventHandler)((Component)this).Events[field.GetValue(null)];
		if (val == null)
		{
			Graphics graphics = e.Graphics;
			Rectangle bounds = e.Bounds;
			if ((e.State & 1) > 0)
			{
				bounds.Width--;
			}
			DrawItemState state = e.State;
			if (e.Index != -1 && ((ListBox)this).Items.Count > 0)
			{
				int imageIndex = ((!UseFirstImage) ? e.Index : 0);
				StiControlPaint.DrawItem(graphics, bounds, state, ((ListControl)this).GetItemText(((ListBox)this).Items[e.Index]), ImageList, imageIndex, ((Control)this).Font, ((Control)this).BackColor, ((Control)this).ForeColor, ((Control)this).RightToLeft);
			}
		}
		else
		{
			val.Invoke((object)this, e);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (!HotTrack)
		{
			return;
		}
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		for (int i = 0; i < ((ListBox)this).Items.Count; i++)
		{
			if (((ListBox)this).GetItemRectangle(i).Contains(pt))
			{
				((ListControl)this).SelectedIndex = i;
				break;
			}
		}
	}

	public StiListBox()
	{
		((ListBox)this).DrawMode = (DrawMode)2;
		((ListBox)this).ItemHeight = (int)Math.Ceiling(19.0 * StiScaleUI.Y);
	}
}
