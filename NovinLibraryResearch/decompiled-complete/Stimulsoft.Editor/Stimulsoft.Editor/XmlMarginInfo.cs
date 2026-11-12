namespace Stimulsoft.Editor;

public class XmlMarginInfo
{
	private Margin owner;

	private int position = EditConsts.DefaultMarginPosition;

	private string penColor = EditConsts.DefaultMarginForeColor.Name;

	private float penWidth = 1f;

	private bool visible = true;

	private bool allowDrag;

	private bool showHints;

	private int[] columnPositions;

	private string columnPenColor = EditConsts.DefaultMarginColumnForeColor.Name;

	private bool columnsVisible;

	public bool AllowDrag
	{
		get
		{
			if (owner == null)
			{
				return allowDrag;
			}
			return owner.AllowDrag;
		}
		set
		{
			allowDrag = value;
			if (owner != null)
			{
				owner.AllowDrag = value;
			}
		}
	}

	public bool ShowHints
	{
		get
		{
			if (owner == null)
			{
				return showHints;
			}
			return owner.ShowHints;
		}
		set
		{
			showHints = value;
			if (owner != null)
			{
				owner.ShowHints = value;
			}
		}
	}

	public string ColumnPenColor
	{
		get
		{
			if (owner == null)
			{
				return columnPenColor;
			}
			return XmlHelper.SerializeColor(owner.ColumnPen.Color);
		}
		set
		{
			columnPenColor = value;
			if (owner != null)
			{
				owner.ColumnPen.Color = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public bool ColumnsVisible
	{
		get
		{
			if (owner == null)
			{
				return columnsVisible;
			}
			return owner.ColumnsVisible;
		}
		set
		{
			columnsVisible = value;
			if (owner != null)
			{
				owner.ColumnsVisible = value;
			}
		}
	}

	public int[] ColumnPositions
	{
		get
		{
			if (owner == null)
			{
				return columnPositions;
			}
			return owner.ColumnPositions;
		}
		set
		{
			columnPositions = value;
			if (owner != null)
			{
				owner.ColumnPositions = value;
			}
		}
	}

	public int Position
	{
		get
		{
			if (owner == null)
			{
				return position;
			}
			return owner.Position;
		}
		set
		{
			position = value;
			if (owner != null)
			{
				owner.Position = value;
			}
		}
	}

	public string PenColor
	{
		get
		{
			if (owner == null)
			{
				return penColor;
			}
			return XmlHelper.SerializeColor(owner.Pen.Color);
		}
		set
		{
			penColor = value;
			if (owner != null)
			{
				owner.Pen.Color = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public float PenWidth
	{
		get
		{
			if (owner == null)
			{
				return penWidth;
			}
			return owner.Pen.Width;
		}
		set
		{
			penWidth = value;
			if (owner != null)
			{
				owner.Pen.Width = value;
			}
		}
	}

	public bool Visible
	{
		get
		{
			if (owner == null)
			{
				return visible;
			}
			return owner.Visible;
		}
		set
		{
			visible = value;
			if (owner != null)
			{
				owner.Visible = value;
			}
		}
	}

	public XmlMarginInfo()
	{
	}

	public XmlMarginInfo(Margin owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			position = Position;
			penColor = PenColor;
			penWidth = PenWidth;
			visible = Visible;
			allowDrag = AllowDrag;
			showHints = ShowHints;
			columnPositions = ColumnPositions;
			columnPenColor = ColumnPenColor;
			columnsVisible = ColumnsVisible;
		}
	}

	public virtual void FixupReferences(Margin owner)
	{
		this.owner = owner;
		Position = position;
		PenColor = penColor;
		PenWidth = penWidth;
		Visible = visible;
		AllowDrag = allowDrag;
		ShowHints = showHints;
		ColumnPositions = columnPositions;
		ColumnPenColor = columnPenColor;
		ColumnsVisible = columnsVisible;
	}
}
