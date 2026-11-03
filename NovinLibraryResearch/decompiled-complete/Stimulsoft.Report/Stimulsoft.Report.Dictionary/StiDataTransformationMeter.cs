namespace Stimulsoft.Report.Dictionary;

public abstract class StiDataTransformationMeter
{
	public string Expression { get; set; }

	public string Label { get; set; }

	public override string ToString()
	{
		string name = GetType().Name;
		if (!string.IsNullOrWhiteSpace(Expression))
		{
			return name + "-\"" + Expression + "\"";
		}
		return name;
	}

	public virtual int GetUniqueCode()
	{
		int num = ((Expression != null) ? Expression.GetHashCode() : 0);
		num = (num * 397) ^ GetType().GetHashCode();
		return (num * 397) ^ ((Label != null) ? Label.GetHashCode() : 0);
	}

	protected StiDataTransformationMeter(string expression, string label)
	{
		Expression = expression;
		Label = label;
	}
}
