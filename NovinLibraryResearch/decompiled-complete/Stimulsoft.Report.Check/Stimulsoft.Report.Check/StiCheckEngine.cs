using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCheckEngine
{
	private class CheckComparer : IComparer<StiCheck>
	{
		public int Compare(StiCheck check1, StiCheck check2)
		{
			if (check1.Status == StiCheckStatus.Error)
			{
				if (check2.Status == StiCheckStatus.Error)
				{
					return 0;
				}
				return -1;
			}
			if (check1.Status == StiCheckStatus.Warning)
			{
				if (check2.Status == StiCheckStatus.Error)
				{
					return 1;
				}
				if (check2.Status == StiCheckStatus.Information)
				{
					return -1;
				}
				return 0;
			}
			if (check2.Status == StiCheckStatus.Error || check2.Status == StiCheckStatus.Warning)
			{
				return 1;
			}
			return 0;
		}
	}

	private static List<StiCheck> checks;

	public static List<StiCheck> Checks
	{
		get
		{
			if (checks == null)
			{
				CreateChecks();
			}
			return checks;
		}
	}

	public double ProgressValue { get; private set; }

	public double ProgressMaximum { get; private set; }

	public string ProgressInformation { get; private set; } = string.Empty;


	public event EventHandler FinishCheckingReport;

	public event EventHandler StartCheckingPages;

	public event EventHandler CheckingPages;

	public event EventHandler FinishCheckingPages;

	public event EventHandler StartCheckingComponents;

	public event EventHandler CheckingComponents;

	public event EventHandler FinishCheckingComponents;

	public event EventHandler StartCheckingDatabases;

	public event EventHandler CheckingDatabases;

	public event EventHandler FinishCheckingDatabases;

	public event EventHandler StartCheckingDataSource;

	public event EventHandler CheckingDataSource;

	public event EventHandler FinishCheckingDataSource;

	public event EventHandler StartCheckingRelations;

	public event EventHandler CheckingRelations;

	public event EventHandler FinishCheckingRelations;

	public event EventHandler StartCheckingVariables;

	public event EventHandler CheckingVariables;

	public event EventHandler FinishCheckingVariables;

	private void InvokeFinishCheckingReport()
	{
		this.FinishCheckingReport?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingPages()
	{
		this.StartCheckingPages?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingPages()
	{
		this.CheckingPages?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingPages()
	{
		this.FinishCheckingPages?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingComponents()
	{
		this.StartCheckingComponents?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingComponents()
	{
		this.CheckingComponents?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingComponents()
	{
		this.FinishCheckingComponents?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingDatabases()
	{
		this.StartCheckingDatabases?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingDatabases()
	{
		this.CheckingDatabases?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingDatabases()
	{
		this.FinishCheckingDatabases?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingDataSource()
	{
		this.StartCheckingDataSource?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingDataSource()
	{
		this.CheckingDataSource?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingDataSource()
	{
		this.FinishCheckingDataSource?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingRelations()
	{
		this.StartCheckingRelations?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingRelations()
	{
		this.CheckingRelations?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingRelations()
	{
		this.FinishCheckingRelations?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeStartCheckingVariables()
	{
		this.StartCheckingVariables?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeCheckingVariables()
	{
		this.CheckingVariables?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeFinishCheckingVariables()
	{
		this.FinishCheckingVariables?.Invoke(this, EventArgs.Empty);
	}

	private static void CreateChecks()
	{
		checks = new List<StiCheck>();
		checks.Add(new StiDuplicatedNameCheck());
		checks.Add(new StiDuplicatedReportNameCheck());
		checks.Add(new StiNoNamePageCheck());
		checks.Add(new StiFunctionsOnlyForEngineV2Check());
		checks.Add(new StiComponentStyleIsNotFoundAtPageCheck());
		checks.Add(new StiNoNameComponentCheck());
		checks.Add(new StiUndefinedComponentCheck());
		checks.Add(new StiUndefinedConnectionCheck());
		checks.Add(new StiDifferentAmountOfKeysInDataRelationCheck());
		checks.Add(new StiKeysInAbsentDataRelationCheck());
		checks.Add(new StiKeysNotFoundRelationCheck());
		checks.Add(new StiKeysTypesMismatchDataRelationCheck());
		checks.Add(new StiNoNameDataRelationCheck());
		checks.Add(new StiNoNameInSourceDataRelationCheck());
		checks.Add(new StiNoNameDataSourceCheck());
		checks.Add(new StiNoNameInSourceDataSourceCheck());
		checks.Add(new StiUndefinedDataSourceCheck());
		checks.Add(new StiCompilationErrorCheck());
		checks.Add(new StiCalculatedColumnRecursionCheck());
		checks.Add(new StiVariableRecursionCheck());
		checks.Add(new StiVariableInitializationCheck());
		checks.Add(new StiVariableTypeCheck());
		checks.Add(new StiDuplicatedNameInSourceCheck());
		checks.Add(new StiExpressionElementCheck());
		checks.Add(new StiFilterCircularDependencyElementCheck());
		checks.Add(new StiLostPointsOnPageCheck());
		checks.Add(new StiOrientationPageCheck());
		checks.Add(new StiColumnsWidthGreaterPageWidthCheck());
		checks.Add(new StiAllowHtmlTagsInTextCheck());
		checks.Add(new StiCanBreakComponentInContainerCheck());
		checks.Add(new StiCanGrowComponentInContainerCheck());
		checks.Add(new StiGrowToHeightOverlappingCheck());
		checks.Add(new StiComponentStyleIsNotFoundAtComponentCheck());
		checks.Add(new StiCorruptedCrossLinePrimitiveCheck());
		checks.Add(new StiPrintOnDoublePassCheck());
		checks.Add(new StiLocationOutsidePageCheck());
		checks.Add(new StiNegativeSizesOfComponentsCheck());
		checks.Add(new StiPanelInEngineV1Check());
		checks.Add(new StiWordWrapCanGrowTextDoesNotFitCheck());
		checks.Add(new StiDuplicatedNameInSourceInDataRelationReportCheck());
		checks.Add(new StiSourcesInAbsentDataRelationCheck());
		checks.Add(new StiIsFirstPageIsLastPageDoublePassCheck());
		checks.Add(new StiComponentBoundsAreOutOfBand());
		checks.Add(new StiComponentDataColumnCheck());
		checks.Add(new StiComponentExpressionCheck());
		checks.Add(new StiComponentResourceCheck());
		checks.Add(new StiTextTextFormatCheck());
		checks.Add(new StiFilterValueCheck());
		checks.Add(new StiComponentEventsAtInterpretationCheck());
		checks.Add(new StiPageEventsAtInterpretationCheck());
		checks.Add(new StiConnectionEventsAtInterpretationCheck());
		checks.Add(new StiReportEventsAtInterpretationCheck());
		checks.Add(new StiChartSeriesValueCheck());
		checks.Add(new StiLicenseTrialCheck());
		checks.Add(new StiTotalPageCountDoublePassCheck());
		checks.Add(new StiIsFirstPassIsSecondPassCheck());
		checks.Add(new StiPrintHeadersAndFootersFromPreviousPageCheck());
		checks.Add(new StiLargeHeightAtPageCheck());
		checks.Add(new StiPrintOnPreviousPageCheck());
		checks.Add(new StiPrintOnPreviousPageCheck2());
		checks.Add(new StiResetPageNumberCheck());
		checks.Add(new StiVerySmallSizesOfComponentsCheck());
		checks.Add(new StiShowInsteadNullValuesCheck());
		checks.Add(new StiPropertiesOnlyEngineV1Check());
		checks.Add(new StiPropertiesOnlyEngineV2Check());
		checks.Add(new StiMinRowsInColumnsCheck());
		checks.Add(new StiDataSourcesForImageCheck());
		checks.Add(new StiCanGrowGrowToHeightComponentInContainerCheck());
		checks.Add(new StiCanGrowWordWrapTextAndWysiwygCheck());
		checks.Add(new StiColumnsWidthGreaterContainerWidthCheck());
		checks.Add(new StiGroupHeaderNotEqualToGroupFooterOnPageCheck());
		checks.Add(new StiCrossGroupHeaderNotEqualToCrossGroupFooterOnPageCheck());
		checks.Add(new StiGroupHeaderNotEqualToGroupFooterOnContainerCheck());
		checks.Add(new StiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck());
		checks.Add(new StiContainerInEngineV2Check());
		checks.Add(new StiSystemTextObsoleteCheck());
		checks.Add(new StiContourTextObsoleteCheck());
		checks.Add(new StiCountDataDataSourceAtDataBandCheck());
		checks.Add(new StiNoConditionAtGroupCheck());
		checks.Add(new StiWidthHeightZeroComponentCheck());
		checks.Add(new StiTextColorEqualToBackColorCheck());
		checks.Add(new StiFontMissingCheck());
	}

	public List<StiCheck> CheckReport(StiReport report)
	{
		List<StiCheck> list = new List<StiCheck>();
		List<StiCheck> list2 = new List<StiCheck>();
		List<StiCheck> list3 = new List<StiCheck>();
		List<StiCheck> list4 = new List<StiCheck>();
		List<StiCheck> list5 = new List<StiCheck>();
		List<StiCheck> list6 = new List<StiCheck>();
		List<StiCheck> list7 = new List<StiCheck>();
		List<StiCheck> list8 = new List<StiCheck>();
		List<StiCheck> list9 = new List<StiCheck>();
		List<StiCheck> list10 = new List<StiCheck>();
		foreach (StiCheck check in Checks)
		{
			if (check is StiLicenseTrialCheck)
			{
				check.Enabled = true;
			}
			if (!check.Enabled)
			{
				continue;
			}
			switch (check.ObjectType)
			{
			case StiCheckObjectType.Report:
				if (check is StiCompilationErrorCheck)
				{
					list3.Add(check);
				}
				else
				{
					list2.Add(check);
				}
				break;
			case StiCheckObjectType.Page:
				list4.Add(check);
				break;
			case StiCheckObjectType.Component:
				list5.Add(check);
				break;
			case StiCheckObjectType.Database:
				list6.Add(check);
				break;
			case StiCheckObjectType.DataSource:
				list7.Add(check);
				break;
			case StiCheckObjectType.DataRelation:
				list8.Add(check);
				break;
			case StiCheckObjectType.DataColumn:
				list9.Add(check);
				break;
			case StiCheckObjectType.Variable:
				list10.Add(check);
				break;
			}
		}
		CheckObject(report, report, list, list2);
		if (!list.Any((StiCheck r) => r is StiDuplicatedNameCheck || r is StiDuplicatedReportNameCheck))
		{
			CheckObject(report, report, list, list3);
		}
		InvokeFinishCheckingReport();
		ProgressMaximum = report.Pages.Count;
		ProgressValue = 0.0;
		InvokeStartCheckingPages();
		foreach (StiPage page in report.Pages)
		{
			ProgressInformation = page.Name;
			ProgressValue++;
			InvokeCheckingPages();
			CheckObject(report, page, list, list4);
		}
		InvokeFinishCheckingPages();
		ProgressMaximum = report.GetComponentsCount();
		ProgressValue = 0.0;
		InvokeStartCheckingComponents();
		foreach (StiPage page2 in report.Pages)
		{
			StiComponentsCollection components = page2.GetComponents();
			foreach (StiComponent item in components)
			{
				ProgressInformation = item.Name;
				ProgressValue++;
				InvokeCheckingComponents();
				CheckObject(report, item, list, list5);
			}
		}
		InvokeFinishCheckingComponents();
		ProgressMaximum = report.Dictionary.Databases.Count;
		ProgressValue = 0.0;
		InvokeStartCheckingDatabases();
		foreach (StiDatabase database in report.Dictionary.Databases)
		{
			ProgressInformation = StiLocalization.Get("QueryBuilder", "Database") + " - '" + database.Name + "'";
			ProgressValue++;
			InvokeCheckingDatabases();
			CheckObject(report, database, list, list6);
		}
		InvokeFinishCheckingDatabases();
		ProgressMaximum = report.Dictionary.DataSources.Count;
		ProgressValue = 0.0;
		InvokeStartCheckingDataSource();
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			ProgressInformation = StiLocalization.Get("PropertyMain", "DataSource") + " - '" + dataSource.Name + "'";
			ProgressValue++;
			InvokeCheckingDataSource();
			CheckObject(report, dataSource, list, list7);
			foreach (StiDataColumn column in dataSource.Columns)
			{
				CheckObject(report, column, list, list9);
			}
		}
		InvokeFinishCheckingDataSource();
		ProgressMaximum = report.Dictionary.Relations.Count;
		ProgressValue = 0.0;
		InvokeStartCheckingRelations();
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			ProgressInformation = StiLocalization.Get("PropertyMain", "DataRelation") + " - '" + relation.Name + "'";
			ProgressValue++;
			InvokeCheckingRelations();
			CheckObject(report, relation, list, list8);
		}
		InvokeFinishCheckingRelations();
		ProgressMaximum = report.Dictionary.Variables.Count;
		ProgressValue = 0.0;
		InvokeStartCheckingVariables();
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			ProgressInformation = StiLocalization.Get("PropertyMain", "Variable") + " - '" + variable.Name + "'";
			ProgressValue++;
			InvokeCheckingVariables();
			CheckObject(report, variable, list, list10);
		}
		InvokeFinishCheckingVariables();
		list.Sort(new CheckComparer());
		return list;
	}

	private static void CheckObject(StiReport report, object obj, List<StiCheck> results, List<StiCheck> checksObject)
	{
		foreach (StiCheck item2 in checksObject)
		{
			object obj2 = item2.ProcessCheck(report, obj);
			if (obj2 == null)
			{
				continue;
			}
			if (obj2 is StiCheck item)
			{
				results.Add(item);
			}
			if (!(obj2 is List<StiCheck> list))
			{
				continue;
			}
			foreach (StiCheck item3 in list)
			{
				results.Add(item3);
			}
		}
	}

	public StiCheckEngine()
	{
		if (StiLocalizationExt.Localization != StiLocalization.Localization)
		{
			StiLocalizationExt.Load();
		}
	}
}
