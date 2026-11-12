using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Viewer.FullScreen;

public class StiElementFullScreenHelper : StiFullScreenHelper
{
	internal Control StoredParent { get; set; }

	internal int StoredIndex { get; set; }

	internal Control CurrentControl { get; set; }

	internal bool CurrentControlShadowVisibility { get; set; }

	internal CheckBox CurrentTab { get; set; }

	public void Show(Control elementControl)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (!base.IsActivated)
		{
			elementControl.Dock = (DockStyle)5;
			CurrentControl = elementControl;
			CurrentTab = base.Viewer.GetSelectedTab();
			((Control)base.Viewer).Padding = new Padding((int)(10.0 * StiScale.Factor));
			StiTableElementControl stiTableElementControl = ((elementControl is StiHolderElementControl stiHolderElementControl) ? stiHolderElementControl.Child : null) as StiTableElementControl;
			bool flag = stiTableElementControl?.IsAllColumnsInvisible ?? false;
			StiSimpleShadow shadow = GetShadow(elementControl);
			CurrentControlShadowVisibility = shadow?.Visible ?? false;
			if (shadow != null)
			{
				shadow.Visible = false;
			}
			StoredParent = elementControl.Parent;
			StoredIndex = elementControl.Parent.Controls.IndexOf(elementControl);
			elementControl.Parent.Controls.Remove(elementControl);
			((Control)base.Viewer.GetElementsPanel()).Controls.Add(elementControl);
			((Control)base.Viewer.GetToolBarPanel()).Visible = false;
			if (stiTableElementControl != null && flag)
			{
				stiTableElementControl.HideAllColumns();
			}
			base.IsActivated = true;
			StiActionButtonsHelper.RefreshButtonsVisibility(elementControl as IStiActionButtons, base.Viewer.GetElementFromControl(elementControl));
			elementControl.BringToFront();
		}
	}

	public void Close()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsActivated && CurrentControl != null)
		{
			StiTableElementControl stiTableElementControl = ((CurrentControl is StiHolderElementControl stiHolderElementControl) ? stiHolderElementControl.Child : null) as StiTableElementControl;
			bool flag = stiTableElementControl?.IsAllColumnsInvisible ?? false;
			if (CurrentControlShadowVisibility)
			{
				GetShadow(CurrentControl).Visible = true;
			}
			Control currentControl = CurrentControl;
			currentControl.Dock = (DockStyle)0;
			CurrentControl = null;
			((Control)base.Viewer).Padding = new Padding(0);
			if (StoredParent != null)
			{
				((Control)base.Viewer.GetElementsPanel()).Controls.Remove(currentControl);
				StoredParent.Controls.Add(currentControl);
				StoredParent.Controls.SetChildIndex(currentControl, StoredIndex);
			}
			if (stiTableElementControl != null && flag)
			{
				stiTableElementControl.HideAllColumns();
			}
			((Control)base.Viewer.GetToolBarPanel()).Visible = base.Viewer.GetMainToolbarVisibility(base.Viewer.Report);
			base.IsActivated = false;
			StiActionButtonsHelper.RefreshButtonsVisibility(currentControl as IStiActionButtons, base.Viewer.GetElementFromControl(currentControl));
			base.Viewer.ResetAllTabs();
			if (CurrentTab != null)
			{
				CurrentTab.Checked = true;
			}
		}
	}

	private static StiSimpleShadow GetShadow(Control control)
	{
		if (!(control is StiHolderElementControl stiHolderElementControl))
		{
			return null;
		}
		if (!(stiHolderElementControl?.Child is IStiElementControl stiElementControl))
		{
			return null;
		}
		return (stiElementControl.Element as IStiSimpleShadow)?.Shadow;
	}

	public StiElementFullScreenHelper(StiDashboardViewerControl viewer)
		: base(viewer)
	{
	}
}
