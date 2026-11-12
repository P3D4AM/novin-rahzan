namespace ExcelDataReader.Core.OpenXmlFormat;

internal class XlsxDimension : XlsxElement
{
	public int FirstRow { get; set; }

	public int LastRow { get; set; }

	public int FirstCol { get; set; }

	public int LastCol { get; set; }

	public bool IsRange { get; set; }

	public XlsxDimension(string value)
		: base(XlsxElementType.Dimension)
	{
		ParseDimensions(value);
	}

	public XlsxDimension(int rows, int cols)
		: base(XlsxElementType.Dimension)
	{
		FirstRow = 1;
		LastRow = rows;
		FirstCol = 1;
		LastCol = cols;
		IsRange = true;
	}

	private void ParseDimensions(string value)
	{
		string[] array = value.Split(new char[1] { ':' });
		ReferenceHelper.ParseReference(array[0], out var column, out var row);
		FirstCol = column;
		FirstRow = row;
		if (array.Length == 1)
		{
			LastCol = FirstCol;
			LastRow = FirstRow;
			return;
		}
		ReferenceHelper.ParseReference(array[1], out column, out row);
		LastCol = column;
		LastRow = row;
		IsRange = true;
	}
}
