using System.Collections;

namespace Stimulsoft.Editor;

public class XmlOutliningInfo
{
	private Outlining owner;

	private bool allowOutlining;

	private OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;

	private OutlineRange[] ranges = new OutlineRange[0];

	private string outlineColor = EditConsts.DefaultOutlineForeColor.Name;

	public bool AllowOutlining
	{
		get
		{
			if (owner == null)
			{
				return allowOutlining;
			}
			return owner.AllowOutlining;
		}
		set
		{
			allowOutlining = value;
			if (owner != null)
			{
				owner.AllowOutlining = value;
			}
		}
	}

	public OutlineOptions OutlineOptions
	{
		get
		{
			if (owner == null)
			{
				return outlineOptions;
			}
			return owner.OutlineOptions;
		}
		set
		{
			outlineOptions = value;
			if (owner != null)
			{
				owner.OutlineOptions = value;
			}
		}
	}

	public string OutlineColor
	{
		get
		{
			if (owner == null)
			{
				return outlineColor;
			}
			return XmlHelper.SerializeColor(owner.OutlineColor);
		}
		set
		{
			outlineColor = value;
			if (owner != null)
			{
				owner.OutlineColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public OutlineRange[] Ranges
	{
		get
		{
			if (owner != null)
			{
				ArrayList arrayList = new ArrayList();
				owner.GetOutlineRanges(arrayList);
				OutlineRange[] array = new OutlineRange[arrayList.Count];
				for (int i = 0; i < arrayList.Count; i++)
				{
					OutlineRange outlineRange = (OutlineRange)arrayList[i];
					array[i] = new OutlineRange(outlineRange.StartPoint, outlineRange.EndPoint, outlineRange.Level, outlineRange.Text, outlineRange.Visible);
				}
				return array;
			}
			return ranges;
		}
		set
		{
			ranges = value;
			if (owner != null)
			{
				owner.SetOutlineRanges(value);
			}
		}
	}

	public XmlOutliningInfo()
	{
	}

	public XmlOutliningInfo(Outlining owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			allowOutlining = AllowOutlining;
			outlineOptions = OutlineOptions;
			outlineColor = OutlineColor;
			ranges = Ranges;
		}
	}

	public virtual void FixupReferences(Outlining owner)
	{
		this.owner = owner;
		AllowOutlining = allowOutlining;
		OutlineOptions = outlineOptions;
		OutlineColor = outlineColor;
		Ranges = ranges;
	}
}
