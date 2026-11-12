using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public abstract class StiFilterElementControl : StiElementControl, IStiRefreshControl
{
	private Timer filterTimer = new Timer
	{
		Interval = 300
	};

	protected bool LockRefresh { get; set; }

	public bool IsInitialized { get; set; }

	public bool IsFirstInitialized { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool ShowActionButtons => false;

	public abstract void RefreshControl(object sender, StiReportControlUpdateEventArgs e);

	public override void RefreshStatus(bool checkProgressStatus)
	{
		if (!IsInitialized)
		{
			base.RefreshStatus(checkProgressStatus);
			IsInitialized = true;
		}
	}

	protected abstract void RefreshFilter();

	protected abstract void ResetFilter();

	protected virtual void RefreshAllChilds()
	{
		if (!((base.Element as StiComponent).Page is StiDashboard stiDashboard))
		{
			return;
		}
		StiDashboardViewerControl viewer = StiDashboardViewerHelper.FindViewer((Control)(object)this);
		if (!StiCrossLinkedFilterHelper.IsCrossLinkedFilter(base.Element as IStiFilterElement))
		{
			(from IStiFilterElement l in from c in stiDashboard.GetComponents().ToList()
					where c is IStiFilterElement
					select c
				where l.GetParentKey() == base.Element.Key
				select viewer.ElementToControl[l] into c
				select (c as StiHolderElementControl)?.Child as StiFilterElementControl into c
				where c != null
				select c).ToList().ForEach(delegate(StiFilterElementControl c)
			{
				c.IsFirstInitialized = false;
				c.IsInitialized = false;
				c.IsShowProgress = false;
				c.RefreshAllChilds();
				c.ResetFilter();
			});
		}
	}

	protected void StartRefreshingFilter()
	{
		if (filterTimer != null && IsFirstInitialized)
		{
			filterTimer.Stop();
			filterTimer.Start();
		}
	}

	private void CreateFilterTimer()
	{
		if (filterTimer != null)
		{
			filterTimer.Tick += delegate
			{
				filterTimer.Stop();
				RefreshFilter();
			};
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (filterTimer != null)
		{
			filterTimer.Stop();
			((Component)(object)filterTimer).Dispose();
			filterTimer = null;
		}
	}

	public StiFilterElementControl(IStiElement element)
		: base(element)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		CreateFilterTimer();
	}
}
