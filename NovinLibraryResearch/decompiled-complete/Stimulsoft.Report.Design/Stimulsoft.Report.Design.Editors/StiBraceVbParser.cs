using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Editor;

namespace Stimulsoft.Report.Design.Editors;

[ToolboxItem(false)]
public class StiBraceVbParser : VbParser
{
	public const int valueWhiteSpace = -1;

	public const int stateValue = -1;

	public const int stateNormal = 0;

	public StiBraceVbParser()
	{
		base.DefaultState = -1;
	}

	private int MoveToNormal()
	{
		currentPos++;
		base.State = 0;
		return 6;
	}

	private int LexWhiteSpace()
	{
		int length = source.Length;
		currentPos++;
		while (currentPos < length && source[currentPos] != '{')
		{
			currentPos++;
		}
		return -1;
	}

	private int MoveToValue()
	{
		currentPos++;
		base.State = -1;
		return 6;
	}

	protected override void InitLexer()
	{
		base.InitLexer();
		RegisterLexerProc(-1, LexWhiteSpace);
		RegisterLexerProc(-1, '{', MoveToNormal);
		RegisterLexerProc(0, '}', MoveToValue);
		ILexStyle lexStyle = Scheme.LexStyles.AddLexStyle();
		lexStyle.BackColor = Color.Transparent;
		lexStyle.ForeColor = Color.DimGray;
	}

	protected override int GetLexerStyle(int token)
	{
		if (token != -1)
		{
			return base.GetLexerStyle(token);
		}
		return Scheme.LexStyles.Count - 1;
	}

	public override void Reset()
	{
		base.Reset();
		base.State = -1;
	}
}
