namespace Stimulsoft.Editor;

public enum XmlLexerToken
{
	None,
	WhiteSpace,
	Body,
	OpenTag,
	CloseTag,
	OpenEndTag,
	CloseEndTag,
	Equal,
	TagName,
	ParamName,
	ParamValue,
	Comment,
	CDATA,
	CDATABody
}
