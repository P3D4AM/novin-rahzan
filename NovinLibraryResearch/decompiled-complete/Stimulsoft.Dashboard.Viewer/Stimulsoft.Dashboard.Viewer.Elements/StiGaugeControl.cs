using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public class StiGaugeControl : StiBaseElementControl
{
	private StiGaugeAtom atom;

	public StiGaugeAtom Atom
	{
		get
		{
			return atom;
		}
		set
		{
			if (atom != value)
			{
				atom = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	internal StiAnimationEngine AnimationEngine { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		PaintAtom(e.Graphics);
		AnimationEngine.Continue();
	}

	protected virtual void PaintAtom(Graphics g)
	{
		if (Atom != null)
		{
			StiElementGdiPainter stiElementGdiPainter = new StiElementGdiPainter();
			if (!stiElementGdiPainter.IsNoResult(Atom.Element, Atom.DataTable) && !stiElementGdiPainter.IsEmptyData(Atom.Element, Atom.DataTable))
			{
				Atom.Location = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
				Atom.Paint(g);
			}
		}
	}

	public void RefreshChild(StiDataTable dataTable)
	{
		Atom = StiElementAtom.Get(new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), base.Element, dataTable, AnimationEngine) as StiGaugeAtom;
	}

	public void RecalculateSize(Rectangle rect)
	{
		if (Atom != null)
		{
			RectangleF contentRectangle = Atom.GetContentRectangle(RectangleD.CreateFromRectangle(rect), base.Element);
			((Control)this).Width = (int)contentRectangle.Width;
			((Control)this).Height = (int)contentRectangle.Height;
		}
	}

	public StiGaugeControl(IStiElement element)
		: base(element)
	{
	}
}
