using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiPivotTableElementCondition : ICloneable, IStiJsonReportObject
{
	string KeyValueMeter { get; set; }

	StiFilterDataType DataType { get; set; }

	StiFilterCondition Condition { get; set; }

	string Value { get; set; }

	Color TextColor { get; set; }

	Color BackColor { get; set; }

	Font Font { get; set; }

	StiConditionPermissions Permissions { get; set; }

	byte[] CustomIcon { get; set; }

	StiFontIcons Icon { get; set; }

	StiIconAlignment IconAlignment { get; set; }

	Size? IconSize { get; }

	Color IconColor { get; set; }

	int GetUniqueCode();

	byte[] GetIcon(bool isExporting);
}
