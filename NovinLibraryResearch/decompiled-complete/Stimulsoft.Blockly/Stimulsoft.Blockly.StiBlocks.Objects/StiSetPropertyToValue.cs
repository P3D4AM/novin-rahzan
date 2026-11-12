using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Blockly.StiBlocks.Objects;

public class StiSetPropertyToValue : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (base.Values.Evaluate("COMPONENT", context) is StiComponent stiComponent)
		{
			string text = base.Fields.Get("PROPERTY");
			object obj = base.Values.Evaluate("VALUE", context);
			switch (text)
			{
			case "ENABLED":
				stiComponent.Enabled = Convert.ToBoolean(obj);
				break;
			case "BACKGROUND":
				if (stiComponent is IStiBrush stiBrush && obj != null)
				{
					Color color = ColorTranslator.FromHtml(obj.ToString());
					stiBrush.Brush = new StiSolidBrush(color);
				}
				break;
			case "FONT":
				if (stiComponent is IStiFont && obj == null)
				{
				}
				break;
			case "WIDTH":
				stiComponent.Width = Convert.ToDouble(obj);
				break;
			case "HEIGHT":
				stiComponent.Height = Convert.ToDouble(obj);
				break;
			}
			stiComponent.Report.Invalidate();
		}
		return base.Evaluate(context);
	}
}
