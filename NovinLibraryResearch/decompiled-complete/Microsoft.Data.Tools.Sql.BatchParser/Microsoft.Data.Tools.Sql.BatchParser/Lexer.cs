using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal sealed class Lexer : IDisposable
{
	private LexerInput _currentInput;

	private bool _popInputAtNextConsume;

	private Token _currentToken;

	private ErrorCode _errorCode;

	private readonly Stack<LexerInput> _inputStack = new Stack<LexerInput>();

	private Func<bool> _lexerFunc;

	private TextRuleFlags _textRuleFlags;

	private PositionStruct _tokenBeginPosition;

	public Token CurrentToken => _currentToken;

	public LexerTokenType CurrentTokenType => _currentToken.TokenType;

	public bool RecognizeSqlCmdSyntax { get; set; }

	public Lexer(TextReader input, string name)
	{
		_currentInput = new LexerInput(input, name);
		_currentToken = null;
		RecognizeSqlCmdSyntax = true;
		SetState(RuleLine);
	}

	public void ConsumeToken()
	{
		if (_currentInput == null)
		{
			return;
		}
		_tokenBeginPosition = new PositionStruct(_currentInput.CurrentLine, _currentInput.CurrentColumn, _currentInput.CurrentOffset, _currentInput.Filename);
		bool flag;
		do
		{
			if (_popInputAtNextConsume)
			{
				PopAndCloseInput();
				_popInputAtNextConsume = false;
			}
			do
			{
				flag = _lexerFunc();
			}
			while (!flag);
			if (CurrentTokenType == LexerTokenType.Eof)
			{
				_popInputAtNextConsume = true;
				if (_inputStack.Count > 0)
				{
					_currentToken = new Token(LexerTokenType.NewLine, _tokenBeginPosition, _tokenBeginPosition, string.Empty, _tokenBeginPosition.Filename);
				}
			}
		}
		while (!flag);
	}

	public void Dispose()
	{
		while (_inputStack.Count > 0)
		{
			PopAndCloseInput();
		}
	}

	public void PopAndCloseInput()
	{
		if (_currentInput != null)
		{
			_currentInput.Dispose();
			_currentInput = null;
		}
		if (_inputStack.Count > 0)
		{
			_currentInput = _inputStack.Pop();
			SetState(RuleLine);
		}
	}

	private static string GetCircularReferenceErrorMessage(string filename)
	{
		return string.Format(CultureInfo.CurrentCulture, SR.BatchParser_CircularReference, filename);
	}

	public void PushInput(TextReader reader, string name)
	{
		if (name.Equals(_currentInput.Filename, StringComparison.OrdinalIgnoreCase))
		{
			RaiseError(ErrorCode.CircularReference, GetCircularReferenceErrorMessage(name));
		}
		foreach (LexerInput item in _inputStack)
		{
			if (name.Equals(item.Filename, StringComparison.OrdinalIgnoreCase))
			{
				RaiseError(ErrorCode.CircularReference, GetCircularReferenceErrorMessage(name));
			}
		}
		_inputStack.Push(_currentInput);
		_currentInput = new LexerInput(reader, name);
		SetState(RuleLine);
		_popInputAtNextConsume = false;
		ConsumeToken();
	}

	private void AcceptBlockComment()
	{
		int num = 0;
		Consume();
		while (true)
		{
			Consume();
			char? c = Lookahead();
			if (!c.HasValue)
			{
				RaiseError(ErrorCode.CommentNotTerminated, string.Format(CultureInfo.CurrentCulture, SR.BatchParser_CommentNotTerminated));
			}
			if (c.Value == '*')
			{
				char? c2 = Lookahead(1);
				if (c2.HasValue && c2.Value == '/')
				{
					Consume();
					if (num == 0)
					{
						break;
					}
					num--;
				}
			}
			else if (c.Value == '/')
			{
				char? c3 = Lookahead(1);
				if (c3.HasValue && c3.Value == '*')
				{
					Consume();
					num++;
				}
			}
		}
		Consume();
		SetToken(LexerTokenType.Comment);
	}

	private void AcceptLineComment()
	{
		Consume();
		char? c;
		do
		{
			Consume();
			c = Lookahead();
		}
		while (c.HasValue && !IsNewLineChar(c.Value));
		SetToken(LexerTokenType.Comment);
	}

	private void AcceptIdentifier()
	{
		char? c;
		do
		{
			Consume();
			c = Lookahead();
		}
		while (c.HasValue && IsIdentifierChar(c.Value));
		SetToken(LexerTokenType.Text);
	}

	private void AcceptNewLine()
	{
		char? c = Lookahead();
		if (c == '\r')
		{
			Consume();
			c = Lookahead();
		}
		if (c == '\n')
		{
			Consume();
		}
		SetToken(LexerTokenType.NewLine);
	}

	private void AcceptEscapableQuotedText(char closingChar)
	{
		while (true)
		{
			Consume();
			char? c = Lookahead();
			if (!c.HasValue)
			{
				RaiseError(ErrorCode.StringNotTerminated, SR.BatchParser_StringNotTerminated);
				break;
			}
			if (c == closingChar)
			{
				Consume();
				char? c2 = Lookahead();
				if (!c2.HasValue || c2 != closingChar)
				{
					break;
				}
			}
		}
	}

	private void AcceptQuotedText(char closingChar)
	{
		char? c;
		do
		{
			Consume();
			c = Lookahead();
		}
		while (c.HasValue && c != closingChar);
		if (!c.HasValue)
		{
			RaiseError(ErrorCode.StringNotTerminated, SR.BatchParser_StringNotTerminated);
		}
		else
		{
			Consume();
		}
	}

	private void AcceptWhitespace()
	{
		char? c;
		do
		{
			Consume();
			c = Lookahead();
		}
		while (c.HasValue && IsWhitespaceChar(c.Value));
		SetToken(LexerTokenType.Whitespace);
	}

	private void Consume()
	{
		_currentInput.Consume();
	}

	private void ChangeStateToBatchCommand(Token token)
	{
		switch (token.TokenType)
		{
		case LexerTokenType.Setvar:
			SetState(RuleSetvar);
			break;
		case LexerTokenType.Go:
			SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeLineComment);
			break;
		case LexerTokenType.Include:
			SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeDoubleQuotedString);
			break;
		case LexerTokenType.OnError:
			SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeLineComment);
			break;
		default:
			SetTextState(TextRuleFlags.ReportWhitespace);
			break;
		}
	}

	private static bool IsDigit(char ch)
	{
		if (ch >= '0')
		{
			return ch <= '9';
		}
		return false;
	}

	private static bool IsLetter(char ch)
	{
		if (ch < 'a' || ch > 'z')
		{
			if (ch >= 'A')
			{
				return ch <= 'Z';
			}
			return false;
		}
		return true;
	}

	private bool IsNewLineChar(char ch)
	{
		if (ch != '\r')
		{
			return ch == '\n';
		}
		return true;
	}

	private bool IsWhitespaceChar(char ch)
	{
		if (ch != ' ')
		{
			return ch == '\t';
		}
		return true;
	}

	private char? Lookahead()
	{
		return _currentInput.Lookahead();
	}

	private char? Lookahead(int lookahead)
	{
		return _currentInput.Lookahead(lookahead);
	}

	private bool RuleError()
	{
		Parser.RaiseError(_errorCode, CurrentToken);
		return true;
	}

	private bool RuleLine()
	{
		char? c = Lookahead();
		if (!c.HasValue)
		{
			SetToken(LexerTokenType.Eof);
			return true;
		}
		switch (c.Value)
		{
		case '\t':
		case ' ':
			AcceptWhitespace();
			return true;
		case '\n':
		case '\r':
			AcceptNewLine();
			return true;
		case ':':
			if (RecognizeSqlCmdSyntax && TryAcceptBatchCommandAndSetToken())
			{
				ChangeStateToBatchCommand(_currentToken);
				return true;
			}
			break;
		case 'G':
		case 'g':
			if (TryAccept("go", wordBoundary: true))
			{
				SetToken(LexerTokenType.Go);
				ChangeStateToBatchCommand(_currentToken);
				return true;
			}
			break;
		}
		SetTextState(TextRuleFlags.RecognizeDoubleQuotedString | TextRuleFlags.RecognizeSingleQuotedString | TextRuleFlags.RecognizeLineComment | TextRuleFlags.RecognizeBlockComment | TextRuleFlags.RecognizeBrace);
		return false;
	}

	private bool RuleSetvar()
	{
		char? c = Lookahead();
		if (!c.HasValue)
		{
			SetToken(LexerTokenType.Eof);
			return true;
		}
		switch (c.Value)
		{
		case '\n':
		case '\r':
			AcceptNewLine();
			SetState(RuleLine);
			return true;
		case '\t':
		case ' ':
			AcceptWhitespace();
			return true;
		default:
			if (IsStartIdentifierChar(c.Value))
			{
				AcceptIdentifier();
				SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeDoubleQuotedString);
				return true;
			}
			do
			{
				Consume();
				c = Lookahead();
			}
			while (c.HasValue && !IsWhitespaceChar(c.Value) && !IsNewLineChar(c.Value));
			RaiseError(ErrorCode.UnrecognizedToken);
			return true;
		}
	}

	private bool RuleText()
	{
		char? c = Lookahead();
		if (!c.HasValue)
		{
			SetToken(LexerTokenType.Eof);
			return true;
		}
		if (c.HasValue)
		{
			if (IsNewLineChar(c.Value))
			{
				AcceptNewLine();
				SetState(RuleLine);
				return true;
			}
			if (_textRuleFlags.HasFlag(TextRuleFlags.ReportWhitespace) && IsWhitespaceChar(c.Value))
			{
				AcceptWhitespace();
				return true;
			}
			if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeBlockComment) || _textRuleFlags.HasFlag(TextRuleFlags.RecognizeLineComment))
			{
				char? c2 = Lookahead(1);
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeBlockComment) && c == '/' && c2 == '*')
				{
					AcceptBlockComment();
					return true;
				}
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeLineComment) && c == '-' && c2 == '-')
				{
					AcceptLineComment();
					return true;
				}
			}
		}
		while (c.HasValue)
		{
			bool flag = false;
			switch (c.Value)
			{
			case '\t':
			case ' ':
				if (_textRuleFlags.HasFlag(TextRuleFlags.ReportWhitespace))
				{
					SetToken(LexerTokenType.Text);
					return true;
				}
				break;
			case '\n':
			case '\r':
				SetToken(LexerTokenType.Text);
				return true;
			case '"':
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeDoubleQuotedString))
				{
					AcceptQuotedText('"');
					flag = true;
				}
				break;
			case '\'':
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeSingleQuotedString))
				{
					AcceptEscapableQuotedText('\'');
					flag = true;
				}
				break;
			case '[':
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeBrace))
				{
					AcceptEscapableQuotedText(']');
					flag = true;
				}
				break;
			case '-':
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeLineComment))
				{
					char? c4 = Lookahead(1);
					if (c.HasValue && c4 == '-')
					{
						SetToken(LexerTokenType.Text);
						return true;
					}
				}
				break;
			case '/':
				if (_textRuleFlags.HasFlag(TextRuleFlags.RecognizeBlockComment))
				{
					char? c3 = Lookahead(1);
					if (c.HasValue && c3 == '*')
					{
						SetToken(LexerTokenType.Text);
						return true;
					}
				}
				break;
			}
			if (!flag)
			{
				Consume();
			}
			c = Lookahead();
		}
		SetToken(LexerTokenType.Text);
		return true;
	}

	private void RaiseError(ErrorCode code, string message = null)
	{
		SetState(RuleError);
		SetToken(LexerTokenType.Error);
		_errorCode = code;
		Parser.RaiseError(_errorCode, CurrentToken, message);
	}

	private void SetState(Func<bool> lexerFunc)
	{
		_lexerFunc = lexerFunc;
	}

	internal void SetTextState(TextRuleFlags textRuleFlags)
	{
		_textRuleFlags = textRuleFlags;
		SetState(RuleText);
	}

	private void SetToken(LexerTokenType lexerTokenType)
	{
		string text = _currentInput.FlushBufferedText();
		_currentToken = new Token(lexerTokenType, _tokenBeginPosition, new PositionStruct(_currentInput.CurrentLine, _currentInput.CurrentColumn, _currentInput.CurrentOffset, _currentInput.Filename), text, _currentInput.Filename);
	}

	private bool TryAccept(string text, bool wordBoundary)
	{
		int num = 0;
		do
		{
			char? c = Lookahead(num);
			if (!c.HasValue || char.ToLowerInvariant(c.Value) != text[num])
			{
				return false;
			}
			num++;
		}
		while (num < text.Length);
		if (wordBoundary)
		{
			char? c2 = Lookahead(text.Length);
			if (c2.HasValue && !IsWhitespaceChar(c2.Value) && !IsNewLineChar(c2.Value) && c2 != '$' && c2 != '/' && c2 != '-' && c2 != '\'' && c2 != '"' && c2 != '(' && c2 != '[' && c2 != '!')
			{
				return false;
			}
		}
		for (num = 0; num < text.Length; num++)
		{
			Consume();
		}
		return true;
	}

	private bool TryAcceptBatchCommandAndSetToken()
	{
		Consume();
		if (TryAccept("reset", wordBoundary: true))
		{
			SetToken(LexerTokenType.Reset);
			return true;
		}
		if (TryAccept("ed", wordBoundary: true))
		{
			SetToken(LexerTokenType.Ed);
			return true;
		}
		if (TryAccept("!!", wordBoundary: true))
		{
			SetToken(LexerTokenType.Execute);
			return true;
		}
		if (TryAccept("quit", wordBoundary: true))
		{
			SetToken(LexerTokenType.Quit);
			return true;
		}
		if (TryAccept("exit", wordBoundary: true))
		{
			SetToken(LexerTokenType.Exit);
			return true;
		}
		if (TryAccept("r", wordBoundary: true))
		{
			SetToken(LexerTokenType.Include);
			return true;
		}
		if (TryAccept("serverlist", wordBoundary: true))
		{
			SetToken(LexerTokenType.Serverlist);
			return true;
		}
		if (TryAccept("setvar", wordBoundary: true))
		{
			SetToken(LexerTokenType.Setvar);
			return true;
		}
		if (TryAccept("list", wordBoundary: true))
		{
			SetToken(LexerTokenType.List);
			return true;
		}
		if (TryAccept("error", wordBoundary: true))
		{
			SetToken(LexerTokenType.ErrorCommand);
			return true;
		}
		if (TryAccept("out", wordBoundary: true))
		{
			SetToken(LexerTokenType.Out);
			return true;
		}
		if (TryAccept("perftrace", wordBoundary: true))
		{
			SetToken(LexerTokenType.Perftrace);
			return true;
		}
		if (TryAccept("connect", wordBoundary: true))
		{
			SetToken(LexerTokenType.Connect);
			return true;
		}
		if (TryAccept("on error", wordBoundary: true))
		{
			SetToken(LexerTokenType.OnError);
			return true;
		}
		if (TryAccept("help", wordBoundary: true))
		{
			SetToken(LexerTokenType.Help);
			return true;
		}
		if (TryAccept("xml", wordBoundary: true))
		{
			SetToken(LexerTokenType.Xml);
			return true;
		}
		if (TryAccept("listvar", wordBoundary: true))
		{
			SetToken(LexerTokenType.ListVar);
			return true;
		}
		return false;
	}

	internal static bool IsIdentifierChar(char ch)
	{
		if (!IsLetter(ch) && !IsDigit(ch) && ch != '_')
		{
			return ch == '-';
		}
		return true;
	}

	internal static bool IsStartIdentifierChar(char ch)
	{
		if (!IsLetter(ch))
		{
			return ch == '_';
		}
		return true;
	}
}
