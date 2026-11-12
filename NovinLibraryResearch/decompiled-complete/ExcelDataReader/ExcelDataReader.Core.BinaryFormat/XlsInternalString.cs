using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsInternalString : IXlsString
{
	private readonly string stringValue;

	public XlsInternalString(string value)
	{
		stringValue = value;
	}

	public string GetValue(Encoding encoding)
	{
		return stringValue;
	}
}
