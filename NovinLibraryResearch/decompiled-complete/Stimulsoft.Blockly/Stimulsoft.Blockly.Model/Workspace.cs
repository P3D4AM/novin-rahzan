using System.Collections.Generic;
using Stimulsoft.Blockly.Blocks.Procedures;

namespace Stimulsoft.Blockly.Model;

public class Workspace : IFragment
{
	public IList<IronBlock> Blocks { get; set; }

	public Workspace()
	{
		Blocks = new List<IronBlock>();
	}

	public virtual object Evaluate(Context context)
	{
		object result = null;
		List<IronBlock> list = new List<IronBlock>();
		foreach (IronBlock block in Blocks)
		{
			if (block is ProceduresDef)
			{
				block.Evaluate(context);
				list.Add(block);
			}
		}
		foreach (IronBlock block2 in Blocks)
		{
			if (!list.Contains(block2))
			{
				result = block2.Evaluate(context);
			}
		}
		return result;
	}
}
