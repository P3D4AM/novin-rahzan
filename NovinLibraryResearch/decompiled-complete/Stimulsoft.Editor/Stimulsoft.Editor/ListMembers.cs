using System;
using System.Collections;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class ListMembers : CodeCompletionProvider, IListMembers, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	private bool showHints = true;

	private bool showQualifiers = true;

	private bool showResults = true;

	private bool showParams;

	public override int ColumnCount => 4;

	[XmlElement("ShowQualifiers")]
	public bool ShowQualifiers
	{
		get
		{
			return showQualifiers;
		}
		set
		{
			showQualifiers = value;
		}
	}

	[XmlElement("ShowResults")]
	public bool ShowResults
	{
		get
		{
			return showResults;
		}
		set
		{
			showResults = value;
		}
	}

	[XmlElement("ShowParams")]
	public bool ShowParams
	{
		get
		{
			return showParams;
		}
		set
		{
			showParams = value;
		}
	}

	[XmlElement("ShowHints")]
	public bool ShowHints
	{
		get
		{
			return showHints;
		}
		set
		{
			showHints = value;
		}
	}

	public new IListMember this[int index] => GetListMember(index);

	public override object XmlInfo
	{
		get
		{
			return new XmlListMembersInfo(this);
		}
		set
		{
			((XmlListMembersInfo)value).FixupReferences(this);
		}
	}

	public ListMembers()
	{
		base.UseHtmlFormatting = true;
	}

	protected override Type GetXmlType()
	{
		return typeof(XmlListMembersInfo);
	}

	public override string GetName(int index)
	{
		return GetListMember(index).Name;
	}

	public override string GetText(int index)
	{
		return GetListMember(index).Name;
	}

	public override int GetImageIndex(int index)
	{
		return GetListMember(index).ImageIndex;
	}

	public override string GetDescription(int index)
	{
		if (showHints)
		{
			return GetListMember(index).Description;
		}
		return string.Empty;
	}

	public IListMember GetListMember(int index)
	{
		return (IListMember)base[index];
	}

	public virtual IListMember CreateListMember()
	{
		return new ListMember(this);
	}

	public IListMember AddMember()
	{
		IListMember listMember = CreateListMember();
		Add(listMember);
		return listMember;
	}

	public virtual IListMember InsertMember(int index)
	{
		IListMember listMember = CreateListMember();
		Insert(index, listMember);
		return listMember;
	}

	public override string GetColumnText(int index, int column)
	{
		return column switch
		{
			0 => GetListMember(index).Qualifier, 
			1 => GetListMember(index).DataType, 
			2 => GetListMember(index).DisplayText, 
			3 => GetListMember(index).ParamText, 
			4 => GetListMember(index).Description, 
			_ => string.Empty, 
		};
	}

	public override bool ColumnVisible(int column)
	{
		return column switch
		{
			0 => showQualifiers, 
			1 => showResults, 
			2 => true, 
			3 => showParams, 
			_ => false, 
		};
	}

	public virtual void ResetShowHints()
	{
		ShowHints = true;
	}

	public virtual void ResetShowParams()
	{
		ShowParams = false;
	}

	public virtual void ResetShowQualifiers()
	{
		ShowQualifiers = true;
	}

	public virtual void ResetShowResults()
	{
		ShowResults = true;
	}
}
