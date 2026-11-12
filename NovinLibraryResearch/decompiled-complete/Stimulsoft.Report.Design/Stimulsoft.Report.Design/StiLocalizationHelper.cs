using System.IO;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public class StiLocalizationHelper
{
	public static void SetLocalization(StiDesignerControl designer, string localizationFile)
	{
		if (string.IsNullOrEmpty(localizationFile))
		{
			StiLocalization.Localization = string.Empty;
			StiSettings.Load();
			StiSettings.Set("Localization", "FileName", string.Empty);
			StiSettings.Save();
			designer.LocalizeDesigner();
			return;
		}
		string directory = StiOptions.Localization.GetDirectory();
		localizationFile = Path.GetFileName(localizationFile);
		StiLocalization.GetParam(localizationFile, out var language, out var _, out var _, directory);
		if (!string.IsNullOrEmpty(language))
		{
			StiLocalization.Localization = localizationFile;
			StiSettings.Load();
			StiSettings.Set("Localization", "FileName", localizationFile);
			StiSettings.Save();
			designer.LocalizeDesigner();
		}
	}
}
