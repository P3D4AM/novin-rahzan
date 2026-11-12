using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiRegionMapAtom : StiElementAtom
{
	internal StiGdiDbsMapContextPainter Context { get; private set; }

	protected StiRegionMapElement RegionMapElement => base.Element as StiRegionMapElement;

	public List<string> CheckedMapKeys { get; set; } = new List<string>();


	public string MouseOverMapKey { get; set; }

	public override void Paint(Graphics g)
	{
		GraphicsState val = g.Save();
		try
		{
			g.TranslateTransform((float)base.Location.X, (float)base.Location.Y);
			Context = new StiGdiDbsMapContextPainter(RegionMapElement)
			{
				CheckedMapKeys = CheckedMapKeys,
				MouseOverKey = MouseOverMapKey,
				KeyValues = new StiMapKeyHelper().GetMapKeysFromNames(GetValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiKeyMapMeter)), RegionMapElement.MapIdent, RegionMapElement.Language, RegionMapElement.Report)?.Cast<object>().ToList(),
				NameValues = GetValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiNameMapMeter)),
				ValueValues = GetValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiValueMapMeter)),
				GroupValues = GetValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGroupMapMeter)),
				ColorValues = GetValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiColorMapMeter)),
				AnimationEngine = base.AnimationEngine
			};
			if (base.AnimationEngine != null)
			{
				base.AnimationEngine.RegisterContextPainter(RegionMapElement, Context);
			}
			Context.Render(g, useZoom: true, new RectangleD(0.0, 0.0, base.Location.Width, base.Location.Height));
		}
		finally
		{
			g.Restore(val);
		}
	}

	protected List<object> GetValues(IStiMeter meter)
	{
		if (DataTable == null || meter == null)
		{
			return null;
		}
		int num = DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return DataTable.Rows.GetArrayItem(num).ToList();
	}

	public override void Render()
	{
	}

	public StiRegionMapAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}

	public StiRegionMapAtom(IStiElement element, RectangleD location, StiDataTable dataTable, StiAnimationEngine animation)
		: base(element, location, dataTable, animation)
	{
	}
}
