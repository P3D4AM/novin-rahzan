using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Editor;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Check;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Interaction;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Design.Tools;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Surface.Editor;
using Stimulsoft.Report.Units;
using Stimulsoft.Report.Viewer.Design;

namespace Stimulsoft.Report.Design;

public static class StiActions
{
	public sealed class MainMenu
	{
		public sealed class States
		{
			public static string GetReportFile(StiDesignerControl designer)
			{
				string text = designer.ReportFileName;
				if (!StiOptions.Designer.ShowFullFileNameInTitle && text.IndexOfInvariant("\\") != -1)
				{
					text = text.Substring(text.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
				}
				return StiOptions.Designer.GetDesignerTitleWithMask(text);
			}

			public static bool GetNewEnabled()
			{
				return true;
			}

			public static bool GetInfoEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetReportOpenEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return true;
				}
				return !designer.Report.IsPageDesigner;
			}

			public static bool GetRecentFilesEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return true;
				}
				return !designer.Report.IsPageDesigner;
			}

			public static bool GetPageNewEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetFormNewEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetFormUINewEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetDashboardNewEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetPageOpenEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetSaveAsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetReportSaveEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetReportSaveAsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetPageSaveAsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetPageDeleteEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null && designer.Report.Pages.Count > 1)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetReportPreviewEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetCheckIssuesEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetReportSetupEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetOptionsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetCloseEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}
		}

		private static PointD GetDesignerCenterPoint(StiDesignerControl designer)
		{
			Form val = ((Control)designer).FindForm();
			Point point = ((Control)val).PointToScreen(Point.Empty);
			double system = StiScale.System;
			double x = (double)(point.X + ((Control)val).Width / 2) * system;
			double y = (double)(point.Y + ((Control)val).Height / 2) * system;
			return new PointD(x, y);
		}

		public static async Task InvokeRegNewReportAsync(StiDesignerControl designer, StiReport report)
		{
			_ = 2;
			try
			{
				designer.SaveDesignerParam();
				designer.LockTabControlRefresh = true;
				designer.LockRefreshDesigner = true;
				if (designer.Report != null)
				{
					designer.Report.CurrentPage = 0;
				}
				report.CurrentPage = 0;
				if (designer.Report != null && designer.Report != report)
				{
					StiDictionaryInitializer.Initialize(designer.Report, report);
				}
				if (designer.Report != null)
				{
					report.Tag = designer.Report.Tag;
				}
				await report.Dictionary.SynchronizeAsync();
				bool isModified = report.IsModified;
				string strReport = report.SaveToString();
				if (designer.Report == null)
				{
					await designer.SetReportAsync(StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport);
				}
				designer.Report.LoadFromString(strReport);
				designer.Report.IsModified = isModified;
				designer.Report.ResetSelection();
				designer.Report.Pages[0].Select();
				designer.Report.Designer = designer;
				await StiDictionary.DoAutoSynchronizeAsync(report);
				designer.ReportKey = null;
				designer.ReportNeverSaved = true;
				designer.ReportItemKey = null;
				designer.UndoRedo?.Clear();
				if (StiDesigner.CanDesignerChangeReportFileName)
				{
					designer.ReportFileName = designer.Report.ReportName + ".mrt";
				}
				designer.Builder.BuildReport();
				designer.InvokeBuidingToolbox();
				designer.LockRefreshDesigner = false;
				designer.LockTabControlRefresh = false;
				((Control)designer).Refresh();
				designer.AddSelectedToPropertyGrid();
				designer.InvokeClearingReportCheckerResults();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRegNewReport(StiDesignerControl designer, StiReport report)
		{
			try
			{
				designer.SaveDesignerParam();
				designer.LockTabControlRefresh = true;
				designer.LockRefreshDesigner = true;
				if (designer.Report != null)
				{
					designer.Report.CurrentPage = 0;
				}
				report.CurrentPage = 0;
				if (designer.Report != null && designer.Report != report)
				{
					StiDictionaryInitializer.Initialize(designer.Report, report);
				}
				if (designer.Report != null)
				{
					report.Tag = designer.Report.Tag;
				}
				report.Dictionary.Synchronize();
				bool isModified = report.IsModified;
				string reportStr = report.SaveToString();
				if (designer.Report == null)
				{
					designer.Report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
				}
				designer.Report.LoadFromString(reportStr);
				designer.Report.IsModified = isModified;
				designer.Report.ResetSelection();
				designer.Report.Pages[0].Select();
				designer.Report.Designer = designer;
				StiDictionary.DoAutoSynchronize(report);
				designer.ReportKey = null;
				designer.ReportNeverSaved = true;
				designer.ReportItemKey = null;
				designer.UndoRedo?.Clear();
				if (StiDesigner.CanDesignerChangeReportFileName)
				{
					designer.ReportFileName = designer.Report.ReportName + ".mrt";
				}
				designer.Builder.BuildReport();
				designer.InvokeBuidingToolbox();
				designer.LockRefreshDesigner = false;
				designer.LockTabControlRefresh = false;
				((Control)designer).Refresh();
				designer.AddSelectedToPropertyGrid();
				designer.InvokeClearingReportCheckerResults();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeReportNew(StiDesignerControl designer)
		{
			try
			{
				if (!designer.TestReportSave())
				{
					designer.DontAskSaveReportInDesigner = true;
					if (StiReportNewDialog.CreateNewReport(designer) != null)
					{
						designer.ResetCloudSettings();
					}
					designer.DontAskSaveReportInDesigner = false;
				}
				designer.SetAutoSaveMode();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeReportNewBlank(StiDesignerControl designer)
		{
			try
			{
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
				}
				if (flag)
				{
					return;
				}
				try
				{
					if (!designer.TestReportSave())
					{
						designer.DontAskSaveReportInDesigner = true;
						StiReport report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
						InvokeRegNewReport(designer, report);
						designer.DontAskSaveReportInDesigner = false;
						designer.ResetCloudSettings();
					}
					designer.SetAutoSaveMode();
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
			}
		}

		public static void InvokeReportWizard(StiDesignerControl designer, StiWizardService wizard)
		{
			try
			{
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
				}
				if (flag)
				{
					return;
				}
				try
				{
					if (!designer.TestReportSave())
					{
						designer.DontAskSaveReportInDesigner = true;
						StiReport stiReport = designer.Report;
						if (stiReport == null)
						{
							stiReport = (designer.Report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport);
						}
						StiReport stiReport3 = wizard.CreateReport(stiReport);
						if (stiReport3 != null)
						{
							InvokeRegNewReport(designer, stiReport3);
							designer.DontAskSaveReportInDesigner = false;
							designer.ResetCloudSettings();
						}
					}
					designer.SetAutoSaveMode();
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
			}
		}

		public static void InvokeReportTemplate(StiDesignerControl designer, string reportName)
		{
			StiReportTemplatesService service = StiReportTemplatesService.GetService();
			if (service == null)
			{
				return;
			}
			try
			{
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
				}
				if (flag)
				{
					return;
				}
				try
				{
					if (!designer.TestReportSave())
					{
						designer.DontAskSaveReportInDesigner = true;
						StiReport reportTemplate = service.GetReportTemplate(reportName);
						if (reportTemplate != null)
						{
							InvokeRegNewReport(designer, reportTemplate);
							designer.DontAskSaveReportInDesigner = false;
							designer.ResetCloudSettings();
						}
					}
					designer.SetAutoSaveMode();
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
			}
		}

		public static void InvokePageNew(StiDesignerControl designer)
		{
			InvokePageNew(designer, -1);
		}

		public static void InvokePageNew(StiDesignerControl designer, int index)
		{
			try
			{
				StiInplaceDesigner.Close();
				StiPage stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { designer.Report }) as StiPage;
				if (!RegionInfo.CurrentRegion.IsMetric)
				{
					stiPage.PaperSize = (PaperKind)1;
					stiPage.PageWidth = designer.Report.Unit.ConvertFromHInches(850.0);
					stiPage.PageHeight = designer.Report.Unit.ConvertFromHInches(1100.0);
				}
				designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFilePageNew"));
				if (index == -1)
				{
					index = designer.PageCodeIndex;
					designer.Report.Pages.Add(stiPage);
				}
				else
				{
					stiPage.Name = StiNameCreation.CreateName(designer.Report, StiNameCreation.GenerateName(stiPage));
					designer.Report.Pages.Insert(index, stiPage);
				}
				designer.Builder.BuildReport();
				designer.TabControl.SelectedTabIndex = index;
				designer.Report.CurrentPage = index;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeFormNew(StiDesignerControl designer)
		{
			InvokeFormNew(designer, -1);
		}

		public static void InvokeFormNew(StiDesignerControl designer, int index)
		{
			try
			{
				StiInplaceDesigner.Close();
				StiForm stiForm = new StiForm(designer.Report);
				designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileFormNew"));
				if (index == -1)
				{
					index = designer.PageCodeIndex;
					designer.Report.Pages.Add(stiForm);
				}
				else
				{
					stiForm.Name = StiNameCreation.CreateName(designer.Report, StiNameCreation.GenerateName(stiForm));
					designer.Report.Pages.Insert(index, stiForm);
				}
				stiForm.Text = stiForm.Name;
				designer.Builder.BuildReport();
				designer.TabControl.SelectedTabIndex = index;
				designer.Report.CurrentPage = index;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeDashboardNewBlank(StiDesignerControl designer)
		{
			try
			{
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
				}
				if (flag)
				{
					return;
				}
				try
				{
					if (!designer.TestReportSave())
					{
						designer.DontAskSaveReportInDesigner = true;
						StiReport report = StiReport.CreateNewDashboard();
						InvokeRegNewReport(designer, report);
						designer.DontAskSaveReportInDesigner = false;
						designer.ResetCloudSettings();
						designer.Builder.BuildReport();
						((Control)designer).Refresh();
					}
					designer.SetAutoSaveMode();
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
			}
		}

		public static void InvokeDashboardNew(StiDesignerControl designer)
		{
			InvokeDashboardNew(designer, -1);
		}

		public static void InvokeDashboardNew(StiDesignerControl designer, int index)
		{
			try
			{
				StiInplaceDesigner.Close();
				StiPage stiPage = StiDashboardCreator.CreateDashboard(designer.Report) as StiPage;
				designer.UndoRedoSave(Loc.Get("MainMenu", "menuFileDashboardNew"));
				if (index == -1)
				{
					index = designer.PageCodeIndex;
					designer.Report.Pages.Add(stiPage);
				}
				else
				{
					stiPage.Name = StiNameCreation.CreateName(designer.Report, StiNameCreation.GenerateName(stiPage));
					designer.Report.Pages.Insert(index, stiPage);
				}
				designer.Builder.BuildReport();
				designer.TabControl.SelectedTabIndex = index;
				designer.Report.CurrentPage = index;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeScreenNewBlank(StiDesignerControl designer)
		{
			try
			{
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(designer);
				}
				if (flag)
				{
					return;
				}
				try
				{
					if (!designer.TestReportSave())
					{
						designer.DontAskSaveReportInDesigner = true;
						StiReport report = StiReport.CreateNewScreen();
						InvokeRegNewReport(designer, report);
						designer.DontAskSaveReportInDesigner = false;
						designer.ResetCloudSettings();
						designer.Builder.BuildReport();
						((Control)designer).Refresh();
					}
					designer.SetAutoSaveMode();
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(designer);
			}
		}

		public static void InvokeScreenNew(StiDesignerControl designer)
		{
			InvokeScreenNew(designer, -1);
		}

		public static void InvokeScreenNew(StiDesignerControl designer, int index)
		{
			try
			{
				StiPage stiPage = StiAppCreator.CreateScreen(designer.Report) as StiPage;
				designer.UndoRedoSave(Loc.Get("MainMenu", "menuFileScreenNew"));
				if (index == -1)
				{
					index = designer.PageCodeIndex;
					designer.Report.Pages.Add(stiPage);
				}
				else
				{
					stiPage.Name = StiNameCreation.CreateName(designer.Report, StiNameCreation.GenerateName(stiPage));
					designer.Report.Pages.Insert(index, stiPage);
				}
				designer.TabControl.SelectedTabIndex = index;
				designer.Report.CurrentPage = index;
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static async Task<bool> InvokeReportOpenAsync(StiDesignerControl designer)
		{
			try
			{
				bool flag = await StiReportOpenDialog.OpenReportAsync(designer);
				designer.SetAutoSaveMode();
				if (flag)
				{
					designer.ResetCloudSettings();
				}
				return flag;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			return false;
		}

		public static bool InvokeReportOpen(StiDesignerControl designer)
		{
			try
			{
				bool flag = StiReportOpenDialog.OpenReport(designer);
				designer.SetAutoSaveMode();
				if (flag)
				{
					designer.ResetCloudSettings();
				}
				return flag;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			return false;
		}

		[Obsolete("Method 'InvokeReportOpenFromGoogleDocs' is not used more!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool InvokeReportOpenFromGoogleDocs(StiDesignerControl designer)
		{
			return false;
		}

		public static void InvokePageOpen(StiDesignerControl designer)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Invalid comparison between Unknown and I4
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (StiOptions.Engine.GlobalEvents.InvokeLoadingPageInDesigner(designer))
				{
					return;
				}
				OpenFileDialog val = new OpenFileDialog();
				try
				{
					StiSettings.Load();
					string text = StiOptions.Designer.PageSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						text = StiSettings.GetStr("StiDesigner", "PageSaveLoadPath", string.Empty);
					}
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						((FileDialog)val).InitialDirectory = text;
					}
					((FileDialog)val).Filter = StiSLService.GetPageFilters(StiSLActions.Load);
					((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageOpen"));
					((FileDialog)val).RestoreDirectory = true;
					if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
					{
						return;
					}
					text = StiOptions.Designer.PageSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						StiSettings.Load();
						StiSettings.Set("StiDesigner", "PageSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
						StiSettings.Save();
					}
					designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFilePageOpen"));
					StiPage stiPage = null;
					string text2 = null;
					switch (StiPageTypeHelper.GetPageTypeFromFile(((FileDialog)val).FileName))
					{
					case "StiForm":
					{
						StiDialogsProvider provider = StiDialogsProvider.GetProvider(designer.Report);
						if (provider == null)
						{
							string text3 = "Designer can't load form from file because it can't find service StiDialogsProvider or assembly 'Stimulsoft.Dialogs'!";
							MessageBox.Show(text3);
							throw new Exception(text3);
						}
						IStiForm stiForm = provider.CreateForm(designer.Report);
						stiPage = provider.CreateForm(designer.Report) as StiPage;
						text2 = "Form";
						break;
					}
					case "StiScreen":
						stiPage = StiAppCreator.CreateScreen(designer.Report) as StiPage;
						text2 = "Screen";
						break;
					case "StiDashboard":
						stiPage = StiDashboardCreator.CreateDashboard(designer.Report) as StiPage;
						text2 = "Dashboard";
						break;
					default:
						stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage;
						text2 = "Page";
						break;
					}
					stiPage.Report = designer.Report;
					designer.Report.Pages.Add(stiPage);
					CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
					FileStream fileStream = null;
					try
					{
						fileStream = new FileStream(((FileDialog)val).FileName, FileMode.Open, FileAccess.Read);
						Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
						StiPageSLService service = (StiReport.IsJsonFile(fileStream) ? ((StiPageSLService)new StiJsonPageSLService()) : ((StiPageSLService)new StiXmlPageSLService()));
						stiPage.Load(service, ((FileDialog)val).FileName);
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
						fileStream.Close();
					}
					stiPage.Components.SortByPriority();
					designer.Report.Pages.Remove(stiPage);
					StiPage stiPage2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { designer.Report }) as StiPage;
					stiPage2.Name = "#%#TempPageForStoreComponents#%#";
					designer.Report.Pages.Add(stiPage2);
					StiComponentsCollection components = new StiComponentsCollection();
					stiPage2.Components = components;
					stiPage.Name = StiNameCreation.CreateName(designer.Report, text2, addOne: true, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
					StiComponentsCollection components2 = stiPage.GetComponents();
					foreach (StiComponent item in components2)
					{
						if (!StiNameCreation.IsValidName(designer.Report, item.Name))
						{
							item.Name = StiNameCreation.CreateName(designer.Report, StiNameCreation.GenerateName(item));
						}
						stiPage2.Components.Add(item);
					}
					designer.Report.Pages.Remove(stiPage2);
					designer.Report.Pages.Add(stiPage);
					if (stiPage.IsPage && stiPage.ReportUnit != null)
					{
						stiPage.Convert(stiPage.ReportUnit, stiPage.Report.Unit);
						stiPage.ReportUnit = null;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)designer).GetType(), "Loading page...ERROR");
				StiLogService.Write(((object)designer).GetType(), ex);
				StiExceptionProvider.Show(ex);
			}
			finally
			{
				designer.Builder.BuildReport();
			}
		}

		public static bool InvokeReportSave(StiDesignerControl designer)
		{
			if (designer != null && designer.SaveAsToSilverlight)
			{
				designer.Report.SaveReportSourceCodeForSilverlight(designer.ReportFileName);
				return true;
			}
			return InvokeInternalReportSave(designer);
		}

		private static bool InvokeInternalReportSave(StiDesignerControl designer)
		{
			try
			{
				InvokeSaveReportScript(designer);
				((Control)designer.TabControl).Focus();
				if ((StiFileUtils.IsReadOnly(designer.ReportFileName) || designer.ReportNeverSaved) && !StiOptions.Designer.DontAskSaveReport && !StiOptions.Designer.IgnoreOptionReportNeverSaved)
				{
					return InvokeReportSaveAs(designer, StiSaveEventSource.Save);
				}
				((Control)designer).Refresh();
				designer.Report.Dictionary.Report = designer.Report;
				if (!StiOptions.Designer.SaveReportWhenDontAskSaveReport && StiOptions.Designer.DontAskSaveReport)
				{
					return true;
				}
				if (StiAccountSettings.CurrentAccount != null && !string.IsNullOrEmpty(designer.ReportItemKey))
				{
					StiDashboardRecentHelper.Add(designer.Report, designer.ReportItemKey);
					if (designer.ReportFileName.Length > 4 && designer.ReportFileName.EndsWith(".mrx"))
					{
						StiAccountSettings.CurrentAccount.Actions.SaveEncryptedReportAsync(designer.ReportItemKey, designer.Report, designer.ReportKey, ((Control)designer).Handle);
					}
					else
					{
						StiAccountSettings.CurrentAccount.Actions.SaveReportAsync(designer.ReportItemKey, designer.Report, ((Control)designer).Handle);
					}
				}
				else if (!StiOptions.Engine.GlobalEvents.InvokeSavingReportInDesigner(designer, saveAs: false, StiSaveEventSource.Save))
				{
					StiDashboardRecentHelper.Add(designer.Report, designer.ReportFileName);
					if (designer.ReportFileName.Length > 4 && designer.ReportFileName.EndsWith(".mrx"))
					{
						string key = ((designer.Report.Password != null) ? designer.Report.Password : designer.ReportKey);
						designer.Report.SaveEncryptedReport(designer.ReportFileName, key);
					}
					else
					{
						designer.Report.ReportFile = designer.ReportFileName;
						if (designer.ReportFileName.EndsWith(".mrt"))
						{
							if (designer.Report.IsJsonReport)
							{
								designer.Report.SaveToJson(designer.ReportFileName);
							}
							else
							{
								designer.Report.Save(designer.ReportFileName);
							}
						}
						else if (designer.ReportFileName.EndsWith(".mrx"))
						{
							string key2 = ((!string.IsNullOrEmpty(designer.ReportKey)) ? designer.ReportKey : designer.Report.Password);
							designer.Report.SaveEncryptedReport(designer.ReportFileName, key2);
						}
						else if (designer.ReportFileName.EndsWith(".mrz"))
						{
							designer.Report.SavePackedReport(designer.ReportFileName);
						}
						if (designer.Report.Info.AutoSaveReportToReportClass)
						{
							designer.Report.SaveReportSourceCode(Path.ChangeExtension(designer.ReportFileName, designer.Report.Language.GetProvider().FileExtension));
						}
					}
					if (StiDesigner.CanDesignerChangeReportFileName)
					{
						designer.AddRecentFile(designer.ReportFileName, null);
					}
					StiOptions.Engine.GlobalEvents.InvokeSavedReportInDesigner(designer);
				}
				designer.ReportNeverSaved = false;
				designer.Report.IsModified = false;
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)designer).GetType(), "Saving report...ERROR");
				StiLogService.Write(((object)designer).GetType(), ex);
				StiExceptionProvider.Show(ex);
			}
			finally
			{
				((Control)designer).Refresh();
			}
			return true;
		}

		private static void InvokeSaveReportScript(StiDesignerControl designer)
		{
			StiDesignerControl stiDesignerControl = designer.DesignerControl as StiDesignerControl;
			if (stiDesignerControl?.Builder != null)
			{
				StiDesignerCodeControl codeEditor = stiDesignerControl.Builder.GetCodeEditor();
				if (codeEditor != null && codeEditor.CodeEditor != null && stiDesignerControl.TabControl.SelectedTabIndex == stiDesignerControl.PageCodeIndex)
				{
					codeEditor.CodeEditor.SaveScript();
				}
			}
		}

		public static bool InvokeReportSaveAs(StiDesignerControl designer)
		{
			return InvokeReportSaveAs(designer, StiSaveEventSource.SaveAs);
		}

		private static bool InvokeReportSaveAs(StiDesignerControl designer, StiSaveEventSource eventSource)
		{
			try
			{
				InvokeSaveReportScript(designer);
				((Control)designer.TabControl).Focus();
				bool flag = StiReportSaveDialog.SaveReport(designer, eventSource);
				if (flag)
				{
					designer.ResetCloudSettings();
				}
				return flag;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			return false;
		}

		[Obsolete("Method 'InvokeReportSaveAsToGoogleDocs' is not used more!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool InvokeReportSaveAsToGoogleDocs(StiDesignerControl designer)
		{
			return false;
		}

		public static void InvokePageSaveAs(StiDesignerControl designer)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Invalid comparison between Unknown and I4
			try
			{
				if (StiOptions.Engine.GlobalEvents.InvokeSavingPageInDesigner(designer))
				{
					return;
				}
				SaveFileDialog val = new SaveFileDialog();
				try
				{
					StiSettings.Load();
					string text = StiOptions.Designer.PageSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						text = StiSettings.GetStr("StiDesigner", "PageSaveLoadPath", string.Empty);
					}
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						((FileDialog)val).InitialDirectory = text;
					}
					StiPage currentPage = designer.Report.GetCurrentPage();
					((FileDialog)val).Filter = StiSLService.GetPageFilters(StiSLActions.Save);
					((FileDialog)val).RestoreDirectory = true;
					((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageSaveAs"));
					((FileDialog)val).DefaultExt = "pg";
					if (currentPage.IsDashboard)
					{
						((FileDialog)val).FileName = StiLocalization.Get("Components", "StiDashboard");
					}
					else if (currentPage.IsForm)
					{
						((FileDialog)val).FileName = StiLocalization.Get("Dialogs", "StiForm");
					}
					else if (currentPage.IsScreen)
					{
						((FileDialog)val).FileName = StiLocalization.Get("Components", "StiScreen");
					}
					else
					{
						((FileDialog)val).FileName = StiLocalization.Get("Components", "StiPage");
					}
					if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
					{
						text = StiOptions.Designer.PageSaveLoadPath;
						if (string.IsNullOrEmpty(text))
						{
							StiSettings.Load();
							StiSettings.Set("StiDesigner", "PageSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
							StiSettings.Save();
						}
						((Control)designer).Refresh();
						currentPage.ReportUnit = currentPage.Unit;
						currentPage.Save(StiSLService.GetPageSLServices(StiSLActions.Save)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
						currentPage.ReportUnit = null;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)designer).GetType(), "Saving page...ERROR");
				StiLogService.Write(((object)designer).GetType(), ex);
				StiExceptionProvider.Show(ex);
			}
		}

		public static void InvokePageDelete(StiDesignerControl designer)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			try
			{
				if (designer.Report.Pages.Count <= 1 || (int)MessageBox.Show(StiLocalization.Get("Questions", "qnPageDelete"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4) != 6)
				{
					return;
				}
				designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFilePageDelete"));
				int selectedTabIndex = designer.TabControl.SelectedTabIndex;
				if (selectedTabIndex > 0)
				{
					designer.TabControl.SelectedTabIndex--;
				}
				designer.Report.Pages.RemoveAt(selectedTabIndex);
				int num = 0;
				int num2 = 0;
				while (num < designer.Report.Pages.Count)
				{
					if (num == selectedTabIndex)
					{
						num2++;
					}
					designer.TabControl.Tabs[num2].Tag = num;
					num++;
					num2++;
				}
				designer.TabControl.Tabs.RemoveAt(selectedTabIndex);
				designer.Builder.BuildReport();
				designer.InvokeBuidingToolbox();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeReportQuickPrint(StiDesignerControl designer)
		{
			InvokeReportPreview(designer, isPreview: false, isQuickPrint: true, isPrint: false);
		}

		public static void InvokeReportPrint(StiDesignerControl designer)
		{
			InvokeReportPreview(designer, isPreview: false, isQuickPrint: false, isPrint: true);
		}

		public static void InvokeReportPreview(StiDesignerControl designer)
		{
			InvokeReportPreview(designer, isPreview: true, isQuickPrint: false, isPrint: false);
		}

		public static void InvokeReportPreview(StiDesignerControl designer, bool isPreview, bool isQuickPrint, bool isPrint)
		{
			InvokeReportPreview(designer, isPreview, isQuickPrint, isPrint, isStardardPreviewOnly: true);
		}

		public static void InvokeReportPreview(StiDesignerControl designer, bool isPreview, bool isQuickPrint, bool isPrint, bool isStardardPreviewOnly)
		{
			if (designer.LockReportPreview || designer.Report == null)
			{
				return;
			}
			Windows7Taskbar.ShowTaskbarProgress(designer.Report, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
			try
			{
				designer.LockReportPreview = true;
				((Control)designer).Refresh();
				designer.Report.ResetRenderedState();
				designer.InvokeClearingCompilerResults();
				designer.CheckHelper.BuildChecks(designer.Report);
				if (designer.CheckHelper.ErrorsCount > 0)
				{
					designer.InvokeShowingCompilerResults(designer.Report.CompilerResults);
				}
				if (designer.CheckHelper.ErrorsCount == 0)
				{
					if (designer.Report.CompiledReport != null)
					{
						InvokeReportPreviewWithCompilation(designer, isPreview, isQuickPrint, isPrint, isStardardPreviewOnly);
					}
					else if (designer.Report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
					{
						InvokeReportPreviewWithInterpretation(designer, isPreview, isQuickPrint, isPrint, isStardardPreviewOnly);
					}
				}
				if (designer.CheckHelper.IsMessagesPresent)
				{
					designer.IsReportPreviewMode = false;
					StiCheckStatusEventArgs e = new StiCheckStatusEventArgs(designer.CheckHelper.ErrorsCount, designer.CheckHelper.WarningsCount, designer.CheckHelper.InformationMessagesCount, designer.CheckHelper.ReportRenderingMessagesCount);
					designer.InvokeShowingReportCheckerResults(e);
				}
				else
				{
					designer.InvokeClearingReportCheckerResults();
				}
			}
			catch (Exception ex)
			{
				StiLogService.Write(typeof(StiDesignerControl), ex);
				if (!(ex is StiReportRenderingStopException))
				{
					StiExceptionProvider.Show(ex);
				}
			}
			finally
			{
				if (!StiDashboardHelper.IsDashboardPresent(designer.Report))
				{
					designer.Report.Dictionary.Connect(loadData: false);
				}
				designer.Report.CompiledReport = null;
				((Control)designer).Refresh();
				designer.LockReportPreview = false;
			}
			Windows7Taskbar.HideTaskbarProgress();
		}

		private static void InvokeReportPreviewWithCompilation(StiDesignerControl designer, bool isPreview, bool isQuickPrint, bool isPrint, bool isStardardPreviewOnly)
		{
			StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(designer);
			if (StiDashboardHelper.IsDashboardPresent(designer.Report.CompiledReport))
			{
				designer.CheckHelper?.BuildReportRenderingMessages(designer.Report);
				designer.Report.CompiledReport.Key = StiKeyHelper.GenerateKey();
				StiDashboardViewerAssembly.Show(designer.Report.CompiledReport);
				Application.DoEvents();
				return;
			}
			designer.Report.Render(showProgress: true);
			designer.CheckHelper?.BuildReportRenderingMessages(designer.Report);
			if (designer.Report.IsRendered)
			{
				if (isPreview)
				{
					if (!isStardardPreviewOnly)
					{
						if (((ContainerControl)designer).ParentForm != null && StiDesigner.ShowPreviewInMdi)
						{
							designer.Report.ShowWithRibbonGUI(((ContainerControl)designer).ParentForm);
						}
						else
						{
							designer.Report.ShowWithRibbonGUI();
						}
					}
					else if (((ContainerControl)designer).ParentForm != null && StiDesigner.ShowPreviewInMdi)
					{
						designer.Report.Show(((ContainerControl)designer).ParentForm);
					}
					else
					{
						designer.Report.Show();
					}
				}
				else if (isQuickPrint)
				{
					designer.Report.Print(showPrintDialog: false);
				}
				else if (isPrint)
				{
					designer.Report.Print(showPrintDialog: true);
				}
			}
			if (StiOptions.Engine.ShowReportRenderingMessages)
			{
				designer?.InvokeShowingReportRenderingMessages();
			}
		}

		private static void InvokeReportPreviewWithInterpretation(StiDesignerControl designer, bool isPreview, bool isQuickPrint, bool isPrint, bool isStardardPreviewOnly)
		{
			StiReport stiReport = StiReportCopyHelper.Copy(designer.Report);
			stiReport.Key = StiKeyHelper.GenerateKey();
			StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(designer);
			if (StiDashboardHelper.IsDashboardPresent(stiReport))
			{
				StiDashboardViewerAssembly.Show(stiReport);
				Application.DoEvents();
			}
			else if (isPreview)
			{
				if (!isStardardPreviewOnly)
				{
					if (((ContainerControl)designer).ParentForm != null && StiDesigner.ShowPreviewInMdi)
					{
						stiReport.ShowWithRibbonGUI(((ContainerControl)designer).ParentForm);
					}
					else
					{
						stiReport.ShowWithRibbonGUI();
					}
				}
				else if (((ContainerControl)designer).ParentForm != null && StiDesigner.ShowPreviewInMdi)
				{
					stiReport.Show(((ContainerControl)designer).ParentForm);
				}
				else
				{
					stiReport.Show();
				}
			}
			else if (isQuickPrint)
			{
				stiReport.Print(showPrintDialog: false);
			}
			else if (isPrint)
			{
				stiReport.Print(showPrintDialog: true);
			}
			if (StiOptions.Engine.ShowReportRenderingMessages)
			{
				designer?.InvokeShowingReportRenderingMessages();
			}
		}

		public static void InvokeReportStandardGuiPreview(StiDesignerControl designer)
		{
			InvokeReportPreview(designer, isPreview: true, isQuickPrint: false, isPrint: false, isStardardPreviewOnly: true);
		}

		public static void InvokePageSetup(StiDesignerControl designer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiComponentDesigner.GetComponentDesigner(designer, designer.Report.GetCurrentPage().GetType())?.Design(designer.Report.GetCurrentPage());
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeReportSetup(StiDesignerControl designer)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiReportSetupForm stiReportSetupForm = new StiReportSetupForm(designer);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiReportSetupForm);
					((Form)stiReportSetupForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiReportSetupForm)?.Dispose();
				}
				designer.Builder.BuildReport(selectedObject: false);
				designer.InvokeBuidingToolbox();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeCheckIssues(StiDesignerControl designer)
		{
			try
			{
				StiReportChecker.RunReportChecker(designer.Report, checkKeyEscape: true);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeGlobalizationStringsEditor(StiDesignerControl designer)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				designer?.UndoRedoSave(StiLocalization.Get("FormGlobalizationEditor", "title"));
				StiGlobalizationEditorForm stiGlobalizationEditorForm = new StiGlobalizationEditorForm(designer.Report);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiGlobalizationEditorForm);
					((Form)stiGlobalizationEditorForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiGlobalizationEditorForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokePreviewSettingsEditor(StiDesignerControl designer)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
				stiReport.PreviewSettings = designer.Report.PreviewSettings;
				stiReport.DashboardViewerSettings = designer.Report.DashboardViewerSettings;
				designer.UndoRedoSave(StiLocalization.Get("FormViewer", "titlePageSettings"));
				StiPreviewSettingsEditorForm.StartTab startTab = (stiReport.GetCurrentPage().IsDashboard ? StiPreviewSettingsEditorForm.StartTab.Dashboard : StiPreviewSettingsEditorForm.StartTab.Report);
				StiPreviewSettingsEditorForm stiPreviewSettingsEditorForm = new StiPreviewSettingsEditorForm(stiReport, startTab);
				try
				{
					designer.AddOwnedForm((Form)(object)stiPreviewSettingsEditorForm);
					((Form)stiPreviewSettingsEditorForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiPreviewSettingsEditorForm)?.Dispose();
				}
				designer.Report.PreviewSettings = stiReport.PreviewSettings;
				designer.Report.DashboardViewerSettings = stiReport.DashboardViewerSettings;
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static async Task InvokeRecentFileAsync(StiDesignerControl designer, StiRecentFile recentFile)
		{
			if (!StiOptions.Engine.GlobalEvents.InvokeOpenRecentFileInDesigner(designer, recentFile))
			{
				try
				{
					if (!designer.TestReportSave())
					{
						StiCacheCleaner.Clean(designer.Report);
						if (recentFile.FileName.StartsWith("cloud"))
						{
							await designer.OpenRecentCloudFileAsync(recentFile);
						}
						else
						{
							await designer.OpenFileAsync(recentFile.FileName);
							designer.ResetCloudSettings();
						}
						designer.InvokeClearingReportCheckerResults();
					}
				}
				catch (Exception ex)
				{
					StiLogService.Write(((object)designer).GetType(), "Open report...ERROR");
					StiLogService.Write(((object)designer).GetType(), ex);
					StiExceptionProvider.Show(ex);
				}
			}
			designer.SetAutoSaveMode();
		}

		public static void InvokeShare(StiDesignerControl designer)
		{
			try
			{
				IStiCloudShareWindow cloudShareWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetCloudShareWindow(designer.Report, designer.ReportItemKey);
				cloudShareWindow.ShowDialog(((Control)designer).Handle);
				if (!cloudShareWindow.IsReportSaveCloud || string.IsNullOrEmpty(cloudShareWindow.ReportItemKey))
				{
					return;
				}
				StiReport stiReport = cloudShareWindow.Report as StiReport;
				if (!string.IsNullOrEmpty(cloudShareWindow.ReportItemKey) && stiReport != null)
				{
					if (StiOptions.Designer.CanDesignerChangeReportFileName)
					{
						string file = "cloud\\" + cloudShareWindow.ReportItemKey + "\\" + stiReport.ReportFile;
						designer.AddRecentFile(file, cloudShareWindow.ReportItemKey);
					}
					designer.ReportItemKey = cloudShareWindow.ReportItemKey;
					designer.Report = stiReport;
					designer.ReportFileName = stiReport.ReportFile;
					designer.ReportNeverSaved = false;
					if (!designer.RibbonDesignerUserControl.RibbonTabPreview.Checked)
					{
						designer.CloseBackStage();
					}
					if (designer?.Report != null)
					{
						designer.Builder.BuildReport();
					}
				}
			}
			catch
			{
			}
		}

		public static void InvokePublish(StiDesignerControl designer)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				IStiPublishWindow publishWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetPublishWindow(designer.Report);
				publishWindow.ShowDialog(((Control)designer).Handle, GetDesignerCenterPoint(designer));
				if (publishWindow.IsNeedToCheck)
				{
					StiReportChecker.RunReportChecker(designer.Report, checkKeyEscape: true);
				}
			}
		}

		public static void InvokeSkillLevel(StiDesignerControl designer)
		{
			if (StiAccountSettings.CurrentAccount?.User == null)
			{
				return;
			}
			Form val = ((Control)designer).FindForm();
			Point point = ((Control)val).PointToScreen(Point.Empty);
			double system = StiScale.System;
			double x = (double)(point.X + ((Control)val).Width / 2) * system;
			double y = (double)(point.Y + ((Control)val).Height / 2) * system;
			IStiChooseSkillLevelWindow chooseSkillLevelWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetChooseSkillLevelWindow(StiAccountSettings.CurrentAccount.User.DesignerSpecification);
			if (chooseSkillLevelWindow.ShowDialog(((Control)designer).Handle, new PointD(x, y)).GetValueOrDefault() && chooseSkillLevelWindow.DesignerSpecification.HasValue)
			{
				if (StiAccountSettings.CurrentAccount.User != null)
				{
					StiAccountSettings.CurrentAccount.User.DesignerSpecification = chooseSkillLevelWindow.DesignerSpecification.GetValueOrDefault();
					StiAccountSettings.CurrentAccount.User.SaveToFile();
				}
				object[] selectedObjects = designer.SelectedObjects;
				designer.SelectedObject = null;
				designer.SelectedObjects = selectedObjects;
				designer.InvokeSelectedObjectsChanged();
			}
		}

		public static void InvokeTrialDaysLeft(StiDesignerControl designer)
		{
			if (StiAccountSettings.CurrentAccount?.User != null)
			{
				Form val = ((Control)designer).FindForm();
				Point point = ((Control)val).PointToScreen(Point.Empty);
				double system = StiScale.System;
				double x = (double)(point.X + ((Control)val).Width / 2) * system;
				double y = (double)(point.Y + ((Control)val).Height / 2) * system;
				IStiTrialDaysLeftWindow trialDaysLeftWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetTrialDaysLeftWindow();
				if (trialDaysLeftWindow.ShowDialog(((Control)designer).Handle, new PointD(x, y)).GetValueOrDefault())
				{
					object[] selectedObjects = designer.SelectedObjects;
					designer.SelectedObject = null;
					designer.SelectedObjects = selectedObjects;
					designer.InvokeSelectedObjectsChanged();
				}
			}
		}

		public static void InvokeGetStarted(StiDesignerControl designer)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				IStiGetStartedWindow startedWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetStartedWindow();
				startedWindow.ShowDialog(((Control)designer).Handle, GetDesignerCenterPoint(designer));
			}
		}

		public static void InvokeClose(StiDesignerControl designer)
		{
			try
			{
				if (!designer.TestReportSave())
				{
					StiCacheCleaner.Clean(designer?.Report);
					designer.Report.Dictionary.Clear();
					designer.Report.Dictionary.Databases.Clear();
					designer.BuildDictionary();
					designer.Report = null;
					designer.TabControl?.Tabs.Clear();
					designer.Builder?.BuildReport();
					designer.SelectedObject = null;
					designer.InvokeSelectedObjectsChanged();
					((Control)designer).Refresh();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeExit(StiDesignerControl designer)
		{
			try
			{
				Form obj = ((Control)designer).FindForm();
				if (obj != null)
				{
					obj.Close();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeDataStore(StiDesignerControl designer)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiDataStoreViewerForm stiDataStoreViewerForm = new StiDataStoreViewerForm(designer.Report.Dictionary);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiDataStoreViewerForm);
					((Form)stiDataStoreViewerForm).ShowDialog();
					((Control)designer).Refresh();
				}
				finally
				{
					((IDisposable)stiDataStoreViewerForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokePagesManager(StiDesignerControl designer)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuToolsPagesManager"));
				StiPagesManagerForm stiPagesManagerForm = new StiPagesManagerForm(designer);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiPagesManagerForm);
					((Form)stiPagesManagerForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiPagesManagerForm)?.Dispose();
				}
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeServicesConfigurator(StiDesignerControl designer)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiServicesForm stiServicesForm = new StiServicesForm(designer, loadServices: true);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiServicesForm);
					((Form)stiServicesForm).ShowDialog();
					designer.InvokeBuidingToolbox();
					((Control)designer).Refresh();
				}
				finally
				{
					((IDisposable)stiServicesForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeOptions(StiDesignerControl designer)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiOptionsForm stiOptionsForm = new StiOptionsForm(designer);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiOptionsForm);
					((Form)stiOptionsForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiOptionsForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAbout(StiDesignerControl designer)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiMainMenuService service = StiMainMenuService.GetService(designer);
				if (service == null)
				{
					string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
					StiLogService.Write(((object)designer).GetType(), text);
					MessageBox.Show(text);
					return;
				}
				if (service.AboutForm == null)
				{
					service.AboutForm = (Form)(object)new StiAboutForm();
					designer.AddOwnedForm(service.AboutForm);
				}
				service.AboutForm.ShowDialog();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeProductHomePage(StiDesignerControl designer)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiMainMenuService service = StiMainMenuService.GetService(designer);
				if (service == null)
				{
					string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
					StiLogService.Write(((object)designer).GetType(), text);
					MessageBox.Show(text);
				}
				else
				{
					StiProcess.Start(service.ProductHomePage);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSupport(StiDesignerControl designer)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiMainMenuService service = StiMainMenuService.GetService(designer);
				if (service == null)
				{
					string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
					StiLogService.Write(((object)designer).GetType(), text);
					MessageBox.Show(text);
				}
				else
				{
					StiProcess.Start(service.Support);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSupportPage(StiDesignerControl designer)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiMainMenuService service = StiMainMenuService.GetService(designer);
				if (service == null)
				{
					string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
					StiLogService.Write(((object)designer).GetType(), text);
					MessageBox.Show(text);
				}
				else
				{
					StiProcess.Start(service.SupportPage);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeFAQPage(StiDesignerControl designer)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiMainMenuService service = StiMainMenuService.GetService(designer);
				if (service == null)
				{
					string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
					StiLogService.Write(((object)designer).GetType(), text);
					MessageBox.Show(text);
				}
				else
				{
					StiProcess.Start(service.FaqPage);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeHowToRegister(StiDesignerControl designer)
		{
			try
			{
				StiProcess.Start("http://www.stimulsoft.com/Purchase.aspx");
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class Clipboard
	{
		public sealed class States
		{
			public static bool CanCut(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.SelectedComponentsOnPage.Count <= 0 || designer.IsPropertiesPanelFocused)
				{
					return false;
				}
				StiAllowClipboardOperationEventArgs stiAllowClipboardOperationEventArgs = new StiAllowClipboardOperationEventArgs(designer.SelectedComponentsOnPage);
				StiOptions.Engine.GlobalEvents.InvokeAllowCutOperation(designer, stiAllowClipboardOperationEventArgs);
				return stiAllowClipboardOperationEventArgs.Allow;
			}

			public static bool CanCopy(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage == null || currentPage.GetSelectedComponents().Count <= 0 || designer.IsPropertiesPanelFocused)
				{
					return false;
				}
				StiAllowClipboardOperationEventArgs stiAllowClipboardOperationEventArgs = new StiAllowClipboardOperationEventArgs(designer.SelectedComponentsOnPage);
				StiOptions.Engine.GlobalEvents.InvokeAllowCopyOperation(designer, stiAllowClipboardOperationEventArgs);
				return stiAllowClipboardOperationEventArgs.Allow;
			}

			public static bool CanDelete(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				bool flag = true;
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (item.AllowDelete)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
				if (designer.SelectedComponentsOnPage.Count > 0)
				{
					return !designer.IsPropertiesPanelFocused;
				}
				return false;
			}

			public static bool CanPaste(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (StiClipboard.Get() == null || designer.IsPropertiesPanelFocused)
				{
					return false;
				}
				StiAllowClipboardOperationEventArgs stiAllowClipboardOperationEventArgs = new StiAllowClipboardOperationEventArgs(designer.SelectedComponentsOnPage);
				StiOptions.Engine.GlobalEvents.InvokeAllowPasteOperation(designer, stiAllowClipboardOperationEventArgs);
				return stiAllowClipboardOperationEventArgs.Allow;
			}

			public static bool CanUndo(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.UndoRedo == null)
				{
					return false;
				}
				if (designer.UndoRedo.CanUndo)
				{
					return !designer.IsPropertiesPanelFocused;
				}
				return false;
			}

			public static bool CanRedo(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.UndoRedo == null)
				{
					return false;
				}
				if (designer.UndoRedo.CanRedo)
				{
					return !designer.IsPropertiesPanelFocused;
				}
				return false;
			}

			public static bool CanSelectAll(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.ComponentsOnPage.Count > 0)
				{
					return !designer.IsPropertiesPanelFocused;
				}
				return false;
			}

			public static bool IsEditorActivated(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.InplaceTextEditorActivated;
			}

			public static bool GetClipboardEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.TabControl.SelectedTabIndex < designer.PageCodeIndex)
				{
					return designer.PageView.GetCurrentPageView() != null;
				}
				return false;
			}

			public static bool GetUndoRedoEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.TabControl.SelectedTabIndex < designer.PageCodeIndex;
			}

			public static string GetUndoText(StiDesignerControl designer)
			{
				if (designer.UndoRedo == null || !designer.UndoRedo.CanUndo)
				{
					return Loc.Get("MainMenu", "menuEditCantUndo");
				}
				return Loc.Get("MainMenu", "menuEditUndo") + " " + designer.UndoRedo.UndoText;
			}

			public static string GetRedoText(StiDesignerControl designer)
			{
				if (designer.UndoRedo == null || !designer.UndoRedo.CanRedo)
				{
					return Loc.Get("MainMenu", "menuEditCantUndo");
				}
				return Loc.Get("MainMenu", "menuEditRedo") + " " + designer.UndoRedo.UndoText;
			}
		}

		public static void InvokeUndo(StiDesignerControl designer, bool closeAllInplaceDesigners = true)
		{
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!States.CanUndo(designer) || designer.TabControl.SelectedTabIndex == designer.PageCodeIndex)
				{
					return;
				}
				designer.LockTabControlRefresh = true;
				designer.LockRefreshDesigner = true;
				int count = designer.Report.Pages.Count;
				StiDataCollection dataStore = designer.Report.Dictionary.DataStore;
				StiUndoRedo stiUndoRedo = designer.UndoRedo.Undo(designer.Report, designer);
				int num = stiUndoRedo.CurrentPage;
				object obj = null;
				if (stiUndoRedo.PageIndex == -1)
				{
					StiComponentsCollection components = designer.Report.GetComponents();
					foreach (StiComponent item in components)
					{
						item.PackService();
						item.Parent = null;
						item.Page = null;
						if (item is StiContainer)
						{
							((StiContainer)item).Components.Clear();
						}
					}
					designer.Report.Pages.Clear();
					designer.Report.IsSerializing = true;
					try
					{
						obj = StiUndoRedo.CreateObjectFromUR(stiUndoRedo, designer.Report);
					}
					finally
					{
						designer.Report.IsSerializing = false;
					}
				}
				else
				{
					if (stiUndoRedo.PageType != StiPageType.Form)
					{
						obj = ((stiUndoRedo.PageType == StiPageType.Screen) ? ((object)StiAppCreator.CreateScreen(designer.Report)) : ((object)((stiUndoRedo.PageType != StiPageType.Dashboard) ? ((IStiReportComponent)(StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage)) : ((IStiReportComponent)StiDashboardCreator.CreateDashboard(designer.Report)))));
					}
					else
					{
						StiDialogsProvider provider = StiDialogsProvider.GetProvider(designer.Report);
						if (provider == null)
						{
							string text = "Designer can't restore form from undo-redo buffer because it can't find service StiDialogsProvider or assembly 'Stimulsoft.Dialogs'!";
							MessageBox.Show(text);
							throw new Exception(text);
						}
						obj = provider.CreateForm(designer.Report);
					}
					designer.Report.IsSerializing = true;
					try
					{
						obj = StiUndoRedo.CreateObjectFromUR(stiUndoRedo, obj);
					}
					finally
					{
						designer.Report.IsSerializing = false;
					}
					StiComponentsCollection components2 = designer.Report.Pages[stiUndoRedo.PageIndex].GetComponents();
					foreach (StiComponent item2 in components2)
					{
						item2.PackService();
						item2.Parent = null;
						item2.Page = null;
						if (item2 is StiContainer)
						{
							((StiContainer)item2).Components.Clear();
						}
					}
				}
				if (stiUndoRedo.PageIndex == -1)
				{
					designer.Report = obj as StiReport;
					designer.Report.Dictionary.DataStore = dataStore;
					designer.Report.Dictionary.Connect(loadData: false);
				}
				else
				{
					designer.Report.Pages[stiUndoRedo.PageIndex] = obj as StiPage;
				}
				foreach (StiPage page in designer.Report.Pages)
				{
					page.Report = designer.Report;
					page.Components.SetParent(page);
					page.GetComponents().ToList().ForEach(delegate(StiComponent c)
					{
						c.Page = page;
					});
				}
				if (num < 0)
				{
					num = 0;
				}
				if (num >= designer.Report.Pages.Count)
				{
					num = designer.Report.Pages.Count - 1;
				}
				designer.Report.CurrentPage = num;
				designer.Report.ApplyStyles();
				if (count != designer.Report.Pages.Count)
				{
					designer.Builder.BuildReport(selectedObject: false);
				}
				else
				{
					if (closeAllInplaceDesigners)
					{
						designer.Builder.RefreshPages(selectedObject: false);
					}
					designer.InvokeRefreshDictionary();
				}
				designer.TabControl.SelectedTabIndex = designer.Report.CurrentPage;
				if (closeAllInplaceDesigners)
				{
					((Control)designer.PageView.GetCurrentPageView()).Focus();
				}
				designer.LockTabControlRefresh = false;
				designer.LockRefreshDesigner = false;
				designer.AddSelectedToPropertyGrid();
				designer.InvokeSelectedObjectsChanged();
				StiInplaceDesigner.Close(closeAllInplaceDesigners);
				StiElementActionHelper.Hide();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRedo(StiDesignerControl designer)
		{
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!States.CanRedo(designer) || designer.TabControl.SelectedTabIndex == designer.PageCodeIndex)
				{
					return;
				}
				int count = designer.Report.Pages.Count;
				StiDataCollection dataStore = designer.Report.Dictionary.DataStore;
				StiUndoRedo stiUndoRedo = designer.UndoRedo.Redo(designer.Report, designer);
				int num = stiUndoRedo.CurrentPage;
				object obj = null;
				if (stiUndoRedo.PageIndex == -1)
				{
					StiComponentsCollection components = designer.Report.GetComponents();
					foreach (StiComponent item in components)
					{
						item.PackService();
						item.Parent = null;
						item.Page = null;
						if (item is StiContainer)
						{
							((StiContainer)item).Components.Clear();
						}
					}
					designer.Report.Pages.Clear();
					designer.Report.IsSerializing = true;
					try
					{
						obj = StiUndoRedo.CreateObjectFromUR(stiUndoRedo, designer.Report);
					}
					finally
					{
						designer.Report.IsSerializing = false;
					}
				}
				else
				{
					if (stiUndoRedo.PageType != StiPageType.Form)
					{
						obj = ((stiUndoRedo.PageType == StiPageType.Screen) ? ((object)(StiActivator.CreateObject("Stimulsoft.App.Components.StiScreen") as IStiScreen)) : ((object)((stiUndoRedo.PageType != StiPageType.Dashboard) ? ((IStiReportComponent)(StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage)) : ((IStiReportComponent)(StiActivator.CreateObject("Stimulsoft.Dashboard.Components.StiDashboard") as IStiDashboard)))));
					}
					else
					{
						StiDialogsProvider provider = StiDialogsProvider.GetProvider(designer.Report);
						if (provider == null)
						{
							string text = "Designer can't restore form from undo-redo buffer because it can't find service StiDialogsProvider or assembly 'Stimulsoft.Dialogs'!";
							MessageBox.Show(text);
							throw new Exception(text);
						}
						obj = provider.CreateForm(designer.Report);
					}
					designer.Report.IsSerializing = true;
					try
					{
						obj = StiUndoRedo.CreateObjectFromUR(stiUndoRedo, obj);
					}
					finally
					{
						designer.Report.IsSerializing = false;
					}
					StiComponentsCollection components2 = designer.Report.Pages[stiUndoRedo.PageIndex].GetComponents();
					foreach (StiComponent item2 in components2)
					{
						item2.PackService();
						item2.Parent = null;
						item2.Page = null;
						if (item2 is StiContainer)
						{
							((StiContainer)item2).Components.Clear();
						}
					}
				}
				if (stiUndoRedo.PageIndex == -1)
				{
					designer.Report = obj as StiReport;
					designer.Report.Dictionary.DataStore = dataStore;
					designer.Report.Dictionary.Connect(loadData: false);
				}
				else
				{
					designer.Report.Pages[stiUndoRedo.PageIndex] = obj as StiPage;
				}
				foreach (StiPage page in designer.Report.Pages)
				{
					page.Report = designer.Report;
				}
				if (num < 0)
				{
					num = 0;
				}
				if (num >= designer.Report.Pages.Count)
				{
					num = designer.Report.Pages.Count - 1;
				}
				designer.Report.CurrentPage = num;
				designer.Report.ApplyStyles();
				if (count != designer.Report.Pages.Count)
				{
					designer.Builder.BuildReport();
				}
				else
				{
					designer.Builder.RefreshPages();
					designer.InvokeRefreshDictionary();
				}
				designer.TabControl.SelectedTabIndex = designer.Report.CurrentPage;
				((Control)designer.PageView.GetCurrentPageView()).Focus();
				designer.AddSelectedToPropertyGrid();
				designer.InvokeSelectedObjectsChanged();
				StiInplaceDesigner.Close();
				StiElementActionHelper.Hide();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeCut(StiDesignerControl designer)
		{
			try
			{
				if (States.CanCut(designer) && designer.TabControl.SelectedTabIndex != designer.PageCodeIndex)
				{
					designer.UndoRedoSave(Loc.Get("MainMenu", "menuEditCut") + " " + Loc.Get("Report", "Components"), onlyCurrentPage: true);
					StiGroup groupFromPage = StiGroup.GetGroupFromPage(designer.Report.GetCurrentPage());
					StiClipboard.Set(groupFromPage.ToString("StiReport Clipboard"));
					designer.Report.GetCurrentPage().RemoveAllSelected();
					designer.AddSelectedToPropertyGrid();
					((Control)designer).Refresh();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeCopy(StiDesignerControl designer)
		{
			try
			{
				if (States.CanCopy(designer) && designer.TabControl.SelectedTabIndex != designer.PageCodeIndex)
				{
					StiGroup groupFromPage = StiGroup.GetGroupFromPage(designer.Report.GetCurrentPage());
					StiClipboard.Set(groupFromPage.ToString("StiReport Clipboard"));
					((Control)designer).Refresh();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokePaste(StiDesignerControl designer)
		{
			InvokePaste(designer, redraw: true);
		}

		public static void InvokePaste(StiDesignerControl designer, bool redraw)
		{
			try
			{
				if (!States.CanPaste(designer) || designer.TabControl.SelectedTabIndex == designer.PageCodeIndex)
				{
					return;
				}
				if (StiClipboard.Get() != null)
				{
					designer.SelectTool.Activate();
					bool flag = false;
					StiMainMenuService service = StiMainMenuService.GetService(designer);
					if (service != null)
					{
						flag = service.PasteUndoRedoSave;
					}
					if (flag)
					{
						designer.UndoRedoSave(Loc.Get("MainMenu", "menuEditPaste") + " " + Loc.Get("Report", "Components"), onlyCurrentPage: true);
					}
					string text = StiClipboard.Get();
					try
					{
						StiGroup stiGroup = StiGroup.CreateFromString(text, "StiReport Clipboard");
						StiInsertionComponents.InsertGroups(designer, stiGroup);
						StiOptions.Engine.GlobalEvents.InvokePasted(designer, new StiPastedEventArgs(stiGroup.Components));
						if (redraw)
						{
							((Control)designer.PageView.GetCurrentPageView().View).Invalidate();
						}
						stiGroup.Components?.ToList()?.Where((StiComponent c) => c is IStiElement)?.Cast<IStiElement>()?.ToList()?.ForEach(delegate(IStiElement c)
						{
							c.Key = StiKeyHelper.GenerateKey();
						});
					}
					catch
					{
						StiPage currentPage = designer.Report.GetCurrentPage();
						currentPage.ResetSelection();
						StiText stiText = new StiText
						{
							Text = 
							{
								Value = text
							}
						};
						stiText.ClientRectangle = designer.Report.GetCurrentPage().Unit.ConvertFromHInches(stiText.DefaultClientRectangle);
						stiText.Left = (currentPage.Width - stiText.Width) / 2.0;
						stiText.Top = (currentPage.Height - stiText.Height) / 2.0;
						stiText.ClientRectangle = stiText.ClientRectangle.AlignToGrid(designer.Report.GetCurrentPage().GridSize, aligningToGrid: true);
						currentPage.Components.Add(stiText);
						stiText.Select();
						StiInsertionComponents.Insert(designer, alignToGrid: true);
						StiOptions.Engine.GlobalEvents.InvokePasted(designer, new StiPastedEventArgs(stiText));
					}
				}
				if (redraw)
				{
					((Control)designer).Refresh();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeDelete(StiDesignerControl designer)
		{
			try
			{
				if (States.CanDelete(designer))
				{
					StiTextEditorToolService service = StiTextEditorToolService.GetService(designer);
					if ((service == null || service.EditedComponent == null) && designer.TabControl.SelectedTabIndex != designer.PageCodeIndex)
					{
						designer.UndoRedoSave(Loc.Get("MainMenu", "menuEditDelete") + " " + Loc.Get("Report", "Components"), onlyCurrentPage: true);
						designer.Report.GetCurrentPage().RemoveAllSelected();
						StiCrossLinePrimitiveHelper.RemoveIncorrectLinesFromContainer(designer.Report.GetCurrentPage().Components);
						designer.AddSelectedToPropertyGrid();
						((Control)designer).Refresh();
						StiInplaceDesigner.Close();
						StiElementActionHelper.Hide();
					}
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSelectAll(StiDesignerControl designer)
		{
			try
			{
				if (States.CanSelectAll(designer) && designer.TabControl.SelectedTabIndex != designer.PageCodeIndex)
				{
					designer.Report.GetCurrentPage().SelectAll();
					((Control)designer).Refresh();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class Dictionary
	{
		public static void InvokeVariableEdit(StiDictionaryPanelService dictionaryPanel, StiVariable variable, StiDesignerControl designerControl)
		{
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Invalid comparison between Unknown and I4
			variable.Key = StiKeyHelper.GetOrGeneratedKey(variable.Key);
			using IStiVariableEditForm stiVariableEditForm = StiDictionaryFormCreator.CreateVariableEditorForm(designerControl.Report, variable, isEditMode: true);
			stiVariableEditForm.VariableAllowUserValues = true;
			designerControl.AddOwnedForm((Form)((stiVariableEditForm is Form) ? stiVariableEditForm : null));
			stiVariableEditForm.Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "VariableEdit"));
			Type type = variable.Type;
			if (StiTypeFinder.FindType(type, typeof(Image)))
			{
				type = typeof(Image);
			}
			stiVariableEditForm.VariableName = variable.Name;
			stiVariableEditForm.VariableAlias = variable.Alias;
			stiVariableEditForm.VariableDescription = variable.Description ?? string.Empty;
			stiVariableEditForm.VariableType = type;
			stiVariableEditForm.VariableReadOnly = variable.ReadOnly;
			stiVariableEditForm.VariableInitBy = variable.InitBy;
			stiVariableEditForm.VariableRequestFromUser = variable.RequestFromUser;
			stiVariableEditForm.VariableAllowUsingAsSqlParameter = variable.AllowUseAsSqlParameter;
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				if (StiTypeFinder.FindType(type, typeof(Range)))
				{
					stiVariableEditForm.VariableInitByExpressionFrom = StiExpressionPacker.PackExpression(variable.InitByExpressionFrom, designerControl, useBraces: false);
					stiVariableEditForm.VariableInitByExpressionTo = StiExpressionPacker.PackExpression(variable.InitByExpressionTo, designerControl, useBraces: false);
				}
				else
				{
					stiVariableEditForm.VariableInitByExpression = StiExpressionPacker.PackExpression(variable.Value, designerControl, useBraces: false);
				}
			}
			else
			{
				bool flag = type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeRange);
				bool flag2 = type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(TimeSpanRange);
				if (flag || flag2)
				{
					stiVariableEditForm.VariableDateTimeDefaultValue = DateTime.Now;
					stiVariableEditForm.VariableDateTimeDefaultValueFrom = DateTime.Now;
					stiVariableEditForm.VariableDateTimeDefaultValueTo = DateTime.Now;
					object valueObject = variable.ValueObject;
					if (valueObject is DateTime || valueObject is TimeSpan || valueObject is DateTime? || valueObject is TimeSpan? || valueObject is DateTimeRange || valueObject is TimeSpanRange)
					{
						if (flag2)
						{
							if (valueObject is TimeSpanRange { From: var from } timeSpanRange)
							{
								if (!from.HasValue)
								{
									stiVariableEditForm.VariableTimeSpanNotAssignedFrom = true;
								}
								else
								{
									stiVariableEditForm.VariableTimeSpanDefaultValueFrom = from.Value;
								}
								TimeSpan? to = timeSpanRange.To;
								if (!to.HasValue)
								{
									stiVariableEditForm.VariableTimeSpanNotAssignedTo = true;
								}
								else
								{
									stiVariableEditForm.VariableTimeSpanDefaultValueTo = to.Value;
								}
							}
							else
							{
								stiVariableEditForm.VariableTimeSpanDefaultValue = (TimeSpan)valueObject;
							}
						}
						if (flag)
						{
							if (valueObject is DateTimeRange dateTimeRange)
							{
								if (!dateTimeRange.From.HasValue)
								{
									stiVariableEditForm.VariableNotAssignedFrom = true;
								}
								else
								{
									stiVariableEditForm.VariableDateTimeDefaultValueFrom = dateTimeRange.From.GetValueOrDefault();
								}
								if (!dateTimeRange.To.HasValue)
								{
									stiVariableEditForm.VariableNotAssignedTo = true;
								}
								else
								{
									stiVariableEditForm.VariableDateTimeDefaultValueTo = dateTimeRange.To.GetValueOrDefault();
								}
							}
							else
							{
								stiVariableEditForm.VariableDateTimeDefaultValue = (DateTime)valueObject;
							}
						}
					}
					else
					{
						stiVariableEditForm.VariableDateTimeDefaultValue = DateTime.Now;
						stiVariableEditForm.VariableDateTimeDefaultValueFrom = DateTime.Now;
						stiVariableEditForm.VariableDateTimeDefaultValueTo = DateTime.Now;
						stiVariableEditForm.VariableNotAssigned = true;
						stiVariableEditForm.VariableNotAssignedFrom = true;
						stiVariableEditForm.VariableNotAssignedTo = true;
						stiVariableEditForm.VariableTimeSpanNotAssigned = true;
						stiVariableEditForm.VariableTimeSpanNotAssignedFrom = true;
						stiVariableEditForm.VariableTimeSpanNotAssignedTo = true;
					}
				}
				else if (type == typeof(bool))
				{
					if (variable.ValueObject is bool)
					{
						stiVariableEditForm.VariableBoolDefaultValue = (bool)variable.ValueObject;
					}
				}
				else if (type == typeof(bool?))
				{
					if (variable.ValueObject is bool?)
					{
						stiVariableEditForm.VariableBoolDefaultValue = ((bool?)variable.ValueObject).GetValueOrDefault();
					}
				}
				else if (StiTypeFinder.FindType(variable.Type, typeof(Image)))
				{
					object valueObject2 = variable.ValueObject;
					stiVariableEditForm.VariableImage = (Image)((valueObject2 is Image) ? valueObject2 : null);
					stiVariableEditForm.VariableType = typeof(Image);
				}
				else if (StiTypeFinder.FindType(type, typeof(Range)))
				{
					Range range = variable.ValueObject as Range;
					stiVariableEditForm.VariableDefaultValueFrom = range.FromObject.ToString();
					stiVariableEditForm.VariableDefaultValueTo = range.ToObject.ToString();
				}
				else
				{
					stiVariableEditForm.VariableDefaultValue = variable.Value;
				}
			}
			if (variable.RequestFromUser && variable.DialogInfo != null)
			{
				stiVariableEditForm.VariableAllowUserValues = variable.DialogInfo.AllowUserValues;
				stiVariableEditForm.VariableBindingValue = variable.DialogInfo.BindingValue;
				stiVariableEditForm.VariableDataSourceInitializationType = variable.DialogInfo.ItemsInitializationType;
				if (variable.DialogInfo.BindingVariable != null)
				{
					foreach (StiVariable bindingVariable in stiVariableEditForm.GetBindingVariables())
					{
						if (bindingVariable.Name == variable.DialogInfo.BindingVariable.Name)
						{
							stiVariableEditForm.VariableBindingVariable = bindingVariable;
						}
					}
				}
				stiVariableEditForm.VariableSortDirection = variable.DialogInfo.SortDirection;
				stiVariableEditForm.VariableSortField = variable.DialogInfo.SortField;
				stiVariableEditForm.VariableBindingColumn = variable.DialogInfo.BindingValuesColumn;
				stiVariableEditForm.VariableKeysColumn = variable.DialogInfo.KeysColumn;
				stiVariableEditForm.VariableValuesColumn = variable.DialogInfo.ValuesColumn;
				stiVariableEditForm.VariableCheckedColumn = variable.DialogInfo.CheckedColumn;
				stiVariableEditForm.Items = ((variable.DialogInfo != null) ? variable.DialogInfo.GetDialogInfoItems(type) : new List<StiDialogInfoItem>());
				if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeRange) || type == typeof(DateTimeList))
				{
					stiVariableEditForm.VariableDateTimeType = variable.DialogInfo.DateTimeType;
				}
				if (type == typeof(string) || type == typeof(StringRange) || type == typeof(StringList))
				{
					stiVariableEditForm.VariableMask = variable.DialogInfo.Mask;
				}
			}
			stiVariableEditForm.VariableSelectionMode = variable.Selection;
			stiVariableEditForm.ShowDialog();
			if ((int)stiVariableEditForm.DialogResult != 1)
			{
				return;
			}
			designerControl.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "VariableEdit"));
			if (stiVariableEditForm.CopyModeActivated)
			{
				string category = variable.Category;
				variable = new StiVariable
				{
					Category = category
				};
			}
			type = stiVariableEditForm.VariableType;
			variable.Name = stiVariableEditForm.VariableName.Trim();
			variable.Alias = stiVariableEditForm.VariableAlias;
			variable.Description = stiVariableEditForm.VariableDescription;
			variable.Type = type;
			variable.ReadOnly = stiVariableEditForm.VariableReadOnly;
			variable.InitBy = stiVariableEditForm.VariableInitBy;
			variable.RequestFromUser = stiVariableEditForm.VariableRequestFromUser;
			variable.AllowUseAsSqlParameter = stiVariableEditForm.VariableAllowUsingAsSqlParameter;
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				if (StiTypeFinder.FindType(type, typeof(Range)))
				{
					variable.InitByExpressionFrom = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpressionFrom, designerControl, useBraces: false);
					variable.InitByExpressionTo = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpressionTo, designerControl, useBraces: false);
				}
				else
				{
					variable.Value = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpression, designerControl, useBraces: false);
				}
			}
			else
			{
				bool flag3 = type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeRange);
				bool flag4 = type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(TimeSpanRange);
				if (flag3 || flag4)
				{
					if (type == typeof(DateTimeRange) || type == typeof(TimeSpanRange))
					{
						if (type == typeof(DateTimeRange))
						{
							DateTime? from2 = ((stiVariableEditForm.VariableNotAssignedFrom || stiVariableEditForm.IsVariableDefaultValueIgnored) ? null : new DateTime?(stiVariableEditForm.VariableDateTimeDefaultValueFrom));
							DateTime? to2 = ((stiVariableEditForm.VariableNotAssignedTo || stiVariableEditForm.IsVariableDefaultValueIgnored) ? null : new DateTime?(stiVariableEditForm.VariableDateTimeDefaultValueTo));
							variable.ValueObject = new DateTimeRange(from2, to2);
						}
						if (type == typeof(TimeSpanRange))
						{
							TimeSpan? from3 = (stiVariableEditForm.VariableTimeSpanNotAssignedFrom ? null : new TimeSpan?(stiVariableEditForm.VariableTimeSpanDefaultValueFrom));
							TimeSpan? to3 = (stiVariableEditForm.VariableTimeSpanNotAssignedTo ? null : new TimeSpan?(stiVariableEditForm.VariableTimeSpanDefaultValueTo));
							variable.ValueObject = new TimeSpanRange(from3, to3);
						}
					}
					else
					{
						if (flag3)
						{
							if (stiVariableEditForm.VariableNotAssigned || stiVariableEditForm.IsVariableDefaultValueIgnored)
							{
								variable.ValueObject = null;
							}
							else
							{
								variable.ValueObject = stiVariableEditForm.VariableDateTimeDefaultValue;
							}
						}
						if (flag4)
						{
							if (stiVariableEditForm.VariableTimeSpanNotAssigned || stiVariableEditForm.IsVariableDefaultValueIgnored)
							{
								variable.ValueObject = null;
							}
							else
							{
								variable.ValueObject = stiVariableEditForm.VariableTimeSpanDefaultValue;
							}
						}
					}
				}
				else if (type == typeof(Image))
				{
					if (stiVariableEditForm.VariableImage == null)
					{
						variable.Type = typeof(Image);
						variable.ValueObject = null;
					}
					else
					{
						variable.Type = ((object)stiVariableEditForm.VariableImage).GetType();
						variable.ValueObject = stiVariableEditForm.VariableImage;
					}
				}
				else if (type == typeof(bool))
				{
					variable.Type = typeof(bool);
					variable.ValueObject = stiVariableEditForm.VariableBoolDefaultValue;
				}
				else if (type == typeof(bool?))
				{
					variable.Type = typeof(bool?);
					variable.ValueObject = stiVariableEditForm.VariableBoolDefaultValue;
				}
				else if (StiTypeFinder.FindType(type, typeof(Range)))
				{
					Range range2 = Activator.CreateInstance(type) as Range;
					range2.Parse(stiVariableEditForm.VariableDefaultValueFrom, stiVariableEditForm.VariableDefaultValueTo);
					variable.ValueObject = range2;
				}
				else
				{
					variable.Value = stiVariableEditForm.VariableDefaultValue;
				}
			}
			if (stiVariableEditForm.VariableRequestFromUser)
			{
				if (variable.DialogInfo == null)
				{
					variable.DialogInfo = new StiDialogInfo();
				}
				variable.DialogInfo.AllowUserValues = stiVariableEditForm.VariableAllowUserValues;
				variable.DialogInfo.BindingValue = stiVariableEditForm.VariableBindingValue;
				variable.DialogInfo.ItemsInitializationType = stiVariableEditForm.VariableDataSourceInitializationType;
				variable.DialogInfo.SortField = stiVariableEditForm.VariableSortField;
				variable.DialogInfo.SortDirection = stiVariableEditForm.VariableSortDirection;
				if (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
				{
					variable.DialogInfo.KeysColumn = stiVariableEditForm.VariableKeysColumn;
					variable.DialogInfo.ValuesColumn = stiVariableEditForm.VariableValuesColumn;
					variable.DialogInfo.CheckedColumn = stiVariableEditForm.VariableCheckedColumn;
					if (stiVariableEditForm.VariableBindingValue)
					{
						variable.DialogInfo.BindingVariable = stiVariableEditForm.VariableBindingVariable;
						variable.DialogInfo.BindingValuesColumn = stiVariableEditForm.VariableBindingColumn;
					}
					else
					{
						variable.DialogInfo.BindingVariable = null;
						variable.DialogInfo.BindingValuesColumn = string.Empty;
					}
					variable.DialogInfo.Keys = new string[0];
					variable.DialogInfo.Values = new string[0];
					variable.DialogInfo.CheckedStates = new bool[0];
				}
				else
				{
					variable.DialogInfo.KeysColumn = string.Empty;
					variable.DialogInfo.ValuesColumn = string.Empty;
					variable.DialogInfo.CheckedColumn = string.Empty;
					variable.DialogInfo.BindingVariable = null;
					variable.DialogInfo.BindingValuesColumn = string.Empty;
					variable.DialogInfo.SetDialogInfoItems(stiVariableEditForm.Items, type);
				}
				if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeRange) || type == typeof(DateTimeList))
				{
					variable.DialogInfo.DateTimeType = stiVariableEditForm.VariableDateTimeType;
				}
				if (type == typeof(string) || type == typeof(StringRange) || type == typeof(StringList))
				{
					variable.DialogInfo.Mask = stiVariableEditForm.VariableMask;
				}
			}
			else
			{
				variable.DialogInfo = null;
			}
			variable.Selection = stiVariableEditForm.VariableSelectionMode;
			if (stiVariableEditForm.CopyModeActivated)
			{
				string text = stiVariableEditForm.VariableName;
				if (designerControl.Report.Dictionary.Variables.Contains(text))
				{
					int num = 1;
					while (true)
					{
						text = ((num != 1) ? string.Format("{0}{1}{2}", stiVariableEditForm.VariableName, Loc.Get("Report", "CopyOf"), num) : (stiVariableEditForm.VariableName + Loc.Get("Report", "CopyOf")));
						if (!designerControl.Report.Dictionary.Variables.Contains(text))
						{
							break;
						}
						num++;
					}
				}
				if (variable.Name == variable.Alias)
				{
					variable.Alias = text;
				}
				variable.Name = text;
				designerControl.Report.Dictionary.Variables.Add(variable);
			}
			dictionaryPanel.BuildDictionary();
			dictionaryPanel.DictionaryControl.SelectNodeByKey(variable.Key);
			StiCacheCleaner.Clean(designerControl.Report);
		}

		public static bool InvokeCalcDataColumnEdit(StiDictionaryPanelService dictionaryPanel, StiCalcDataColumn column, StiDesignerControl designerControl)
		{
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Invalid comparison between Unknown and I4
			StiCalcDataColumnEditForm stiCalcDataColumnEditForm = new StiCalcDataColumnEditForm(designerControl.Report, isEditMode: true);
			try
			{
				designerControl?.AddOwnedForm((Form)(object)stiCalcDataColumnEditForm);
				((Control)stiCalcDataColumnEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CalcColumnEdit"));
				((Control)stiCalcDataColumnEditForm.tbName).Text = column.Name;
				((Control)stiCalcDataColumnEditForm.tbAlias).Text = column.Alias;
				((Control)stiCalcDataColumnEditForm.tbValue).Text = column.Value;
				StiTypeWrapper[] typeWrappers = StiTypeWrapper.GetTypeWrappers();
				List<StiTypeWrapper> list = new List<StiTypeWrapper>(typeWrappers);
				int num = 0;
				int num2 = -1;
				StiTypeWrapper[] array = typeWrappers;
				foreach (StiTypeWrapper stiTypeWrapper in array)
				{
					if ((column.Type != null && stiTypeWrapper.Type.ToString() == column.Type.ToString()) || (column.Type == null && stiTypeWrapper.Type == typeof(object)))
					{
						num2 = num;
						break;
					}
					num++;
				}
				if (num2 == -1)
				{
					list.Add(new StiTypeWrapper(column.Type));
					num2 = list.Count - 1;
				}
				ObjectCollection items = ((ComboBox)stiCalcDataColumnEditForm.cbType).Items;
				object[] array2 = list.ToArray();
				items.AddRange(array2);
				((ListControl)stiCalcDataColumnEditForm.cbType).SelectedIndex = num2;
				((Form)stiCalcDataColumnEditForm).ShowDialog();
				if ((int)((Form)stiCalcDataColumnEditForm).DialogResult == 1)
				{
					designerControl.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ColumnEdit"));
					StiDataSource stiDataSource = null;
					if (stiCalcDataColumnEditForm.CopyModeActivated)
					{
						stiDataSource = column.DataSource;
						column = new StiCalcDataColumn();
						stiDataSource.Columns.Add(column);
					}
					column.Name = ((Control)stiCalcDataColumnEditForm.tbName).Text;
					column.Alias = ((Control)stiCalcDataColumnEditForm.tbAlias).Text;
					column.Value = ((Control)stiCalcDataColumnEditForm.tbValue).Text;
					column.Type = ((StiTypeWrapper)((ComboBox)stiCalcDataColumnEditForm.cbType).SelectedItem).Type;
					if (stiCalcDataColumnEditForm.CopyModeActivated)
					{
						string text = ((Control)stiCalcDataColumnEditForm.tbName).Text;
						if (stiDataSource.Columns.Contains(text))
						{
							int num3 = 1;
							while (true)
							{
								text = ((num3 != 1) ? string.Format("{0}{1}{2}", ((Control)stiCalcDataColumnEditForm.tbName).Text, Loc.Get("Report", "CopyOf"), num3) : (((Control)stiCalcDataColumnEditForm.tbName).Text + Loc.Get("Report", "CopyOf")));
								if (!stiDataSource.Columns.Contains(text))
								{
									break;
								}
								num3++;
							}
						}
						if (column.Name == column.Alias)
						{
							column.Alias = text;
						}
						column.Name = text;
					}
					if (dictionaryPanel != null)
					{
						dictionaryPanel.BuildDictionary();
					}
					else
					{
						designerControl.BuildDictionary();
					}
					return stiCalcDataColumnEditForm.CopyModeActivated;
				}
			}
			finally
			{
				((IDisposable)stiCalcDataColumnEditForm)?.Dispose();
			}
			return false;
		}

		public static void InvokeDuplicate(StiDictionaryPanelService dictionaryPanel)
		{
			TreeNode selectedNode = ((TreeView)dictionaryPanel.DictionaryControl.tvData).SelectedNode;
			object obj = ((selectedNode != null) ? selectedNode.Tag : null);
			if (obj is StiVariable && (obj as StiVariable).IsCategory)
			{
				InvokeVariableCategoryDuplicate(dictionaryPanel, obj as StiVariable);
			}
			else if (obj is StiVariable)
			{
				InvokeElementDuplicate(dictionaryPanel, StiDataType.Variable, obj);
			}
			else if (obj is StiDataSource)
			{
				InvokeElementDuplicate(dictionaryPanel, StiDataType.DataSource, obj);
			}
			else if (obj is StiDataRelation)
			{
				InvokeElementDuplicate(dictionaryPanel, StiDataType.DataRelation, obj);
			}
			else if (obj is StiDatabase)
			{
				InvokeElementDuplicate(dictionaryPanel, StiDataType.Database, obj);
			}
			else if (obj is StiResource)
			{
				InvokeElementDuplicate(dictionaryPanel, StiDataType.Resource, obj);
			}
		}

		public static void InvokeVariableCategoryDuplicate(StiDictionaryPanelService dictionaryPanel, StiVariable variable)
		{
			try
			{
				StiDesignerControl designer = dictionaryPanel.Designer;
				designer.UndoRedoSave(Loc.Get("Buttons", "Duplicate") + " - '" + variable.Category + "'");
				StiVariable stiVariable = new StiVariable(GetUniqueName(designer.Report, StiDataType.Variable, variable.Category, isVariableCategories: true));
				stiVariable.Key = StiKeyHelper.GenerateKey();
				designer.Report.Dictionary.Variables.Add(stiVariable);
				dictionaryPanel.BuildDictionary();
				dictionaryPanel.DictionaryControl.SelectNodeByKey(stiVariable.Key);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		private static void InvokeElementDuplicate(StiDictionaryPanelService dictionaryPanel, StiDataType type, object element)
		{
			try
			{
				StiDesignerControl designer = dictionaryPanel.Designer;
				designer.UndoRedoSave(Loc.Get("Buttons", "Duplicate") + " - '" + (element as IStiName).Name + "'");
				IStiName stiName = element as IStiName;
				IStiAlias stiAlias = element as IStiAlias;
				object obj = (element as ICloneable).Clone();
				IStiName stiName2 = obj as IStiName;
				IStiAlias stiAlias2 = obj as IStiAlias;
				IStiAppCell stiAppCell = obj as IStiAppCell;
				stiAppCell.SetKey(StiKeyHelper.GenerateKey());
				stiName2.Name = GetUniqueName(designer.Report, type, stiName.Name);
				stiAlias2.Alias = ((stiName.Name == stiAlias.Alias) ? stiName2.Name : stiAlias.Alias);
				switch (type)
				{
				case StiDataType.Variable:
					designer.Report.Dictionary.Variables.Add(obj as StiVariable);
					break;
				case StiDataType.DataSource:
					designer.Report.Dictionary.DataSources.Add(obj as StiDataSource);
					break;
				case StiDataType.DataRelation:
					designer.Report.Dictionary.Relations.Add(obj as StiDataRelation);
					break;
				case StiDataType.Database:
					designer.Report.Dictionary.Databases.Add(obj as StiDatabase);
					break;
				case StiDataType.Resource:
					designer.Report.Dictionary.Resources.Add(obj as StiResource);
					break;
				default:
					throw new NotSupportedException($"The {type} is not supported in the StiActions.Dictionary.InvokeElementDuplicate method!");
				}
				dictionaryPanel.BuildDictionary();
				dictionaryPanel.DictionaryControl.SelectNodeByKey(stiAppCell.GetKey());
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		private static string GetUniqueName(StiReport report, StiDataType type, string baseName, bool isVariableCategories = false)
		{
			int num = 2;
			string text = (baseName += Loc.Get("Report", "CopyOf"));
			while ((type != StiDataType.Resource || report.Dictionary.Resources.Contains(text)) && (type != StiDataType.DataSource || report.Dictionary.DataSources.Contains(text)) && (type != StiDataType.DataRelation || report.Dictionary.Relations.Contains(text)) && (type != StiDataType.Database || report.Dictionary.Databases.Contains(text)) && (type != StiDataType.Variable || report.Dictionary.Variables.Contains(text) || isVariableCategories) && !(type == StiDataType.Variable && !report.Dictionary.Variables.ContainsCategory(text) && isVariableCategories))
			{
				text = $"{baseName}{num++}";
			}
			return text;
		}
	}

	public sealed class Templates
	{
		public static void InvokeOpen(StiDesignerControl designer)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Invalid comparison between Unknown and I4
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				OpenFileDialog val = new OpenFileDialog();
				try
				{
					StiSettings.Load();
					string text = StiOptions.Designer.TemplatesSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						text = StiSettings.GetStr("StiDesigner", "TemplatesSaveLoadPath", string.Empty);
					}
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						((FileDialog)val).InitialDirectory = text;
					}
					((FileDialog)val).Filter = StiSLService.GetPageFilters(StiSLActions.Load);
					((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageOpen"));
					((FileDialog)val).RestoreDirectory = true;
					if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
					{
						return;
					}
					text = StiOptions.Designer.PageSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						StiSettings.Load();
						StiSettings.Set("StiDesigner", "PageSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
						StiSettings.Save();
					}
					designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFilePageOpen"));
					StiPage stiPage = null;
					string pageTypeFromFile = StiPageTypeHelper.GetPageTypeFromFile(((FileDialog)val).FileName);
					if (pageTypeFromFile == "StiForm")
					{
						StiDialogsProvider provider = StiDialogsProvider.GetProvider(designer.Report);
						if (provider == null)
						{
							string text2 = "Designer can't load form from file because it can't find service StiDialogsProvider or assembly 'Stimulsoft.Dialogs'!";
							MessageBox.Show(text2);
							throw new Exception(text2);
						}
						IStiForm stiForm = provider.CreateForm(designer.Report);
						stiPage = provider.CreateForm(designer.Report) as StiPage;
					}
					else
					{
						stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage;
					}
					stiPage.Report = designer.Report;
					stiPage.Load(StiSLService.GetPageSLServices(StiSLActions.Load)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
					stiPage.Convert(stiPage.Unit, StiUnit.HundredthsOfInch);
					stiPage.Components.SortByPriority();
					StiGroup stiGroup = new StiGroup();
					stiGroup.Components = stiPage.Components;
					designer.SelectTool.SuspendTickCount = Environment.TickCount;
					StiInsertionComponents.InsertGroups(designer, stiGroup, designer.Report.Info.AlignToGrid);
					((Control)designer.PageView.GetCurrentPageView().View).Invalidate();
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)designer).GetType(), "Loading template...ERROR");
				StiLogService.Write(((object)designer).GetType(), ex);
				StiExceptionProvider.Show(ex);
			}
			finally
			{
				designer.Builder.BuildReport();
			}
		}

		public static void InvokeSave(StiDesignerControl designer)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Invalid comparison between Unknown and I4
			try
			{
				SaveFileDialog val = new SaveFileDialog();
				try
				{
					StiSettings.Load();
					string text = StiOptions.Designer.TemplatesSaveLoadPath;
					if (string.IsNullOrEmpty(text))
					{
						text = StiSettings.GetStr("StiDesigner", "TemplatesSaveLoadPath", string.Empty);
					}
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						((FileDialog)val).InitialDirectory = text;
					}
					((FileDialog)val).Filter = StiSLService.GetPageFilters(StiSLActions.Save);
					((FileDialog)val).RestoreDirectory = true;
					((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileSaveAs"));
					((FileDialog)val).FileName = StiLocalization.Get("Wizards", "groupTemplates");
					((FileDialog)val).DefaultExt = "pg";
					if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
					{
						text = StiOptions.Designer.TemplatesSaveLoadPath;
						if (string.IsNullOrEmpty(text))
						{
							StiSettings.Load();
							StiSettings.Set("StiDesigner", "TemplatesSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
							StiSettings.Save();
						}
						((Control)designer).Refresh();
						StiPage stiPage = designer.Report.GetCurrentPage().Clone(cloneProperties: true, cloneComponents: false) as StiPage;
						stiPage.ReportUnit = designer.Report.GetCurrentPage().Unit;
						StiGroup groupFromPage = StiGroup.GetGroupFromPage(designer.Report.GetCurrentPage());
						stiPage.Components = groupFromPage.Components;
						stiPage.Save(StiSLService.GetPageSLServices(StiSLActions.Save)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class Font
	{
		public sealed class States
		{
			public static bool GetFontBoldChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontBoldChecked(designer, fontObject);
			}

			public static bool GetFontBoldChecked(StiDesignerControl designer, object fontObject)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (fontObject == null)
				{
					return false;
				}
				Font val = (Font)fontObject;
				return (val.Style & 1) > 0;
			}

			public static bool GetFontItalicChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontItalicChecked(designer, fontObject);
			}

			public static bool GetFontItalicChecked(StiDesignerControl designer, object fontObject)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (fontObject == null)
				{
					return false;
				}
				Font val = (Font)fontObject;
				return (val.Style & 2) > 0;
			}

			public static bool GetFontUnderlineChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontUnderlineChecked(designer, fontObject);
			}

			public static bool GetFontUnderlineChecked(StiDesignerControl designer, object fontObject)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (fontObject == null)
				{
					return false;
				}
				Font val = (Font)fontObject;
				return (val.Style & 4) > 0;
			}

			public static bool GetFontStrikeoutChecked(StiDesignerControl designer, object fontObject)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (fontObject == null)
				{
					return false;
				}
				Font val = (Font)fontObject;
				return (val.Style & 8) > 0;
			}

			public static string GetFontName(StiDesignerControl designer)
			{
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontName(fontObject);
			}

			public static string GetFontName(object fontObject)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Expected O, but got Unknown
				if (fontObject == null)
				{
					return string.Empty;
				}
				Font val = (Font)fontObject;
				return val.FontFamily.Name;
			}

			public static float GetFontSize(StiDesignerControl designer)
			{
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontSize(fontObject);
			}

			public static float GetFontSize(object fontObject)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Expected O, but got Unknown
				if (fontObject == null)
				{
					return 8f;
				}
				Font val = (Font)fontObject;
				return val.Size;
			}

			public static Color GetTextBrush(StiDesignerControl designer)
			{
				StiBrush textBrush = ((designer.Report != null) ? (designer.Report.GetCurrentPage().GetTextBrush() as StiBrush) : null);
				return GetTextBrush(designer, textBrush);
			}

			public static Color GetTextBrush(StiDesignerControl designer, StiBrush textBrush)
			{
				if (textBrush != null)
				{
					return StiBrush.ToColor(textBrush);
				}
				return Color.White;
			}

			public static bool GetFontBoldEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontBoldEnabled(designer, fontObject);
			}

			public static bool GetFontBoldEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontItalicEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontItalicEnabled(designer, fontObject);
			}

			public static bool GetFontItalicEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontUnderlineEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontUnderlineEnabled(designer, fontObject);
			}

			public static bool GetFontStrikeoutEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (item is IStiTextElement)
					{
						return false;
					}
				}
				return fontObject != null;
			}

			public static bool GetFontUnderlineEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontNameEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontNameEnabled(designer, fontObject);
			}

			public static bool GetFontNameEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontSizeEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontSizeEnabled(designer, fontObject);
			}

			public static bool GetFontSizeEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontGrowEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontGrowEnabled(designer, fontObject);
			}

			public static bool GetFontGrowEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetFontShrinkEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetFontShrinkEnabled(designer, fontObject);
			}

			public static bool GetFontShrinkEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetClearContentsEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object fontObject = designer.Report?.GetCurrentPage().GetFont();
				return GetClearContentsEnabled(designer, fontObject);
			}

			public static bool GetClearContentsEnabled(StiDesignerControl designer, object fontObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return fontObject != null;
			}

			public static bool GetTextBrushEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiBrush textBrush = ((designer.Report != null) ? (designer.Report.GetCurrentPage().GetTextBrush() as StiBrush) : null);
				return GetTextBrushEnabled(designer, textBrush);
			}

			public static bool GetTextBrushEnabled(StiDesignerControl designer, StiBrush textBrush)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return textBrush != null;
			}
		}

		public static void InvokeFontName(StiDesignerControl designer, string fontName)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: true, setSize: false, setStyleBold: false, setStyleItalic: false, setStyleUnderline: false, setStyleStrikeout: false, fontName, 0f, valueOfBold: false, valueOfItalic: false, valueOfUnderline: false, valueOfStrikeout: false);
				designer.GetStyleFromComponents();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeFontSize(StiDesignerControl designer, float fontSize)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: false, setSize: true, setStyleBold: false, setStyleItalic: false, setStyleUnderline: false, setStyleStrikeout: false, null, fontSize, valueOfBold: false, valueOfItalic: false, valueOfUnderline: false, valueOfStrikeout: false);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBold(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: false, setSize: false, setStyleBold: true, setStyleItalic: false, setStyleUnderline: false, setStyleStrikeout: false, null, 0f, value, valueOfItalic: false, valueOfUnderline: false, valueOfStrikeout: false);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeItalic(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleItalic"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: false, setSize: false, setStyleBold: false, setStyleItalic: true, setStyleUnderline: false, setStyleStrikeout: false, null, 0f, valueOfBold: false, value, valueOfUnderline: false, valueOfStrikeout: false);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeUnderline(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleUnderline"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: false, setSize: false, setStyleBold: false, setStyleItalic: false, setStyleUnderline: true, setStyleStrikeout: false, null, 0f, valueOfBold: false, valueOfItalic: false, value, valueOfStrikeout: false);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeStrikeout(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "FontStrikeout"), onlyCurrentPage: true);
				SetFont(designer, setFontFamily: false, setSize: false, setStyleBold: false, setStyleItalic: false, setStyleUnderline: false, setStyleStrikeout: true, null, 0f, valueOfBold: false, valueOfItalic: false, valueOfUnderline: false, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeTextBrush(StiDesignerControl designer, Color textColor)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "TextColor"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextBrush(new StiSolidBrush(textColor));
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeGrowFont(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontGrow"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().GrowFont();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeShrinkFont(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontShrink"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().ShrinkFont();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeClearContents(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditClearContents"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().ClearContents();
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetFont(StiDesignerControl designer, bool setFontFamily, bool setSize, bool setStyleBold, bool setStyleItalic, bool setStyleUnderline, bool setStyleStrikeout, string fontName, float fontSize, bool valueOfBold, bool valueOfItalic, bool valueOfUnderline, bool valueOfStrikeout)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			FontStyle val = (FontStyle)0;
			if (valueOfBold)
			{
				val = (FontStyle)(val | 1);
			}
			if (valueOfItalic)
			{
				val = (FontStyle)(val | 2);
			}
			if (valueOfUnderline)
			{
				val = (FontStyle)(val | 4);
			}
			if (valueOfStrikeout)
			{
				val = (FontStyle)(val | 8);
			}
			if (string.IsNullOrEmpty(fontName))
			{
				fontName = "Arial";
			}
			if (fontSize == 0f)
			{
				fontSize = 1f;
			}
			Font font = StiFontCollection.CreateFont(fontName, fontSize, StiFontUtils.CorrectStyle(fontName, val));
			designer.Report.GetCurrentPage().SetFont(font, setFontFamily, setSize, setStyleBold, setStyleItalic, setStyleUnderline, setStyleStrikeout, setUnit: true, setGdiCharSet: true, setGdiVerticalFont: true);
			((Control)designer).Refresh();
		}
	}

	public sealed class ContentAlignment
	{
		public sealed class States
		{
			public static bool GetAlignTopChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignTopChecked(designer, verticalAlignmentObj);
			}

			public static bool GetAlignTopChecked(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (verticalAlignmentObj == null)
				{
					return false;
				}
				return (StiVertAlignment)verticalAlignmentObj == StiVertAlignment.Top;
			}

			public static bool GetAlignMiddleChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignMiddleChecked(designer, verticalAlignmentObj);
			}

			public static bool GetAlignMiddleChecked(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (verticalAlignmentObj == null)
				{
					return false;
				}
				return (StiVertAlignment)verticalAlignmentObj == StiVertAlignment.Center;
			}

			public static bool GetAlignBottomChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignBottomChecked(designer, verticalAlignmentObj);
			}

			public static bool GetAlignBottomChecked(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (verticalAlignmentObj == null)
				{
					return false;
				}
				StiVertAlignment stiVertAlignment = (StiVertAlignment)verticalAlignmentObj;
				return stiVertAlignment == StiVertAlignment.Bottom;
			}

			public static bool GetAlignLeftChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorAlignment = designer.Report.GetCurrentPage().GetTextHorAlignment();
				object horAlignment = designer.Report.GetCurrentPage().GetHorAlignment();
				return GetAlignLeftChecked(designer, textHorAlignment, horAlignment);
			}

			public static bool GetAlignLeftChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignLeftChecked(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignLeftChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj != null)
				{
					StiTextHorAlignment stiTextHorAlignment = (StiTextHorAlignment)textHorizontalAlignmentObj;
					return stiTextHorAlignment == StiTextHorAlignment.Left;
				}
				if (horizontalAlignmentObj != null)
				{
					StiHorAlignment stiHorAlignment = (StiHorAlignment)horizontalAlignmentObj;
					return stiHorAlignment == StiHorAlignment.Left;
				}
				if (crossHorizontalAlignmentObj != null)
				{
					StiCrossHorAlignment stiCrossHorAlignment = (StiCrossHorAlignment)crossHorizontalAlignmentObj;
					return stiCrossHorAlignment == StiCrossHorAlignment.Left;
				}
				return false;
			}

			public static bool GetAlignCenterChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				object horizontalAlignmentObj = designer.Report?.GetCurrentPage().GetHorAlignment();
				return GetAlignCenterChecked(designer, textHorizontalAlignmentObj, horizontalAlignmentObj);
			}

			public static bool GetAlignCenterChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignCenterChecked(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignCenterChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj != null)
				{
					StiTextHorAlignment stiTextHorAlignment = (StiTextHorAlignment)textHorizontalAlignmentObj;
					return stiTextHorAlignment == StiTextHorAlignment.Center;
				}
				if (horizontalAlignmentObj != null)
				{
					StiHorAlignment stiHorAlignment = (StiHorAlignment)horizontalAlignmentObj;
					return stiHorAlignment == StiHorAlignment.Center;
				}
				if (crossHorizontalAlignmentObj != null)
				{
					StiCrossHorAlignment stiCrossHorAlignment = (StiCrossHorAlignment)crossHorizontalAlignmentObj;
					return stiCrossHorAlignment == StiCrossHorAlignment.Center;
				}
				return false;
			}

			public static bool GetAlignRightChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				object horizontalAlignmentObj = designer.Report?.GetCurrentPage().GetHorAlignment();
				return GetAlignRightChecked(designer, textHorizontalAlignmentObj, horizontalAlignmentObj);
			}

			public static bool GetAlignRightChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignRightChecked(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignRightChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj != null)
				{
					StiTextHorAlignment stiTextHorAlignment = (StiTextHorAlignment)textHorizontalAlignmentObj;
					return stiTextHorAlignment == StiTextHorAlignment.Right;
				}
				if (horizontalAlignmentObj != null)
				{
					StiHorAlignment stiHorAlignment = (StiHorAlignment)horizontalAlignmentObj;
					return stiHorAlignment == StiHorAlignment.Right;
				}
				if (crossHorizontalAlignmentObj != null)
				{
					StiCrossHorAlignment stiCrossHorAlignment = (StiCrossHorAlignment)crossHorizontalAlignmentObj;
					return stiCrossHorAlignment == StiCrossHorAlignment.Right;
				}
				return false;
			}

			public static bool GetAlignWidthChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				return GetAlignWidthChecked(designer, textHorizontalAlignmentObj);
			}

			public static bool GetAlignWidthChecked(StiDesignerControl designer, object textHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignWidthChecked(designer, textHorizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignWidthChecked(StiDesignerControl designer, object textHorizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj != null)
				{
					StiTextHorAlignment stiTextHorAlignment = (StiTextHorAlignment)textHorizontalAlignmentObj;
					return stiTextHorAlignment == StiTextHorAlignment.Width;
				}
				if (crossHorizontalAlignmentObj != null)
				{
					StiCrossHorAlignment stiCrossHorAlignment = (StiCrossHorAlignment)crossHorizontalAlignmentObj;
					return stiCrossHorAlignment == StiCrossHorAlignment.Width;
				}
				return false;
			}

			public static bool GetWordWrapChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetWordWrap();
				if (obj is bool)
				{
					return (bool)obj;
				}
				return false;
			}

			public static bool GetAlignTopEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignTopEnabled(designer, verticalAlignmentObj);
			}

			public static bool GetAlignTopEnabled(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return verticalAlignmentObj != null;
			}

			public static bool GetAlignMiddleEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignMiddleEnabled(designer, verticalAlignmentObj);
			}

			public static bool GetAlignMiddleEnabled(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return verticalAlignmentObj != null;
			}

			public static bool GetAlignBottomEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object verticalAlignmentObj = designer.Report?.GetCurrentPage().GetVertAlignment();
				return GetAlignBottomEnabled(designer, verticalAlignmentObj);
			}

			public static bool GetAlignBottomEnabled(StiDesignerControl designer, object verticalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return verticalAlignmentObj != null;
			}

			public static bool GetAlignLeftEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorAlignment = designer.Report.GetCurrentPage().GetTextHorAlignment();
				object horAlignment = designer.Report.GetCurrentPage().GetHorAlignment();
				return GetAlignLeftEnabled(designer, textHorAlignment, horAlignment);
			}

			public static bool GetAlignLeftEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignLeftEnabled(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignLeftEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj == null && horizontalAlignmentObj == null)
				{
					return crossHorizontalAlignmentObj != null;
				}
				return true;
			}

			public static bool GetAlignCenterEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				object horizontalAlignmentObj = designer.Report?.GetCurrentPage().GetHorAlignment();
				return GetAlignCenterEnabled(designer, textHorizontalAlignmentObj, horizontalAlignmentObj);
			}

			public static bool GetAlignCenterEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignCenterEnabled(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignCenterEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj == null && horizontalAlignmentObj == null)
				{
					return crossHorizontalAlignmentObj != null;
				}
				return true;
			}

			public static bool GetAlignRightEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				object horizontalAlignmentObj = designer.Report?.GetCurrentPage().GetHorAlignment();
				return GetAlignRightEnabled(designer, textHorizontalAlignmentObj, horizontalAlignmentObj);
			}

			public static bool GetAlignRightEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignRightEnabled(designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignRightEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object horizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj == null && horizontalAlignmentObj == null)
				{
					return crossHorizontalAlignmentObj != null;
				}
				return true;
			}

			public static bool GetAlignWidthEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object textHorizontalAlignmentObj = designer.Report?.GetCurrentPage().GetTextHorAlignment();
				return GetAlignWidthEnabled(designer, textHorizontalAlignmentObj);
			}

			public static bool GetAlignWidthEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object crossHorAlignment = designer.Report.GetCurrentPage().GetCrossHorAlignment();
				return GetAlignWidthEnabled(designer, textHorizontalAlignmentObj, crossHorAlignment);
			}

			public static bool GetAlignWidthEnabled(StiDesignerControl designer, object textHorizontalAlignmentObj, object crossHorizontalAlignmentObj)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (textHorizontalAlignmentObj == null)
				{
					return crossHorizontalAlignmentObj != null;
				}
				return true;
			}

			public static bool GetWordWrapEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetWordWrap();
				return obj != null;
			}

			public static bool GetRotateTextEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				bool result = false;
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if ((item is IStiTextOptions || item is IStiEnumAngle) && StiRestrictionsHelper.IsAllowChange(item))
					{
						result = true;
						break;
					}
				}
				return result;
			}

			public static bool GetLineSpacingEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (item is StiText && StiRestrictionsHelper.IsAllowChange(item))
					{
						return true;
					}
				}
				return false;
			}
		}

		public static void InvokeAlignLeft(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignLeft"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Left);
				designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Left);
				designer.Report.GetCurrentPage().SetCrossHorAlign(StiCrossHorAlignment.Left);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignCenter(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignCenter"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Center);
				designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Center);
				designer.Report.GetCurrentPage().SetCrossHorAlign(StiCrossHorAlignment.Center);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignRight(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignRight"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Right);
				designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Right);
				designer.Report.GetCurrentPage().SetCrossHorAlign(StiCrossHorAlignment.Right);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignWidth(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignWidth"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Width);
				designer.Report.GetCurrentPage().SetCrossHorAlign(StiCrossHorAlignment.Width);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignTop(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignTop"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetVertAlign(StiVertAlignment.Top);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignMiddle(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignMiddle"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetVertAlign(StiVertAlignment.Center);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignBottom(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignBottom"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetVertAlign(StiVertAlignment.Bottom);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		private static void SetLineSpacing(StiDesignerControl designer, float value)
		{
			StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
			foreach (StiComponent item in selectedComponentsOnPage)
			{
				if (item is StiText stiText)
				{
					stiText.LineSpacing = value;
				}
			}
		}

		public static void InvokeLineSpacing(StiDesignerControl designer, float value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "LineSpacing"), onlyCurrentPage: true);
				SetLineSpacing(designer, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRotate0(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiAngleAngle0"), onlyCurrentPage: true);
				SetRotateAngle(designer, 0f);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRotate45(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiAngleAngle45"), onlyCurrentPage: true);
				SetRotateAngle(designer, 45f);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRotate90(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiAngleAngle90"), onlyCurrentPage: true);
				SetRotateAngle(designer, 90f);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRotate180(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiAngleAngle180"), onlyCurrentPage: true);
				SetRotateAngle(designer, 180f);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeRotate270(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiAngleAngle270"), onlyCurrentPage: true);
				SetRotateAngle(designer, 270f);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeWordWrap(StiDesignerControl designer, bool wordWrap)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "WordWrap"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetWordWrap(wordWrap);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetRotateAngle(StiDesignerControl designer, float angle)
		{
			StiComponentsCollection selectedComponents = designer.Report.GetCurrentPage().GetSelectedComponents();
			foreach (StiComponent item in selectedComponents)
			{
				if (item is IStiTextOptions)
				{
					((IStiTextOptions)item).TextOptions.Angle = angle;
				}
				if (item is IStiEnumAngle)
				{
					switch ((int)angle)
					{
					case 0:
						((IStiEnumAngle)item).Angle = StiAngle.Angle0;
						break;
					case 90:
						((IStiEnumAngle)item).Angle = StiAngle.Angle90;
						break;
					case 180:
						((IStiEnumAngle)item).Angle = StiAngle.Angle180;
						break;
					case 270:
						((IStiEnumAngle)item).Angle = StiAngle.Angle270;
						break;
					}
				}
			}
			designer.Builder.Redraw();
		}
	}

	public sealed class Borders
	{
		public sealed class States
		{
			public static bool GetBordersAllChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersAllChecked(designer, borderObject);
			}

			public static bool GetBordersAllChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return ((StiBorder)borderObject).Side == StiBorderSides.All;
				}
				if (borderObject is StiSimpleBorder)
				{
					return ((StiSimpleBorder)borderObject).Side == StiBorderSides.All;
				}
				return false;
			}

			public static bool GetBordersNoneChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersNoneChecked(designer, borderObject);
			}

			public static bool GetBordersNoneChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return ((StiBorder)borderObject).Side == StiBorderSides.None;
				}
				if (borderObject is StiSimpleBorder)
				{
					return ((StiSimpleBorder)borderObject).Side == StiBorderSides.None;
				}
				return false;
			}

			public static bool GetBordersLeftChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersLeftChecked(designer, borderObject);
			}

			public static bool GetBordersLeftChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return (((StiBorder)borderObject).Side & StiBorderSides.Left) > StiBorderSides.None;
				}
				if (borderObject is StiSimpleBorder)
				{
					return (((StiSimpleBorder)borderObject).Side & StiBorderSides.Left) > StiBorderSides.None;
				}
				return false;
			}

			public static bool GetBordersRightChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersRightChecked(designer, borderObject);
			}

			public static bool GetBordersRightChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return (((StiBorder)borderObject).Side & StiBorderSides.Right) > StiBorderSides.None;
				}
				if (borderObject is StiSimpleBorder)
				{
					return (((StiSimpleBorder)borderObject).Side & StiBorderSides.Right) > StiBorderSides.None;
				}
				return false;
			}

			public static bool GetBordersTopChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage()?.GetBorder();
				return GetBordersTopChecked(designer, borderObject);
			}

			public static bool GetBordersTopChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return (((StiBorder)borderObject).Side & StiBorderSides.Top) > StiBorderSides.None;
				}
				if (borderObject is StiSimpleBorder)
				{
					return (((StiSimpleBorder)borderObject).Side & StiBorderSides.Top) > StiBorderSides.None;
				}
				return false;
			}

			public static bool GetBordersBottomChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersBottomChecked(designer, borderObject);
			}

			public static bool GetBordersBottomChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return (((StiBorder)borderObject).Side & StiBorderSides.Bottom) > StiBorderSides.None;
				}
				if (borderObject is StiSimpleBorder)
				{
					return (((StiSimpleBorder)borderObject).Side & StiBorderSides.Bottom) > StiBorderSides.None;
				}
				return false;
			}

			public static bool GetBordersShadowChecked(StiDesignerControl designer)
			{
				if (designer?.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetBorder();
				if (obj != null && !(obj is StiSimpleBorder))
				{
					return GetBordersShadowChecked(designer, obj);
				}
				object obj2 = designer.Report?.GetCurrentPage().GetSimpleShadow();
				if (obj2 != null)
				{
					return GetBordersShadowChecked(designer, obj);
				}
				return false;
			}

			public static bool GetBordersShadowChecked(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (borderObject is StiBorder)
				{
					return ((StiBorder)borderObject).DropShadow;
				}
				if (borderObject is StiSimpleShadow)
				{
					return ((StiSimpleShadow)borderObject).Visible;
				}
				return false;
			}

			public static StiPenStyle GetBordersStyle(StiDesignerControl designer)
			{
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersStyle(designer, borderObject);
			}

			public static StiPenStyle GetBordersStyle(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return StiPenStyle.None;
				}
				if (borderObject is StiBorder)
				{
					return ((StiBorder)borderObject).Style;
				}
				if (borderObject is StiSimpleBorder)
				{
					return ((StiSimpleBorder)borderObject).Style;
				}
				return StiPenStyle.None;
			}

			public static Color GetBordersColor(StiDesignerControl designer)
			{
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersColor(designer, borderObject);
			}

			public static Color GetBordersColor(StiDesignerControl designer, object borderObject)
			{
				if (designer?.Report == null || borderObject == null)
				{
					return Color.White;
				}
				if (borderObject is StiBorder)
				{
					return ((StiBorder)borderObject).Color;
				}
				if (borderObject is StiSimpleBorder)
				{
					return ((StiSimpleBorder)borderObject).Color;
				}
				return Color.White;
			}

			public static Color GetFillBrushColor(StiDesignerControl designer)
			{
				StiBrush brush = ((designer.Report != null) ? (designer.Report.GetCurrentPage().GetBrush() as StiBrush) : null);
				return GetFillBrushColor(designer, brush);
			}

			public static Color GetFillBrushColor(StiDesignerControl designer, StiBrush brush)
			{
				if (brush == null)
				{
					return Color.White;
				}
				return StiBrush.ToColor(brush);
			}

			public static bool GetBordersAllEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersAllChecked(designer, borderObject);
			}

			public static bool GetBordersAllEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersNoneEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage()?.GetBorder();
				return GetBordersNoneEnabled(designer, borderObject);
			}

			public static bool GetBordersNoneEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersLeftEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersLeftChecked(designer, borderObject);
			}

			public static bool GetBordersLeftEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersRightEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersRightEnabled(designer, borderObject);
			}

			public static bool GetBordersRightEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersTopEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersTopEnabled(designer, borderObject);
			}

			public static bool GetBordersTopEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersBottomEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersBottomEnabled(designer, borderObject);
			}

			public static bool GetBordersBottomEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersShadowEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersShadowEnabled(designer, borderObject);
			}

			public static bool GetBordersShadowEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || StiInplaceEditor.IsEditorActivated)
				{
					return false;
				}
				if (!(borderObject is StiBorder))
				{
					return borderObject is StiSimpleShadow;
				}
				return true;
			}

			public static bool GetBordersFillBrushEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersFillBrushEnabled(designer, borderObject);
			}

			public static bool GetBordersFillBrushEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersColorEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersColorEnabled(designer, borderObject);
			}

			public static bool GetBordersColorEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetBordersStyleEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				object borderObject = designer.Report?.GetCurrentPage().GetBorder();
				return GetBordersStyleEnabled(designer, borderObject);
			}

			public static bool GetBordersStyleEnabled(StiDesignerControl designer, object borderObject)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				return borderObject != null;
			}

			public static bool GetFillBrushEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				StiBrush brush = ((designer.Report != null) ? (designer.Report.GetCurrentPage().GetBrush() as StiBrush) : null);
				return GetFillBrushEnabled(designer, brush);
			}

			public static bool GetFillBrushEnabled(StiDesignerControl designer, StiBrush brush)
			{
				if (designer.Report == null || (StiInplaceEditor.IsEditorActivated && designer.IsDashboardSelected))
				{
					return false;
				}
				StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
				if (selectedComponentsOnPage.Count > 0)
				{
					StiComponent stiComponent = selectedComponentsOnPage[0];
					if (stiComponent is IStiBackColor && StiAppExpressionHelper.IsExpressionSpecified(stiComponent, "BackColor"))
					{
						return false;
					}
					if (stiComponent is IStiBrush && StiAppExpressionHelper.IsExpressionSpecified(stiComponent, "Brush"))
					{
						return false;
					}
				}
				return brush != null;
			}
		}

		public static void SetBorder(StiDesignerControl designer, StiBorderSides side, bool value)
		{
			StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
			if (designer.Report.GetCurrentPage().IsSelected)
			{
				selectedComponentsOnPage.Add(designer.Report.GetCurrentPage());
			}
			foreach (StiComponent item in selectedComponentsOnPage)
			{
				SetBorderOne(item, side, value);
			}
			((Control)designer).Refresh();
		}

		public static void SetBorderOne(StiComponent comp, StiBorderSides side, bool value)
		{
			if (comp is IStiSimpleBorder)
			{
				if (!(comp is IStiSimpleBorder stiSimpleBorder))
				{
					return;
				}
				switch (side)
				{
				case StiBorderSides.All:
					stiSimpleBorder.Border.Side = StiBorderSides.All;
					return;
				case StiBorderSides.None:
					stiSimpleBorder.Border.Side = StiBorderSides.None;
					return;
				}
				if (value)
				{
					stiSimpleBorder.Border.Side = stiSimpleBorder.Border.Side | side;
				}
				else
				{
					stiSimpleBorder.Border.Side = stiSimpleBorder.Border.Side ^ side;
				}
			}
			else
			{
				if (!(comp is IStiBorder) || !(comp is IStiBorder stiBorder))
				{
					return;
				}
				switch (side)
				{
				case StiBorderSides.All:
					stiBorder.Border.Side = StiBorderSides.All;
					return;
				case StiBorderSides.None:
					stiBorder.Border.Side = StiBorderSides.None;
					return;
				}
				if (value)
				{
					stiBorder.Border.Side = stiBorder.Border.Side | side;
				}
				else
				{
					stiBorder.Border.Side = stiBorder.Border.Side ^ side;
				}
			}
		}

		public static void SetBorderShadow(StiDesignerControl designer, bool dropShadow)
		{
			StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
			if (designer.Report.GetCurrentPage().IsSelected)
			{
				selectedComponentsOnPage.Add(designer.Report.GetCurrentPage());
			}
			foreach (StiComponent item in selectedComponentsOnPage)
			{
				if (item is IStiBorder stiBorder)
				{
					stiBorder.Border.DropShadow = dropShadow;
				}
				if (item is IStiSimpleShadow stiSimpleShadow)
				{
					stiSimpleShadow.Shadow.Visible = dropShadow;
				}
			}
			((Control)designer).Refresh();
		}

		public static void SetBorderColor(StiDesignerControl designer, Color color)
		{
			StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
			if (designer.Report.GetCurrentPage().IsSelected)
			{
				selectedComponentsOnPage.Add(designer.Report.GetCurrentPage());
			}
			foreach (StiComponent item in selectedComponentsOnPage)
			{
				if (item is IStiSimpleBorder stiSimpleBorder)
				{
					stiSimpleBorder.Border.Color = color;
				}
				if (item is IStiBorder stiBorder)
				{
					stiBorder.Border.Color = color;
				}
				if (item is IStiBorderColor stiBorderColor)
				{
					stiBorderColor.BorderColor = color;
				}
			}
			((Control)designer).Refresh();
		}

		public static void SetBorderStyle(StiDesignerControl designer, StiPenStyle style)
		{
			StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
			if (designer.Report.GetCurrentPage().IsSelected)
			{
				selectedComponentsOnPage.Add(designer.Report.GetCurrentPage());
			}
			foreach (StiComponent item in selectedComponentsOnPage)
			{
				if (item is IStiSimpleBorder stiSimpleBorder)
				{
					stiSimpleBorder.Border.Style = style;
				}
				if (item is IStiBorder stiBorder)
				{
					stiBorder.Border.Style = style;
				}
			}
			((Control)designer).Refresh();
		}

		public static void InvokeBorderAll(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesAll"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.All, value: true);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderNone(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesNone"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.None, value: false);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderTop(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesTop"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.Top, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderLeft(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.Left, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderBottom(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.Bottom, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderRight(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiBorderSidesRight"), onlyCurrentPage: true);
				SetBorder(designer, StiBorderSides.Right, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderShadow(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Shadow"), onlyCurrentPage: true);
				SetBorderShadow(designer, value);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeFillBrush(StiDesignerControl designer, Color fillColor)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Background"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetBrush(new StiSolidBrush(fillColor));
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderColor(StiDesignerControl designer, Color borderColor)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "BorderColor"), onlyCurrentPage: true);
				SetBorderColor(designer, borderColor);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBorderStyle(StiDesignerControl designer, StiPenStyle style)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "BorderStyle"), onlyCurrentPage: true);
				SetBorderStyle(designer, style);
				designer.GetStyleFromComponents();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBordersDialog(StiDesignerControl designer)
		{
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Invalid comparison between Unknown and I4
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Invalid comparison between Unknown and I4
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Borders"), onlyCurrentPage: true);
				StiComponentsCollection selected = designer.GetSelected();
				StiComponent stiComponent = null;
				foreach (StiComponent item in selected)
				{
					if (item is IStiBorder)
					{
						stiComponent = item;
						break;
					}
					if (item is IStiSimpleBorder)
					{
						stiComponent = item;
						break;
					}
				}
				if (stiComponent == null)
				{
					return;
				}
				if (selected.ToList().Any((StiComponent c) => c is IStiSimpleBorder))
				{
					StiSimpleBorderEditorForm stiSimpleBorderEditorForm = new StiSimpleBorderEditorForm(((IStiSimpleBorder)stiComponent).Border);
					try
					{
						if ((int)((Form)stiSimpleBorderEditorForm).ShowDialog() == 1)
						{
							foreach (StiComponent item2 in selected)
							{
								if (item2 is IStiSimpleBorder)
								{
									((IStiSimpleBorder)item2).Border = stiSimpleBorderEditorForm.Border.Clone() as StiSimpleBorder;
								}
							}
							((Control)designer).Refresh();
							designer.InvokeSelectedObjectsChanged();
						}
					}
					finally
					{
						((IDisposable)stiSimpleBorderEditorForm)?.Dispose();
					}
				}
				else
				{
					StiBorderEditorForm stiBorderEditorForm = new StiBorderEditorForm(((IStiBorder)stiComponent).Border);
					try
					{
						if ((int)((Form)stiBorderEditorForm).ShowDialog() == 1)
						{
							foreach (StiComponent item3 in selected)
							{
								if (item3 is IStiBorder)
								{
									((IStiBorder)item3).Border = stiBorderEditorForm.Border.Clone() as StiBorder;
								}
							}
							((Control)designer).Refresh();
							designer.InvokeSelectedObjectsChanged();
						}
					}
					finally
					{
						((IDisposable)stiBorderEditorForm)?.Dispose();
					}
				}
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class TextFormat
	{
		public sealed class States
		{
			public static string GetTextFormatString(StiDesignerControl designer)
			{
				StiFormatService service = designer.Report?.GetCurrentPage().GetTextFormat();
				return GetTextFormatString(designer, service);
			}

			public static string GetTextFormatString(StiDesignerControl designer, StiFormatService service)
			{
				if (service == null)
				{
					return StiLocalization.Get("Toolbars", "ToolbarTextFormat");
				}
				return service.ServiceName;
			}

			public static bool GetTextFormatEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiFormatService service = designer.Report?.GetCurrentPage().GetTextFormat();
				return GetTextFormatEnabled(designer, service);
			}

			public static bool GetTextFormatEnabled(StiDesignerControl designer, StiFormatService service)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return service != null;
			}

			public static bool GetDateTimeFormatEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiFormatService service = designer.Report?.GetCurrentPage().GetTextFormat();
				return GetDateTimeFormatEnabled(designer, service);
			}

			public static bool GetDateTimeFormatEnabled(StiDesignerControl designer, StiFormatService service)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return service != null;
			}

			public static bool GetCurrencySymbolEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiFormatService service = designer.Report?.GetCurrentPage().GetTextFormat();
				return GetCurrencySymbolEnabled(designer, service);
			}

			public static bool GetCurrencySymbolEnabled(StiDesignerControl designer, StiFormatService service)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return service != null;
			}
		}

		public static void InvokeTextFormatDialog(StiDesignerControl designer)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Invalid comparison between Unknown and I4
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "TextFormat"), onlyCurrentPage: true);
				if (!((from c in designer.GetSelected().ToList()
					where c.GetFirstFormatObject() is IStiTextFormat
					select c.GetFirstFormatObject()).FirstOrDefault() is IStiTextFormat stiTextFormat))
				{
					return;
				}
				StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiTextFormat, designer);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
					if ((int)((Form)stiTextFormatEditorForm).ShowDialog() == 1)
					{
						designer.UndoRedoSave(StiLocalization.Get("Buttons", "Duplicate"));
						designer.Report.IsModified = true;
						designer.Report.GetCurrentPage().SetTextFormat(stiTextFormat.TextFormat);
						((Control)designer).Refresh();
						designer.InvokeSelectedObjectsChanged();
					}
				}
				finally
				{
					((IDisposable)stiTextFormatEditorForm)?.Dispose();
				}
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeGeneralTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiGeneralFormatService());
		}

		public static void InvokeNumberTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiNumberFormatService());
		}

		public static void InvokeCurrencyTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiCurrencyFormatService());
		}

		public static void InvokeDateTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiDateFormatService());
		}

		public static void InvokeTimeTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiTimeFormatService());
		}

		public static void InvokePercentageTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiPercentageFormatService());
		}

		public static void InvokeBooleanTextFormat(StiDesignerControl designer)
		{
			InvokeTextFormat(designer, new StiBooleanFormatService());
		}

		public static void InvokeTextFormat(StiDesignerControl designer, StiFormatService service)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "TextFormat"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetTextFormat(service);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class Account
	{
		public sealed class States
		{
			public static bool GetShareEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetSchedulerEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetPublishEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetReportPreviewEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}
		}
	}

	public sealed class Style
	{
		public sealed class States
		{
			public static bool GetStyleDesignerEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.Report.IsPageDesigner;
				}
				return false;
			}

			public static bool GetCopyStyleEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetStylesEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.SelectedObjects == null || (designer.SelectedObjects.Length == 1 && (designer.SelectedObjects.FirstOrDefault() is StiPage || designer.SelectedObjects.FirstOrDefault() is StiReport)) || designer.Report.IsPageDesigner)
				{
					return false;
				}
				return designer.SelectedObjects.Length != 0;
			}

			public static bool GetConditionsEnabled(StiDesignerControl designer)
			{
				if (designer.IsDashboardSelected)
				{
					StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault();
					if (stiComponent == null)
					{
						stiComponent = designer.Report.GetCurrentPage();
					}
					bool flag = stiComponent is IStiChartElement || stiComponent is IStiIndicatorElement || stiComponent is IStiProgressElement || stiComponent is IStiTableElement || stiComponent is IStiPivotTableElement;
					return designer.SelectedComponentsOnPage.ToList().Count == 1 && flag;
				}
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.SelectedObjects == null || (designer.SelectedObjects.Length == 1 && designer.SelectedObjects.FirstOrDefault() is StiReport) || designer.Report.IsPageDesigner)
				{
					return false;
				}
				return designer.SelectedObjects.Length != 0;
			}

			public static bool GetConditionsMarked(StiDesignerControl designer)
			{
				if (!GetConditionsEnabled(designer))
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault();
					if (stiComponent == null)
					{
						stiComponent = designer.Report.GetCurrentPage();
					}
					if (stiComponent is IStiChartElement stiChartElement)
					{
						return stiChartElement.FetchChartConditions().Count > 0;
					}
					if (stiComponent is IStiIndicatorElement stiIndicatorElement)
					{
						return stiIndicatorElement.FetchIndicatorConditions().Count > 0;
					}
					if (stiComponent is IStiProgressElement stiProgressElement)
					{
						return stiProgressElement.FetchProgressConditions().Count > 0;
					}
					if (stiComponent is IStiTableElement stiTableElement)
					{
						return stiTableElement.TableConditions.Count > 0;
					}
					if (stiComponent is IStiPivotTableElement stiPivotTableElement)
					{
						return stiPivotTableElement.PivotTableConditions.Count > 0;
					}
				}
				IEnumerable<object> enumerable = designer.SelectedObjects?.Where((object c) => c is StiComponent);
				if (enumerable != null && enumerable.Count() > 0)
				{
					return designer.SelectedObjects.Cast<StiComponent>().Any((StiComponent c) => c.Conditions.Count > 0);
				}
				return false;
			}

			public static bool GetInteractionEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.SelectedObjects == null || (designer.SelectedObjects.Length == 1 && designer.SelectedObjects.FirstOrDefault() is StiReport) || designer.Report.IsPageDesigner)
				{
					return false;
				}
				StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault();
				if (stiComponent == null)
				{
					stiComponent = designer.Report.GetCurrentPage();
				}
				if (stiComponent is StiTableOfContents)
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					return StiInplaceEditor.IsInteractionEditorAllowed(stiComponent as IStiElement);
				}
				return designer.SelectedObjects.Length != 0;
			}

			public static bool GetInteractionMarked(StiDesignerControl designer)
			{
				if (!GetInteractionEnabled(designer))
				{
					return false;
				}
				StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault();
				if (stiComponent == null)
				{
					stiComponent = designer.Report.GetCurrentPage();
				}
				if (stiComponent == null)
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					IStiDashboardInteraction stiDashboardInteraction = (stiComponent as IStiElementInteraction)?.DashboardInteraction;
					if (stiDashboardInteraction == null)
					{
						return false;
					}
					return !stiDashboardInteraction.IsDefault;
				}
				return !stiComponent.Interaction.IsDefaultExt;
			}
		}

		public static void InvokeInteraction(StiDesignerControl designer)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Invalid comparison between Unknown and I4
			try
			{
				if (designer.Report == null)
				{
					return;
				}
				StiComponent stiComponent = designer.SelectedComponentsOnPage.ToList().FirstOrDefault();
				if (stiComponent == null)
				{
					stiComponent = designer.Report.GetCurrentPage();
				}
				if (stiComponent is IStiElement || stiComponent is IStiDashboard)
				{
					StiInplaceEditor.OnInteractionEditorClick(stiComponent);
					return;
				}
				StiInteractionEditorForm stiInteractionEditorForm = new StiInteractionEditorForm(stiComponent);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiInteractionEditorForm);
					if ((int)((Form)stiInteractionEditorForm).ShowDialog() == 1)
					{
						((Control)designer).Refresh();
						designer.InvokeSelectedObjectsChanged();
					}
				}
				finally
				{
					((IDisposable)stiInteractionEditorForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeConditions(StiDesignerControl designer)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Invalid comparison between Unknown and I4
			try
			{
				if (designer.Report == null)
				{
					return;
				}
				if (designer.SelectedObject is IStiElement)
				{
					IStiElementConditionsEditorForm elementConditionsEditorFormFactory = StiDashboardDesignAssembly.GetElementConditionsEditorFormFactory((IStiElement)designer.SelectedObject);
					if (elementConditionsEditorFormFactory != null)
					{
						Form form = (Form)((elementConditionsEditorFormFactory is Form) ? elementConditionsEditorFormFactory : null);
						designer?.AddOwnedForm(form);
						elementConditionsEditorFormFactory.ShowDialog();
						if (designer != null)
						{
							((Control)designer).Refresh();
						}
						designer?.Builder.Redraw();
						return;
					}
				}
				StiComponentsCollection stiComponentsCollection = null;
				StiConditionsCollection stiConditionsCollection = null;
				Hashtable globalConditions = new Hashtable();
				stiComponentsCollection = ((designer.SelectedComponentsOnPage.Count <= 0) ? new StiComponentsCollection { designer.Report.GetCurrentPage() } : designer.SelectedComponentsOnPage);
				stiConditionsCollection = StiConditionsHelper.GetConditions(stiComponentsCollection, ref globalConditions);
				StiConditionsEditorForm stiConditionsEditorForm = new StiConditionsEditorForm(designer.Report, stiConditionsCollection, stiComponentsCollection[0]);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiConditionsEditorForm);
					if ((int)((Form)stiConditionsEditorForm).ShowDialog() == 1)
					{
						StiConditionsHelper.SetConditions(stiComponentsCollection, stiConditionsCollection, globalConditions);
					}
					((Control)designer).Refresh();
					designer.InvokeSelectedObjectsChanged();
					stiComponentsCollection = null;
					stiConditionsCollection = null;
					globalConditions = null;
				}
				finally
				{
					((IDisposable)stiConditionsEditorForm)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeStyleDesigner(StiDesignerControl designer)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(designer, designer.Report);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiStyleDesigner);
					((Form)stiStyleDesigner).ShowDialog();
					designer.Report.ApplyStyles();
					((Control)designer).Refresh();
					designer.InvokeSelectedObjectsChanged();
				}
				finally
				{
					((IDisposable)stiStyleDesigner)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSelectDashboardAutoStyle(StiDesignerControl designer)
		{
			try
			{
				if (designer.Report == null)
				{
					return;
				}
				if (StiInplaceDesigner.IsEditorActivated)
				{
					if (StiInplaceDesigner.EditableComponent is IStiDashboardElementStyle)
					{
						IStiDashboardElementStyle stiDashboardElementStyle = (IStiDashboardElementStyle)StiInplaceDesigner.EditableComponent;
						stiDashboardElementStyle.Style = StiElementStyleIdent.Auto;
					}
					StiInplaceDesigner.InvokeStyleApplied();
					((Control)designer).Refresh();
					designer.InvokeSelectedObjectsChanged();
					return;
				}
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Style"), onlyCurrentPage: true);
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (item is IStiElement || item is IStiComponentUI)
					{
						StiComponent stiComponent2 = item;
						if (StiInplaceDesigner.IsEditorActivated && StiInplaceDesigner.OriginalComponent == stiComponent2)
						{
							stiComponent2 = StiInplaceDesigner.EditableComponent;
						}
						if (stiComponent2 is IStiDashboardElementStyle)
						{
							IStiDashboardElementStyle stiDashboardElementStyle2 = (IStiDashboardElementStyle)stiComponent2;
							stiDashboardElementStyle2.Style = StiElementStyleIdent.Auto;
						}
					}
				}
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSelectStyle(StiDesignerControl designer, StiBaseStyle style)
		{
			try
			{
				if (designer.Report == null)
				{
					return;
				}
				if (StiInplaceDesigner.IsEditorActivated && designer.Report.GetCurrentPage().IsDashboardOrScreen)
				{
					InvokeSelectStyle(StiInplaceDesigner.EditableComponent, style);
					StiInplaceDesigner.InvokeStyleApplied();
					((Control)designer).Refresh();
					designer.InvokeSelectedObjectsChanged();
					return;
				}
				if (!StiInplaceDesigner.IsEditorActivated || !StiInplaceDesigner.AllForms.Any((StiInplaceEditorForm f) => f is StiChartEditorFormV2))
				{
					designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Style"), onlyCurrentPage: true);
				}
				if (style is StiDashboardStyle)
				{
					if (style is StiDashboardStyle stiDashboardStyle)
					{
						if (designer.Report.GetCurrentPage() is IStiDashboard stiDashboard)
						{
							stiDashboard.Style = stiDashboardStyle.Ident;
						}
						if (designer.Report.GetCurrentPage() is IStiScreen stiScreen)
						{
							stiScreen.Style = stiDashboardStyle.Ident;
						}
					}
				}
				else
				{
					foreach (StiComponent item in designer.SelectedComponentsOnPage)
					{
						if (item is IStiElement)
						{
							StiComponent stiComponent2 = item;
							if (StiInplaceDesigner.IsEditorActivated && StiInplaceDesigner.OriginalComponent == stiComponent2)
							{
								stiComponent2 = StiInplaceDesigner.EditableComponent;
							}
							InvokeSelectStyle(stiComponent2, style);
						}
						else if (item is IStiComponentUI)
						{
							StiComponent stiComponent3 = item;
							if (StiInplaceDesigner.IsEditorActivated && StiInplaceDesigner.OriginalComponent == stiComponent3)
							{
								stiComponent3 = StiInplaceDesigner.EditableComponent;
							}
							InvokeSelectStyle(stiComponent3, style);
						}
						else if (item is StiChart)
						{
							StiChart stiChart = (StiChart)item;
							if (style is StiChartStyle || style is StiCustomStyle)
							{
								stiChart.CustomStyleName = style.Name;
								stiChart.Core.ApplyStyle(new StiCustomStyle(style.Name));
							}
							else
							{
								stiChart.CustomStyleName = "";
								stiChart.Core.ApplyStyle(style.Clone() as IStiChartStyle);
							}
						}
						else if (item is StiMap)
						{
							StiMap stiMap = (StiMap)item;
							if (style is StiMapStyleFX)
							{
								stiMap.ComponentStyle = null;
								stiMap.MapStyle = ((StiMapStyleFX)style).StyleId;
							}
							else if (style is StiMapStyle)
							{
								stiMap.ComponentStyle = style.Name;
							}
						}
						else if (item is StiGauge)
						{
							StiGauge stiGauge = (StiGauge)item;
							if (style is IStiGaugeStyle)
							{
								stiGauge.Style = (IStiGaugeStyle)style.Clone();
								stiGauge.CustomStyleName = null;
							}
							else if (style is StiGaugeStyle)
							{
								stiGauge.CustomStyleName = ((StiGaugeStyle)style).Name;
							}
						}
						else if (item is StiCrossTab)
						{
							StiCrossTab stiCrossTab = (StiCrossTab)item;
							if (style == null)
							{
								stiCrossTab.CrossTabStyle = null;
							}
							else
							{
								if (!(style is StiCrossTabStyle))
								{
									continue;
								}
								StiCrossTabStyle stiCrossTabStyle = (StiCrossTabStyle)style;
								if (string.IsNullOrEmpty(stiCrossTabStyle.Name))
								{
									Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
									for (int i = 0; i < styleColors.Length; i++)
									{
										if (styleColors[i] == stiCrossTabStyle.Color)
										{
											stiCrossTab.CrossTabStyleIndex = i;
											break;
										}
									}
								}
								else
								{
									stiCrossTab.CrossTabStyle = stiCrossTabStyle.Name;
								}
							}
						}
						else if (item is StiTable)
						{
							StiTable stiTable = (StiTable)item;
							if (style is StiTableStyleFX)
							{
								stiTable.ComponentStyle = null;
								stiTable.TableStyleFX = (StiTableStyleFX)style.Clone();
							}
							else
							{
								stiTable.ComponentStyle = style.Name;
							}
						}
						else if (style == null)
						{
							item.ComponentStyle = string.Empty;
						}
						else
						{
							item.ComponentStyle = style.Name;
						}
					}
				}
				designer.Report.IsModified = true;
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
				designer.InvokeSelectedStylesChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		private static void InvokeSelectStyle(StiComponent component, StiBaseStyle style)
		{
			if (component is IStiGaugeElement)
			{
				IStiGaugeElement stiGaugeElement = (IStiGaugeElement)component;
				if (style is StiGaugeStyleXF)
				{
					stiGaugeElement.Style = ((StiGaugeStyleXF)style).StyleIdent;
					stiGaugeElement.CustomStyleName = null;
				}
				else
				{
					stiGaugeElement.Style = StiElementStyleIdent.Custom;
					stiGaugeElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiChartElement)
			{
				IStiChartElement stiChartElement = (IStiChartElement)component;
				if (style is Stimulsoft.Report.Chart.StiChartStyle)
				{
					stiChartElement.Style = ((Stimulsoft.Report.Chart.StiChartStyle)style).StyleIdent;
					stiChartElement.CustomStyleName = null;
				}
				else
				{
					stiChartElement.Style = StiElementStyleIdent.Custom;
					stiChartElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiRegionMapElement)
			{
				IStiRegionMapElement stiRegionMapElement = (IStiRegionMapElement)component;
				if (style is StiMapStyleFX)
				{
					stiRegionMapElement.Style = ((StiMapStyleFX)style).StyleIdent;
					stiRegionMapElement.CustomStyleName = null;
				}
				else
				{
					stiRegionMapElement.Style = StiElementStyleIdent.Custom;
					stiRegionMapElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiControlElement)
			{
				IStiControlElement stiControlElement = (IStiControlElement)component;
				if (style is StiControlElementStyle)
				{
					StiControlElementStyle stiControlElementStyle = (StiControlElementStyle)style;
					stiControlElement.Style = stiControlElementStyle.Ident;
					stiControlElement.CustomStyleName = null;
				}
				else
				{
					stiControlElement.Style = StiElementStyleIdent.Custom;
					stiControlElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiIndicatorElement)
			{
				IStiIndicatorElement stiIndicatorElement = (IStiIndicatorElement)component;
				if (style is StiIndicatorElementStyle)
				{
					StiIndicatorElementStyle stiIndicatorElementStyle = (StiIndicatorElementStyle)style;
					stiIndicatorElement.Style = stiIndicatorElementStyle.Ident;
					stiIndicatorElement.CustomStyleName = null;
				}
				else
				{
					stiIndicatorElement.Style = StiElementStyleIdent.Custom;
					stiIndicatorElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiProgressElement)
			{
				IStiProgressElement stiProgressElement = (IStiProgressElement)component;
				if (style is StiProgressElementStyle)
				{
					StiProgressElementStyle stiProgressElementStyle = (StiProgressElementStyle)style;
					stiProgressElement.Style = stiProgressElementStyle.Ident;
					stiProgressElement.CustomStyleName = null;
				}
				else
				{
					stiProgressElement.Style = StiElementStyleIdent.Custom;
					stiProgressElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiTableElement)
			{
				IStiTableElement stiTableElement = (IStiTableElement)component;
				if (style is StiTableElementStyle)
				{
					StiTableElementStyle stiTableElementStyle = (StiTableElementStyle)style;
					stiTableElement.Style = stiTableElementStyle.Ident;
					stiTableElement.CustomStyleName = null;
				}
				else
				{
					stiTableElement.Style = StiElementStyleIdent.Custom;
					stiTableElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiCardsElement stiCardsElement)
			{
				if (style is StiCardsElementStyle)
				{
					StiCardsElementStyle stiCardsElementStyle = (StiCardsElementStyle)style;
					stiCardsElement.Style = stiCardsElementStyle.Ident;
					stiCardsElement.CustomStyleName = null;
				}
				else
				{
					stiCardsElement.Style = StiElementStyleIdent.Custom;
					stiCardsElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiPivotTableElement)
			{
				IStiPivotTableElement stiPivotTableElement = (IStiPivotTableElement)component;
				if (style is StiPivotElementStyle)
				{
					StiPivotElementStyle stiPivotElementStyle = (StiPivotElementStyle)style;
					stiPivotTableElement.Style = stiPivotElementStyle.Ident;
					stiPivotTableElement.CustomStyleName = null;
				}
				else
				{
					stiPivotTableElement.Style = StiElementStyleIdent.Custom;
					stiPivotTableElement.CustomStyleName = style.Name;
				}
			}
			else if (component is IStiComponentUI && component is IStiDashboardElementStyle)
			{
				IStiDashboardElementStyle stiDashboardElementStyle = (IStiDashboardElementStyle)component;
				if (style is StiControlElementStyle)
				{
					StiControlElementStyle stiControlElementStyle2 = (StiControlElementStyle)style;
					stiDashboardElementStyle.Style = stiControlElementStyle2.Ident;
					stiDashboardElementStyle.CustomStyleName = null;
				}
				else
				{
					stiDashboardElementStyle.Style = StiElementStyleIdent.Custom;
					stiDashboardElementStyle.CustomStyleName = style.Name;
				}
			}
		}
	}

	public class Layout
	{
		public sealed class States
		{
			public static StiComponentsCollection GetSizeComponentsCollection(StiDesignerControl designer)
			{
				StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (StiRestrictionsHelper.IsAllowChange(item))
					{
						stiComponentsCollection.Add(item);
					}
				}
				stiComponentsCollection.SortBySelectionTick();
				return stiComponentsCollection;
			}

			public static StiComponentsCollection GetSelectedComponentsCollection(StiDesignerControl designer)
			{
				StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
				foreach (StiComponent item in designer.SelectedComponentsOnPage)
				{
					if (StiRestrictionsHelper.IsAllowChange(item))
					{
						stiComponentsCollection.Add(item);
					}
				}
				return stiComponentsCollection;
			}

			public static string GetMakeSameSizeText(StiDesignerControl designer)
			{
				return GetMakeSameSizeText(designer, GetSizeComponentsCollection(designer));
			}

			public static string GetMakeSameSizeText(StiDesignerControl designer, StiComponentsCollection sizeCollection)
			{
				if (sizeCollection.Count > 0)
				{
					return StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameSize"), sizeCollection[0].Name));
				}
				return string.Empty;
			}

			public static string GetMakeSameWidthText(StiDesignerControl designer)
			{
				return GetMakeSameWidthText(designer, GetSizeComponentsCollection(designer));
			}

			public static string GetMakeSameWidthText(StiDesignerControl designer, StiComponentsCollection sizeCollection)
			{
				if (sizeCollection.Count > 0)
				{
					return StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameWidth"), sizeCollection[0].Name));
				}
				return string.Empty;
			}

			public static string GetMakeSameHeightText(StiDesignerControl designer)
			{
				return GetMakeSameHeightText(designer, GetSizeComponentsCollection(designer));
			}

			public static string GetMakeSameHeightText(StiDesignerControl designer, StiComponentsCollection sizeCollection)
			{
				if (sizeCollection.Count > 0)
				{
					return StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameHeight"), sizeCollection[0].Name));
				}
				return string.Empty;
			}

			public static bool GetSizeEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetSizeEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetSizeEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count > 1;
				}
				return false;
			}

			public static bool GetMakeSameSizeEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetMakeSameSizeEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMakeSameSizeEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count > 1;
				}
				return false;
			}

			public static bool GetMakeSameWidthEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetMakeSameWidthEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMakeSameWidthEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count > 1;
				}
				return false;
			}

			public static bool GetMakeSameHeightEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetMakeSameHeightEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMakeSameHeightEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count > 1;
				}
				return false;
			}

			public static bool GetBringToFrontEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetBringToFrontEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetBringToFrontEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 2;
				}
				return false;
			}

			public static bool GetSendToBackEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetSendToBackEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetSendToBackEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 2;
				}
				return false;
			}

			public static bool GetMoveForwardEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 2;
				}
				return false;
			}

			public static bool GetMoveBackwardEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetBringToFrontEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMoveBackwardEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 2;
				}
				return false;
			}

			public static bool GetLinkEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetLinkEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetLinkEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 1;
				}
				return false;
			}

			public static bool GetLockEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetLockEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetLockEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 1;
				}
				return false;
			}

			public static bool GetAlignToGridEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignToGridEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignToGridEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 1;
				}
				return false;
			}

			public static bool GetCenterHorizontallyEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetCenterHorizontallyEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetCenterHorizontallyEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 1;
				}
				return false;
			}

			public static bool GetCenterVerticallyEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetCenterVerticallyEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetCenterVerticallyEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null && selectedComponents.Count >= 1)
				{
					return designer.ComponentsOnPage.Count >= 1;
				}
				return false;
			}

			public static bool GetAlignEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 1;
				}
				return false;
			}

			public static bool GetAlignLeftEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignLeftEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignLeftEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetAlignCenterEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignCenterEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignCenterEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetAlignRightEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignRightEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignRightEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetAlignTopEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignTopEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignTopEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetAlignMiddleEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignMiddleEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignMiddleEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetAlignBottomEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetAlignBottomEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetAlignBottomEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetMakeHorizontalSpacingEqualEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetMakeHorizontalSpacingEqualEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMakeHorizontalSpacingEqualEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetMakeVerticalSpacingEqualEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return GetMakeVerticalSpacingEqualEnabled(designer, GetSelectedComponentsCollection(designer));
			}

			public static bool GetMakeVerticalSpacingEqualEnabled(StiDesignerControl designer, StiComponentsCollection selectedComponents)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.Report != null)
				{
					return selectedComponents.Count >= 2;
				}
				return false;
			}

			public static bool GetLockChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object @lock = designer.Report.GetCurrentPage().GetLock();
				if (@lock == null)
				{
					return false;
				}
				return (bool)@lock;
			}

			public static bool GetLinkChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object link = designer.Report.GetCurrentPage().GetLink();
				if (link == null)
				{
					return false;
				}
				return (bool)link;
			}
		}

		public static void InvokeAlignToGrid(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignToGrid"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignToGrid();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignLeft(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignLeft"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Left);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignCenter(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "CenterHorizontally"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Center);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignRight(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignRight"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Right);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignTop(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignTop"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Top);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignMiddle(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "CenterVertically"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Middle);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAlignBottom(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "AlignBottom"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().AlignTo(StiAligning.Bottom);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMakeSameSize(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MakeSameSize"), onlyCurrentPage: true);
				StiComponentsCollection selectedComponents = designer.Report.GetCurrentPage().GetSelectedComponents();
				selectedComponents.SortBySelectionTick();
				designer.Report.GetCurrentPage().MakeSameSize(new SizeD(selectedComponents[0].Width, selectedComponents[0].Height));
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMakeSameWidth(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MakeSameWidth"), onlyCurrentPage: true);
				StiComponentsCollection selectedComponents = designer.Report.GetCurrentPage().GetSelectedComponents();
				selectedComponents.SortBySelectionTick();
				designer.Report.GetCurrentPage().MakeSameWidth(selectedComponents[0].Width);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMakeSameHeight(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MakeSameHeight"), onlyCurrentPage: true);
				StiComponentsCollection selectedComponents = designer.Report.GetCurrentPage().GetSelectedComponents();
				selectedComponents.SortBySelectionTick();
				designer.Report.GetCurrentPage().MakeSameHeight(selectedComponents[0].Height);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMakeHorizontalSpacingEqual(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().MakeHorizontalSpacingEqual();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMakeVerticalSpacingEqual(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().MakeVerticalSpacingEqual();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeCenterVertically(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "CenterVertically"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetCenterVertically();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeCenterHorizontally(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "CenterHorizontally"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetCenterHorizontally();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBringToFront(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "BringToFront"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().BringToFront();
				designer.Report.GetCurrentPage().Correct();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSendToBack(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "SendToBack"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SendToBack();
				designer.Report.GetCurrentPage().Correct();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMoveForward(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MoveForward"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().MoveForward();
				designer.Report.GetCurrentPage().Correct();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMoveBackward(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "MoveBackward"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().MoveBackward();
				designer.Report.GetCurrentPage().Correct();
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeLock(StiDesignerControl designer, bool lockState)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "Lock"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetLock(lockState);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeLink(StiDesignerControl designer, bool linkState)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "Link"), onlyCurrentPage: true);
				designer.Report.GetCurrentPage().SetLink(linkState);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class DockStyle
	{
		public sealed class States
		{
			public static bool GetDockStyleFillChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.Fill;
			}

			public static bool GetDockStyleNoneChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.None;
			}

			public static bool GetDockStyleLeftChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.Left;
			}

			public static bool GetDockStyleRightChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.Right;
			}

			public static bool GetDockStyleTopChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.Top;
			}

			public static bool GetDockStyleBottomChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				object obj = designer.Report?.GetCurrentPage().GetDockStyle();
				if (obj == null)
				{
					return false;
				}
				return (StiDockStyle)obj == StiDockStyle.Bottom;
			}

			public static bool GetDockStyleFillEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}

			public static bool GetDockStyleNoneEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}

			public static bool GetDockStyleLeftEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}

			public static bool GetDockStyleRightEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}

			public static bool GetDockStyleTopEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}

			public static bool GetDockStyleBottomEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report?.GetCurrentPage().GetDockStyle() != null;
			}
		}

		public static void SetDockStyle(StiDesignerControl designer, bool fill, bool none, bool left, bool right, bool top, bool bottom)
		{
			try
			{
				if (designer.Report != null)
				{
					StiDockStyle dockStyle = StiDockStyle.None;
					if (bottom)
					{
						dockStyle = StiDockStyle.Bottom;
					}
					else if (top)
					{
						dockStyle = StiDockStyle.Top;
					}
					else if (left)
					{
						dockStyle = StiDockStyle.Left;
					}
					else if (right)
					{
						dockStyle = StiDockStyle.Right;
					}
					else if (fill)
					{
						dockStyle = StiDockStyle.Fill;
					}
					designer.Report.GetCurrentPage().SetDockStyle(dockStyle);
					((Control)designer).Refresh();
					designer.InvokeSelectedObjectsChanged();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeDockStyleFill(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleFill"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: true, none: false, left: false, right: false, top: false, bottom: false);
		}

		public static void InvokeDockStyleNone(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleNone"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: false, none: true, left: false, right: false, top: false, bottom: false);
		}

		public static void InvokeDockStyleTop(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleTop"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: false, none: false, left: false, right: false, top: true, bottom: false);
		}

		public static void InvokeDockStyleLeft(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleLeft"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: false, none: false, left: true, right: false, top: false, bottom: false);
		}

		public static void InvokeDockStyleBottom(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleBottom"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: false, none: false, left: false, right: false, top: false, bottom: true);
		}

		public static void InvokeDockStyleRight(StiDesignerControl designer)
		{
			designer.UndoRedoSave(StiLocalization.Get("PropertyEnum", "StiDockStyleRight"), onlyCurrentPage: true);
			SetDockStyle(designer, fill: false, none: false, left: false, right: true, top: false, bottom: false);
		}
	}

	public sealed class Page
	{
		public sealed class States
		{
			public static bool GetOrientationPortraitChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (designer.IsDashboardSelected)
				{
					return currentPage.Width <= currentPage.Height;
				}
				return currentPage.Orientation == StiPageOrientation.Portrait;
			}

			public static bool GetOrientationLandscapeChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (designer.IsDashboardSelected)
				{
					return currentPage.Width > currentPage.Height;
				}
				return currentPage.Orientation == StiPageOrientation.Landscape;
			}

			public static bool GetColumnsOneChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Columns < 2;
			}

			public static bool GetColumnsTwoChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Columns == 2;
			}

			public static bool GetColumnsThreeChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Columns == 3;
			}

			public static bool GetSurfaceViewDesktopChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage.IsDashboard || currentPage.IsScreen)
				{
					return ((IStiSurface)currentPage).IsDesktopSurfaceSelected;
				}
				return false;
			}

			public static bool GetSurfaceViewMobileChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage.IsDashboard || currentPage.IsScreen)
				{
					return ((IStiSurface)currentPage).IsMobileSurfaceSelected;
				}
				return false;
			}

			public static bool GetSurfaceViewMobilePresent(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage.IsDashboard || currentPage.IsScreen)
				{
					if (!((IStiSurface)currentPage).IsMobileSurfaceSelected)
					{
						return ((IStiSurface)currentPage).IsMobileSurfacePresent;
					}
					return true;
				}
				return false;
			}

			public static bool GetSurfaceViewComponentsVisible(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				StiPage currentPage = designer.Report.GetCurrentPage();
				if ((currentPage.IsDashboard || currentPage.IsScreen) && ((IStiSurface)currentPage).IsMobileSurfaceSelected)
				{
					return StiUnplacedElementsHelper.ElementsForm != null;
				}
				return false;
			}

			public static bool GetSurfaceViewComponentsEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (!(designer.Report.GetCurrentPage() is IStiDashboard stiDashboard))
				{
					return false;
				}
				if (GetSurfaceViewMobileChecked(designer) && GetSurfaceViewMobilePresent(designer))
				{
					return stiDashboard.GetUnplacedElements().Any();
				}
				return false;
			}

			public static bool GetMarginsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.IsFormOrScreenSelected;
				}
				return false;
			}

			public static bool GetOrientationEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.IsFormOrScreenSelected;
				}
				return false;
			}

			public static bool GetPageSizeEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.IsFormOrScreenSelected;
				}
				return false;
			}

			public static bool GetColumnsEnabled(StiDesignerControl designer)
			{
				if (designer.Report != null)
				{
					return !designer.IsFormOrScreenSelected;
				}
				return false;
			}

			public static bool GetPageSetupDialogEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				_ = designer.IsNotPageSelected;
				return true;
			}
		}

		public static void InvokeMarginsNormal(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "MarginsNormal"), onlyCurrentPage: true);
				decimal d = (decimal)designer.Report.Unit.ConvertFromHInches(39.0);
				d = Math.Round(d, 1);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Margins = new StiMargins((double)d, (double)d, (double)d, (double)d);
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMarginsNarrow(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "MarginsNarrow"), onlyCurrentPage: true);
				decimal d = (decimal)designer.Report.Unit.ConvertFromHInches(39.0);
				d = Math.Round(d, 1) / 2m;
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Margins = new StiMargins((double)d, (double)d, (double)d, (double)d);
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeMarginsWide(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "MarginsWide"), onlyCurrentPage: true);
				decimal d = (decimal)designer.Report.Unit.ConvertFromHInches(39.0);
				d = Math.Round(d, 1) * 2m;
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Margins = new StiMargins((double)d, (double)d, (double)d, (double)d);
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeOrientationPortrait(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormPageSetup", "PageOrientationPortrait"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (designer.IsDashboardSelected)
				{
					double width = currentPage.Width;
					double height = currentPage.Height;
					currentPage.Height = width;
					currentPage.Width = height;
					((StiDesignerControl)designer.DesignerControl)?.Builder.BuildReport();
					StiDashboardSizeHelper.Place(currentPage as IStiDashboard, width, height);
				}
				else
				{
					currentPage.Orientation = StiPageOrientation.Portrait;
				}
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeOrientationLandscape(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormPageSetup", "PageOrientationLandscape"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (designer.IsDashboardSelected)
				{
					double width = currentPage.Width;
					double height = currentPage.Height;
					currentPage.Height = width;
					currentPage.Width = height;
					((StiDesignerControl)designer.DesignerControl)?.Builder.BuildReport();
					StiDashboardSizeHelper.Place(currentPage as IStiDashboard, width, height);
				}
				else
				{
					currentPage.Orientation = StiPageOrientation.Landscape;
				}
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeColumnsOne(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "ColumnsOne"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Columns = 0;
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeColumnsTwo(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "ColumnsTwo"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Columns = 2;
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeColumnsThree(StiDesignerControl designer)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("FormDesigner", "ColumnsThree"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Columns = 3;
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSurfaceViewDesktop(StiDesignerControl designer)
		{
			try
			{
				if (designer.Report.GetCurrentPage() is IStiSurface { IsDesktopSurfaceSelected: false } stiSurface)
				{
					stiSurface.SwitchSurfaceViewMode(StiSurfaceViewMode.Desktop);
					designer.Builder.BuildReport();
					InvokeSurfaceUnplacedElementsWizard(designer, stiSurface);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSurfaceViewMobile(StiDesignerControl designer)
		{
			try
			{
				if (designer.Report.GetCurrentPage() is IStiSurface { IsMobileSurfaceSelected: false } stiSurface)
				{
					stiSurface.SwitchSurfaceViewMode(StiSurfaceViewMode.Mobile);
					designer.Builder.BuildReport();
					InvokeSurfaceUnplacedElementsWizard(designer, stiSurface);
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSurfaceViewRemoveMobileSurface(StiDesignerControl designer)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			try
			{
				if (designer.Report.GetCurrentPage() is IStiSurface { IsMobileSurfacePresent: not false } stiSurface)
				{
					DialogResult val = MessageBox.Show(Loc.Get("Questions", "qnRemove"), Loc.Get("FormTitles", "DesignerApplication"), (MessageBoxButtons)4, (MessageBoxIcon)32, (MessageBoxDefaultButton)256);
					if ((int)val == 6)
					{
						stiSurface.SwitchSurfaceViewMode(StiSurfaceViewMode.Desktop);
						stiSurface.RemoveMobileSurface();
						designer.Builder.BuildReport();
						InvokeSurfaceUnplacedElementsWizard(designer, stiSurface);
					}
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeSurfaceViewComponents(StiDesignerControl designer)
		{
			try
			{
				if (designer.Report.GetCurrentPage() is IStiSurface { IsMobileSurfacePresent: not false } stiSurface)
				{
					IStiUnplacedElementsForm elementsForm = StiUnplacedElementsHelper.ElementsForm;
					Form val = (Form)((elementsForm is Form) ? elementsForm : null);
					if (val != null)
					{
						val.Close();
					}
					else
					{
						InvokeSurfaceUnplacedElementsWizard(designer, stiSurface);
					}
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		private static void InvokeSurfaceUnplacedElementsWizard(StiDesignerControl designer, IStiSurface page)
		{
			IStiUnplacedElementsForm elementsForm = StiUnplacedElementsHelper.ElementsForm;
			IStiUnplacedElementsForm obj = ((elementsForm is Form) ? elementsForm : null);
			if (obj != null)
			{
				((Form)obj).Close();
			}
			List<StiComponent> unplacedElements = page.GetUnplacedElements();
			if (unplacedElements != null && unplacedElements.Count != 0)
			{
				Form val = (Form)((page is IStiDashboard) ? /*isinst with value type is only supported in some contexts*/: /*isinst with value type is only supported in some contexts*/);
				if (val != null)
				{
					StiInplaceDesigner.AddEditor(val as StiInplaceEditorForm);
					StiPageViewControl currentPageView = designer.PageView.GetCurrentPageView();
					((Control)val).Width = StiScale.I(192);
					((Control)val).Height = ((Control)currentPageView).Height;
					Rectangle rectangle = ((Control)designer).RectangleToScreen(((Control)designer).ClientRectangle);
					Rectangle rectangle2 = ((Control)currentPageView).RectangleToScreen(((Control)currentPageView).ClientRectangle);
					Rectangle rectangle3 = ((Control)currentPageView.View).RectangleToScreen(((Control)currentPageView.View).ClientRectangle);
					Point point = new Point(rectangle.Right - ((Control)val).Width, rectangle.Y + (rectangle.Height - ((Control)val).Height) / 2);
					Point point2 = new Point(rectangle3.Right, rectangle2.Y);
					val.Location = ((point.X < point2.X) ? point : point2);
					designer?.AddOwnedForm(val);
					((Control)val).Show();
					designer.RibbonDesignerUserControl.RefreshPage();
				}
			}
		}

		public static void InvokePageSetupDialog(StiDesignerControl designer)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiComponentDesigner.GetComponentDesigner(designer, designer.Report.GetCurrentPage().GetType())?.Design(designer.Report.GetCurrentPage());
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class Watermark
	{
		public sealed class States
		{
			public static bool GetEditorEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (!designer.IsPageSelected && !designer.IsDashboardSelected)
				{
					return designer.IsScreenSelected;
				}
				return true;
			}
		}

		public static void InvokeWatermarkEditor(StiDesignerControl designer)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage.IsPage)
				{
					StiPageSetupForm stiPageSetupForm = new StiPageSetupForm(currentPage, designer, 2);
					try
					{
						designer.AddOwnedForm((Form)(object)stiPageSetupForm);
						((Form)stiPageSetupForm).ShowDialog();
						return;
					}
					finally
					{
						((IDisposable)stiPageSetupForm)?.Dispose();
					}
				}
				if (currentPage.IsDashboard)
				{
					IStiDashboardWatermark stiDashboardWatermark = currentPage.GetSelectedComponents().ToList().FirstOrDefault() as IStiDashboardWatermark;
					if (stiDashboardWatermark == null)
					{
						stiDashboardWatermark = currentPage as IStiDashboardWatermark;
					}
					Form watermarkFormFactory = StiDashboardDesignAssembly.GetWatermarkFormFactory(stiDashboardWatermark);
					try
					{
						designer.AddOwnedForm(watermarkFormFactory);
						watermarkFormFactory.ShowDialog();
						return;
					}
					finally
					{
						((IDisposable)watermarkFormFactory)?.Dispose();
					}
				}
				if (currentPage.IsScreen)
				{
					IStiWatermarkUI stiWatermarkUI = currentPage.GetSelectedComponents().ToList().FirstOrDefault() as IStiWatermarkUI;
					if (stiWatermarkUI == null)
					{
						stiWatermarkUI = currentPage as IStiWatermarkUI;
					}
					Form watermarkFormFactory2 = StiAppDesignAssembly.GetWatermarkFormFactory(stiWatermarkUI);
					try
					{
						designer.AddOwnedForm(watermarkFormFactory2);
						watermarkFormFactory2.ShowDialog();
						return;
					}
					finally
					{
						((IDisposable)watermarkFormFactory2)?.Dispose();
					}
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class WatermarkText
	{
		public sealed class States
		{
			public static bool GetFontBoldChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsFormSelected)
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Font.Bold;
			}

			public static bool GetFontItalicChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsFormSelected)
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Font.Italic;
			}

			public static bool GetFontUnderlineChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsFormSelected)
				{
					return false;
				}
				if (designer.IsDashboardSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Font.Underline;
			}

			public static string GetFontName(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return string.Empty;
				}
				if (designer.IsNotPageSelected)
				{
					return string.Empty;
				}
				if (designer.IsDashboardSelected)
				{
					return string.Empty;
				}
				return designer.Report.GetCurrentPage().Watermark.Font.Name;
			}

			public static float GetFontSize(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return 0f;
				}
				if (designer.IsNotPageSelected)
				{
					return 0f;
				}
				if (designer.IsDashboardSelected)
				{
					return 0f;
				}
				return designer.Report.GetCurrentPage().Watermark.Font.Size;
			}

			public static Color GetTextBrush(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return Color.White;
				}
				if (designer.IsNotPageSelected)
				{
					return Color.White;
				}
				return StiBrush.ToColor(designer.Report.GetCurrentPage().Watermark.TextBrush);
			}

			public static string GetText(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return string.Empty;
				}
				if (designer.IsNotPageSelected)
				{
					return string.Empty;
				}
				return designer.Report.GetCurrentPage().Watermark.Text;
			}

			public static float GetAngle(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return 0f;
				}
				if (designer.IsNotPageSelected)
				{
					return 0f;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle;
			}

			public static bool GetShowBehindChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.ShowBehind;
			}

			public static bool GetAngle0Checked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle == 0f;
			}

			public static bool GetAngle45Checked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle == 45f;
			}

			public static bool GetAngle90Checked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle == 90f;
			}

			public static bool GetAngle180Checked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle == 180f;
			}

			public static bool GetAngle270Checked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.Angle == 270f;
			}

			public static bool GetFontBoldEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetFontItalicEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetFontUnderlineEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetFontNameEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetFontSizeEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetTextBrushEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetTextEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetAngleEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetShowBehindEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}
		}

		public static void InvokeFontName(StiDesignerControl designer, string fontName)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Font = StiFontUtils.ChangeFontName(currentPage.Watermark.Font, fontName);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeFontSize(StiDesignerControl designer, float fontSize)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Font = StiFontUtils.ChangeFontSize(currentPage.Watermark.Font, fontSize);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeBold(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleBold"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Font = StiFontUtils.ChangeFontStyleBold(currentPage.Watermark.Font, value);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeItalic(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleItalic"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Font = StiFontUtils.ChangeFontStyleItalic(currentPage.Watermark.Font, value);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeUnderline(StiDesignerControl designer, bool value)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("Toolbars", "FontStyleUnderline"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Font = StiFontUtils.ChangeFontStyleUnderline(currentPage.Watermark.Font, value);
				((Control)designer).Refresh();
				designer.InvokeSelectedObjectsChanged();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeTextBrush(StiDesignerControl designer, Color textColor)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "TextColor"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.TextBrush = new StiSolidBrush(textColor);
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeText(StiDesignerControl designer, string text)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Text"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Text = text;
				designer.InvokeSelectedObjectsChanged();
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeAngle(StiDesignerControl designer, float angle)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Angle"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.Angle = angle;
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeShowBehind(StiDesignerControl designer, bool showBehind)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "ShowBehind"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.ShowBehind = showBehind;
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class WatermarkImage
	{
		public sealed class States
		{
			public static ContentAlignment GetImageAlignment(StiDesignerControl designer)
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (designer.Report == null)
				{
					return (ContentAlignment)32;
				}
				if (designer.IsNotPageSelected)
				{
					return (ContentAlignment)32;
				}
				return designer.Report.GetCurrentPage().Watermark.ImageAlignment;
			}

			public static bool GetShowImageBehindChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return designer.Report.GetCurrentPage().Watermark.ShowImageBehind;
			}

			public static float GetImageTransparency(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return 0f;
				}
				if (designer.IsNotPageSelected)
				{
					return 0f;
				}
				return designer.Report.GetCurrentPage().Watermark.ImageTransparency;
			}

			public static bool GetImageAlignmentTopLeftChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 1;
			}

			public static bool GetImageAlignmentTopCenterChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 2;
			}

			public static bool GetImageAlignmentTopRightChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 4;
			}

			public static bool GetImageAlignmentMiddleLeftChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 16;
			}

			public static bool GetImageAlignmentMiddleCenterChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 32;
			}

			public static bool GetImageAlignmentMiddleRightChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 64;
			}

			public static bool GetImageAlignmentBottomLeftChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 256;
			}

			public static bool GetImageAlignmentBottomCenterChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 512;
			}

			public static bool GetImageAlignmentBottomRightChecked(StiDesignerControl designer)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Invalid comparison between Unknown and I4
				if (designer.Report == null)
				{
					return false;
				}
				if (designer.IsNotPageSelected)
				{
					return false;
				}
				return (int)designer.Report.GetCurrentPage().Watermark.ImageAlignment == 1024;
			}

			public static bool GetImageAlignmentEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetShowImageBehindEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetLoadImageEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}

			public static bool GetImageTransparencyEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !designer.IsNotPageSelected;
			}
		}

		public static void InvokeLoadImage(StiDesignerControl designer)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			try
			{
				Image image = designer.Report.GetCurrentPage().Watermark.TakeGdiImage();
				StiSimpleImageEditorForm stiSimpleImageEditorForm = new StiSimpleImageEditorForm(image);
				try
				{
					designer?.AddOwnedForm((Form)(object)stiSimpleImageEditorForm);
					if ((int)((Form)stiSimpleImageEditorForm).ShowDialog() == 1)
					{
						designer.Report.GetCurrentPage().Watermark.PutImage(stiSimpleImageEditorForm.Image);
					}
				}
				finally
				{
					((IDisposable)stiSimpleImageEditorForm)?.Dispose();
				}
				designer.InvokeSelectedObjectsChanged();
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeShowImageBehind(StiDesignerControl designer, bool showBehind)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "ShowImageBehind"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.ShowImageBehind = showBehind;
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeImageAlignment(StiDesignerControl designer, ContentAlignment alignment)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "ImageAlignment"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.ImageAlignment = alignment;
				((Control)designer).Refresh();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void InvokeImageTransparency(StiDesignerControl designer, int imageTransparency)
		{
			try
			{
				designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "ImageTransparency"), onlyCurrentPage: true);
				StiPage currentPage = designer.Report.GetCurrentPage();
				currentPage.Watermark.ImageTransparency = imageTransparency;
				designer.InvokeSelectedObjectsChanged();
				designer.Builder.Redraw();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	public sealed class ViewOptions
	{
		public sealed class States
		{
			public static bool GetShowGridEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetAlignToGridEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetShowHeadersEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !(designer.Report.GetCurrentPage() is IStiDashboard);
			}

			public static bool GetShowRulersEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !(designer.Report.GetCurrentPage() is IStiDashboard);
			}

			public static bool GetShowOrderEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return true;
			}

			public static bool GetViewNormalEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetViewPageBreakViewEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetGridModeEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			public static bool GetQuickInfoEnabled(StiDesignerControl designer)
			{
				return designer.Report != null;
			}

			internal static bool GetQuickInfoShowContentEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !(designer.Report.GetCurrentPage() is IStiDashboard);
			}

			internal static bool GetQuickInfoShowFieldsEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !(designer.Report.GetCurrentPage() is IStiDashboard);
			}

			internal static bool GetQuickInfoShowFieldsOnlyEnabled(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return !(designer.Report.GetCurrentPage() is IStiDashboard);
			}

			public static bool GetShowGridChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ShowGrid;
			}

			public static bool GetAlignToGridChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.AlignToGrid;
			}

			public static bool GetShowHeadersChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ShowHeaders;
			}

			public static bool GetShowRulersChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ShowRulers;
			}

			public static bool GetShowOrderChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ShowOrder;
			}

			public static bool GetViewNormalChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ViewMode == StiViewMode.Normal;
			}

			public static bool GetViewPageBreakViewChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.ViewMode == StiViewMode.PageBreakPreview;
			}

			public static bool GetGridModeLinesChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.GridMode == StiGridMode.Lines;
			}

			public static bool GetGridModeDotsChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				return designer.Report.Info.GridMode == StiGridMode.Dots;
			}

			public static bool GetShowToolboxChecked(StiDesignerControl designer)
			{
				if (designer.Report == null)
				{
					return false;
				}
				if (!(designer.Services.GetService(typeof(StiToolbarService)) is StiToolboxToolbarService { ToolBar: not null } stiToolboxToolbarService))
				{
					return false;
				}
				return stiToolboxToolbarService.ToolBar.Visible;
			}
		}

		public static void InvokeShowGrid(StiDesignerControl designer)
		{
			designer.Report.Info.ShowGrid = !designer.Report.Info.ShowGrid;
			((Control)designer).Refresh();
		}

		public static void InvokeAlignToGrid(StiDesignerControl designer)
		{
			designer.Report.Info.AlignToGrid = !designer.Report.Info.AlignToGrid;
			((Control)designer).Refresh();
		}

		public static void InvokeShowHeaders(StiDesignerControl designer)
		{
			designer.Report.Info.ShowHeaders = !designer.Report.Info.ShowHeaders;
			if (StiOptions.Designer.AutoLargeHeight)
			{
				StiPage currentPage = designer.Report.GetCurrentPage();
				if (currentPage != null)
				{
					currentPage.DockToContainer();
					currentPage.SortByPriority();
					currentPage.CheckLargeHeight();
				}
			}
			((Control)designer).Refresh();
		}

		public static void InvokeShowRulers(StiDesignerControl designer)
		{
			designer.Report.Info.ShowRulers = !designer.Report.Info.ShowRulers;
			designer.Builder.BuildReport();
		}

		public static void InvokeShowOrder(StiDesignerControl designer)
		{
			designer.Report.Info.ShowOrder = !designer.Report.Info.ShowOrder;
			((Control)designer).Refresh();
		}

		public static void InvokeNormal(StiDesignerControl designer)
		{
			designer.Report.Info.ViewMode = StiViewMode.Normal;
			((Control)designer).Refresh();
		}

		public static void InvokePageBreakPreview(StiDesignerControl designer)
		{
			designer.Report.Info.ViewMode = StiViewMode.PageBreakPreview;
			((Control)designer).Refresh();
		}

		public static void InvokeGridModeLines(StiDesignerControl designer)
		{
			designer.Report.Info.GridMode = StiGridMode.Lines;
			((Control)designer).Refresh();
		}

		public static void InvokeGridModeDots(StiDesignerControl designer)
		{
			designer.Report.Info.GridMode = StiGridMode.Dots;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoNone(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.None;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowComponentsNames(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowComponentsNames;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowAliases(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowAliases;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowContent(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowContent;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowFields(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowFields;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowFieldsOnly(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowFieldsOnly;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoOverlay(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoOverlay = !designer.Report.Info.QuickInfoOverlay;
			((Control)designer).Refresh();
		}

		public static void InvokeQuickInfoShowEvents(StiDesignerControl designer)
		{
			designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowEvents;
			((Control)designer).Refresh();
		}
	}

	public sealed class Zoom
	{
		public static void SetZoomOnePage(StiDesignerControl designer)
		{
			try
			{
				StiPageViewControl currentPageView = designer.PageView.GetCurrentPageView();
				if (currentPageView != null)
				{
					double num = designer.Report.GetCurrentPage().Unit.ConvertToHInches(designer.Report.GetCurrentPage().DisplayRectangle.Width) * StiScale.Factor;
					double num2 = designer.Report.GetCurrentPage().Unit.ConvertToHInches(designer.Report.GetCurrentPage().DisplayRectangle.Height) * StiScale.Factor;
					int width = ((Control)currentPageView).Width;
					int height = ((Control)currentPageView).Height;
					double val = ((double)width - 40.0 * StiScale.X) / num;
					double val2 = ((double)height - 40.0 * StiScale.Y) / num2;
					designer.SetZoom((float)Math.Round(Math.Min(val, val2), 2));
					designer.Builder.BuildReport();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetZoomPageWidth(StiDesignerControl designer)
		{
			try
			{
				StiPageViewControl currentPageView = designer.PageView.GetCurrentPageView();
				if (currentPageView != null)
				{
					double num = designer.Report.GetCurrentPage().Unit.ConvertToHInches(designer.Report.GetCurrentPage().DisplayRectangle.Width) * StiScale.Factor;
					int width = ((Control)currentPageView).Width;
					designer.SetZoom(Math.Round(((double)width - 40.0 * StiScale.X) / num, 2));
					designer.Builder.BuildReport();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetZoomPageHeight(StiDesignerControl designer)
		{
			try
			{
				StiPageViewControl currentPageView = designer.PageView.GetCurrentPageView();
				if (currentPageView != null)
				{
					double num = designer.Report.GetCurrentPage().Unit.ConvertToHInches(designer.Report.GetCurrentPage().DisplayRectangle.Height) * StiScale.Factor;
					int height = ((Control)currentPageView).Height;
					designer.SetZoom(Math.Round(((double)height - 40.0 * StiScale.Y) / num, 2));
					designer.Builder.BuildReport();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetZoom100(StiDesignerControl designer)
		{
			try
			{
				designer.SetZoom(1f);
				designer.Builder.BuildReport();
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}

		public static void SetZoom(StiDesignerControl designer, double value)
		{
			designer.SetZoom(value);
			designer.Builder.BuildReport();
		}
	}
}
