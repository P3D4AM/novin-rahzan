using System;
using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
internal class AgreedDeductionDetComparer : IEqualityComparer<AgreedDeductionDet>
{
	public bool Equals(AgreedDeductionDet x, AgreedDeductionDet y)
	{
		if (x != null && y != null && x.MonthYear == y.MonthYear)
		{
			return true;
		}
		return false;
	}

	public int GetHashCode(AgreedDeductionDet obj)
	{
		return obj.MonthYear.GetHashCode();
	}
}
