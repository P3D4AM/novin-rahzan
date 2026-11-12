using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal class TextBlock
{
	private Parser _parser;

	private IEnumerable<Token> _tokens;

	public TextBlock(Parser parser, Token token)
		: this(parser, new Token[1] { token })
	{
	}

	public TextBlock(Parser parser, IEnumerable<Token> tokens)
	{
		_parser = parser;
		_tokens = tokens;
	}

	public void GetText(bool resolveVariables, out string text, out LineInfo lineInfo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<VariableReference> variableRefs = null;
		if (!resolveVariables)
		{
			foreach (Token token in _tokens)
			{
				stringBuilder.Append(token.Text);
			}
		}
		else
		{
			variableRefs = new List<VariableReference>();
			foreach (Token token2 in _tokens)
			{
				if (token2.TokenType == LexerTokenType.Text)
				{
					stringBuilder.Append(_parser.ResolveVariables(token2, stringBuilder.Length, variableRefs));
				}
				else
				{
					stringBuilder.Append(token2.Text);
				}
			}
		}
		lineInfo = new LineInfo(_tokens, variableRefs);
		text = stringBuilder.ToString();
	}
}
