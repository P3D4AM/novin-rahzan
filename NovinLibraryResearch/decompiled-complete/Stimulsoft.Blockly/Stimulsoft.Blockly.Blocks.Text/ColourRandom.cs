using System;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Text;

public class ColourRandom : IronBlock
{
	private Random random = new Random();

	public override object Evaluate(Context context)
	{
		byte[] array = new byte[3];
		random.NextBytes(array);
		return string.Format("#{0}", string.Join("", array.Select((byte x) => $"{x:x2}")));
	}
}
