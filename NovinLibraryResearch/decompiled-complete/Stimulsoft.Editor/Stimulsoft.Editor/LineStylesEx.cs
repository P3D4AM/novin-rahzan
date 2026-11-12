using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[Serializable]
public class LineStylesEx : ArrayList, ILineStylesEx, IList, ICollection, IEnumerable
{
	private SyntaxEdit owner;

	public new LineStyle this[int index]
	{
		get
		{
			return (LineStyle)base[index];
		}
		set
		{
			((LineStyle)base[index]).Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlLineStylesInfo(this);
		}
		set
		{
			((XmlLineStylesInfo)value).FixupReferences(this);
		}
	}

	public LineStylesEx(SyntaxEdit owner)
	{
		this.owner = owner;
	}

	protected internal ILineStyle GetStyle(int index)
	{
		if (index < 0 || index >= Count)
		{
			return null;
		}
		return this[index];
	}

	public void Assign(ILineStylesEx source)
	{
		Clear();
		foreach (LineStyle item in source)
		{
			AddLineStyle(item.Name, item.ForeColor, item.BackColor, item.PenColor, item.ImageIndex, item.Options);
		}
	}

	public ILineStyle GetLineStyle(int index)
	{
		return this[index];
	}

	public int IndexOfName(string name)
	{
		for (int i = 0; i < Count; i++)
		{
			if (((ILineStyle)this[i]).Name == name)
			{
				return i;
			}
		}
		return -1;
	}

	public int AddLineStyle()
	{
		return Add(new LineStyle(owner));
	}

	public int AddLineStyle(string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options)
	{
		return Add(new LineStyle(owner, name, foreColor, backColor, penColor, imageIndex, options));
	}

	public int Add(LineStyle value)
	{
		return base.Add(value);
	}

	public void AddRange(LineStyle[] values)
	{
		base.AddRange(values);
	}
}
