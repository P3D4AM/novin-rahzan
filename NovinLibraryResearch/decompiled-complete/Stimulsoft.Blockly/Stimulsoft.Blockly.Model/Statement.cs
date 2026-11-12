namespace Stimulsoft.Blockly.Model;

public class Statement : IFragment
{
	public string Name { get; set; }

	public IronBlock Block { get; set; }

	public object Evaluate(Context context)
	{
		if (Block == null)
		{
			return null;
		}
		return Block.Evaluate(context);
	}
}
