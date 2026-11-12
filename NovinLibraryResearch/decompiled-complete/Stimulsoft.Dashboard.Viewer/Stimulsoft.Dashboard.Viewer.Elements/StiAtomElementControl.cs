using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public class StiAtomElementControl : StiElementControl, IStiRefreshControl
{
	private StiElementAtom atom;

	public StiElementAtom Atom
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

	public bool IsInitialized { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool ShowActionButtons
	{
		get
		{
			if (!(base.Element is IStiTextElement) && !(base.Element is IStiImageElement))
			{
				return !(base.Element is IStiShapeElement);
			}
			return false;
		}
	}

	public virtual void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			base.AnimationEngine?.Finish();
			Atom.Render();
			((Control)this).Invalidate();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Atom?.Dispose();
			Atom = null;
			base.AnimationEngine?.Finish();
			base.AnimationEngine?.Dispose();
			base.AnimationEngine = null;
		}
		base.Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		PaintAtom(e.Graphics);
		PaintProgress(e.Graphics);
	}

	public override void CreateChild()
	{
		Atom = StiElementAtom.Get(new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), base.Element, null, base.AnimationEngine);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		Atom = StiElementAtom.Get(new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), base.Element, dataTable, base.AnimationEngine);
		IsInitialized = true;
	}

	public virtual void PaintAtom(Graphics g)
	{
		if (!base.IsActiveProgress && Atom != null)
		{
			StiElementGdiPainter stiElementGdiPainter = new StiElementGdiPainter();
			if (stiElementGdiPainter.IsNoResult(Atom.Element, Atom.DataTable) && !base.IsShowProgress && IsInitialized)
			{
				stiElementGdiPainter.PaintNoResult(g, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), Atom.Element, 1.0);
				return;
			}
			Atom.Location = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
			Atom.Paint(g);
		}
	}

	public StiAtomElementControl(IStiElement element)
		: base(element)
	{
	}
}
