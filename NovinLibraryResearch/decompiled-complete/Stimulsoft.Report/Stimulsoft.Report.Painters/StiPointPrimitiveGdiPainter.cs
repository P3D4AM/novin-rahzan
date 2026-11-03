using System;
using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiPointPrimitiveGdiPainter : StiComponentGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
	}
}
