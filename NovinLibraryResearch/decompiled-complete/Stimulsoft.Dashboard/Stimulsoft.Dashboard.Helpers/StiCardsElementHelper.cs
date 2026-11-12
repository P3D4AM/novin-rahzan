using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiCardsElementHelper
{
	internal static decimal MinDataColumnValue(StiCardsColumn column, StiDataTable dataTable, int columnIndex)
	{
		if (!(column is StiDataBarsCardsColumn) && !(column is StiColorScaleCardsColumn) && !(column is StiBubbleCardsColumn))
		{
			return 0m;
		}
		if (!dataTable.Rows.Any())
		{
			return 0m;
		}
		return dataTable.Rows.Min((object[] r) => StiValueHelper.TryToDecimal(r[columnIndex]));
	}

	internal static decimal MaxDataColumnValue(StiCardsColumn column, StiDataTable dataTable, int columnIndex)
	{
		if (!(column is StiDataBarsCardsColumn) && !(column is StiColorScaleCardsColumn) && !(column is StiBubbleCardsColumn))
		{
			return 0m;
		}
		if (!dataTable.Rows.Any())
		{
			return 0m;
		}
		return dataTable.Rows.Max((object[] r) => StiValueHelper.TryToDecimal(r[columnIndex]));
	}
}
