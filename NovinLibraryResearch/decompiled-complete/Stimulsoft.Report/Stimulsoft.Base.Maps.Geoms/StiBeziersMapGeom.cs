using System.Text;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiBeziersMapGeom : StiMapGeom
{
	public double[] SvgValues { get; set; }

	public double[] Array { get; set; }

	public override StiMapGeomType GeomType => StiMapGeomType.Beziers;

	public override PointD GetLastPoint()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(Array[Array.Length - 2], Array[Array.Length - 1]);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Beziers=");
		if (Array != null)
		{
			double[] array = Array;
			foreach (double num in array)
			{
				stringBuilder.Append(num.ToString());
				stringBuilder.Append(", ");
			}
		}
		return stringBuilder.ToString();
	}
}
