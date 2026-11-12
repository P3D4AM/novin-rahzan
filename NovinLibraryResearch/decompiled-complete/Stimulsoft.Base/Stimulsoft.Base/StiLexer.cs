using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Stimulsoft.Base;

public sealed class StiLexer
{
	private StringBuilder text;

	private List<int> positions = new List<int>();

	public StringBuilder Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			BaseText = value.ToString();
		}
	}

	internal string BaseText { get; set; }

	public int PositionInText { get; set; }

	public bool IsVB { get; set; }

	public void SavePosToken()
	{
		positions.Add(PositionInText);
	}

	public StiPosition GetPosition(int positionInText)
	{
		StiPosition result = new StiPosition(1, 1);
		for (int i = 0; i < positionInText; i++)
		{
			result.Column++;
			if (Text[i] == '\n')
			{
				result.Line++;
				result.Column = 1;
			}
		}
		return result;
	}

	public void Skip()
	{
		while (PositionInText < Text.Length && (char.IsWhiteSpace(Text[PositionInText]) || char.IsControl(Text[PositionInText])))
		{
			PositionInText++;
		}
	}

	public bool WaitLparen2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.LPar;
	}

	public bool WaitComma2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.Comma;
	}

	public bool WaitAssign2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.Assign;
	}

	public bool WaitRparen2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.RPar;
	}

	public bool WaitLbrace2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.LBrace;
	}

	public bool WaitSemicolon2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.SemiColon;
	}

	public bool WaitRbrace2()
	{
		StiToken token = GetToken();
		return token.Type == StiTokenType.RBrace;
	}

	public StiToken ScanNumber()
	{
		int positionInText = PositionInText;
		bool flag = false;
		while (PositionInText != Text.Length && char.IsDigit(Text[PositionInText]))
		{
			PositionInText++;
		}
		if (PositionInText != Text.Length && Text[PositionInText] == '.' && PositionInText + 1 != Text.Length && char.IsDigit(Text[PositionInText + 1]))
		{
			PositionInText++;
			while (PositionInText != Text.Length && char.IsDigit(Text[PositionInText]))
			{
				PositionInText++;
			}
			flag = true;
		}
		string text = BaseText.Substring(positionInText, PositionInText - positionInText);
		if (flag)
		{
			NumberFormatInfo provider = new NumberFormatInfo
			{
				CurrencyDecimalSeparator = "."
			};
			return new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, double.Parse(text, provider));
		}
		string text2 = text;
		try
		{
			if (text2.Length > 19)
			{
				return new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, text2);
			}
			ulong num = ulong.Parse(text2);
			return new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, num);
		}
		catch (Exception ex)
		{
			if (ex is OverflowException || ex is FormatException)
			{
				return new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, text2);
			}
			throw;
		}
	}

	public StiToken ScanIdent()
	{
		int positionInText = PositionInText;
		string text = string.Empty;
		while (PositionInText != Text.Length && (char.IsLetterOrDigit(Text[PositionInText]) || Text[PositionInText] == '_' || Text[PositionInText] == '№'))
		{
			text += Text[PositionInText++];
		}
		return new StiToken(StiTokenType.Ident, positionInText, PositionInText - positionInText, text);
	}

	public StiToken ScanString()
	{
		int positionInText = PositionInText;
		PositionInText++;
		StringBuilder stringBuilder = new StringBuilder();
		while (PositionInText != Text.Length && Text[PositionInText] != '"')
		{
			char c = Text[PositionInText++];
			stringBuilder.Append(c);
			if (!IsVB && c == '\\' && PositionInText != Text.Length)
			{
				stringBuilder.Append(Text[PositionInText++]);
			}
		}
		if (PositionInText == Text.Length)
		{
			new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, stringBuilder.ToString());
		}
		PositionInText++;
		return new StiToken(StiTokenType.Value, positionInText, PositionInText - positionInText, stringBuilder.ToString());
	}

	public StiToken ScanChar()
	{
		if (++PositionInText == Text.Length)
		{
			return new StiToken(StiTokenType.Value, PositionInText - 3, 3, ' ');
		}
		char charValue = Text[PositionInText++];
		if (PositionInText == Text.Length || Text[PositionInText] != '\'')
		{
			return new StiToken(StiTokenType.Value, PositionInText - 3, 3, charValue);
		}
		PositionInText++;
		return new StiToken(StiTokenType.Value, PositionInText - 3, 3, charValue);
	}

	public void UngetToken()
	{
		PositionInText = positions[positions.Count - 1];
		positions.RemoveAt(positions.Count - 1);
	}

	public StiToken GetToken()
	{
		Skip();
		if (Text.Length <= PositionInText)
		{
			return new StiToken(StiTokenType.EOF, PositionInText, 0);
		}
		if (char.IsLetter(Text[PositionInText]) || Text[PositionInText] == '_' || Text[PositionInText] == '№')
		{
			int positionInText = PositionInText;
			SavePosToken();
			StiToken stiToken = ScanIdent();
			string text = (string)stiToken.Data;
			if (!(text == "true"))
			{
				if (text == "false")
				{
					return new StiToken(StiTokenType.Value, positionInText, 5, false);
				}
				return stiToken;
			}
			return new StiToken(StiTokenType.Value, positionInText, 4, true);
		}
		if (char.IsDigit(Text[PositionInText]))
		{
			SavePosToken();
			return ScanNumber();
		}
		if (Text[PositionInText] == '"')
		{
			SavePosToken();
			return ScanString();
		}
		if (Text[PositionInText] == '\'')
		{
			SavePosToken();
			return ScanChar();
		}
		switch (Text[PositionInText])
		{
		case '€':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Euro, PositionInText - 1, 1);
		case '®':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Copyright, PositionInText - 1, 1);
		case '(':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.LPar, PositionInText - 1, 1);
		case ')':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.RPar, PositionInText - 1, 1);
		case '{':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.LBrace, PositionInText - 1, 1);
		case '}':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.RBrace, PositionInText - 1, 1);
		case ',':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Comma, PositionInText - 1, 1);
		case '.':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Dot, PositionInText - 1, 1);
		case ';':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.SemiColon, PositionInText - 1, 1);
		case ':':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Colon, PositionInText - 1, 1);
		case '!':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Minus, PositionInText - 1, 1);
		case '*':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Mult, PositionInText - 1, 1);
		case '^':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Not, PositionInText - 1, 1);
		case '/':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Div, PositionInText - 1, 1);
		case '\\':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Splash, PositionInText - 1, 1);
		case '%':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Percent, PositionInText - 1, 1);
		case '#':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Sharp, PositionInText - 1, 1);
		case '$':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Dollar, PositionInText - 1, 1);
		case '@':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Ampersand, PositionInText - 1, 1);
		case '[':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.LBracket, PositionInText - 1, 1);
		case ']':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.RBracket, PositionInText - 1, 1);
		case '?':
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Question, PositionInText - 1, 1);
		case '|':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '|')
			{
				PositionInText++;
				return new StiToken(StiTokenType.DoubleOr, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Or, PositionInText - 1, 1);
		case '&':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '&')
			{
				PositionInText++;
				return new StiToken(StiTokenType.DoubleAnd, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.And, PositionInText - 1, 1);
		case '+':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '+')
			{
				PositionInText++;
				return new StiToken(StiTokenType.DoublePlus, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Plus, PositionInText - 1, 1);
		case '-':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '-')
			{
				PositionInText++;
				return new StiToken(StiTokenType.DoubleMinus, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Minus, PositionInText - 1, 1);
		case '=':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '=')
			{
				PositionInText++;
				return new StiToken(StiTokenType.Equal, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Assign, PositionInText - 1, 1);
		case '<':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '=')
			{
				PositionInText++;
				return new StiToken(StiTokenType.LeftEqual, PositionInText - 2, 2);
			}
			if (PositionInText != Text.Length && Text[PositionInText] == '<')
			{
				PositionInText++;
				return new StiToken(StiTokenType.Shl, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Left, PositionInText - 1, 1);
		case '>':
			SavePosToken();
			PositionInText++;
			if (PositionInText != Text.Length && Text[PositionInText] == '=')
			{
				PositionInText++;
				return new StiToken(StiTokenType.RightEqual, PositionInText - 2, 2);
			}
			if (PositionInText != Text.Length && Text[PositionInText] == '>')
			{
				PositionInText++;
				return new StiToken(StiTokenType.Shr, PositionInText - 2, 2);
			}
			return new StiToken(StiTokenType.Right, PositionInText - 1, 1);
		default:
			SavePosToken();
			PositionInText++;
			return new StiToken(StiTokenType.Unknown, PositionInText - 1, 1);
		}
	}

	public void Reset()
	{
		positions.Clear();
		PositionInText = 0;
	}

	public static string ReplaceWithPrefix(string textValue, string prefix, string oldValue, string newValue)
	{
		StringBuilder stringBuilder = new StringBuilder(textValue);
		StiLexer stiLexer = new StiLexer(textValue);
		StiToken stiToken = stiLexer.GetToken();
		if (stiToken.Type == StiTokenType.EOF)
		{
			return textValue;
		}
		StiToken stiToken2 = null;
		do
		{
			stiToken2 = stiLexer.GetToken();
			if (stiToken2.Type == StiTokenType.Ident && stiToken.Type == StiTokenType.Ident && (string)stiToken.Data == prefix && (string)stiToken2.Data == oldValue)
			{
				stringBuilder = stringBuilder.Replace(oldValue, newValue, stiToken2.Index, stiToken2.Length);
				stiLexer.PositionInText += newValue.Length;
			}
			stiToken = stiToken2;
		}
		while (stiToken2.Type != StiTokenType.EOF);
		return stringBuilder.ToString();
	}

	public void ReplaceWithPrefix(string prefix, string oldValue, string newValue)
	{
		Reset();
		StiToken stiToken = GetToken();
		if (stiToken.Type == StiTokenType.EOF)
		{
			return;
		}
		StiToken stiToken2 = null;
		do
		{
			stiToken2 = GetToken();
			if (stiToken2.Type == StiTokenType.Ident && stiToken.Type == StiTokenType.Ident && (string)stiToken.Data == prefix && (string)stiToken2.Data == oldValue)
			{
				text = text.Replace(oldValue, newValue, stiToken2.Index, stiToken2.Length);
				PositionInText += newValue.Length;
			}
			stiToken = stiToken2;
		}
		while (stiToken2.Type != StiTokenType.EOF);
		BaseText = text.ToString();
	}

	public void ReplaceWithNotEqualPrefix(StiTokenType prefix, string oldValue, string newValue)
	{
		Reset();
		StiToken stiToken = GetToken();
		if (stiToken.Type == StiTokenType.EOF)
		{
			return;
		}
		StiToken stiToken2 = null;
		do
		{
			stiToken2 = GetToken();
			if (stiToken2.Type == StiTokenType.Ident && stiToken.Type != prefix && (string)stiToken2.Data == oldValue)
			{
				text = text.Replace(oldValue, newValue, stiToken2.Index, stiToken2.Length);
				PositionInText += newValue.Length;
			}
			stiToken = stiToken2;
		}
		while (stiToken2.Type != StiTokenType.EOF);
		BaseText = text.ToString();
	}

	public static bool IdentExists(string str, string name, bool caseSensitive)
	{
		StiLexer stiLexer = new StiLexer(str);
		while (true)
		{
			StiToken token = stiLexer.GetToken();
			if (token == null || token.Type == StiTokenType.EOF)
			{
				return false;
			}
			if (token.Type == StiTokenType.Ident && token.Data != null)
			{
				if (caseSensitive && token.Data.ToString() == name)
				{
					return true;
				}
				if (!caseSensitive && token.Data.ToString().ToLowerInvariant() == name.ToLowerInvariant())
				{
					break;
				}
			}
		}
		return true;
	}

	public static List<StiToken> GetAllTokens(string str)
	{
		List<StiToken> list = new List<StiToken>();
		StiLexer stiLexer = new StiLexer(str);
		while (true)
		{
			StiToken token = stiLexer.GetToken();
			if (token == null || token.Type == StiTokenType.EOF)
			{
				break;
			}
			list.Add(token);
		}
		return list;
	}

	public StiLexer(string textValue)
	{
		BaseText = textValue;
		text = new StringBuilder(textValue);
		PositionInText = 0;
	}
}
