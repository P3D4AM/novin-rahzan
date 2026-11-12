namespace Babel.ParserGenerator;

public abstract class AScanner<YYSTYPE, YYLTYPE> where YYSTYPE : struct where YYLTYPE : IMerge<YYLTYPE>
{
	public YYSTYPE yylval;

	public YYLTYPE yylloc;

	public abstract int yylex();

	public abstract void yyerror(string message);
}
