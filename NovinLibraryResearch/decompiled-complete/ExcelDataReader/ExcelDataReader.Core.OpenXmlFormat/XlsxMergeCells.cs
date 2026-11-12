using System.Collections.Generic;

namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxMergeCells : XlsxElement
{
	public List<CellRange> Value { get; set; }

	public XlsxMergeCells()
		: base(XlsxElementType.MergeCells)
	{
	}
}
