using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlSyntaxStringsInfo
{
	private SyntaxStrings owner;

	private int[] tabStops = new int[1] { EditConsts.DefaultTabStop };

	private string[] lines = new string[0];

	private bool useSpaces;

	private bool removeTrailingSpaces;

	private string delimiters = EditConsts.DefaultDelimiters;

	private string lineTerminator = "\r\n";

	public int[] TabStops
	{
		get
		{
			if (owner == null)
			{
				return tabStops;
			}
			return owner.TabStops;
		}
		set
		{
			tabStops = value;
			if (owner != null)
			{
				owner.TabStops = value;
			}
		}
	}

	public bool UseSpaces
	{
		get
		{
			if (owner == null)
			{
				return useSpaces;
			}
			return owner.UseSpaces;
		}
		set
		{
			useSpaces = value;
			if (owner != null)
			{
				owner.UseSpaces = value;
			}
		}
	}

	public string Delimiters
	{
		get
		{
			if (owner == null)
			{
				return delimiters;
			}
			return owner.DelimiterString;
		}
		set
		{
			delimiters = value;
			if (owner != null)
			{
				owner.DelimiterString = value;
			}
		}
	}

	public string[] Lines
	{
		get
		{
			if (owner != null)
			{
				string[] array = new string[owner.Count];
				for (int i = 0; i < owner.Count; i++)
				{
					array[i] = owner[i];
				}
				return array;
			}
			return lines;
		}
		set
		{
			lines = value;
			if (owner == null)
			{
				return;
			}
			owner.BeginUpdate();
			try
			{
				owner.Clear();
				foreach (string value2 in value)
				{
					owner.Add(value2);
				}
			}
			finally
			{
				owner.EndUpdate();
			}
		}
	}

	[XmlIgnore]
	public string LineTerminator
	{
		get
		{
			if (owner == null)
			{
				return lineTerminator;
			}
			return owner.LineTerminator;
		}
		set
		{
			lineTerminator = value;
			if (owner != null)
			{
				owner.LineTerminator = value;
			}
		}
	}

	public bool RemoveTrailingSpaces
	{
		get
		{
			if (owner == null)
			{
				return removeTrailingSpaces;
			}
			return owner.RemoveTrailingSpaces;
		}
		set
		{
			removeTrailingSpaces = value;
			if (owner != null)
			{
				owner.RemoveTrailingSpaces = value;
			}
		}
	}

	public XmlSyntaxStringsInfo()
	{
	}

	public XmlSyntaxStringsInfo(SyntaxStrings owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			tabStops = TabStops;
			useSpaces = UseSpaces;
			delimiters = Delimiters;
			removeTrailingSpaces = RemoveTrailingSpaces;
			lineTerminator = LineTerminator;
			lines = Lines;
		}
	}

	public virtual void FixupReferences(SyntaxStrings owner)
	{
		this.owner = owner;
		TabStops = tabStops;
		UseSpaces = useSpaces;
		Delimiters = delimiters;
		RemoveTrailingSpaces = removeTrailingSpaces;
		LineTerminator = lineTerminator;
		Lines = lines;
	}
}
