using System.ComponentModel;

namespace Stimulsoft.Base;

public class StiExcelOptions : StiFileDataOptions
{
	[DefaultValue(false)]
	public bool FirstRowIsHeader { get; set; }

	public StiExcelOptions(byte[] content, bool firstRowIsHeader = false)
		: base(content)
	{
		FirstRowIsHeader = firstRowIsHeader;
	}
}
