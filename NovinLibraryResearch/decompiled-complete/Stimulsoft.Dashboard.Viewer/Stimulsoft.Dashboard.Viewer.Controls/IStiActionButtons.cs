using System.Collections.Generic;

namespace Stimulsoft.Dashboard.Viewer.Controls;

public interface IStiActionButtons
{
	StiSaveActionButton SaveButton { get; set; }

	StiViewDataActionButton ViewDataButton { get; set; }

	StiDrillUpActionButton DrillUpButton { get; set; }

	StiDrillDownActionButton DrillDownButton { get; set; }

	StiDrillDownSelectedActionButton DrillDownSelectedButton { get; set; }

	StiDrillDownCancelActionButton DrillDownCancelButton { get; set; }

	StiFilterMultiOptionActionCheckBox FilterMultiOptionButton { get; set; }

	StiOpenFullScreenActionButton OpenFullScreenButton { get; set; }

	StiCloseFullScreenActionButton CloseFullScreenButton { get; set; }

	StiRemoveFilterActionButton RemoveFilterButton { get; set; }

	StiSortActionButton SortButton { get; set; }

	StiColumnSelectionActionButton SelectColumnsButton { get; set; }

	List<StiViewStateActionButton> ViewStateButtons { get; set; }
}
