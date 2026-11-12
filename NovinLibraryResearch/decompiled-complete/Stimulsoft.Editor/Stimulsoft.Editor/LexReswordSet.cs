using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LexReswordSet : ILexReswordSet
{
	private ILexStyle reswordStyle;

	private ArrayList reswords;

	private Hashtable reswordsHash;

	private int reswordStyleIndex = -1;

	private ILexScheme scheme;

	private ILexReswordSets reswordSets;

	private bool caseSensitive;

	private string name = string.Empty;

	[XmlIgnore]
	[Description("Gets or sets a value indicating whether \"LexReswordSet\" is case-sensitive or not.")]
	public bool CaseSensitive
	{
		get
		{
			return caseSensitive;
		}
		set
		{
			if (caseSensitive != value)
			{
				caseSensitive = value;
				UpdateReswords();
			}
		}
	}

	[XmlIgnore]
	[Description("Gets or sets an \"ILexStyle\" of resword collection.")]
	public ILexStyle ReswordStyle
	{
		get
		{
			return reswordStyle;
		}
		set
		{
			reswordStyle = value;
		}
	}

	[Description("Gets or set name for the \"LexReswordSet\".")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[Description("Represents index of the \"LexReswordSet\" within the reswords collection.")]
	public int Index
	{
		get
		{
			if (reswordSets == null)
			{
				return -1;
			}
			return reswordSets.IndexOf(this);
		}
	}

	[XmlElement("ResWordStyle")]
	[Description("Gets or sets index of the ReswordStyle within the lexical style collection.")]
	public int ReswordStyleIndex
	{
		get
		{
			if (reswordStyle == null)
			{
				return reswordStyleIndex;
			}
			return reswordStyle.Index;
		}
		set
		{
			reswordStyleIndex = value;
			if (scheme != null && value >= 0)
			{
				reswordStyle = ((LexScheme)scheme).GetLexStyle(value);
			}
		}
	}

	[XmlArray("ResWords")]
	[XmlArrayItem("word")]
	[Description("Gets or sets a collection of keywords for the \"LexReswordSet\".")]
	public string[] Reswords
	{
		get
		{
			string[] array = new string[reswords.Count];
			reswords.CopyTo(array);
			return array;
		}
		set
		{
			Clear();
			foreach (string resword in value)
			{
				AddResword(resword);
			}
		}
	}

	public LexReswordSet()
	{
		reswords = new ArrayList();
		reswordsHash = new Hashtable();
	}

	public LexReswordSet(ILexScheme scheme, ILexReswordSets reswordSets, bool caseSensitive)
		: this()
	{
		this.scheme = scheme;
		this.reswordSets = reswordSets;
		this.caseSensitive = caseSensitive;
	}

	protected internal void FixupReswordSet(ILexScheme scheme, ILexReswordSets reswordSets, bool caseSensitive)
	{
		this.scheme = scheme;
		this.reswordSets = reswordSets;
		CaseSensitive = caseSensitive;
		ReswordStyleIndex = reswordStyleIndex;
	}

	protected internal void UpdateReswords()
	{
		reswordsHash.Clear();
		foreach (string resword in reswords)
		{
			string text2 = (caseSensitive ? resword : resword.ToLower(CultureInfo.InvariantCulture));
			if (!reswordsHash.Contains(text2))
			{
				reswordsHash.Add(text2, text2);
			}
		}
	}

	public int AddResword(string resword)
	{
		reswords.Add(resword);
		string text = (caseSensitive ? resword : resword.ToLower(CultureInfo.InvariantCulture));
		if (!reswordsHash.Contains(text))
		{
			reswordsHash.Add(text, text);
		}
		return reswords.Count - 1;
	}

	public void Clear()
	{
		reswords.Clear();
		reswordsHash.Clear();
	}

	public bool FindResword(string resword)
	{
		return reswordsHash.Contains(caseSensitive ? resword : resword.ToLower(CultureInfo.InvariantCulture));
	}
}
