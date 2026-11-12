using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiTreeStateCheckBox : CheckBox
{
	private StiControlColorCollection colors;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	private bool IsHot => new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height).Contains(((Control)this).PointToClient(Cursor.Position));

	protected override void OnPaint(PaintEventArgs e)
	{
		((ButtonBase)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		DrawBackground(graphics);
		DrawCheck(graphics);
		DrawText(graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnMouseMove(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseMove(mevent);
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ButtonBase)this).OnMouseEnter(e);
		((Control)this).Invalidate();
	}

	private void DrawText(Graphics g)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Color color = (IsHot ? Colors.HotForeColor : Colors.ForeColor);
		TextFormatFlags val = (TextFormatFlags)32804;
		Rectangle textRect = GetTextRect();
		TextRenderer.DrawText((IDeviceContext)(object)g, ((Control)this).Text, ((Control)this).Font, textRect, color, val);
	}

	private void DrawBackground(Graphics g)
	{
		StiDrawing.FillRectangle(rect: new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height), g: g, color: IsHot ? Colors.HotBackColor : Colors.BackColor);
	}

	private void DrawCheck(Graphics g)
	{
		Color color = (IsHot ? Colors.HotGlyphColor : Colors.GlyphColor);
		Rectangle checkedRect = GetCheckedRect();
		StiCheckPainter.PaintCheckBox(g, checkedRect, GetCheckState(), color);
	}

	private StiCheckState GetCheckState()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		CheckState checkState = ((CheckBox)this).CheckState;
		if ((int)checkState != 0)
		{
			if ((int)checkState == 1)
			{
				return StiCheckState.Checked;
			}
			return StiCheckState.Indeterminate;
		}
		return StiCheckState.Unchecked;
	}

	private Rectangle GetCheckedRect()
	{
		int checkSize = StiCheckPainter.GetCheckSize();
		return new Rectangle(StiScale.I4, (((Control)this).Height - checkSize) / 2 - (int)StiScale.Y, checkSize, checkSize);
	}

	private Rectangle GetTextRect()
	{
		Rectangle checkedRect = GetCheckedRect();
		return new Rectangle(checkedRect.Right + StiScale.I4, 0, ((Control)this).Width - checkedRect.Width - StiScale.I6, ((Control)this).Height);
	}

	public StiTreeStateCheckBox()
	{
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		Colors = new StiControlColorCollection((Control)(object)this);
	}
}
