namespace Babel;

internal struct TokenDefinition
{
	public TokenType TokenType;

	public TokenTriggers TokenTriggers;

	public TokenDefinition(TokenType type, TokenTriggers triggers)
	{
		TokenType = type;
		TokenTriggers = triggers;
	}
}
