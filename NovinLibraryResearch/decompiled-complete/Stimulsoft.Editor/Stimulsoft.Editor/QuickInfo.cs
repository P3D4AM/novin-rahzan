using System;
using System.Collections;

namespace Stimulsoft.Editor;

public class QuickInfo : CodeCompletionProvider, IQuickInfo, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	public string Text
	{
		get
		{
			if (Count <= 0)
			{
				return string.Empty;
			}
			return (string)this[0];
		}
		set
		{
			if (Count > 0)
			{
				this[0] = value;
			}
			else
			{
				Add(value);
			}
		}
	}

	public override object XmlInfo
	{
		get
		{
			return new XmlQuickInfo(this);
		}
		set
		{
			((XmlQuickInfo)value).FixupReferences(this);
		}
	}

	public QuickInfo()
	{
		base.UseHtmlFormatting = true;
		Add(string.Empty);
	}

	protected override Type GetXmlType()
	{
		return typeof(XmlQuickInfo);
	}

	public override string GetName(int index)
	{
		return Text;
	}

	public override string GetText(int index)
	{
		return Text;
	}
}
