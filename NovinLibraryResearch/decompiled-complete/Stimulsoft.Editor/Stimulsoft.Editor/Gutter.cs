using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class Gutter : IGutter
{
	private Brush brush;

	private Pen pen;

	private bool visible = true;

	private int width = EditConsts.DefaultGutterWidth;

	private GutterOptions options;

	private ImageList images;

	private ImageList internalImages;

	private ArrayList ranges;

	private int updateCount;

	private int lineNumberWidth;

	private int lineNumberLength;

	private StringAlignment lineNumbersAlignment;

	private int lineNumbersStart = EditConsts.DefaultLineNumbersStart;

	private int lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;

	private int lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;

	private int outliningLeftIndent = EditConsts.DefaultOutliningIndent;

	private int outliningRightIndent = EditConsts.DefaultOutliningIndent;

	private int bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;

	private int wrapImageIndex = EditConsts.DefaultWrapImageIndex;

	private Color lineModificatorChangedColor;

	private Color lineModificatorSavedColor;

	private Color lineNumbersForeColor;

	private Color lineNumbersBackColor;

	private bool drawLineBookmarks;

	private bool showBookmarkHints = true;

	private DrawInfo drawInfo;

	private Color lineBookmarksColor = EditConsts.DefaultLineBookmarksColor;

	private int userMarginWidth = EditConsts.DefaultUserMarginWidth;

	private Color userMarginForeColor;

	private Color userMarginBackColor;

	private string userMarginText = EditConsts.DefaultUserMarginText;

	private Regex userMarginRegex;

	private ISyntaxEdit owner;

	protected internal bool Transparent
	{
		get
		{
			if (owner != null)
			{
				return owner.Transparent;
			}
			return false;
		}
	}

	protected internal Regex UserMarginRegex
	{
		get
		{
			if (userMarginRegex == null)
			{
				userMarginRegex = new Regex("\\\\\\[[a-zA-Z_0-9]+\\]", RegexOptions.Singleline);
			}
			return userMarginRegex;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle Rect
	{
		get
		{
			Rectangle result = ((owner != null) ? ((SyntaxEdit)owner).GetClientRect(excludeNonClient: true) : new Rectangle(0, 0, 0, 0));
			result.Width = GetWidth();
			return result;
		}
	}

	[Description("Gets or sets the width of the gutter.")]
	public int Width
	{
		get
		{
			return width;
		}
		set
		{
			if (width != value)
			{
				width = value;
				if (visible)
				{
					Update();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Brush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			if (brush != value)
			{
				brush = value;
				Update();
			}
		}
	}

	[Description("Gets or sets background color of the gutter.")]
	public Color BrushColor
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (!(brush is SolidBrush))
			{
				return EditConsts.DefaultGutterBackColor;
			}
			return ((SolidBrush)brush).Color;
		}
		set
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (brush is SolidBrush && ((SolidBrush)brush).Color != value)
			{
				((SolidBrush)brush).Color = value;
				Update();
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

	[Description("Gets or sets color of the gutter line.")]
	public Color PenColor
	{
		get
		{
			return pen.Color;
		}
		set
		{
			if (pen.Color != value)
			{
				pen.Color = value;
				Update();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether the gutter area is visible.")]
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
				if (owner != null && owner.WordWrap)
				{
					owner.UpdateWordWrap();
				}
				Update();
			}
		}
	}

	[Description("Gets or sets number of the first line being painted on the gutter.")]
	public int LineNumbersStart
	{
		get
		{
			return lineNumbersStart;
		}
		set
		{
			if (lineNumbersStart != value)
			{
				lineNumbersStart = value;
				LineNumbersChanged();
				Update();
			}
		}
	}

	[Description("Gets or sets line numbers indentation from the left gutter border.")]
	public int LineNumbersLeftIndent
	{
		get
		{
			return lineNumbersLeftIndent;
		}
		set
		{
			if (lineNumbersLeftIndent != value)
			{
				lineNumbersLeftIndent = value;
				LineNumbersChanged();
			}
		}
	}

	[Description("Gets or sets line numbers indentation from the right gutter border.")]
	public int LineNumbersRightIndent
	{
		get
		{
			return lineNumbersRightIndent;
		}
		set
		{
			if (lineNumbersRightIndent != value)
			{
				lineNumbersRightIndent = value;
				LineNumbersChanged();
			}
		}
	}

	[Description("Gets or sets foreground color for the line numbers.")]
	public Color LineNumbersForeColor
	{
		get
		{
			return lineNumbersForeColor;
		}
		set
		{
			if (lineNumbersForeColor != value)
			{
				lineNumbersForeColor = value;
				Update(NeedLineNumbers());
			}
		}
	}

	[Description("Gets or sets background color for the line numbers.")]
	public Color LineNumbersBackColor
	{
		get
		{
			return lineNumbersBackColor;
		}
		set
		{
			if (lineNumbersBackColor != value)
			{
				lineNumbersBackColor = value;
				Update(NeedLineNumbers());
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line numbers alignment information.")]
	public StringAlignment LineNumbersAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return lineNumbersAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (lineNumbersAlignment != value)
			{
				lineNumbersAlignment = value;
				Update(NeedLineNumbers());
			}
		}
	}

	[Description("Gets or sets outlining indentation from the left gutter border.")]
	public int OutliningLeftIndent
	{
		get
		{
			return outliningLeftIndent;
		}
		set
		{
			if (outliningLeftIndent != value)
			{
				outliningLeftIndent = value;
				Update();
			}
		}
	}

	[Description("Gets or sets outlining indentation from the right gutter border.")]
	public int OutliningRightIndent
	{
		get
		{
			return outliningRightIndent;
		}
		set
		{
			if (outliningRightIndent != value)
			{
				outliningRightIndent = value;
				Update();
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets a \"GutterOptions\" that determine gutter appearance and behaviour.")]
	public GutterOptions Options
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
				LineNumbersChanged();
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ImageList Images
	{
		get
		{
			if (images == null)
			{
				return internalImages;
			}
			return images;
		}
		set
		{
			if (images != value)
			{
				images = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a value that specifies index of item in the image collection used to paint bookmark.")]
	public int BookMarkImageIndex
	{
		get
		{
			return bookMarkImageIndex;
		}
		set
		{
			if (bookMarkImageIndex != value)
			{
				bookMarkImageIndex = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a value that specifies index of item in the image collection used to paint special mark indicating the wrapped line.")]
	public int WrapImageIndex
	{
		get
		{
			return wrapImageIndex;
		}
		set
		{
			if (wrapImageIndex != value)
			{
				wrapImageIndex = value;
				Update();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether Edit control should draw triangle at bookmark position inside line.")]
	public bool DrawLineBookmarks
	{
		get
		{
			return drawLineBookmarks;
		}
		set
		{
			if (drawLineBookmarks != value)
			{
				drawLineBookmarks = value;
				if (owner != null)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether Edit control should display text describing bookmark in form of tooltip window when mouse pointer is over the gutter bookmark.")]
	public bool ShowBookmarkHints
	{
		get
		{
			return showBookmarkHints;
		}
		set
		{
			showBookmarkHints = value;
		}
	}

	[Description("Gets or sets a color of the line bookmarks.")]
	public Color LineBookmarksColor
	{
		get
		{
			return lineBookmarksColor;
		}
		set
		{
			if (lineBookmarksColor != value)
			{
				lineBookmarksColor = value;
				if (drawLineBookmarks && owner != null)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[Description("Gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the modified state.")]
	public Color LineModificatorChangedColor
	{
		get
		{
			return lineModificatorChangedColor;
		}
		set
		{
			if (lineModificatorChangedColor != value)
			{
				lineModificatorChangedColor = value;
				Update((options & GutterOptions.PaintLineModificators) != 0);
			}
		}
	}

	[Description("Gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the saved state.")]
	public Color LineModificatorSavedColor
	{
		get
		{
			return lineModificatorSavedColor;
		}
		set
		{
			if (lineModificatorSavedColor != value)
			{
				lineModificatorSavedColor = value;
				Update((options & GutterOptions.PaintLineModificators) != 0);
			}
		}
	}

	public int UserMarginWidth
	{
		get
		{
			return userMarginWidth;
		}
		set
		{
			if (userMarginWidth != value)
			{
				userMarginWidth = value;
				if ((options & GutterOptions.PaintUserMargin) != 0)
				{
					Update();
				}
			}
		}
	}

	[Description("Gets or sets foreground color of the user margin.")]
	public Color UserMarginForeColor
	{
		get
		{
			return userMarginForeColor;
		}
		set
		{
			if (userMarginForeColor != value)
			{
				userMarginForeColor = value;
				if ((options & GutterOptions.PaintUserMargin) != 0)
				{
					Update();
				}
			}
		}
	}

	[Description("Gets or sets background color of the user margin.")]
	public Color UserMarginBackColor
	{
		get
		{
			return userMarginBackColor;
		}
		set
		{
			if (userMarginBackColor != value)
			{
				userMarginBackColor = value;
				if ((options & GutterOptions.PaintUserMargin) != 0)
				{
					Update();
				}
			}
		}
	}

	[Description("Gets or sets text of the user margin.")]
	public string UserMarginText
	{
		get
		{
			return userMarginText;
		}
		set
		{
			if (userMarginText != value)
			{
				userMarginText = value;
				if ((options & GutterOptions.PaintUserMargin) != 0)
				{
					Update();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlGutterInfo(this);
		}
		set
		{
			((XmlGutterInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event EventHandler Click;

	[Browsable(false)]
	public event EventHandler DoubleClick;

	[Browsable(false)]
	public event DrawUserMarginEvent DrawUserMargin;

	public Gutter()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		drawInfo = default(DrawInfo);
		brush = (Brush)new SolidBrush(EditConsts.DefaultGutterBackColor);
		pen = new Pen(EditConsts.DefaultGutterForeColor, 1f);
		internalImages = new ImageList();
		internalImages.ImageSize = new Size(15, 15);
		ranges = new ArrayList();
		options = EditConsts.DefaultGutterOptions;
		lineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
		lineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
		lineModificatorChangedColor = EditConsts.DefaultLineModificatorChangedColor;
		lineModificatorSavedColor = EditConsts.DefaultLineModificatorSavedColor;
		userMarginForeColor = EditConsts.DefaultUserMarginForeColor;
		userMarginBackColor = EditConsts.DefaultUserMarginBackColor;
		try
		{
			ResourceManager resourceManager = new ResourceManager(typeof(Resources));
			_003F val = internalImages;
			object? @object = resourceManager.GetObject("SyntaxEdit.Gutter.Images.ImageStream");
			((ImageList)val).ImageStream = (ImageListStreamer)((@object is ImageListStreamer) ? @object : null);
		}
		catch
		{
		}
		internalImages.TransparentColor = EditConsts.DefaultTransparentColor;
	}

	public Gutter(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	~Gutter()
	{
		ranges.Clear();
		brush.Dispose();
		pen.Dispose();
		internalImages.ImageStream = null;
		((Component)(object)internalImages).Dispose();
	}

	private int GetLineNumbersWidth()
	{
		if ((options & GutterOptions.PaintLineNumbers) == 0)
		{
			return 0;
		}
		return lineNumberWidth;
	}

	private bool NeedPaint(ref Rectangle rect)
	{
		if (owner != null && !rect.IsEmpty)
		{
			int fontHeight = owner.Painter.FontHeight;
			if (fontHeight > 0)
			{
				int bottom = rect.Bottom;
				rect.Y = rect.Top / fontHeight * fontHeight;
				rect.Height = bottom - rect.Y;
			}
			return true;
		}
		return false;
	}

	private void CenterOutlineRect(ref int l, ref int t, int w)
	{
		int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
		int num = w - defaultCollasedImageWidth >> 1;
		if (num > 0)
		{
			l += num;
			t += num;
		}
	}

	private void DrawLine(IPainter painter, int x1, int y1, int x2, int y2)
	{
		painter.DrawLine(x1, y1, x2, y2);
		if (!Transparent)
		{
			if (x1 == x2)
			{
				painter.ExcludeClipRect(x1, y1, 1, y2 - y1);
			}
			else
			{
				painter.ExcludeClipRect(x1, y1, x2 - x1, 1);
			}
		}
	}

	private void DrawRectangle(IPainter painter, Rectangle rect, bool drawPlus)
	{
		Rectangle rect2 = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 1, rect.Height - 1);
		if (!Transparent)
		{
			Color backColor = painter.BackColor;
			try
			{
				painter.BackColor = ((visible && (owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != 0) ? BrushColor : ((SyntaxEdit)owner).GetBackColor(readOnly: false));
				painter.FillRectangle(rect2);
			}
			finally
			{
				painter.BackColor = backColor;
			}
		}
		int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
		painter.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
		painter.DrawLine(rect.Left + 2, rect.Top + (defaultCollasedImageWidth >> 1), rect.Left + defaultCollasedImageWidth - 2, rect.Top + (defaultCollasedImageWidth >> 1));
		if (drawPlus)
		{
			painter.DrawLine(rect.Left + (defaultCollasedImageWidth >> 1), rect.Top + 2, rect.Left + (defaultCollasedImageWidth >> 1), rect.Top + defaultCollasedImageWidth - 2);
		}
		if (!Transparent)
		{
			painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width + 1, rect.Height + 1);
		}
	}

	private void DrawOutline(IPainter painter, int line, int l, int t, int w, bool end)
	{
		int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
		int num = t;
		CenterOutlineRect(ref l, ref t, w);
		Rectangle rect = new Rectangle(l, t, defaultCollasedImageWidth, w);
		drawInfo.Init();
		drawInfo.Line = line;
		Color foreColor = painter.ForeColor;
		try
		{
			painter.ForeColor = owner.Outlining.OutlineColor;
			if (!owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.OutlineArea, drawInfo))
			{
				DrawLine(painter, l + (defaultCollasedImageWidth >> 1), num, l + (defaultCollasedImageWidth >> 1), num + w);
				if (end)
				{
					DrawLine(painter, l + (defaultCollasedImageWidth >> 1), num + w - 1, l + defaultCollasedImageWidth + 1, num + w - 1);
				}
			}
		}
		finally
		{
			painter.ForeColor = foreColor;
		}
		owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.OutlineArea, drawInfo);
	}

	private void DrawOutlineImage(IPainter painter, int line, int l, int t, int w, bool drawLines, bool visible, bool before, bool after)
	{
		int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
		int num = t;
		CenterOutlineRect(ref l, ref t, w);
		Rectangle rect = new Rectangle(l, t, defaultCollasedImageWidth, defaultCollasedImageWidth);
		drawInfo.Init();
		drawInfo.Line = line;
		IOutlining outlining = owner.Outlining;
		Color backColor = painter.BackColor;
		Color foreColor = painter.ForeColor;
		try
		{
			painter.BackColor = outlining.OutlineColor;
			painter.ForeColor = outlining.OutlineColor;
			if (owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.OutlineImage, drawInfo))
			{
				return;
			}
			DrawRectangle(painter, rect, !visible);
			if (drawLines)
			{
				if (before)
				{
					DrawLine(painter, l + (defaultCollasedImageWidth >> 1), num, l + (defaultCollasedImageWidth >> 1), t);
				}
				if (after)
				{
					DrawLine(painter, l + (defaultCollasedImageWidth >> 1), t + defaultCollasedImageWidth, l + (defaultCollasedImageWidth >> 1), num + w);
				}
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.OutlineImage, drawInfo);
		}
		finally
		{
			painter.BackColor = backColor;
			painter.ForeColor = foreColor;
		}
	}

	private int CompareRange(Point pt, Point sPt, Point ePt)
	{
		if (pt.Y < sPt.Y || (pt.Y == sPt.Y && pt.X < sPt.X))
		{
			return 1;
		}
		if (pt.Y > ePt.Y || (pt.Y == ePt.Y && pt.X >= ePt.X && ePt.X != int.MaxValue))
		{
			return -1;
		}
		return 0;
	}

	private void DrawLineNumber(IPainter painter, int line, int left, int top, bool drawOnGutter)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected I4, but got Unknown
		if (owner == null)
		{
			return;
		}
		Rectangle rect = new Rectangle(left, top, lineNumberWidth - (lineNumbersLeftIndent + lineNumbersRightIndent), painter.FontHeight);
		drawInfo.Init();
		drawInfo.Line = line;
		Color textColor = painter.TextColor;
		Color backColor = painter.BackColor;
		bool transparent = Transparent;
		try
		{
			painter.TextColor = lineNumbersForeColor;
			painter.BackColor = (drawOnGutter ? BrushColor : lineNumbersBackColor);
			if (transparent)
			{
				painter.Opaque = false;
			}
			drawInfo.Text = (line + lineNumbersStart).ToString();
			if (!owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.LineNumber, drawInfo))
			{
				StringAlignment val = lineNumbersAlignment;
				switch ((int)val)
				{
				case 0:
					painter.TextOut(drawInfo.Text, -1, rect, clipped: false, !transparent);
					break;
				case 2:
				{
					int num2 = painter.StringWidth(drawInfo.Text);
					painter.TextOut(drawInfo.Text, -1, rect, rect.Left + rect.Width - num2, rect.Top, clipped: false, !transparent);
					break;
				}
				case 1:
				{
					int num = painter.StringWidth(drawInfo.Text);
					painter.TextOut(drawInfo.Text, -1, rect, rect.Left + (rect.Width - num) / 2, rect.Top, clipped: false, !transparent);
					break;
				}
				}
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.LineNumber, drawInfo);
		}
		finally
		{
			if (transparent)
			{
				painter.Opaque = true;
			}
			painter.TextColor = textColor;
			painter.BackColor = backColor;
		}
	}

	private string GetUserMarginText(string text, int line, int chars)
	{
		MatchCollection matchCollection = UserMarginRegex.Matches(text);
		for (int num = matchCollection.Count - 1; num >= 0; num--)
		{
			Match match = matchCollection[num];
			if (match.Success)
			{
				string text2 = text.Substring(match.Index, match.Length);
				if (string.Compare(text2, EditConsts.LineTag) == 0)
				{
					text2 = (line + 1).ToString();
				}
				if (string.Compare(text2, EditConsts.CharsTag) == 0)
				{
					text2 = chars.ToString();
				}
				else
				{
					OnDrawUserMargin(ref text2, line);
				}
				text = text.Remove(match.Index, match.Length);
				text = text.Insert(match.Index, text2);
			}
		}
		return text;
	}

	private void DrawLineOnUserMargin(IPainter painter, int pos, int line, int left, int top)
	{
		if (owner == null)
		{
			return;
		}
		Rectangle rect = new Rectangle(left, top, userMarginWidth, painter.FontHeight);
		drawInfo.Init();
		drawInfo.Line = line;
		Color textColor = painter.TextColor;
		Color backColor = painter.BackColor;
		bool transparent = Transparent;
		try
		{
			painter.TextColor = userMarginForeColor;
			painter.BackColor = userMarginBackColor;
			if (transparent)
			{
				painter.Opaque = false;
			}
			drawInfo.Text = ((pos == 0) ? GetUserMarginText(userMarginText, line, owner.Lines.GetLength(line)) : string.Empty);
			if (!owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.UserMargin, drawInfo))
			{
				painter.TextOut(drawInfo.Text, -1, rect, clipped: true, !transparent);
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.UserMargin, drawInfo);
		}
		finally
		{
			if (transparent)
			{
				painter.Opaque = true;
			}
			painter.TextColor = textColor;
			painter.BackColor = backColor;
		}
	}

	private void DrawLineModificator(IPainter painter, int line, int left, int top)
	{
		if (owner == null)
		{
			return;
		}
		Rectangle rect = new Rectangle(left, top, GetLineModificatorWidth(checkOutlining: false), painter.FontHeight);
		drawInfo.Init();
		drawInfo.Line = line;
		Color backColor = painter.BackColor;
		try
		{
			bool saved;
			bool flag = owner.Source.IsLineModified(line, out saved);
			if (flag)
			{
				painter.BackColor = (saved ? LineModificatorSavedColor : LineModificatorChangedColor);
			}
			if (!owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.LineModificator, drawInfo) && flag)
			{
				painter.FillRectangle(rect);
				painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.LineModificator, drawInfo);
		}
		finally
		{
			painter.BackColor = backColor;
		}
	}

	private bool NeedLineNumbers()
	{
		return (options & GutterOptions.PaintLineNumbers) != 0;
	}

	private int GetLineNumbersLeft(bool useIndent)
	{
		if (!NeedLineNumbers())
		{
			return 0;
		}
		return ((visible && (options & GutterOptions.PaintLinesOnGutter) == 0) ? GetPaintWidth() : 0) + (useIndent ? lineNumbersLeftIndent : 0);
	}

	private int GetLineNumbersRight(bool useIndent)
	{
		if (!NeedLineNumbers())
		{
			return 0;
		}
		return GetLineNumbersLeft(useIndent) + lineNumberWidth - (useIndent ? (lineNumbersLeftIndent + lineNumbersRightIndent) : 0);
	}

	private int GetOutlineLeft(bool useIndent)
	{
		if (owner != null && (owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != 0 && Visible)
		{
			return GetPaintWidth() - GetUserMarginWidth() - GetOutlineWidth() + (useIndent ? outliningLeftIndent : 0);
		}
		return GetWidth() - GetOutlineWidth() + (useIndent ? outliningLeftIndent : 0);
	}

	private int GetOutlineWidth()
	{
		if (!AllowOutlining())
		{
			return 0;
		}
		return owner.Painter.FontHeight + outliningLeftIndent + outliningRightIndent;
	}

	private int GetUserMarginWidth()
	{
		if ((options & GutterOptions.PaintUserMargin) == 0)
		{
			return 0;
		}
		return userMarginWidth;
	}

	private int GetLineModificatorWidth(bool checkOutlining)
	{
		int num = (((options & GutterOptions.PaintLineModificators) != 0) ? EditConsts.DefaultLineModificatorWidth : 0);
		if (num != 0 && checkOutlining && AllowOutlining() && owner != null && (!Visible || (owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == 0))
		{
			num = 0;
		}
		return num;
	}

	private int GetLineModificatorLeft()
	{
		return GetWidth() - ((AllowOutlining() && owner != null && (!Visible || (owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == 0)) ? GetOutlineWidth() : GetLineModificatorWidth(checkOutlining: false));
	}

	private int GetPaintWidth()
	{
		return GetPaintWidth(CheckOutlining: true);
	}

	private int GetPaintWidth(bool CheckOutlining)
	{
		int num = (visible ? Width : 0);
		if ((options & GutterOptions.PaintLinesOnGutter) != 0)
		{
			num = Math.Max(num, GetLineNumbersWidth());
		}
		num += GetUserMarginWidth();
		if (CheckOutlining && owner != null && (owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != 0)
		{
			num += GetOutlineWidth();
		}
		return num;
	}

	private void DrawImage(IPainter painter, int imageIndex, int line, int left, int top, int right, bool bookmark)
	{
		if (CanDrawImage(imageIndex, left, right))
		{
			drawInfo.Init();
			drawInfo.Line = line;
			drawInfo.GutterImage = imageIndex;
			Rectangle rect = new Rectangle(left, top, Images.ImageSize.Width, Images.ImageSize.Height);
			if (!owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter | DrawState.GutterImage | (bookmark ? DrawState.BookMark : DrawState.None), drawInfo))
			{
				painter.DrawImage(Images, imageIndex, rect);
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter | DrawState.GutterImage, drawInfo);
		}
	}

	private void UpdateLineNumberLength()
	{
		if (owner == null)
		{
			return;
		}
		int val = Math.Max(Math.Max(owner.DisplayLines.GetCount(), owner.Lines.Count) + lineNumbersStart - 1, 0);
		if ((options & GutterOptions.PaintLinesBeyondEof) != 0)
		{
			int num;
			if (owner.Pages.PageType == PageType.PageLayout)
			{
				IEditPage editPage = ((owner.Pages.Count > 0) ? owner.Pages[owner.Pages.Count - 1] : owner.Pages.DefaultPage);
				num = editPage.EndLine;
			}
			else
			{
				num = owner.Scrolling.WindowOriginY;
				int fontHeight = owner.Painter.FontHeight;
				if (fontHeight != 0)
				{
					int num2 = ((SyntaxEdit)owner).ClientHeight();
					num += num2 / fontHeight + ((num2 % fontHeight != 0) ? 1 : 0);
				}
			}
			num = owner.DisplayLines.DisplayPointToPoint(0, num).Y;
			val = Math.Max(val, num);
		}
		lineNumberLength = val.ToString().Length;
	}

	private void UpdateLineNumberWidth()
	{
		if (owner != null)
		{
			lineNumberWidth = lineNumberLength * owner.Painter.StringWidth("9") + lineNumbersLeftIndent + lineNumbersRightIndent;
		}
	}

	private bool AllowOutlining()
	{
		if (owner != null)
		{
			return owner.Outlining.AllowOutlining;
		}
		return false;
	}

	private Point ScreenToClient(int x, int y)
	{
		if (owner != null && owner.Pages.PageType == PageType.PageLayout)
		{
			Rectangle clientRect = owner.Pages.GetPageAtPoint(x, y).ClientRect;
			x -= clientRect.Left;
			y -= clientRect.Top;
		}
		return new Point(x, y);
	}

	private void DrawGutter(IPainter painter, Rectangle rect, int startLine)
	{
		int imageIndex = -1;
		bool bookMark = false;
		DrawGutter(painter, rect, calcIndex: false, startLine, 0, ref imageIndex, ref bookMark);
	}

	private bool IsBookmarkImageIndex(int index)
	{
		if (index < 0 || index > 9)
		{
			return index == bookMarkImageIndex;
		}
		return true;
	}

	private void DrawGutter(IPainter painter, Rectangle rect, bool calcIndex, int line, int imageX, ref int imageIndex, ref bool bookMark)
	{
		int num = rect.Top;
		int fontHeight = painter.FontHeight;
		OutlineOptions outlineOptions = owner.Outlining.OutlineOptions;
		bool flag = (outlineOptions & OutlineOptions.DrawLines) != 0;
		bool allowOutlining = owner.Outlining.AllowOutlining;
		bool flag2 = (options & GutterOptions.PaintLinesBeyondEof) != 0;
		bool flag3 = NeedLineNumbers();
		bool flag4 = (options & GutterOptions.PaintBookMarks) != 0;
		bool flag5 = GetUserMarginWidth() > 1;
		bool flag6 = flag3 && (visible & ((options & GutterOptions.PaintLinesOnGutter) != 0));
		bool flag7 = (options & GutterOptions.PaintLineModificators) != 0;
		int lineNumbersLeft = GetLineNumbersLeft(useIndent: true);
		int lineNumbersRight = GetLineNumbersRight(useIndent: true);
		int outlineLeft = GetOutlineLeft(useIndent: true);
		int lineModificatorLeft = GetLineModificatorLeft();
		int num2 = rect.Left + GetPaintWidth(CheckOutlining: false);
		int num3 = GetPaintWidth() - GetUserMarginWidth();
		int count = owner.DisplayLines.GetCount();
		int num4 = 0;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		while (num < rect.Bottom || calcIndex)
		{
			Point point = owner.DisplayLines.DisplayPointToPoint(0, line);
			if (line >= count && !(flag3 && flag2))
			{
				break;
			}
			if (flag7 && !calcIndex)
			{
				DrawLineModificator(painter, point.Y, lineModificatorLeft, num);
			}
			if (flag3 && !calcIndex)
			{
				if (point.X == 0)
				{
					DrawLineNumber(painter, point.Y, lineNumbersLeft, num, flag6);
				}
				else if (!Transparent)
				{
					Color backColor = painter.BackColor;
					try
					{
						painter.BackColor = (flag6 ? BrushColor : lineNumbersBackColor);
						painter.FillRectangle(lineNumbersLeft, num, lineNumbersRight - lineNumbersLeft, fontHeight);
					}
					finally
					{
						painter.BackColor = backColor;
					}
				}
			}
			if (flag5 && !calcIndex)
			{
				DrawLineOnUserMargin(painter, point.X, point.Y, num3, num);
			}
			if (line < count)
			{
				if (Visible)
				{
					arrayList3.Clear();
					int num5 = -1;
					if (owner.WordWrap && point.X != 0 && wrapImageIndex >= 0 && CanDrawImage(wrapImageIndex))
					{
						arrayList3.Add(wrapImageIndex);
					}
					if (flag4)
					{
						Point endPoint = ((DisplayStrings)owner.DisplayLines).DisplayPointToPoint(int.MaxValue, line, wrapEnd: true, rangeStart: false, tabEnd: false);
						owner.Source.BookMarks.GetBookMarks(point, endPoint, arrayList);
						foreach (IBookMark item in arrayList)
						{
							if (!owner.Outlining.AllowOutlining || owner.Outlining.IsVisible(new Point(item.Pos, item.Line)))
							{
								int index = item.Index;
								if (index == int.MaxValue)
								{
									index = bookMarkImageIndex;
								}
								if (CanDrawImage(index))
								{
									arrayList3.Add(index);
									num5 = index;
									break;
								}
							}
						}
					}
					if (num >= rect.Top - Images.ImageSize.Height && point.X == 0 && (!owner.Outlining.AllowOutlining || owner.Outlining.IsVisible(point.Y)))
					{
						((LineStyles)owner.Source.LineStyles).GetLineStyles(point.Y, arrayList2);
						if (arrayList2.Count > 0)
						{
							for (int num6 = arrayList2.Count - 1; num6 >= 0; num6--)
							{
								ILineStyle style = ((LineStylesEx)owner.LineStyles).GetStyle(((LStyle)arrayList2[num6]).Index);
								if (style != null)
								{
									int imageIndex2 = style.ImageIndex;
									if (CanDrawImage(imageIndex2))
									{
										arrayList3.Add(imageIndex2);
									}
								}
							}
						}
					}
					if (arrayList3.Count > 0)
					{
						int num7 = 0;
						Size imageSize = Images.ImageSize;
						Rectangle rect2 = Rectangle.Empty;
						if (!Transparent && imageSize.Width != 0)
						{
							rect2 = new Rectangle(num7, num, Math.Min(arrayList3.Count, num2 / imageSize.Width) * imageSize.Width, imageSize.Height);
							painter.FillRectangle(rect2);
						}
						foreach (int item2 in arrayList3)
						{
							if (calcIndex && imageX >= num7 && imageX < num7 + imageSize.Width)
							{
								imageIndex = item2;
								bookMark = IsBookmarkImageIndex(item2);
								return;
							}
							if (!calcIndex)
							{
								DrawImage(painter, item2, point.Y, num7, num, num2, num5 == item2);
							}
							if (num7 <= num2 - imageSize.Width * 2)
							{
								num7 += imageSize.Width;
							}
						}
						if (!Transparent && !rect2.IsEmpty)
						{
							painter.ExcludeClipRect(rect2.Left, rect2.Top, rect2.Width, rect2.Height);
						}
					}
				}
				if (allowOutlining && !calcIndex)
				{
					owner.Outlining.GetOutlineRanges(ranges, point.Y);
					DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
					Point ePt = displayStrings.DisplayPointToPoint(int.MaxValue, line, wrapEnd: true, rangeStart: false, tabEnd: false);
					bool flag8 = false;
					bool flag9 = true;
					foreach (IOutlineRange range2 in ranges)
					{
						if (CompareRange(range2.StartPoint, point, ePt) == 0)
						{
							flag9 &= range2.Visible;
							flag8 = true;
						}
					}
					if (flag8)
					{
						IRange range = (IRange)ranges[0];
						DrawOutlineImage(painter, point.Y, outlineLeft, num, fontHeight, flag, flag9, CompareRange(range.StartPoint, point, ePt) > 0, CompareRange(range.EndPoint, point, ePt) < 0);
					}
					if (!flag8 && flag)
					{
						bool flag10 = false;
						bool flag11 = false;
						foreach (IOutlineRange range3 in ranges)
						{
							int num9 = CompareRange(range3.StartPoint, point, ePt);
							int num10 = CompareRange(range3.EndPoint, point, ePt);
							flag10 = flag10 || (num9 > 0 && num10 <= 0);
							flag11 = flag11 || num10 == 0;
						}
						if (flag10)
						{
							DrawOutline(painter, point.Y, outlineLeft, num, fontHeight, flag11);
						}
					}
				}
			}
			if (calcIndex)
			{
				break;
			}
			num4 = num;
			num += fontHeight;
			line++;
		}
		if (!calcIndex)
		{
			if (flag3 && !Transparent && num > rect.Top && lineNumbersRight > lineNumbersLeft)
			{
				painter.ExcludeClipRect(lineNumbersLeft, rect.Top, lineNumbersRight - lineNumbersLeft, num - rect.Top);
			}
			if (flag5 && !Transparent && num > rect.Top)
			{
				painter.ExcludeClipRect(num3, rect.Top, GetUserMarginWidth() - 1, num - rect.Top);
			}
		}
	}

	protected internal int GetWidth()
	{
		int num = (visible ? Width : 0);
		int lineNumbersWidth = GetLineNumbersWidth();
		num = (((options & GutterOptions.PaintLinesOnGutter) != 0) ? Math.Max(num, lineNumbersWidth) : (num + lineNumbersWidth));
		return num + GetLineModificatorWidth(checkOutlining: true) + GetOutlineWidth() + GetUserMarginWidth();
	}

	protected void Update(bool needChange)
	{
		if (updateCount == 0 && owner != null && needChange)
		{
			((SyntaxEdit)owner).UpdateCaret();
			owner.Invalidate();
		}
	}

	protected void BeginUpdate()
	{
		updateCount++;
	}

	protected void EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
	}

	protected void Update()
	{
		Update(needChange: true);
	}

	protected internal bool LineNumbersChanged()
	{
		bool flag = NeedLineNumbers();
		if (flag)
		{
			int num = lineNumberWidth;
			UpdateLineNumberLength();
			UpdateLineNumberWidth();
			flag = num != lineNumberWidth;
			if (flag && owner != null)
			{
				if (owner.WordWrap)
				{
					owner.UpdateWordWrap();
				}
				owner.Invalidate();
				((SyntaxEdit)owner).UpdateCaret();
			}
		}
		return flag;
	}

	protected internal bool IsMouseOnGutter(int x, int y, bool strictCheck)
	{
		x = ScreenToClient(x, y).X;
		if (x >= Rect.Left)
		{
			return x <= Rect.Left + (strictCheck ? GetPaintWidth() : GetWidth());
		}
		return false;
	}

	protected internal bool IsMouseOnOutlineArea(int x, int y)
	{
		if (AllowOutlining())
		{
			x = ScreenToClient(x, y).X;
			int num = GetOutlineLeft(useIndent: false) + Rect.Left;
			if (x >= num)
			{
				return x <= num + GetOutlineWidth();
			}
			return false;
		}
		return false;
	}

	protected internal bool CanDrawImage(int index, int left, int right)
	{
		if (index >= 0 && index < Images.Images.Count)
		{
			return left + Images.ImageSize.Width <= right;
		}
		return false;
	}

	protected internal bool CanDrawImage(int index)
	{
		if (index >= 0)
		{
			return index < Images.Images.Count;
		}
		return false;
	}

	protected internal bool IsMouseOnLineModificatorArea(int x, int y)
	{
		if ((options & GutterOptions.PaintLineModificators) != 0)
		{
			x = ScreenToClient(x, y).X;
			int lineModificatorLeft = GetLineModificatorLeft();
			if (x >= lineModificatorLeft)
			{
				return x <= lineModificatorLeft + GetLineModificatorWidth(checkOutlining: false);
			}
			return false;
		}
		return false;
	}

	protected internal bool IsMouseOnLineNumberArea(int x, int y)
	{
		if ((options & GutterOptions.PaintLineNumbers) != 0)
		{
			x = ScreenToClient(x, y).X;
			if (x >= GetLineNumbersLeft(useIndent: false))
			{
				return x <= GetLineNumbersRight(useIndent: false);
			}
			return false;
		}
		return false;
	}

	protected internal bool IsMouseOnGutterImage(int x, int y, out int imageIndex, out bool bookMark)
	{
		imageIndex = -1;
		bookMark = false;
		if (Visible && IsMouseOnGutter(x, y, strictCheck: true))
		{
			x = ScreenToClient(x, y).X;
			Point point = owner.ScreenToDisplay(x, y);
			Rectangle rect = Rect;
			DrawGutter(owner.Painter, new Rectangle(rect.Left, rect.Top, Width, 0), calcIndex: true, point.Y, x, ref imageIndex, ref bookMark);
		}
		return imageIndex >= 0;
	}

	protected internal bool IsMouseOnOutlineImage(int x, int y, ref Point outPt)
	{
		if (AllowOutlining())
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			outPt = owner.ScreenToDisplay(x, y);
			outPt.X = 0;
			int y2 = outPt.Y;
			outPt = displayStrings.DisplayPointToPoint(outPt);
			owner.Outlining.GetOutlineRanges(ranges, outPt.Y);
			Point ePt = displayStrings.DisplayPointToPoint(int.MaxValue, y2, wrapEnd: true, rangeStart: false, tabEnd: false);
			bool flag = false;
			foreach (IOutlineRange range in ranges)
			{
				if (CompareRange(range.StartPoint, outPt, ePt) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int l = GetOutlineLeft(useIndent: true) + Rect.Left;
				int fontHeight = owner.Painter.FontHeight;
				Point point = ScreenToClient(x, y);
				int top = Rect.Top;
				int t = ((fontHeight == 0) ? point.Y : (top + (point.Y - top) / fontHeight * fontHeight));
				int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
				CenterOutlineRect(ref l, ref t, fontHeight);
				if (point.X >= l - 1 && point.X <= l + defaultCollasedImageWidth + 1 && point.Y >= t - 1)
				{
					return point.Y <= t + defaultCollasedImageWidth + 1;
				}
				return false;
			}
		}
		return false;
	}

	public void Assign(IGutter source)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		BeginUpdate();
		try
		{
			width = source.Width;
			if (source.Brush is SolidBrush)
			{
				((SolidBrush)brush).Color = ((SolidBrush)source.Brush).Color;
			}
			Pen.Color = source.Pen.Color;
			Pen.Width = source.Pen.Width;
			Visible = source.Visible;
			LineNumbersStart = source.LineNumbersStart;
			LineNumbersLeftIndent = source.LineNumbersLeftIndent;
			LineNumbersRightIndent = source.LineNumbersRightIndent;
			LineNumbersForeColor = source.LineNumbersForeColor;
			LineNumbersBackColor = source.LineNumbersBackColor;
			LineNumbersAlignment = source.LineNumbersAlignment;
			UserMarginForeColor = source.UserMarginForeColor;
			UserMarginBackColor = source.UserMarginBackColor;
			UserMarginText = source.UserMarginText;
			UserMarginWidth = source.UserMarginWidth;
			OutliningLeftIndent = source.OutliningLeftIndent;
			OutliningRightIndent = source.OutliningRightIndent;
			Options = source.Options;
			BookMarkImageIndex = source.BookMarkImageIndex;
			WrapImageIndex = source.WrapImageIndex;
			if (images == null)
			{
				Images.ImageStream = source.Images.ImageStream;
			}
			else
			{
				Images = source.Images;
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public virtual void OnClick(EventArgs args)
	{
		if (this.Click != null)
		{
			this.Click(this, args);
		}
	}

	public virtual void OnDrawUserMargin(ref string text, int line)
	{
		if (this.DrawUserMargin != null)
		{
			DrawUserMarginEventArgs drawUserMarginEventArgs = new DrawUserMarginEventArgs(text, line);
			this.DrawUserMargin(this, drawUserMarginEventArgs);
			if (drawUserMarginEventArgs.Handled)
			{
				text = drawUserMarginEventArgs.Text;
			}
		}
	}

	public virtual void OnDoubleClick(EventArgs args)
	{
		if (this.DoubleClick != null)
		{
			this.DoubleClick(this, args);
		}
	}

	public void Paint(IPainter painter, Rectangle rect)
	{
		Paint(painter, rect, (owner != null) ? owner.Scrolling.WindowOriginY : 0);
	}

	public void Paint(IPainter painter, Rectangle rect, int startLine)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		if (!NeedPaint(ref rect))
		{
			return;
		}
		Color backColor = painter.BackColor;
		try
		{
			if (painter.FontHeight > 0)
			{
				startLine += rect.Top / painter.FontHeight;
			}
			painter.BackColor = ((brush is SolidBrush) ? ((SolidBrush)brush).Color : EditConsts.DefaultGutterBackColor);
			drawInfo.Init();
			if (owner.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter, drawInfo))
			{
				return;
			}
			DrawGutter(painter, new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height), startLine);
			int num = GetPaintWidth() - GetUserMarginWidth();
			if (Visible && num != 0)
			{
				if (!Transparent)
				{
					painter.FillRectangle(0, rect.Top, num - 1, rect.Height);
				}
				painter.DrawLine(num - 1, 0, num - 1, rect.Bottom - 1, Pen.Color, (int)Pen.Width, Pen.DashStyle);
			}
			num = GetUserMarginWidth();
			if (num != 0)
			{
				int num2 = GetPaintWidth() - num;
				if (!Transparent)
				{
					painter.BackColor = userMarginBackColor;
					painter.FillRectangle(num2, rect.Top, num, rect.Height);
				}
				painter.DrawDotLine(num2 + num - 1, 0, num2 + num - 1, rect.Bottom, userMarginForeColor);
			}
			if ((options & GutterOptions.PaintLineNumbers) != 0 && ((options & GutterOptions.PaintLinesOnGutter) == 0 || !Visible))
			{
				int lineNumbersLeft = GetLineNumbersLeft(useIndent: false);
				int lineNumbersRight = GetLineNumbersRight(useIndent: false);
				if (lineNumbersLeft < lineNumbersRight)
				{
					if (!Transparent)
					{
						painter.BackColor = lineNumbersBackColor;
						painter.FillRectangle(lineNumbersLeft, rect.Top, lineNumbersRight - lineNumbersLeft, rect.Height);
					}
					painter.DrawDotLine(lineNumbersRight - 1, 0, lineNumbersRight - 1, rect.Bottom, lineNumbersForeColor);
				}
			}
			if (!Transparent)
			{
				if ((owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == 0 || !Visible)
				{
					num = GetOutlineWidth();
					if (num != 0)
					{
						painter.BackColor = ((SyntaxEdit)owner).GetBackColor(readOnly: false);
						painter.FillRectangle(GetOutlineLeft(useIndent: false), rect.Top, num, rect.Height);
					}
				}
				num = GetLineModificatorWidth(checkOutlining: true);
				if (num != 0)
				{
					painter.BackColor = ((SyntaxEdit)owner).GetBackColor(readOnly: false);
					painter.FillRectangle(GetLineModificatorLeft(), rect.Top, num, rect.Height);
				}
			}
			owner.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter, drawInfo);
		}
		finally
		{
			painter.BackColor = backColor;
		}
	}

	public virtual void ResetWidth()
	{
		Width = EditConsts.DefaultGutterWidth;
	}

	public void ResetUserMarginWidth()
	{
		UserMarginWidth = EditConsts.DefaultUserMarginWidth;
	}

	public void ResetUserMarginForeColor()
	{
		UserMarginForeColor = EditConsts.DefaultUserMarginForeColor;
	}

	public void ResetUserMarginBackColor()
	{
		UserMarginForeColor = EditConsts.DefaultUserMarginBackColor;
	}

	public void ResetUserMarginText()
	{
		UserMarginText = EditConsts.DefaultUserMarginText;
	}

	public virtual void ResetBrushColor()
	{
		BrushColor = EditConsts.DefaultGutterBackColor;
	}

	public virtual void ResetPenColor()
	{
		PenColor = EditConsts.DefaultGutterForeColor;
	}

	public virtual void ResetLineNumbersForeColor()
	{
		LineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
	}

	public virtual void ResetLineNumbersBackColor()
	{
		LineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
	}

	public virtual void ResetLineModificatorChangedColor()
	{
		LineModificatorChangedColor = EditConsts.DefaultLineModificatorChangedColor;
	}

	public virtual void ResetLineModificatorSavedColor()
	{
		LineModificatorSavedColor = EditConsts.DefaultLineModificatorSavedColor;
	}

	public virtual void ResetVisible()
	{
		Visible = true;
	}

	public virtual void ResetLineNumbersStart()
	{
		LineNumbersStart = EditConsts.DefaultLineNumbersStart;
	}

	public virtual void ResetLineNumbersLeftIndent()
	{
		LineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
	}

	public virtual void ResetLineNumbersRightIndent()
	{
		LineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
	}

	public virtual void ResetLineNumbersAlignment()
	{
		LineNumbersAlignment = (StringAlignment)0;
	}

	public virtual void ResetOutliningLeftIndent()
	{
		OutliningLeftIndent = EditConsts.DefaultOutliningIndent;
	}

	public virtual void ResetOutliningRightIndent()
	{
		OutliningRightIndent = EditConsts.DefaultOutliningIndent;
	}

	public virtual void ResetOptions()
	{
		Options = EditConsts.DefaultGutterOptions;
	}

	public virtual void ResetBookMarkImageIndex()
	{
		BookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
	}

	public virtual void ResetWrapImageIndex()
	{
		WrapImageIndex = EditConsts.DefaultWrapImageIndex;
	}

	public virtual void ResetDrawLineBookmarks()
	{
		DrawLineBookmarks = false;
	}

	public virtual void ResetLineBookmarksColor()
	{
		LineBookmarksColor = EditConsts.DefaultLineBookmarksColor;
	}

	public void ResetShowBookmarkHints()
	{
		ShowBookmarkHints = true;
	}

	public bool ShouldSerializeBrushColor()
	{
		return BrushColor != EditConsts.DefaultGutterBackColor;
	}

	public bool ShouldSerializePenColor()
	{
		return PenColor != EditConsts.DefaultGutterForeColor;
	}

	public bool ShouldSerializeLineNumbersForeColor()
	{
		return lineNumbersForeColor != EditConsts.DefaultLineNumbersForeColor;
	}

	public bool ShouldSerializeLineNumbersBackColor()
	{
		return lineNumbersBackColor != EditConsts.DefaultLineNumbersBackColor;
	}

	public bool ShouldSerializeLineBookmarksColor()
	{
		return lineBookmarksColor != EditConsts.DefaultLineBookmarksColor;
	}

	public bool ShouldSerializeLineModificatorChangedColor()
	{
		return lineModificatorChangedColor != EditConsts.DefaultLineModificatorChangedColor;
	}

	public virtual bool ShouldSerializeLineModificatorSavedColor()
	{
		return lineModificatorSavedColor != EditConsts.DefaultLineModificatorSavedColor;
	}

	public bool ShouldSerializeWidth()
	{
		return width != EditConsts.DefaultGutterWidth;
	}

	public bool ShouldSerializeUserMarginWidth()
	{
		return userMarginWidth != EditConsts.DefaultUserMarginWidth;
	}

	public bool ShouldSerializeUserMarginForeColor()
	{
		return userMarginForeColor != EditConsts.DefaultUserMarginForeColor;
	}

	public bool ShouldSerializeUserMarginBackColor()
	{
		return userMarginBackColor != EditConsts.DefaultUserMarginBackColor;
	}

	public bool ShouldSerializeUserMarginText()
	{
		return userMarginText != EditConsts.DefaultUserMarginText;
	}

	public bool ShouldSerializeLineNumbersStart()
	{
		return lineNumbersStart != EditConsts.DefaultLineNumbersStart;
	}

	public bool ShouldSerializeLineNumbersLeftIndent()
	{
		return lineNumbersLeftIndent != EditConsts.DefaultLineNumbersIndent;
	}

	public bool ShouldSerializeLineNumbersRightIndent()
	{
		return lineNumbersRightIndent != EditConsts.DefaultLineNumbersIndent;
	}

	public bool ShouldSerializeOutliningLeftIndent()
	{
		return outliningLeftIndent != EditConsts.DefaultOutliningIndent;
	}

	public bool ShouldSerializeOutliningRightIndent()
	{
		return outliningRightIndent != EditConsts.DefaultOutliningIndent;
	}

	public bool ShouldSerializeBookMarkImageIndex()
	{
		return bookMarkImageIndex != EditConsts.DefaultBookMarkImageIndex;
	}

	public bool ShouldSerializeWrapImageIndex()
	{
		return wrapImageIndex != EditConsts.DefaultWrapImageIndex;
	}

	public bool ShouldSerializeOptions()
	{
		return options != EditConsts.DefaultGutterOptions;
	}
}
