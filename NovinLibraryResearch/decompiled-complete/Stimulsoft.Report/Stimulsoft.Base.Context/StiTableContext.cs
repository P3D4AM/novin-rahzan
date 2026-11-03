using System.Collections.Generic;
using System.Drawing;

namespace Stimulsoft.Base.Context;

public abstract class StiTableContext
{
	public List<StiTableGeom> Geoms { get; set; } = new List<StiTableGeom>();


	public abstract void Render(RectangleF rect);
}
