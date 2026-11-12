using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlGutterInfo
{
	private Gutter owner;

	private int width = EditConsts.DefaultGutterWidth;

	private string backColor = EditConsts.DefaultGutterBackColor.Name;

	private string penColor = EditConsts.DefaultGutterForeColor.Name;

	private float penWidth = 1f;

	private bool visible = true;

	private bool showBookmarkHints = true;

	private int lineNumbersStart = EditConsts.DefaultLineNumbersStart;

	private int lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;

	private int lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;

	private string lineNumbersForeColor = Consts.DefaultControlForeColor.Name;

	private string lineNumbersBackColor = Consts.DefaultControlBackColor.Name;

	private string lineModificatorChangedColor = EditConsts.DefaultLineModificatorChangedColor.Name;

	private string lineModificatorSavedColor = EditConsts.DefaultLineModificatorSavedColor.Name;

	private StringAlignment lineNumbersAlignment = StringFormat.GenericTypographic.Alignment;

	private GutterOptions options = EditConsts.DefaultGutterOptions;

	private int bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;

	private int wrapImageIndex = EditConsts.DefaultWrapImageIndex;

	private bool drawLineBookmarks;

	public int Width
	{
		get
		{
			if (owner == null)
			{
				return width;
			}
			return owner.Width;
		}
		set
		{
			width = value;
			if (owner != null)
			{
				owner.Width = value;
			}
		}
	}

	public string BackColor
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return backColor;
			}
			if (!(owner.Brush is SolidBrush))
			{
				return string.Empty;
			}
			return XmlHelper.SerializeColor(((SolidBrush)owner.Brush).Color);
		}
		set
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			backColor = value;
			if (owner != null && owner.Brush is SolidBrush)
			{
				((SolidBrush)owner.Brush).Color = XmlHelper.DeserializeColor(value);
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

	public int LineNumbersStart
	{
		get
		{
			if (owner == null)
			{
				return lineNumbersStart;
			}
			return owner.LineNumbersStart;
		}
		set
		{
			lineNumbersStart = value;
			if (owner != null)
			{
				owner.LineNumbersStart = value;
			}
		}
	}

	public int LineNumbersLeftIndent
	{
		get
		{
			if (owner == null)
			{
				return lineNumbersLeftIndent;
			}
			return owner.LineNumbersLeftIndent;
		}
		set
		{
			lineNumbersLeftIndent = value;
			if (owner != null)
			{
				owner.LineNumbersLeftIndent = value;
			}
		}
	}

	public int LineNumbersRightIndent
	{
		get
		{
			if (owner == null)
			{
				return lineNumbersRightIndent;
			}
			return owner.LineNumbersRightIndent;
		}
		set
		{
			lineNumbersRightIndent = value;
			if (owner != null)
			{
				owner.LineNumbersRightIndent = value;
			}
		}
	}

	public string LineNumbersForeColor
	{
		get
		{
			if (owner == null)
			{
				return lineNumbersForeColor;
			}
			return XmlHelper.SerializeColor(owner.LineNumbersForeColor);
		}
		set
		{
			lineNumbersForeColor = value;
			if (owner != null)
			{
				owner.LineNumbersForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string LineNumbersBackColor
	{
		get
		{
			if (owner == null)
			{
				return lineNumbersBackColor;
			}
			return XmlHelper.SerializeColor(owner.LineNumbersBackColor);
		}
		set
		{
			lineNumbersBackColor = value;
			if (owner != null)
			{
				owner.LineNumbersBackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public StringAlignment LineNumbersAlignment
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return lineNumbersAlignment;
			}
			return owner.LineNumbersAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			lineNumbersAlignment = value;
			if (owner != null)
			{
				owner.LineNumbersAlignment = value;
			}
		}
	}

	public GutterOptions Options
	{
		get
		{
			if (owner == null)
			{
				return options;
			}
			return owner.Options;
		}
		set
		{
			options = value;
			if (owner != null)
			{
				owner.Options = value;
			}
		}
	}

	public int BookMarkImageIndex
	{
		get
		{
			if (owner == null)
			{
				return bookMarkImageIndex;
			}
			return owner.BookMarkImageIndex;
		}
		set
		{
			bookMarkImageIndex = value;
			if (owner != null)
			{
				owner.BookMarkImageIndex = value;
			}
		}
	}

	public int WrapImageIndex
	{
		get
		{
			if (owner == null)
			{
				return wrapImageIndex;
			}
			return owner.WrapImageIndex;
		}
		set
		{
			wrapImageIndex = value;
			if (owner != null)
			{
				owner.WrapImageIndex = value;
			}
		}
	}

	public bool DrawLineBookmarks
	{
		get
		{
			if (owner == null)
			{
				return drawLineBookmarks;
			}
			return owner.DrawLineBookmarks;
		}
		set
		{
			drawLineBookmarks = value;
			if (owner != null)
			{
				owner.DrawLineBookmarks = value;
			}
		}
	}

	public bool ShowBookmarkHints
	{
		get
		{
			if (owner == null)
			{
				return showBookmarkHints;
			}
			return owner.ShowBookmarkHints;
		}
		set
		{
			showBookmarkHints = value;
			if (owner != null)
			{
				owner.ShowBookmarkHints = value;
			}
		}
	}

	public string LineModificatorChangedColor
	{
		get
		{
			if (owner == null)
			{
				return lineModificatorChangedColor;
			}
			return XmlHelper.SerializeColor(owner.LineModificatorChangedColor);
		}
		set
		{
			lineModificatorChangedColor = value;
			if (owner != null)
			{
				owner.LineModificatorChangedColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string LineModificatorSavedColor
	{
		get
		{
			if (owner == null)
			{
				return lineModificatorSavedColor;
			}
			return XmlHelper.SerializeColor(owner.LineModificatorSavedColor);
		}
		set
		{
			lineModificatorSavedColor = value;
			if (owner != null)
			{
				owner.LineModificatorSavedColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlGutterInfo()
	{
	}//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
	//IL_00b0: Unknown result type (might be due to invalid IL or missing references)


	public XmlGutterInfo(Gutter owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			width = Width;
			backColor = BackColor;
			penColor = PenColor;
			penWidth = PenWidth;
			visible = Visible;
			lineNumbersStart = LineNumbersStart;
			lineNumbersLeftIndent = LineNumbersLeftIndent;
			lineNumbersRightIndent = LineNumbersRightIndent;
			lineNumbersForeColor = LineNumbersForeColor;
			lineNumbersBackColor = LineNumbersBackColor;
			lineNumbersAlignment = LineNumbersAlignment;
			options = Options;
			bookMarkImageIndex = BookMarkImageIndex;
			wrapImageIndex = WrapImageIndex;
			drawLineBookmarks = DrawLineBookmarks;
			showBookmarkHints = ShowBookmarkHints;
			lineModificatorChangedColor = LineModificatorChangedColor;
			lineModificatorSavedColor = LineModificatorSavedColor;
		}
	}

	public virtual void FixupReferences(Gutter owner)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		Width = width;
		BackColor = backColor;
		PenColor = penColor;
		PenWidth = penWidth;
		Visible = visible;
		LineNumbersStart = lineNumbersStart;
		LineNumbersLeftIndent = lineNumbersLeftIndent;
		LineNumbersRightIndent = lineNumbersRightIndent;
		LineNumbersForeColor = lineNumbersForeColor;
		LineNumbersBackColor = lineNumbersBackColor;
		LineNumbersAlignment = lineNumbersAlignment;
		Options = options;
		BookMarkImageIndex = bookMarkImageIndex;
		WrapImageIndex = wrapImageIndex;
		DrawLineBookmarks = drawLineBookmarks;
		ShowBookmarkHints = showBookmarkHints;
		LineModificatorChangedColor = lineModificatorChangedColor;
		LineModificatorSavedColor = lineModificatorSavedColor;
	}
}
