using System.Collections.Generic;

namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxCols : XlsxElement
{
	public List<Col> Value { get; set; }

	public XlsxCols()
		: base(XlsxElementType.Cols)
	{
	}
}
