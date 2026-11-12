namespace Stimulsoft.Report.Design.Panels;

public static class StiDictionaryPanelHelper
{
	public static void ActivatePanel(StiDesignerControl designer)
	{
		StiDictionaryPanelService dictionaryService = designer.GetDictionaryService();
		if (dictionaryService?.DockingBar != null && dictionaryService?.DockingItem != null)
		{
			dictionaryService.DockingItem.Selected = true;
		}
	}

	public static void ShowProgress(StiDesignerControl designer, string message = "", bool useTimer = true, string processGuid = null)
	{
		designer.GetDictionaryService()?.ShowProgress(message, useTimer, processGuid);
	}

	public static void UpdateMessageProgress(StiDesignerControl designer, string message)
	{
		designer.GetDictionaryService()?.UpdateMessage(message);
	}

	public static void HideProgress(StiDesignerControl designer, string processGuid = null)
	{
		designer.GetDictionaryService()?.HideProgress(processGuid);
	}

	public static void BuildDictionary(StiDesignerControl designer)
	{
		designer.GetDictionaryService()?.BuildDictionary();
	}
}
