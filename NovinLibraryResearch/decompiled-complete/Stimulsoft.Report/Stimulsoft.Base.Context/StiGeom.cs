using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public abstract class StiGeom : IStiJsonReportObject
{
	public abstract StiGeomType Type { get; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		JObject val = new JObject();
		((JContainer)val).Add((object)new JProperty("Ident", (object)GetType().Name));
		((JContainer)val).Add((object)new JProperty("Type", (object)Type.ToString()));
		return val;
	}

	protected JObject[] SaveGeomListToJsonObject(List<StiSegmentGeom> geoms, StiJsonSaveMode mode)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (geoms != null)
		{
			JObject[] array = (JObject[])(object)new JObject[geoms.Count];
			for (int i = 0; i < geoms.Count; i++)
			{
				array[i] = geoms[i].SaveToJsonObject(mode);
			}
			return array;
		}
		return null;
	}

	protected JObject[] SavePointFArrayToJsonObject(PointF[] points)
	{
		if (points != null)
		{
			JObject[] array = (JObject[])(object)new JObject[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				array[i] = SavePointFToJsonObject(points[i]);
			}
			return array;
		}
		return null;
	}

	protected string SaveBrushToJsonObject(object brush, StiJsonSaveMode mode)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		if (brush is Color color)
		{
			return $"Color,{color.A},{color.R},{color.G},{color.B}";
		}
		if (brush is StiBrush)
		{
			return Serialize.JBrush((StiBrush)brush, (StiBrush)null);
		}
		return null;
	}

	protected JObject SaveRectToJsonObject(object rect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (rect is Rectangle)
		{
			return SaveRectangleToJsonObject((Rectangle)rect);
		}
		if (rect is RectangleF)
		{
			return SaveRectangleFToJsonObject((RectangleF)rect);
		}
		if (rect is RectangleD)
		{
			return SaveRectangleDToJsonObject((RectangleD)rect);
		}
		return null;
	}

	public static JObject SavePointFToJsonObject(PointF pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		JObject val = new JObject();
		((JContainer)val).Add((object)new JProperty("X", (object)pos.X));
		((JContainer)val).Add((object)new JProperty("Y", (object)pos.Y));
		return val;
	}

	public static JObject SaveRectangleToJsonObject(Rectangle rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		JObject val = new JObject();
		((JContainer)val).Add((object)new JProperty("X", (object)rect.X));
		((JContainer)val).Add((object)new JProperty("Y", (object)rect.Y));
		((JContainer)val).Add((object)new JProperty("Width", (object)rect.Width));
		((JContainer)val).Add((object)new JProperty("Height", (object)rect.Height));
		return val;
	}

	public static JObject SaveRectangleFToJsonObject(RectangleF rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		JObject val = new JObject();
		((JContainer)val).Add((object)new JProperty("X", (object)rect.X));
		((JContainer)val).Add((object)new JProperty("Y", (object)rect.Y));
		((JContainer)val).Add((object)new JProperty("Width", (object)rect.Width));
		((JContainer)val).Add((object)new JProperty("Height", (object)rect.Height));
		return val;
	}

	public static JObject SaveRectangleDToJsonObject(RectangleD rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		JObject val = new JObject();
		((JContainer)val).Add((object)new JProperty("X", (object)((RectangleD)(ref rect)).X));
		((JContainer)val).Add((object)new JProperty("Y", (object)((RectangleD)(ref rect)).Y));
		((JContainer)val).Add((object)new JProperty("Width", (object)((RectangleD)(ref rect)).Width));
		((JContainer)val).Add((object)new JProperty("Height", (object)((RectangleD)(ref rect)).Height));
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
	}
}
