using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class EditRuler : Control, IEditRuler, IControl
{
	private enum RulerHitTest
	{
		None,
		LeftIndent,
		RightIndent,
		Page
	}

	private int markWidth;

	private int pageStart;

	private int pageWidth;

	private int rulerStart;

	private int rulerWidth;

	private RulerOptions options;

	private RulerUnits units;

	private bool vertical;

	private RulerIndent leftIndent;

	private RulerIndent rightIndent;

	private Pen linePen;

	private int drawX = -1;

	private Size dpi;

	private Color indentBackColor;

	private const int cRulerTop = 6;

	private const int cRulerHeight = 16;

	private const int cTenthMMPerInch = 254;

	private const int cPointsPerInt = 4;

	[Description("Gets or sets default distance between adjacent marks.")]
	public int MarkWidth
	{
		get
		{
			return markWidth;
		}
		set
		{
			if (markWidth != value)
			{
				markWidth = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets beginning of the page part of the ruler.")]
	public int PageStart
	{
		get
		{
			return pageStart;
		}
		set
		{
			if (pageStart != value)
			{
				pageStart = value;
				UpdateIndents();
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets width, in pixels between left and right indentations indicating width of the page.")]
	public int PageWidth
	{
		get
		{
			return pageWidth;
		}
		set
		{
			if (pageWidth != value)
			{
				pageWidth = value;
				UpdateIndents();
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets position, in pixels, of left indentation specifying start of the page.")]
	public int RulerStart
	{
		get
		{
			return rulerStart;
		}
		set
		{
			if (rulerStart != value)
			{
				rulerStart = value;
				UpdateIndents();
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets ruler width.")]
	public int RulerWidth
	{
		get
		{
			return rulerWidth;
		}
		set
		{
			if (rulerWidth != value)
			{
				rulerWidth = value;
				UpdateIndents();
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets \"RulerOptions\" determining ruler behaviour.")]
	public RulerOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			if (options != value)
			{
				options = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets ruler measurement units.")]
	public RulerUnits Units
	{
		get
		{
			return units;
		}
		set
		{
			if (units != value)
			{
				units = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets background color of indentation parts of the ruler.")]
	public Color IndentBackColor
	{
		get
		{
			return indentBackColor;
		}
		set
		{
			if (indentBackColor != value)
			{
				indentBackColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets a boolean value indicating whether ruler has vertical or horizontal direction.")]
	public bool Vertical
	{
		get
		{
			return vertical;
		}
		set
		{
			if (vertical != value)
			{
				vertical = value;
				UpdateRuler();
			}
		}
	}

	[Description("Indicates whether some of ruler indentation is in dragging state.")]
	public bool IsDragging
	{
		get
		{
			if (!leftIndent.Dragging)
			{
				return rightIndent.Dragging;
			}
			return true;
		}
	}

	[Description("Occurs when some of ruler indentations has changed its position due to the dragging operation.")]
	public event EventHandler Change;

	public EditRuler()
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		vertical = false;
		pageStart = 85;
		pageWidth = 425;
		rulerStart = 24;
		rulerWidth = 600;
		markWidth = 8;
		units = EditConsts.DefaultRulerUnits;
		options = EditConsts.DefaultRulerOptions;
		leftIndent = new RulerIndent(IndentOrientation.Near, pageStart - rulerStart);
		rightIndent = new RulerIndent(IndentOrientation.Far, rulerWidth - pageWidth - (pageStart - rulerStart));
		((Control)this).Width = 200;
		((Control)this).Height = EditConsts.DefaultRulerHeight;
		((Control)this).Font = new Font(EditConsts.DefaultRulerFontName, EditConsts.DefaultRulerFontSize);
		((Control)this).Cursor = Cursors.Default;
		((Control)this).SetStyle((ControlStyles)73730, true);
		linePen = new Pen(Color.Black, 1f);
		linePen.DashStyle = (DashStyle)4;
		dpi = OSUtils.GetScreenCaps();
		((Control)this).BackColor = EditConsts.DefaultRulerBackColor;
		IndentBackColor = EditConsts.DefaultRulerIndentBackColor;
		UpdateRuler();
	}

	~EditRuler()
	{
		try
		{
			linePen.Dispose();
		}
		finally
		{
			((Component)this).Finalize();
		}
	}

	public void Assign(IEditRuler source)
	{
		Vertical = source.Vertical;
		PageStart = source.PageStart;
		PageWidth = source.PageWidth;
		RulerStart = source.RulerStart;
		RulerWidth = source.RulerWidth;
		MarkWidth = source.MarkWidth;
		Units = source.Units;
		Options = source.Options;
	}

	private Rectangle GetLeftRulerRect()
	{
		if (vertical)
		{
			return new Rectangle(6, rulerStart, 16, pageStart - rulerStart);
		}
		return new Rectangle(rulerStart, 6, pageStart - rulerStart, 16);
	}

	private Rectangle GetRightRulerRect()
	{
		if (vertical)
		{
			return new Rectangle(6, pageStart, 16, rulerWidth - (pageStart - rulerStart));
		}
		return new Rectangle(pageStart, 6, rulerWidth - (pageStart - rulerStart), 16);
	}

	private Rectangle GetPageRect()
	{
		if (vertical)
		{
			return new Rectangle(6, pageStart, 16, pageWidth);
		}
		return new Rectangle(pageStart, 6, pageWidth, 16);
	}

	private Rectangle GetRulerRect()
	{
		if (vertical)
		{
			return new Rectangle(6, rulerStart, 16, rulerWidth);
		}
		return new Rectangle(rulerStart, 6, rulerWidth, 16);
	}

	private Rectangle GetIndentHitRect(RulerIndent Indent)
	{
		Rectangle empty = Rectangle.Empty;
		return vertical ? ((Indent.Orientation != 0) ? new Rectangle(6, pageStart + pageWidth - EditConsts.DefaultRulerHitWidth, 16, EditConsts.DefaultRulerHitWidth * 2) : new Rectangle(6, rulerStart + Indent.Indent - EditConsts.DefaultRulerHitWidth, 16, EditConsts.DefaultRulerHitWidth * 2)) : ((Indent.Orientation != 0) ? new Rectangle(pageStart + pageWidth - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 16) : new Rectangle(rulerStart + Indent.Indent - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 16));
	}

	private Rectangle GetIndentRect(RulerIndent indent)
	{
		Rectangle empty = Rectangle.Empty;
		return vertical ? ((indent.Orientation != 0) ? new Rectangle(6, pageWidth + pageStart, 16, indent.Indent) : new Rectangle(6, rulerStart, 16, indent.Indent)) : ((indent.Orientation != 0) ? new Rectangle(pageWidth + pageStart, 6, indent.Indent, 16) : new Rectangle(rulerStart, 6, indent.Indent, 16));
	}

	private int GetIndentPos(RulerIndent ind)
	{
		if (ind.Orientation == IndentOrientation.Near)
		{
			return rulerStart + ind.Indent;
		}
		return rulerStart + rulerWidth - ind.Indent;
	}

	private RulerHitTest GetRulerHitTest(int x, int y)
	{
		Point pt = new Point(x, y);
		if (GetIndentRect(leftIndent).Contains(pt))
		{
			return RulerHitTest.LeftIndent;
		}
		if (GetIndentRect(rightIndent).Contains(pt))
		{
			return RulerHitTest.RightIndent;
		}
		if (GetPageRect().Contains(pt))
		{
			return RulerHitTest.Page;
		}
		return RulerHitTest.None;
	}

	private int GetDiscreteInterval(int x, int prevIndent)
	{
		int num = x;
		int num2 = 0;
		int num3 = (vertical ? dpi.Height : dpi.Width);
		if ((options & RulerOptions.Discrete) != 0)
		{
			switch (units)
			{
			case RulerUnits.Milimeters:
				num2 = ((x <= prevIndent) ? ((int)Math.Ceiling((double)(num * 254 * 4 / (num3 * 100)))) : ((int)Math.Floor((double)(num * 254 * 4 / (num3 * 100)))));
				num = (int)Math.Round((double)(num2 * num3 * 100 / 1016));
				break;
			case RulerUnits.Inches:
				num2 = ((x <= prevIndent) ? ((int)Math.Ceiling((double)(num * 4 / num3))) : ((int)Math.Floor((double)(num * 4 / num3))));
				num = (int)Math.Round((double)(num2 * num3 / 4));
				break;
			}
		}
		return num;
	}

	private float GetRulerStep()
	{
		int num = (vertical ? dpi.Height : dpi.Width);
		return units switch
		{
			RulerUnits.Milimeters => (float)num * 100f / 1016f, 
			RulerUnits.Inches => (float)num / 4f, 
			_ => markWidth, 
		};
	}

	private void UpdateRuler()
	{
		if (vertical)
		{
			((Control)this).Width = EditConsts.DefaultRulerHeight;
		}
		else
		{
			((Control)this).Height = EditConsts.DefaultRulerHeight;
		}
		UpdateIndents();
		((Control)this).Invalidate();
	}

	private void UpdateIndents()
	{
		leftIndent.Indent = pageStart - rulerStart;
		rightIndent.Indent = rulerWidth - pageWidth - (pageStart - rulerStart);
	}

	private void IndentUp(RulerIndent Indent)
	{
		Indent.Dragging = false;
		if (drawX != -1)
		{
			DrawLine(drawX, erase: true);
		}
	}

	private void IndentDown(RulerIndent indent)
	{
		indent.Dragging = true;
	}

	private void IndentMove(RulerIndent indent, Point pt)
	{
		int val = (vertical ? pt.Y : pt.X);
		if (indent.Orientation == IndentOrientation.Near)
		{
			val = Math.Min(val, pageStart + pageWidth - EditConsts.DefaultRulerMinWidth);
			val = Math.Max(val - rulerStart, EditConsts.DefaultRulerIndentSize);
			indent.Indent = GetDiscreteInterval(val, indent.Indent);
			pageWidth += pageStart - rulerStart - indent.Indent;
			pageStart = rulerStart + indent.Indent;
		}
		else
		{
			val = Math.Max(val, pageStart + EditConsts.DefaultRulerMinWidth);
			val = Math.Max(rulerStart + rulerWidth - val, EditConsts.DefaultRulerIndentSize);
			indent.Indent = GetDiscreteInterval(val, indent.Indent);
			pageWidth = rulerWidth - indent.Indent - (pageStart - rulerStart);
		}
		val = GetIndentPos(indent);
		if (drawX != val)
		{
			if (drawX != -1)
			{
				DrawLine(drawX, erase: true);
			}
			DrawLine(val, erase: false);
		}
		((Control)this).Invalidate();
	}

	private void DrawLine(int x, bool erase)
	{
		if ((options & RulerOptions.DisplayDragLine) == 0 || ((Control)this).Parent == null)
		{
			return;
		}
		drawX = x;
		if (((Control)this).Parent == null)
		{
			return;
		}
		x += (Vertical ? ((Control)this).Top : ((Control)this).Left);
		Point point = (vertical ? new Point(0, x) : new Point(x, 0));
		Point point2 = (vertical ? new Point(((Control)this).Parent.ClientSize.Width, x) : new Point(x, ((Control)this).Parent.ClientSize.Height));
		Size size = (vertical ? new Size(((Control)this).Parent.ClientSize.Width, 1) : new Size(1, ((Control)this).Parent.ClientSize.Height));
		if (erase)
		{
			((Control)this).Parent.Invalidate(new Rectangle(point, size));
			return;
		}
		Graphics val = ((Control)this).Parent.CreateGraphics();
		try
		{
			val.DrawLine(linePen, point, point2);
		}
		finally
		{
			val.Dispose();
		}
	}

	private void DrawRuler(Graphics graphics, bool direction, Rectangle rect)
	{
		if (vertical)
		{
			graphics.RotateTransform(-90f);
			graphics.TranslateTransform(0f, (float)(rect.Top + rect.Bottom), (MatrixOrder)1);
		}
		float rulerStep = GetRulerStep();
		int num = (vertical ? rect.Top : rect.Left);
		int num2 = (vertical ? rect.Bottom : rect.Right);
		float num3 = (direction ? num : (num2 - (int)((Control)this).Font.Size / 2));
		int num4 = 1;
		int num5 = 0;
		string empty = string.Empty;
		PointF empty2 = PointF.Empty;
		PointF empty3 = PointF.Empty;
		SizeF empty4 = SizeF.Empty;
		while (direction ? (num3 < (float)num2 - rulerStep - (float)((int)((Control)this).Font.Size / 2)) : (num3 > (float)num + rulerStep))
		{
			num3 = (direction ? (num3 + rulerStep) : (num3 - rulerStep));
			int num6 = ((num4 % 4 == 0) ? ((Control)this).Font.Height : ((num4 % 2 != 0) ? 1 : (vertical ? (rect.Width / 3) : (rect.Height / 3))));
			num5 = (vertical ? (rect.Left + (rect.Width - num6) / 2) : (rect.Top + (rect.Height - num6) / 2));
			empty2 = new PointF(num3, num5);
			empty3 = new PointF(num3, num5 + num6);
			if (num4 % 4 == 0)
			{
				empty = (num4 / 4).ToString();
				empty4 = graphics.MeasureString(empty, ((Control)this).Font);
				empty2.X -= (int)(empty4.Width / 2f);
				graphics.DrawString(empty, ((Control)this).Font, Brushes.Black, empty2);
			}
			else
			{
				graphics.DrawLine(Pens.Black, empty2, empty3);
			}
			num4++;
		}
		graphics.ResetTransform();
	}

	private bool CheckCursor(Point pt)
	{
		if ((options & RulerOptions.AllowDrag) != 0 && (GetIndentHitRect(leftIndent).Contains(pt) || GetIndentHitRect(rightIndent).Contains(pt)))
		{
			Cursor val = (vertical ? Cursors.SizeNS : Cursors.SizeWE);
			OSUtils.SetCursor(val.Handle);
			return true;
		}
		return false;
	}

	protected virtual void OnChange(object sender)
	{
		if (this.Change != null)
		{
			this.Change(this, new RulerEventArgs(sender));
		}
	}

	protected override void WndProc(ref Message m)
	{
		int msg = ((Message)(ref m)).Msg;
		if (msg == 32 && CheckCursor(((Control)this).PointToClient(Control.MousePosition)))
		{
			((Message)(ref m)).Result = (IntPtr)1;
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		if (((Control)this).Parent != null && ((Control)this).Parent.CanFocus)
		{
			((Control)this).Parent.Focus();
		}
		drawX = -1;
		if ((options & RulerOptions.AllowDrag) != 0)
		{
			Point pt = new Point(e.X, e.Y);
			if (GetIndentHitRect(leftIndent).Contains(pt))
			{
				IndentDown(leftIndent);
			}
			else if (GetIndentHitRect(rightIndent).Contains(pt))
			{
				IndentDown(rightIndent);
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		Point pt = new Point(e.X, e.Y);
		if (leftIndent.Dragging)
		{
			IndentMove(leftIndent, pt);
		}
		if (rightIndent.Dragging)
		{
			IndentMove(rightIndent, pt);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (leftIndent.Dragging)
		{
			OnChange(leftIndent);
		}
		IndentUp(leftIndent);
		if (rightIndent.Dragging)
		{
			OnChange(rightIndent);
		}
		IndentUp(rightIndent);
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		Graphics graphics = pe.Graphics;
		graphics.FillRectangle(SystemBrushes.Window, GetPageRect());
		leftIndent.DrawIndent(graphics, GetIndentRect(leftIndent), vertical, IndentBackColor, ((Control)this).BackColor);
		rightIndent.DrawIndent(graphics, GetIndentRect(rightIndent), vertical, IndentBackColor, ((Control)this).BackColor);
		DrawRuler(graphics, vertical, GetLeftRulerRect());
		DrawRuler(graphics, !vertical, GetRightRulerRect());
	}

	public virtual void CancelDragging()
	{
		if (leftIndent.Dragging)
		{
			leftIndent.CancelDragging();
			pageWidth += pageStart - rulerStart - leftIndent.Indent;
			pageStart = rulerStart + leftIndent.Indent;
			if (drawX != -1)
			{
				DrawLine(drawX, erase: true);
			}
			drawX = -1;
			((Control)this).Invalidate();
		}
		if (rightIndent.Dragging)
		{
			rightIndent.CancelDragging();
			pageWidth = rulerWidth - rightIndent.Indent - (pageStart - rulerStart);
			if (drawX != -1)
			{
				DrawLine(drawX, erase: true);
			}
			drawX = -1;
			((Control)this).Invalidate();
		}
	}

	public void ResetOptions()
	{
		Options = EditConsts.DefaultRulerOptions;
	}

	public void ResetUnits()
	{
		Units = EditConsts.DefaultRulerUnits;
	}

	public void ResetIndentBackColor()
	{
		IndentBackColor = EditConsts.DefaultRulerIndentBackColor;
	}

	void IControl.Invalidate()
	{
		((Control)this).Invalidate();
	}

	void IControl.Invalidate(Rectangle rect)
	{
		((Control)this).Invalidate(rect);
	}

	void IControl.Update()
	{
		((Control)this).Update();
	}

	Point IControl.PointToClient(Point p)
	{
		return ((Control)this).PointToClient(p);
	}

	Point IControl.PointToScreen(Point p)
	{
		return ((Control)this).PointToScreen(p);
	}

	bool IControl.Focus()
	{
		return ((Control)this).Focus();
	}

	bool IControl.get_CanFocus()
	{
		return ((Control)this).CanFocus;
	}

	bool IControl.get_Visible()
	{
		return ((Control)this).Visible;
	}

	void IControl.set_Visible(bool value)
	{
		((Control)this).Visible = value;
	}

	Point IControl.get_Location()
	{
		return ((Control)this).Location;
	}

	void IControl.set_Location(Point value)
	{
		((Control)this).Location = value;
	}

	int IControl.get_Left()
	{
		return ((Control)this).Left;
	}

	void IControl.set_Left(int value)
	{
		((Control)this).Left = value;
	}

	int IControl.get_Top()
	{
		return ((Control)this).Top;
	}

	void IControl.set_Top(int value)
	{
		((Control)this).Top = value;
	}

	int IControl.get_Width()
	{
		return ((Control)this).Width;
	}

	void IControl.set_Width(int value)
	{
		((Control)this).Width = value;
	}

	int IControl.get_Height()
	{
		return ((Control)this).Height;
	}

	void IControl.set_Height(int value)
	{
		((Control)this).Height = value;
	}

	void IControl.set_Bounds(Rectangle value)
	{
		((Control)this).Bounds = value;
	}

	Rectangle IControl.get_ClientRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	Control IControl.get_Parent()
	{
		return ((Control)this).Parent;
	}

	void IControl.set_Parent(Control value)
	{
		((Control)this).Parent = value;
	}

	bool IControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	bool IControl.get_IsHandleCreated()
	{
		return ((Control)this).IsHandleCreated;
	}

	void IControl.add_Click(EventHandler value)
	{
		((Control)this).Click += value;
	}

	void IControl.remove_Click(EventHandler value)
	{
		((Control)this).Click -= value;
	}
}
