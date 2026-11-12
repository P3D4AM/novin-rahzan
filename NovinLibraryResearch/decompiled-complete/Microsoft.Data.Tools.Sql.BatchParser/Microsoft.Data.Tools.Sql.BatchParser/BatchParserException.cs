using System;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal sealed class BatchParserException : Exception
{
	private const string ErrorCodeName = "ErrorCode";

	private const string BeginName = "Begin";

	private const string EndName = "End";

	private const string TextName = "Text";

	private const string TokenTypeName = "TokenType";

	private ErrorCode _errorCode;

	private PositionStruct _begin;

	private PositionStruct _end;

	private string _text;

	private LexerTokenType _tokenType;

	public ErrorCode ErrorCode => _errorCode;

	public PositionStruct Begin => _begin;

	public PositionStruct End => _end;

	public string Text => _text;

	public LexerTokenType TokenType => _tokenType;

	public BatchParserException(ErrorCode errorCode, Token token, string message)
		: base(message)
	{
		_errorCode = errorCode;
		_begin = token.Begin;
		_end = token.End;
		_text = token.Text;
		_tokenType = token.TokenType;
	}
}
