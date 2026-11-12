using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Options;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public abstract class StiProgressElementControl : StiBaseElementControl
{
	private const int ProgressSize = 100;

	private Timer progressTimer;

	private long progressTick;

	private float currentProgressValue;

	private static object lockObject = new object();

	private bool isShowProgress;

	private bool isShowNoResult;

	private bool isShowEmptyData;

	public StiDataTable DataTable { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsShowProgress
	{
		get
		{
			return isShowProgress;
		}
		set
		{
			if (value != isShowProgress)
			{
				isShowProgress = value;
				if (value)
				{
					StartProgressTimer();
				}
				else
				{
					StopProgressTimer();
				}
				if (StiDashboardOptions.Data.ProgressRefreshType == StiDashboardProgressRefreshType.Invalidate)
				{
					((Control)this).Invalidate();
				}
				else if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
				{
					Application.DoEvents();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsActiveProgress
	{
		get
		{
			if (IsShowProgress)
			{
				return !StiComponentProgressHelper.IsHidenProgress(progressTick);
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsShowNoResult
	{
		get
		{
			return isShowNoResult;
		}
		set
		{
			if (isShowNoResult != value)
			{
				isShowNoResult = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsShowEmptyData
	{
		get
		{
			return isShowEmptyData;
		}
		set
		{
			if (isShowEmptyData != value)
			{
				isShowEmptyData = value;
				((Control)this).Invalidate();
			}
		}
	}

	public virtual void ShowProgress()
	{
		IsShowProgress = true;
	}

	public virtual void RemoveProgress()
	{
		IsShowProgress = false;
	}

	private void StopProgressTimer()
	{
		lock (lockObject)
		{
			if (progressTimer != null)
			{
				progressTimer.Stop();
				progressTimer.Tick -= ProgressTimer_Tick;
				((Component)(object)progressTimer).Dispose();
				progressTimer = null;
			}
		}
	}

	private void StartProgressTimer()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		lock (lockObject)
		{
			progressTick = Environment.TickCount;
			if (progressTimer == null)
			{
				progressTimer = new Timer
				{
					Interval = 50
				};
				progressTimer.Tick += ProgressTimer_Tick;
				progressTimer.Start();
			}
		}
	}

	public virtual void PaintProgress(Graphics g)
	{
		if (IsActiveProgress)
		{
			StiProgressStatus progressStatus = (StiComponentProgressHelper.IsLongProgress(progressTick) ? StiProgressStatus.Long : StiProgressStatus.Short);
			StiComponentGdiPainter.PaintProgress(g, RectangleD.CreateFromRectangle(GetProgressRect()), progressStatus, currentProgressValue);
		}
	}

	private Rectangle GetProgressRect()
	{
		int val = Math.Min(((Control)this).Width, ((Control)this).Height);
		val = Math.Min(100, val);
		return new Rectangle((((Control)this).Width - val) / 2, (((Control)this).Height - val) / 2, val, val);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			RemoveProgress();
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		RectangleD rect = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
		if (IsShowNoResult && !IsShowProgress)
		{
			StiElementGdiPainter stiElementGdiPainter = StiPainter.GetPainter(base.Element.GetType(), StiGuiMode.Gdi) as StiElementGdiPainter;
			stiElementGdiPainter.PaintNoResult(e.Graphics, rect, base.Element, 1.0);
		}
		else if (IsShowEmptyData && !IsShowProgress)
		{
			StiElementGdiPainter stiElementGdiPainter2 = StiPainter.GetPainter(base.Element.GetType(), StiGuiMode.Gdi) as StiElementGdiPainter;
			stiElementGdiPainter2.PaintEmptyData(e.Graphics, rect, base.Element, 1.0);
		}
		else
		{
			PaintProgress(e.Graphics);
		}
	}

	private void ProgressTimer_Tick(object sender, EventArgs e)
	{
		if (IsShowProgress)
		{
			currentProgressValue += 10f;
			if (currentProgressValue >= 360f)
			{
				currentProgressValue = 0f;
			}
			((Control)this).Invalidate();
		}
	}

	public StiProgressElementControl(IStiElement element)
		: base(element)
	{
	}
}
