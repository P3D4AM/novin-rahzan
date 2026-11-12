using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class EditPages : IEditPages, IEnumerable
{
	private class PageComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Rectangle pageRect = ((EditPage)x).PageRect;
			int num = (int)y;
			if (pageRect.Top > num)
			{
				return 1;
			}
			if (pageRect.Bottom <= num)
			{
				return -1;
			}
			return 0;
		}
	}

	private class PageTextComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			EditPage editPage = (EditPage)x;
			int num = (int)y;
			if (editPage.StartLine > num)
			{
				return 1;
			}
			if (editPage.EndLine < num)
			{
				return -1;
			}
			return 0;
		}
	}

	private ISyntaxEdit owner;

	private ArrayList items;

	private int updateCount;

	private IComparer pageComparer;

	private IComparer pageTextComparer;

	private bool displayWhiteSpace = true;

	private Color backColor = EditConsts.DefaultPageBackColor;

	private Color borderColor = EditConsts.DefaultPageBorderColor;

	private PageType pageType;

	private PrinterSettings printerSettings;

	private Size defaultPageSize;

	private Margins defaultMargins = new Margins(EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin);

	private bool defaultLandscape;

	private PaperKind defaultPageKind;

	private IEditRuler horzRuler;

	private IEditRuler vertRuler;

	private RulerOptions rulerOptions;

	private RulerUnits rulerUnits;

	private EditRulers rulers;

	private bool applyRulerToAllPages;

	private bool transparent;

	private bool initialized;

	private Size caps;

	protected internal ISyntaxEdit Owner => owner;

	protected internal Size DefaultPageSize => defaultPageSize;

	protected internal Margins DefaultMargins => defaultMargins;

	protected internal bool DefaultLandscape => defaultLandscape;

	protected internal PaperKind DefaultPageKind => defaultPageKind;

	protected internal Size Caps => caps;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IEditPage this[int index]
	{
		get
		{
			return items[index] as IEditPage;
		}
		set
		{
			if (items[index] != value)
			{
				items[index] = value;
				((EditPage)value).Pages = this;
				Update();
			}
		}
	}

	[DefaultValue(PageType.Normal)]
	[Description("Gets or sets value specifying the way of viewing Edit control's content.")]
	public PageType PageType
	{
		get
		{
			return pageType;
		}
		set
		{
			if (pageType != value)
			{
				pageType = value;
				InitPages();
				RecalculatePages();
				UpdateRulerAllowDrag();
			}
		}
	}

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets properties for default \"IEditPage\" object in the pages collection.")]
	public IEditPage DefaultPage
	{
		get
		{
			if (Count == 0)
			{
				Add();
			}
			return this[0];
		}
		set
		{
			DefaultPage.Assign(value);
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a boolean value that indicates whether Edit control should draw whitespace area between pages.")]
	public bool DisplayWhiteSpace
	{
		get
		{
			return displayWhiteSpace;
		}
		set
		{
			if (displayWhiteSpace != value)
			{
				displayWhiteSpace = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a background color of each page in the collection.")]
	public Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			if (backColor != value)
			{
				backColor = value;
				owner.Invalidate();
			}
		}
	}

	[Description("Gets or sets a border color of each page in the collection.")]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			if (borderColor != value)
			{
				borderColor = value;
				owner.Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Width => DefaultPage.BoundsRect.Width + DefaultPage.HorzOffset * 2;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Height
	{
		get
		{
			IEditPage editPage = ((Count > 0) ? this[Count - 1] : DefaultPage);
			return editPage.Origin.Y + editPage.BoundsRect.Height + editPage.VertOffset;
		}
	}

	[Description("Gets or sets background color of the pages rulers.")]
	public Color RulerBackColor
	{
		get
		{
			if (horzRuler != null)
			{
				return horzRuler.BackColor;
			}
			if (vertRuler != null)
			{
				return vertRuler.BackColor;
			}
			return EditConsts.DefaultRulerBackColor;
		}
		set
		{
			HorzRuler.BackColor = value;
			VertRuler.BackColor = value;
		}
	}

	[Description("Gets or sets background color of the indentations parts of the pages rulers.")]
	public Color RulerIndentBackColor
	{
		get
		{
			if (horzRuler != null)
			{
				return horzRuler.IndentBackColor;
			}
			if (vertRuler != null)
			{
				return vertRuler.IndentBackColor;
			}
			return EditConsts.DefaultRulerIndentBackColor;
		}
		set
		{
			HorzRuler.IndentBackColor = value;
			VertRuler.IndentBackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IEditRuler HorzRuler
	{
		get
		{
			if (horzRuler == null)
			{
				horzRuler = new EditRuler();
				horzRuler.Visible = false;
				horzRuler.Parent = (Control)(object)(SyntaxEdit)owner;
				((Control)(EditRuler)horzRuler).SendToBack();
				horzRuler.Location = new Point(0, 0);
				horzRuler.Width = owner.ClientRect.Width;
				((Control)(EditRuler)horzRuler).Anchor = (AnchorStyles)13;
				horzRuler.Change += RulerChanged;
				UpdateRulerAllowDrag();
			}
			return horzRuler;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IEditRuler VertRuler
	{
		get
		{
			if (vertRuler == null)
			{
				vertRuler = new EditRuler();
				vertRuler.Vertical = true;
				vertRuler.Visible = false;
				vertRuler.Parent = (Control)(object)(SyntaxEdit)owner;
				((Control)(EditRuler)vertRuler).SendToBack();
				vertRuler.Change += RulerChanged;
				vertRuler.Location = new Point(0, 0);
				vertRuler.Height = owner.ClientRect.Height;
				((Control)(EditRuler)vertRuler).Anchor = (AnchorStyles)7;
				UpdateRulerAllowDrag();
			}
			return vertRuler;
		}
	}

	[DefaultValue(RulerUnits.Inches)]
	[Description("Gets or sets measurement units of the pages rulers.")]
	public RulerUnits RulerUnits
	{
		get
		{
			return rulerUnits;
		}
		set
		{
			if (rulerUnits != value)
			{
				rulerUnits = value;
				if (horzRuler != null)
				{
					horzRuler.Units = value;
				}
				if (vertRuler != null)
				{
					vertRuler.Units = value;
				}
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets options of the pages rulers.")]
	public RulerOptions RulerOptions
	{
		get
		{
			return rulerOptions;
		}
		set
		{
			if (rulerOptions != value)
			{
				rulerOptions = value;
				if (horzRuler != null)
				{
					horzRuler.Options = value;
				}
				if (vertRuler != null)
				{
					vertRuler.Options = value;
				}
			}
		}
	}

	[DefaultValue(EditRulers.None)]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets the rulers displayed withing Edit control.")]
	public EditRulers Rulers
	{
		get
		{
			return rulers;
		}
		set
		{
			if (rulers != value)
			{
				rulers = value;
				if ((rulers & EditRulers.Horizonal) != 0)
				{
					HorzRuler.Visible = true;
				}
				else if (horzRuler != null)
				{
					horzRuler.Visible = false;
				}
				if ((rulers & EditRulers.Vertical) != 0)
				{
					VertRuler.Visible = true;
				}
				else if (vertRuler != null)
				{
					vertRuler.Visible = false;
				}
				UpdateRulerSize();
				RecalculatePages();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Count => items.Count;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IList Items => items;

	[Description("Gets or sets kind of the pages specifying standard paper size.")]
	public PaperKind PageKind
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return DefaultPage.PageKind;
		}
		set
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			UpdatePageSize(update: true);
			Size pageSize = new Size(0, 0);
			foreach (PaperSize paperSize in printerSettings.PaperSizes)
			{
				PaperSize val = paperSize;
				if (val.Kind == value)
				{
					pageSize = new Size(val.Width, val.Height);
					break;
				}
			}
			if (pageSize.IsEmpty)
			{
				return;
			}
			BeginUpdate();
			try
			{
				IEnumerator enumerator2 = GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						IEditPage editPage = (IEditPage)enumerator2.Current;
						editPage.PageKind = value;
						editPage.PageSize = pageSize;
					}
				}
				finally
				{
					IDisposable disposable = enumerator2 as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			finally
			{
				EndUpdate();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a boolean value that indicates that changing of rulers indentation applies to all pages rather than to current page.")]
	public bool ApplyRulerToAllPages
	{
		get
		{
			return applyRulerToAllPages;
		}
		set
		{
			applyRulerToAllPages = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a boolean value that indicates whether Edit control should draw background around the pages.")]
	public bool Transparent
	{
		get
		{
			return transparent;
		}
		set
		{
			if (transparent != value)
			{
				transparent = value;
				if (owner != null)
				{
					((SyntaxEdit)owner).TransparentChanged();
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
			return new XmlEditPagesInfo(this);
		}
		set
		{
			((XmlEditPagesInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event DrawHeaderEvent DrawHeader;

	public EditPages()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		items = new ArrayList();
		pageComparer = new PageComparer();
		pageTextComparer = new PageTextComparer();
		rulerUnits = RulerUnits.Inches;
		rulerOptions = EditConsts.DefaultRulerOptions;
		caps = InitCaps();
		InitDefaultPageSettings();
	}

	public EditPages(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	[Description("Returns an enumerator that can iterate through a collection.")]
	public IEnumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}

	private Size InitCaps()
	{
		return OSUtils.GetScreenCaps();
	}

	protected internal void InitDefaultPageSettings()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		defaultPageSize = new Size(827 * caps.Width / 100, 1169 * caps.Height / 100);
		defaultPageKind = (PaperKind)0;
		defaultMargins = new Margins(100, 100, 100, 100);
		defaultLandscape = false;
	}

	protected internal void InitPages()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (Count <= 0)
		{
			return;
		}
		BeginUpdate();
		try
		{
			for (int i = 0; i < Count; i++)
			{
				IEditPage editPage = this[i];
				if (editPage.UsePrinterSettings)
				{
					editPage.Margins = defaultMargins;
					editPage.PageKind = defaultPageKind;
					editPage.PageSize = defaultPageSize;
					editPage.Landscape = defaultLandscape;
				}
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	protected internal void InitDefaultPageSettings(PageSettings pageSettings, bool updateSize)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (updateSize)
			{
				PaperSize paperSize = pageSettings.PaperSize;
				defaultPageSize = new Size(paperSize.Width, paperSize.Height);
				defaultPageKind = paperSize.Kind;
				defaultMargins = pageSettings.Margins;
				defaultLandscape = pageSettings.Landscape;
			}
			else
			{
				InitDefaultPageSettings();
			}
		}
		catch
		{
			InitDefaultPageSettings();
		}
	}

	protected internal void InitDefaultPrinterSettings(PrinterSettings printerSettings)
	{
		this.printerSettings = printerSettings;
		InitDefaultPageSettings(printerSettings.DefaultPageSettings, updateSize: false);
	}

	protected internal void UpdatePageSize()
	{
		UpdatePageSize(update: false);
	}

	protected internal void UpdatePageSize(bool update)
	{
		if (!initialized && printerSettings != null && (update || PageType != 0 || rulers != 0))
		{
			InitDefaultPageSettings(printerSettings.DefaultPageSettings, updateSize: true);
			initialized = true;
		}
	}

	protected internal void UpdatePageSize(PaperKind pageKind, ref Size size)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (updateCount != 0 || printerSettings == null)
		{
			return;
		}
		foreach (PaperSize paperSize in printerSettings.PaperSizes)
		{
			PaperSize val = paperSize;
			if (val.Kind == pageKind)
			{
				size = new Size(val.Width, val.Height);
				UpdateScroll();
				break;
			}
		}
	}

	protected internal EditPage GetPage(int index)
	{
		if (Count == 0)
		{
			Add();
		}
		if (index < 0)
		{
			index = 0;
		}
		while (index >= Count)
		{
			Add().Assign(this[Count - 2]);
		}
		return this[index] as EditPage;
	}

	protected internal void UpdateRulers()
	{
		if (owner == null || rulers == EditRulers.None)
		{
			return;
		}
		IEditPage pageAt = GetPageAt(owner.DisplayLines.PointToDisplayPoint(owner.Position));
		if (pageAt != null)
		{
			Rectangle boundsRect = pageAt.BoundsRect;
			Rectangle clientRect = pageAt.ClientRect;
			if ((rulers & EditRulers.Horizonal) != 0)
			{
				IEditRuler editRuler = HorzRuler;
				editRuler.RulerStart = boundsRect.Left - editRuler.Left;
				editRuler.RulerWidth = boundsRect.Width;
				editRuler.PageStart = clientRect.Left - editRuler.Left;
				editRuler.PageWidth = clientRect.Width;
				editRuler.MarkWidth = owner.Painter.FontWidth;
				editRuler.Update();
			}
			if ((rulers & EditRulers.Vertical) != 0)
			{
				IEditRuler editRuler2 = VertRuler;
				editRuler2.RulerStart = boundsRect.Top - editRuler2.Top;
				editRuler2.RulerWidth = boundsRect.Height;
				editRuler2.PageStart = clientRect.Top - editRuler2.Top;
				editRuler2.PageWidth = clientRect.Height;
				editRuler2.MarkWidth = owner.Painter.FontWidth;
				editRuler2.Update();
			}
		}
	}

	protected internal void UpdatePages(int startIndex, int endIndex, bool changed)
	{
		if (updateCount > 0 && changed)
		{
			return;
		}
		updateCount++;
		try
		{
			int num = Math.Max(startIndex, 0);
			int count = owner.DisplayLines.GetCount();
			int num2 = 0;
			bool flag = pageType == PageType.PageLayout;
			bool flag2 = displayWhiteSpace && flag;
			int num3 = 0;
			if (num > 0)
			{
				EditPage editPage = (EditPage)this[num - 1];
				num3 = editPage.GetBounds(includeSpace: true).Bottom;
				num2 = editPage.EndLine + 1;
			}
			if (changed)
			{
				owner.UpdateWordWrap(num2, int.MaxValue);
			}
			int fontHeight = owner.Painter.FontHeight;
			while ((num2 < count || count == 0) && num <= endIndex)
			{
				EditPage page = GetPage(num);
				page.StartLine = num2;
				int num4 = Math.Max((fontHeight <= 0) ? 1 : (page.ClientRect.Height / fontHeight), 1);
				page.EndLine = num2 + num4 - 1;
				int num5 = (((rulers & EditRulers.Vertical) != 0) ? EditConsts.DefaultRulerHeight : 0);
				int num6 = ((num == 0 && (rulers & EditRulers.Horizonal) != 0) ? EditConsts.DefaultRulerHeight : 0);
				page.Origin = new Point((flag ? page.HorzOffset : 0) + num5, num3 + (flag2 ? page.VertOffset : ((num == 0 && flag) ? 4 : 0)) + num6);
				num3 += page.GetBounds(includeSpace: true).Height + num6;
				num2 += num4;
				if (num2 >= count)
				{
					break;
				}
				num++;
			}
			if (endIndex == int.MaxValue && ((DisplayStrings)owner.DisplayLines).AlreadyScanned())
			{
				for (int num7 = Count - 1; num7 > num; num7--)
				{
					items.RemoveAt(num7);
				}
			}
			if (changed)
			{
				UpdateRulers();
			}
		}
		finally
		{
			updateCount--;
		}
	}

	protected virtual void RulerChanged(object sender, EventArgs e)
	{
		if (owner == null || !(sender is IEditRuler))
		{
			return;
		}
		int num;
		int num2;
		if (ApplyRulerToAllPages)
		{
			num = 0;
			num2 = Count - 1;
		}
		else if (owner.Selection.IsEmpty)
		{
			num = GetPageIndexAt(owner.DisplayLines.PointToDisplayPoint(owner.Position));
			num2 = num;
		}
		else
		{
			Rectangle selectionRect = owner.Selection.SelectionRect;
			num = GetPageIndexAt(owner.DisplayLines.PointToDisplayPoint(selectionRect.Location));
			num2 = GetPageIndexAt(owner.DisplayLines.PointToDisplayPoint(selectionRect.Right, selectionRect.Bottom));
		}
		BeginUpdate();
		try
		{
			IEditRuler editRuler = (IEditRuler)sender;
			RulerIndent rulerIndent = ((RulerEventArgs)e).Object as RulerIndent;
			for (int i = Math.Max(num, 0); i <= num2; i++)
			{
				EditPage editPage = this[i] as EditPage;
				if (editRuler.Vertical)
				{
					if (rulerIndent.Orientation == IndentOrientation.Near)
					{
						editPage.TopIndent = rulerIndent.Indent;
					}
					else
					{
						editPage.BottomIndent = rulerIndent.Indent;
					}
				}
				else if (rulerIndent.Orientation == IndentOrientation.Near)
				{
					editPage.LeftIndent = rulerIndent.Indent;
				}
				else
				{
					editPage.RightIndent = rulerIndent.Indent;
				}
			}
			((SyntaxEdit)owner).UpdateCaret();
		}
		finally
		{
			EndUpdate();
		}
	}

	protected internal void UpdateRulerSize()
	{
		Rectangle clientRectangle = owner.ClientRectangle;
		if ((rulers & EditRulers.Horizonal) != 0 && (rulers & EditRulers.Vertical) != 0)
		{
			VertRuler.Top = HorzRuler.Height;
			VertRuler.Height = clientRectangle.Height - VertRuler.Top;
			HorzRuler.Left = VertRuler.Width;
			HorzRuler.Width = clientRectangle.Width - HorzRuler.Left;
		}
		else if ((rulers & EditRulers.Horizonal) != 0)
		{
			HorzRuler.Left = 0;
			HorzRuler.Width = clientRectangle.Width;
		}
		else if ((rulers & EditRulers.Vertical) != 0)
		{
			VertRuler.Top = 0;
			VertRuler.Height = clientRectangle.Height;
		}
	}

	protected internal bool IsTransparent(bool nonClient)
	{
		if (!nonClient || !transparent)
		{
			if (owner != null)
			{
				return owner.Transparent;
			}
			return false;
		}
		return true;
	}

	protected void UpdateScroll()
	{
		((Scrolling)((SyntaxEdit)owner).Scrolling).UpdateScroll();
	}

	protected void RecalculatePages()
	{
		UpdatePageSize();
		Update();
		Invalidate(null);
		if (owner != null)
		{
			((SyntaxEdit)owner).UpdateCaret();
			UpdateScroll();
		}
	}

	protected void UpdateRulerAllowDrag()
	{
		if (horzRuler != null)
		{
			if (pageType == PageType.PageLayout && (rulerOptions & RulerOptions.AllowDrag) != 0)
			{
				horzRuler.Options |= RulerOptions.AllowDrag;
			}
			else
			{
				horzRuler.Options &= ~RulerOptions.AllowDrag;
			}
		}
		if (vertRuler != null)
		{
			if (pageType == PageType.PageLayout && (rulerOptions & RulerOptions.AllowDrag) != 0)
			{
				vertRuler.Options |= RulerOptions.AllowDrag;
			}
			else
			{
				vertRuler.Options &= ~RulerOptions.AllowDrag;
			}
		}
	}

	public void InitDefaultPageSettings(PageSettings pageSettings)
	{
		InitDefaultPageSettings(pageSettings, updateSize: true);
	}

	public void Paint(IPainter painter, Rectangle rect)
	{
		if (Count == 0)
		{
			return;
		}
		int num = 0;
		for (int i = Math.Max(GetPageIndexAtPoint(rect.Location), 0); i < Count; i++)
		{
			IEditPage editPage = this[i];
			editPage.Paint(painter);
			num = editPage.BoundsRect.Bottom + 1;
			if (displayWhiteSpace)
			{
				num += 2;
			}
			if (num >= rect.Bottom)
			{
				return;
			}
		}
		if (pageType != PageType.PageLayout || num >= rect.Bottom || IsTransparent(nonClient: true))
		{
			return;
		}
		Rectangle rect2 = new Rectangle(rect.Left, num, rect.Width, rect.Bottom - num);
		Color color = painter.BackColor;
		painter.BackColor = BackColor;
		try
		{
			painter.FillRectangle(rect2);
		}
		finally
		{
			painter.BackColor = color;
		}
	}

	public int Add(IEditPage page)
	{
		((EditPage)page).Pages = this;
		((EditPage)page).Index = Count;
		return items.Add(page);
	}

	public void Remove(IEditPage page)
	{
		((EditPage)page).Pages = null;
		items.Remove(page);
	}

	public void Clear()
	{
		items.Clear();
	}

	public int BeginUpdate()
	{
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
		return updateCount;
	}

	public void Update(IEditPage page)
	{
		Update(page, changed: false);
	}

	public void Invalidate(IEditPage page)
	{
		if (updateCount == 0)
		{
			if (page != null)
			{
				owner.Invalidate(((EditPage)page).PageRect);
			}
			else
			{
				owner.Invalidate();
			}
		}
	}

	public void Update(IEditPage page, bool changed)
	{
		UpdatePages(page.Index, int.MaxValue, changed);
	}

	public void Update()
	{
		UpdatePages(0, int.MaxValue, changed: true);
		Invalidate(null);
	}

	public IEditPage Add()
	{
		IEditPage editPage = new EditPage(this);
		Add(editPage);
		return editPage;
	}

	public int GetPageIndexAtPoint(int x, int y)
	{
		int num = items.BinarySearch(y, pageComparer);
		if (num >= 0)
		{
			return num;
		}
		if (Count > 0 && y < ((EditPage)this[0]).PageRect.Top)
		{
			return 0;
		}
		return Count - 1;
	}

	public int GetPageIndexAtPoint(Point point)
	{
		return GetPageIndexAtPoint(point.X, point.Y);
	}

	public int GetPageIndexAt(int x, int y)
	{
		int num = items.BinarySearch(y, pageTextComparer);
		if (num >= 0)
		{
			return num;
		}
		if (y < 0)
		{
			return 0;
		}
		return Count - 1;
	}

	public int GetPageIndexAt(Point point)
	{
		return GetPageIndexAt(point.X, point.Y);
	}

	public int GetPageIndexAtCursor()
	{
		if (owner == null)
		{
			return -1;
		}
		return GetPageIndexAt(owner.DisplayLines.PointToDisplayPoint(owner.Position));
	}

	public IEditPage GetPageAt(int x, int y)
	{
		return GetPage(GetPageIndexAt(x, y));
	}

	public IEditPage GetPageAt(Point point)
	{
		return GetPage(GetPageIndexAt(point));
	}

	public IEditPage GetPageAtCursor()
	{
		return GetPage(GetPageIndexAtCursor());
	}

	public IEditPage GetPageAtPoint(int x, int y)
	{
		return GetPage(GetPageIndexAtPoint(x, y));
	}

	public IEditPage GetPageAtPoint(Point point)
	{
		return GetPage(GetPageIndexAtPoint(point));
	}

	public void OnDrawHeader(ref string text)
	{
		if (this.DrawHeader != null)
		{
			DrawHeaderEventArgs drawHeaderEventArgs = new DrawHeaderEventArgs(text);
			this.DrawHeader(this, drawHeaderEventArgs);
			if (drawHeaderEventArgs.Handled)
			{
				text = drawHeaderEventArgs.Text;
			}
		}
	}

	public void CancelDragging()
	{
		if (horzRuler != null)
		{
			horzRuler.CancelDragging();
		}
		if (vertRuler != null)
		{
			vertRuler.CancelDragging();
		}
	}

	public void ResetPageType()
	{
		PageType = PageType.Normal;
	}

	public void ResetBackColor()
	{
		BackColor = EditConsts.DefaultPageBackColor;
	}

	public void ResetBorderColor()
	{
		BorderColor = EditConsts.DefaultPageBorderColor;
	}

	public void ResetDisplayWhiteSpace()
	{
		DisplayWhiteSpace = true;
	}

	public void ResetRulerUnits()
	{
		RulerUnits = RulerUnits.Inches;
	}

	public void ResetRulerOptions()
	{
		RulerOptions = EditConsts.DefaultRulerOptions;
	}

	public void ResetRulers()
	{
		Rulers = EditRulers.None;
	}

	public void ResetRulerBackColor()
	{
		RulerBackColor = EditConsts.DefaultRulerBackColor;
	}

	public void ResetRulerIndentBackColor()
	{
		RulerIndentBackColor = EditConsts.DefaultRulerIndentBackColor;
	}

	public bool ShouldSerializeBackColor()
	{
		return backColor != EditConsts.DefaultPageBackColor;
	}

	public bool ShouldSerializeBorderColor()
	{
		return borderColor != EditConsts.DefaultPageBorderColor;
	}

	public bool ShouldSerializeRulerOptions()
	{
		return rulerOptions != EditConsts.DefaultRulerOptions;
	}

	public bool ShouldSerializePageKind()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return PageKind != DefaultPageKind;
	}

	public bool ShouldSerializeRulerBackColor()
	{
		return RulerBackColor != EditConsts.DefaultRulerBackColor;
	}

	public bool ShouldSerializeRulerIndentBackColor()
	{
		return RulerIndentBackColor != EditConsts.DefaultRulerIndentBackColor;
	}
}
