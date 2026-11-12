using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public abstract class StiElementControl : StiProgressElementControl, IStiMouseEventsControl
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDashboardViewerControl Viewer => StiDashboardViewerHelper.FindViewer((Control)(object)this);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Control Child { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool ShowActionButtons => true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool IsQuerable => base.Element.IsQuerable;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool IsDefined => base.Element.IsDefined;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool IsManuallyEnteredData
	{
		get
		{
			if (base.Element is IStiManuallyEnteredData stiManuallyEnteredData)
			{
				return stiManuallyEnteredData.DataMode == StiDataMode.ManuallyEnteringData;
			}
			return false;
		}
	}

	[Browsable(false)]
	internal StiAnimationEngine AnimationEngine { get; set; }

	public virtual StiDataTable GetData()
	{
		StiDataTable stiDataTable = (base.Element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.Get(base.Element);
	}

	public virtual StiDataTable GetOrCreateData()
	{
		return StiElementDataCache.GetOrCreate(base.Element);
	}

	public virtual void RefreshStatus(bool checkProgressStatus)
	{
		if (!base.Element.IsQuerable || !base.Element.IsDefined || (base.IsShowProgress && checkProgressStatus))
		{
			return;
		}
		StiDataTable dataTable = GetData();
		if (dataTable != null)
		{
			RemoveProgress();
			RefreshChild(dataTable);
			return;
		}
		ShowProgress();
		BackgroundWorker worker = new BackgroundWorker();
		worker.DoWork += delegate
		{
			dataTable = GetOrCreateData();
		};
		worker.RunWorkerCompleted += delegate
		{
			try
			{
				RemoveProgress();
				RefreshChild(dataTable);
				worker.Dispose();
			}
			catch
			{
			}
		};
		worker.RunWorkerAsync();
	}

	public virtual void CreateChild()
	{
	}

	public virtual void RefreshChild(StiDataTable dataTable)
	{
		if (Child != null)
		{
			SetChildVisibilityFromNoResult(dataTable);
		}
	}

	public void RefreshActionButtons()
	{
		StiActionButtonsHelper.RefreshButtonsVisibility(((Control)this).Parent as IStiActionButtons, base.Element);
	}

	public void RefreshViewer()
	{
		Viewer?.RefreshViewer();
	}

	public void RefreshViewerView()
	{
		Viewer?.RenderView();
	}

	public void RefreshViewerData()
	{
		Viewer?.RenderData();
	}

	public void NewTab(StiPage page, string value, List<DictionaryEntry> parameters)
	{
		Viewer?.NewTab(page, value, parameters);
	}

	protected virtual void SetChildVisibilityFromNoResult(StiDataTable dataTable)
	{
		StiElementGdiPainter stiElementGdiPainter = new StiElementGdiPainter();
		SetChildVisibility(!stiElementGdiPainter.IsNoResult(base.Element, dataTable));
	}

	protected void SetChildVisibility(bool visible)
	{
		if (Child == null)
		{
			return;
		}
		if (Child.InvokeRequired)
		{
			Child.BeginInvoke((Delegate)(Action)delegate
			{
				Child.Visible = visible;
			});
		}
		else
		{
			Child.Visible = visible;
		}
	}

	public virtual void RemoveFilter()
	{
	}

	public virtual void SwitchFilterMultiOption()
	{
	}

	protected StiControlColorCollection GetColorCollection()
	{
		return StiControlColorCollection.GetElementColors(base.Element);
	}

	public override void ShowProgress()
	{
		base.ShowProgress();
		SetChildVisibility(visible: false);
	}

	public override void RemoveProgress()
	{
		base.RemoveProgress();
		if (Child != null)
		{
			SetChildVisibilityFromNoResult(base.DataTable);
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		RefreshActionButtons();
		(Child as IStiMouseEventsControl)?.InvokeMouseEnter();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		RefreshActionButtons();
		(Child as IStiMouseEventsControl)?.InvokeMouseLeave();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		RefreshActionButtons();
		(Child as IStiMouseEventsControl)?.InvokeMouseMove(e);
	}

	public void InvokeMouseEnter()
	{
		((Control)this).OnMouseEnter((EventArgs)null);
	}

	public void InvokeMouseLeave()
	{
		((Control)this).OnMouseLeave((EventArgs)null);
	}

	public void InvokeMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	public StiElementControl(IStiElement element)
		: base(element)
	{
		AnimationEngine = new StiAnimationEngine();
		CreateChild();
	}
}
