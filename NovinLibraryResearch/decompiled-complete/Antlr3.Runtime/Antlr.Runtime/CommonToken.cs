using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Antlr.Runtime;

[Serializable]
public class CommonToken : IToken
{
	private int type;

	private int line;

	private int charPositionInLine = -1;

	private int channel;

	[NonSerialized]
	private ICharStream input;

	private string text;

	private int index = -1;

	private int start;

	private int stop;

	public string Text
	{
		get
		{
			if (text != null)
			{
				return text;
			}
			if (input == null)
			{
				return null;
			}
			if (start <= stop && stop < input.Count)
			{
				return input.Substring(start, stop - start + 1);
			}
			return "<EOF>";
		}
		set
		{
			text = value;
		}
	}

	public int Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public int Line
	{
		get
		{
			return line;
		}
		set
		{
			line = value;
		}
	}

	public int CharPositionInLine
	{
		get
		{
			return charPositionInLine;
		}
		set
		{
			charPositionInLine = value;
		}
	}

	public int Channel
	{
		get
		{
			return channel;
		}
		set
		{
			channel = value;
		}
	}

	public int StartIndex
	{
		get
		{
			return start;
		}
		set
		{
			start = value;
		}
	}

	public int StopIndex
	{
		get
		{
			return stop;
		}
		set
		{
			stop = value;
		}
	}

	public int TokenIndex
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	public ICharStream InputStream
	{
		get
		{
			return input;
		}
		set
		{
			input = value;
		}
	}

	public CommonToken()
	{
	}

	public CommonToken(int type)
	{
		this.type = type;
	}

	public CommonToken(ICharStream input, int type, int channel, int start, int stop)
	{
		this.input = input;
		this.type = type;
		this.channel = channel;
		this.start = start;
		this.stop = stop;
	}

	public CommonToken(int type, string text)
	{
		this.type = type;
		channel = 0;
		this.text = text;
	}

	public CommonToken(IToken oldToken)
	{
		text = oldToken.Text;
		type = oldToken.Type;
		line = oldToken.Line;
		index = oldToken.TokenIndex;
		charPositionInLine = oldToken.CharPositionInLine;
		channel = oldToken.Channel;
		input = oldToken.InputStream;
		if (oldToken is CommonToken)
		{
			start = ((CommonToken)oldToken).start;
			stop = ((CommonToken)oldToken).stop;
		}
	}

	public override string ToString()
	{
		string text = "";
		if (channel > 0)
		{
			text = ",channel=" + channel;
		}
		string text2 = Text;
		if (text2 != null)
		{
			text2 = Regex.Replace(text2, "\n", "\\\\n");
			text2 = Regex.Replace(text2, "\r", "\\\\r");
			text2 = Regex.Replace(text2, "\t", "\\\\t");
		}
		else
		{
			text2 = "<no text>";
		}
		return "[@" + TokenIndex + "," + start + ":" + stop + "='" + text2 + "',<" + type + ">" + text + "," + line + ":" + CharPositionInLine + "]";
	}

	[OnSerializing]
	internal void OnSerializing(StreamingContext context)
	{
		if (text == null)
		{
			text = Text;
		}
	}
}
