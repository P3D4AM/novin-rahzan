using System.Text;

namespace ExcelDataReader;

public class ExcelReaderConfiguration
{
	public Encoding FallbackEncoding { get; set; } = Encoding.GetEncoding(1252);


	public string Password { get; set; }

	public char[] AutodetectSeparators { get; set; } = new char[5] { ',', ';', '\t', '|', '#' };


	public bool LeaveOpen { get; set; }

	public int AnalyzeInitialCsvRows { get; set; }
}
