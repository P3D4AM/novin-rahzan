using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LexState : ILexState
{
	private string name;

	private string desc;

	private bool caseSensitive;

	private ILexSyntaxBlocks lexSyntaxBlocks;

	private string expression;

	private Regex regex;

	private Hashtable blockTable;

	private ILexScheme scheme;

	protected internal Hashtable BlockTable => blockTable;

	[Description("Gets or sets name of the \"LexState\".")]
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

	[Description("Gets or sets description of the \"ILexState\".")]
	public string Desc
	{
		get
		{
			return desc;
		}
		set
		{
			desc = value;
		}
	}

	[Description("Gets or sets a value indicating whether the analizer should perform case sensitive parsing for this \"LexState\".")]
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
				DoFixupBlocks();
			}
		}
	}

	[XmlIgnore]
	[Description("Gets or sets a collection of lexical syntax blocks.")]
	public ILexSyntaxBlocks LexSyntaxBlocks
	{
		get
		{
			return lexSyntaxBlocks;
		}
		set
		{
			lexSyntaxBlocks.Clear();
			foreach (LexSyntaxBlock item in value)
			{
				lexSyntaxBlocks.Add(item);
			}
			FixupBlocks(scheme);
		}
	}

	[XmlArray]
	[XmlArrayItem("SyntaxBlock")]
	[Description("Gets or sets \"LexSyntaxBlocks\" property in the form of LexSyntaxBlock array.")]
	public LexSyntaxBlock[] SyntaxBlocks
	{
		get
		{
			LexSyntaxBlock[] array = new LexSyntaxBlock[lexSyntaxBlocks.Count];
			((LexSyntaxBlocks)lexSyntaxBlocks).CopyTo(array);
			return array;
		}
		set
		{
			lexSyntaxBlocks.Clear();
			foreach (LexSyntaxBlock value2 in value)
			{
				lexSyntaxBlocks.Add(value2);
			}
			FixupBlocks(scheme);
		}
	}

	[XmlIgnore]
	[Description("Represents a collective regular expression pattern for the \"LexState\".")]
	public string Expression => expression;

	[XmlIgnore]
	[Description("Represents a regular expression that contains \"Expression\" as a pattern.")]
	public Regex Regex => regex;

	[Description("Represents index of the \"LexState\" within the lexical state collection.")]
	public int Index
	{
		get
		{
			if (scheme == null)
			{
				return -1;
			}
			return scheme.LexStates.IndexOf(this);
		}
	}

	[XmlIgnore]
	[Description("Gets or sets \"ILexScheme\" that owns this \"LexState\".")]
	public ILexScheme Scheme
	{
		get
		{
			return scheme;
		}
		set
		{
			scheme = value;
		}
	}

	public LexState()
	{
		lexSyntaxBlocks = new LexSyntaxBlocks(this, scheme);
		regex = new Regex(string.Empty);
		blockTable = new Hashtable();
	}

	public LexState(ILexScheme scheme)
		: this()
	{
		this.scheme = scheme;
	}

	private void DoFixupBlocks()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (LexSyntaxBlock lexSyntaxBlock in lexSyntaxBlocks)
		{
			lexSyntaxBlock.CaseSensitive = caseSensitive;
			string text = lexSyntaxBlock.Expression;
			if (text != null && text != string.Empty)
			{
				stringBuilder.Append(string.Format("(?<{0}>{1})", "_" + lexSyntaxBlock.Index, text) + "|");
			}
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		AssignExpression(stringBuilder.ToString());
	}

	private void InitBlockHash()
	{
		blockTable.Clear();
		int[] groupNumbers = regex.GetGroupNumbers();
		int[] array = groupNumbers;
		foreach (int num in array)
		{
			string text = regex.GroupNameFromNumber(num);
			if (text != null && text.Length > 0 && text[0] == '_')
			{
				int index = int.Parse(text.Substring(1));
				blockTable.Add(num, lexSyntaxBlocks[index]);
			}
		}
	}

	protected internal void AssignExpression(string expr)
	{
		if (expression != expr)
		{
			expression = expr;
			RegexOptions regexOptions = RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
			if (!caseSensitive)
			{
				regexOptions |= RegexOptions.IgnoreCase;
			}
			regex = new Regex(expr, regexOptions);
			InitBlockHash();
		}
	}

	protected internal void FixupBlocks(ILexScheme scheme)
	{
		this.scheme = scheme;
		foreach (LexSyntaxBlock lexSyntaxBlock in lexSyntaxBlocks)
		{
			lexSyntaxBlock.FixupBlock(scheme, lexSyntaxBlocks, CaseSensitive);
		}
		DoFixupBlocks();
	}

	public ILexSyntaxBlock FindSyntaxBlock(string name)
	{
		foreach (ILexSyntaxBlock lexSyntaxBlock in lexSyntaxBlocks)
		{
			if (lexSyntaxBlock.Name == name)
			{
				return lexSyntaxBlock;
			}
		}
		return null;
	}

	public virtual void ResetCaseSensitive()
	{
		CaseSensitive = false;
	}
}
