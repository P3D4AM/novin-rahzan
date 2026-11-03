using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.ShapeTypes.Design;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.ShapeType.bmp")]
[StiServiceCategoryBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.ShapeType.bmp")]
[TypeConverter(typeof(StiShapeTypeServiceConverter))]
public abstract class StiShapeTypeService : StiService, IStiPropertyGridObject, IStiJsonReportObject
{
	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiShapeTypeService;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryShapes");

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiShapeTypeService);

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
	}

	internal static StiShapeTypeService CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiShapeTypeService stiShapeTypeService = StiOptions.Services.Shapes.FirstOrDefault((StiShapeTypeService x) => ((StiService)x).ServiceEnabled && ((object)x).GetType().Name == ident);
		if (stiShapeTypeService == null)
		{
			throw new Exception("Type is not found!");
		}
		StiShapeTypeService stiShapeTypeService2 = stiShapeTypeService.CreateNew();
		stiShapeTypeService2.LoadFromJsonObject(jObject);
		return stiShapeTypeService2;
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public virtual StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public virtual void Paint(object context, StiShape shape, RectangleF rect, float zoom)
	{
		StiShapeTypePainter painter = StiShapeTypePainter.GetPainter(((object)this).GetType(), (StiGuiMode)(!(context is Graphics)));
		painter.Paint(context, shape, this, rect, zoom);
	}

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public virtual StiShapeTypeService CreateNew()
	{
		throw new NotImplementedException();
	}
}
