using System.Text;
using Babel.ParserGenerator;

namespace Babel;

public class TokenInfo
{
	public TokenType Type;

	public int EndIndex;

	public int StartIndex;

	public int Token;

	public TokenTriggers Trigger;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Token: ").Append(ShiftReduceParser.TerminalToString(Token)).AppendLine();
		stringBuilder.Append("Type: ").Append(Type).AppendLine();
		stringBuilder.Append("Index: (").Append(StartIndex).Append(", ")
			.Append(EndIndex)
			.Append(")")
			.AppendLine();
		stringBuilder.Append("Triggers: ").Append(Trigger).AppendLine();
		return stringBuilder.ToString();
	}
}
