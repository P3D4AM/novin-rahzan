using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.Progress;

[ToolboxItem(false)]
public class StiProgressControl : StiBaseElementControl
{
	private bool lockResize;

	private StiProgressAtom atom;

	public StiProgressAtom Atom
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

	protected override void OnPaint(PaintEventArgs e)
	{
		PaintAtom(e.Graphics);
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
		Atom = StiElementAtom.Get(new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), base.Element, dataTable) as StiProgressAtom;
	}

	public void RecalculateSize(Rectangle rect)
	{
		if (Atom != null && !lockResize)
		{
			lockResize = true;
			RectangleF contentRectangle = Atom.GetContentRectangle(RectangleD.CreateFromRectangle(rect), base.Element);
			((Control)this).Width = (int)contentRectangle.Width;
			((Control)this).Height = (int)contentRectangle.Height;
			lockResize = false;
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetBoundsCore(x, y, width, height, specified);
		RecalculateSize(new Rectangle(x, y, width, height));
	}

	public StiProgressControl(IStiElement element)
		: base(element)
	{
	}
}
