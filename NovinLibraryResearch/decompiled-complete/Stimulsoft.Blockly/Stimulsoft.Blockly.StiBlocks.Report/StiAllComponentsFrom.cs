using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Blockly.StiBlocks.Report;

public class StiAllComponentsFrom : IronBlock
{
	public override object Evaluate(Context context)
	{
		string name = base.Fields.Get("NAME").ToString();
		StiComponent component = GetComponent(context, name);
		if (component is StiPage stiPage)
		{
			return stiPage.GetComponents().ToList();
		}
		if (component is StiContainer stiContainer)
		{
			return stiContainer.GetComponents().ToList();
		}
		return null;
	}

	private StiComponent GetComponent(Context context, string name)
	{
		StiComponent stiComponent = null;
		if (context.Report.IsRendered)
		{
			foreach (StiPage renderedPage in context.Report.RenderedPages)
			{
				if (renderedPage.Name == name)
				{
					return renderedPage;
				}
			}
			return context.Report.RenderedPages.GetComponentByName(name);
		}
		return context.Report.GetComponentByName(name);
	}
}
