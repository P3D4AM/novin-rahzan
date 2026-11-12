using System.Collections.Generic;

namespace Microsoft.Data.Tools.Sql.BatchParser;

internal class LineInfo
{
	private IEnumerable<Token> _tokens;

	private IEnumerable<VariableReference> _variableRefs;

	public LineInfo(IEnumerable<Token> tokens, IEnumerable<VariableReference> variableRefs)
	{
		_tokens = tokens;
		_variableRefs = variableRefs;
	}

	public PositionStruct GetStreamPositionForOffset(int offset)
	{
		if (_variableRefs != null)
		{
			offset = CalculateVarsUnresolvedOffset(offset);
		}
		int num = 0;
		Token token = null;
		foreach (Token token2 in _tokens)
		{
			token = token2;
			if (num + token2.Text.Length > offset)
			{
				CalculateLineColumnForOffset(token2, offset - num, out var line, out var column);
				return new PositionStruct(line, column, token2.Begin.Offset + (offset - num), token2.Filename);
			}
			num += token2.Text.Length;
		}
		if (token != null)
		{
			return new PositionStruct(token.End.Line, token.End.Column, token.End.Offset, token.Filename);
		}
		return new PositionStruct(1, 1, 0, string.Empty);
	}

	internal static void CalculateLineColumnForOffset(Token token, int offset, out int line, out int column)
	{
		CalculateLineColumnForOffset(token.Text, offset, 0, token.Begin.Line, token.Begin.Column, out line, out column);
	}

	internal static void CalculateLineColumnForOffset(string text, int offset, int offsetDelta, int lineDelta, int columnDelta, out int line, out int column)
	{
		line = lineDelta;
		column = columnDelta;
		int num = offsetDelta;
		while (num < offset)
		{
			bool flag = false;
			if (text[num] == '\r')
			{
				flag = true;
			}
			else if (text[num] == '\n')
			{
				line++;
				column = 0;
			}
			num++;
			if (flag && num < text.Length && text[num] != '\n')
			{
				line++;
				column = 0;
			}
			column++;
		}
	}

	private int CalculateVarsUnresolvedOffset(int offset)
	{
		int num = 0;
		foreach (VariableReference variableRef in _variableRefs)
		{
			if (variableRef.Start >= offset)
			{
				break;
			}
			if (variableRef.VariableValue != null && offset < variableRef.Start + variableRef.VariableValue.Length)
			{
				offset = variableRef.Start;
				break;
			}
			if (variableRef.VariableValue != null)
			{
				num += variableRef.Length - variableRef.VariableValue.Length;
			}
		}
		return offset + num;
	}
}
