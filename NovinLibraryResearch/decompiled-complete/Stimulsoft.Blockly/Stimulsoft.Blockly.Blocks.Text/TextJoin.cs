using System.Linq;
using System.Text;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class TextJoin : IronBlock
{
	public override object Evaluate(Context context)
	{
		int num = int.Parse(base.Mutations.GetValue("items"));
		StringBuilder stringBuilder = new StringBuilder();
		int i;
		for (i = 0; i < num; i++)
		{
			if (base.Values.Any((Value x) => x.Name == $"ADD{i}"))
			{
				stringBuilder.Append(base.Values.Evaluate($"ADD{i}", context));
			}
		}
		return stringBuilder.ToString();
	}
}
