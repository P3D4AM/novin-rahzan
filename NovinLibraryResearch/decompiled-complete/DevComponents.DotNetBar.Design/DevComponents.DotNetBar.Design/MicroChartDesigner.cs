using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class MicroChartDesigner : ControlDesigner
{
	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			SetDataPointsDefaults();
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDataPointsDefaults()
	{
		if (((ControlDesigner)this).Control is MicroChart microChart)
		{
			microChart.DataPoints = MicroChartItemDesigner.GetRandomDataPoints();
		}
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is MicroChart microChart)
		{
			microChart.Style = eDotNetBarStyle.StyleManagerControlled;
		}
	}
}
