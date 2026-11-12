using System;

namespace Stimulsoft.Base;

public class StiLexerException : Exception
{
	public StiLexerError LexerError { get; set; }

	public StiLexerException(StiLexerError error)
	{
		LexerError = error;
	}
}
