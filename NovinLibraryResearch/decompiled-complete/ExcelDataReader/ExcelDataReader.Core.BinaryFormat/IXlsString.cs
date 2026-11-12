using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal interface IXlsString
{
	string GetValue(Encoding encoding);
}
