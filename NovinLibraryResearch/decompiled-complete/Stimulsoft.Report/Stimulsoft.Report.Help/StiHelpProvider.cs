using System;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Help;

public static class StiHelpProvider
{
	public static void ShowHelpViewer(string url)
	{
		try
		{
			if (!string.IsNullOrEmpty(url) && !StringExt.EndsWithInvariant(url, "?toc=0"))
			{
				url += "?toc=0";
			}
			string url2 = url ?? "user-manual/";
			StiHelpViewerForm.Show(url2);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public static void ShowHelpDefaultBrowser(string url)
	{
		try
		{
			string cultureName = StiLocalization.CultureName;
			string text = ((!(cultureName == "ru")) ? "en" : "ru");
			StiProcess.Start("https://www.stimulsoft.com/" + text + "/documentation/online/" + url);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}
}
