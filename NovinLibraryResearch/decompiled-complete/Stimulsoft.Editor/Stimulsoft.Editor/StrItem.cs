using System.ComponentModel;

namespace Stimulsoft.Editor;

public class StrItem : IStrItem
{
	private string str;

	private int lexState;

	private int prevLexState;

	private StrItemState state;

	private short[] colorData;

	[Description("Gets or sets string content of the \"StrItem\".")]
	public virtual string String
	{
		get
		{
			return str;
		}
		set
		{
			if (str != value)
			{
				str = value;
				colorData = new short[str.Length];
			}
		}
	}

	[Description("Gets or sets color information of the \"StrItem\".")]
	public short[] ColorData
	{
		get
		{
			return colorData;
		}
		set
		{
			colorData = value;
		}
	}

	[Description("Gets or sets current state of the \"StrItem\".")]
	public StrItemState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}

	[Description("Gets or sets index of lexical state at item end.")]
	public int LexState
	{
		get
		{
			return lexState;
		}
		set
		{
			lexState = value;
		}
	}

	[Description("Gets or sets index of lexical state at item start.")]
	public int PrevLexState
	{
		get
		{
			return prevLexState;
		}
		set
		{
			prevLexState = value;
		}
	}

	public StrItem(string s)
	{
		String = s;
	}

	public static void SetColorFlag(ref short[] colorData, int start, int len, ColorFlags flag, bool setFlag)
	{
		for (int i = start; i < start + len; i++)
		{
			short num = colorData[i];
			byte b = (byte)flag;
			byte b2 = (setFlag ? ((byte)((num >> 8) | b)) : ((byte)((num >> 8) & ~(byte)flag)));
			colorData[i] = (short)((byte)num | (b2 << 8));
		}
	}

	public void SetColorFlag(int start, int len, ColorFlags flag, bool setFlag)
	{
		SetColorFlag(ref colorData, start, len, flag, setFlag);
	}

	public void GetColorFlag(int pos, out ColorFlags flag)
	{
		int num = colorData[pos];
		flag = (ColorFlags)(num >> 8);
	}

	public static string[] Split(string text)
	{
		if (text == null || !(text != string.Empty))
		{
			return new string[0];
		}
		return text.Replace("\r\n", '\r'.ToString()).Split(Consts.crlfArray, int.MaxValue);
	}
}
