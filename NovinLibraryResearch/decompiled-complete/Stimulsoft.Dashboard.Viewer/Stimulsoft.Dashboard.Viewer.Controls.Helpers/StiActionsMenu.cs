using System;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Viewer.Export;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

public class StiActionsMenu
{
	protected IStiElement Element { get; }

	protected Control Control { get; }

	protected StiContextMenu ExpressionMenu { get; }

	public virtual void CreateItems(IStiElement element)
	{
		ExpressionMenu.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(element);
		((ToolStrip)ExpressionMenu).Items.Clear();
		if (StiAccountSettings.CurrentAccount != null && element is IStiDashboard && element.Report.CalculationMode == StiCalculationMode.Interpretation && (element.Report.DashboardViewerSettings & StiDashboardViewerSettings.ShowReportSnapshots) > StiDashboardViewerSettings.None)
		{
			CreateSaveToReportSnapshot(element);
		}
		if (IsExportsVisible(element))
		{
			CreateSaveToPdfMenuItem(element);
			CreateSaveToExcelMenuItem(element);
			CreateSaveToHtmlMenuItem(element);
			CreateSaveToDataMenuItem(element);
			CreateSaveToImageMenuItem(element);
		}
	}

	protected virtual void CreateSaveToReportSnapshot(IStiElement element)
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "ReportSnapshot");
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStripItem)stiContextMenuItem2).Click += delegate
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Invalid comparison between Unknown and I4
			if (StiAccountSettings.CurrentAccount != null)
			{
				StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
				if (!StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)stiDashboardViewerControl).Handle) || !StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() || !StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired())
				{
					return;
				}
			}
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((ToolStripDropDown)ExpressionMenu).Close();
				StiDashboardViewerControl stiDashboardViewerControl2 = StiDashboardViewerHelper.FindViewer(Control);
				if (stiDashboardViewerControl2 != null)
				{
					((Control)stiDashboardViewerControl2).Enabled = false;
				}
				string fileName = GetFileName(element);
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "ReportEmbededDataFiles");
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
				((FileDialog)val).FileName = StiDashboardExportDialog.CorrectFileName(fileName);
				((FileDialog)val).DefaultExt = "mrt";
				((FileDialog)val).FilterIndex = StiSettings.GetInt("Viewer", "SaveDialogFilterIndex", 1);
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
				{
					StiReport stiReport = element.Report.Clone() as StiReport;
					stiReport.SaveSnapshot(((FileDialog)val).FileName);
					stiReport.Dispose();
				}
				if (stiDashboardViewerControl2 != null)
				{
					((Control)stiDashboardViewerControl2).Enabled = true;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		if (IsExportsVisible(element))
		{
			((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
	}

	private static bool IsExportsVisible(IStiElement element)
	{
		if (element is StiDashboard)
		{
			return (element.Report.DashboardViewerSettings & StiDashboardViewerSettings.ShowExports) > StiDashboardViewerSettings.None;
		}
		return true;
	}

	private string GetFileName(IStiElement element)
	{
		IStiInteractionLayout stiInteractionLayout = (element as IStiElementInteraction)?.DashboardInteraction as IStiInteractionLayout;
		string text;
		if (!string.IsNullOrWhiteSpace(stiInteractionLayout?.FileName))
		{
			text = StiReportParser.Parse(stiInteractionLayout.FileName, element);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
		}
		StiReport report = element.Report;
		text = Path.GetFileNameWithoutExtension(report.ReportFile);
		if (string.IsNullOrEmpty(text))
		{
			text = report.ReportName;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = report.ReportAlias;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = StiLocalization.Get("Components", "StiReport");
		}
		return text;
	}

	protected virtual void CreateSaveToPdfMenuItem(IStiElement element)
	{
		if (!StiOptions.Viewer.Windows.Exports.ShowPdf)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = "Adobe PDF";
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem item = stiContextMenuItem;
		((ToolStripItem)item).Click += delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			StiDashboardPdfExportSetupForm stiDashboardPdfExportSetupForm = new StiDashboardPdfExportSetupForm();
			try
			{
				if ((int)((Form)stiDashboardPdfExportSetupForm).ShowDialog() == 1)
				{
					StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = false;
					}
					StiDashboardExportSettings exportSettings = stiDashboardPdfExportSetupForm.GetExportSettings();
					StiDashboardExportDialog.Save(((ToolStripItem)item).Tag as IStiElement, exportSettings);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = true;
					}
				}
			}
			finally
			{
				((IDisposable)stiDashboardPdfExportSetupForm)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)item);
	}

	protected virtual void CreateSaveToExcelMenuItem(IStiElement element)
	{
		if (!StiOptions.Viewer.Windows.Exports.ShowExcel)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = "Microsoft Excel";
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem item = stiContextMenuItem;
		((ToolStripItem)item).Click += delegate
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			StiDashboardExcelExportSetupForm stiDashboardExcelExportSetupForm = new StiDashboardExcelExportSetupForm(element);
			try
			{
				if ((int)((Form)stiDashboardExcelExportSetupForm).ShowDialog() == 1)
				{
					StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = false;
					}
					StiDashboardExportSettings exportSettings = stiDashboardExcelExportSetupForm.GetExportSettings();
					StiDashboardExportDialog.Save(((ToolStripItem)item).Tag as IStiElement, exportSettings);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = true;
					}
				}
			}
			finally
			{
				((IDisposable)stiDashboardExcelExportSetupForm)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)item);
	}

	protected virtual void CreateSaveToHtmlMenuItem(IStiElement element)
	{
		if (!StiOptions.Viewer.Windows.Exports.ShowHtml)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = "HTML";
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem item = stiContextMenuItem;
		((ToolStripItem)item).Click += delegate
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			StiDashboardHtmlExportSetupForm stiDashboardHtmlExportSetupForm = new StiDashboardHtmlExportSetupForm(element);
			try
			{
				if ((int)((Form)stiDashboardHtmlExportSetupForm).ShowDialog() == 1)
				{
					StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = false;
					}
					StiDashboardExportSettings exportSettings = stiDashboardHtmlExportSetupForm.GetExportSettings();
					StiDashboardExportDialog.Save(((ToolStripItem)item).Tag as IStiElement, exportSettings);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = true;
					}
				}
			}
			finally
			{
				((IDisposable)stiDashboardHtmlExportSetupForm)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)item);
	}

	protected virtual void CreateSaveToDataMenuItem(IStiElement element)
	{
		if (!(element is IStiTableElement) || !StiOptions.Viewer.Windows.Exports.ShowCsv)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("PropertyMain", "Data");
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem item = stiContextMenuItem;
		((ToolStripItem)item).Click += delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			StiDashboardDataExportSetupForm stiDashboardDataExportSetupForm = new StiDashboardDataExportSetupForm();
			try
			{
				if ((int)((Form)stiDashboardDataExportSetupForm).ShowDialog() == 1)
				{
					StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = false;
					}
					StiDashboardExportSettings exportSettings = stiDashboardDataExportSetupForm.GetExportSettings();
					StiDashboardExportDialog.Save(((ToolStripItem)item).Tag as IStiElement, exportSettings);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = true;
					}
				}
			}
			finally
			{
				((IDisposable)stiDashboardDataExportSetupForm)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)item);
	}

	protected virtual void CreateSaveToImageMenuItem(IStiElement element)
	{
		if (!StiOptions.Viewer.Windows.Exports.ShowImage)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("PropertyMain", "Image");
		((ToolStripItem)stiContextMenuItem).Tag = element;
		((ToolStripItem)stiContextMenuItem).Enabled = StiDashboardExportAssembly.IsAssemblyLoaded;
		StiContextMenuItem item = stiContextMenuItem;
		((ToolStripItem)item).Click += delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			StiDashboardImageExportSetupForm stiDashboardImageExportSetupForm = new StiDashboardImageExportSetupForm();
			try
			{
				if ((int)((Form)stiDashboardImageExportSetupForm).ShowDialog() == 1)
				{
					StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(Control);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = false;
					}
					StiImageDashboardExportSettings stiImageDashboardExportSettings = (StiImageDashboardExportSettings)stiDashboardImageExportSetupForm.GetExportSettings();
					stiImageDashboardExportSettings.Width = ((StiComponent)element).Width;
					stiImageDashboardExportSettings.Height = ((StiComponent)element).Height;
					if (element is IStiDashboard && stiDashboardViewerControl != null)
					{
						stiImageDashboardExportSettings.Width = (double)(((Control)stiDashboardViewerControl).Width - 1) / StiScale.Factor;
						stiImageDashboardExportSettings.Height = (double)(((Control)stiDashboardViewerControl).Height - 1) / StiScale.Factor;
					}
					stiImageDashboardExportSettings.Width = Math.Max(1.0, stiImageDashboardExportSettings.Width);
					stiImageDashboardExportSettings.Height = Math.Max(1.0, stiImageDashboardExportSettings.Height);
					StiDashboardExportDialog.Save(((ToolStripItem)item).Tag as IStiElement, stiImageDashboardExportSettings);
					if (stiDashboardViewerControl != null)
					{
						((Control)stiDashboardViewerControl).Enabled = true;
					}
				}
			}
			finally
			{
				((IDisposable)stiDashboardImageExportSetupForm)?.Dispose();
			}
		};
		((ToolStrip)ExpressionMenu).Items.Add((ToolStripItem)(object)item);
	}

	public StiActionsMenu(StiContextMenu expressionMenu, Control control, IStiElement element)
	{
		ExpressionMenu = expressionMenu;
		Element = element;
		Control = control;
	}
}
