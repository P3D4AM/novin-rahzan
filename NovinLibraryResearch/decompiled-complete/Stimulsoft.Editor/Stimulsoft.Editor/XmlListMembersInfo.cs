using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[XmlRoot("ListMembersCollection")]
public class XmlListMembersInfo
{
	private ListMembers owner;

	private ListMember[] members;

	private bool showHints;

	private bool showQualifiers;

	private bool showResults;

	private bool showParams;

	private bool showDescriptions;

	public bool ShowDescriptions
	{
		get
		{
			if (owner == null)
			{
				return showDescriptions;
			}
			return owner.ShowDescriptions;
		}
		set
		{
			showDescriptions = value;
			if (owner != null)
			{
				owner.ShowDescriptions = value;
			}
		}
	}

	public bool ShowHints
	{
		get
		{
			if (owner == null)
			{
				return showHints;
			}
			return owner.ShowHints;
		}
		set
		{
			showHints = value;
			if (owner != null)
			{
				owner.ShowHints = value;
			}
		}
	}

	public bool ShowQualifiers
	{
		get
		{
			if (owner == null)
			{
				return showQualifiers;
			}
			return owner.ShowQualifiers;
		}
		set
		{
			showQualifiers = value;
			if (owner != null)
			{
				owner.ShowQualifiers = value;
			}
		}
	}

	public bool ShowResults
	{
		get
		{
			if (owner == null)
			{
				return showResults;
			}
			return owner.ShowResults;
		}
		set
		{
			showResults = value;
			if (owner != null)
			{
				owner.ShowResults = value;
			}
		}
	}

	public bool ShowParams
	{
		get
		{
			if (owner == null)
			{
				return showParams;
			}
			return owner.ShowParams;
		}
		set
		{
			showParams = value;
			if (owner != null)
			{
				owner.ShowParams = value;
			}
		}
	}

	[XmlArray]
	[XmlArrayItem("ListMember")]
	public ListMember[] ListMembers
	{
		get
		{
			if (owner != null)
			{
				ListMember[] array = new ListMember[owner.Count];
				owner.CopyTo(array);
				return array;
			}
			return members;
		}
		set
		{
			members = value;
			if (owner != null)
			{
				owner.Clear();
				foreach (ListMember value2 in value)
				{
					owner.Add(value2);
				}
			}
		}
	}

	public XmlListMembersInfo()
	{
	}

	public XmlListMembersInfo(ListMembers owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void FixupReferences(ListMembers owner)
	{
		this.owner = owner;
		ListMembers = members;
		ShowHints = showHints;
		ShowQualifiers = showQualifiers;
		ShowResults = showResults;
		ShowParams = showParams;
		ShowDescriptions = showDescriptions;
	}
}
