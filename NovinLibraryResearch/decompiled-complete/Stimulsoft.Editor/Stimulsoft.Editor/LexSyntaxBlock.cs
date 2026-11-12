using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LexSyntaxBlock : ILexSyntaxBlock
{
	private string name;

	private string desc;

	private ILexStyle style;

	private ILexState leaveState;

	private ArrayList expressions;

	private ILexReswordSets lexReswordSets;

	private ILexScheme scheme;

	private ILexSyntaxBlocks blocks;

	private bool caseSensitive;

	private int lexStyleIndex = -1;

	private int leaveStateIndex = -1;

	[Description("Gets or sets name of the \"LexSyntaxBlock\".")]
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

	[Description("Gets or sets description of the \"LexSyntaxBlock\".")]
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

	[XmlIgnore]
	[Description("Gets or sets a value indicating whether \"LexSyntaxBlock\" is case-sensitive or not.")]
	public bool CaseSensitive
	{
		get
		{
			return caseSensitive;
		}
		set
		{
			if (caseSensitive == value)
			{
				return;
			}
			caseSensitive = value;
			foreach (LexReswordSet lexReswordSet in lexReswordSets)
			{
				lexReswordSet.CaseSensitive = value;
			}
		}
	}

	[XmlIgnore]
	[Description("Gets or sets an \"ILexStyle\" object, applicable for text that matches \"LexSyntaxBlock\" expression.")]
	public ILexStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
		}
	}

	[XmlIgnore]
	[Description("Gets or sets \"ILexState\" object that specifies lexical resulting state after lexical analyzer locates text, that matches to the \"LexSyntaxBlock.Expression\".")]
	public ILexState LeaveState
	{
		get
		{
			return leaveState;
		}
		set
		{
			leaveState = value;
		}
	}

	[XmlElement("LexStyle")]
	[Description("Represents index of the \"Style\" within the style collection.")]
	public int LexStyleIndex
	{
		get
		{
			if (style == null)
			{
				return lexStyleIndex;
			}
			return style.Index;
		}
		set
		{
			lexStyleIndex = value;
			if (scheme != null && value >= 0)
			{
				style = ((LexScheme)scheme).GetLexStyle(value);
			}
		}
	}

	[XmlElement("LeaveState")]
	[Description("Represents index of the \"LeaveState\" within the state collection.")]
	public int LeaveStateIndex
	{
		get
		{
			if (leaveState == null)
			{
				return leaveStateIndex;
			}
			return leaveState.Index;
		}
		set
		{
			leaveStateIndex = value;
			if (scheme != null && value >= 0)
			{
				leaveState = ((LexScheme)scheme).GetLexState(value);
			}
		}
	}

	[XmlIgnore]
	[Description("Gets or sets collection of the \"ILexReswordSet\" object containing reserwed words.")]
	public ILexReswordSets LexReswordSets
	{
		get
		{
			return lexReswordSets;
		}
		set
		{
			lexReswordSets.Clear();
			foreach (ILexReswordSet item in value)
			{
				lexReswordSets.Add(item);
			}
		}
	}

	[XmlArray("ResWordSets")]
	[XmlArrayItem("ResWordSet")]
	[Description("Gets or sets the \"LexReswordSets\" property in the form of the LexReswordSet array.")]
	public LexReswordSet[] ReswordSets
	{
		get
		{
			LexReswordSet[] array = new LexReswordSet[lexReswordSets.Count];
			((LexReswordSets)lexReswordSets).CopyTo(array);
			return array;
		}
		set
		{
			lexReswordSets.Clear();
			foreach (ILexReswordSet value2 in value)
			{
				lexReswordSets.Add(value2);
			}
		}
	}

	[XmlArray]
	[Description("Gets or sets collection of the expressions used by \"Lexer\" to parse the text.")]
	public string[] Expressions
	{
		get
		{
			string[] array = new string[expressions.Count];
			expressions.CopyTo(array);
			return array;
		}
		set
		{
			expressions.Clear();
			foreach (string value2 in value)
			{
				expressions.Add(value2);
			}
		}
	}

	[Description("Represents a collective regular expression pattern for the \"LexSyntaxBlock.Expressions\".")]
	public string Expression
	{
		get
		{
			switch (expressions.Count)
			{
			case 0:
				return string.Empty;
			case 1:
				return (string)expressions[0];
			default:
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string expression in expressions)
				{
					stringBuilder.Append($"({expression})" + "|");
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				return stringBuilder.ToString();
			}
			}
		}
	}

	[Description("Represents index of the \"LexSyntaxBlock\" within the lexical syntax block collection.")]
	public int Index
	{
		get
		{
			if (blocks == null)
			{
				return -1;
			}
			return blocks.IndexOf(this);
		}
	}

	[XmlIgnore]
	[Description("Gets or sets \"ILexScheme\" that owns this \"LexSyntaxBlock\".")]
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

	public LexSyntaxBlock()
	{
		expressions = new ArrayList();
		lexReswordSets = new LexReswordSets();
	}

	public LexSyntaxBlock(ILexScheme scheme, ILexSyntaxBlocks blocks)
		: this()
	{
		this.scheme = scheme;
		this.blocks = blocks;
	}

	protected internal void FixupBlock(ILexScheme scheme, ILexSyntaxBlocks blocks, bool caseSensitive)
	{
		this.scheme = scheme;
		this.blocks = blocks;
		LexStyleIndex = lexStyleIndex;
		LeaveStateIndex = leaveStateIndex;
		foreach (LexReswordSet lexReswordSet in lexReswordSets)
		{
			lexReswordSet.FixupReswordSet(scheme, lexReswordSets, caseSensitive);
		}
	}

	public int FindResword(string resword)
	{
		for (int i = 0; i < lexReswordSets.Count; i++)
		{
			if (lexReswordSets[i].FindResword(resword))
			{
				return i;
			}
		}
		return -1;
	}

	public int AddExpression(string expression)
	{
		return expressions.Add(expression);
	}

	public bool ShouldSerializeReswords()
	{
		return false;
	}

	public bool ShouldSerializeReswordSets()
	{
		return lexReswordSets.Count > 0;
	}
}
