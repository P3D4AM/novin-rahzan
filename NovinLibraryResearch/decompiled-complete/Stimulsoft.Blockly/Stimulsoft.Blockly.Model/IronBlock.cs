using System.Collections.Generic;

namespace Stimulsoft.Blockly.Model;

public abstract class IronBlock : IFragment
{
	public string Id { get; set; }

	public IList<Field> Fields { get; set; }

	public IList<Value> Values { get; set; }

	public IList<Statement> Statements { get; set; }

	public string Type { get; set; }

	public bool Inline { get; set; }

	public IronBlock Next { get; set; }

	public IList<Mutation> Mutations { get; set; }

	public virtual object Evaluate(Context context)
	{
		if (Next != null && context.EscapeMode == EscapeMode.None)
		{
			return Next.Evaluate(context);
		}
		return null;
	}

	public IronBlock()
	{
		Fields = new List<Field>();
		Values = new List<Value>();
		Statements = new List<Statement>();
		Mutations = new List<Mutation>();
	}
}
