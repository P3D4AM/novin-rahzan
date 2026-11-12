using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class Selection : ISelection
{
	private ISyntaxEdit owner;

	private SelectionType selectionType;

	private Rectangle selectionRect;

	private SelectionOptions options = EditConsts.DefaultSelectionOptions;

	private AllowedSelectionMode allowedSelectionMode = EditConsts.DefaultSelectionMode;

	private Color foreColor = EditConsts.DefaultHighlightForeColor;

	private Color backColor = EditConsts.DefaultHighlightBackColor;

	private Color inActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;

	private Color inActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;

	private Color borderColor = EditConsts.DefaultSelectionBorderColor;

	private int updateCount;

	private bool atTopLeftEnd;

	private bool atBottomRightEnd;

	private Rectangle oldSelectionRect = Rectangle.Empty;

	private SelectionType oldSelectionType;

	private SelectionState selectionState;

	private Point selStart = new Point(0, 0);

	private Point selEnd = new Point(0, 0);

	private bool selForward;

	private Timer selTimer;

	private KeyEvent moveSelection;

	private StringEvent tabifyLineEvent;

	private StringEvent unTabifyLineEvent;

	private StringEvent indentLineEvent;

	private StringEvent unIndentLineEvent;

	private StringEvent lowerCaseLineEvent;

	private StringEvent upperCaseLineEvent;

	private StringEvent capitalizeLineEvent;

	private StringEvent deleteWhiteSpaceEvent;

	private StringEvent commentLineEvent;

	private StringEvent uncommentLineEvent;

	[Browsable(false)]
	public bool IsEmpty => selectionType == SelectionType.None;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SelectionState SelectionState
	{
		get
		{
			return selectionState;
		}
		set
		{
			selectionState = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			return owner.Source.TextPointToAbsolutePosition(GetSelectionPoint());
		}
		set
		{
			selectionType = SelectionType.None;
			owner.Source.MoveTo(owner.Source.AbsolutePositionToTextPoint(value));
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionLength
	{
		get
		{
			return SelectedText.Length;
		}
		set
		{
			SetSelection(SelectionType.Stream, GetSelectionPoint(), owner.Source.AbsolutePositionToTextPoint(SelectionStart + value));
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedColorData
	{
		get
		{
			if (IsEmpty)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			for (int i = 0; i < SelectedCount(); i++)
			{
				string text = SelectedData(i);
				if (text != null)
				{
					if (!flag)
					{
						stringBuilder.Append(new string('\0', owner.LineTerminator.Length));
					}
					stringBuilder.Append(text);
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			if (IsEmpty)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			for (int i = 0; i < SelectedCount(); i++)
			{
				string text = SelectedString(i);
				if (text != null)
				{
					if (!flag)
					{
						stringBuilder.Append(owner.LineTerminator);
					}
					stringBuilder.Append(text);
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			SetSelectedText(value, SelectionType.Stream);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle SelectionRect
	{
		get
		{
			return selectionRect;
		}
		set
		{
			SetSelection(selectionType, value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SelectionType SelectionType
	{
		get
		{
			return selectionType;
		}
		set
		{
			SetSelection(value, selectionRect);
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets options determining behaviour or the \"Selection\".")]
	public SelectionOptions Options
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
				if ((SelectionOptions.DisableSelection & value) != 0)
				{
					Clear();
				}
				if (owner != null)
				{
					((SyntaxEdit)owner).CheckResizeRedraw();
				}
				InvalidateSelection();
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets type of selection allowed to Edit control content.")]
	public AllowedSelectionMode AllowedSelectionMode
	{
		get
		{
			return allowedSelectionMode;
		}
		set
		{
			if (allowedSelectionMode != value)
			{
				allowedSelectionMode = value;
				SetSelection(selectionType, selectionRect);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int UpdateCount => updateCount;

	[Description("Gets or sets a foreground color of the \"SelectedText\" when owner control has input focus.")]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				InvalidateSelection();
			}
		}
	}

	[Description("Gets or sets a background color of the \"SelectedText\" when owner control has input focus.")]
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
				InvalidateSelection();
			}
		}
	}

	[Description("Gets or sets a foreground color of the \"SelectedText\" when owner control does not have input focus.")]
	public Color InActiveForeColor
	{
		get
		{
			return inActiveForeColor;
		}
		set
		{
			if (inActiveForeColor != value)
			{
				inActiveForeColor = value;
				InvalidateSelection();
			}
		}
	}

	[Description("Gets or sets a background color of the \"SelectedText\" when owner control does not have input focus.")]
	public Color InActiveBackColor
	{
		get
		{
			return inActiveBackColor;
		}
		set
		{
			if (inActiveBackColor != value)
			{
				inActiveBackColor = value;
				InvalidateSelection();
			}
		}
	}

	[Description("Gets or sets a color of the selection border.")]
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
				InvalidateSelection();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlSelectionInfo(this);
		}
		set
		{
			((XmlSelectionInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event EventHandler SelectionChanged;

	public Selection()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		moveSelection = MoveSelection;
		tabifyLineEvent = TabifyLine;
		unTabifyLineEvent = UnTabifyLine;
		indentLineEvent = IndentLine;
		unIndentLineEvent = UnIndentLine;
		lowerCaseLineEvent = LowerCaseLine;
		upperCaseLineEvent = UpperCaseLine;
		capitalizeLineEvent = CapitalizeLine;
		deleteWhiteSpaceEvent = DeleteWhiteSpace;
		commentLineEvent = CommentLine;
		uncommentLineEvent = UncommentLine;
		selTimer = new Timer();
		selTimer.Enabled = false;
		selTimer.Interval = EditConsts.DefaultSelDelay;
		selTimer.Tick += OnSelect;
	}

	public Selection(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	~Selection()
	{
		((Component)(object)selTimer).Dispose();
	}

	private void SetRectLeft(ref Rectangle rect, int left)
	{
		int width = rect.Right - left;
		rect.X = left;
		rect.Width = width;
	}

	private void SetRectTop(ref Rectangle rect, int top)
	{
		int height = rect.Bottom - top;
		rect.Y = top;
		rect.Height = height;
	}

	private void SwapMaxInt(ref int left, ref int right)
	{
		if (right < left)
		{
			int num = left;
			left = right;
			right = num;
		}
	}

	private void SwapRect(ref Rectangle rect, bool swapMax)
	{
		Point location = rect.Location;
		Point location2 = rect.Location + rect.Size;
		rect.Location = location2;
		rect.Width = location.X - rect.X;
		rect.Height = location.Y - rect.Y;
	}

	private void CheckSelectionMode(ref SelectionType selType)
	{
		if (allowedSelectionMode == AllowedSelectionMode.None)
		{
			selType = SelectionType.None;
			return;
		}
		switch (selType)
		{
		case SelectionType.Stream:
			if ((allowedSelectionMode & AllowedSelectionMode.Stream) == 0)
			{
				selType = SelectionType.Block;
			}
			break;
		case SelectionType.Block:
			if ((allowedSelectionMode & AllowedSelectionMode.Block) == 0)
			{
				selType = SelectionType.Stream;
			}
			break;
		}
	}

	private void SelectBlock(SelectionType selType, SelectionType newSelType, Point position, KeyEvent action)
	{
		Rectangle rectangle = SelectionRect;
		int x = position.X;
		int y = position.Y;
		int num = x;
		int num2 = y;
		if (newSelType == SelectionType.Block && !owner.Painter.IsMonoSpaced)
		{
			newSelType = SelectionType.Stream;
		}
		CheckSelectionMode(ref newSelType);
		BeginUpdate();
		try
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			if (selType == SelectionType.None || !IsValidPos(position))
			{
				rectangle.X = position.X;
				rectangle.Y = position.Y;
				rectangle.Width = 0;
				rectangle.Height = 0;
			}
			Point point = displayStrings.PointToDisplayPoint(x, y);
			if (action != null)
			{
				if (newSelType == SelectionType.Block && !owner.WordWrap)
				{
					TextSource textSource = owner.Source as TextSource;
					NavigateOptions navigateOptions = textSource.NavigateOptions;
					try
					{
						textSource.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
						action();
					}
					finally
					{
						textSource.SetNavigateOptions(navigateOptions);
					}
				}
				else
				{
					action();
				}
			}
			num = owner.Position.X;
			num2 = owner.Position.Y;
			if (num2 >= owner.Lines.Count)
			{
				num = 0;
				num2 = owner.Lines.Count;
			}
			if (newSelType == SelectionType.Block)
			{
				Point point2 = displayStrings.PointToDisplayPoint(num, num2);
				if (point2.X < point.X)
				{
					int x2 = point.X;
					point.X = point2.X;
					point2.X = x2;
				}
				if (point2.Y < point.Y)
				{
					int y2 = point.Y;
					point.Y = point2.Y;
					point2.Y = y2;
				}
				point = displayStrings.DisplayPointToPoint(point.X, point.Y, wrapEnd: true, rangeStart: false, tabEnd: false);
				point2 = displayStrings.DisplayPointToPoint(point2.X, point2.Y, wrapEnd: true, rangeStart: false, tabEnd: false);
				rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			}
			else
			{
				rectangle = new Rectangle(x, y, num - x, num2 - y);
				if (rectangle.Height < 0 || (rectangle.Width < 0 && rectangle.Height == 0))
				{
					SwapRect(ref rectangle, swapMax: false);
				}
			}
			if (owner.Source.UpdateCount == 0 && !IsEmpty)
			{
				Region selectionRegion = GetSelectionRegion(SelectionType, SelectionRect);
				if (selectionRegion != null)
				{
					((Control)(SyntaxEdit)owner).Invalidate(selectionRegion);
					selectionRegion.Dispose();
				}
			}
			if (newSelType != 0)
			{
				if (rectangle.Location == owner.Position)
				{
					atTopLeftEnd = displayStrings.AtLineEnd;
				}
				if (owner.Position == new Point(rectangle.Right, rectangle.Bottom))
				{
					atBottomRightEnd = displayStrings.AtLineEnd;
				}
			}
			SetSelection(newSelType, rectangle);
		}
		finally
		{
			EndUpdate();
		}
	}

	private void SelectBlock(SelectionType newSelType, KeyEvent action)
	{
		UpdateSelStart(checkIfEmpty: true);
		SelectBlock(selectionType, newSelType, selStart, action);
	}

	private Point GetSelectionPoint()
	{
		if (!IsEmpty)
		{
			return selectionRect.Location;
		}
		return owner.Position;
	}

	private Point UpdateWordSelectionEnd(Point pt)
	{
		Point result = pt;
		int left = 0;
		int right = 0;
		if ((SelectionOptions.WordSelect & Options) != 0 && owner.Lines.GetWord(pt.Y, pt.X, out left, out right) && (pt.Y != selStart.Y || selStart.X < left || selStart.X > right))
		{
			bool flag = pt.Y > selStart.Y || (pt.Y == selStart.Y && pt.X >= selStart.X);
			result.X = (flag ? (right + 1) : left);
		}
		return result;
	}

	private void UpdateWordSelection(ref Point pt, bool direction)
	{
		if (selectionState != SelectionState.SelectWord)
		{
			return;
		}
		if (selectionType == SelectionType.Block)
		{
			TextSource textSource = owner.Source as TextSource;
			NavigateOptions navigateOptions = textSource.NavigateOptions;
			try
			{
				textSource.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
				textSource.ValidatePosition(ref pt);
			}
			finally
			{
				textSource.SetNavigateOptions(navigateOptions);
			}
		}
		else
		{
			owner.Source.ValidatePosition(ref pt);
		}
		if (owner.Lines.GetWord(pt.Y, pt.X, out var left, out var right))
		{
			pt.X = (direction ? (right + 1) : left);
		}
	}

	private void MoveSelection()
	{
		Point pt = UpdateWordSelectionEnd(selEnd);
		UpdateWordSelection(ref pt, selForward);
		if (selectionType == SelectionType.Block)
		{
			TextSource textSource = owner.Source as TextSource;
			NavigateOptions navigateOptions = textSource.NavigateOptions;
			try
			{
				textSource.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
				owner.Position = pt;
				return;
			}
			finally
			{
				textSource.SetNavigateOptions(navigateOptions);
			}
		}
		owner.Position = pt;
	}

	private string TabifyLine(string str)
	{
		string text = string.Empty;
		int num = 0;
		while (num < str.Length)
		{
			if (str[num] == ' ')
			{
				int i;
				for (i = num; i + 1 < str.Length && str[i + 1] == ' '; i++)
				{
				}
				text += ((SyntaxStrings)owner.Lines).GetIndentString(i - num + 1, owner.Lines.TabPosToPos(str, num), useSpaces: false);
				num = i + 1;
			}
			else
			{
				text += str[num];
				num++;
			}
		}
		return text;
	}

	private string UnTabifyLine(string str)
	{
		return owner.Lines.GetTabString(str);
	}

	private string IndentLine(string str)
	{
		ISyntaxStrings lines = owner.Lines;
		return (lines.UseSpaces ? new string(' ', lines.GetTabStop(0)) : "\t") + str;
	}

	private string UnIndentLine(string str)
	{
		if (str != string.Empty)
		{
			ISyntaxStrings lines = owner.Lines;
			int val = ((!lines.UseSpaces) ? 1 : lines.GetTabStop(0));
			string text = str.Substring(0, Math.Min(val, str.Length));
			val = text.Length;
			val -= text.TrimStart(null).Length;
			if (val > 0)
			{
				str = str.Remove(0, val);
			}
		}
		return str;
	}

	private string LowerCaseLine(string str)
	{
		return str.ToLower(CultureInfo.InvariantCulture);
	}

	private string UpperCaseLine(string str)
	{
		return str.ToUpper(CultureInfo.InvariantCulture);
	}

	private string CapitalizeLine(string str)
	{
		char[] array = str.ToLower(CultureInfo.InvariantCulture).ToCharArray();
		ISyntaxStrings lines = owner.Lines;
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = !lines.IsDelimiter(array[i]);
			if (i != 0)
			{
				flag = flag && lines.IsDelimiter(array[i - 1]);
			}
			if (flag)
			{
				array[i] = char.ToUpper(array[i]);
			}
		}
		return new string(array);
	}

	private string DeleteWhiteSpace(string str)
	{
		string text = string.Empty;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (c != ' ' && c != '\t')
			{
				text += c;
			}
		}
		return text;
	}

	private string CommentLine(string str)
	{
		if (owner.Lexer is ISyntaxParser)
		{
			string singleLineComment = ((ISyntaxParser)owner.Lexer).GetSingleLineComment();
			if (singleLineComment != string.Empty)
			{
				return singleLineComment + str;
			}
		}
		return str;
	}

	private string UncommentLine(string str)
	{
		if (owner.Lexer is ISyntaxParser)
		{
			string singleLineComment = ((ISyntaxParser)owner.Lexer).GetSingleLineComment();
			if (singleLineComment != string.Empty)
			{
				int num = str.IndexOf(singleLineComment);
				if (num >= 0 && num <= str.Length - str.TrimStart(new char[0]).Length)
				{
					return str.Remove(num, singleLineComment.Length);
				}
			}
		}
		return str;
	}

	private Region GetSelectionRegion(SelectionType selectionType, Rectangle rect)
	{
		return ((SyntaxEdit)owner).GetRectRegion(selectionType, rect, atTopLeftEnd, atBottomRightEnd);
	}

	private string SafeSubString(string s, int left, int right)
	{
		if (left < s.Length)
		{
			if (right < s.Length)
			{
				return s.Substring(left, right - left);
			}
			return s.Substring(left);
		}
		return string.Empty;
	}

	private string SafeSubString(short[] data, int left, int right)
	{
		if (left < data.Length)
		{
			char[] array = new char[Math.Min(right, data.Length) - left];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (char)data[i + left];
			}
			return new string(array);
		}
		return string.Empty;
	}

	private int GetSelectedBounds(int index, ArrayList list)
	{
		switch (selectionType)
		{
		case SelectionType.Block:
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			for (int i = displayStrings.PointToDisplayPoint(0, index).Y; i <= displayStrings.PointToDisplayPoint(owner.Lines.GetLength(index), index).Y; i++)
			{
				if (!GetSelectionForLine(i, out var x, out var x2, checkBounds: false))
				{
					continue;
				}
				Point point = displayStrings.DisplayPointToPoint(x, i);
				Point point2 = displayStrings.DisplayPointToPoint(x2, i, wrapEnd: false, rangeStart: false, tabEnd: true);
				if (index < point.Y || index > point2.Y)
				{
					continue;
				}
				if (index == point.Y)
				{
					if (index == point2.Y)
					{
						x = point.X;
						x2 = point2.X;
					}
					else
					{
						x = point.X;
						x2 = int.MaxValue;
					}
				}
				else if (index == point2.Y)
				{
					x = 0;
					x2 = point2.X;
				}
				else
				{
					x = 0;
					x2 = int.MaxValue;
				}
				list.Add(new Range(x, index, x2, index));
			}
			break;
		}
		case SelectionType.Stream:
		{
			if (index > selectionRect.Bottom || index < selectionRect.Top)
			{
				break;
			}
			int x;
			int x2;
			if (index == selectionRect.Top)
			{
				if (index == selectionRect.Bottom)
				{
					x = selectionRect.Left;
					x2 = SelectionRect.Right;
				}
				else
				{
					x = selectionRect.Left;
					x2 = int.MaxValue;
				}
			}
			else if (index == selectionRect.Bottom)
			{
				x = 0;
				x2 = selectionRect.Right;
			}
			else
			{
				x = 0;
				x2 = int.MaxValue;
			}
			list.Add(new Range(x, index, x2, index));
			break;
		}
		}
		return list.Count;
	}

	private int DoInsertString(SelectionType selType, string s, bool insertLine)
	{
		if (insertLine)
		{
			if (selType == SelectionType.Block)
			{
				owner.MoveLineDown();
			}
			else
			{
				owner.Source.BreakLine();
				owner.MoveTo(0, owner.Position.Y + 1);
			}
		}
		owner.Source.Insert(s);
		return s.Length;
	}

	private EventHandlers GetHandlers()
	{
		return ((KeyList)owner.KeyList).Handlers;
	}

	private int GetTabIndent(int indent)
	{
		return owner.Lines.TabPosToPos(new string('\t', indent), indent);
	}

	protected internal void DoSelectionChanged()
	{
		((SyntaxEdit)owner).DoSelectionChanged();
		if (this.SelectionChanged != null)
		{
			this.SelectionChanged(this, EventArgs.Empty);
		}
	}

	protected void WriteToClipboard()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			DataObject val = new DataObject();
			string selectedText = SelectedText;
			if (selectedText != string.Empty)
			{
				val.SetText(selectedText);
				if ((options & SelectionOptions.RtfClipboard) != 0)
				{
					SyntaxStrings syntaxStrings = new SyntaxStrings(null);
					syntaxStrings.Owner = owner;
					syntaxStrings.SetTextAndData(selectedText, owner.Source.NeedParse() ? SelectedColorData : null);
					StringWriter stringWriter = new StringWriter();
					try
					{
						syntaxStrings.SaveStream(stringWriter, ExportFormat.Rtf);
						selectedText = stringWriter.ToString();
						if (selectedText != string.Empty)
						{
							val.SetText(selectedText, (TextDataFormat)2);
						}
					}
					finally
					{
						stringWriter.Close();
					}
				}
			}
			else
			{
				val.SetData((object)selectedText);
			}
			if (SelectionType != 0)
			{
				val.SetData(DataFormats.Serializable, (object)SelectionType);
			}
			Clipboard.SetDataObject((object)val, true);
		}
		catch
		{
		}
	}

	protected bool CanSelectBlock()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		return (Control.ModifierKeys & 0x40000) > 0;
	}

	protected void UpdateSelRect(ref SelectionType oldSelectionType, SelectionType newSelectionType, ref Rectangle oldRect, Rectangle newRect, ref bool changed)
	{
		changed = false;
		if (oldSelectionType == newSelectionType && !(oldRect != newRect))
		{
			return;
		}
		bool flag = owner.Source.UpdateCount == 0;
		Region val = (flag ? GetSelectionRegion(oldSelectionType, oldRect) : null);
		if ((SelectionOptions.DisableSelection & options) != 0)
		{
			newSelectionType = SelectionType.None;
		}
		else if (IsSelectionRectEmpty(newSelectionType, newRect))
		{
			newSelectionType = SelectionType.None;
		}
		if (newSelectionType == SelectionType.None)
		{
			newRect = Rectangle.Empty;
			atTopLeftEnd = false;
			atBottomRightEnd = false;
		}
		changed = true;
		oldRect = newRect;
		oldSelectionType = newSelectionType;
		if (flag)
		{
			Region selectionRegion = GetSelectionRegion(newSelectionType, newRect);
			if (val == null)
			{
				val = selectionRegion;
			}
			else if (selectionRegion != null)
			{
				val.Union(selectionRegion);
				selectionRegion.Dispose();
			}
			if (val != null)
			{
				((Control)(SyntaxEdit)owner).Invalidate(val, false);
				val.Dispose();
			}
		}
	}

	protected void SmartFormat(bool needFormat, bool needIndent, int line)
	{
		if (!((TextSource)owner.Source).NeedFormatText())
		{
			return;
		}
		if (IsEmpty)
		{
			SetSelection(SelectionType.Stream, new Rectangle(0, line, owner.Lines[line].Length, 0));
		}
		if (IsEmpty)
		{
			return;
		}
		TextSource textSource = (TextSource)owner.Source;
		BeginUpdate();
		textSource.BeginUpdate(UpdateReason.InsertBlock);
		textSource.State |= NotifyState.SmartFormat;
		StoreSelection(textSource, out var selType, out var index, out var index2, out var index3);
		try
		{
			if (needFormat)
			{
				textSource.FormatText();
			}
			ISyntaxParser syntaxParser = (ISyntaxParser)owner.Source.Lexer;
			SyntaxStrings syntaxStrings = (SyntaxStrings)owner.Lines;
			for (int i = selectionRect.Top; i <= selectionRect.Bottom; i++)
			{
				IStrItem item = syntaxStrings.GetItem(i);
				if (item == null)
				{
					continue;
				}
				string @string = item.String;
				string text = @string.TrimStart(new char[0]);
				if (!(text != string.Empty || needIndent))
				{
					continue;
				}
				string text2 = syntaxParser.SmartFormatLine(i, @string, item.ColorData);
				int num = @string.Length - text.Length;
				int smartIndent = syntaxParser.GetSmartIndent(i, autoIndent: false);
				if (smartIndent < 0 && !(text2 != @string))
				{
					continue;
				}
				string text3 = ((smartIndent >= 0) ? syntaxStrings.GetIndentString(GetTabIndent(smartIndent), 0) : string.Empty);
				if (text2 != @string)
				{
					bool flag = text2.Length == @string.Length;
					if (flag)
					{
						textSource.DisablePositionUpdate();
					}
					try
					{
						textSource.MoveTo(0, i);
						textSource.DeleteRight(int.MaxValue);
						textSource.Insert(text3 + text2.TrimStart(new char[0]));
					}
					finally
					{
						if (flag)
						{
							textSource.EnablePositionUpdate();
						}
					}
				}
				else if (text3 != @string.Substring(0, num))
				{
					if (num >= 0)
					{
						textSource.MoveTo(0, i);
						textSource.DeleteRight(num);
					}
					textSource.Insert(text3);
				}
			}
		}
		finally
		{
			RestoreSelection(textSource, selType, index, index2, index3);
			textSource.EndUpdate();
			EndUpdate();
		}
	}

	private bool SelectionContainsReadonlyLines()
	{
		if (!IsEmpty)
		{
			for (int i = SelectionRect.Top; i < SelectionRect.Bottom; i++)
			{
				if (owner.Source.IsLineReadonly(i))
				{
					return true;
				}
			}
		}
		return false;
	}

	protected internal bool NeedDragScroll(Point pt)
	{
		Rectangle clientRect = owner.ClientRect;
		Rectangle rectangle = clientRect;
		rectangle.Inflate(-owner.Painter.FontWidth, -owner.Painter.FontHeight);
		if (owner.WordWrap)
		{
			if (!owner.WrapAtMargin)
			{
				if (pt.Y >= clientRect.Top && pt.Y <= clientRect.Bottom)
				{
					if (pt.Y >= rectangle.Top)
					{
						return pt.Y > rectangle.Bottom;
					}
					return true;
				}
				return false;
			}
			if (pt.X >= owner.DisplayToScreen(owner.EditMargin.Position, 0).X)
			{
				if (pt.Y >= clientRect.Top && pt.Y <= clientRect.Bottom)
				{
					if (pt.Y >= rectangle.Top)
					{
						return pt.Y > rectangle.Bottom;
					}
					return true;
				}
				return false;
			}
		}
		if (clientRect.Contains(pt))
		{
			return !rectangle.Contains(pt);
		}
		return false;
	}

	protected internal bool DoDragScroll(Point pt)
	{
		Rectangle clientRect = owner.ClientRect;
		int windowOriginX = owner.Scrolling.WindowOriginX;
		int windowOriginY = owner.Scrolling.WindowOriginY;
		bool flag = owner.Pages.PageType == PageType.PageLayout;
		int fontHeight = owner.Painter.FontHeight;
		int fontWidth = owner.Painter.FontWidth;
		if (pt.Y - fontHeight < clientRect.Top)
		{
			owner.Scrolling.WindowOriginY -= ((!flag) ? 1 : fontHeight);
		}
		else if (pt.Y + fontHeight > clientRect.Bottom)
		{
			owner.Scrolling.WindowOriginY += ((!flag) ? 1 : fontHeight);
		}
		else if (pt.X - fontWidth < clientRect.Left)
		{
			owner.Scrolling.WindowOriginX -= ((!flag) ? 1 : fontWidth);
		}
		else if (pt.X + fontWidth > clientRect.Right)
		{
			owner.Scrolling.WindowOriginX += ((!flag) ? 1 : fontWidth);
		}
		if (windowOriginX == owner.Scrolling.WindowOriginX)
		{
			return windowOriginY != owner.Scrolling.WindowOriginY;
		}
		return true;
	}

	protected internal Rectangle GetSelectionRectInPixels()
	{
		if (!IsEmpty)
		{
			Point point = ((SyntaxEdit)owner).TextToScreen(selectionRect.Location, atTopLeftEnd);
			Point point2 = ((SyntaxEdit)owner).TextToScreen(selectionRect.Location + selectionRect.Size, atBottomRightEnd);
			return new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y + owner.Painter.FontHeight);
		}
		return new Rectangle(0, 0, 0, 0);
	}

	protected internal void OnSelect(object source, EventArgs e)
	{
		if (selectionState == SelectionState.None || owner.Source.UpdateCount > 0)
		{
			return;
		}
		Point pt = owner.PointToClient(Cursor.Position);
		bool lineEnd = false;
		Point point = Point.Empty;
		TextSource textSource = owner.Source as TextSource;
		NavigateOptions navigateOptions = textSource.NavigateOptions;
		try
		{
			textSource.SetNavigateOptions(navigateOptions & ~NavigateOptions.BeyondEof);
			point = ((SyntaxEdit)owner).ScreenToText(pt.X, pt.Y, ref lineEnd);
		}
		finally
		{
			textSource.SetNavigateOptions(navigateOptions);
		}
		if (point.Equals((object?)selEnd) && selectionState != SelectionState.SelectLine)
		{
			return;
		}
		DisplayStrings displayStrings = owner.DisplayLines as DisplayStrings;
		switch (selectionState)
		{
		case SelectionState.Drag:
			if (owner.ClientRect.Contains(pt) && !NeedDragScroll(pt))
			{
				BeginUpdate();
				displayStrings.LineEndUpdateCount++;
				try
				{
					displayStrings.AtLineEnd = lineEnd;
					owner.Position = point;
					break;
				}
				finally
				{
					displayStrings.LineEndUpdateCount--;
					EndUpdate();
				}
			}
			break;
		case SelectionState.Select:
			BeginUpdate();
			displayStrings.LineEndUpdateCount++;
			try
			{
				selEnd = point;
				displayStrings.AtLineEnd = lineEnd;
				SelectBlock((e == null) ? selectionType : SelectionType.None, (!CanSelectBlock()) ? SelectionType.Stream : SelectionType.Block, selStart, moveSelection);
				break;
			}
			finally
			{
				displayStrings.LineEndUpdateCount--;
				EndUpdate();
			}
		case SelectionState.SelectLine:
			BeginUpdate();
			displayStrings.LineEndUpdateCount++;
			try
			{
				displayStrings.AtLineEnd = lineEnd;
				Point point2 = selStart;
				if (point.Y < selStart.Y)
				{
					point2 = new Point(0, selStart.Y + 1);
				}
				else if (point.Y >= selStart.Y)
				{
					point2.X = 0;
				}
				if (point.X != 0)
				{
					point2.X = 0;
					selectionState = SelectionState.Select;
				}
				else if (point.Y >= selStart.Y)
				{
					point.Y++;
				}
				if (!selEnd.Equals((object?)point) || !selStart.Equals((object?)point2))
				{
					selEnd = point;
					SelectBlock((e == null) ? selectionType : SelectionType.None, (!CanSelectBlock()) ? SelectionType.Stream : SelectionType.Block, point2, moveSelection);
				}
				break;
			}
			finally
			{
				displayStrings.LineEndUpdateCount--;
				EndUpdate();
			}
		case SelectionState.SelectWord:
			BeginUpdate();
			displayStrings.LineEndUpdateCount++;
			try
			{
				displayStrings.AtLineEnd = lineEnd;
				selForward = selEnd.Y > selStart.Y || (selEnd.Y == selStart.Y && selEnd.X >= selStart.X);
				Point pt2 = selStart;
				UpdateWordSelection(ref pt2, !selForward);
				selEnd = point;
				SelectBlock((e == null) ? selectionType : SelectionType.None, (!CanSelectBlock()) ? SelectionType.Stream : SelectionType.Block, pt2, moveSelection);
				break;
			}
			finally
			{
				displayStrings.LineEndUpdateCount--;
				EndUpdate();
			}
		}
	}

	protected internal bool GetSelectionForLine(int index, out int left, out int right, bool checkBounds)
	{
		bool flag = false;
		left = 0;
		right = 0;
		if (!IsEmpty)
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			Point point = displayStrings.PointToDisplayPoint(selectionRect.Left, selectionRect.Top, atTopLeftEnd);
			Point point2 = displayStrings.PointToDisplayPoint(selectionRect.Right, selectionRect.Bottom, atBottomRightEnd);
			flag = index >= point.Y && index <= point2.Y;
			if (flag)
			{
				if (selectionType == SelectionType.Block || point.Y == point2.Y)
				{
					left = point.X;
					right = point2.X;
					if (selectionType == SelectionType.Block && left == right && point.Y != point2.Y)
					{
						if (left != 0 || index != point2.Y)
						{
							right = int.MaxValue;
						}
						left = 0;
					}
				}
				else if (index == point.Y)
				{
					left = point.X;
					right = int.MaxValue;
				}
				else if (index == point2.Y)
				{
					left = 0;
					right = point2.X;
				}
				else
				{
					left = 0;
					right = int.MaxValue;
				}
				SwapMaxInt(ref left, ref right);
			}
		}
		if (flag)
		{
			if (checkBounds)
			{
				return right > left;
			}
			return true;
		}
		return false;
	}

	protected internal bool IsValidPos(Point position)
	{
		switch (selectionType)
		{
		case SelectionType.Stream:
			if (selectionRect.Top != position.Y || selectionRect.Left != position.X)
			{
				if (selectionRect.Bottom == position.Y)
				{
					return selectionRect.Right == position.X;
				}
				return false;
			}
			return true;
		case SelectionType.Block:
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			Point point = displayStrings.PointToDisplayPoint(selectionRect.Left, selectionRect.Top, atTopLeftEnd);
			Point point2 = displayStrings.PointToDisplayPoint(selectionRect.Right, selectionRect.Bottom, atBottomRightEnd);
			Point point3 = displayStrings.PointToDisplayPoint(position);
			if (point.Y == point3.Y || point2.Y == point3.Y)
			{
				if (point.X != point3.X)
				{
					return point2.X == point3.X;
				}
				return true;
			}
			return false;
		}
		default:
			return true;
		}
	}

	protected internal bool IsSelectionRectEmpty(Rectangle rect)
	{
		return IsSelectionRectEmpty(selectionType, rect);
	}

	protected internal bool IsSelectionRectEmpty(SelectionType selectionType, Rectangle rect)
	{
		switch (selectionType)
		{
		case SelectionType.Stream:
			if (rect.Height >= 0)
			{
				if (rect.Height == 0)
				{
					return rect.Width <= 0;
				}
				return false;
			}
			return true;
		case SelectionType.Block:
		{
			DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
			if (rect.Height >= 0)
			{
				return displayStrings.PointToDisplayPoint(rect.Left, rect.Top, atTopLeftEnd).X > displayStrings.PointToDisplayPoint(rect.Right, rect.Bottom, atBottomRightEnd).X;
			}
			return true;
		}
		default:
			return true;
		}
	}

	protected internal bool IsSelectionRectEmpty()
	{
		return IsSelectionRectEmpty(selectionRect);
	}

	protected internal void InvalidateSelection()
	{
		if (owner.Source.UpdateCount == 0)
		{
			Region selectionRegion = GetSelectionRegion(selectionType, selectionRect);
			if (selectionRegion != null)
			{
				((Control)(SyntaxEdit)owner).Invalidate(selectionRegion, false);
				selectionRegion.Dispose();
			}
		}
	}

	protected internal void UpdateSelection()
	{
		if (!IsEmpty && ((options & SelectionOptions.HideSelection) != 0 || ForeColor != InActiveForeColor || BackColor != InActiveBackColor))
		{
			InvalidateSelection();
		}
	}

	protected internal void UpdateSelStart(bool checkIfEmpty)
	{
		UpdateSelStart(owner.Position, checkIfEmpty);
	}

	protected internal void UpdateSelStart(Point position, bool checkIfEmpty)
	{
		if (!checkIfEmpty || selectionType == SelectionType.None || !IsValidPos(position))
		{
			selStart = position;
		}
	}

	protected internal void DeleteRight(bool deleteWord)
	{
		if (ShouldDeleteBlock())
		{
			Delete();
		}
		else if (owner.Position.X >= owner.Lines.GetLength(owner.Position.Y))
		{
			owner.Source.UnBreakLine();
		}
		else if (deleteWord)
		{
			bool flag = true;
			owner.Source.BeginUpdate(UpdateReason.Delete);
			BeginUpdate();
			try
			{
				SelectWordRight();
				flag = Delete();
			}
			finally
			{
				EndUpdate();
				owner.Source.EndUpdate();
			}
			if (!flag && !IsEmpty)
			{
				Clear();
			}
		}
		else
		{
			owner.Source.DeleteRight(1);
		}
	}

	protected internal void DeleteLeft(bool deleteWord)
	{
		if (ShouldDeleteBlock())
		{
			Delete();
		}
		else if (owner.Position.X == 0)
		{
			if (owner.Position.Y > 0)
			{
				owner.Source.BeginUpdate(UpdateReason.UnBreak);
				try
				{
					owner.MoveTo(owner.Lines.GetLength(owner.Position.Y - 1), owner.Position.Y - 1);
					owner.Source.UnBreakLine();
				}
				finally
				{
					owner.Source.EndUpdate();
				}
			}
		}
		else if (deleteWord)
		{
			bool flag = true;
			BeginUpdate();
			owner.Source.BeginUpdate(UpdateReason.Delete);
			try
			{
				SelectWordLeft();
				flag = Delete();
			}
			finally
			{
				owner.Source.EndUpdate();
				EndUpdate();
			}
			if (!flag && !IsEmpty)
			{
				Clear();
			}
		}
		else
		{
			owner.Source.DeleteLeft(1);
		}
	}

	protected internal void StoreSelection(ITextSource source, out SelectionType selType, out int index1, out int index2, out int index3)
	{
		selType = SelectionType;
		if (!IsEmpty)
		{
			index1 = source.StorePosition(selectionRect.Location);
			index2 = source.StorePosition(new Point(selectionRect.Right, selectionRect.Bottom));
		}
		else
		{
			index1 = -1;
			index2 = -1;
		}
		index3 = source.StorePosition(source.Position);
	}

	protected internal void RestoreSelection(ITextSource source, SelectionType selType, int index1, int index2, int index3)
	{
		source.Position = source.RestorePosition(index3);
		if (index1 >= 0 && index2 >= 0)
		{
			Point selectionEnd = source.RestorePosition(index2);
			Point selectionStart = source.RestorePosition(index1);
			SetSelection(selType, selectionStart, selectionEnd);
		}
		else
		{
			Clear();
		}
	}

	protected internal void PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		if (updateCount == 0 && !IsEmpty)
		{
			Point pt = selectionRect.Location;
			Point pt2 = new Point(selectionRect.Right, selectionRect.Bottom);
			bool flag = SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: false);
			bool flag2 = SortList.UpdatePos(x, y, deltaX, deltaY, ref pt2, endPos: true);
			if (flag || flag2)
			{
				SetSelection(selectionType, new Rectangle(pt.X, pt.Y, pt2.X - pt.X, pt2.Y - pt.Y));
			}
		}
	}

	protected internal void StartSelection()
	{
		selTimer.Start();
	}

	protected internal void EndSelection()
	{
		if (selectionState != 0)
		{
			selTimer.Stop();
			selectionState = SelectionState.None;
			selForward = false;
		}
	}

	protected internal bool ShouldDelete()
	{
		if ((options & SelectionOptions.OverwriteBlocks) != 0 && (options & SelectionOptions.PersistentBlocks) == 0)
		{
			return !IsEmpty;
		}
		return false;
	}

	protected internal bool ShouldDeleteBlock()
	{
		if ((options & SelectionOptions.PersistentBlocks) == 0)
		{
			return !IsEmpty;
		}
		return false;
	}

	protected internal IDataObject GetDataObject()
	{
		try
		{
			return Clipboard.GetDataObject();
		}
		catch
		{
			return null;
		}
	}

	protected internal bool IsPosInSelection(int x, int y, bool checkEnd)
	{
		if (!IsEmpty)
		{
			Point point = owner.DisplayLines.PointToDisplayPoint(x, y);
			if (GetSelectionForLine(point.Y, out var left, out var right))
			{
				if (point.X >= left)
				{
					if (point.X >= right)
					{
						if (checkEnd)
						{
							return point.X == right;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}
		return false;
	}

	public void Assign(ISelection source)
	{
		Options = source.Options;
		BackColor = source.BackColor;
		ForeColor = source.ForeColor;
		InActiveBackColor = source.InActiveBackColor;
		InActiveForeColor = source.InActiveForeColor;
		BorderColor = source.BorderColor;
		SetSelection(source.SelectionType, source.SelectionRect);
	}

	public void ChangeBlock(StringEvent action)
	{
		ChangeBlock(action, changeIfEmpty: false, extendFirstLine: false);
	}

	public void ChangeBlock(StringEvent action, bool changeIfEmpty, bool extendFirstLine)
	{
		if (!changeIfEmpty && IsEmpty)
		{
			return;
		}
		int num = 0;
		ITextSource source = owner.Source;
		if (IsEmpty)
		{
			string text = source.Lines[source.Position.Y];
			if (!extendFirstLine && source.Position.X >= text.Length)
			{
				return;
			}
			source.BeginUpdate(UpdateReason.Other);
			try
			{
				if (extendFirstLine)
				{
					num = source.Position.X - text.Length;
					source.MoveToChar(0);
					source.DeleteRight(int.MaxValue);
					text = action(text);
					num += text.Length;
					source.Insert(text);
					source.MoveToChar(num);
				}
				else
				{
					char c = text[source.Position.X];
					source.DeleteRight(1);
					source.Insert(action(c.ToString()));
				}
				return;
			}
			finally
			{
				source.EndUpdate();
			}
		}
		num = 0;
		if (extendFirstLine)
		{
			Rectangle rectangle = SelectionRect;
			num = rectangle.Left;
			SelectionRect = new Rectangle(0, rectangle.Top, rectangle.Right, rectangle.Height);
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = SelectedCount();
		for (int i = 0; i < num2; i++)
		{
			string text = SelectedString(i);
			string text2 = ((i != num2 - 1 || selectionType != SelectionType.Stream || selectionRect.Right != 0) ? action(text) : text);
			if (extendFirstLine && i == 0)
			{
				num += text2.Length - text.Length;
			}
			stringBuilder.Append(text2);
			if (i < num2 - 1)
			{
				stringBuilder.Append(source.LineTerminator);
			}
		}
		SetSelectedText(stringBuilder.ToString(), (SelectionType == SelectionType.None) ? SelectionType.Stream : SelectionType);
		if (extendFirstLine)
		{
			Rectangle rectangle = SelectionRect;
			SetSelection(SelectionType, new Point(Math.Max(rectangle.Left + num, 0), rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
		}
	}

	public bool GetSelectionForLine(int index, out int left, out int right)
	{
		return GetSelectionForLine(index, out left, out right, checkBounds: true);
	}

	public bool IsPosInSelection(Point position)
	{
		return IsPosInSelection(position.X, position.Y, checkEnd: false);
	}

	public bool IsPosInSelection(int x, int y)
	{
		return IsPosInSelection(x, y, checkEnd: false);
	}

	public void Clear()
	{
		SelectionType = SelectionType.None;
	}

	public bool Move(Point position, bool deleteOrigin)
	{
		bool flag = !IsPosInSelection(position) && !owner.Source.Readonly && !IsEmpty;
		if (flag)
		{
			bool flag2 = true;
			SelectionType selType = selectionType;
			ITextSource source = owner.Source;
			BeginUpdate();
			source.BeginUpdate(UpdateReason.Other);
			try
			{
				int index = source.StorePosition(position);
				string selectedText = SelectedText;
				if (deleteOrigin)
				{
					flag2 = Delete();
				}
				source.Position = source.RestorePosition(index);
				if (flag2)
				{
					Clear();
					SetSelectedText(selectedText, selType);
				}
			}
			finally
			{
				source.EndUpdate();
				EndUpdate();
			}
			if (!flag2 && !IsEmpty)
			{
				Clear();
			}
		}
		return flag;
	}

	public void SetSelection(SelectionType selType, Rectangle selRect)
	{
		CheckSelectionMode(ref selectionType);
		bool changed = false;
		UpdateSelRect(ref selectionType, selType, ref selectionRect, selRect, ref changed);
		if (changed && updateCount == 0)
		{
			DoSelectionChanged();
		}
		if (IsEmpty)
		{
			selectionType = SelectionType.None;
		}
	}

	public void SetSelection(SelectionType selectionType, Point selectionStart, Point selectionEnd)
	{
		SetSelection(selectionType, new Rectangle(selectionStart.X, selectionStart.Y, selectionEnd.X - selectionStart.X, selectionEnd.Y - selectionStart.Y));
	}

	public void SelectCharLeft()
	{
		SelectCharLeft(SelectionType.Stream);
	}

	public void SelectCharRight()
	{
		SelectCharRight(SelectionType.Stream);
	}

	public void SelectWordLeft()
	{
		SelectWordLeft(SelectionType.Stream);
	}

	public void SelectWordRight()
	{
		SelectWordRight(SelectionType.Stream);
	}

	public void SelectLineUp()
	{
		SelectLineUp(SelectionType.Stream);
	}

	public void SelectLineDown()
	{
		SelectLineDown(SelectionType.Stream);
	}

	public void SelectPageUp()
	{
		SelectPageUp(SelectionType.Stream);
	}

	public void SelectPageDown()
	{
		SelectPageDown(SelectionType.Stream);
	}

	public void SelectScreenTop()
	{
		SelectScreenTop(SelectionType.Stream);
	}

	public void SelectScreenBottom()
	{
		SelectScreenBottom(SelectionType.Stream);
	}

	public void SelectLineBegin()
	{
		SelectLineBegin(SelectionType.Stream);
	}

	public void SelectLineEnd()
	{
		SelectLineEnd(SelectionType.Stream);
	}

	public void SelectFileBegin()
	{
		SelectFileBegin(SelectionType.Stream);
	}

	public void SelectFileEnd()
	{
		SelectFileEnd(SelectionType.Stream);
	}

	public void SelectLine()
	{
		TextSource textSource = owner.Source as TextSource;
		NavigateOptions navigateOptions = textSource.NavigateOptions;
		try
		{
			textSource.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEof);
			Point position = owner.DisplayLines.PointToDisplayPoint(owner.Position);
			position.X = 0;
			Point selectionStart = owner.DisplayLines.DisplayPointToPoint(position);
			position.Y++;
			Point selectionEnd = owner.DisplayLines.DisplayPointToPoint(position);
			SetSelection(SelectionType.Stream, selectionStart, selectionEnd);
		}
		finally
		{
			textSource.SetNavigateOptions(navigateOptions);
		}
	}

	public void SelectWord()
	{
		ISyntaxStrings lines = owner.Lines;
		Point position = owner.Position;
		string text = owner.Lines[position.Y];
		if (position.X > 0 && position.X < text.Length && owner.Lines.IsDelimiter(text, position.X) && !owner.Lines.IsDelimiter(text, position.X - 1))
		{
			position.X--;
		}
		if (lines.GetWord(position.Y, position.X, out var left, out var right))
		{
			owner.MoveTo(right + 1, position.Y);
			selStart = new Point(left, position.Y);
			SetSelection(SelectionType.Stream, new Rectangle(left, position.Y, right - left + 1, 0));
		}
	}

	public bool SelectCurrentWord()
	{
		ISyntaxStrings lines = owner.Lines;
		Point position = owner.Position;
		while (position.Y < lines.Count)
		{
			string text = lines[position.Y];
			int length = text.Length;
			while (position.X < length && lines.IsDelimiter(text, position.X))
			{
				position.X++;
			}
			if (position.X < length)
			{
				break;
			}
			position.Y++;
			position.X = 0;
		}
		if (position.Y < lines.Count)
		{
			owner.Position = position;
			SelectWord();
		}
		return position.Y < lines.Count;
	}

	public bool SelectNextWord()
	{
		ISyntaxStrings lines = owner.Lines;
		Point position = owner.Position;
		string text = lines[position.Y];
		while (position.X < text.Length && !lines.IsDelimiter(text, position.X))
		{
			position.X++;
		}
		owner.Position = position;
		return SelectCurrentWord();
	}

	public void SelectAll()
	{
		if (owner.Lines.Count > 0)
		{
			selStart = owner.Position;
			SetSelection(SelectionType.Stream, new Rectangle(0, 0, owner.Lines.GetLength(owner.Lines.Count - 1), owner.Lines.Count - 1));
		}
	}

	public void SelectCharLeft(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveCharLeftEvent);
	}

	public void SelectCharRight(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveCharRightEvent);
	}

	public void SelectWordLeft(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveWordLeftEvent);
	}

	public void SelectWordRight(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveWordRightEvent);
	}

	public void SelectLineUp(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveLineUpEvent);
	}

	public void SelectLineDown(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveLineDownEvent);
	}

	public void SelectPageUp(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().movePageUpEvent);
	}

	public void SelectPageDown(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().movePageDownEvent);
	}

	public void SelectScreenTop(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveScreenTopEvent);
	}

	public void SelectScreenBottom(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveScreenBottomEvent);
	}

	public void SelectLineBegin(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveLineBeginEvent);
	}

	public void SelectLineEnd(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveLineEndEvent);
	}

	public void SelectFileBegin(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveFileBeginEvent);
	}

	public void SelectFileEnd(SelectionType selectionType)
	{
		SelectBlock(selectionType, GetHandlers().moveFileEndEvent);
	}

	public void SelectToOpenBrace()
	{
		SelectBlock((SelectionType == SelectionType.None) ? SelectionType.Stream : SelectionType, GetHandlers().moveToOpenBraceEvent);
	}

	public void SelectToCloseBrace()
	{
		SelectBlock((SelectionType == SelectionType.None) ? SelectionType.Stream : SelectionType, GetHandlers().moveToCloseBraceEvent);
	}

	public void SelectToBrace()
	{
		Point position = owner.Position;
		bool flag = false;
		bool flag2 = false;
		if (Array.IndexOf(owner.Braces.OpenBraces, owner.Lines.GetCharAt(position)) >= 0)
		{
			flag = false;
			flag2 = true;
		}
		else if (Array.IndexOf(owner.Braces.ClosingBraces, owner.Lines.GetCharAt(position)) >= 0)
		{
			flag = false;
			flag2 = true;
		}
		else if (position.X > 0 && Array.IndexOf(owner.Braces.OpenBraces, owner.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0)
		{
			flag = true;
			flag2 = true;
		}
		else if (position.X > 0 && Array.IndexOf(owner.Braces.ClosingBraces, owner.Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0)
		{
			flag = false;
			flag2 = false;
		}
		SelectBlock(selectionType, (SelectionType == SelectionType.None) ? SelectionType.Stream : SelectionType, position, GetHandlers().moveToBraceEvent);
		if ((flag || flag2) && !owner.Position.Equals((object?)position) && !IsEmpty)
		{
			Rectangle selRect = SelectionRect;
			if (flag && selRect.X > 0)
			{
				selRect.X--;
				selRect.Width++;
			}
			if (flag2)
			{
				selRect.Width++;
			}
			SetSelection(selectionType, selRect);
		}
	}

	public void CommentSelection()
	{
		if (owner.Lexer is ISyntaxParser)
		{
			if (IsEmpty)
			{
				SelectLine();
			}
			SelectionType = SelectionType.Stream;
			ChangeBlock(commentLineEvent, changeIfEmpty: false, extendFirstLine: true);
		}
	}

	public void UncommentSelection()
	{
		if (owner.Lexer is ISyntaxParser)
		{
			if (IsEmpty)
			{
				SelectLine();
			}
			SelectionType = SelectionType.Stream;
			ChangeBlock(uncommentLineEvent, changeIfEmpty: false, extendFirstLine: true);
		}
	}

	public void SmartIndent()
	{
		if ((Options & SelectionOptions.SmartFormat) == 0 || !((TextSource)owner.Source).NeedFormatText())
		{
			return;
		}
		TextSource textSource = (TextSource)owner.Source;
		ISyntaxParser syntaxParser = (ISyntaxParser)textSource.Lexer;
		Point blockPt;
		ISyntaxNode block = syntaxParser.GetBlock(textSource.Position, out blockPt);
		if (block != null)
		{
			syntaxParser.ReparseBlock(blockPt);
			textSource.BeginUpdate(UpdateReason.Insert);
			try
			{
				int index = textSource.StorePosition(textSource.Position);
				Clear();
				SmartFormat(needFormat: false, needIndent: true, owner.Position.Y);
				Clear();
				textSource.State |= NotifyState.Edit;
				textSource.Position = textSource.RestorePosition(index);
			}
			finally
			{
				textSource.EndUpdate();
			}
		}
	}

	public void SmartFormatBlock()
	{
		if ((Options & SelectionOptions.SmartFormat) == 0 || !((TextSource)owner.Source).NeedFormatText())
		{
			return;
		}
		TextSource textSource = (TextSource)owner.Source;
		ISyntaxParser syntaxParser = (ISyntaxParser)textSource.Lexer;
		Point position = textSource.Position;
		textSource.FormatText();
		Point blockPt;
		ISyntaxNode block = syntaxParser.GetBlock(position, out blockPt);
		if (block != null)
		{
			IRange range = block.Range;
			SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
			int index = textSource.StorePosition(textSource.Position);
			SmartFormat(needFormat: false, needIndent: false, owner.Position.Y);
			if (owner.Braces.BracesOptions != 0 && !owner.Braces.UseRoundRect)
			{
				textSource.TempHighlightBraces(new Rectangle[1]
				{
					new Rectangle(selectionRect.Left, selectionRect.Top, blockPt.X - selectionRect.Left, blockPt.Y - selectionRect.Top)
				});
			}
			textSource.Position = textSource.RestorePosition(index);
			textSource.DoHighlightBraces();
			Clear();
		}
	}

	public void SmartFormat()
	{
		SmartFormat(needFormat: true, needIndent: false, owner.Position.Y);
	}

	public void SmartFormat(int line)
	{
		SmartFormat(needFormat: false, needIndent: false, line);
	}

	public void SmartFormatDocument()
	{
		TextSource textSource = owner.Source as TextSource;
		if (!textSource.NeedFormatText())
		{
			return;
		}
		textSource.BeginUpdate(UpdateReason.Other);
		try
		{
			StoreSelection(textSource, out var selType, out var index, out var index2, out var index3);
			SelectAll();
			SmartFormat();
			RestoreSelection(textSource, selType, index, index2, index3);
		}
		finally
		{
			textSource.EndUpdate();
		}
	}

	public void Tabify()
	{
		ChangeBlock(tabifyLineEvent);
	}

	public void UnTabify()
	{
		ChangeBlock(unTabifyLineEvent);
	}

	public void Indent()
	{
		ChangeBlock(indentLineEvent, changeIfEmpty: true, SelectionType != SelectionType.Block);
	}

	public void UnIndent()
	{
		ChangeBlock(unIndentLineEvent, changeIfEmpty: true, SelectionType != SelectionType.Block);
	}

	public void LowerCase()
	{
		ChangeBlock(lowerCaseLineEvent, changeIfEmpty: true, extendFirstLine: false);
	}

	public void UpperCase()
	{
		ChangeBlock(upperCaseLineEvent, changeIfEmpty: true, extendFirstLine: false);
	}

	public void Capitalize()
	{
		ChangeBlock(capitalizeLineEvent, changeIfEmpty: true, extendFirstLine: false);
	}

	public void DeleteWhiteSpace()
	{
		if (IsEmpty)
		{
			string text = owner.Lines[owner.Position.Y];
			if (owner.Position.X < text.Length && owner.Position.X <= text.Length - text.TrimStart(new char[0]).Length)
			{
				SelectLineBegin();
				ChangeBlock(deleteWhiteSpaceEvent);
				Clear();
			}
		}
		else
		{
			ChangeBlock(deleteWhiteSpaceEvent);
		}
	}

	public void ToggleOutlining()
	{
		DisplayStrings displayStrings = (DisplayStrings)owner.DisplayLines;
		ArrayList arrayList = new ArrayList();
		if (!IsEmpty)
		{
			displayStrings.GetOutlineRanges(arrayList, selectionRect.Location, selectionRect.Location + selectionRect.Size);
		}
		if (arrayList.Count == 0)
		{
			IOutlineRange outlineRange = displayStrings.GetOutlineRange(owner.Position);
			if (outlineRange != null)
			{
				arrayList.Add(outlineRange);
			}
		}
		if (arrayList.Count != 0)
		{
			displayStrings.ToggleOutlining(arrayList, null);
		}
	}

	public void CollapseToDefinitions()
	{
		owner.DisplayLines.CollapseToDefinitions();
	}

	public void SwapAnchor()
	{
		if (IsEmpty)
		{
			return;
		}
		BeginUpdate();
		try
		{
			if (owner.Position == selectionRect.Location + selectionRect.Size)
			{
				owner.Position = selectionRect.Location;
			}
			else
			{
				owner.Position = selectionRect.Location + selectionRect.Size;
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool CanCopy()
	{
		if (IsEmpty)
		{
			return (options & SelectionOptions.CopyLineWhenEmpty) != 0;
		}
		return true;
	}

	public bool CanCut()
	{
		if (!owner.Source.Readonly && (!IsEmpty || (options & SelectionOptions.CopyLineWhenEmpty) != 0))
		{
			return !SelectionContainsReadonlyLines();
		}
		return false;
	}

	public bool CanPaste()
	{
		if (owner.Readonly || owner.Source.IsLineReadonly(owner.Position.Y))
		{
			return false;
		}
		IDataObject dataObject = GetDataObject();
		if (dataObject != null && (dataObject.GetDataPresent(DataFormats.UnicodeText) || dataObject.GetDataPresent(DataFormats.Text)))
		{
			return !SelectionContainsReadonlyLines();
		}
		return false;
	}

	public void Copy()
	{
		if (CanCopy())
		{
			bool isEmpty = IsEmpty;
			if (isEmpty)
			{
				SelectLine();
			}
			WriteToClipboard();
			if ((options & SelectionOptions.DeselectOnCopy) != 0 || isEmpty)
			{
				Clear();
			}
		}
	}

	public void Cut()
	{
		if (CanCut())
		{
			if (IsEmpty)
			{
				SelectLine();
			}
			WriteToClipboard();
			Delete();
		}
	}

	public void Paste()
	{
		IDataObject dataObject = GetDataObject();
		object obj = ((dataObject != null) ? dataObject.GetData(DataFormats.UnicodeText) : null);
		if (obj == null && dataObject != null)
		{
			obj = dataObject.GetData(DataFormats.Text);
		}
		if (obj != null && dataObject != null)
		{
			object data = dataObject.GetData(DataFormats.Serializable);
			SetSelectedText((string)obj, (!(data is SelectionType) || (SelectionType)data == SelectionType.None) ? SelectionType.Stream : ((SelectionType)data));
			if ((options & SelectionOptions.ConvertToSpacesOnPaste) != 0 && owner.Lines.UseSpaces)
			{
				UnTabify();
			}
			if ((options & SelectionOptions.SmartFormat) != 0 && !IsEmpty && selectionRect.Height > 0)
			{
				SmartFormat();
			}
			if ((options & SelectionOptions.PersistentBlocks) == 0)
			{
				Clear();
			}
			UpdateSelStart(checkIfEmpty: false);
		}
	}

	public bool Delete()
	{
		if (owner.Readonly)
		{
			return false;
		}
		bool flag = true;
		if (!IsEmpty)
		{
			ITextSource source = owner.Source;
			source.BeginUpdate(UpdateReason.Delete);
			try
			{
				BeginUpdate();
				try
				{
					if (selectionType == SelectionType.Stream)
					{
						flag = source.DeleteBlock(SelectionRect);
					}
					else
					{
						ArrayList arrayList = new ArrayList();
						for (int i = selectionRect.Top; i <= selectionRect.Bottom; i++)
						{
							GetSelectedBounds(i, arrayList);
						}
						for (int num = arrayList.Count - 1; num >= 0; num--)
						{
							IRange range = (IRange)arrayList[num];
							source.MoveTo(range.StartPoint.X, range.StartPoint.Y);
							source.DeleteRight((range.EndPoint.X == int.MaxValue) ? range.EndPoint.X : (range.EndPoint.X - range.StartPoint.X));
							if (range.EndPoint.X == int.MaxValue)
							{
								source.UnBreakLine();
							}
						}
					}
				}
				finally
				{
					source.EndUpdate();
				}
				if (flag)
				{
					Clear();
				}
			}
			finally
			{
				EndUpdate();
			}
		}
		return flag;
	}

	public int BeginUpdate()
	{
		if (updateCount == 0)
		{
			oldSelectionRect = selectionRect;
			oldSelectionType = selectionType;
		}
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0 && (!oldSelectionRect.Equals((object?)selectionRect) || oldSelectionType != selectionType))
		{
			DoSelectionChanged();
		}
		return updateCount;
	}

	public bool CanDrag(Point position)
	{
		if (!owner.Source.Readonly && (SelectionOptions.DisableDragging & options) == 0)
		{
			return !IsPosInSelection(position);
		}
		return false;
	}

	public void DragTo(Point position, bool deleteOrigin)
	{
		Move(position, deleteOrigin);
	}

	public Point SelectionToTextPoint(Point position)
	{
		switch (selectionType)
		{
		case SelectionType.Stream:
			if (position.Y == 0)
			{
				return new Point(position.X + selectionRect.Left, selectionRect.Top);
			}
			return new Point(position.X, selectionRect.Top + position.Y);
		case SelectionType.Block:
			return new Point(position.X + selectionRect.Left, position.Y + selectionRect.Top);
		default:
			return position;
		}
	}

	public Point TextToSelectionPoint(Point position)
	{
		switch (selectionType)
		{
		case SelectionType.Stream:
			if (position.Y == selectionRect.Top)
			{
				return new Point(position.X - selectionRect.Left, 0);
			}
			return new Point(position.X, position.Y - selectionRect.Top);
		case SelectionType.Block:
			return new Point(position.X - selectionRect.Left, position.Y - selectionRect.Top);
		default:
			return position;
		}
	}

	public void SetSelectedText(string text, SelectionType selType)
	{
		BeginUpdate();
		TextSource textSource = owner.Source as TextSource;
		textSource.BeginUpdate(UpdateReason.Other);
		NavigateOptions navigateOptions = textSource.NavigateOptions;
		try
		{
			textSource.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEol);
			((SyntaxEdit)owner).FinishVerticalNavigate();
			Point point = (IsEmpty ? textSource.Position : SelectionRect.Location);
			bool flag = !IsEmpty && textSource.Position.Equals((object?)point);
			string text2 = textSource.Lines[point.Y];
			int length = text2.Length;
			if (point.X > length && !textSource.Lines.UseSpaces)
			{
				point.X = length + textSource.Lines.GetIndentString(point.X - length, textSource.Lines.TabPosToPos(text2, length)).Length;
			}
			if (!Delete())
			{
				return;
			}
			if (text != string.Empty && text != null)
			{
				if (selType == SelectionType.Stream)
				{
					textSource.InsertBlock(text);
					SetSelection(SelectionType.Stream, point, owner.Position);
					if (flag)
					{
						owner.Position = point;
					}
				}
				else
				{
					textSource.Position = point;
					textSource.BeginUpdate(UpdateReason.Insert);
					try
					{
						string[] array = StrItem.Split(text);
						length = 0;
						for (int i = 0; i < array.Length; i++)
						{
							if (i > 0 && selType == SelectionType.Block)
							{
								textSource.Navigate(-length, 0);
							}
							length = DoInsertString(selType, array[i], i != 0);
						}
						SetSelection(selType, point, owner.Position);
						if (flag)
						{
							owner.Position = point;
						}
					}
					finally
					{
						owner.Source.EndUpdate();
					}
				}
			}
			selStart = new Point(selectionRect.Left, selectionRect.Top);
			selEnd = new Point(selectionRect.Right, selectionRect.Bottom);
		}
		finally
		{
			textSource.SetNavigateOptions(navigateOptions);
			textSource.EndUpdate();
			EndUpdate();
		}
	}

	public int SelectedCount()
	{
		if (selectionType == SelectionType.None)
		{
			return 0;
		}
		return SelectionRect.Bottom - SelectionRect.Top + 1;
	}

	public string SelectedString(int index)
	{
		if (IsEmpty)
		{
			return null;
		}
		int top = selectionRect.Top;
		ArrayList arrayList = new ArrayList();
		GetSelectedBounds(index + top, arrayList);
		if (arrayList.Count == 0)
		{
			return null;
		}
		switch (SelectionType)
		{
		case SelectionType.Block:
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < arrayList.Count; i++)
			{
				IRange range2 = (IRange)arrayList[i];
				stringBuilder.Append(SafeSubString(owner.Lines[range2.StartPoint.Y], range2.StartPoint.X, range2.EndPoint.X));
				if (i != arrayList.Count - 1)
				{
					stringBuilder.Append(owner.LineTerminator);
				}
			}
			return stringBuilder.ToString();
		}
		case SelectionType.Stream:
		{
			IRange range = (IRange)arrayList[0];
			return SafeSubString(owner.Lines[range.StartPoint.Y], range.StartPoint.X, range.EndPoint.X);
		}
		default:
			return null;
		}
	}

	public string SelectedData(int index)
	{
		if (IsEmpty)
		{
			return null;
		}
		int top = selectionRect.Top;
		ArrayList arrayList = new ArrayList();
		GetSelectedBounds(index + top, arrayList);
		if (arrayList.Count == 0)
		{
			return null;
		}
		switch (SelectionType)
		{
		case SelectionType.Block:
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < arrayList.Count; i++)
			{
				IRange range2 = (IRange)arrayList[i];
				IStrItem item2 = owner.Lines.GetItem(range2.StartPoint.Y);
				if (item2 != null)
				{
					stringBuilder.Append(SafeSubString(item2.ColorData, range2.StartPoint.X, range2.EndPoint.X));
				}
				if (i != arrayList.Count - 1)
				{
					stringBuilder.Append(new string('\0', owner.LineTerminator.Length));
				}
			}
			return stringBuilder.ToString();
		}
		case SelectionType.Stream:
		{
			IRange range = (IRange)arrayList[0];
			IStrItem item = owner.Lines.GetItem(range.StartPoint.Y);
			if (item == null)
			{
				return string.Empty;
			}
			return SafeSubString(item.ColorData, range.StartPoint.X, range.EndPoint.X);
		}
		default:
			return null;
		}
	}

	public void DeleteLeft()
	{
		DeleteLeft(deleteWord: false);
	}

	public void DeleteWordLeft()
	{
		DeleteLeft(deleteWord: true);
	}

	public void DeleteRight()
	{
		DeleteRight(deleteWord: false);
	}

	public void DeleteWordRight()
	{
		DeleteRight(deleteWord: true);
	}

	public void NewLine()
	{
		if (owner.SingleLineMode)
		{
			return;
		}
		bool flag = true;
		TextSource textSource = owner.Source as TextSource;
		textSource.BeginUpdate(UpdateReason.Break);
		try
		{
			if (!((SyntaxEdit)owner).ProcessEnter())
			{
				if (ShouldDelete())
				{
					flag = Delete();
				}
				if (textSource.NewLine() && (textSource.IndentOptions & IndentOptions.AutoIndent) != 0)
				{
					string code = string.Empty;
					if (textSource.NeedAutoComplete() && textSource.Lines[textSource.Position.Y].Trim() == string.Empty && textSource.ProcessAutoComplete(out code))
					{
						Point position = textSource.Position;
						textSource.InsertBlock(code);
						SetSelection(SelectionType.Stream, position, owner.Position);
						SmartFormat();
						textSource.Position = position;
						Clear();
						if (textSource.Lines[textSource.Position.Y].Trim() == string.Empty)
						{
							textSource.DeleteLeft(textSource.Position.X);
							textSource.IndentLine();
						}
					}
					else
					{
						((SyntaxEdit)owner).ProcessAutoIndent('\r');
					}
				}
			}
		}
		finally
		{
			textSource.EndUpdate();
		}
		if (!flag && !IsEmpty)
		{
			Clear();
		}
	}

	public void NewLineAbove()
	{
		if (!((SyntaxEdit)owner).ProcessEnter())
		{
			owner.Source.NewLineAbove();
		}
	}

	public void NewLineBelow()
	{
		if (!((SyntaxEdit)owner).ProcessEnter())
		{
			owner.Source.NewLineBelow();
		}
	}

	public void ProcessEscape()
	{
		SelectionType = SelectionType.None;
	}

	public void CharTransponse()
	{
		Point position = owner.Position;
		string text = owner.Lines[position.Y];
		if (text.Length < 2)
		{
			return;
		}
		position.X = Math.Max(Math.Min(position.X, text.Length - 1), 1);
		ITextSource source = owner.Source;
		source.BeginUpdate(UpdateReason.Insert);
		try
		{
			char c = text[position.X];
			source.MoveToChar(position.X);
			source.DeleteRight(1);
			owner.MoveToChar(position.X - 1);
			source.Insert(c.ToString());
			source.MoveToChar(position.X + 1);
		}
		finally
		{
			source.EndUpdate();
		}
	}

	public void WordTransponse()
	{
		owner.Source.BeginUpdate(UpdateReason.Insert);
		try
		{
			Point position = owner.Position;
			if (SelectCurrentWord() && !IsEmpty)
			{
				Rectangle rectangle = SelectionRect;
				SelectionType selectionType = SelectionType;
				string selectedText = SelectedText;
				SelectNextWord();
				if (!IsEmpty)
				{
					string selectedText2 = SelectedText;
					Delete();
					InsertString(selectedText);
					owner.MoveTo(rectangle.Location);
					owner.Selection.SetSelection(selectionType, rectangle);
					Delete();
					InsertString(selectedText2);
					return;
				}
			}
			owner.Position = position;
		}
		finally
		{
			owner.Source.EndUpdate();
		}
	}

	public void LineTransponse()
	{
		if (owner.Position.Y < owner.Lines.Count - 1)
		{
			ITextSource source = owner.Source;
			source.BeginUpdate(UpdateReason.Insert);
			try
			{
				string text = owner.Lines[owner.Position.Y];
				owner.MoveLineBegin();
				source.DeleteRight(int.MaxValue);
				source.UnBreakLine();
				owner.MoveLineEnd();
				source.BreakLine();
				source.MoveTo(0, source.Position.Y + 1);
				source.Insert(text);
			}
			finally
			{
				source.EndUpdate();
			}
		}
	}

	public void CutLine()
	{
		if (IsEmpty)
		{
			SelectLine();
		}
		Cut();
	}

	public void DeleteLine()
	{
		if (IsEmpty)
		{
			SelectLine();
		}
		Delete();
	}

	public void ProcessTab()
	{
		if (((SyntaxEdit)owner).ProcessTab(owner.Position))
		{
			return;
		}
		if (IsEmpty || SelectedCount() == 1)
		{
			if (owner.Lines.UseSpaces)
			{
				ISyntaxStrings lines = owner.Lines;
				Point position = owner.Position;
				position.X = lines.TabPosToPos(lines[position.Y], position.X);
				int num = lines.GetTabStop(position.X) - position.X;
				if (num > 0)
				{
					InsertString(new string(' ', num));
				}
			}
			else
			{
				InsertString('\t'.ToString());
			}
		}
		else if (!owner.Source.Readonly && !SelectionContainsReadonlyLines())
		{
			Indent();
		}
	}

	public void ProcessShiftTab()
	{
		if (!((SyntaxEdit)owner).ProcessShiftTab(owner.Position))
		{
			if (IsEmpty)
			{
				owner.MoveToChar(owner.Lines.GetPrevTabStop(owner.Position.X));
			}
			else
			{
				UnIndent();
			}
		}
	}

	public void ToggleOverWrite()
	{
		owner.Source.OverWrite = !owner.Source.OverWrite;
	}

	public void InsertString(string str)
	{
		bool flag = true;
		owner.Source.BeginUpdate(UpdateReason.Insert);
		try
		{
			if (ShouldDelete())
			{
				flag = Delete();
			}
			owner.Source.Insert(str);
		}
		finally
		{
			owner.Source.EndUpdate();
		}
		if (!flag && !IsEmpty)
		{
			Clear();
		}
	}

	public virtual void ResetBackColor()
	{
		BackColor = EditConsts.DefaultHighlightBackColor;
	}

	public virtual void ResetForeColor()
	{
		ForeColor = EditConsts.DefaultHighlightForeColor;
	}

	public virtual void ResetInActiveBackColor()
	{
		InActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;
	}

	public virtual void ResetInActiveForeColor()
	{
		InActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;
	}

	public virtual void ResetBorderColor()
	{
		BorderColor = EditConsts.DefaultSelectionBorderColor;
	}

	public virtual void ResetOptions()
	{
		Options = EditConsts.DefaultSelectionOptions;
	}

	public virtual void ResetAllowedSelectionMode()
	{
		AllowedSelectionMode = EditConsts.DefaultSelectionMode;
	}

	public bool ShouldSerializeBackColor()
	{
		return backColor != EditConsts.DefaultHighlightBackColor;
	}

	public bool ShouldSerializeForeColor()
	{
		return foreColor != EditConsts.DefaultHighlightForeColor;
	}

	public bool ShouldSerializeInActiveBackColor()
	{
		return inActiveBackColor != EditConsts.DefaultInactiveHighlightBackColor;
	}

	public bool ShouldSerializeInActiveForeColor()
	{
		return inActiveForeColor != EditConsts.DefaultInactiveHighlightForeColor;
	}

	public bool ShouldSerializeBorderColor()
	{
		return borderColor != EditConsts.DefaultSelectionBorderColor;
	}

	public bool ShouldSerializeOptions()
	{
		return options != EditConsts.DefaultSelectionOptions;
	}

	public bool ShouldSerializeAllowedSelectionMode()
	{
		return AllowedSelectionMode != EditConsts.DefaultSelectionMode;
	}
}
