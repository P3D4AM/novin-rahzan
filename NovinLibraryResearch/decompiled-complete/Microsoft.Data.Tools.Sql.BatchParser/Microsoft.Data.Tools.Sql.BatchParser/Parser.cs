using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal sealed class Parser : IDisposable
{
	private readonly ICommandHandler _commandHandler;

	private Lexer _lexer;

	private List<Token> _tokenBuffer;

	private readonly IVariableResolver _variableResolver;

	public bool ThrowOnUnresolvedVariable { get; set; }

	private Token LookaheadToken => _lexer.CurrentToken;

	private LexerTokenType LookaheadTokenType => _lexer.CurrentTokenType;

	public Parser(ICommandHandler commandHandler, IVariableResolver variableResolver, TextReader reader, string name)
	{
		_commandHandler = commandHandler;
		_variableResolver = variableResolver;
		_lexer = new Lexer(reader, name);
		_tokenBuffer = new List<Token>();
	}

	public void Dispose()
	{
		if (_lexer != null)
		{
			_lexer.Dispose();
			_lexer = null;
		}
	}

	private bool AcceptWhitespaceOrComment()
	{
		bool result = false;
		while (LookaheadTokenType == LexerTokenType.Comment || LookaheadTokenType == LexerTokenType.Whitespace)
		{
			Accept();
			result = true;
		}
		return result;
	}

	private bool Accept(LexerTokenType lexerTokenType)
	{
		if (LookaheadTokenType == lexerTokenType)
		{
			Accept();
			return true;
		}
		return false;
	}

	private void AddTokenToStringBuffer()
	{
		Token lookaheadToken = LookaheadToken;
		if (lookaheadToken.TokenType != LexerTokenType.Comment)
		{
			CheckVariableReferences(lookaheadToken);
		}
		if (lookaheadToken.TokenType == LexerTokenType.NewLine && lookaheadToken.Text.Length == 0)
		{
			PositionStruct begin = lookaheadToken.Begin;
			PositionStruct end = new PositionStruct(begin.Line + 1, 1, begin.Offset, begin.Filename);
			_tokenBuffer.Add(new Token(LexerTokenType.NewLine, begin, end, "\r\n", begin.Filename));
		}
		else
		{
			_tokenBuffer.Add(lookaheadToken);
		}
	}

	private void CheckVariableReferences(Token token)
	{
		AddVariableReferences(token, 0, null);
	}

	private void AddVariableReferences(Token token, int offset, IList<VariableReference> variableRefs)
	{
		if (!_lexer.RecognizeSqlCmdSyntax)
		{
			return;
		}
		string text = token.Text;
		int i = 0;
		int num = -1;
		int num2 = 0;
		for (; i < text.Length; i++)
		{
			char c = text[i];
			if (num2 == 0 && c == '$')
			{
				num2 = 1;
				continue;
			}
			switch (num2)
			{
			case 1:
				switch (c)
				{
				case '(':
					num2 = 2;
					break;
				default:
					num2 = 0;
					break;
				case '$':
					break;
				}
				break;
			case 2:
				if (Lexer.IsStartIdentifierChar(c))
				{
					num = i;
					num2 = 3;
				}
				else
				{
					RaiseError(ErrorCode.InvalidVariableName, GetSubToken(token, i - 2, i + 1));
				}
				break;
			case 3:
				if (c == ')')
				{
					variableRefs?.Add(new VariableReference(offset + num - 2, i - num + 3, text.Substring(num, i - num)));
					num2 = 0;
				}
				else if (!Lexer.IsIdentifierChar(c))
				{
					RaiseError(ErrorCode.InvalidVariableName, GetSubToken(token, num - 2, i + 1));
				}
				break;
			}
		}
		if (num2 == 2 || num2 == 3)
		{
			RaiseError(ErrorCode.InvalidVariableName, GetSubToken(token, num - 2, i));
		}
	}

	private static Token GetSubToken(Token token, int startOffset, int endOffset, LexerTokenType? newTokenType = null)
	{
		LexerTokenType tokenType = (newTokenType.HasValue ? newTokenType.Value : token.TokenType);
		string text = token.Text.Substring(startOffset, endOffset - startOffset);
		string filename = token.Begin.Filename;
		LineInfo.CalculateLineColumnForOffset(token, startOffset, out var line, out var column);
		PositionStruct begin = new PositionStruct(line, column, token.Begin.Offset + startOffset, filename);
		LineInfo.CalculateLineColumnForOffset(token, endOffset, out var line2, out var column2);
		PositionStruct end = new PositionStruct(line2, column2, token.Begin.Offset + endOffset, filename);
		return new Token(tokenType, begin, end, text, filename);
	}

	private LexerTokenType Accept()
	{
		_lexer.ConsumeToken();
		return LookaheadTokenType;
	}

	private void ExecuteBatch(int repeatCount)
	{
		if (_commandHandler.Go(new TextBlock(this, _tokenBuffer), repeatCount) == BatchParserAction.Abort)
		{
			RaiseError(ErrorCode.Aborted);
		}
		_tokenBuffer = new List<Token>();
	}

	private bool Expect(LexerTokenType lexerTokenType)
	{
		if (LookaheadTokenType == lexerTokenType)
		{
			return true;
		}
		RaiseError(ErrorCode.TokenExpected);
		return false;
	}

	private bool ExpectAndAccept(LexerTokenType lexerTokenType)
	{
		if (Accept(lexerTokenType))
		{
			return true;
		}
		RaiseError(ErrorCode.TokenExpected);
		return false;
	}

	internal void Parse()
	{
		Accept();
		ParseLines();
	}

	private void ParseGo()
	{
		int result = 1;
		AcceptWhitespaceOrComment();
		if (LookaheadTokenType == LexerTokenType.Text)
		{
			string text = ResolveVariables(LookaheadToken, 0, null);
			for (int i = 0; i < text.Length; i++)
			{
				if (!char.IsDigit(text[i]))
				{
					RaiseError(ErrorCode.InvalidNumber);
				}
			}
			if (!int.TryParse(text, out result))
			{
				RaiseError(ErrorCode.InvalidNumber);
			}
			Accept(LexerTokenType.Text);
			AcceptWhitespaceOrComment();
		}
		if (LookaheadTokenType != LexerTokenType.Eof)
		{
			ExpectAndAccept(LexerTokenType.NewLine);
		}
		ExecuteBatch(result);
	}

	private void ParseInclude()
	{
		Accept(LexerTokenType.Whitespace);
		Expect(LexerTokenType.Text);
		Token lookaheadToken = LookaheadToken;
		if (lookaheadToken.Text.StartsWith("--", StringComparison.Ordinal))
		{
			RaiseError(ErrorCode.TokenExpected);
		}
		_lexer.SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeDoubleQuotedString | TextRuleFlags.RecognizeLineComment);
		Accept();
		AcceptWhitespaceOrComment();
		if (LookaheadTokenType != LexerTokenType.Eof)
		{
			Expect(LexerTokenType.NewLine);
		}
		IEnumerable<Token> tokens = ((lookaheadToken.Text.IndexOf('"') == -1) ? new Token[1] { lookaheadToken } : SplitQuotedTextToken(lookaheadToken));
		if (_commandHandler.Include(new TextBlock(this, tokens), out var stream, out var newFilename) == BatchParserAction.Abort)
		{
			RaiseError(ErrorCode.Aborted);
		}
		if (stream != null)
		{
			PushInput(stream, newFilename);
		}
	}

	private IEnumerable<Token> SplitQuotedTextToken(Token token)
	{
		string text = token.Text;
		if (text.Length <= 1)
		{
			return new Token[1] { token };
		}
		IList<Token> list = new List<Token>();
		int num = 0;
		for (int num2 = text.IndexOf('"'); num2 != -1; num2 = text.IndexOf('"', num))
		{
			if (num2 != num)
			{
				list.Add(GetSubToken(token, num, num2));
				num = num2;
			}
			int num3 = text.IndexOf('"', num2 + 1);
			if (num3 == -1)
			{
				break;
			}
			if (num3 + 1 < text.Length && text[num3 + 1] == '"')
			{
				list.Add(GetSubToken(token, num2 + 1, num3 + 1, LexerTokenType.TextVerbatim));
			}
			else
			{
				list.Add(GetSubToken(token, num2 + 1, num3, LexerTokenType.TextVerbatim));
			}
			num = num3 + 1;
		}
		if (num != text.Length)
		{
			list.Add(GetSubToken(token, num, text.Length));
		}
		return list;
	}

	internal void PushInput(TextReader reader, string filename)
	{
		_lexer.PushInput(reader, filename);
	}

	private void ParseLines()
	{
		while (true)
		{
			LexerTokenType lookaheadTokenType = LookaheadTokenType;
			switch (lookaheadTokenType)
			{
			case LexerTokenType.OnError:
			{
				RemoveLastWhitespaceToken();
				Token lookaheadToken2 = LookaheadToken;
				Accept();
				ParseOnErrorCommand(lookaheadToken2);
				break;
			}
			case LexerTokenType.Eof:
				if (_tokenBuffer.Count > 0)
				{
					ExecuteBatch(1);
				}
				return;
			case LexerTokenType.Go:
				RemoveLastWhitespaceToken();
				Accept();
				ParseGo();
				break;
			case LexerTokenType.Include:
				RemoveLastWhitespaceToken();
				Accept();
				ParseInclude();
				break;
			case LexerTokenType.Text:
			case LexerTokenType.Whitespace:
			case LexerTokenType.NewLine:
			case LexerTokenType.Comment:
				AddTokenToStringBuffer();
				Accept();
				break;
			case LexerTokenType.Setvar:
			{
				Token lookaheadToken = LookaheadToken;
				RemoveLastWhitespaceToken();
				Accept();
				ParseSetvar(lookaheadToken);
				break;
			}
			case LexerTokenType.Reset:
			case LexerTokenType.Ed:
			case LexerTokenType.Execute:
			case LexerTokenType.Quit:
			case LexerTokenType.Exit:
			case LexerTokenType.Serverlist:
			case LexerTokenType.List:
			case LexerTokenType.ErrorCommand:
			case LexerTokenType.Out:
			case LexerTokenType.Perftrace:
			case LexerTokenType.Connect:
			case LexerTokenType.Help:
			case LexerTokenType.Xml:
			case LexerTokenType.ListVar:
				RaiseError(ErrorCode.UnsupportedCommand, string.Format(CultureInfo.CurrentCulture, SR.EE_ExecutionError_CommandNotSupported, lookaheadTokenType));
				break;
			default:
				RaiseError(ErrorCode.UnrecognizedToken);
				break;
			}
		}
	}

	private void RemoveLastWhitespaceToken()
	{
		if (_tokenBuffer.Count > 0 && _tokenBuffer[_tokenBuffer.Count - 1].TokenType == LexerTokenType.Whitespace)
		{
			_tokenBuffer.RemoveAt(_tokenBuffer.Count - 1);
		}
	}

	private void ParseOnErrorCommand(Token onErrorToken)
	{
		ExpectAndAccept(LexerTokenType.Whitespace);
		Expect(LexerTokenType.Text);
		string text = ResolveVariables(LookaheadToken, 0, null);
		OnErrorAction action;
		if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
		{
			action = OnErrorAction.Exit;
		}
		else
		{
			if (!text.Equals("ignore", StringComparison.OrdinalIgnoreCase))
			{
				RaiseError(ErrorCode.UnrecognizedToken);
				return;
			}
			action = OnErrorAction.Ignore;
		}
		Accept(LexerTokenType.Text);
		AcceptWhitespaceOrComment();
		if (LookaheadTokenType != LexerTokenType.Eof)
		{
			Expect(LexerTokenType.NewLine);
		}
		if (_commandHandler.OnError(onErrorToken, action) == BatchParserAction.Abort)
		{
			RaiseError(ErrorCode.Aborted);
		}
	}

	private void ParseSetvar(Token setvarToken)
	{
		string value = null;
		Accept(LexerTokenType.Whitespace);
		Expect(LexerTokenType.Text);
		string text = LookaheadToken.Text;
		Accept();
		Accept(LexerTokenType.Whitespace);
		switch (LookaheadTokenType)
		{
		case LexerTokenType.Text:
			value = UnquoteVariableValue(LookaheadToken.Text);
			_lexer.SetTextState(TextRuleFlags.ReportWhitespace | TextRuleFlags.RecognizeDoubleQuotedString | TextRuleFlags.RecognizeLineComment);
			Accept();
			AcceptWhitespaceOrComment();
			if (LookaheadTokenType != LexerTokenType.NewLine && LookaheadTokenType != LexerTokenType.Eof)
			{
				RaiseError(ErrorCode.UnrecognizedToken);
			}
			if (LookaheadTokenType != LexerTokenType.Eof)
			{
				Accept();
			}
			break;
		case LexerTokenType.NewLine:
		case LexerTokenType.Eof:
			Accept();
			break;
		default:
			RaiseError(ErrorCode.UnrecognizedToken);
			break;
		}
		_variableResolver.SetVariable(setvarToken.Begin, text, value);
	}

	internal void RaiseError(ErrorCode errorCode, string message = null)
	{
		RaiseError(errorCode, LookaheadToken, message);
	}

	internal static void RaiseError(ErrorCode errorCode, Token token, string message = null)
	{
		if (message == null)
		{
			message = string.Format(CultureInfo.CurrentCulture, SR.BatchParser_IncorrectSyntax, token.Text);
		}
		throw new BatchParserException(errorCode, token, message);
	}

	internal string ResolveVariables(Token inputToken, int offset, List<VariableReference> variableRefs)
	{
		List<VariableReference> list = new List<VariableReference>();
		AddVariableReferences(inputToken, offset, list);
		string text = inputToken.Text;
		if (list.Count == 0)
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		PositionStruct? positionStruct = null;
		foreach (VariableReference item in list)
		{
			int line;
			int column;
			if (positionStruct.HasValue)
			{
				LineInfo.CalculateLineColumnForOffset(inputToken.Text, item.Start - offset, positionStruct.Value.Offset, positionStruct.Value.Line, positionStruct.Value.Column, out line, out column);
			}
			else
			{
				LineInfo.CalculateLineColumnForOffset(inputToken, item.Start - offset, out line, out column);
			}
			positionStruct = new PositionStruct(line, column, item.Start - offset, inputToken.Filename);
			string variable = _variableResolver.GetVariable(positionStruct.Value, item.VariableName);
			if (variable == null)
			{
				if (ThrowOnUnresolvedVariable)
				{
					RaiseError(ErrorCode.VariableNotDefined, inputToken, string.Format(CultureInfo.CurrentCulture, SR.BatchParser_VariableNotDefined, item.VariableName));
				}
			}
			else
			{
				item.VariableValue = variable;
				stringBuilder.Append(inputToken.Text, num, item.Start - offset - num);
				stringBuilder.Append(variable);
				num = item.Start - offset + item.Length;
			}
		}
		if (num != text.Length)
		{
			stringBuilder.Append(text, num, text.Length - num);
		}
		variableRefs?.AddRange(list);
		return stringBuilder.ToString();
	}

	private string UnquoteVariableValue(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Length >= 2 && s[0] == '"' && s[s.Length - 1] == s[0])
			{
				for (int i = 1; i < s.Length - 1; i++)
				{
					if (s[i] == '"')
					{
						if (i + 1 >= s.Length - 1 || s[i + 1] != '"')
						{
							RaiseError(ErrorCode.TokenExpected);
						}
						i++;
					}
				}
				return s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
			}
			if (s[0] == '"' || s[s.Length - 1] == '"')
			{
				RaiseError(ErrorCode.TokenExpected);
			}
			else
			{
				if (s.Length >= 2 && s[0] == '-' && s[1] == '-')
				{
					return null;
				}
				if (s.IndexOf('"') >= 0)
				{
					RaiseError(ErrorCode.TokenExpected);
				}
			}
		}
		return s;
	}

	public void SetRecognizeSqlCmdSyntax(bool recognizeSqlCmdSyntax)
	{
		_lexer.RecognizeSqlCmdSyntax = recognizeSqlCmdSyntax;
	}

	public static TextReader GetBufferedTextReaderForFile(string filename)
	{
		return new StringReader(File.ReadAllText(filename));
	}

	internal void SetBatchDelimiter(string batchSeparator)
	{
	}

	public static string TokenTypeToCommandString(LexerTokenType lexerTokenType)
	{
		return lexerTokenType switch
		{
			LexerTokenType.Connect => "Connect", 
			LexerTokenType.Ed => "Ed", 
			LexerTokenType.ErrorCommand => "Error", 
			LexerTokenType.Execute => "!!", 
			LexerTokenType.Exit => "Exit", 
			LexerTokenType.Help => "Help", 
			LexerTokenType.List => "List", 
			LexerTokenType.ListVar => "ListVar", 
			LexerTokenType.OnError => "On Error", 
			LexerTokenType.Out => "Out", 
			LexerTokenType.Perftrace => "PerfTrace", 
			LexerTokenType.Quit => "Quit", 
			LexerTokenType.Reset => "Reset", 
			LexerTokenType.Serverlist => "ServerList", 
			LexerTokenType.Xml => "Xml", 
			_ => lexerTokenType.ToString(), 
		};
	}
}
