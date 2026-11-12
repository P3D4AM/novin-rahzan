namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxRow : XlsxElement
{
	public Row Row { get; set; }

	public XlsxRow()
		: base(XlsxElementType.Row)
	{
	}
}
