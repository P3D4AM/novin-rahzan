using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiTableAlignmentHelper
{
	internal static StiHorAlignment GetHorAlign(IStiAppDataCell cell)
	{
		Type type = StiMeterHelper.ToDataType(cell);
		if (type.IsNumericType())
		{
			return StiHorAlignment.Right;
		}
		if (type.IsDateType())
		{
			return StiHorAlignment.Center;
		}
		return StiHorAlignment.Left;
	}
}
