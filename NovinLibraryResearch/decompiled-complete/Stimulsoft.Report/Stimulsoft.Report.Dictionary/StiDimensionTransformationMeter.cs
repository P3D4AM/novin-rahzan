using System.ComponentModel;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Dictionary;

public class StiDimensionTransformationMeter : StiDataTransformationMeter, IStiDimensionMeter, IStiMeter
{
	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	public override int GetUniqueCode()
	{
		int num = ((base.Expression != null) ? base.Expression.GetHashCode() : 0);
		num = (num * 397) ^ GetType().GetHashCode();
		num = (num * 397) ^ ((base.Label != null) ? base.Label.GetHashCode() : 0);
		return (num * 397) ^ ((Key != null) ? Key.GetHashCode() : 0);
	}

	public StiDimensionTransformationMeter(string expression, string label, string key)
		: base(expression, label)
	{
		Key = key;
	}
}
