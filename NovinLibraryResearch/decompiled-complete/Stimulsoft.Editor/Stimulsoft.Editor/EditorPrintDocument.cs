using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class EditorPrintDocument : PrintDocument
{
	private IPrinting printing;

	private ISyntaxEdit syntaxEdit;

	private bool startPrinting;

	private int page;

	private int pages;

	private int pageIndex;

	private int pageCount;

	private int startLine;

	public EditorPrintDocument(IPrinting printing, PrinterSettings settings)
	{
		this.printing = printing;
		((PrintDocument)this).PrinterSettings = settings;
	}

	private int GetPagesCount(ISyntaxEdit edit, Rectangle rect, out int linesPerPage)
	{
		edit.Bounds = rect;
		((DisplayStrings)edit.DisplayLines).ScanToEnd(parseToEnd: true);
		int num = edit.DisplayLines.GetCount() - 1;
		linesPerPage = Math.Max(edit.LinesInHeight(), 1);
		pages = num / linesPerPage;
		if (num % linesPerPage != 0)
		{
			pages++;
		}
		return pages;
	}

	private int GetPagesCount(ISyntaxEdit edit)
	{
		PageSettings defaultPageSettings = ((PrintDocument)this).PrinterSettings.DefaultPageSettings;
		Rectangle rect = new Rectangle(0, 0, defaultPageSettings.PaperSize.Width - defaultPageSettings.Margins.Left - defaultPageSettings.Margins.Right, defaultPageSettings.PaperSize.Height - defaultPageSettings.Margins.Top - defaultPageSettings.Margins.Bottom);
		int linesPerPage;
		return GetPagesCount(edit, rect, out linesPerPage);
	}

	protected override void OnBeginPrint(PrintEventArgs e)
	{
		((PrintDocument)this).OnBeginPrint(e);
		startPrinting = true;
		page = 0;
		pageIndex = 0;
		startLine = 0;
	}

	protected override void OnEndPrint(PrintEventArgs e)
	{
		((PrintDocument)this).OnEndPrint(e);
	}

	protected override void OnPrintPage(PrintPageEventArgs ev)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		((PrintDocument)this).OnPrintPage(ev);
		Rectangle marginBounds = ev.MarginBounds;
		if (startPrinting)
		{
			int linesPerPage = 0;
			pages = GetPagesCount(syntaxEdit, ev.MarginBounds, out linesPerPage);
			pageCount = Math.Max(pages, 1);
			if ((int)((PrintDocument)this).PrinterSettings.PrintRange == 2)
			{
				pages = Math.Min(pages, ((PrintDocument)this).PrinterSettings.ToPage - ((PrintDocument)this).PrinterSettings.FromPage + 1);
				pageIndex = ((PrintDocument)this).PrinterSettings.FromPage - 1;
				startLine += linesPerPage * pageIndex;
			}
			pages = Math.Max(pages, 1);
			startPrinting = false;
		}
		Rectangle rectangle = marginBounds;
		int fontHeight = syntaxEdit.Painter.FontHeight;
		if (fontHeight != 0)
		{
			rectangle.Height = rectangle.Height / fontHeight * fontHeight;
		}
		float scaleX = ev.Graphics.DpiX / 100f;
		float scaleY = ev.Graphics.DpiY / 100f;
		IPainter painter = syntaxEdit.Painter;
		painter.BeginPaint(ev.Graphics);
		try
		{
			painter.Transform(0, 0, scaleX, scaleY);
			try
			{
				if ((printing.Options & PrintOptions.UseHeader) != 0)
				{
					Rectangle rect = new Rectangle(marginBounds.Left - printing.Header.Offset.X, marginBounds.Top - (printing.Header.Font.Height + printing.Header.Offset.Y), marginBounds.Width + printing.Header.Offset.X * 2, printing.Header.Font.Height);
					printing.Header.Paint(painter, rect, pageIndex, pageCount, pageNumbers: false);
				}
				if ((printing.Options & PrintOptions.UseFooter) != 0 || (printing.Options & PrintOptions.PageNumbers) != 0)
				{
					Rectangle rect2 = new Rectangle(marginBounds.Left - printing.Footer.Offset.X, marginBounds.Bottom + printing.Footer.Offset.Y, marginBounds.Width + printing.Footer.Offset.X * 2, printing.Footer.Font.Height);
					printing.Footer.Paint(painter, rect2, pageIndex, pageCount, (printing.Options & PrintOptions.PageNumbers) != 0);
				}
			}
			finally
			{
				painter.EndTransform();
			}
			((SyntaxEdit)syntaxEdit).PaintWindow(painter, startLine, new Rectangle(new Point(0, 0), rectangle.Size), marginBounds.Location, scaleX, scaleY, specialPaint: true, inPrinting: true);
			startLine += Math.Max(syntaxEdit.LinesInHeight(), 1);
			page++;
			pageIndex++;
			ev.HasMorePages = page < pages;
		}
		finally
		{
			painter.EndPaint();
		}
	}

	public void Init(ISyntaxEdit edit)
	{
		Init(edit, calcPages: false);
	}

	public void Init(ISyntaxEdit edit, bool calcPages)
	{
		((PrintDocument)this).PrinterSettings.MinimumPage = 1;
		((PrintDocument)this).PrinterSettings.FromPage = Math.Max(((PrintDocument)this).PrinterSettings.FromPage, 1);
		((PrintDocument)this).PrinterSettings.ToPage = Math.Max(((PrintDocument)this).PrinterSettings.ToPage, 1);
		syntaxEdit = ((Printing)printing).OnCreatePrintEdit();
		if ((printing.Options & PrintOptions.PrintSelection) != 0 && !edit.Selection.IsEmpty)
		{
			syntaxEdit.Lexer = edit.Lexer;
			((SyntaxStrings)syntaxEdit.Lines).SetTextAndData(edit.Selection.SelectedText, edit.Source.NeedParse() ? ((Selection)edit.Selection).SelectedColorData : null);
		}
		else
		{
			syntaxEdit.Source = edit.Source;
		}
		syntaxEdit.BorderStyle = EditBorderStyle.None;
		syntaxEdit.Font = edit.Font;
		((SyntaxEdit)syntaxEdit).UpdateMonospaced();
		syntaxEdit.WordWrap = (printing.Options & PrintOptions.WordWrap) != 0;
		syntaxEdit.Gutter.Assign(edit.Gutter);
		syntaxEdit.Gutter.Visible = false;
		syntaxEdit.Gutter.Options = (((printing.Options & PrintOptions.LineNumbers) != 0) ? GutterOptions.PaintLineNumbers : GutterOptions.None);
		syntaxEdit.Gutter.LineNumbersBackColor = syntaxEdit.BackColor;
		if ((printing.Options & PrintOptions.PrintSelection) != 0 && !edit.Selection.IsEmpty)
		{
			syntaxEdit.Gutter.LineNumbersStart += edit.Selection.SelectionRect.Top;
		}
		syntaxEdit.EditMargin.Visible = false;
		syntaxEdit.Braces.BracesOptions = BracesOptions.None;
		if ((printing.Options & PrintOptions.UseSyntax) == 0)
		{
			syntaxEdit.DisableSyntaxPaint = true;
		}
		if ((printing.Options & PrintOptions.UseColors) == 0)
		{
			syntaxEdit.DisableColorPaint = true;
			syntaxEdit.Gutter.LineNumbersForeColor = syntaxEdit.ForeColor;
		}
		syntaxEdit.Scrolling.WindowOriginX = 0;
		syntaxEdit.Scrolling.WindowOriginY = 0;
		if (calcPages)
		{
			((PrintDocument)this).PrinterSettings.ToPage = Math.Max(GetPagesCount(syntaxEdit), 1);
		}
		((Printing)printing).OnInitialized();
	}
}
