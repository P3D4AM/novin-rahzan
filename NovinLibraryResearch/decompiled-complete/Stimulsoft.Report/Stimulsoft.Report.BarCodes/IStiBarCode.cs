using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.BarCodes;

public interface IStiBarCode : IStiBackColor, IStiForeColor
{
	bool AutoScale { get; set; }

	bool ShowLabelText { get; set; }

	StiBarCodeTypeService BarCodeType { get; set; }

	string GetBarCodeString();
}
