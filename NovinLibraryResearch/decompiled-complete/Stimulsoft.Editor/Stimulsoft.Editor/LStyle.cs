using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LStyle : BookMark
{
	private IRange range;

	private int priority;

	[XmlIgnore]
	public IRange Range
	{
		get
		{
			return range;
		}
		set
		{
			range = value;
		}
	}

	[XmlElement("Range")]
	public Range LineRange
	{
		get
		{
			return (Range)range;
		}
		set
		{
			range = value;
		}
	}

	public int Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public LStyle()
	{
	}

	public LStyle(int line, int ch, int index)
		: base(line, ch, index)
	{
	}

	public LStyle(int line, int ch, int index, int priority, IRange range)
		: this(line, ch, index)
	{
		this.priority = priority;
		this.range = range;
	}

	public virtual void Assign(LStyle source)
	{
		base.Assign(source);
		priority = source.Priority;
		range = source.Range;
	}
}
