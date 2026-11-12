using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Design.Tools;

[StiServiceCategoryBitmap(typeof(StiToolService), "Stimulsoft.Report.Design.Bmp.Tools.ToolBox.bmp")]
[StiToolbox(true)]
public abstract class StiToolService : StiDesignerInitService
{
	public sealed override string ServiceCategory => Loc.Get("Toolbox", "title");

	public sealed override Type ServiceType => typeof(StiToolService);

	public virtual Keys Shortcut => (Keys)0;

	public abstract int Position { get; }

	public virtual bool Active { get; private set; }

	public ButtonItem Button { get; set; }

	public event EventHandler Activated;

	public event EventHandler Deactivated;

	protected virtual void OnActivated(EventArgs e)
	{
	}

	public virtual void InvokeActivated(object sender, EventArgs e)
	{
		OnActivated(e);
		this.Activated?.Invoke(sender, e);
	}

	protected virtual void OnDeactivated(EventArgs e)
	{
	}

	public virtual void InvokeDeactivated(object sender, EventArgs e)
	{
		OnDeactivated(e);
		this.Deactivated?.Invoke(sender, e);
	}

	public virtual void Activate()
	{
		if (!Active)
		{
			InvokeActivated(this, EventArgs.Empty);
		}
		Designer.InvokeResetingToolbox();
		Active = true;
		if (Button != null)
		{
			Button.Checked = true;
		}
		Designer.CurrentTool = this;
		if (Designer.PageView != null && Designer.CurrentPageView != null)
		{
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
		}
	}

	public virtual void Reset()
	{
		if (Active)
		{
			InvokeDeactivated(this, EventArgs.Empty);
		}
		Active = false;
		if (Button != null)
		{
			Button.Checked = false;
		}
	}

	public virtual Cursor GetCursor()
	{
		return Cursors.Default;
	}

	public virtual string GetStatusBarInformation()
	{
		StiPageViewControl currentPageView = Designer.CurrentPageView;
		if (currentPageView == null)
		{
			return string.Empty;
		}
		Point point = ((Control)currentPageView.View).PointToClient(Cursor.Position);
		double num = Designer.XToPage(point.X);
		double num2 = Designer.YToPage(point.Y);
		return $"X:{num:F} Y:{num2:F} ";
	}

	public virtual void OnButtonClick(object sender, EventArgs e)
	{
		Activate();
	}

	public virtual void OnClick(object sender, EventArgs e)
	{
		((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
	}

	public virtual void OnDragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	public virtual void OnDragLeave(object sender, EventArgs e)
	{
	}

	public virtual void OnDragDrop(object sender, DragEventArgs e)
	{
	}

	public virtual bool OnDoubleClick(object sender, EventArgs e)
	{
		return false;
	}

	public virtual void OnMouseDown(object sender, MouseEventArgs e)
	{
		((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
	}

	public virtual void OnMouseMove(object sender, MouseEventArgs e)
	{
		Designer.InvokeRefreshStatusBar();
		if (!(this is StiSelectToolService))
		{
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
		}
	}

	public virtual void OnMouseUp(object sender, MouseEventArgs e)
	{
		((Control)Designer.CurrentPageView.View).Cursor = GetCursor();
	}
}
