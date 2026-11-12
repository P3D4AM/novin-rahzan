using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Stimulsoft.Editor;

public class DisplayStrings : IDisplayStrings, IEnumerator, IStringList, ICollection, IEnumerable, IImport, IExport, ITabulation, IWordWrap, ITextSearch, IWordBreak, ICollapsable, IFmtExport, IFmtImport
{
	private class WrapComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point point = (Point)x;
			Point point2 = (Point)y;
			int num = point.Y - point2.Y;
			if (num == 0)
			{
				num = point.X - point2.X;
			}
			return num;
		}
	}

	private class LineWrapComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((Point)x).Y - (int)y;
		}
	}

	private class RealWrapComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point point = (Point)x;
			Point point2 = (Point)y;
			int num = point.Y - point2.Y;
			if (num == 0)
			{
				return Math.Max(point.X - point2.X, 0);
			}
			return Math.Max(num, 0);
		}
	}

	private class DisplayWrapComparer : IComparer
	{
		private SortList list;

		public DisplayWrapComparer(SortList list)
		{
			this.list = list;
		}

		public int Compare(object x, object y)
		{
			return Math.Max(((Point)x).Y + list.CompareIndex + 1 - (int)y, 0);
		}
	}

	private class WrapList : SortList
	{
		private IComparer wrapComparer;

		private IComparer lineWrapComparer;

		private IComparer realWrapComparer;

		private IComparer displayWrapComparer;

		private DisplayStrings owner;

		public WrapList(DisplayStrings owner)
		{
			this.owner = owner;
			wrapComparer = new WrapComparer();
			lineWrapComparer = new LineWrapComparer();
			realWrapComparer = new RealWrapComparer();
			displayWrapComparer = new DisplayWrapComparer(this);
		}

		private void GetPosAndLen(int index, int idx, ref int p, ref int len)
		{
			if (idx >= 0)
			{
				Point point = (Point)this[idx];
				if (point.Y == index - idx - 1)
				{
					p = point.X;
				}
			}
			if (idx + 1 < Count)
			{
				Point point2 = (Point)this[idx + 1];
				if (point2.Y == index - idx - 1)
				{
					len = point2.X - p;
				}
			}
		}

		public void GetWrapBounds(int index, ref int p, ref int len)
		{
			if (Count > 0)
			{
				if (!FindLast(index, out var index2, displayWrapComparer))
				{
					index2 = -1;
				}
				GetPosAndLen(index, index2, ref p, ref len);
			}
		}

		public void GetRealPoint(ref Point point, bool checkEnd, ref bool lineEnd)
		{
			owner.EnsureWrapped(point.Y, wrapped: true);
			int y = point.Y;
			if (FindLast(point.Y, out var index, displayWrapComparer))
			{
				Point point2 = (Point)this[index];
				point.Y -= index + 1;
				if (point2.Y == point.Y && point.X != int.MaxValue)
				{
					point.X += point2.X;
				}
			}
			else
			{
				index = -1;
			}
			lineEnd = false;
			int p = 0;
			int len = int.MaxValue;
			GetPosAndLen(y, index, ref p, ref len);
			if (len != int.MaxValue)
			{
				lineEnd = point.X >= p + len;
				if (checkEnd & lineEnd)
				{
					point.X = p + len;
				}
			}
		}

		public void GetDisplayPoint(ref Point point, bool lineEnd)
		{
			owner.EnsureWrapped(point.Y, wrapped: false);
			if (!FindLast(point, out var index, realWrapComparer))
			{
				return;
			}
			Point point2 = (Point)this[index];
			if (lineEnd && point2.X == point.X && point2.Y == point.Y)
			{
				if (index > 0)
				{
					point2 = (Point)this[index - 1];
					if (point2.Y == point.Y)
					{
						point.X -= point2.X;
					}
				}
				point.Y += index;
			}
			else
			{
				if (point2.Y == point.Y && point.X != int.MaxValue)
				{
					point.X -= point2.X;
				}
				point.Y += index + 1;
			}
		}

		public int LineToDisplayLine(int index)
		{
			Point point = new Point(int.MaxValue, index);
			if (FindLast(point, out var index2, realWrapComparer))
			{
				return index + index2 + 1;
			}
			return index;
		}

		public int DisplayLineToLine(int index)
		{
			if (FindLast(index, out var index2, displayWrapComparer))
			{
				return index - (index2 + 1);
			}
			return index;
		}

		public Point AddItem(int line, int ch)
		{
			Point point = new Point(ch, line);
			if (FindExact(point, out var index, wrapComparer))
			{
				this[index] = point;
			}
			else
			{
				Insert(index, point);
			}
			return point;
		}

		public void ClearLines(int first, int last)
		{
			FindLast(last, out var index, lineWrapComparer);
			for (int num = Math.Min(index, Count - 1); num >= 0; num--)
			{
				int y = ((Point)this[num]).Y;
				if (y >= first && y <= last)
				{
					RemoveAt(num);
				}
				else if (y < first)
				{
					break;
				}
			}
		}
	}

	private int currentIndex;

	private bool allowOutlining;

	private OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;

	private OutlineList outlineList;

	private WrapList wrapList;

	private bool wordWrap;

	private bool wrapAtMargin;

	private int lastWrapped = -1;

	private int lastDisplayIndex = -1;

	private int lastWrapIndex = -1;

	private int wrapMargin;

	private char[] tabArray = new char[1] { '\t' };

	private int maxLineWidth;

	private int maxLineIndex = -1;

	private bool maxLineFlag = true;

	private ISyntaxStrings lines;

	private ISyntaxEdit owner;

	private bool atLineEnd;

	private int lineEndUpdateCount;

	private ArrayList collapsedList;

	[Description("Gets an object that represents current string.")]
	public object Current
	{
		get
		{
			if (currentIndex >= 0 && currentIndex < GetCount())
			{
				return this[currentIndex];
			}
			return null;
		}
	}

	[Description("Gets number of lines displayed in Edit control.")]
	public int Count
	{
		get
		{
			ScanToEnd(parseToEnd: false);
			return GetCount();
		}
	}

	[Description("Gets a value indicating whether access to the string collection is synchronized.")]
	public bool IsSynchronized => lines.IsSynchronized;

	[Description("Gets an object that can be used to synchronize access to the \"DisplayStrings\" object.")]
	public object SyncRoot => lines.SyncRoot;

	public int CollapsedCount
	{
		get
		{
			if (!allowOutlining)
			{
				return 0;
			}
			return outlineList.CollapsedList.Count;
		}
	}

	[Description("Keeps track of calls to \"BeginUndoUpdate\" and \"EndUndoUpdate\" so that they can be nested.")]
	public int UpdateCount => outlineList.UpdateCount;

	[Description("Gets or sets a value indicating whether outlining is enabled.")]
	public bool AllowOutlining
	{
		get
		{
			return allowOutlining;
		}
		set
		{
			if (allowOutlining != value)
			{
				allowOutlining = value;
				if (!value)
				{
					UnOutline();
				}
				else
				{
					owner.Outlining.OutlineText();
				}
			}
			if (owner != null)
			{
				owner.Invalidate();
			}
		}
	}

	[Description("Gets or sets options representing outlining appearance and behaviour.")]
	public OutlineOptions OutlineOptions
	{
		get
		{
			return outlineOptions;
		}
		set
		{
			if (outlineOptions != value)
			{
				outlineList.BeginUpdate();
				try
				{
					outlineOptions = value;
				}
				finally
				{
					outlineList.EndUpdate();
				}
			}
		}
	}

	[Description("Gets position of the wrap margin.")]
	public int WrapMargin
	{
		get
		{
			if (wrapAtMargin)
			{
				return owner.EditMargin.Position * owner.Painter.FontWidth;
			}
			return owner.ClientRect.Width - ((Gutter)owner.Gutter).GetWidth();
		}
	}

	[Description("Gets or sets a value indicating whether a \"DisplayStrings\" object automatically wraps words to the beginning of the next line when necessary.")]
	public bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			if (wordWrap != value)
			{
				wordWrap = value;
				UpdateWordWrap();
			}
		}
	}

	[Description("Gets or sets a value indicating whether a \"DisplayStrings\" object automatically wraps words at margin position.")]
	public bool WrapAtMargin
	{
		get
		{
			return wrapAtMargin;
		}
		set
		{
			if (wrapAtMargin != value)
			{
				wrapAtMargin = value;
				if (wordWrap)
				{
					UpdateWordWrap();
				}
			}
		}
	}

	[Description("Gets or sets an array of chars used to separate words in a text.")]
	public char[] Delimiters
	{
		get
		{
			return lines.Delimiters;
		}
		set
		{
			lines.Delimiters = value;
		}
	}

	[Description("Gets or sets \"Delimiters\" as a single string.")]
	public string DelimiterString
	{
		get
		{
			return lines.DelimiterString;
		}
		set
		{
			lines.DelimiterString = value;
		}
	}

	[Description("Represents \">Delimiters\" as a key-and-value pairs collection where each key is a single delimiter character.")]
	public Hashtable DelimTable => lines.DelimTable;

	[Description("Gets or sets the character columns that the cursor will move to each time you press Tab.")]
	public int[] TabStops
	{
		get
		{
			return lines.TabStops;
		}
		set
		{
			lines.TabStops = value;
		}
	}

	[Description("Gets or sets a value indicating whether indent operations insert space characters rather than TAB characters.")]
	public bool UseSpaces
	{
		get
		{
			return lines.UseSpaces;
		}
		set
		{
			lines.UseSpaces = value;
		}
	}

	[Description("Gets or sets a string value that terminates line.")]
	public string LineTerminator
	{
		get
		{
			return lines.LineTerminator;
		}
		set
		{
			lines.LineTerminator = value;
		}
	}

	protected internal bool AtLineEnd
	{
		get
		{
			return atLineEnd;
		}
		set
		{
			atLineEnd = value;
		}
	}

	protected internal int LineEndUpdateCount
	{
		get
		{
			return lineEndUpdateCount;
		}
		set
		{
			lineEndUpdateCount = value;
		}
	}

	[Description("Gets width of the largest line in the \"DisplayString\" object.")]
	public int MaxLineWidth
	{
		get
		{
			if (NeedMaxLine() && maxLineFlag)
			{
				RecalcMaxLine();
			}
			return maxLineWidth;
		}
	}

	[Description("Gets or sets the collection of underlying \"real\" collection of text lines.")]
	public ISyntaxStrings Lines
	{
		get
		{
			return lines;
		}
		set
		{
			if (lines != value)
			{
				lines = value;
				LinesChanged();
			}
		}
	}

	public string this[int index]
	{
		get
		{
			string line = string.Empty;
			short[] data = null;
			GetDisplayString(index, ref line, ref data, needData: false);
			return line;
		}
	}

	public string Text
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			string lineTerminator = lines.LineTerminator;
			for (int i = 0; i < GetCount(); i++)
			{
				stringBuilder.Append(this[i] + lineTerminator);
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - lineTerminator.Length, lineTerminator.Length);
			}
			return stringBuilder.ToString();
		}
		set
		{
		}
	}

	public object XmlInfo
	{
		get
		{
			return new XmlDisplayStringsInfo(this);
		}
		set
		{
			((XmlDisplayStringsInfo)value).FixupReferences(this);
		}
	}

	public DisplayStrings(ISyntaxEdit owner, ISyntaxStrings strings)
	{
		this.owner = owner;
		outlineList = new OutlineList(this);
		wrapList = new WrapList(this);
		collapsedList = new ArrayList();
		Lines = strings;
	}

	public void CopyTo(Array array, int index)
	{
		lines.CopyTo(array, index);
	}

	public IEnumerator GetEnumerator()
	{
		return this;
	}

	public bool MoveNext()
	{
		currentIndex++;
		return currentIndex < GetCount();
	}

	public void Reset()
	{
		currentIndex = 0;
	}

	public int GetCount()
	{
		return lines.Count + wrapList.Count - outlineList.CollapsedCount;
	}

	public int GetOutlineLevel(Point point)
	{
		IOutlineRange outlineRange = GetOutlineRange(point);
		if (outlineRange == null)
		{
			return 0;
		}
		return outlineRange.Level + 1;
	}

	public IOutlineRange Outline(int first, int last, int level)
	{
		return Outline(new Point(0, first), new Point(int.MaxValue, last), level, EditConsts.DefaultOutlineText);
	}

	public IOutlineRange Outline(int first, int last)
	{
		return Outline(new Point(0, first), new Point(int.MaxValue, last), GetOutlineLevel(new Point(0, first)), EditConsts.DefaultOutlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, int level)
	{
		return Outline(startPoint, endPoint, level, EditConsts.DefaultOutlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint)
	{
		return Outline(startPoint, endPoint, GetOutlineLevel(startPoint), EditConsts.DefaultOutlineText);
	}

	public IOutlineRange Outline(int first, int last, string outlineText)
	{
		return Outline(new Point(0, first), new Point(int.MaxValue, last), GetOutlineLevel(new Point(0, first)), outlineText);
	}

	public IOutlineRange Outline(int first, int last, int level, string outlineText)
	{
		return Outline(new Point(0, first), new Point(int.MaxValue, last), level, outlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText)
	{
		return Outline(startPoint, endPoint, GetOutlineLevel(startPoint), outlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText)
	{
		if (allowOutlining)
		{
			IOutlineRange outlineRange = new OutRange(outlineList, startPoint, endPoint, level, outlineText);
			outlineList.Add(outlineRange);
			return outlineRange;
		}
		return null;
	}

	public void UnOutline(Point point)
	{
		outlineList.RemoveRange(point);
	}

	public void UnOutline(int index)
	{
		outlineList.RemoveRange(index);
	}

	public void UnOutline()
	{
		outlineList.Clear();
	}

	public IOutlineRange GetOutlineRange(Point point)
	{
		if (allowOutlining)
		{
			return outlineList.FindRange(point) as IOutlineRange;
		}
		return null;
	}

	public IOutlineRange GetOutlineRange(int index)
	{
		if (allowOutlining)
		{
			return outlineList.FindRange(index) as IOutlineRange;
		}
		return null;
	}

	public int GetOutlineRanges(IList ranges, int index)
	{
		if (allowOutlining)
		{
			outlineList.GetRanges(ranges, index);
		}
		else
		{
			ranges.Clear();
		}
		return ranges.Count;
	}

	public int GetOutlineRanges(IList ranges, Point point)
	{
		if (allowOutlining)
		{
			outlineList.GetRanges(ranges, point);
		}
		else
		{
			ranges.Clear();
		}
		return ranges.Count;
	}

	public int GetOutlineRanges(IList ranges, Point startPoint, Point endPoint)
	{
		if (allowOutlining)
		{
			outlineList.GetRanges(ranges, startPoint, endPoint);
		}
		else
		{
			ranges.Clear();
		}
		return ranges.Count;
	}

	public int GetOutlineRanges(IList ranges)
	{
		if (allowOutlining)
		{
			outlineList.GetRanges(ranges);
		}
		else
		{
			ranges.Clear();
		}
		return ranges.Count;
	}

	public bool IsExpanded(int index)
	{
		return GetOutlineRange(index)?.Visible ?? false;
	}

	public bool IsCollapsed(int index)
	{
		IOutlineRange outlineRange = GetOutlineRange(index);
		if (outlineRange != null && outlineRange.StartPoint.Y == index)
		{
			return !outlineRange.Visible;
		}
		return false;
	}

	public bool IsVisible(Point point)
	{
		return outlineList.IsVisible(point);
	}

	public bool IsVisible(int index)
	{
		return outlineList.IsVisible(index);
	}

	public void Collapse(int index)
	{
		ArrayList arrayList = new ArrayList();
		GetOutlineRanges(arrayList, index);
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange item in arrayList)
			{
				if (item.StartPoint.Y == index && item.Visible)
				{
					item.Visible = false;
				}
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void Expand(int index)
	{
		ArrayList arrayList = new ArrayList();
		GetOutlineRanges(arrayList, index);
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange item in arrayList)
			{
				if (item.StartPoint.Y == index && !item.Visible)
				{
					item.Visible = true;
				}
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void EnsureExpanded(int index)
	{
		if (!allowOutlining)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		GetOutlineRanges(arrayList, index);
		bool flag = false;
		foreach (IOutlineRange item in arrayList)
		{
			if (!item.Visible)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange item2 in arrayList)
			{
				item2.Visible = true;
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void EnsureExpanded(Point point)
	{
		if (!allowOutlining)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		GetOutlineRanges(arrayList, point);
		bool flag = false;
		foreach (IOutlineRange item in arrayList)
		{
			if (!item.Visible && !item.StartPoint.Equals((object?)point))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange item2 in arrayList)
			{
				if (!item2.Visible && !item2.StartPoint.Equals((object?)point))
				{
					item2.Visible = true;
				}
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void ToggleOutlining()
	{
		ToggleOutlining(outlineList.GetRanges(), GetOutlineRange(owner.Position));
	}

	public void ToggleOutlining(IList ranges, IOutlineRange range)
	{
		if (ranges.Count != 0)
		{
			if (range == null)
			{
				range = (IOutlineRange)ranges[0];
			}
			if (range.Visible)
			{
				FullCollapse(ranges);
			}
			else
			{
				FullExpand(ranges);
			}
		}
	}

	public void CollapseToDefinitions()
	{
		if (!((TextSource)owner.Source).NeedOutlineText())
		{
			return;
		}
		ISyntaxParser syntaxParser = owner.Lexer as ISyntaxParser;
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange outline in outlineList)
			{
				ISyntaxNode nodeAt = syntaxParser.GetNodeAt(outline.StartPoint);
				outline.Visible = nodeAt != null && syntaxParser.IsDeclaration(nodeAt);
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void FullExpand(IList ranges)
	{
		if (ranges.Count <= 0)
		{
			return;
		}
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange range in ranges)
			{
				range.Visible = true;
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void FullCollapse(IList ranges)
	{
		outlineList.BeginUpdate();
		try
		{
			foreach (IOutlineRange range in ranges)
			{
				range.Visible = false;
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public void FullExpand()
	{
		FullExpand(outlineList.GetRanges());
	}

	public void FullCollapse()
	{
		FullCollapse(outlineList.GetRanges());
	}

	public void SetOutlineRanges(IList ranges)
	{
		SetOutlineRanges(ranges, preserveVisible: false);
	}

	public void SetOutlineRanges(IList ranges, bool preserveVisible)
	{
		outlineList.BeginUpdate();
		try
		{
			ArrayList arrayList = new ArrayList();
			foreach (IOutlineRange range2 in ranges)
			{
				bool flag = range2.Visible;
				if (preserveVisible)
				{
					if (flag)
					{
						flag = outlineList.FindCollapsedRange(range2.StartPoint) == null;
					}
					else
					{
						IRange range = outlineList.FindExactRange(range2.StartPoint);
						flag = range != null && ((IOutlineRange)range).Visible;
					}
				}
				arrayList.Add(new OutRange(outlineList, range2.StartPoint, range2.EndPoint, range2.Level, range2.DisplayText, flag));
			}
			outlineList.Clear();
			foreach (IOutlineRange item in arrayList)
			{
				outlineList.Add(item);
			}
		}
		finally
		{
			outlineList.EndUpdate();
		}
	}

	public string GetOutlineHint(IOutlineRange range)
	{
		string text = string.Empty;
		int num = Math.Min(lines.Count - 1, range.EndPoint.Y);
		int num2 = 0;
		int num3 = -1;
		ArrayList arrayList = new ArrayList();
		for (int i = range.StartPoint.Y; i <= num; i++)
		{
			string text2 = lines[i];
			if (i == range.EndPoint.Y)
			{
				text2 = text2.Substring(0, Math.Min(text2.Length, range.EndPoint.X));
			}
			if (i == range.StartPoint.Y)
			{
				if (range.StartPoint.X >= text2.TrimEnd(new char[0]).Length)
				{
					continue;
				}
				int num4 = range.StartPoint.X;
				if (num4 <= text2.Length - text2.TrimStart(new char[0]).Length)
				{
					num4 = 0;
				}
				text2 = text2.Substring(Math.Min(num4, text2.TrimEnd(new char[0]).Length));
			}
			text2 = lines.GetTabString(text2);
			if (text2.Trim() != string.Empty)
			{
				num3 = ((num3 != -1) ? Math.Min(num3, text2.Length - text2.TrimStart(new char[0]).Length) : (text2.Length - text2.TrimStart(new char[0]).Length));
			}
			arrayList.Add(text2);
			if (i == num)
			{
				break;
			}
			num2++;
			if (num2 >= EditConsts.MaxHintWindowCount)
			{
				break;
			}
		}
		foreach (string item in arrayList)
		{
			text = ((!(text == string.Empty)) ? (text + "\r\n" + ((num3 > 0 && num3 < item.Length) ? item.Substring(num3) : item)) : ((num3 > 0 && num3 < item.Length) ? item.Substring(num3) : item));
		}
		if (num2 >= EditConsts.MaxHintWindowCount)
		{
			text += EditConsts.DottedText;
		}
		return text;
	}

	public int BeginUpdate()
	{
		return outlineList.BeginUpdate();
	}

	public int EndUpdate()
	{
		return outlineList.EndUpdate();
	}

	public virtual void ResetAllowOutlining()
	{
		AllowOutlining = false;
	}

	public void ResetOutlineOptions()
	{
		OutlineOptions = EditConsts.DefaultOutlineOptions;
	}

	private void ClearLastLine()
	{
		lastWrapped = -1;
		lastWrapIndex = -1;
		lastDisplayIndex = -1;
	}

	private void EnsureWrapped(int index, bool wrapped)
	{
		if ((wrapped && index > lastWrapIndex) || (!wrapped && index > lastDisplayIndex))
		{
			if (index != int.MaxValue)
			{
				index += EditConsts.DefaultWrapDelta;
			}
			int displayLine = ((lastWrapped + 1 > 0) ? (PointToDisplayPoint(int.MaxValue, lastWrapped).Y + 1) : 0);
			WrapLines(lastWrapped + 1, index, displayLine);
		}
	}

	private void WrapLines(int first, int last, int displayLine)
	{
		wrapMargin = WrapMargin;
		bool flag = owner.Source.Lexer != null && !owner.Painter.IsMonoSpaced;
		if (flag)
		{
			owner.Source.ParseToString(last);
		}
		int index = Math.Max(first, 0);
		int num = Math.Min(last, lines.Count - 1);
		int num2 = index;
		if (allowOutlining)
		{
			outlineList.CheckVisible(ref index);
			num2 = outlineList.LineToDisplayLine(index);
		}
		int num3 = 0;
		int height = owner.ClientRect.Height;
		int page = -1;
		IEditPages pages = owner.Pages;
		if (pages.PageType != 0)
		{
			page = ((index > 0) ? pages.GetPageAt(0, displayLine).Index : 0);
			if (!wrapAtMargin)
			{
				wrapMargin = ((EditPage)pages[page]).DisplayWidth;
			}
		}
		else
		{
			int fontHeight = owner.Painter.FontHeight;
			num3 = owner.Scrolling.WindowOriginY + ((fontHeight != 0) ? (owner.Height / fontHeight) : 0) + EditConsts.DefaultWrapDelta;
		}
		for (int i = index; i <= num; i++)
		{
			if (!WrapLine(i, pages, ref page, num2, ref displayLine, ref wrapMargin, flag))
			{
				continue;
			}
			num2++;
			lastWrapped = i;
			lastDisplayIndex = num2;
			if (last != int.MaxValue)
			{
				continue;
			}
			if (pages.PageType != 0)
			{
				if (pages[page].BoundsRect.Top > height)
				{
					break;
				}
			}
			else if (num2 >= num3)
			{
				break;
			}
		}
		lastWrapIndex = wrapList.LineToDisplayLine(lastDisplayIndex);
	}

	private void CheckPage(IEditPages pages, ref int page, int displayLine, ref int margin)
	{
		if (pages[page].EndLine < displayLine)
		{
			page++;
			((EditPages)pages).UpdatePages(page, page, changed: false);
			if (!wrapAtMargin)
			{
				margin = ((EditPage)pages[page]).DisplayWidth;
			}
		}
	}

	private void WordWrapUpdated(bool update, int first, int last)
	{
		owner.Notification(this, new NotifyEventArgs(NotifyState.WordWrap, first, last, update));
	}

	private void UnWrapLines(int first, int last)
	{
		ClearLastLine();
		if (first == 0 && last == int.MaxValue)
		{
			wrapList.Clear();
			return;
		}
		if (allowOutlining)
		{
			first = outlineList.LineToDisplayLine(first);
			last = outlineList.LineToDisplayLine(last);
		}
		wrapList.ClearLines(first, last);
	}

	protected virtual bool WrapLine(int index, IEditPages pages, ref int page, int displayIndex, ref int displayLine, ref int margin, bool needData)
	{
		if (!allowOutlining || IsVisible(index))
		{
			string line = string.Empty;
			short[] data = null;
			GetOutlineString(index, ref line, ref data, needData, applyTabs: true);
			int num = 0;
			int length = line.Length;
			if (length == 0)
			{
				if (page >= 0)
				{
					CheckPage(pages, ref page, displayLine, ref margin);
				}
				displayLine++;
			}
			else
			{
				while (num < length)
				{
					if (page >= 0)
					{
						CheckPage(pages, ref page, displayLine, ref margin);
					}
					owner.MeasureLine(line, data, num, -1, margin, out var chars, exact: true);
					if (num + chars < length)
					{
						for (int num2 = chars; num2 > 0; num2--)
						{
							if (IsDelimiter(line, num + num2 - 1))
							{
								chars = num2;
								break;
							}
						}
					}
					if (chars <= 0)
					{
						chars = 1;
					}
					num += chars;
					if (num < length)
					{
						wrapList.AddItem(displayIndex, num);
					}
					displayLine++;
				}
			}
			return true;
		}
		return false;
	}

	public bool UpdateWordWrap()
	{
		return UpdateWordWrap(0, int.MaxValue);
	}

	public bool UpdateWordWrap(int first, int last)
	{
		bool flag;
		if (wordWrap)
		{
			int count = wrapList.Count;
			int displayLine = ((first > 0) ? (PointToDisplayPoint(int.MaxValue, first - 1).Y + 1) : 0);
			UnWrapLines(first, last);
			WrapLines(first, last, displayLine);
			flag = first != last || wrapList.Count != count;
		}
		else
		{
			flag = wrapList.Count != 0;
			UnWrapLines(0, int.MaxValue);
		}
		if (flag)
		{
			last = int.MaxValue;
		}
		WordWrapUpdated(flag, first, last);
		return flag;
	}

	public int GetWrapMargin()
	{
		return wrapMargin;
	}

	public virtual void ResetWordWrap()
	{
		WordWrap = false;
	}

	public virtual void ResetWrapAtMargin()
	{
		WrapAtMargin = false;
	}

	public bool IsDelimiter(string str, int pos)
	{
		return lines.IsDelimiter(str, pos);
	}

	public bool IsDelimiter(int index, int pos)
	{
		return lines.IsDelimiter(this[index], pos);
	}

	public bool IsDelimiter(char ch)
	{
		return lines.IsDelimiter(ch);
	}

	public bool GetWord(string str, int pos, out int left, out int right)
	{
		return lines.GetWord(str, pos, out left, out right);
	}

	public bool GetWord(int index, int pos, out int left, out int right)
	{
		return GetWord(this[index], pos, out left, out right);
	}

	public string GetTextAt(int line, int pos)
	{
		string text = this[line];
		if (GetWord(text, pos, out var left, out var right))
		{
			return text.Substring(left, right - left + 1);
		}
		return string.Empty;
	}

	public string GetTextAt(Point position)
	{
		return GetTextAt(position.Y, position.X);
	}

	public virtual void ResetDelimiters()
	{
		Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
	}

	public virtual void ResetTabStops()
	{
		TabStops = new int[1] { EditConsts.DefaultTabStop };
	}

	public virtual void ResetUseSpaces()
	{
		UseSpaces = false;
	}

	public bool SaveFile(string fileName)
	{
		return SaveFile(fileName, ExportFormat.Text, null);
	}

	public bool SaveFile(string fileName, Encoding encoding)
	{
		return SaveFile(fileName, ExportFormat.Text, encoding);
	}

	public bool SaveFile(string fileName, ExportFormat format)
	{
		return SaveFile(fileName, format, null);
	}

	public bool SaveFile(string fileName, ExportFormat format, Encoding encoding)
	{
		bool result = true;
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			try
			{
				StreamWriter streamWriter = ((encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream));
				try
				{
					streamWriter.NewLine = LineTerminator;
					SaveStream(streamWriter, format);
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				stream.Close();
			}
			lines.AfterSave();
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
			result = false;
		}
		return result;
	}

	public bool SaveStream(TextWriter writer)
	{
		return SaveStream(writer, ExportFormat.Text);
	}

	public bool SaveStream(Stream stream)
	{
		return SaveStream(stream, ExportFormat.Text, null);
	}

	public bool SaveStream(TextWriter writer, ExportFormat format)
	{
		FmtFiler fmtFiler = new FmtFiler(owner, lines, format);
		return fmtFiler.Write(writer);
	}

	public bool SaveStream(Stream stream, ExportFormat format)
	{
		return SaveStream(stream, format, null);
	}

	public bool SaveStream(Stream stream, Encoding encoding)
	{
		return SaveStream(stream, ExportFormat.Text, encoding);
	}

	public bool SaveStream(Stream stream, ExportFormat format, Encoding encoding)
	{
		TextWriter textWriter = ((encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream));
		try
		{
			return SaveStream(textWriter, format);
		}
		finally
		{
			textWriter.Close();
		}
	}

	public bool LoadFile(string fileName)
	{
		return LoadFile(fileName, ExportFormat.Text, null);
	}

	public bool LoadFile(string fileName, ExportFormat format)
	{
		return LoadFile(fileName, format, null);
	}

	public bool LoadFile(string fileName, Encoding encoding)
	{
		return LoadFile(fileName, ExportFormat.Text, encoding);
	}

	public bool LoadFile(string fileName, ExportFormat format, Encoding encoding)
	{
		bool result = true;
		try
		{
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
				try
				{
					LoadStream(streamReader, format);
				}
				finally
				{
					streamReader.Close();
				}
			}
			finally
			{
				stream.Close();
			}
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
			result = false;
		}
		return result;
	}

	public bool LoadStream(TextReader reader)
	{
		return LoadStream(reader, ExportFormat.Text);
	}

	public bool LoadStream(Stream stream)
	{
		return LoadStream(stream, ExportFormat.Text, null);
	}

	public bool LoadStream(TextReader reader, ExportFormat format)
	{
		FmtFiler fmtFiler = new FmtFiler(owner, lines, format);
		return fmtFiler.Read(reader);
	}

	public bool LoadStream(Stream stream, ExportFormat format)
	{
		return LoadStream(stream, format, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		return LoadStream(stream, ExportFormat.Text, encoding);
	}

	public bool LoadStream(Stream stream, ExportFormat format, Encoding encoding)
	{
		TextReader textReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
		try
		{
			return LoadStream(textReader, format);
		}
		finally
		{
			textReader.Close();
		}
	}

	private int GetLineWidth(int index)
	{
		if (index < 0)
		{
			return 0;
		}
		string line = string.Empty;
		short[] data = null;
		GetDisplayString(index, ref line, ref data, needData: true);
		return owner.MeasureLine(line, data, 0, -1);
	}

	private void GetOutlineString(int index, ref string line, ref short[] data, bool needData, bool applyTabs)
	{
		if (allowOutlining)
		{
			outlineList.CheckVisible(ref index);
			if (GetString(index, ref line, ref data, needData, applyTabs: false))
			{
				int length = line.Length;
				outlineList.GetCollapsedRanges(index, collapsedList);
				for (int num = collapsedList.Count - 1; num >= 0; num--)
				{
					IOutlineRange outlineRange = (IOutlineRange)collapsedList[num];
					if (!outlineRange.IsEmpty)
					{
						string displayText = outlineRange.DisplayText;
						int num2 = 0;
						int num3 = 0;
						int num4 = Math.Min(line.Length, length);
						num2 = Math.Min(outlineRange.StartPoint.X, num4);
						if (outlineRange.EndPoint.Y == index)
						{
							num3 = Math.Min(num4 - num2, outlineRange.EndPoint.X - outlineRange.StartPoint.X);
							line = line.Remove(num2, num3).Insert(num2, displayText);
							if (needData)
							{
								short[] array = new short[data.Length - num3 + displayText.Length];
								Array.Copy(data, 0, array, 0, num2);
								Array.Copy(data, num2 + num3, array, num2 + displayText.Length, data.Length - num3 - num2);
								data = array;
								StrItem.SetColorFlag(ref data, num2, displayText.Length, ColorFlags.OutlineSection, setFlag: true);
							}
						}
						else
						{
							string str = string.Empty;
							short[] colorData = null;
							if (GetString(outlineRange.EndPoint.Y, ref str, ref colorData, needData, applyTabs: false))
							{
								if (outlineRange.EndPoint.X < str.Length)
								{
									line = line.Substring(0, Math.Min(num2, line.Length)) + displayText + str.Substring(outlineRange.EndPoint.X);
								}
								else
								{
									line = line.Substring(0, Math.Min(num2, line.Length)) + displayText;
								}
								if (needData)
								{
									int num5 = Math.Max(colorData.Length - outlineRange.EndPoint.X, 0);
									short[] array2 = new short[num2 + displayText.Length + num5];
									Array.Copy(data, 0, array2, 0, num2);
									if (num5 > 0)
									{
										Array.Copy(colorData, outlineRange.EndPoint.X, array2, num2 + displayText.Length, num5);
									}
									data = array2;
									StrItem.SetColorFlag(ref data, num2, displayText.Length, ColorFlags.OutlineSection, setFlag: true);
								}
							}
						}
					}
				}
			}
			if (applyTabs)
			{
				((SyntaxStrings)lines).GetTabString(ref line, ref data, needData && data != null);
			}
		}
		else
		{
			GetString(index, ref line, ref data, needData, applyTabs);
		}
	}

	private void GetSubString(int p, int len, ref string line, ref short[] data, bool needData)
	{
		int length = line.Length;
		p = Math.Min(p, length);
		len = Math.Min(len, length - p);
		line = line.Substring(p, len);
		if (needData)
		{
			short[] array = new short[len];
			Array.Copy(data, p, array, 0, len);
			data = array;
		}
	}

	private void GetWrapBounds(int index, ref int p, ref int len)
	{
		if (wordWrap)
		{
			wrapList.GetWrapBounds(index, ref p, ref len);
		}
	}

	private void MaxLineChanged(UpdateReason reason, int x, int y, int deltaX, int deltaY)
	{
		if (maxLineFlag || !NeedMaxLine())
		{
			return;
		}
		int y2 = PointToDisplayPoint(x, y).Y;
		switch (reason)
		{
		case UpdateReason.Insert:
		{
			int lineWidth = GetLineWidth(y2);
			if (maxLineWidth < lineWidth)
			{
				maxLineWidth = lineWidth;
				maxLineIndex = y2;
			}
			break;
		}
		case UpdateReason.InsertBlock:
		{
			int y3 = PointToDisplayPoint(x + deltaX, y + deltaY).Y;
			for (int i = y2; i <= y3; i++)
			{
				int lineWidth2 = GetLineWidth(i);
				if (maxLineWidth < lineWidth2)
				{
					maxLineWidth = lineWidth2;
					maxLineIndex = y2;
				}
			}
			break;
		}
		case UpdateReason.Delete:
			if (y2 == maxLineIndex)
			{
				maxLineFlag = true;
			}
			break;
		case UpdateReason.DeleteBlock:
		{
			int y4 = PointToDisplayPoint(x - deltaX, y - deltaY).Y;
			if (maxLineIndex >= y2 && maxLineIndex <= y4)
			{
				maxLineFlag = true;
			}
			break;
		}
		case UpdateReason.UnBreak:
		{
			int lineWidth3 = GetLineWidth(y2);
			if (maxLineWidth < lineWidth3)
			{
				maxLineWidth = lineWidth3;
				maxLineIndex = y2;
			}
			else if (y2 < maxLineIndex)
			{
				maxLineIndex--;
				if (maxLineIndex < 0)
				{
					maxLineFlag = true;
				}
			}
			break;
		}
		case UpdateReason.Break:
			if (y2 == maxLineIndex)
			{
				maxLineFlag = true;
			}
			else if (y2 < maxLineIndex)
			{
				maxLineIndex++;
			}
			break;
		default:
			maxLineFlag = true;
			break;
		}
	}

	private void RecalcMaxLine()
	{
		int num = 0;
		maxLineWidth = 0;
		ScanToEnd(parseToEnd: false);
		for (int i = 0; i < GetCount(); i++)
		{
			num = GetLineWidth(i);
			if (maxLineWidth < num)
			{
				maxLineWidth = num;
				maxLineIndex = i;
			}
		}
		maxLineFlag = false;
	}

	protected void ApplyWhiteSpace(string str, ref short[] colorData)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == ' ')
			{
				StrItem.SetColorFlag(ref colorData, i, 1, ColorFlags.WhiteSpace, setFlag: true);
			}
		}
	}

	protected internal void CollapseChanged(IOutlineRange range)
	{
		int first = 0;
		int last = int.MaxValue;
		if (wordWrap)
		{
			if (range != null)
			{
				first = range.StartPoint.Y;
				if (range.StartPoint.Y == range.EndPoint.Y)
				{
					last = range.EndPoint.Y;
				}
				UpdateWordWrap(range.StartPoint.Y, int.MaxValue);
			}
			else
			{
				UpdateWordWrap(0, int.MaxValue);
			}
		}
		MaxLineChanged();
		owner.Notification(this, new NotifyEventArgs(NotifyState.Outline, first, last, update: true));
	}

	protected void LinesChanged()
	{
		UpdateWordWrap();
		MaxLineChanged();
	}

	protected internal bool NeedMaxLine()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		if (!owner.WordWrap)
		{
			if ((int)owner.Scrolling.ScrollBars != 3 && (int)owner.Scrolling.ScrollBars != 1)
			{
				if ((int)owner.Scrolling.ScrollBars == 19 || (int)owner.Scrolling.ScrollBars == 17)
				{
					return owner.Scrolling.DefaultHorzScrollSize <= 0;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	protected internal bool GetString(int index, ref string str, ref short[] colorData, bool needData, bool applyTabs)
	{
		if (index >= 0 && index < lines.Count)
		{
			IStrItem item = lines.GetItem(index);
			str = item.String;
			if (needData)
			{
				colorData = item.ColorData;
				ApplyWhiteSpace(str, ref colorData);
			}
			if (applyTabs)
			{
				((SyntaxStrings)lines).GetTabString(ref str, ref colorData, needData);
			}
			return true;
		}
		return false;
	}

	protected internal int GetDisplayString(int index, ref string line, ref short[] data, bool needData)
	{
		int result = index;
		if ((wordWrap && wrapList.Count > 0) || allowOutlining)
		{
			Point point = new Point(0, index);
			if (wordWrap)
			{
				bool lineEnd = false;
				wrapList.GetRealPoint(ref point, checkEnd: false, ref lineEnd);
			}
			if (allowOutlining)
			{
				outlineList.GetRealPoint(ref point, rangeStart: false);
			}
			GetOutlineString(point.Y, ref line, ref data, needData, applyTabs: true);
			result = point.Y;
			if (wordWrap)
			{
				int p = 0;
				int len = int.MaxValue;
				GetWrapBounds(index, ref p, ref len);
				if (p != 0 || len != int.MaxValue)
				{
					GetSubString(p, len, ref line, ref data, needData && data != null);
				}
			}
		}
		else
		{
			GetString(index, ref line, ref data, needData, applyTabs: true);
		}
		return result;
	}

	protected internal Point DisplayPointToPoint(int x, int y, bool wrapEnd, bool rangeStart, bool tabEnd)
	{
		bool lineEnd = false;
		return DisplayPointToPoint(x, y, wrapEnd, rangeStart, tabEnd, ref lineEnd);
	}

	protected internal Point DisplayPointToPoint(int x, int y, bool wrapEnd, bool rangeStart, bool tabEnd, ref bool lineEnd)
	{
		Point point = new Point(x, y);
		int num = point.Y - Math.Max(GetCount() - 1, 0);
		if (num > 0)
		{
			if ((owner.NavigateOptions & NavigateOptions.BeyondEof) != 0)
			{
				return new Point(x, Math.Max(owner.Lines.Count - 1, 0) + num);
			}
			point.Y -= num;
		}
		lineEnd = false;
		if (wordWrap)
		{
			wrapList.GetRealPoint(ref point, wrapEnd, ref lineEnd);
		}
		if (point.X != int.MaxValue)
		{
			string line = string.Empty;
			short[] data = null;
			GetOutlineString(allowOutlining ? outlineList.DisplayLineToLine(point.Y) : point.Y, ref line, ref data, needData: false, applyTabs: false);
			point.X = ((SyntaxStrings)lines).PosToTabPos(line, point.X, tabEnd);
		}
		if (allowOutlining)
		{
			outlineList.GetRealPoint(ref point, rangeStart);
		}
		return point;
	}

	protected internal Point PointToDisplayPoint(int x, int y, bool lineEnd)
	{
		Point point = new Point(x, y);
		if (allowOutlining)
		{
			outlineList.GetDisplayPoint(ref point);
		}
		if (point.X != int.MaxValue)
		{
			string line = string.Empty;
			short[] data = null;
			GetOutlineString(y, ref line, ref data, needData: false, applyTabs: false);
			point.X = lines.TabPosToPos(line, point.X);
		}
		if (wordWrap)
		{
			wrapList.GetDisplayPoint(ref point, lineEnd);
		}
		return point;
	}

	protected internal Point PointToDisplayPoint(Point point, bool lineEnd)
	{
		return PointToDisplayPoint(point.X, point.Y, lineEnd);
	}

	protected internal bool AlreadyScanned()
	{
		if (WordWrap)
		{
			return lastWrapped >= lines.Count - outlineList.CollapsedCount - 1;
		}
		return true;
	}

	protected internal void ScanToEnd(bool parseToEnd)
	{
		if (wordWrap)
		{
			EnsureWrapped(lines.Count - 1, wrapped: false);
		}
		if (parseToEnd)
		{
			owner.Source.ParseToString(owner.Lines.Count - 1);
		}
	}

	protected internal void BlockDeleting(Rectangle rect)
	{
		if (allowOutlining)
		{
			outlineList.BlockDeleting(rect);
		}
	}

	protected internal void PositionChanged(UpdateReason reason, int x, int y, int deltaX, int deltaY)
	{
		if (allowOutlining)
		{
			outlineList.PositionChanged(x, y, deltaX, deltaY);
		}
		MaxLineChanged(reason, x, y, deltaX, deltaY);
	}

	protected internal void MaxLineChanged()
	{
		maxLineFlag = true;
	}

	public Point DisplayPointToPoint(int x, int y)
	{
		bool lineEnd = false;
		return DisplayPointToPoint(x, y, wrapEnd: false, rangeStart: false, tabEnd: false, ref lineEnd);
	}

	public Point PointToDisplayPoint(int x, int y)
	{
		return PointToDisplayPoint(x, y, atLineEnd);
	}

	public Point PointToDisplayPoint(Point point)
	{
		return PointToDisplayPoint(point.X, point.Y, atLineEnd);
	}

	public Point DisplayPointToPoint(Point point)
	{
		return DisplayPointToPoint(point.X, point.Y, wrapEnd: false, rangeStart: false, tabEnd: false);
	}

	public short[] GetColorData(int index)
	{
		string line = string.Empty;
		short[] data = null;
		GetDisplayString(index, ref line, ref data, needData: true);
		return data;
	}

	public int GetLexStyle(Point position)
	{
		short[] colorData = GetColorData(position.Y);
		if (colorData == null || position.X < 0 || position.X >= colorData.Length)
		{
			return -1;
		}
		return colorData[position.X] - 1;
	}

	public bool Find(string str, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
	{
		return SyntaxStrings.Find(this, lines.DelimTable, str, options, expression, ref position, out len, out match);
	}
}
