using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class Margin : IMargin
{
	private Pen pen;

	private Pen columnPen;

	private int position = EditConsts.DefaultMarginPosition;

	private int[] columnPositions = new int[1] { EditConsts.DefaultColumnMarginPosition };

	private bool visible;

	private bool columnsVisible;

	private int updateCount;

	private bool allowDrag;

	private bool showHints;

	private bool isDragging;

	private ISyntaxEdit owner;

	private int drawX = -1;

	private int drawY = -1;

	[Description("Gets or sets value indicating position, in characters, of the vertical line within the text portion of the control.")]
	public int Position
	{
		get
		{
			return position;
		}
		set
		{
			if (position != value)
			{
				position = value;
				Update(needUpdate: true);
			}
		}
	}

	[Description("Gets or sets the character columns where additional column margin will be drawn.")]
	public int[] ColumnPositions
	{
		get
		{
			return columnPositions;
		}
		set
		{
			columnPositions = new int[value.Length];
			Array.Copy(value, columnPositions, value.Length);
			int num = 0;
			int[] array = columnPositions;
			foreach (int num2 in array)
			{
				if (num2 <= num)
				{
					ErrorHandler.Error(new Exception(string.Format(StringConsts.InvalidMarginColumn, value.ToString())));
				}
				num = num2;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Pen Pen
	{
		get
		{
			return pen;
		}
		set
		{
			if (pen != value)
			{
				pen = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Pen ColumnPen
	{
		get
		{
			return columnPen;
		}
		set
		{
			if (columnPen != value)
			{
				columnPen = value;
				Update();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether vertical line should be painted.")]
	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				Update();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether column margins should be painted.")]
	public bool ColumnsVisible
	{
		get
		{
			return columnsVisible;
		}
		set
		{
			if (columnsVisible != value)
			{
				columnsVisible = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a color of the margin line.")]
	public Color PenColor
	{
		get
		{
			return pen.Color;
		}
		set
		{
			pen.Color = value;
		}
	}

	[Description("Gets or sets a color of the column margin line.")]
	public Color ColumnPenColor
	{
		get
		{
			return columnPen.Color;
		}
		set
		{
			columnPen.Color = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether drag operation can performed to \"Margin\".")]
	public bool AllowDrag
	{
		get
		{
			return allowDrag;
		}
		set
		{
			allowDrag = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether \"Margin\" should display some hint when mouse pointer is over the margin area.")]
	public bool ShowHints
	{
		get
		{
			return showHints;
		}
		set
		{
			showHints = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDragging
	{
		get
		{
			return isDragging;
		}
		set
		{
			if (isDragging != value)
			{
				isDragging = value;
				if (drawX >= 0)
				{
					DrawLine(drawX, drawY, erase: true);
				}
				owner.Invalidate();
				drawX = -1;
				drawY = -1;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlMarginInfo(this);
		}
		set
		{
			((XmlMarginInfo)value).FixupReferences(this);
		}
	}

	public Margin()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		pen = new Pen(EditConsts.DefaultMarginForeColor, 1f);
		columnPen = new Pen(EditConsts.DefaultMarginColumnForeColor, 1f);
		columnPen.DashStyle = (DashStyle)2;
	}

	public Margin(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	~Margin()
	{
		pen.Dispose();
		columnPen.Dispose();
	}

	protected internal void BeginUpdate()
	{
		updateCount++;
	}

	protected internal void EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update(needUpdate: true);
		}
	}

	protected internal void Update(bool needUpdate)
	{
		if (updateCount == 0 && owner != null)
		{
			owner.Invalidate();
			if (needUpdate && owner.WordWrap && owner.WrapAtMargin)
			{
				owner.UpdateWordWrap();
			}
		}
	}

	protected internal void Update()
	{
		Update(needUpdate: false);
	}

	protected internal void DrawLine(int x, int y, bool erase)
	{
		drawX = x;
		drawY = y;
		if (owner == null)
		{
			return;
		}
		SyntaxEdit syntaxEdit = (SyntaxEdit)owner;
		Rectangle clientRect;
		if (syntaxEdit.Pages.PageType == PageType.PageLayout)
		{
			IEditPage pageAtPoint = syntaxEdit.Pages.GetPageAtPoint(x, y);
			if (pageAtPoint == null)
			{
				return;
			}
			clientRect = pageAtPoint.ClientRect;
		}
		else
		{
			clientRect = syntaxEdit.ClientRect;
		}
		if (erase)
		{
			((Control)syntaxEdit).Invalidate(new Rectangle(x, clientRect.Top, 1, clientRect.Height));
			return;
		}
		Graphics val = ((Control)syntaxEdit).CreateGraphics();
		try
		{
			val.DrawLine(pen, x, clientRect.Top, x, clientRect.Bottom - 1);
		}
		finally
		{
			val.Dispose();
		}
	}

	protected internal int GetColumnPosition(int pos)
	{
		int num = columnPositions.Length;
		if (num == 0)
		{
			return EditConsts.DefaultColumnMarginPosition;
		}
		int num2 = pos - columnPositions[num - 1];
		int num3 = 0;
		if (num2 >= 0)
		{
			num3 = ((num != 1) ? (columnPositions[num - 1] - columnPositions[num - 2]) : columnPositions[0]);
			return columnPositions[num - 1] + (num2 / num3 + 1) * num3;
		}
		for (num3 = 0; pos >= columnPositions[num3]; num3++)
		{
		}
		return columnPositions[num3];
	}

	public void Assign(IMargin source)
	{
		BeginUpdate();
		try
		{
			Pen.Width = source.Pen.Width;
			Pen.Color = source.Pen.Color;
			ColumnPen.Width = source.ColumnPen.Width;
			ColumnPen.Color = source.ColumnPen.Color;
			Position = source.Position;
			ColumnPositions = source.ColumnPositions;
			Visible = source.Visible;
			ColumnsVisible = source.ColumnsVisible;
			AllowDrag = source.AllowDrag;
			ShowHints = source.ShowHints;
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool Contains(int x, int y)
	{
		if (owner != null)
		{
			if (owner.Pages.PageType == PageType.PageLayout)
			{
				IEditPage pageAtPoint = owner.Pages.GetPageAtPoint(x, y);
				if (pageAtPoint == null || !pageAtPoint.ClientRect.Contains(x, y))
				{
					return false;
				}
			}
			Point point = owner.ScreenToDisplay(x, y);
			int x2 = ((SyntaxEdit)owner).DisplayToScreen(Position, point.Y, average: true).X;
			if (x > x2 - EditConsts.DefaultRulerHitWidth)
			{
				return x <= x2 + EditConsts.DefaultRulerHitWidth;
			}
			return false;
		}
		return false;
	}

	public void DragTo(int x, int y)
	{
		if (drawX >= 0)
		{
			DrawLine(drawX, drawY, erase: true);
		}
		DrawLine(x, y, erase: false);
	}

	public void CancelDragging()
	{
		IsDragging = false;
	}

	public void Paint(IPainter painter, Rectangle rect)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (!isDragging)
		{
			painter.DrawLine(rect.Left, 0, rect.Left, rect.Bottom, pen.Color, (int)pen.Width, pen.DashStyle);
		}
	}

	public void PaintColumn(IPainter painter, Rectangle rect)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)columnPen.DashStyle == 2)
		{
			painter.DrawDotLine(rect.Left, 0, rect.Left, rect.Bottom, columnPen.Color);
		}
		else
		{
			painter.DrawLine(rect.Left, 0, rect.Left, rect.Bottom, columnPen.Color, (int)columnPen.Width, columnPen.DashStyle);
		}
	}

	public virtual void ResetPenColor()
	{
		PenColor = EditConsts.DefaultMarginForeColor;
	}

	public virtual void ResetColumnsPenColor()
	{
		PenColor = EditConsts.DefaultMarginColumnForeColor;
	}

	public virtual void ResetPosition()
	{
		Position = EditConsts.DefaultMarginPosition;
	}

	public virtual void ResetColumnPositions()
	{
		ColumnPositions = new int[1] { EditConsts.DefaultColumnMarginPosition };
	}

	public virtual void ResetVisible()
	{
		Visible = false;
	}

	public virtual void ResetColumnsVisible()
	{
		ColumnsVisible = false;
	}

	public void ResetAllowDrag()
	{
		AllowDrag = false;
	}

	public void ResetShowHints()
	{
		ShowHints = false;
	}

	public bool ShouldSerializePosition()
	{
		return position != EditConsts.DefaultMarginPosition;
	}

	public bool ShouldSerializeColumnPositions()
	{
		if (columnPositions.Length == 1)
		{
			return columnPositions[0] != EditConsts.DefaultColumnMarginPosition;
		}
		return true;
	}

	public bool ShouldSerializePenColor()
	{
		return PenColor != EditConsts.DefaultMarginForeColor;
	}

	public bool ShouldSerializeColumnPenColor()
	{
		return ColumnPenColor != EditConsts.DefaultMarginColumnForeColor;
	}
}
