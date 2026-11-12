namespace Stimulsoft.Base;

public class StiToken
{
	public int Index { get; set; }

	public int Length { get; set; }

	public StiTokenType Type { get; set; }

	public object Data { get; set; }

	public override string ToString()
	{
		return Type switch
		{
			StiTokenType.Value => $"{Type}={Data}", 
			StiTokenType.Ident => $"{Type}({Data})", 
			_ => Type.ToString(), 
		};
	}

	public StiToken(StiTokenType type)
		: this(type, 0, 0)
	{
	}

	public StiToken(StiTokenType type, int index, int length)
	{
		Type = type;
		Index = index;
		Length = length;
	}

	public StiToken(StiTokenType type, int index, int length, char charValue)
		: this(type, index, length)
	{
		Data = charValue;
	}

	public StiToken(StiTokenType type, int index, int length, string stringValue)
		: this(type, index, length)
	{
		Data = stringValue;
	}

	public StiToken(StiTokenType type, int index, int length, object obj)
		: this(type, index, length)
	{
		Data = obj;
	}
}
