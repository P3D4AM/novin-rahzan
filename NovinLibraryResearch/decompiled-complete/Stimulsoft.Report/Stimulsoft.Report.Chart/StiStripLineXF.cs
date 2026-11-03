using System;

namespace Stimulsoft.Report.Chart;

public class StiStripLineXF : ICloneable
{
	private object valueObject;

	private double valueObj;

	public object ValueObject
	{
		get
		{
			return valueObject;
		}
		set
		{
			valueObject = value;
		}
	}

	public double Value
	{
		get
		{
			return valueObj;
		}
		set
		{
			valueObj = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiStripLineXF(object valueObject, double value)
	{
		this.valueObject = valueObject;
		valueObj = value;
	}
}
