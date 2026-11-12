using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class Parser : Lexer, IParser, ILexer
{
	private class ParserState
	{
		public int LineIndex;

		public int Token;

		public int TokenPos;

		public int CurrentPos;

		public int State;
	}

	private ArrayList stack;

	private int state;

	private int token;

	protected int lineIndex;

	protected int tokenPos;

	protected int currentPos;

	protected string source;

	private IStringList strings;

	private IStringList internalStrings;

	protected ArrayList Stack => stack;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Eof
	{
		get
		{
			if (token != -1 && strings != null)
			{
				return lineIndex >= strings.Count;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Token
	{
		get
		{
			return token;
		}
		set
		{
			token = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int State
	{
		get
		{
			return state;
		}
		set
		{
			if (state != value)
			{
				state = value;
				StateChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string TokenString
	{
		get
		{
			if (source == null || tokenPos >= source.Length)
			{
				return string.Empty;
			}
			return source.Substring(tokenPos, Math.Min(currentPos, source.Length) - tokenPos);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point TokenPosition => ValidatePosition(new Point(tokenPos, lineIndex));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point CurrentPosition => ValidatePosition(new Point(currentPos, lineIndex));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual IStringList Strings
	{
		get
		{
			return strings;
		}
		set
		{
			strings = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] Lines
	{
		get
		{
			if (strings == null)
			{
				return null;
			}
			string[] array = new string[strings.Count];
			strings.CopyTo(array, 0);
			return array;
		}
		set
		{
			if (internalStrings == null)
			{
				internalStrings = new StringList();
			}
			strings = internalStrings;
			foreach (string value2 in value)
			{
				((StringList)strings).Add(value2);
			}
		}
	}

	public Parser()
	{
		stack = new ArrayList();
	}

	protected internal bool IsStackEmpty()
	{
		return stack.Count == 0;
	}

	protected virtual void ResetLine(int line)
	{
		source = ((strings != null && line >= 0 && line < strings.Count) ? strings[line] : null);
	}

	protected virtual bool UpdateLine()
	{
		currentPos = 0;
		tokenPos = 0;
		source = null;
		while (strings != null && (source == null || source == string.Empty) && lineIndex < strings.Count)
		{
			lineIndex++;
			ResetLine(lineIndex);
		}
		if (source != null)
		{
			return source != string.Empty;
		}
		return false;
	}

	protected virtual bool IsValidToken(int Token)
	{
		return true;
	}

	public virtual void Reset(int line, int pos, int state)
	{
		lineIndex = line;
		token = 0;
		tokenPos = pos;
		currentPos = pos;
		State = state;
		ResetLine(line);
	}

	protected virtual void StateChanged()
	{
	}

	protected virtual Point ValidatePosition(Point position)
	{
		if (strings != null && position.Y >= strings.Count)
		{
			if (strings.Count != 0)
			{
				return new Point(strings[strings.Count - 1].Length, strings.Count - 1);
			}
			return new Point(0, 0);
		}
		return position;
	}

	public virtual void Reset()
	{
		Reset(0, 0, base.DefaultState);
	}

	public virtual int NextToken()
	{
		tokenPos = currentPos;
		if ((source == null || currentPos >= source.Length) && !UpdateLine())
		{
			token = -1;
		}
		else
		{
			int len = 0;
			State = ParseText(state, lineIndex, source, ref tokenPos, ref len, ref token);
			currentPos = tokenPos + len;
		}
		return token;
	}

	public int NextToken(out string str)
	{
		int result = NextToken();
		str = TokenString;
		return result;
	}

	public int PeekToken()
	{
		SaveState();
		try
		{
			return NextToken();
		}
		finally
		{
			RestoreState();
		}
	}

	public int PeekToken(out string str)
	{
		SaveState();
		try
		{
			int result = NextToken();
			str = TokenString;
			return result;
		}
		finally
		{
			RestoreState();
		}
	}

	public int PeekValidToken()
	{
		SaveState();
		try
		{
			return NextValidToken();
		}
		finally
		{
			RestoreState();
		}
	}

	public int PeekValidToken(out string str)
	{
		SaveState();
		try
		{
			int result = NextValidToken();
			str = TokenString;
			return result;
		}
		finally
		{
			RestoreState();
		}
	}

	protected virtual int DoNextValidToken()
	{
		int result = NextToken();
		while (!Eof && !IsValidToken(result))
		{
			result = NextToken();
		}
		return result;
	}

	protected void SetToken(int token)
	{
		this.token = token;
	}

	public virtual int NextValidToken()
	{
		return DoNextValidToken();
	}

	public int NextValidToken(out string str)
	{
		int result = NextValidToken();
		str = TokenString;
		return result;
	}

	public virtual void SaveState()
	{
		ParserState parserState = new ParserState();
		parserState.LineIndex = lineIndex;
		parserState.Token = token;
		parserState.TokenPos = tokenPos;
		parserState.CurrentPos = currentPos;
		parserState.State = state;
		stack.Add(parserState);
	}

	public void RestoreState()
	{
		RestoreState(restore: true);
	}

	public virtual void RestoreState(bool restore)
	{
		if (stack.Count > 0)
		{
			if (restore)
			{
				ParserState parserState = (ParserState)stack[stack.Count - 1];
				lineIndex = parserState.LineIndex;
				token = parserState.Token;
				tokenPos = parserState.TokenPos;
				currentPos = parserState.CurrentPos;
				State = parserState.State;
				ResetLine(lineIndex);
			}
			stack.RemoveAt(stack.Count - 1);
		}
	}
}
