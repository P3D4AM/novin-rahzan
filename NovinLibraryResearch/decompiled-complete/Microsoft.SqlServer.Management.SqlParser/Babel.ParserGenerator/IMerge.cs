namespace Babel.ParserGenerator;

public interface IMerge<YYLTYPE>
{
	YYLTYPE Merge(YYLTYPE last);
}
