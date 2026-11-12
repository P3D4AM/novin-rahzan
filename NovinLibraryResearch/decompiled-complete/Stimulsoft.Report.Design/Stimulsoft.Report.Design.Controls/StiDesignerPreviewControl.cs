using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Viewer.Design;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiDesignerPreviewControl : UserControl
{
	private IContainer components;

	private bool stateRender;

	public StiWinViewerWrapper ViewerControl { get; set; }

	internal StiDesignerControl Designer { get; set; }

	internal string TextInfo { get; private set; }

	internal bool StateRender
	{
		get
		{
			return stateRender;
		}
		set
		{
			stateRender = value;
			InvokeStateRenderChanged();
		}
	}

	internal event EventHandler StateRenderChanged;

	public StiDesignerPreviewControl(StiDesignerControl designer)
	{
		Designer = designer;
		ViewerControl = new StiWinViewerWrapper(null, removeShortcuts: true, allowRibbon: true);
		((Control)ViewerControl).Dock = (DockStyle)5;
		((Control)ViewerControl).Location = new Point(0, 0);
		((Control)ViewerControl).Name = "ViewerControl";
		ViewerControl.Report = null;
		((Control)ViewerControl).RightToLeft = (RightToLeft)0;
		ViewerControl.ShowCloseButton = false;
		((Control)ViewerControl).Size = new Size(680, 536);
		((Control)ViewerControl).TabIndex = 1;
		ViewerControl.LockButtonOpen(value: true);
		((Control)this).Controls.Add((Control)(object)ViewerControl);
		InitializeComponent();
		designer.DesignerPreviewControl = this;
		ViewerControl.IgnoreEscapeKey = true;
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "StiDesignerPreviewControl";
		((Control)this).Size = new Size(680, 560);
		((Control)this).ResumeLayout(false);
	}

	public void Localize()
	{
		if (!((Component)this).DesignMode)
		{
			ViewerControl?.Localize();
		}
	}

	public void DoRefresh()
	{
		ButtonRefresh_Click(this, EventArgs.Empty);
	}

	public void DoStop()
	{
		ButtonStop_Click(this, EventArgs.Empty);
	}

	public void ApplyStyle()
	{
		ViewerControl.ApplyStyle();
	}

	internal void ClearOldReport()
	{
		if (ViewerControl != null && ViewerControl.Report != null)
		{
			StiReport report = ViewerControl.Report;
			ViewerControl.Report = null;
			report.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}

	private void Report_Rendering(object sender, EventArgs e)
	{
		if (Designer != null && Designer.Report != null)
		{
			StiReport stiReport = Designer.Report.CompiledReport ?? Designer.Report;
			TextInfo = stiReport.StatusString;
			Application.DoEvents();
		}
	}

	private void ButtonStop_Click(object sender, EventArgs e)
	{
		Designer.Report.IsStopped = true;
	}

	internal void ButtonRefresh_Click(object sender, EventArgs e)
	{
		if (Designer.LockReportPreview || Designer.Report == null)
		{
			return;
		}
		try
		{
			ViewerControl.LockButtonOpen(value: true);
			ViewerControl.UpdateControlsState();
			TextInfo = StiLocalization.Get("FormDesigner", "CompilingReport");
			Application.DoEvents();
			Designer.LockReportPreview = true;
			((Control)this).Refresh();
			Designer.Report.ResetRenderedState();
			Designer.InvokeClearingCompilerResults();
			Designer.CheckHelper.BuildChecks(Designer.Report);
			if (Designer.CheckHelper.ErrorsCount > 0)
			{
				Designer.InvokeShowingCompilerResults(Designer.Report.CompilerResults);
			}
			if (Designer.Report.IsPageDesigner)
			{
				ClearOldReport();
				StiReport stiReport = StiReportCopyHelper.Copy(Designer.Report);
				stiReport.RenderedPages.Clear();
				stiReport.RenderedPages.Add(Designer.Report.Pages[0].Clone() as StiPage);
				ViewerControl.Report = stiReport;
				ViewerControl.LoadConfig();
				((Control)ViewerControl).Refresh();
			}
			else if (Designer.CheckHelper.ErrorsCount == 0)
			{
				if (Designer.Report.CompiledReport != null)
				{
					ClearOldReport();
					try
					{
						Designer.Report.CompiledReport.BeginRender += CompiledReport_BeginRender;
						Designer.Report.CompiledReport.EndRender += CompiledReport_EndRender;
						Designer.Report.CompiledReport.Rendering += Report_Rendering;
						StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(Designer);
						if (Designer.Report == null)
						{
							return;
						}
						Designer.Report.Render(showProgress: false);
						if (Designer.Report == null)
						{
							return;
						}
						Designer?.CheckHelper?.BuildReportRenderingMessages(Designer.Report);
						ViewerControl.Report = Designer.Report.CompiledReport;
						bool bookmarksButtonChecked = ViewerControl.BookmarksButtonChecked;
						ViewerControl.LoadConfig();
						ViewerControl.BookmarksButtonChecked = bookmarksButtonChecked;
						ViewerControl.LockButtonOpen(value: false);
						((Control)ViewerControl).Refresh();
						if (StiOptions.Engine.ShowReportRenderingMessages && Designer != null)
						{
							Designer.InvokeShowingReportRenderingMessages();
						}
					}
					finally
					{
						if (Designer.Report.CompiledReport != null)
						{
							Designer.Report.CompiledReport.BeginRender -= CompiledReport_BeginRender;
							Designer.Report.CompiledReport.EndRender -= CompiledReport_EndRender;
							Designer.Report.CompiledReport.Rendering -= Report_Rendering;
						}
					}
				}
				else if (Designer.Report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
				{
					ClearOldReport();
					StiReport stiReport2 = StiReportCopyHelper.Copy(Designer.Report);
					try
					{
						stiReport2.BeginRender += CompiledReport_BeginRender;
						stiReport2.EndRender += CompiledReport_EndRender;
						stiReport2.Rendering += Report_Rendering;
						StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(Designer);
						Designer.Report.CompiledReport = stiReport2;
						stiReport2.Progress = Designer.Report.Progress;
						stiReport2.Render(showProgress: false);
						Designer.Report.CompiledReport = null;
						Designer.CheckHelper?.BuildReportRenderingMessages(stiReport2);
						ViewerControl.Report = stiReport2;
						ViewerControl.LoadConfig();
						((Control)ViewerControl).Refresh();
						if (StiOptions.Engine.ShowReportRenderingMessages)
						{
							Designer?.InvokeShowingReportRenderingMessages();
						}
					}
					finally
					{
						stiReport2.BeginRender -= CompiledReport_BeginRender;
						stiReport2.EndRender -= CompiledReport_EndRender;
						stiReport2.Rendering -= Report_Rendering;
					}
				}
			}
			if (Designer.CheckHelper.IsMessagesPresent)
			{
				Designer.IsReportPreviewMode = true;
				StiCheckStatusEventArgs e2 = new StiCheckStatusEventArgs(Designer.CheckHelper.ErrorsCount, Designer.CheckHelper.WarningsCount, Designer.CheckHelper.InformationMessagesCount, Designer.CheckHelper.ReportRenderingMessagesCount);
				Designer.InvokeShowingReportCheckerResults(e2);
			}
			else
			{
				Designer.InvokeClearingReportCheckerResults();
			}
			Designer.UpdateCheckHelperButton();
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(IStiDesignerBase), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			if (Designer.Report != null)
			{
				Designer.Report.CompiledReport?.Dictionary.Disconnect();
				ViewerControl.LockButtonOpen(value: true);
				Designer.Report.Dictionary.Connect(loadData: false);
				Designer.Report.CompiledReport = null;
				TextInfo = "";
				Designer.LockReportPreview = false;
				StateRender = false;
				ViewerControl.LockButtonOpen(value: false);
				ViewerControl.UpdateControlsState();
			}
		}
	}

	private void CompiledReport_EndRender(object sender, EventArgs e)
	{
		StateRender = false;
		StiReport stiReport = sender as StiReport;
		foreach (object database in stiReport.Dictionary.Databases)
		{
			if (database is StiQuickBooksDatabase stiQuickBooksDatabase)
			{
				((StiQuickBooksDatabase)Designer.Report.Dictionary.Databases[stiQuickBooksDatabase.Name]).ConnectionString = stiQuickBooksDatabase.ConnectionString;
			}
		}
	}

	private void CompiledReport_BeginRender(object sender, EventArgs e)
	{
		StateRender = true;
	}

	private void InvokeStateRenderChanged()
	{
		this.StateRenderChanged?.Invoke(this, EventArgs.Empty);
	}
}
