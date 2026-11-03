using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Components;

public abstract class StiUnifiedExpression : StiExpression
{
	private string propertyName = string.Empty;

	private StiComponent parent;

	public override string Value
	{
		get
		{
			if (parent == null)
			{
				return base.Value;
			}
			return ((StiService)parent).Properties.Get((object)propertyName, (object)string.Empty) as string;
		}
		set
		{
			if (parent != null)
			{
				((StiService)parent).Properties.Set((object)propertyName, (object)value, (object)string.Empty);
			}
			else
			{
				base.Value = value;
			}
		}
	}

	public void Set(StiComponent parent, string propertyName, string value)
	{
		this.parent = parent;
		this.propertyName = propertyName;
		Value = value;
	}

	public StiUnifiedExpression()
		: this(string.Empty)
	{
	}

	public StiUnifiedExpression(string value)
		: base(value)
	{
	}

	public StiUnifiedExpression(StiComponent parent, string propertyName)
	{
		this.parent = parent;
		this.propertyName = propertyName;
	}
}
