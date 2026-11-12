using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Viewer.Controls.Table;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Chart;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

internal static class StiActionButtonsHelper
{
	internal static void RefreshButtonsVisibility(IStiActionButtons actionButtons, IStiElement element)
	{
		if (actionButtons == null)
		{
			return;
		}
		Control val = (Control)((actionButtons is Control) ? actionButtons : null);
		int position = val.Width - GetHorDistance();
		Point pt = val.PointToClient(Cursor.Position);
		bool flag = val.ClientRectangle.Contains(pt);
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		double zoom = (element as StiComponent).Report.Info.Zoom;
		position = CorrectPositionForVerticalScrollBar(val, position);
		StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(val);
		if (stiDashboardViewerControl == null)
		{
			return;
		}
		bool isActivated = stiDashboardViewerControl.ElementFullScreen.IsActivated;
		bool flag2 = actionButtons is StiHolderElementControl && (actionButtons as StiHolderElementControl).IsDrillSelectionActivated;
		if (stiCornerRadius != null && stiCornerRadius.TopRight > 0f)
		{
			position -= StiScale.I((double)stiCornerRadius.TopRight * zoom);
		}
		if (actionButtons.SaveButton != null)
		{
			((Control)actionButtons.SaveButton).Visible = stiDashboardViewerControl != null && flag && !flag2;
			if (((Control)actionButtons.SaveButton).Visible)
			{
				((Control)actionButtons.SaveButton).Left = position - ((Control)actionButtons.SaveButton).Width;
				position = ((Control)actionButtons.SaveButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.ViewDataButton != null)
		{
			((Control)actionButtons.ViewDataButton).Visible = stiDashboardViewerControl != null && flag;
			if (((Control)actionButtons.ViewDataButton).Visible)
			{
				((Control)actionButtons.ViewDataButton).Left = position - ((Control)actionButtons.ViewDataButton).Width;
				position = ((Control)actionButtons.ViewDataButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.CloseFullScreenButton != null)
		{
			((Control)actionButtons.CloseFullScreenButton).Visible = stiDashboardViewerControl != null && isActivated && flag && !flag2;
			if (((Control)actionButtons.CloseFullScreenButton).Visible)
			{
				((Control)actionButtons.CloseFullScreenButton).Left = position - ((Control)actionButtons.CloseFullScreenButton).Width;
				position = ((Control)actionButtons.CloseFullScreenButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.OpenFullScreenButton != null)
		{
			((Control)actionButtons.OpenFullScreenButton).Visible = stiDashboardViewerControl != null && flag && !isActivated && !flag2;
			if (((Control)actionButtons.OpenFullScreenButton).Visible)
			{
				((Control)actionButtons.OpenFullScreenButton).Left = position - ((Control)actionButtons.OpenFullScreenButton).Width;
				position = ((Control)actionButtons.OpenFullScreenButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.DrillUpButton != null)
		{
			((Control)actionButtons.DrillUpButton).Visible = stiDashboardViewerControl != null && flag && StiDrillDownHelper.IsDrillUpAvailable(element) && !flag2;
			if (((Control)actionButtons.DrillUpButton).Visible)
			{
				((Control)actionButtons.DrillUpButton).Left = position - ((Control)actionButtons.DrillUpButton).Width;
				position = ((Control)actionButtons.DrillUpButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.DrillDownButton != null)
		{
			((Control)actionButtons.DrillDownButton).Visible = stiDashboardViewerControl != null && flag && StiDrillDownHelper.IsDrillDownAvailable(element) && !flag2;
			if (((Control)actionButtons.DrillDownButton).Visible)
			{
				((Control)actionButtons.DrillDownButton).Left = position - ((Control)actionButtons.DrillDownButton).Width;
				position = ((Control)actionButtons.DrillDownButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.DrillDownCancelButton != null)
		{
			((Control)actionButtons.DrillDownCancelButton).Visible = stiDashboardViewerControl != null && flag && StiDrillDownHelper.IsDrillAvailable(element) && flag2;
			if (((Control)actionButtons.DrillDownCancelButton).Visible)
			{
				((Control)actionButtons.DrillDownCancelButton).Left = position - ((Control)actionButtons.DrillDownCancelButton).Width;
				position = ((Control)actionButtons.DrillDownCancelButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.DrillDownSelectedButton != null)
		{
			((Control)actionButtons.DrillDownSelectedButton).Visible = stiDashboardViewerControl != null && flag && StiDrillDownHelper.IsDrillAvailable(element) && flag2;
			if (((Control)actionButtons.DrillDownSelectedButton).Visible)
			{
				((Control)actionButtons.DrillDownSelectedButton).Left = position - ((Control)actionButtons.DrillDownSelectedButton).Width;
				position = ((Control)actionButtons.DrillDownSelectedButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.RemoveFilterButton != null)
		{
			((Control)actionButtons.RemoveFilterButton).Visible = stiDashboardViewerControl != null && flag && !isActivated && StiUserFiltersHelper.IsFilterPresent(element) && !flag2;
			if (((Control)actionButtons.RemoveFilterButton).Visible)
			{
				((Control)actionButtons.RemoveFilterButton).Left = position - ((Control)actionButtons.RemoveFilterButton).Width;
				position = ((Control)actionButtons.RemoveFilterButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.FilterMultiOptionButton != null)
		{
			((Control)actionButtons.FilterMultiOptionButton).Visible = stiDashboardViewerControl != null && flag && !isActivated && StiUserFiltersHelper.IsFilterPresent(element) && !flag2;
			if (((Control)actionButtons.FilterMultiOptionButton).Visible)
			{
				((Control)actionButtons.FilterMultiOptionButton).Left = position - ((Control)actionButtons.FilterMultiOptionButton).Width;
				position = ((Control)actionButtons.FilterMultiOptionButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.SortButton != null)
		{
			((Control)actionButtons.SortButton).Visible = stiDashboardViewerControl != null && flag && !flag2 && StiSortMenuHelper.IsAllowUserSorting(element);
			if (((Control)actionButtons.SortButton).Visible)
			{
				((Control)actionButtons.SortButton).Left = position - ((Control)actionButtons.SortButton).Width;
				position = ((Control)actionButtons.SortButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.SelectColumnsButton != null)
		{
			StiTableElement tableElement = element as StiTableElement;
			((Control)actionButtons.SelectColumnsButton).Visible = stiDashboardViewerControl != null && flag && StiColumnSelectionMenuHelper.IsAllowUserColumnsSelection(element) && tableElement != null && tableElement.Columns.Any((StiTableColumn c) => StiTableColumnVisibilityHelper.GetVisible(c, tableElement.Report));
			if (((Control)actionButtons.SelectColumnsButton).Visible)
			{
				((Control)actionButtons.SelectColumnsButton).Left = position - ((Control)actionButtons.SelectColumnsButton).Width;
				position = ((Control)actionButtons.SelectColumnsButton).Left - GetHorDistance();
			}
		}
		if (actionButtons.ViewStateButtons != null)
		{
			foreach (StiViewStateActionButton viewStateButton in actionButtons.ViewStateButtons)
			{
				((Control)viewStateButton).Visible = stiDashboardViewerControl != null && flag;
				((Control)viewStateButton).Left = position - ((Control)viewStateButton).Width;
				position = ((Control)viewStateButton).Left - GetHorDistance();
			}
		}
		IStiActionButtons obj = ((actionButtons is Control) ? actionButtons : null);
		if (obj != null)
		{
			((Control)obj).Invalidate();
		}
	}

	private static int CorrectPositionForVerticalScrollBar(Control parent, int position)
	{
		if (!(parent is StiHolderElementControl stiHolderElementControl))
		{
			return position;
		}
		if (!(stiHolderElementControl.Child is StiTableElementControl stiTableElementControl))
		{
			return position;
		}
		IStiTitleElement stiTitleElement = stiTableElementControl.Element as IStiTitleElement;
		if (stiTitleElement?.Title != null && stiTitleElement.Title.Visible)
		{
			return position;
		}
		StiTableViewControl tableViewControl = stiTableElementControl.TableViewControl;
		if (tableViewControl == null)
		{
			return position;
		}
		if (((Control)((IEnumerable)((Control)tableViewControl).Controls).OfType<VScrollBar>().First()).Visible)
		{
			position -= SystemInformation.VerticalScrollBarWidth + StiScale.I4;
		}
		return position;
	}

	internal static void CreateActionButtons(IStiActionButtons actionButtons, IStiElement element)
	{
		Control val = (Control)((actionButtons is Control) ? actionButtons : null);
		Point position = new Point(val.Width - GetHorDistance(), GetVertDistance());
		actionButtons.SaveButton = CreateSaveButton(element, position);
		if (actionButtons.SaveButton != null)
		{
			position.X = ((Control)actionButtons.SaveButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.SaveButton);
			StiSaveActionButton saveButton = actionButtons.SaveButton;
			if (saveButton != null)
			{
				((Control)saveButton).BringToFront();
			}
		}
		actionButtons.ViewDataButton = CreateViewDataButton(element, position);
		if (actionButtons.ViewDataButton != null)
		{
			position.X = ((Control)actionButtons.ViewDataButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.ViewDataButton);
			StiViewDataActionButton viewDataButton = actionButtons.ViewDataButton;
			if (viewDataButton != null)
			{
				((Control)viewDataButton).BringToFront();
			}
		}
		actionButtons.CloseFullScreenButton = CreateCloseFullScreenButton(element, position);
		if (actionButtons.CloseFullScreenButton != null)
		{
			val.Controls.Add((Control)(object)actionButtons.CloseFullScreenButton);
			((Control)actionButtons.CloseFullScreenButton).BringToFront();
		}
		actionButtons.OpenFullScreenButton = CreateOpenFullScreenButton(element, position);
		if (actionButtons.OpenFullScreenButton != null)
		{
			position.X = ((Control)actionButtons.OpenFullScreenButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.OpenFullScreenButton);
			StiOpenFullScreenActionButton openFullScreenButton = actionButtons.OpenFullScreenButton;
			if (openFullScreenButton != null)
			{
				((Control)openFullScreenButton).BringToFront();
			}
		}
		actionButtons.DrillUpButton = CreateDrillUpButton(element, position);
		if (actionButtons.DrillUpButton != null)
		{
			position.X = ((Control)actionButtons.DrillUpButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.DrillUpButton);
			StiDrillUpActionButton drillUpButton = actionButtons.DrillUpButton;
			if (drillUpButton != null)
			{
				((Control)drillUpButton).BringToFront();
			}
		}
		actionButtons.DrillDownButton = CreateDrillDownButton(element, position);
		if (actionButtons.DrillDownButton != null)
		{
			position.X = ((Control)actionButtons.DrillDownButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.DrillDownButton);
			StiDrillDownActionButton drillDownButton = actionButtons.DrillDownButton;
			if (drillDownButton != null)
			{
				((Control)drillDownButton).BringToFront();
			}
		}
		actionButtons.DrillDownCancelButton = CreateDrillCancelButton(element, position);
		if (actionButtons.DrillDownCancelButton != null)
		{
			position.X = ((Control)actionButtons.DrillDownCancelButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.DrillDownCancelButton);
			StiDrillDownCancelActionButton drillDownCancelButton = actionButtons.DrillDownCancelButton;
			if (drillDownCancelButton != null)
			{
				((Control)drillDownCancelButton).BringToFront();
			}
		}
		actionButtons.DrillDownSelectedButton = CreateDrillApplyButton(element, position);
		if (actionButtons.DrillDownSelectedButton != null)
		{
			position.X = ((Control)actionButtons.DrillDownSelectedButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.DrillDownSelectedButton);
			StiDrillDownSelectedActionButton drillDownSelectedButton = actionButtons.DrillDownSelectedButton;
			if (drillDownSelectedButton != null)
			{
				((Control)drillDownSelectedButton).BringToFront();
			}
		}
		actionButtons.RemoveFilterButton = CreateRemoveFilterButton(element, position);
		if (actionButtons.RemoveFilterButton != null)
		{
			position.X = ((Control)actionButtons.RemoveFilterButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.RemoveFilterButton);
			StiRemoveFilterActionButton removeFilterButton = actionButtons.RemoveFilterButton;
			if (removeFilterButton != null)
			{
				((Control)removeFilterButton).BringToFront();
			}
		}
		actionButtons.FilterMultiOptionButton = CreateFilterMultiOptionButton(element, position);
		if (actionButtons.FilterMultiOptionButton != null)
		{
			position.X = ((Control)actionButtons.FilterMultiOptionButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.FilterMultiOptionButton);
			((Control)actionButtons.FilterMultiOptionButton).BringToFront();
		}
		actionButtons.SortButton = CreateSortButton(element, position);
		if (actionButtons.SortButton != null)
		{
			position.X = ((Control)actionButtons.SortButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.SortButton);
			StiSortActionButton sortButton = actionButtons.SortButton;
			if (sortButton != null)
			{
				((Control)sortButton).BringToFront();
			}
		}
		actionButtons.SelectColumnsButton = CreateSelectColumnsButton(element, position);
		if (actionButtons.SelectColumnsButton != null)
		{
			position.X = ((Control)actionButtons.SelectColumnsButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)actionButtons.SelectColumnsButton);
			StiColumnSelectionActionButton selectColumnsButton = actionButtons.SelectColumnsButton;
			if (selectColumnsButton != null)
			{
				((Control)selectColumnsButton).BringToFront();
			}
		}
		IStiUserViewStates stiUserViewStates = element as IStiUserViewStates;
		if (!StiUserViewStatesMenuHelper.IsAllowUserViewStatesChanging(element) || stiUserViewStates?.UserViewStates == null || stiUserViewStates == null || stiUserViewStates.UserViewStates.Count <= 1)
		{
			return;
		}
		actionButtons.ViewStateButtons = new List<StiViewStateActionButton>();
		List<StiUserViewState> userViewStates = stiUserViewStates.UserViewStates;
		userViewStates.Reverse();
		int num = 0;
		foreach (StiUserViewState item in userViewStates)
		{
			StiViewStateActionButton stiViewStateActionButton = CreateViewStateButton(element, position, num);
			((Control)stiViewStateActionButton).Tag = item;
			actionButtons.ViewStateButtons.Add(stiViewStateActionButton);
			position.X = ((Control)stiViewStateActionButton).Left - GetHorDistance();
			val.Controls.Add((Control)(object)stiViewStateActionButton);
			if (stiViewStateActionButton != null)
			{
				((Control)stiViewStateActionButton).BringToFront();
			}
			num++;
		}
	}

	private static StiSaveActionButton CreateSaveButton(IStiElement element, Point position)
	{
		if (!StiOptions.Viewer.Windows.ShowSaveButton.GetValueOrDefault(true) || (!StiOptions.Viewer.Windows.Exports.ShowImage && !StiOptions.Viewer.Windows.Exports.ShowPdf && !StiOptions.Viewer.Windows.Exports.ShowExcel && (!StiOptions.Viewer.Windows.Exports.ShowData || !(element is IStiTableElement))))
		{
			return null;
		}
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiInteractionLayout { ShowSaveButton: false })
		{
			return null;
		}
		return new StiSaveActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiViewDataActionButton CreateViewDataButton(IStiElement element, Point position)
	{
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiInteractionLayout { ShowViewDataButton: false })
		{
			return null;
		}
		return new StiViewDataActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiViewStateActionButton CreateViewStateButton(IStiElement element, Point position, int index)
	{
		List<StiUserViewState> list = (element as IStiUserViewStates)?.UserViewStates;
		StiUserViewState state = list[index];
		bool isDarkStyle = StiDashboardStyleHelper.IsDarkStyle(element);
		return new StiViewStateActionButton(StiControlColorCollection.GetActionColors(element), position, isDarkStyle, state);
	}

	private static StiOpenFullScreenActionButton CreateOpenFullScreenButton(IStiElement element, Point position)
	{
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiInteractionLayout { ShowFullScreenButton: false })
		{
			return null;
		}
		return new StiOpenFullScreenActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiSortActionButton CreateSortButton(IStiElement element, Point position)
	{
		if (!StiSortMenuHelper.IsAllowUserSorting(element))
		{
			return null;
		}
		return new StiSortActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiColumnSelectionActionButton CreateSelectColumnsButton(IStiElement element, Point position)
	{
		if (!StiColumnSelectionMenuHelper.IsAllowUserColumnsSelection(element))
		{
			return null;
		}
		return new StiColumnSelectionActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiDrillUpActionButton CreateDrillUpButton(IStiElement element, Point position)
	{
		return new StiDrillUpActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiDrillDownActionButton CreateDrillDownButton(IStiElement element, Point position)
	{
		return new StiDrillDownActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiDrillDownSelectedActionButton CreateDrillApplyButton(IStiElement element, Point position)
	{
		StiDrillDownSelectedActionButton stiDrillDownSelectedActionButton = new StiDrillDownSelectedActionButton(StiControlColorCollection.GetActionColors(element), position);
		string cleaned = Loc.GetCleaned("Dashboard", "DrillDownSelected");
		int num = TextRenderer.MeasureText(cleaned, ((Control)stiDrillDownSelectedActionButton).Font).Width + (int)Math.Ceiling(StiScale.XX(4.0));
		((Control)stiDrillDownSelectedActionButton).Left = position.X - (int)StiScale.XX(22 + num + 4);
		((Control)stiDrillDownSelectedActionButton).Width = (int)StiScale.XX(22 + num);
		return stiDrillDownSelectedActionButton;
	}

	private static StiDrillDownCancelActionButton CreateDrillCancelButton(IStiElement element, Point position)
	{
		StiDrillDownCancelActionButton stiDrillDownCancelActionButton = new StiDrillDownCancelActionButton(StiControlColorCollection.GetActionColors(element), position);
		string cleaned = Loc.GetCleaned("Buttons", "Cancel");
		Size size = TextRenderer.MeasureText(cleaned, ((Control)stiDrillDownCancelActionButton).Font);
		double num = Math.Max(StiScale.XX(40.0), size.Width + (int)Math.Ceiling(StiScale.XX(4.0)));
		((Control)stiDrillDownCancelActionButton).Left = position.X - (int)StiScale.XX(22.0 + num + 4.0);
		((Control)stiDrillDownCancelActionButton).Width = (int)StiScale.XX(22.0 + num);
		return stiDrillDownCancelActionButton;
	}

	private static StiRemoveFilterActionButton CreateRemoveFilterButton(IStiElement element, Point position)
	{
		return new StiRemoveFilterActionButton(StiControlColorCollection.GetActionColors(element), position);
	}

	private static StiFilterMultiOptionActionCheckBox CreateFilterMultiOptionButton(IStiElement element, Point position)
	{
		StiFilterMultiOptionActionCheckBox stiFilterMultiOptionActionCheckBox = new StiFilterMultiOptionActionCheckBox(StiControlColorCollection.GetActionColors(element), position);
		((CheckBox)stiFilterMultiOptionActionCheckBox).Checked = GetFilterMultiOption(element);
		((ButtonBase)stiFilterMultiOptionActionCheckBox).Image = GetFilterMultiOptionImage(element);
		return stiFilterMultiOptionActionCheckBox;
	}

	private static StiCloseFullScreenActionButton CreateCloseFullScreenButton(IStiElement element, Point position)
	{
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiInteractionLayout { ShowFullScreenButton: false })
		{
			return null;
		}
		StiCloseFullScreenActionButton stiCloseFullScreenActionButton = new StiCloseFullScreenActionButton(StiControlColorCollection.GetActionColors(element), position);
		string text = Loc.Get("Buttons", "Close");
		int num = TextRenderer.MeasureText(text, ((Control)stiCloseFullScreenActionButton).Font).Width + 8;
		((Control)stiCloseFullScreenActionButton).Left = position.X - (int)StiScale.XX(22.0) + num + StiScaleUI.I10;
		((Control)stiCloseFullScreenActionButton).Width = (int)StiScale.XX(22.0) + num + StiScaleUI.I10;
		return stiCloseFullScreenActionButton;
	}

	internal static void AttachEvents(Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (control is IStiActionButtons buttons)
		{
			AttachViewStateButtonEvents(buttons, control, element, menuActions);
			AttachSortButtonEvents(buttons, control, element, menuActions);
			AttachSelectColumnsButtonEvents(buttons, control, element, menuActions);
			AttachSaveButtonEvents(buttons, control, element, menuActions);
			AttachViewDataButtonEvents(buttons, control, element, menuActions);
			AttachOpenFullScreenButtonEvents(buttons);
			AttachDrillUpButtonEvents(buttons, element);
			AttachDrillDownButtonEvents(buttons, element);
			AttachDrillDownSelectedButtonEvents(buttons, element);
			AttachDrillCancelButtonEvents(buttons, element);
			AttachFilterMultiOptionButtonEvents(buttons, element);
			AttachRemoveFilterButtonEvents(buttons, element);
			AttachCloseFullScreenButtonEvents(buttons);
		}
	}

	private static void AttachViewStateButtonEvents(IStiActionButtons buttons, Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (buttons.ViewStateButtons == null || buttons.ViewStateButtons.Count == 0)
		{
			return;
		}
		foreach (StiViewStateActionButton viewStateButton in buttons.ViewStateButtons)
		{
			((Control)viewStateButton).Click += delegate(object sender, EventArgs e)
			{
				StiViewStateActionButton stiViewStateActionButton = sender as StiViewStateActionButton;
				StiUserViewState stiUserViewState = ((Control)stiViewStateActionButton).Tag as StiUserViewState;
				IStiUserViewStates stiUserViewStates = element as IStiUserViewStates;
				if (stiUserViewState != null && stiUserViewStates != null)
				{
					StiHolderElementControl stiHolderElementControl = ((Control)stiViewStateActionButton).Parent as StiHolderElementControl;
					if (stiUserViewStates.SelectedViewStateKey != stiUserViewState.Key)
					{
						StiAnimationEngine stiAnimationEngine = stiHolderElementControl?.ElementChild?.AnimationEngine;
						if (stiAnimationEngine != null)
						{
							if (stiAnimationEngine.IsRunning())
							{
								return;
							}
							stiAnimationEngine.StartReverse(0.3);
						}
					}
					stiUserViewStates.SwitchSelectedViewState(stiUserViewState.Key);
					if (!element.IsDefined)
					{
						StiAtomElementControl stiAtomElementControl = stiHolderElementControl?.ElementChild as StiAtomElementControl;
						stiAtomElementControl?.CreateChild();
						if (stiAtomElementControl != null)
						{
							((Control)stiAtomElementControl).Invalidate();
						}
					}
					stiHolderElementControl?.ViewStateButtons?.ForEach(delegate(StiViewStateActionButton c)
					{
						((Control)c).Invalidate();
					});
					stiHolderElementControl?.ElementChild?.RefreshStatus(checkProgressStatus: true);
					RefreshButtonsVisibility(buttons, element);
				}
			};
		}
	}

	private static void AttachSortButtonEvents(IStiActionButtons buttons, Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (buttons.SortButton != null)
		{
			((Control)buttons.SortButton).Click += delegate(object sender, EventArgs e)
			{
				Control val = (Control)((sender is Control) ? sender : null);
				new StiSortMenu(menuActions, control, element).CreateItems(element);
				Point point = new Point(val.Bounds.Right, val.Bounds.Bottom + StiScale.I1);
				point = control.PointToScreen(point);
				((ToolStripDropDown)menuActions).Show(point, (ToolStripDropDownDirection)2);
			};
		}
	}

	private static void AttachSelectColumnsButtonEvents(IStiActionButtons buttons, Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (buttons.SelectColumnsButton != null)
		{
			((Control)buttons.SelectColumnsButton).Click += delegate(object sender, EventArgs e)
			{
				Control val = (Control)((sender is Control) ? sender : null);
				new StiColumnSelectionMenu(menuActions, control, element).CreateItems(element);
				Point point = new Point(val.Bounds.Right, val.Bounds.Bottom + StiScale.I1);
				point = control.PointToScreen(point);
				((ToolStripDropDown)menuActions).Show(point, (ToolStripDropDownDirection)2);
			};
		}
	}

	private static void AttachSaveButtonEvents(IStiActionButtons buttons, Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (buttons.SaveButton != null)
		{
			((Control)buttons.SaveButton).Click += delegate(object sender, EventArgs e)
			{
				Control val = (Control)((sender is Control) ? sender : null);
				new StiActionsMenu(menuActions, control, element).CreateItems(element);
				Point point = new Point(val.Bounds.Right, val.Bounds.Bottom + StiScale.I1);
				point = control.PointToScreen(point);
				((ToolStripDropDown)menuActions).Show(point, (ToolStripDropDownDirection)2);
			};
		}
	}

	private static void AttachViewDataButtonEvents(IStiActionButtons buttons, Control control, IStiElement element, StiContextMenu menuActions)
	{
		if (buttons.ViewDataButton != null)
		{
			((Control)buttons.ViewDataButton).Click += delegate(object sender, EventArgs e)
			{
				Control val = (Control)((sender is Control) ? sender : null);
				StiViewDataForm stiViewDataForm = new StiViewDataForm(element);
				StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(control);
				((Form)stiViewDataForm).Show((IWin32Window)(object)((stiDashboardViewerControl != null) ? ((Control)stiDashboardViewerControl).FindForm() : null));
			};
		}
	}

	private static void AttachOpenFullScreenButtonEvents(IStiActionButtons buttons)
	{
		if (buttons.OpenFullScreenButton != null)
		{
			((Control)buttons.OpenFullScreenButton).Click += delegate(object sender, EventArgs e)
			{
				object obj = ((sender is Control) ? sender : null);
				Control val = ((obj != null) ? ((Control)obj).Parent : null);
				StiDashboardViewerHelper.FindViewer(val)?.ElementFullScreen.Show(val);
			};
		}
	}

	private static void AttachDrillUpButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.DrillUpButton == null)
		{
			return;
		}
		((Control)buttons.DrillUpButton).Click += delegate(object sender, EventArgs e)
		{
			if (element is IStiDrillDownElement)
			{
				StiDrillDownHelper.DrillUp(element);
				object obj = ((sender is Control) ? sender : null);
				Control control = ((obj != null) ? ((Control)obj).Parent : null);
				RefreshViewer(control);
				RefreshButtonsVisibility(buttons, element);
			}
		};
	}

	private static void AttachDrillDownButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.DrillDownButton == null)
		{
			return;
		}
		((Control)buttons.DrillDownButton).Click += delegate(object sender, EventArgs e)
		{
			object obj = ((sender is Control) ? sender : null);
			if (((obj != null) ? ((Control)obj).Parent : null) is StiHolderElementControl stiHolderElementControl)
			{
				stiHolderElementControl.IsDrillSelectionActivated = true;
				RefreshButtonsVisibility(buttons, element);
			}
		};
	}

	private static void AttachDrillDownSelectedButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.DrillDownSelectedButton == null)
		{
			return;
		}
		((Control)buttons.DrillDownSelectedButton).Click += delegate(object sender, EventArgs e)
		{
			object obj = ((sender is Control) ? sender : null);
			if (((obj != null) ? ((Control)obj).Parent : null) is StiHolderElementControl { Child: StiChartElementControl child } stiHolderElementControl)
			{
				List<StiDataFilterRule> filters = child.GetFilters(isDrillDown: true);
				StiDrillDownHelper.DrillDown(child.ChartElement, filters);
				stiHolderElementControl.IsDrillSelectionActivated = false;
				stiHolderElementControl.Child.Invalidate();
				child.RemoveFilter();
				RefreshViewer((Control)(object)child);
				RefreshButtonsVisibility(buttons, element);
			}
		};
	}

	private static void AttachDrillCancelButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.DrillDownCancelButton == null)
		{
			return;
		}
		((Control)buttons.DrillDownCancelButton).Click += delegate(object sender, EventArgs e)
		{
			object obj = ((sender is Control) ? sender : null);
			if (((obj != null) ? ((Control)obj).Parent : null) is StiHolderElementControl stiHolderElementControl)
			{
				(stiHolderElementControl.Child as StiChartElementControl)?.ResetGeomSelection();
				stiHolderElementControl.IsDrillSelectionActivated = false;
				stiHolderElementControl.Child.Invalidate();
				RefreshButtonsVisibility(buttons, element);
			}
		};
	}

	private static void AttachFilterMultiOptionButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.FilterMultiOptionButton != null)
		{
			((CheckBox)buttons.FilterMultiOptionButton).CheckedChanged += delegate(object sender, EventArgs e)
			{
				object obj = ((sender is Control) ? sender : null);
				Control val = ((obj != null) ? ((Control)obj).Parent : null);
				InvertFilterMultiOption(element);
				((ButtonBase)buttons.FilterMultiOptionButton).Image = GetFilterMultiOptionImage(element);
				(val as StiHolderElementControl)?.SwitchFilterMultiOption();
			};
		}
	}

	private static void AttachRemoveFilterButtonEvents(IStiActionButtons buttons, IStiElement element)
	{
		if (buttons.RemoveFilterButton == null)
		{
			return;
		}
		((Control)buttons.RemoveFilterButton).Click += delegate(object sender, EventArgs e)
		{
			object obj = ((sender is Control) ? sender : null);
			Control val = ((obj != null) ? ((Control)obj).Parent : null);
			(val as StiHolderElementControl)?.RemoveFilter();
			StiDashboardViewerHelper.FindViewer(val)?.RemoveFilter(element);
			if (buttons.RemoveFilterButton != null)
			{
				((Control)buttons.RemoveFilterButton).Visible = false;
			}
			if (buttons.FilterMultiOptionButton != null)
			{
				((Control)buttons.FilterMultiOptionButton).Visible = false;
			}
		};
	}

	private static void AttachCloseFullScreenButtonEvents(IStiActionButtons buttons)
	{
		if (buttons.CloseFullScreenButton == null)
		{
			return;
		}
		((Control)buttons.CloseFullScreenButton).Click += delegate(object sender, EventArgs e)
		{
			object obj = ((sender is Control) ? sender : null);
			Control control = ((obj != null) ? ((Control)obj).Parent : null);
			StiDashboardViewerControl stiDashboardViewerControl = StiDashboardViewerHelper.FindViewer(control);
			if (stiDashboardViewerControl != null)
			{
				stiDashboardViewerControl.ElementFullScreen.Close();
				stiDashboardViewerControl.RefreshViewerButtons(stiDashboardViewerControl.Report);
			}
		};
	}

	public static bool GetFilterMultiOption(IStiElement element)
	{
		StiSettings.Load();
		return StiSettings.GetBool("DashboardViewer-FilterMultiOption", element.GetKey());
	}

	private static void InvertFilterMultiOption(IStiElement element)
	{
		StiSettings.Set("DashboardViewer-FilterMultiOption", element.GetKey(), !GetFilterMultiOption(element));
	}

	private static Image GetFilterMultiOptionImage(IStiElement element)
	{
		bool isDark = StiDashboardStyleHelper.IsDarkStyle(element);
		if (!GetFilterMultiOption(element))
		{
			return StiDashboardViewerImages.Actions.FilterMultiOptionOff(isDark);
		}
		return StiDashboardViewerImages.Actions.FilterMultiOptionOn(isDark);
	}

	private static void RefreshViewer(Control control)
	{
		StiDashboardViewerHelper.FindViewer(control)?.RefreshViewer();
	}

	private static int GetHorDistance()
	{
		return StiScale.XXI(4.0);
	}

	private static int GetVertDistance()
	{
		return StiScale.YYI(4.0);
	}
}
