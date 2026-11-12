using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Design;

public static class StiReportSaveDialog
{
	public static bool SaveReport(StiReport report)
	{
		return SaveReport(null, report, StiSaveEventSource.Underfined);
	}

	public static bool SaveReport(StiReport report, StiSaveEventSource eventSource)
	{
		return SaveReport(null, report, eventSource);
	}

	public static bool SaveReport(StiDesignerControl designer)
	{
		return SaveReport(designer, designer.Report, StiSaveEventSource.Underfined);
	}

	public static bool SaveReport(StiDesignerControl designer, StiSaveEventSource eventSource)
	{
		return SaveReport(designer, designer.Report, eventSource);
	}

	private static bool SaveReport(StiDesignerControl designer, StiReport report, StiSaveEventSource eventSource)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Invalid comparison between Unknown and I4
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Invalid comparison between Unknown and I4
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		try
		{
			if (designer != null)
			{
				((Control)designer).Refresh();
			}
			bool flag2 = false;
			if (designer != null)
			{
				flag2 = StiOptions.Engine.GlobalEvents.InvokeSavingReportInDesigner(designer, saveAs: true, eventSource);
			}
			if (!flag2)
			{
				SaveFileDialog val = new SaveFileDialog();
				try
				{
					val.OverwritePrompt = true;
					((FileDialog)val).DefaultExt = "mrt";
					string text = report.ReportFile;
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							text = text.Trim();
							text = Path.GetDirectoryName(text) + "\\";
						}
						catch
						{
							text = null;
						}
					}
					string empty = string.Empty;
					if (text == null)
					{
						StiSettings.Load();
						empty = StiOptions.Designer.ReportSaveLoadPath;
						if (string.IsNullOrEmpty(empty))
						{
							empty = StiSettings.GetStr("StiDesigner", "ReportSaveLoadPath", string.Empty);
						}
						if (!string.IsNullOrEmpty(empty) && Directory.Exists(empty))
						{
							((FileDialog)val).InitialDirectory = empty;
						}
					}
					else
					{
						((FileDialog)val).InitialDirectory = text;
					}
					StringBuilder stringBuilder = new StringBuilder();
					int num = -1;
					string fileExtension = report.Language.GetProvider().FileExtension;
					string reportFilters = StiSLService.GetReportFilters(StiSLActions.Save);
					stringBuilder.Append(reportFilters);
					string[] array = reportFilters.Split(new char[1] { '|' });
					int num2 = 1;
					for (int i = 0; i < array.Length; i += 2)
					{
						if (array[i].Contains("JSON"))
						{
							num = num2;
							break;
						}
						num2++;
					}
					if (StiOptions.Designer.AllowSaveReportTemplateWithEmbededData && StiAccountSettings.CurrentAccount != null)
					{
						stringBuilder.Append("|");
						stringBuilder.Append(StiLocalization.Get("FileFilters", "ReportEmbededDataFiles"));
					}
					if (StiOptions.Designer.AllowSaveToAssembly)
					{
						stringBuilder.Append("|");
						stringBuilder.Append(StiLocalization.Get("FileFilters", "DllFiles"));
					}
					if (StiOptions.Designer.AllowSaveToSourceCode)
					{
						stringBuilder.Append("|");
						stringBuilder.AppendFormat(StiLocalization.Get("FileFilters", "LanguageFiles"), report.Language, fileExtension, fileExtension);
					}
					if (StiOptions.Designer.AllowSaveToSourceCodeForSilverlight)
					{
						stringBuilder.Append("|");
						stringBuilder.AppendFormat(StiLocalization.Get("FileFilters", "LanguageForSilverlightFiles"), report.Language, fileExtension, fileExtension);
					}
					if (StiOptions.Designer.AllowSaveSourceCodeForInheritedReports)
					{
						stringBuilder.Append("|");
						stringBuilder.AppendFormat(StiLocalization.Get("FileFilters", "InheritedLanguageFiles"), report.Language, fileExtension, fileExtension);
					}
					if (StiOptions.Designer.AllowSaveStandaloneReport)
					{
						stringBuilder.Append("|");
						stringBuilder.AppendFormat(StiLocalization.Get("FileFilters", "StandaloneReportFiles"), report.Language, fileExtension, fileExtension);
					}
					((FileDialog)val).Filter = stringBuilder.ToString();
					string value = (string.IsNullOrEmpty(designer.ReportKey) ? StiSettings.GetStr("SaveDialog", "DefaultExt", "mrt") : "mrx");
					string[] array2 = ((FileDialog)val).Filter.Split(new char[1] { '|' });
					for (int j = 0; j < array2.Length; j++)
					{
						if (j % 2 != 0 && array2[j].Contains(value))
						{
							((FileDialog)val).FilterIndex = (j - 1) / 2 + 1;
							break;
						}
					}
					((FileDialog)val).RestoreDirectory = true;
					((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSaveAs"));
					string fileName = report.ReportFile;
					if (designer != null)
					{
						fileName = Path.GetFileNameWithoutExtension(designer.ReportFileName);
					}
					((FileDialog)val).FileName = fileName;
					int filterIndex = 1;
					if (!string.IsNullOrEmpty(designer.ReportKey))
					{
						filterIndex = 2;
					}
					else if (report.IsJsonReport)
					{
						filterIndex = 4;
					}
					((FileDialog)val).FilterIndex = filterIndex;
					if ((int)((CommonDialog)val).ShowDialog() == 1 && !string.IsNullOrEmpty(((FileDialog)val).FileName))
					{
						StiSettings.Set("SaveDialog", "DefaultExt", new FileInfo(((FileDialog)val).FileName).Extension);
						designer.SaveAsToSilverlight = false;
						string key = string.Empty;
						if (((FileDialog)val).FileName.Substring(((FileDialog)val).FileName.Length - 4, 4) == ".mrx")
						{
							if (!string.IsNullOrEmpty(designer.ReportKey))
							{
								key = designer.ReportKey;
							}
							else
							{
								StiSavePasswordForm stiSavePasswordForm = new StiSavePasswordForm();
								try
								{
									if ((int)((Form)stiSavePasswordForm).ShowDialog() != 1)
									{
										return false;
									}
									key = stiSavePasswordForm.Password;
								}
								finally
								{
									((IDisposable)stiSavePasswordForm)?.Dispose();
								}
							}
						}
						flag = true;
						Windows7Taskbar.ShowTaskbarProgress(report.Designer, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
						if (text == null)
						{
							empty = StiOptions.Designer.ReportSaveLoadPath;
							if (string.IsNullOrEmpty(empty))
							{
								StiSettings.Load();
								StiSettings.Set("StiDesigner", "ReportSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
								StiSettings.Save();
							}
						}
						string text2 = Path.GetExtension(((FileDialog)val).FileName).ToLower(CultureInfo.InvariantCulture);
						switch (text2)
						{
						case ".dll":
							if (StiOptions.Engine.AllowSetCurrentDirectory)
							{
								Environment.CurrentDirectory = StiOptions.Configuration.ApplicationDirectory;
							}
							report.CompiledReport = null;
							report.Compile(((FileDialog)val).FileName, StiOutputType.ClassLibrary);
							if (designer != null && designer.CheckHelper != null)
							{
								designer.CheckHelper.BuildChecks(report);
								if (designer.CheckHelper.ErrorsCount > 0)
								{
									MessageBox.Show(StiLocalization.Get("Errors", "ReportCannotBeSaveDueToErrors"), StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSaveAs")), (MessageBoxButtons)0, (MessageBoxIcon)16);
								}
							}
							if (designer != null)
							{
								CompilerResults compilerResults = report.CompilerResults;
								if (compilerResults != null && ((CollectionBase)(object)compilerResults.Errors)?.Count > 0)
								{
									designer.InvokeShowingCompilerResults(report.CompilerResults);
								}
							}
							break;
						case ".exe":
							if (StiOptions.Engine.AllowSetCurrentDirectory)
							{
								Environment.CurrentDirectory = StiOptions.Configuration.ApplicationDirectory;
							}
							report.CompiledReport = null;
							report.CompileStandaloneReport(((FileDialog)val).FileName, StiStandaloneReportType.Show);
							if (designer != null && designer.CheckHelper != null)
							{
								designer.CheckHelper.BuildChecks(report);
								if (designer.CheckHelper.ErrorsCount > 0)
								{
									MessageBox.Show(StiLocalization.Get("Errors", "ReportCannotBeSaveDueToErrors"), StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSaveAs")), (MessageBoxButtons)0, (MessageBoxIcon)16);
								}
							}
							if (designer != null && ((CollectionBase)(object)report.CompilerResults.Errors).Count > 0)
							{
								designer.InvokeShowingCompilerResults(report.CompilerResults);
							}
							break;
						case ".cs":
						case ".vb":
						{
							if (designer != null)
							{
								((Control)designer).Refresh();
							}
							array = ((FileDialog)val).Filter.Split(new char[1] { '|' });
							string[] array4 = string.Format(Loc.Get("FileFilters", "InheritedLanguageFiles"), report.Language, fileExtension, fileExtension).Split(new char[1] { '|' });
							string[] array5 = string.Format(Loc.Get("FileFilters", "LanguageForSilverlightFiles"), report.Language, fileExtension, fileExtension).Split(new char[1] { '|' });
							if (array[(((FileDialog)val).FilterIndex - 1) * 2] == array4[0])
							{
								report.SaveReportSourceCode(((FileDialog)val).FileName, saveForInheritedReports: true);
							}
							else if (array[(((FileDialog)val).FilterIndex - 1) * 2] == array5[0])
							{
								report.SaveReportSourceCodeForSilverlight(((FileDialog)val).FileName);
								designer.SaveAsToSilverlight = true;
								designer.ReportFileName = ((FileDialog)val).FileName;
							}
							else
							{
								report.SaveReportSourceCode(((FileDialog)val).FileName);
							}
							break;
						}
						default:
						{
							List<StiReportSLService> reportSLServices = StiSLService.GetReportSLServices(StiSLActions.Save);
							string[] array3 = string.Format(Loc.Get("FileFilters", "ReportEmbededDataFiles"), report.Language, fileExtension, fileExtension).Split(new char[1] { '|' });
							bool flag3 = ((FileDialog)val).Filter.Split(new char[1] { '|' })[(((FileDialog)val).FilterIndex - 1) * 2] == array3[0];
							if (designer != null && !flag3)
							{
								designer.ReportNeverSaved = false;
								if (StiDesigner.CanDesignerChangeReportFileName)
								{
									designer.ReportFileName = ((FileDialog)val).FileName;
								}
								((Control)designer).Refresh();
							}
							report.ReportFile = ((FileDialog)val).FileName;
							StiDashboardRecentHelper.Add(report, ((FileDialog)val).FileName);
							switch (text2.ToLower(CultureInfo.InvariantCulture))
							{
							case ".mrt":
								if (num == ((FileDialog)val).FilterIndex)
								{
									StiReportSLService service = reportSLServices.FirstOrDefault((StiReportSLService x) => x is StiJsonReportSLService);
									report.Save(service, ((FileDialog)val).FileName);
									if (report.Info.AutoSaveReportToReportClass)
									{
										report.SaveReportSourceCode(Path.ChangeExtension(((FileDialog)val).FileName, report.Language.GetProvider().FileExtension));
									}
								}
								else if (flag3)
								{
									StiReport stiReport = report.Clone() as StiReport;
									StiDataResourceHelper.SaveSnapshot(stiReport);
									StiReportSLService service2 = reportSLServices.FirstOrDefault((StiReportSLService x) => x is StiXmlReportSLService);
									stiReport.Save(service2, ((FileDialog)val).FileName);
									stiReport.Dispose();
								}
								else
								{
									StiReportSLService service3 = reportSLServices.FirstOrDefault((StiReportSLService x) => x is StiXmlReportSLService);
									report.Save(service3, ((FileDialog)val).FileName);
									if (report.Info.AutoSaveReportToReportClass)
									{
										report.SaveReportSourceCode(Path.ChangeExtension(((FileDialog)val).FileName, report.Language.GetProvider().FileExtension));
									}
								}
								break;
							case ".mrz":
								foreach (StiReportSLService item in reportSLServices)
								{
									if (item is StiPackedReportSLService)
									{
										report.Save(item, ((FileDialog)val).FileName);
										if (report.Info.AutoSaveReportToReportClass)
										{
											report.SaveReportSourceCode(Path.ChangeExtension(((FileDialog)val).FileName, report.Language.GetProvider().FileExtension));
										}
									}
								}
								break;
							case ".mrx":
								report.SaveEncryptedReport(((FileDialog)val).FileName, key);
								if (report.Info.AutoSaveReportToReportClass)
								{
									report.SaveReportSourceCode(Path.ChangeExtension(((FileDialog)val).FileName, report.Language.GetProvider().FileExtension));
								}
								break;
							}
							if (designer != null && StiDesigner.CanDesignerChangeReportFileName)
							{
								designer.AddRecentFile(designer.ReportFileName, null);
							}
							report.IsModified = false;
							break;
						}
						}
						designer.ReportItemKey = null;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				flag = true;
				designer.ReportNeverSaved = false;
			}
		}
		catch (Exception ex)
		{
			flag = false;
			StiLogService.Write(typeof(StiReportSaveDialog), "Saving report...ERROR");
			StiLogService.Write(typeof(StiReportSaveDialog), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			if (designer != null && designer.Report != null)
			{
				designer.Builder.BuildReport();
			}
		}
		if (flag)
		{
			StiOptions.Engine.GlobalEvents.InvokeSavedReportInDesigner(designer);
		}
		Windows7Taskbar.HideTaskbarProgress();
		return flag;
	}
}
