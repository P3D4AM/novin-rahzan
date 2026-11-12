using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace Stimulsoft.Report.Check;

public sealed class StiCheckHelper
{
	public int ErrorsCount { get; private set; }

	public int WarningsCount { get; private set; }

	public int InformationMessagesCount { get; private set; }

	public int ReportRenderingMessagesCount { get; private set; }

	public List<StiCheck> Checks { get; private set; }

	public List<StiCheck> ReportRenderingMessagesChecks { get; private set; }

	public bool IsMessagesPresent
	{
		get
		{
			if (ErrorsCount <= 0 && InformationMessagesCount <= 0 && ReportRenderingMessagesCount <= 0)
			{
				return WarningsCount > 0;
			}
			return true;
		}
	}

	public void BuildChecks(StiReport report)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		string text = null;
		if (report.CompiledReport == null && report.CalculationMode == StiCalculationMode.Compilation && (report.CompilerResults == null || ((CollectionBase)(object)report.CompilerResults.Errors).Count == 0))
		{
			try
			{
				report.Compile();
			}
			catch (Exception ex)
			{
				if (ex is TargetInvocationException ex2)
				{
					string text2 = string.Empty;
					if (ex2.InnerException.TargetSite != null)
					{
						text2 = "TargetSite: " + ex2.InnerException.TargetSite.Name;
					}
					text = ex2.Message + " " + ex2.InnerException.Message + " (" + text2 + ")";
				}
			}
		}
		StiCheckEngine stiCheckEngine = new StiCheckEngine();
		Checks = stiCheckEngine.CheckReport(report);
		if (text != null)
		{
			Checks.Add(new StiCompilationErrorCheck
			{
				Element = report,
				Error = new CompilerError("", 0, 0, "", "TargetInvocationException: '" + text + "'")
			});
		}
		ErrorsCount = 0;
		WarningsCount = 0;
		InformationMessagesCount = 0;
		foreach (StiCheck check in Checks)
		{
			switch (check.Status)
			{
			case StiCheckStatus.Error:
				ErrorsCount++;
				break;
			case StiCheckStatus.Warning:
				WarningsCount++;
				break;
			case StiCheckStatus.Information:
				InformationMessagesCount++;
				break;
			}
		}
	}

	public void BuildReportRenderingMessages(StiReport report)
	{
		ReportRenderingMessagesCount = 0;
		ReportRenderingMessagesChecks = new List<StiCheck>();
		report = ((report.CompiledReport != null) ? report.CompiledReport : report);
		if (report.ReportRenderingMessages == null || report.ReportRenderingMessages.Count <= 0)
		{
			return;
		}
		StringEnumerator enumerator = report.ReportRenderingMessages.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				StiReportRenderingMessageCheck stiReportRenderingMessageCheck = new StiReportRenderingMessageCheck();
				stiReportRenderingMessageCheck.SetMessage(current);
				Checks.Add(stiReportRenderingMessageCheck);
				ReportRenderingMessagesChecks.Add(stiReportRenderingMessageCheck);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		ReportRenderingMessagesCount = report.ReportRenderingMessages.Count;
	}
}
