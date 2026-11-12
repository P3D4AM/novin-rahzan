namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxElement
{
	public XlsxElementType Type { get; }

	public XlsxElement(XlsxElementType type)
	{
		Type = type;
	}
}
