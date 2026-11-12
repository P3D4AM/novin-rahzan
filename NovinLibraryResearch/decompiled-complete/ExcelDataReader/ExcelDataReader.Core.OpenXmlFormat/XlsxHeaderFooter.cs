namespace ExcelDataReader.Core.OpenXmlFormat;

internal sealed class XlsxHeaderFooter : XlsxElement
{
	public HeaderFooter Value { get; }

	public XlsxHeaderFooter(HeaderFooter headerFooter)
		: base(XlsxElementType.HeaderFooter)
	{
		Value = headerFooter;
	}
}
