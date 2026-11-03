using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearRange : StiRangeBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearRange;

	public override string LocalizeName => "LinearRange";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.EndValue(),
			propertiesHelper.StartValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.EndWidth(),
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.StartWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	protected internal override void DrawRange(StiGaugeContextPainter context, StiScaleBase scale)
	{
		if (scale == null)
		{
			return;
		}
		float startWidth = base.StartWidth;
		float endWidth = base.EndWidth;
		if (startWidth != 0f && endWidth != 0f)
		{
			RectangleF rect = new RectangleF(0f, 0f, 0f, 0f);
			StiGraphicsPathLinesGaugeGeom stiGraphicsPathLinesGaugeGeom = scale.barGeometry.DrawGeometry(context, base.StartValue, base.EndValue, startWidth, endWidth, base.Offset, base.Placement, ref rect, returnOnlyRect: false);
			if (stiGraphicsPathLinesGaugeGeom != null)
			{
				StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, stiGraphicsPathLinesGaugeGeom.Points[0], base.Brush, base.BorderBrush, base.BorderWidth);
				stiGraphicsPathGaugeGeom.Geoms.Add(stiGraphicsPathLinesGaugeGeom);
				stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
				context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
			}
		}
	}

	public override StiRangeBase CreateNew()
	{
		return new StiLinearRange();
	}
}
