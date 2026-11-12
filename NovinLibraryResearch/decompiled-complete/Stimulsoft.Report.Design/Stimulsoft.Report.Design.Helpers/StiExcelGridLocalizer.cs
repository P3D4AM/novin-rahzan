using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Helpers;

public static class StiExcelGridLocalizer
{
	public static void Localize(StiExcelGrid grid)
	{
		grid.LocalizationOfFileImageFilter = StiFileDialogHelper.GetImageFilter();
		grid.LocalizationOfItemLoadImage = Loc.Get("FormDesigner", "LoadImage").Replace("...", "");
		grid.LocalizationOfItemRemoveImage = Loc.Get("Buttons", "Remove");
		grid.LocalizationOfItemInsertRowAbove = Loc.Get("MainMenu", "menuInsertRowAbove");
		grid.LocalizationOfItemInsertRowBelow = Loc.Get("MainMenu", "menuInsertRowBelow");
		grid.LocalizationOfItemMoveRowUp = Loc.Get("QueryBuilder", "MoveUp");
		grid.LocalizationOfItemMoveRowDown = Loc.Get("QueryBuilder", "MoveDown");
		grid.LocalizationOfItemDeleteRow = Loc.Get("Buttons", "Delete");
	}
}
