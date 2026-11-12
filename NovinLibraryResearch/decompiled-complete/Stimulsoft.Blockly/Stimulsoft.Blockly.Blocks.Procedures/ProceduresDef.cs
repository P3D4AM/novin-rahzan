using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Procedures;

public class ProceduresDef : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("NAME");
		Statement statement = base.Statements.FirstOrDefault((Statement x) => x.Name == "STACK");
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		if (statement == null)
		{
			statement = new Statement
			{
				Block = null,
				Name = "STACK"
			};
		}
		if (base.Values.Any((Value x) => x.Name == "RETURN"))
		{
			ValueBlock valueBlock = new ValueBlock(base.Values.First((Value x) => x.Name == "RETURN"));
			if (statement.Block == null)
			{
				statement.Block = valueBlock;
			}
			else
			{
				FindEndOfChain(statement.Block).Next = valueBlock;
			}
		}
		if (context.Functions.ContainsKey(text))
		{
			context.Functions[text] = statement;
		}
		else
		{
			context.Functions.Add(text, statement);
		}
		return null;
	}

	private static IronBlock FindEndOfChain(IronBlock block)
	{
		if (block.Next == null)
		{
			return block;
		}
		return FindEndOfChain(block.Next);
	}
}
