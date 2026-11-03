using System.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiLinesGeom : StiGeom
{
	public StiPenGeom Pen { get; set; }

	public PointF[] Points { get; set; }

	public override StiGeomType Type => StiGeomType.Lines;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		if (Pen != null)
		{
			((JContainer)val).Add((object)new JProperty("Pen", (object)Pen.SaveToJsonObject(mode)));
		}
		object[] array = SavePointFArrayToJsonObject(Points);
		((JContainer)val).Add((object)new JProperty("Points", array));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiLinesGeom(StiPenGeom pen, PointF[] points)
	{
		Pen = pen;
		Points = points;
	}
}
