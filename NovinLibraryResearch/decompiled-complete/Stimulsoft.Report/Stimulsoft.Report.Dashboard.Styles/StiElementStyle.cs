using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiElementStyle : StiBaseStyle
{
	public abstract StiElementStyleIdent Ident { get; }

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return new StiPropertyCollection();
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
	}

	public override void SetStyleToComponent(StiComponent component)
	{
	}
}
