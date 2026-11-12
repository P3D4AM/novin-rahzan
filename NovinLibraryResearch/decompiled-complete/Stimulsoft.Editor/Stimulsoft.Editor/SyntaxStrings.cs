using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class SyntaxStrings : ISyntaxStrings, IStringList, ICollection, IEnumerable, IImport, IExport, ITextSearch, IFmtExport, ITabulationEx, ITabulation, IWordBreak, INotifyEx, INotify
{
	private ArrayList list;

	private ITextSource source;

	private ISyntaxEdit owner;

	private int updateCount;

	private bool removeTrailingSpaces;

	private int currentIndex;

	private string lineTerminator = "\r\n";

	private StringBuilder tabBuilder;

	private int[] tabStops = new int[1] { EditConsts.DefaultTabStop };

	private char[] tabArray = new char[1] { '\t' };

	private ArrayList tabList;

	private bool useSpaces;

	private int firstChanged = -1;

	private int lastChanged = -1;

	private char[] delimiters;

	private Hashtable delimTable;

	[Description("Gets or sets the strings in the \"SyntaxStrings\" as a single string with the individual strings delimited by carriage returns.")]
	public string Text
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrItem item in list)
			{
				stringBuilder.Append(item.String + lineTerminator);
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - lineTerminator.Length, lineTerminator.Length);
			}
			return stringBuilder.ToString();
		}
		set
		{
			SetTextAndData(value, null);
		}
	}

	protected internal ISyntaxEdit Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	[Description("Gets or set a boolean value thet indicates whether \"SyntaxStrings\" should remove trailing spaces from the end of each its strings.")]
	public bool RemoveTrailingSpaces
	{
		get
		{
			return removeTrailingSpaces;
		}
		set
		{
			removeTrailingSpaces = value;
		}
	}

	[Description("Gets the text source owning the \"SyntaxStrings\".")]
	public ITextSource Source => source;

	public string this[int index]
	{
		get
		{
			if (index < 0 || index >= Count)
			{
				return string.Empty;
			}
			return ((StrItem)list[index]).String;
		}
		set
		{
			if (index >= 0 && index < list.Count)
			{
				StrItem strItem = (StrItem)list[index];
				strItem.String = value;
				strItem.State &= ~StrItemState.Parsed;
				Changed(index);
			}
		}
	}

	[Description("Indicates whether the collection has a fixed size.")]
	public bool IsFixedSize => false;

	[Description("Gets a value indicating whether the collection is read-only.")]
	public bool IsReadonly => false;

	[Description("Represents current string in the collection.")]
	public object Current
	{
		get
		{
			if (currentIndex >= 0 && currentIndex < Count)
			{
				return ((StrItem)list[currentIndex]).String;
			}
			return null;
		}
	}

	[Description("Represents number of strings in the collection.")]
	public int Count => list.Count;

	[Description("Gets a value indicating whether access to the collection is synchronized.")]
	public bool IsSynchronized => list.IsSynchronized;

	[Description("Gets an object that can be used to synchronize access to the \"SyntaxStrings\" object.")]
	public object SyncRoot => list.SyncRoot;

	[XmlIgnore]
	[DefaultValue("\r\n")]
	[Description("Gets or sets a string value that terminates line.")]
	public string LineTerminator
	{
		get
		{
			return lineTerminator;
		}
		set
		{
			lineTerminator = value;
		}
	}

	[Description("Gets or sets the character columns that the cursor will move to each time you press Tab.")]
	public int[] TabStops
	{
		get
		{
			return tabStops;
		}
		set
		{
			tabStops = new int[value.Length];
			Array.Copy(value, tabStops, value.Length);
			tabList.Clear();
			int num = 0;
			int[] array = tabStops;
			foreach (int num2 in array)
			{
				if (num2 <= num)
				{
					ErrorHandler.Error(new Exception(string.Format(StringConsts.InvalidTabStop, value.ToString())));
				}
				num = num2;
			}
		}
	}

	[Description("Gets or sets a value indicating whether indent or TAB operations insert space characters rather than TAB characters.")]
	public bool UseSpaces
	{
		get
		{
			return useSpaces;
		}
		set
		{
			useSpaces = value;
		}
	}

	[Description("Keeps track of calls to \"BeginUpdate\" and \"EndUpdate\" so that they can be nested.")]
	public int UpdateCount => updateCount;

	[Description("Represents number of the first changed line.")]
	public int FirstChanged
	{
		get
		{
			return firstChanged;
		}
		set
		{
			firstChanged = value;
		}
	}

	[Description("Represents number of the last changed line.")]
	public int LastChanged
	{
		get
		{
			return lastChanged;
		}
		set
		{
			lastChanged = value;
		}
	}

	[Description("Gets or sets an array of characters used as delimiters between words in the text.")]
	public char[] Delimiters
	{
		get
		{
			return delimiters;
		}
		set
		{
			delimiters = new char[value.Length];
			Array.Copy(value, delimiters, value.Length);
			UpdateDelimTable();
		}
	}

	[Description("Gets or sets \"Delimiters\" as a single string.")]
	public string DelimiterString
	{
		get
		{
			string text = string.Empty;
			char[] array = Delimiters;
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if (c > ' ')
				{
					text += c;
				}
			}
			return text;
		}
		set
		{
			InitDelimiters(value.ToCharArray());
		}
	}

	[Description("Represents \"Delimiters\" as a key-and-value pairs collection where each key is a single delimiter character.")]
	public Hashtable DelimTable => delimTable;

	public object XmlInfo
	{
		get
		{
			return new XmlSyntaxStringsInfo(this);
		}
		set
		{
			((XmlSyntaxStringsInfo)value).FixupReferences(this);
		}
	}

	private event EventHandler notifyHandler;

	public SyntaxStrings(TextSource source)
		: this()
	{
		this.source = source;
	}

	public SyntaxStrings()
	{
		list = new ArrayList();
		tabList = new ArrayList();
		tabBuilder = new StringBuilder();
		delimTable = new Hashtable();
		InitDelimiters(EditConsts.DefaultDelimiters.ToCharArray());
	}

	~SyntaxStrings()
	{
		delimTable.Clear();
	}

	public virtual IStrItem CreateStrItem(string s)
	{
		if (source != null)
		{
			return source.CreateStrItem(s);
		}
		return new StrItem(s);
	}

	private short[] GetColorData(string s, int left, int len)
	{
		if (left < s.Length)
		{
			s = ((left + len >= s.Length) ? s.Substring(left) : s.Substring(left, len));
			short[] array = new short[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				array[i] = (short)s[i];
			}
			return array;
		}
		return null;
	}

	protected virtual int AddData(string value, short[] colorData)
	{
		IStrItem strItem = CreateStrItem(value);
		if (colorData != null && colorData.Length == value.Length)
		{
			strItem.ColorData = colorData;
		}
		int num = list.Add(strItem);
		Changed(num);
		return num;
	}

	public int Add(string value)
	{
		int num = list.Add(CreateStrItem(value));
		Changed(num);
		return num;
	}

	public void Clear()
	{
		list.Clear();
		Changed(0, int.MaxValue);
	}

	public bool Contains(string value)
	{
		foreach (StrItem item in list)
		{
			if (item.String.Equals(value))
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(string value)
	{
		for (int i = 0; i < Count; i++)
		{
			if (((StrItem)list[i]).String.Equals(value))
			{
				return i;
			}
		}
		return -1;
	}

	public void Insert(int index, string value)
	{
		list.Insert(index, CreateStrItem(value));
		Changed(index, int.MaxValue);
	}

	public void Remove(string value)
	{
		int num = IndexOf(value);
		if (num >= 0)
		{
			RemoveAt(num);
		}
	}

	public void RemoveAt(int index)
	{
		if (index >= 0 && index < Count)
		{
			list.RemoveAt(index);
			if (index < Count)
			{
				((StrItem)list[index]).State &= ~StrItemState.Parsed;
			}
			Changed(index, int.MaxValue);
		}
	}

	public IStrItem GetItem(int index)
	{
		if (index < 0 || index >= Count)
		{
			return CreateStrItem(string.Empty);
		}
		return (StrItem)list[index];
	}

	public int GetLength(int index)
	{
		if (index < 0 || index >= Count)
		{
			return 0;
		}
		return ((StrItem)list[index]).String.Length;
	}

	public void Changed(int first, int last)
	{
		if (firstChanged == -1)
		{
			firstChanged = first;
		}
		else
		{
			firstChanged = Math.Min(firstChanged, first);
		}
		lastChanged = Math.Max(lastChanged, last);
		StringsChanged();
	}

	public void Changed(int index)
	{
		Changed(index, index);
	}

	public IList ToArrayList()
	{
		ArrayList arrayList = new ArrayList(Count);
		foreach (StrItem item in list)
		{
			arrayList.Add(item.String);
		}
		return arrayList;
	}

	public string[] ToStringArray()
	{
		string[] array = new string[Count];
		for (int i = 0; i < Count; i++)
		{
			array[i] = this[i];
		}
		return array;
	}

	public char GetCharAt(Point position)
	{
		return GetCharAt(position.X, position.Y);
	}

	public char GetCharAt(int x, int y)
	{
		IStrItem item = GetItem(y);
		if (item != null && x >= 0 && x < item.String.Length)
		{
			return item.String[x];
		}
		return '\0';
	}

	public int GetLexStyle(Point position)
	{
		short[] colorData = ((StrItem)list[position.Y]).ColorData;
		if (colorData == null || position.X < 0 || position.X >= colorData.Length)
		{
			return -1;
		}
		return colorData[position.X] - 1;
	}

	public string RemoveSpaces(string s)
	{
		if (!removeTrailingSpaces)
		{
			return s;
		}
		return s.TrimEnd(new char[0]);
	}

	public int TextPointToAbsolutePosition(Point position)
	{
		return TextPointToAbsolutePosition(this, position, lineTerminator);
	}

	public Point AbsolutePositionToTextPoint(int position)
	{
		return AbsolutePositionToTextPoint(this, position, lineTerminator);
	}

	public void SetTextAndData(string text, string data)
	{
		if (source != null)
		{
			source.BeginUpdate(UpdateReason.Other);
		}
		BeginUpdate();
		try
		{
			Clear();
			if (text != null && text != string.Empty)
			{
				StringReader stringReader = new StringReader(text);
				int num = 0;
				string text2;
				while ((text2 = stringReader.ReadLine()) != null)
				{
					if (data != null)
					{
						string text3 = RemoveSpaces(text2);
						AddData(text3, GetColorData(data, num, text3.Length));
						num += text2.Length;
						if (num < text.Length - 1 && text[num] == '\r' && text[num + 1] == '\n')
						{
							num += 2;
						}
						else if (num < text.Length && (text[num] == '\r' || text[num] == '\n'))
						{
							num++;
						}
					}
					else
					{
						Add(RemoveSpaces(text2));
					}
				}
				char c = text[text.Length - 1];
				if (c == '\r' || c == '\n')
				{
					Add(string.Empty);
				}
			}
			if (source != null)
			{
				source.State |= NotifyState.CountChanged;
				if (data != null)
				{
					((TextSource)source).SetLastParsed(Count);
				}
			}
			Changed(0, int.MaxValue);
		}
		finally
		{
			EndUpdate();
			if (source != null)
			{
				source.EndUpdate();
			}
		}
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public bool MoveNext()
	{
		currentIndex++;
		return currentIndex < Count;
	}

	public void Reset()
	{
		currentIndex = 0;
	}

	public void CopyTo(Array array, int index)
	{
		list.CopyTo(array, index);
	}

	private static bool IsWholeWord(Hashtable delimTable, string s, int start, int len, ref int wordStart, ref int wordEnd)
	{
		wordStart = start;
		wordEnd = start + len;
		int length = s.Length;
		if (wordStart <= 0 || !IsDelimiter(delimTable, s, wordStart))
		{
			while (wordStart > 0 && !IsDelimiter(delimTable, s, wordStart - 1))
			{
				wordStart--;
			}
		}
		if (wordEnd >= length - 1 || wordEnd <= 0 || !IsDelimiter(delimTable, s, wordEnd - 1))
		{
			while (wordEnd < length && !IsDelimiter(delimTable, s, wordEnd))
			{
				wordEnd++;
			}
		}
		if (wordStart == start)
		{
			return len == wordEnd - wordStart;
		}
		return false;
	}

	public static int TextPointToAbsolutePosition(IStringList list, Point position, string lineTerminator)
	{
		int num = 0;
		int length = lineTerminator.Length;
		int count = list.Count;
		for (int i = 0; i < position.Y; i++)
		{
			num += ((i < count) ? list[i].Length : 0) + length;
		}
		if (position.Y < count)
		{
			num += Math.Min(position.X, list[position.Y].Length);
		}
		return num;
	}

	public static Point AbsolutePositionToTextPoint(IStringList list, int position, string lineTerminator)
	{
		Point result = new Point(0, 0);
		int length = lineTerminator.Length;
		int count = list.Count;
		while (position > 0)
		{
			int num = ((result.Y < count) ? list[result.Y].Length : 0);
			if (position > num)
			{
				result.Y++;
				position -= num + length;
				continue;
			}
			result.X = position;
			break;
		}
		return result;
	}

	public static bool Find(IStringList list, Hashtable delimTable, string str, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
	{
		match = null;
		len = str?.Length ?? 0;
		if (str == null || str == string.Empty)
		{
			return false;
		}
		bool flag = (options & SearchOptions.CaseSensitive) == 0 && expression == null;
		if (flag)
		{
			str = str.ToUpper(CultureInfo.InvariantCulture);
		}
		string text = null;
		int wordStart = 0;
		int wordEnd = 0;
		if ((options & SearchOptions.BackwardSearch) != 0)
		{
			if (expression != null && (expression.Options & RegexOptions.Multiline) != 0)
			{
				int val = TextPointToAbsolutePosition(list, position, list.LineTerminator);
				string text2 = list.Text;
				match = expression.Match(text2, Math.Min(val, text2.Length));
				if (match.Success && match.Length > 0)
				{
					len = match.Length;
					position = AbsolutePositionToTextPoint(list, match.Index, list.LineTerminator);
					return true;
				}
				return false;
			}
			position.Y = Math.Min(position.Y, list.Count - 1);
			while (position.Y >= 0)
			{
				text = list[position.Y];
				if (position.X > 0 || (text == string.Empty && position.X == 0 && expression != null))
				{
					if (flag)
					{
						text = text.ToUpper(CultureInfo.InvariantCulture);
					}
					if (expression != null)
					{
						match = ((text == string.Empty) ? expression.Match(text) : expression.Match(text, Math.Min(position.X, text.Length)));
						if (match.Success)
						{
							len = match.Length;
							position.X = match.Index;
						}
						else
						{
							position.X = -1;
						}
					}
					else if (text != string.Empty)
					{
						position.X = text.LastIndexOf(str, Math.Min(position.X - 1, text.Length - 1));
					}
					else
					{
						position.X = -1;
					}
					if (position.X >= 0 && (options & SearchOptions.WholeWordsOnly) != 0 && !IsWholeWord(delimTable, text, position.X, len, ref wordStart, ref wordEnd))
					{
						position.X = wordStart - 1;
						continue;
					}
					if (position.X >= 0)
					{
						return true;
					}
				}
				position.Y--;
				if (position.Y >= 0)
				{
					position.X = list[position.Y].Length;
				}
			}
		}
		else
		{
			if (expression != null && (expression.Options & RegexOptions.Multiline) != 0)
			{
				int num = TextPointToAbsolutePosition(list, position, list.LineTerminator);
				string text3 = list.Text;
				if (num < text3.Length)
				{
					match = expression.Match(text3, num);
					if (match.Success && match.Length > 0)
					{
						len = match.Length;
						position = AbsolutePositionToTextPoint(list, match.Index, list.LineTerminator);
						return true;
					}
				}
				return false;
			}
			while (position.Y < list.Count)
			{
				text = list[position.Y];
				if (position.X < text.Length || (text == string.Empty && position.X == 0 && expression != null))
				{
					if (flag)
					{
						text = text.ToUpper(CultureInfo.InvariantCulture);
					}
					position.X = Math.Min(position.X, text.Length - 1);
					if (expression != null)
					{
						match = ((text == string.Empty) ? expression.Match(text) : expression.Match(text, position.X));
						if (match.Success)
						{
							len = match.Length;
							position.X = match.Index;
						}
						else
						{
							position.X = -1;
						}
					}
					else if (text != string.Empty)
					{
						position.X = text.IndexOf(str, position.X);
					}
					else
					{
						position.X = -1;
					}
					if (position.X >= 0 && (options & SearchOptions.WholeWordsOnly) != 0 && !IsWholeWord(delimTable, text, position.X, len, ref wordStart, ref wordEnd))
					{
						position.X = ((position.X == wordEnd) ? (position.X + 1) : wordEnd);
						continue;
					}
					if (position.X >= 0)
					{
						return true;
					}
				}
				position.Y++;
				position.X = 0;
			}
		}
		return false;
	}

	public bool Find(string str, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
	{
		return Find(this, delimTable, str, options, expression, ref position, out len, out match);
	}

	public bool SaveFile(string fileName)
	{
		return SaveFile(fileName, null);
	}

	public bool SaveFile(string fileName, ExportFormat format)
	{
		return SaveFile(fileName, format, null);
	}

	public bool SaveFile(string fileName, Encoding encoding)
	{
		return SaveFile(fileName, ExportFormat.Text, encoding);
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
			AfterSave();
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
			result = false;
		}
		return result;
	}

	public void AfterSave()
	{
		if (source != null)
		{
			if ((source.UndoOptions & UndoOptions.UndoAfterSave) == 0)
			{
				source.ClearUndo();
				source.ClearRedo();
			}
			source.Modified = false;
		}
	}

	public bool SaveStream(TextWriter writer)
	{
		return SaveStream(writer, ExportFormat.Text);
	}

	public bool SaveStream(TextWriter writer, ExportFormat format)
	{
		FmtFiler fmtFiler = new FmtFiler((owner != null) ? owner : ((source != null) ? source.ActiveEdit : null), this, format);
		return fmtFiler.Write(writer);
	}

	public bool SaveStream(Stream stream)
	{
		return SaveStream(stream, null);
	}

	public bool SaveStream(Stream stream, Encoding encoding)
	{
		return SaveStream(stream, ExportFormat.Text, encoding);
	}

	public bool SaveStream(Stream stream, ExportFormat format)
	{
		return SaveStream(stream, format, null);
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

	public bool ShouldSerializeLineTerminator()
	{
		return lineTerminator != "\r\n";
	}

	public bool LoadFile(string fileName)
	{
		return LoadFile(fileName, null);
	}

	public bool LoadFile(string fileName, Encoding encoding)
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
					LoadStream(streamReader);
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
		if (source != null)
		{
			source.BeginUpdate(UpdateReason.Other);
		}
		BeginUpdate();
		try
		{
			Clear();
			string s;
			while ((s = reader.ReadLine()) != null)
			{
				Add(RemoveSpaces(s));
			}
			if (source != null)
			{
				source.State |= NotifyState.CountChanged;
			}
			Changed(0, int.MaxValue);
		}
		finally
		{
			EndUpdate();
			if (source != null)
			{
				source.EndUpdate();
			}
		}
		return true;
	}

	public bool LoadStream(Stream stream)
	{
		return LoadStream(stream, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		TextReader textReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
		try
		{
			return LoadStream(textReader);
		}
		finally
		{
			textReader.Close();
		}
	}

	private int GetStop(int pos)
	{
		int num = tabStops.Length;
		if (num == 0)
		{
			return EditConsts.DefaultTabStop;
		}
		int num2 = pos - tabStops[num - 1];
		int num3 = 0;
		if (num2 >= 0)
		{
			num3 = ((num != 1) ? (tabStops[num - 1] - tabStops[num - 2]) : tabStops[0]);
			return tabStops[num - 1] + (num2 / num3 + 1) * num3;
		}
		for (num3 = 0; pos >= tabStops[num3]; num3++)
		{
		}
		return tabStops[num3];
	}

	protected internal int PosToTabPos(string str, int pos, bool tabEnd)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		string[] array = str.Split(tabArray);
		for (int i = 0; i < array.Length - 1; i++)
		{
			int length = array[i].Length;
			num2 = GetTabStop(num + length) - num - length;
			num += length + num2;
			if (pos < num)
			{
				if (num - pos < num2 && !tabEnd)
				{
					num3 += num2 - (num - pos);
				}
				break;
			}
			num3 += num2 - 1;
		}
		return pos - num3;
	}

	protected internal string GetIndentString(int count, int p, bool useSpaces)
	{
		if (useSpaces)
		{
			return new string(' ', count);
		}
		int num = 0;
		int num2 = 0;
		while (num < count)
		{
			int num3 = GetTabStop(p) - p;
			if (num + num3 > count)
			{
				break;
			}
			num += num3;
			p += num3;
			num2++;
		}
		return new string('\t', num2) + new string(' ', count - num);
	}

	protected internal void GetTabString(ref string str, ref short[] data, bool needData)
	{
		if (str == null || str == string.Empty || str.IndexOf('\t') < 0)
		{
			return;
		}
		tabBuilder.Length = 0;
		int num = 0;
		int num2 = 0;
		bool flag = true;
		string[] array = str.Split(tabArray);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (flag)
			{
				tabBuilder.Append(text);
				flag = false;
				num = text.Length;
			}
			else
			{
				num2 = GetTabStop(num) - num;
				tabBuilder.Append(new string(' ', num2));
				tabBuilder.Append(text);
				num += text.Length + num2;
			}
		}
		str = tabBuilder.ToString();
		if (!needData)
		{
			return;
		}
		short[] colorData = new short[str.Length];
		num = 0;
		int num3 = 0;
		num2 = 0;
		flag = true;
		string[] array3 = array;
		foreach (string text2 in array3)
		{
			if (flag)
			{
				flag = false;
				num = text2.Length;
				num3 = num;
				if (num != 0)
				{
					Array.Copy(data, 0, colorData, 0, num);
				}
				continue;
			}
			num2 = GetTabStop(num) - num;
			for (int k = num; k < num + num2; k++)
			{
				colorData[k] = data[num3];
			}
			StrItem.SetColorFlag(ref colorData, num + num2 / 2, 1, ColorFlags.Tabulation, setFlag: true);
			int length = text2.Length;
			Array.Copy(data, num3 + 1, colorData, num + num2, length);
			num += length + num2;
			num3 += length + 1;
		}
		data = colorData;
	}

	public string GetIndentString(int count, int p)
	{
		return GetIndentString(count, p, useSpaces);
	}

	public int GetTabStop(int pos)
	{
		if (pos == int.MaxValue)
		{
			return pos;
		}
		for (int i = tabList.Count; i <= pos; i++)
		{
			tabList.Add(GetStop(i));
		}
		return (int)tabList[pos];
	}

	public int GetPrevTabStop(int pos)
	{
		GetTabStop(pos);
		int num = (int)tabList[pos];
		while (pos > 0 && (int)tabList[pos] == num)
		{
			pos--;
		}
		if (num < 0 || pos <= 0)
		{
			return 0;
		}
		return (int)tabList[pos];
	}

	public string GetTabString(string s)
	{
		short[] data = null;
		GetTabString(ref s, ref data, needData: false);
		return s;
	}

	public int TabPosToPos(string str, int pos)
	{
		if (pos < 0)
		{
			return pos;
		}
		int num = pos - str.Length;
		pos = GetTabString(str.Substring(0, Math.Min(pos, str.Length))).Length;
		if (num > 0)
		{
			return pos + num;
		}
		return pos;
	}

	public int PosToTabPos(string str, int pos)
	{
		return PosToTabPos(str, pos, tabEnd: false);
	}

	public virtual void ResetTabStops()
	{
		TabStops = new int[1] { EditConsts.DefaultTabStop };
	}

	public virtual void ResetUseSpaces()
	{
		UseSpaces = false;
	}

	public void Assign(ISyntaxStrings source)
	{
		BeginUpdate();
		try
		{
			Clear();
			foreach (string item in source)
			{
				Add(item);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	protected void StringsChanged()
	{
		if (updateCount == 0)
		{
			if (this.notifyHandler != null)
			{
				this.notifyHandler(this, EventArgs.Empty);
			}
			firstChanged = -1;
			lastChanged = -1;
		}
	}

	public int BeginUpdate()
	{
		if (updateCount == 0)
		{
			firstChanged = -1;
			lastChanged = -1;
		}
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		StringsChanged();
		return updateCount;
	}

	public void AddNotifier(INotifier sender)
	{
		notifyHandler += sender.Notification;
	}

	public void RemoveNotifier(INotifier sender)
	{
		notifyHandler -= sender.Notification;
	}

	public void Notify()
	{
		StringsChanged();
	}

	private void InitDelimiters(char[] delims)
	{
		delimiters = new char[((delims != null) ? delims.Length : 0) + 32 + 1];
		for (int i = 0; i <= 32; i++)
		{
			delimiters[i] = (char)i;
		}
		if (delims != null)
		{
			for (int j = 0; j < delims.Length; j++)
			{
				delimiters[32 + j + 1] = delims[j];
			}
		}
		UpdateDelimTable();
	}

	private void UpdateDelimTable()
	{
		delimTable.Clear();
		char[] array = delimiters;
		foreach (char c in array)
		{
			delimTable[c] = c;
		}
	}

	protected internal bool GetWord(string str, int pos, out int left, out int right, Hashtable delims)
	{
		int length = str.Length;
		left = 0;
		right = 0;
		if (str != string.Empty && pos <= length)
		{
			if (pos == length)
			{
				pos--;
			}
			if (IsDelimiter(str, pos, delims))
			{
				left = pos;
				while (left > 0 && IsDelimiter(str, left - 1, delims))
				{
					left--;
				}
				right = pos;
				while (right < length - 1 && IsDelimiter(str, right + 1, delims))
				{
					right++;
				}
			}
			else
			{
				left = pos;
				while (left > 0 && !IsDelimiter(str, left - 1, delims))
				{
					left--;
				}
				right = pos;
				while (right < length - 1 && !IsDelimiter(str, right + 1, delims))
				{
					right++;
				}
			}
			return true;
		}
		return false;
	}

	protected internal bool IsDelimiter(string str, int pos, Hashtable delims)
	{
		char c = str[pos];
		if (!delimTable.ContainsKey(c))
		{
			return delims?.ContainsKey(c) ?? false;
		}
		return true;
	}

	public static bool IsDelimiter(Hashtable delimTable, string str, int pos)
	{
		return delimTable.ContainsKey(str[pos]);
	}

	public bool IsDelimiter(string str, int pos)
	{
		return delimTable.ContainsKey(str[pos]);
	}

	public bool IsDelimiter(int index, int pos)
	{
		return IsDelimiter(this[index], pos);
	}

	public bool IsDelimiter(char ch)
	{
		return delimTable.ContainsKey(ch);
	}

	public bool GetWord(string str, int pos, out int left, out int right)
	{
		return GetWord(str, pos, out left, out right, null);
	}

	public bool GetWord(int index, int pos, out int left, out int right)
	{
		return GetWord(this[index], pos, out left, out right);
	}

	public string GetTextAt(int pos, int line)
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
		return GetTextAt(position.X, position.Y);
	}

	public void ResetDelimiters()
	{
		Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
	}
}
