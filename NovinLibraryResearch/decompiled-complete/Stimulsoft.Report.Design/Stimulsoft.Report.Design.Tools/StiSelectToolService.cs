using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Dashboard.Chart;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design.Tools;

[StiServiceBitmap(typeof(StiSelectToolService), "Stimulsoft.Report.Design.Bmp.Tools.Select.bmp")]
public class StiSelectToolService : StiToolService
{
	private StiAction action;

	private static Cursor resizeColumnsCursor;

	private static Cursor resizeRowsCursor;

	private bool isMouseMove = true;

	private bool selectedInsideContainer;

	private bool requereUpdateSelectedObjects;

	private Rectangle oldRect = Rectangle.Empty;

	private bool copyCheck;

	private bool selectedBeforeCopyCheck;

	private StiComponent selectedComponent;

	private double scale = StiScale.Factor;

	public override string ServiceName => Loc.Get("Toolbox", "Select");

	public StiTableHelper TableHelper { get; set; }

	public StiTableHelperV2 TableHelperV2 { get; set; }

	public bool RedrawAll { get; set; }

	public bool AltKeyMode { get; set; }

	public bool ControlKeyMode { get; set; }

	public bool AltKey => (Control.ModifierKeys & 0x40000) > 0;

	public bool ControlKey => (Control.ModifierKeys & 0x20000) > 0;

	public override int Position => 1;

	public override Keys Shortcut => (Keys)112;

	public StiAction Action
	{
		get
		{
			return action;
		}
		set
		{
			if (action != value)
			{
				action = value;
				if (action == StiAction.None || action == StiAction.Select || action == StiAction.Move)
				{
					StiElementActionHelper.CheckStatus(null, Designer);
				}
				else
				{
					StiElementActionHelper.Hide();
				}
				if (Designer?.Report != null)
				{
					base.Info.CurrentAction = value;
				}
			}
		}
	}

	internal bool InsertComponents { get; set; }

	internal bool SkipUndoRedoOperation { get; set; }

	internal bool DragDropAction { get; set; }

	internal StiComponent CreatedComponent { get; set; }

	internal bool IsCreatingComponent
	{
		get
		{
			if (Designer?.Info != null)
			{
				return Designer.Info.IsComponentCreating;
			}
			return false;
		}
		set
		{
			if (Designer?.Info != null)
			{
				Designer.Info.IsComponentCreating = value;
			}
		}
	}

	internal bool IsShiftMode { get; set; }

	internal bool IsDragDropComponent
	{
		get
		{
			if (Designer?.Info != null)
			{
				return Designer.Info.IsDragDropComponent;
			}
			return false;
		}
		set
		{
			if (Designer?.Info != null)
			{
				Designer.Info.IsDragDropComponent = value;
			}
		}
	}

	internal bool IsDragDropFromToolBox
	{
		get
		{
			if (Designer?.Info != null)
			{
				return Designer.Info.IsDragDropFromToolBox;
			}
			return false;
		}
		set
		{
			if (Designer?.Info != null)
			{
				Designer.Info.IsDragDropFromToolBox = value;
			}
		}
	}

	internal StiComponent LatestDragingComponent { get; set; }

	internal int StartPageX { get; set; }

	internal int StartPageY { get; set; }

	internal RectangleD OldSelectedRect { get; set; } = RectangleD.Empty;


	internal long? SuspendTickCount { get; set; }

	public static Cursor ResizeColumnsCursor
	{
		get
		{
			if (StiScale.Factor > 3.0)
			{
				return Cursors.SizeWE;
			}
			if (resizeColumnsCursor != (Cursor)null)
			{
				return resizeColumnsCursor;
			}
			if (resizeColumnsCursor == (Cursor)null)
			{
				resizeColumnsCursor = ((StiScale.Factor == 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.ResizeColumns.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.ResizeColumns_x2.cur"));
			}
			return resizeColumnsCursor;
		}
	}

	public static Cursor ResizeRowsCursor
	{
		get
		{
			if (StiScale.Factor > 3.0)
			{
				return Cursors.SizeNS;
			}
			if (resizeRowsCursor != (Cursor)null)
			{
				return resizeRowsCursor;
			}
			if (resizeRowsCursor == (Cursor)null)
			{
				resizeRowsCursor = ((StiScale.Factor == 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.ResizeRows.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.ResizeRows_x2.cur"));
			}
			return resizeRowsCursor;
		}
	}

	public override void OnMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Invalid comparison between Unknown and I4
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Invalid comparison between Unknown and I4
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Invalid comparison between Unknown and I4
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Invalid comparison between Unknown and I4
		base.OnMouseDown(sender, e);
		Designer.PageView.StartPos = ((Control)Designer.CurrentPageView.View).PointToScreen(new Point(e.X, e.Y));
		isMouseMove = false;
		selectedInsideContainer = false;
		AltKeyMode = AltKey;
		ControlKeyMode = ControlKey;
		if ((int)e.Button == 1048576 && InsertComponents)
		{
			OnMouseUp(sender, e);
			return;
		}
		if ((int)e.Button == 1048576 && !InsertComponents)
		{
			StiPageViewControl stiPageViewControl = (StiPageViewControl)(object)((Control)(StiViewControl)sender).Parent;
			SetMoveOffsetRectangle();
			double num = Designer.XToPage(e.X);
			double num2 = Designer.YToPage(e.Y);
			if (action == StiAction.SelectRow)
			{
				ChangeSelectRowTable(num, num2);
				return;
			}
			if (action == StiAction.SelectColumn)
			{
				ChangeSelectColumnTable(num, num2);
				return;
			}
			if (StiTableHelper.IsTableMode(Designer))
			{
				Action = GetTableAction(num, num2);
				if (Action != 0 && Action != StiAction.Move)
				{
					base.Info.IsTableMode = true;
					Designer.AddSelectedToPropertyGrid(updateSelectedObjects: false);
					requereUpdateSelectedObjects = true;
					Designer.Builder.Redraw();
					base.Info.IsComponentsMoving = true;
					Designer.UndoRedoSave(Loc.GetMain("Resize"), onlyCurrentPage: true);
					TableHelper = new StiTableHelper(Designer, AltKeyMode);
					TableHelper.SaveComponentsLocation();
					TableHelper.SaveCursorLocation(e.X, e.Y);
					TableHelper.SaveSelectedRectangle();
					return;
				}
			}
			if (Designer.IsDashboardSelected)
			{
				IEnumerable<StiComponent> enumerable = from c in stiPageViewControl.Page.GetSelectedComponents().ToList()
					where c is IStiTitleElement
					select c;
				foreach (StiComponent item in enumerable)
				{
					RectangleD rectangle = StiMarginHelper.ApplyMargin(item as IStiElement, item.GetDisplayRectangle());
					RectangleD rect = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, rectangle));
					RectangleD titleRect = StiInplaceEditorTitleElement.GetTitleRect(rect, item);
					if (!titleRect.IsEmpty && titleRect.Contains(e.X, e.Y))
					{
						StiInplaceEditorTitleElement.InvokeTitleClick(item);
						return;
					}
					IStiUserViewStates stiUserViewStates = item as IStiUserViewStates;
					if (stiUserViewStates?.UserViewStates != null)
					{
						bool flag = StiSortMenuHelper.IsAllowUserSorting(selectedComponent as IStiElement);
						List<StiUserViewState> list = stiUserViewStates.UserViewStates.ToList();
						list.Reverse();
						int num3 = 0;
						foreach (StiUserViewState item2 in list)
						{
							RectangleD viewStateRect = StiInplaceEditorTitleElement.GetViewStateRect(rect, item, num3);
							if (!viewStateRect.IsEmpty && viewStateRect.Contains(e.X, e.Y))
							{
								stiUserViewStates.SwitchSelectedViewState(item2.Key);
								StiInplaceEditorForm editorForm = StiInplaceDesigner.EditorForm;
								if (editorForm != null && !((Control)editorForm).IsDisposed && !((Control)editorForm).Disposing)
								{
									(editorForm as IStiElementEditorForm)?.ReadPropertiesFromElement();
									(editorForm as IStiElementEditorForm)?.RefreshUiPanel();
								}
								return;
							}
							num3++;
						}
					}
					if (StiSortMenuHelper.IsAllowUserSorting(item as IStiElement))
					{
						RectangleD sortRect = StiInplaceEditorTitleElement.GetSortRect(rect, item);
						if (!sortRect.IsEmpty && sortRect.Contains(e.X, e.Y))
						{
							StiContextMenu stiContextMenu = new StiContextMenu();
							stiContextMenu.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(item as IStiElement);
							Rectangle rectangle2 = ((Control)Designer.PageView.GetCurrentPageView().View).RectangleToScreen(sortRect.ToRectangle());
							new StiSortMenu(stiContextMenu, (Control)(object)Designer, item as IStiElement).CreateItems(item as IStiElement);
							((ToolStripDropDown)stiContextMenu).Show(rectangle2.X, rectangle2.Bottom);
							return;
						}
					}
				}
			}
			StiComponent stiComponent = stiPageViewControl.Page.GetComponentInControl(num, num2, base.CurrentPage is IStiForm, StiGuiMode.Gdi, forGetDesigner: true);
			if (stiComponent != null)
			{
				if (stiComponent is StiTable stiTable)
				{
					TableHelperV2 = new StiTableHelperV2(Designer, AltKeyMode);
					TableHelperV2.SelectedTable = stiTable;
					TableHelperV2.SaveComponentsLocation(stiTable);
					TableHelperV2.SaveCursorLocation(e.X, e.Y);
					TableHelperV2.SaveSelectedRectangle();
				}
				else if (TableHelperV2 != null)
				{
					TableHelperV2.SelectedTable = null;
				}
				if (stiComponent is IStiTableCell selectedCell && stiComponent.Parent is StiTable table)
				{
					if (TableHelperV2 != null && TableHelperV2.SelectedCell != null)
					{
						Action = GetTableAction(num, num2);
						if (Action != 0 && Action != StiAction.Move)
						{
							TableHelperV2.SaveComponentsLocation(table);
							TableHelperV2.SaveCursorLocation(e.X, e.Y);
							TableHelperV2.SaveSelectedRectangle();
							return;
						}
					}
					Designer.UndoRedoSave(Loc.GetMain("Resize"), onlyCurrentPage: true);
					TableHelperV2 = new StiTableHelperV2(Designer, AltKeyMode);
					TableHelperV2.SelectedCell = selectedCell;
					TableHelperV2.SaveComponentsLocation(table);
					TableHelperV2.SaveCursorLocation(e.X, e.Y);
					TableHelperV2.SaveSelectedRectangle();
				}
				else if (TableHelperV2 != null)
				{
					TableHelperV2.SelectedCell = null;
				}
				bool flag2 = false;
				RectangleD rect2 = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, stiComponent.GetDisplayRectangle()));
				if (stiComponent is StiBand)
				{
					rect2.Height = ((StiBand)stiComponent).HeaderSize * base.CurrentPage.Zoom * scale;
				}
				StiQuickButton[] quickButtons = Designer.GetQuickButtons(stiComponent);
				if (quickButtons != null && StiQuickButtonHelper.CheckVisibleQuickButtons(rect2))
				{
					int num4 = 0;
					StiQuickButton[] array = quickButtons;
					foreach (StiQuickButton stiQuickButton in array)
					{
						RectangleD quickButtonRect = StiQuickButtonHelper.GetQuickButtonRect(quickButtons, num4, rect2);
						if (quickButtonRect.Contains(e.X, e.Y))
						{
							ResetQuickButtons();
							stiQuickButton.OnClick((Control)sender, new Point((int)quickButtonRect.X, (int)quickButtonRect.Bottom), stiComponent, Designer);
							flag2 = true;
							break;
						}
						num4++;
					}
				}
				Action = StiAction.None;
				if (!flag2)
				{
					base.Info.IsComponentsMoving = true;
					StiElementActionHelper.CheckStatus(stiComponent, Designer);
					StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, stiComponent.GetType());
					Action = componentDesigner.PointInRect(Designer, stiComponent, e.X, e.Y);
					if (((Control.ModifierKeys & 0x10000) == 0 && (Control.ModifierKeys & 0x20000) == 0) & !stiComponent.IsSelected)
					{
						Designer.Report.ResetSelection();
						StiElementActionHelper.CheckStatus(stiComponent, Designer);
					}
					if ((Control.ModifierKeys & 0x10000) > 0)
					{
						stiComponent.Invert();
						StiElementActionHelper.CheckStatus(stiComponent, Designer);
					}
					else
					{
						if ((Control.ModifierKeys & 0x20000) != 0)
						{
							if (Designer.SelectedObjects != null && Designer.SelectedObjects.Length > 1 && Designer.SelectedObjects.Contains(stiComponent))
							{
								stiComponent.IsSelected = false;
							}
							selectedComponent = stiComponent;
							selectedBeforeCopyCheck = stiComponent.IsSelected;
							copyCheck = true;
						}
						if (stiComponent is StiTable stiTable2)
						{
							stiTable2.Select();
							Designer.SelectedObject = stiTable2;
							Designer.LockAddSelectedToPropertyGrid = true;
						}
						else
						{
							stiComponent.Select();
							StiElementActionHelper.CheckStatus(stiComponent, Designer);
						}
					}
					if (stiComponent is IStiTableElement table2)
					{
						StiTableElementClickHelper.SetPointClick(table2, new PointD(Designer.XToPage(e.X), Designer.YToPage(e.Y)));
					}
					if (stiComponent is IStiPivotTableElement table3)
					{
						StiPivotTableElementClickHelper.SetPointClick(table3, new PointD(Designer.XToPage(e.X), Designer.YToPage(e.Y)));
					}
					Designer.AddSelectedToPropertyGrid(updateSelectedObjects: false);
					requereUpdateSelectedObjects = true;
					Designer.Builder.Redraw();
					if (!(stiComponent is IStiTableCell) && (stiComponent.Locked || stiComponent.Inherited))
					{
						Action = StiAction.None;
					}
					if (stiComponent is StiContainer && (Control.ModifierKeys & 0x10000) > 0)
					{
						Action = StiAction.Select;
						StartPageX = e.X;
						StartPageY = e.Y;
						selectedInsideContainer = true;
					}
					foreach (object item3 in Designer.SelectedComponentsOnPage)
					{
						if (item3 is StiComponent comp)
						{
							if (!StiRestrictionsHelper.IsAllowMove(comp) && Action == StiAction.Move)
							{
								Action = StiAction.None;
								break;
							}
							if (!StiRestrictionsHelper.IsAllowResize(comp) && (Action == StiAction.Select || Action == StiAction.ResizeColumns || Action == StiAction.ResizeRows || Action == StiAction.SizeBottom || Action == StiAction.SizeLeft || Action == StiAction.SizeLeftBottom || Action == StiAction.SizeLeftTop || Action == StiAction.SizeRight || Action == StiAction.SizeRightBottom || Action == StiAction.SizeRightTop || Action == StiAction.SizeTop))
							{
								Action = StiAction.None;
								break;
							}
						}
					}
				}
			}
			if (stiComponent != null && stiComponent is IStiForm && Action == StiAction.Move)
			{
				stiComponent = null;
			}
			if (stiComponent == null)
			{
				Designer.PageView.IsAlignSelection = false;
				if ((Control.ModifierKeys & 0x20000) == 0)
				{
					Designer.Report.ResetSelection();
					StiElementActionHelper.CheckStatus(null, Designer);
				}
				Designer.SelectedObject = base.CurrentPage;
				requereUpdateSelectedObjects = true;
				base.CurrentPage.Select();
				Action = StiAction.Select;
				StartPageX = e.X;
				StartPageY = e.Y;
				base.CurrentPage.SelectedRectangle = ConvertRectangle(new RectangleD(num, num2, 0.0, 0.0));
				Designer.Builder.Redraw();
			}
			Designer.AssignTagProperty();
		}
		if ((int)e.Button == 2097152 && !InsertComponents)
		{
			StiPageViewControl stiPageViewControl2 = (StiPageViewControl)(object)((Control)(StiViewControl)sender).Parent;
			StiComponent stiComponent2 = stiPageViewControl2.Page.GetComponentInControl(Designer.XToPage(e.X), Designer.YToPage(e.Y), usePage: false, StiGuiMode.Gdi, forGetDesigner: true);
			if (stiComponent2 == null)
			{
				stiComponent2 = stiPageViewControl2.Page;
			}
			if (!stiComponent2.IsSelected)
			{
				Designer.Report.ResetSelection();
				stiComponent2.Select();
				StiElementActionHelper.CheckStatus(stiComponent2, Designer);
				Designer.AddSelectedToPropertyGrid(updateSelectedObjects: false);
				requereUpdateSelectedObjects = true;
				Designer.Builder.Redraw();
				((Control)Designer).Refresh();
			}
			if (stiComponent2 is IStiTableElement stiTableElement)
			{
				RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, ((StiComponent)stiTableElement).GetDisplayRectangle()));
				Designer.Builder.Redraw(rectangleD.ToRectangle());
			}
		}
	}

	public override void OnMouseMove(object sender, MouseEventArgs e)
	{
		base.OnMouseMove(sender, e);
		isMouseMove = true;
		bool flag = false;
		if (!base.Info.IsComponentsMoving && (Action == StiAction.None || Action == StiAction.SelectColumn || Action == StiAction.SelectRow) && !Designer.PageView.IsDrawSelection)
		{
			CheckMouseOverComponents(e);
		}
		if (!base.Info.IsComponentsMoving && Action == StiAction.None && !Designer.PageView.IsDrawSelection)
		{
			CheckInplaceEditorTitle();
			CheckQuickButtons();
		}
		if (Action != 0)
		{
			StiTextEditorToolService.GetService(Designer)?.TextEditorClose();
		}
		switch (Action)
		{
		case StiAction.SelectColumn:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = ((StiScale.Factor != 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.SelectColumns_x2.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.SelectTableColumns.cur"));
			break;
		case StiAction.SelectRow:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = ((StiScale.Factor != 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.SelectRows_x2.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.SelectTableRows.cur"));
			break;
		case StiAction.Select:
		{
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.Select);
			Rectangle rectangle = new Rectangle(StartPageX, StartPageY, e.X - StartPageX, e.Y - StartPageY);
			RectangleD selectedRectangle = base.CurrentPage.SelectedRectangle;
			RectangleD selectedRectangle2 = ConvertRectangle(StiConvert.RectangleToPage(Designer, rectangle));
			base.CurrentPage.SelectedRectangle = selectedRectangle2;
			selectedRectangle2 = selectedRectangle2.Normalize().FitToRectangle(selectedRectangle);
			if (Designer.IsFormSelected || Designer.IsDashboardSelected)
			{
				Designer.Builder.Redraw();
				return;
			}
			if (selectedRectangle2.Width > 0.0 && selectedRectangle2.Height > 0.0)
			{
				Rectangle clipRect = StiConvert.RectangleToScreen(Designer, selectedRectangle2);
				clipRect.Inflate(2, 2);
				Designer.Builder.Redraw(clipRect);
			}
			break;
		}
		case StiAction.Move:
			flag = true;
			if (!IsDragDropComponent)
			{
				((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.Move);
			}
			if (ControlKeyMode && copyCheck && StiOptions.Designer.AllowCopyControlOperation)
			{
				double num = Math.Abs(Designer.PageView.StartPos.X - Cursor.Position.X) + Math.Abs(Designer.PageView.StartPos.Y - Cursor.Position.Y);
				if (num > 4.0)
				{
					StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
					if (selectedComponents.Count > 0)
					{
						RectangleD rectangleD3 = selectedComponents[0].ComponentToPage(selectedComponents[0].ClientRectangle);
						StiClipboard.LocalMode = true;
						StiActions.Clipboard.InvokeCopy(Designer);
						StiActions.Clipboard.InvokePaste(Designer, redraw: false);
						Designer.AddSelectedToPropertyGrid(updateSelectedObjects: false);
						requereUpdateSelectedObjects = true;
						StiClipboard.LocalMode = false;
						selectedComponents = base.CurrentPage.GetSelectedComponents();
						if (selectedComponents.Count > 0)
						{
							RectangleD rectangleD4 = selectedComponents[0].ComponentToPage(selectedComponents[0].ClientRectangle);
							double num2 = rectangleD3.Left - rectangleD4.Left;
							double num3 = rectangleD3.Top - rectangleD4.Top;
							foreach (StiComponent item2 in selectedComponents)
							{
								item2.Left += num2;
								item2.Top += num3;
							}
						}
					}
					copyCheck = false;
				}
			}
			if (!ControlKeyMode || !copyCheck)
			{
				SetMoveOffsetRectangle();
				Redraw(move: true);
			}
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeLeft:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeLeft);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: false);
			}
			else
			{
				SetSizeLeftOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeRight:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeRight);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: false);
			}
			else
			{
				SetSizeRightOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeTop:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeTop);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: false, resizeVertically: true);
			}
			else
			{
				SetSizeTopOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeBottom:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeBottom);
			if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: false, resizeVertically: true);
			}
			else if (TableHelperV2 != null && TableHelperV2.IsSelectedTable)
			{
				SetSizeBottomOffsetRectangle();
				TableHelperV2.ProcessResizeTable(resizeHorizontally: false, resizeVertically: true);
			}
			else if (TableHelperV2 != null && TableHelperV2.IsSelectedCell)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else
			{
				SetSizeBottomOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeLeftTop:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeLeftTop);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: true);
			}
			else
			{
				SetSizeLeftTopOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeLeftBottom:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeLeftBottom);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: true);
			}
			else
			{
				SetSizeLeftBottomOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeRightTop:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeRightTop);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: true);
			}
			else
			{
				SetSizeRightTopOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.SizeRightBottom:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.SizeRightBottom);
			if (TableHelperV2 != null && TableHelperV2.IsAnySelected)
			{
				TableHelperV2.ProcessResizeCell();
			}
			else if (base.Info.IsTableMode)
			{
				TableHelper.ProcessResizeTable(resizeHorizontally: true, resizeVertically: true);
			}
			else
			{
				SetSizeRightBottomOffsetRectangle();
			}
			Redraw(move: false);
			if (!base.CurrentPage.OffsetRectangle.IsEmpty)
			{
				StiElementActionHelper.Hide();
			}
			break;
		case StiAction.ResizeColumns:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.ResizeColumns);
			TableHelper.ProcessResizeColumns();
			Redraw(move: false);
			break;
		case StiAction.ResizeRows:
			flag = true;
			((Control)Designer.CurrentPageView.View).Cursor = GetCursor(StiAction.ResizeRows);
			TableHelper.ProcessResizeRows();
			Redraw(move: false);
			break;
		default:
		{
			if (Designer.SelectedObject == null)
			{
				break;
			}
			StiAction stiAction = StiAction.None;
			if (StiTableHelper.IsTableMode(Designer))
			{
				double pageX = Designer.XToPage(e.X);
				double pageY = Designer.YToPage(e.Y);
				stiAction = GetTableAction(pageX, pageY);
				if (stiAction == StiAction.ResizeRows)
				{
					stiAction = GetTableAction(pageX, pageY);
				}
			}
			else
			{
				List<object> list = new List<object>();
				object[] selectedObjects = Designer.SelectedObjects;
				foreach (object item in selectedObjects)
				{
					list.Add(item);
				}
				foreach (object item3 in list)
				{
					if (item3 is StiComponent stiComponent && (!(stiComponent is StiPage) || stiComponent is IStiForm) && stiComponent.Page != null)
					{
						StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, stiComponent.GetType());
						StiAction stiAction2 = componentDesigner.PointInRect(Designer, stiComponent, e.X, e.Y);
						if (stiAction2 != 0 && (stiAction2 != StiAction.Move || stiAction == StiAction.None))
						{
							stiAction = stiAction2;
						}
					}
				}
				foreach (IStiElement item4 in (from c in Designer.Report.GetCurrentPage().GetComponents().ToList()
					where c is IStiElement
					select c).Cast<IStiElement>())
				{
					if (item4 is IStiTableElement stiTableElement)
					{
						StiTableElementMouseOverHelper.SetMouseOverPoint(stiTableElement, new PointD(Designer.XToPage(e.X), Designer.YToPage(e.Y)));
						RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, ((StiComponent)stiTableElement).GetDisplayRectangle()));
						Designer.Builder.Redraw(rectangleD.ToRectangle());
					}
					if (item4 is IStiPivotTableElement stiPivotTableElement)
					{
						StiPivotTableElementMouseOverHelper.SetMouseOverPoint(stiPivotTableElement, new PointD(Designer.XToPage(e.X), Designer.YToPage(e.Y)));
						RectangleD rectangleD2 = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, ((StiComponent)stiPivotTableElement).GetDisplayRectangle()));
						Designer.Builder.Redraw(rectangleD2.ToRectangle());
					}
				}
			}
			if (stiAction != StiAction.Move)
			{
				((Control)Designer.CurrentPageView.View).Cursor = GetCursor(stiAction);
				flag = true;
			}
			break;
		}
		}
		if (!flag)
		{
			((Control)Designer.CurrentPageView.View).Cursor = Cursors.Default;
		}
	}

	public override void OnMouseUp(object sender, MouseEventArgs e)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Invalid comparison between Unknown and I4
		if (SuspendTickCount.HasValue && Environment.TickCount - SuspendTickCount < 300)
		{
			SuspendTickCount = null;
			return;
		}
		SuspendTickCount = null;
		oldRect = Rectangle.Empty;
		base.OnMouseUp(sender, e);
		bool flag = true;
		if (selectedBeforeCopyCheck && copyCheck)
		{
			selectedComponent.IsSelected = !selectedBeforeCopyCheck;
		}
		if ((int)e.Button == 1048576)
		{
			StiPageViewControl stiPageViewControl = (StiPageViewControl)(object)((Control)(StiViewControl)sender).Parent;
			switch (Action)
			{
			case StiAction.Select:
				Designer.LockAddSelectedToPropertyGrid = false;
				Action = StiAction.None;
				if ((Control.ModifierKeys & 0x20000) == 0)
				{
					Designer.Report.ResetSelection();
				}
				if ((Control.ModifierKeys & 0x10000) == 0)
				{
					stiPageViewControl.Page.SelectInRectangleCheckBands(base.CurrentPage.SelectedRectangle);
				}
				else
				{
					stiPageViewControl.Page.SelectInRectangle(base.CurrentPage.SelectedRectangle);
				}
				if (selectedInsideContainer && base.CurrentPage.GetFirstSelectableCompanent() is StiContainer stiContainer2)
				{
					stiContainer2.IsSelected = false;
				}
				base.CurrentPage.SelectedRectangle = RectangleD.Empty;
				if (stiPageViewControl.Page.GetSelectedComponents().Count == 0)
				{
					stiPageViewControl.Page.Select();
				}
				break;
			case StiAction.Move:
			{
				Action = StiAction.None;
				RectangleD offsetRectangle2 = base.CurrentPage.OffsetRectangle;
				Designer.LockAddSelectedToPropertyGrid = true;
				if ((base.CurrentPage.OffsetRectangle.Left != 0.0 || base.CurrentPage.OffsetRectangle.Top != 0.0) && !InsertComponents)
				{
					Designer.UndoRedoSave(Loc.GetMain("Move"), onlyCurrentPage: true);
				}
				base.CurrentPage.OffsetRectangle = new RectangleD(0.0, 0.0, 0.0, 0.0);
				offsetRectangle2 = offsetRectangle2.AlignToGrid(base.CurrentPage.GridSize, base.Info.AlignToGrid && !AltKeyMode);
				stiPageViewControl.Page.ChangePosition(offsetRectangle2);
				CheckPrimitives();
				if (InsertComponents)
				{
					StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
					if (selectedComponents != null && selectedComponents.Count > 1 && selectedComponents.ToList().All((StiComponent c) => c is StiBand))
					{
						StiComponent firstBand = selectedComponents.ToList().FirstOrDefault();
						double pos = firstBand.Top;
						selectedComponents.ToList().Skip(1).ToList()
							.ForEach(delegate(StiComponent c)
							{
								c.Top = pos;
								pos += firstBand.Height / 10.0;
							});
					}
					stiPageViewControl.Page.Correct(onlySelect: true);
				}
				bool flag2 = false;
				if (IsDragDropComponent)
				{
					Designer.Report.IsSelected = false;
				}
				if (IsDragDropComponent && LatestDragingComponent is StiText)
				{
					StiText stiText = LatestDragingComponent as StiText;
					if (Designer.UndoRedo != null)
					{
						Designer.UndoRedo.UndoStackPop();
					}
					StiText highlightedText = GetHighlightedText();
					if (highlightedText != null)
					{
						if (StiOptions.Designer.IsHighlightComponentServiceEnabled)
						{
							flag = false;
							StiComponentsCollection selected = Designer.GetSelected();
							Designer.ActivateHighlightComponentService(highlightedText, stiText, (selected.Count > 1) ? selected[1] : null);
						}
						else
						{
							ResetHighlightState();
							flag2 = true;
							if ((Control.ModifierKeys & 0x20000) > 0)
							{
								highlightedText.Text.Value += stiText.Text.Value;
							}
							else
							{
								highlightedText.Text.Value = stiText.Text.Value;
							}
						}
					}
					else
					{
						stiPageViewControl.Page.Correct(onlySelect: true);
					}
					Designer.TextAliasesHash.Clear();
				}
				if (flag2)
				{
					base.CurrentPage.RemoveAllSelected();
					stiPageViewControl.Page.Correct();
				}
				else
				{
					StiComponentsCollection selectedComponentsWithChilds = base.CurrentPage.GetSelectedComponentsWithChilds();
					foreach (StiComponent item in selectedComponentsWithChilds)
					{
						item.Dockable = true;
					}
					StiComponent firstSelectableCompanent = base.CurrentPage.GetFirstSelectableCompanent();
					StiContainer stiContainer = null;
					if (firstSelectableCompanent != null)
					{
						stiContainer = firstSelectableCompanent.Parent;
						if (firstSelectableCompanent.Parent is StiBand)
						{
							firstSelectableCompanent.Parent.Components.SortBandsByLeftPosition();
						}
						else
						{
							firstSelectableCompanent.Parent.Components.SortBandsByTopPosition();
						}
					}
					if (InsertComponents)
					{
						stiPageViewControl.Page.DockToContainer();
					}
					if (firstSelectableCompanent is StiBand)
					{
						stiPageViewControl.Page.Correct(onlySelect: true);
					}
					else if (isMouseMove)
					{
						stiPageViewControl.Page.Correct();
					}
					if (flag && !IsDragDropComponent && firstSelectableCompanent != null && firstSelectableCompanent.Parent is StiBand && stiContainer != null && stiContainer != firstSelectableCompanent.Parent && selectedComponentsWithChilds.Count <= 2)
					{
						Designer.ActivatePlacementComponentService(firstSelectableCompanent, null);
					}
					StiElementActionHelper.CheckStatus(selectedComponentsWithChilds.ToList().LastOrDefault(), Designer);
				}
				InsertComponents = false;
				break;
			}
			case StiAction.SizeLeft:
			case StiAction.SizeRight:
			case StiAction.SizeTop:
			case StiAction.SizeBottom:
			case StiAction.SizeLeftTop:
			case StiAction.SizeLeftBottom:
			case StiAction.SizeRightTop:
			case StiAction.SizeRightBottom:
			{
				RectangleD offsetRectangle = base.CurrentPage.OffsetRectangle;
				Designer.LockAddSelectedToPropertyGrid = true;
				Action = StiAction.None;
				if (!SkipUndoRedoOperation)
				{
					Designer.UndoRedoSave(Loc.GetMain("Resize"), onlyCurrentPage: true);
				}
				SkipUndoRedoOperation = false;
				offsetRectangle = offsetRectangle.AlignToGrid(base.CurrentPage.GridSize, base.Info.AlignToGrid && !AltKeyMode);
				base.CurrentPage.OffsetRectangle = new RectangleD(0.0, 0.0, 0.0, 0.0);
				stiPageViewControl.Page.ChangePosition(offsetRectangle);
				if (IsCreatingComponent)
				{
					CheckSize(CreatedComponent);
					CreatedComponent.Dockable = true;
					Designer.Report.ResetSelection();
					CreatedComponent.Select();
					StiElementActionHelper.CheckStatus(CreatedComponent, Designer);
				}
				CheckPrimitives();
				stiPageViewControl.Page.Correct();
				CorrectScroll();
				break;
			}
			}
			if (base.CurrentPage is IStiForm)
			{
				Designer.CurrentPageView.SetPageViewLocation();
			}
			base.CurrentPage.Normalize();
			Designer.PageView.IsDrawSelection = false;
			((Control)Designer).Refresh();
			Designer.LockAddSelectedToPropertyGrid = false;
		}
		if (requereUpdateSelectedObjects)
		{
			Designer.InvokeSelectedObjectsChanged();
		}
		copyCheck = false;
		CheckSubreport(CreatedComponent);
		if (IsDragDropComponent)
		{
			((Control)Designer.CurrentPageView.View).Focus();
			IsDragDropComponent = false;
			StiComponentsCollection selectedComponents2 = base.CurrentPage.GetSelectedComponents();
			if (selectedComponents2.Count > 0)
			{
				if (selectedComponents2[0] is StiDataBand stiDataBand && (stiDataBand.DataSource != null || stiDataBand.BusinessObject != null) && StiOptions.Designer.AllowRunDataBandPlacementWizard)
				{
					StiDataBandPlacementWizardForm stiDataBandPlacementWizardForm = new StiDataBandPlacementWizardForm(stiDataBand, null);
					try
					{
						Designer.AddOwnedForm((Form)(object)stiDataBandPlacementWizardForm);
						((Form)stiDataBandPlacementWizardForm).ShowDialog();
						if (stiDataBandPlacementWizardForm.PlacementComponent == StiPlacementComponentWizard.Table)
						{
							base.CurrentPage.Components.Remove(stiDataBand);
							StiTable table = stiDataBandPlacementWizardForm.Table;
							base.CurrentPage.Components.Add(table);
							table.AutoSizeCells();
							StiOptions.Engine.GlobalEvents.InvokeCheckDataBandNameCreatedInWizard(new StiCheckDataBandNameCreatedInWizardArgs(table));
						}
						else
						{
							StiOptions.Engine.GlobalEvents.InvokeCheckDataBandNameCreatedInWizard(new StiCheckDataBandNameCreatedInWizardArgs(stiDataBand));
						}
					}
					finally
					{
						((IDisposable)stiDataBandPlacementWizardForm)?.Dispose();
					}
				}
				else if (flag)
				{
					Designer.ActivatePlacementComponentService(selectedComponents2[0], (selectedComponents2.Count > 1) ? selectedComponents2[1] : null);
				}
			}
		}
		if (IsCreatingComponent)
		{
			if (IsShiftMode)
			{
				Designer.CreateTool.ComponentForCreating = CreatedComponent;
				Designer.CurrentTool = Designer.CreateTool;
				Designer.CreateTool.Action = StiCreateAction.CreateComponent;
				Designer.CreateTool.Activate();
				Designer.CreateTool.ShiftMode = true;
			}
			if (base.Info.RunDesignerAfterInsert && !DragDropAction && !(CreatedComponent is IStiReportControl) && CreatedComponent.Parent != null)
			{
				StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, CreatedComponent.GetType());
				componentDesigner.FirstRun = true;
				componentDesigner.Design(CreatedComponent);
			}
			IsCreatingComponent = false;
			CreatedComponent = null;
			IsShiftMode = false;
			DragDropAction = false;
			Designer.AddSelectedToPropertyGrid();
		}
		Action = StiAction.None;
		if (TableHelper != null)
		{
			TableHelper.Dispose();
			TableHelper = null;
			base.Info.IsTableMode = false;
		}
	}

	public override bool OnDoubleClick(object sender, EventArgs e)
	{
		Action = StiAction.None;
		if (Designer.IsDashboardSelected)
		{
			StiPageViewControl stiPageViewControl = (StiPageViewControl)(object)((Control)(StiViewControl)sender).Parent;
			IEnumerable<StiComponent> enumerable = (from IStiTitleElement t in from c in stiPageViewControl.Page.GetSelectedComponents().ToList()
					where c is IStiTitleElement
					select c
				where t.Title != null && t.Title.Visible
				select t).Cast<StiComponent>();
			foreach (StiComponent item in enumerable)
			{
				RectangleD rect = item.GetDisplayRectangle();
				if (item is IStiMargin)
				{
					rect = StiMarginHelper.ApplyMargin(item as IStiElement, rect);
				}
				rect = StiBorderElementHelper.GetBorderContentRect(rect, item as IStiElement);
				RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, rect));
				IStiTitleElement stiTitleElement = item as IStiTitleElement;
				SizeD? sizeD = StiTitleMeasureHelper.MeasureTitle(stiTitleElement.Title);
				RectangleD rectangleD2 = new RectangleD(rectangleD.X, rectangleD.Y, rectangleD.Width - sizeD.Value.Height - (double)StiScale.I8, sizeD.Value.Height * item.Page.Zoom);
				StiCornerRadius stiCornerRadius = (item as IStiCornerRadius)?.CornerRadius;
				if (stiCornerRadius != null && (stiCornerRadius.TopLeft > 0f || stiCornerRadius.TopRight > 0f))
				{
					rectangleD2.X += (double)stiCornerRadius.TopLeft * item.Page.Zoom * StiScale.Factor;
					rectangleD2.Width -= (double)(stiCornerRadius.TopLeft + stiCornerRadius.TopRight) * item.Page.Zoom * StiScale.Factor;
				}
				Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
				if (rectangleD2.Contains(point.X, point.Y))
				{
					((StiViewControl)sender).OpenEditor(null);
					return true;
				}
			}
		}
		InsertComponents = false;
		return false;
	}

	internal void SetMoveOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD((double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale), 0.0, 0.0), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeLeftOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD((double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), 0.0, (double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), 0.0), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeRightOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		bool flag = false;
		double num = 10000000000000.0;
		foreach (StiBand item in Designer.SelectedObjects.Where((object c) => c is StiBand))
		{
			flag = true;
			num = Math.Min(num, item.Width);
		}
		RectangleD rect = new RectangleD(0.0, 0.0, (double)(position.X - startPos.X) / (base.CurrentPage.Zoom * scale), 0.0);
		RectangleD offsetRectangle = base.CurrentPage.Unit.ConvertFromHInches(rect).AlignToGrid(base.CurrentPage.GridSize, Grid.IsAlign(Designer) && !AltKeyMode);
		if (flag && offsetRectangle.Width < 0.0 && 0.0 - offsetRectangle.Width > num)
		{
			offsetRectangle.Width = 0.0 - num;
		}
		base.CurrentPage.OffsetRectangle = offsetRectangle;
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeTopOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD(0.0, (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale), 0.0, (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale)), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeBottomOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		bool flag = false;
		double num = 10000000000000.0;
		foreach (StiBand item in Designer.SelectedObjects.Where((object c) => c is StiBand))
		{
			flag = true;
			num = Math.Min(num, item.Height);
		}
		RectangleD rect = new RectangleD(0.0, 0.0, 0.0, (double)(position.Y - startPos.Y) / (base.CurrentPage.Zoom * scale));
		RectangleD offsetRectangle = base.CurrentPage.Unit.ConvertFromHInches(rect).AlignToGrid(base.CurrentPage.GridSize, Grid.IsAlign(Designer) && !AltKeyMode);
		if (flag && offsetRectangle.Height < 0.0 && 0.0 - offsetRectangle.Height > num)
		{
			offsetRectangle.Height = 0.0 - num;
		}
		base.CurrentPage.OffsetRectangle = offsetRectangle;
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeLeftTopOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD((double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale), (double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale)), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeLeftBottomOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD((double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), 0.0, (double)(startPos.X - position.X) / (base.CurrentPage.Zoom * scale), (double)(position.Y - startPos.Y) / (base.CurrentPage.Zoom * scale)), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeRightTopOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD(0.0, (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale), (double)(position.X - startPos.X) / (base.CurrentPage.Zoom * scale), (double)(startPos.Y - position.Y) / (base.CurrentPage.Zoom * scale)), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetSizeRightBottomOffsetRectangle()
	{
		Point position = Cursor.Position;
		Point startPos = Designer.PageView.StartPos;
		SetOffsetRectangle(rect: new RectangleD(0.0, 0.0, (double)(position.X - startPos.X) / (base.CurrentPage.Zoom * scale), (double)(position.Y - startPos.Y) / (base.CurrentPage.Zoom * scale)), page: base.CurrentPage);
		SetFormViewLocation(base.CurrentPage, Designer.PageView);
	}

	private void SetOffsetRectangle(StiPage page, RectangleD rect)
	{
		page.OffsetRectangle = page.Unit.ConvertFromHInches(rect).AlignToGrid(page.GridSize, Grid.IsAlign(Designer) && !AltKeyMode);
	}

	private static void SetFormViewLocation(StiPage page, StiPageView pageView)
	{
		if (page.IsForm)
		{
			pageView.GetCurrentPageView().SetPageViewLocation();
		}
	}

	private void ChangeSelectRowTable(double pageX, double pageY)
	{
		StiComponentsCollection components = base.CurrentPage.GetComponents();
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] is StiTable stiTable)
			{
				RectangleD displayRectangle = stiTable.GetDisplayRectangle();
				displayRectangle.X -= base.CurrentPage.GridSize * 2.0;
				if (displayRectangle.Contains(pageX, pageY))
				{
					SelectRowTable(stiTable, pageY);
					break;
				}
			}
		}
	}

	private void ChangeSelectColumnTable(double pageX, double pageY)
	{
		StiComponentsCollection components = base.CurrentPage.GetComponents();
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] is StiTable stiTable)
			{
				RectangleD displayRectangle = stiTable.GetDisplayRectangle();
				displayRectangle.Y -= base.CurrentPage.GridSize * 2.0;
				if (displayRectangle.Contains(pageX, pageY))
				{
					SelectColumnTable(stiTable, pageX);
					break;
				}
			}
		}
	}

	private void SelectRowTable(StiTable table, double y)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Invalid comparison between Unknown and I4
		List<object> list = new List<object>();
		if ((Control.ModifierKeys & 0x20000) == 0)
		{
			base.CurrentPage.ResetSelection();
		}
		else
		{
			for (int i = 0; i < Designer.SelectedObjects.Length; i++)
			{
				list.Add(Designer.SelectedObjects[i]);
			}
		}
		double num = (base.Info.ShowHeaders ? y : (y - base.CurrentPage.Unit.ConvertFromHInches(table.HeaderSize / (base.CurrentPage.Zoom * scale))));
		for (int j = 0; j < table.RowCount; j++)
		{
			StiComponent stiComponent = table.Components[j * table.ColumnCount];
			RectangleD displayRectangle = stiComponent.GetDisplayRectangle();
			if (!((num > displayRectangle.Top) & (num < displayRectangle.Bottom)))
			{
				continue;
			}
			if (!stiComponent.IsSelected)
			{
				int num2 = j * table.ColumnCount;
				for (int k = num2; k < num2 + table.ColumnCount; k++)
				{
					StiComponent stiComponent2 = table.Components[k];
					list.Add(stiComponent2);
					stiComponent2.Select();
				}
				Designer.SelectedObjects = list.ToArray();
				if ((Control.ModifierKeys & 0x20000) > 0)
				{
					((Control)Designer).Refresh();
				}
			}
			break;
		}
	}

	private void SelectColumnTable(StiTable table, double x)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		List<object> list = new List<object>();
		if ((Control.ModifierKeys & 0x20000) == 0)
		{
			base.CurrentPage.ResetSelection();
		}
		else
		{
			for (int i = 0; i < Designer.SelectedObjects.Length; i++)
			{
				list.Add(Designer.SelectedObjects[i]);
			}
		}
		for (int j = 0; j < table.ColumnCount; j++)
		{
			StiComponent stiComponent = table.Components[j];
			RectangleD displayRectangle = stiComponent.GetDisplayRectangle();
			if (!(x > displayRectangle.Left) || !(x < displayRectangle.Right))
			{
				continue;
			}
			if (!stiComponent.IsSelected)
			{
				for (int k = 0; k < table.RowCount; k++)
				{
					StiComponent stiComponent2 = table.Components[j + k * table.ColumnCount];
					list.Add(stiComponent2);
					stiComponent2.Select();
				}
				Designer.SelectedObjects = list.ToArray();
			}
			break;
		}
	}

	public static StiText GetTextInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				StiText textInControl = GetTextInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (textInControl != null)
				{
					return textInControl;
				}
			}
			if (stiComponent is StiText && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as StiText;
			}
		}
		return null;
	}

	public static StiImage GetImageInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				StiImage imageInControl = GetImageInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (imageInControl != null)
				{
					return imageInControl;
				}
			}
			if (stiComponent is StiImage && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as StiImage;
			}
		}
		return null;
	}

	public static IStiImageElement GetImageElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiImageElement imageElementInControl = GetImageElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (imageElementInControl != null)
				{
					return imageElementInControl;
				}
			}
			if (stiComponent is IStiImageElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiImageElement;
			}
		}
		return null;
	}

	public static IStiTextElement GetTextElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiTextElement textElementInControl = GetTextElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (textElementInControl != null)
				{
					return textElementInControl;
				}
			}
			if (stiComponent is IStiTextElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiTextElement;
			}
		}
		return null;
	}

	public static IStiTableElement GetTableElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiTableElement tableElementInControl = GetTableElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (tableElementInControl != null)
				{
					return tableElementInControl;
				}
			}
			if (stiComponent is IStiTableElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiTableElement;
			}
		}
		return null;
	}

	public static IStiPivotTableElement GetPivotElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiPivotTableElement pivotElementInControl = GetPivotElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (pivotElementInControl != null)
				{
					return pivotElementInControl;
				}
			}
			if (stiComponent is IStiPivotTableElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiPivotTableElement;
			}
		}
		return null;
	}

	public static IStiChartElement GetChartElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiChartElement chartElementInControl = GetChartElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (chartElementInControl != null)
				{
					return chartElementInControl;
				}
			}
			if (stiComponent is IStiChartElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiChartElement;
			}
		}
		return null;
	}

	public static IStiGaugeElement GetGaugeElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiGaugeElement gaugeElementInControl = GetGaugeElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (gaugeElementInControl != null)
				{
					return gaugeElementInControl;
				}
			}
			if (stiComponent is IStiGaugeElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiGaugeElement;
			}
		}
		return null;
	}

	public static IStiProgressElement GetProgressElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiProgressElement progressElementInControl = GetProgressElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (progressElementInControl != null)
				{
					return progressElementInControl;
				}
			}
			if (stiComponent is IStiProgressElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiProgressElement;
			}
		}
		return null;
	}

	public static IStiIndicatorElement GetIndicatorElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiIndicatorElement indicatorElementInControl = GetIndicatorElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (indicatorElementInControl != null)
				{
					return indicatorElementInControl;
				}
			}
			if (stiComponent is IStiIndicatorElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiIndicatorElement;
			}
		}
		return null;
	}

	public static IStiRegionMapElement GetRegionMapElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiRegionMapElement regionMapElementInControl = GetRegionMapElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (regionMapElementInControl != null)
				{
					return regionMapElementInControl;
				}
			}
			if (stiComponent is IStiRegionMapElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiRegionMapElement;
			}
		}
		return null;
	}

	public static IStiOnlineMapElement GetOnlineMapElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiOnlineMapElement onlineMapElementInControl = GetOnlineMapElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (onlineMapElementInControl != null)
				{
					return onlineMapElementInControl;
				}
			}
			if (stiComponent is IStiOnlineMapElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiOnlineMapElement;
			}
		}
		return null;
	}

	public static IStiListBoxElement GetListBoxElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiListBoxElement listBoxElementInControl = GetListBoxElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (listBoxElementInControl != null)
				{
					return listBoxElementInControl;
				}
			}
			if (stiComponent is IStiListBoxElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiListBoxElement;
			}
		}
		return null;
	}

	public static IStiComboBoxElement GetComboBoxElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiComboBoxElement comboBoxElementInControl = GetComboBoxElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (comboBoxElementInControl != null)
				{
					return comboBoxElementInControl;
				}
			}
			if (stiComponent is IStiComboBoxElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiComboBoxElement;
			}
		}
		return null;
	}

	public static IStiTreeViewElement GetTreeViewElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiTreeViewElement treeViewElementInControl = GetTreeViewElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (treeViewElementInControl != null)
				{
					return treeViewElementInControl;
				}
			}
			if (stiComponent is IStiTreeViewElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiTreeViewElement;
			}
		}
		return null;
	}

	public static IStiTreeViewBoxElement GetTreeViewBoxElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiTreeViewBoxElement treeViewBoxElementInControl = GetTreeViewBoxElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (treeViewBoxElementInControl != null)
				{
					return treeViewBoxElementInControl;
				}
			}
			if (stiComponent is IStiTreeViewBoxElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiTreeViewBoxElement;
			}
		}
		return null;
	}

	public static IStiDatePickerElement GetDatePickerElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiDatePickerElement datePickerElementInControl = GetDatePickerElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (datePickerElementInControl != null)
				{
					return datePickerElementInControl;
				}
			}
			if (stiComponent is IStiDatePickerElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiDatePickerElement;
			}
		}
		return null;
	}

	public static IStiElement GetElementInControl(StiContainer container, double x, double y)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent.IsSelected)
			{
				continue;
			}
			if (stiComponent is StiContainer stiContainer)
			{
				IStiElement elementInControl = GetElementInControl(stiContainer, x - stiContainer.Left, y - stiContainer.Top);
				if (elementInControl != null)
				{
					return elementInControl;
				}
			}
			if (stiComponent is IStiElement && stiComponent.ClientRectangle.Contains(x, y))
			{
				return stiComponent as IStiElement;
			}
		}
		return null;
	}

	internal void ResetHighlightState()
	{
		ResetHighlightState(base.CurrentPage);
	}

	private void ResetHighlightState(StiContainer container)
	{
		foreach (StiComponent component in container.Components)
		{
			if (component is StiContainer container2)
			{
				ResetHighlightState(container2);
			}
			component.HighlightState = StiHighlightState.Hide;
		}
	}

	private StiText GetHighlightedText()
	{
		return GetHighlightedText(base.CurrentPage);
	}

	private StiText GetHighlightedText(StiContainer container)
	{
		int num = container.Components.Count - 1;
		while (num >= 0)
		{
			StiComponent stiComponent = container.Components[num];
			num--;
			if (stiComponent is StiContainer container2)
			{
				StiText highlightedText = GetHighlightedText(container2);
				if (highlightedText != null)
				{
					return highlightedText;
				}
			}
			if (stiComponent.HighlightState == StiHighlightState.Active)
			{
				return stiComponent as StiText;
			}
		}
		return null;
	}

	private StiAction GetTableAction(double pageX, double pageY)
	{
		RectangleD selectedRectangle = StiTableHelper.GetSelectedRectangle(Designer);
		double num = base.CurrentPage.Unit.ConvertFromHInches(4.0);
		StiAction stiAction = StiActionUtils.PointInRect(num, pageX, pageY, selectedRectangle, selectedRectangle, isSelected: true, locked: false);
		if (stiAction == StiAction.None || stiAction == StiAction.Move || stiAction == StiAction.Select)
		{
			Hashtable xx = new Hashtable();
			Hashtable yy = new Hashtable();
			StiTableHelper.GetSelectedLines(Report.Designer, ref xx, ref yy, isPaint: false);
			if (selectedRectangle.Y <= pageY && selectedRectangle.Bottom >= pageY)
			{
				foreach (double key in xx.Keys)
				{
					if (!(key - num < pageX) || !(key + num > pageX))
					{
						continue;
					}
					if (!Report.IsPageDesigner)
					{
						object[] selected = Report.GetSelected();
						for (int i = 0; i < selected.Length; i++)
						{
							if (selected[i] is StiTable || selected[i] is IStiTableCell)
							{
								return StiAction.None;
							}
						}
					}
					return StiAction.ResizeColumns;
				}
			}
			if (selectedRectangle.X <= pageX && selectedRectangle.Right >= pageX)
			{
				foreach (double key2 in yy.Keys)
				{
					if (!(key2 - num < pageY) || !(key2 + num > pageY))
					{
						continue;
					}
					if (!Report.IsPageDesigner)
					{
						object[] selected2 = Report.GetSelected();
						for (int j = 0; j < selected2.Length; j++)
						{
							if (selected2[j] is StiTable || selected2[j] is IStiTableCell)
							{
								return StiAction.None;
							}
						}
					}
					return StiAction.ResizeRows;
				}
			}
		}
		return stiAction;
	}

	private RectangleD ConvertRectangle(RectangleD rect)
	{
		return rect.AlignToGrid(base.CurrentPage.GridSize, base.Info.AlignToGrid && !AltKeyMode).Normalize();
	}

	private void CheckPrimitives()
	{
		if (!IsDragDropComponent && !IsCreatingComponent && !InsertComponents)
		{
			return;
		}
		StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
		foreach (StiComponent item in selectedComponents)
		{
			if (item is StiCrossLinePrimitive stiCrossLinePrimitive && StiRestrictionsHelper.IsAllowSelect(item))
			{
				RectangleD clientRectangle = stiCrossLinePrimitive.ClientRectangle;
				StiStartPointPrimitive stiStartPointPrimitive = new StiStartPointPrimitive
				{
					Left = clientRectangle.Left,
					Top = clientRectangle.Top,
					Width = 0.0,
					Height = 0.0
				};
				base.CurrentPage.Components.Add(stiStartPointPrimitive);
				StiEndPointPrimitive stiEndPointPrimitive = new StiEndPointPrimitive();
				if (stiCrossLinePrimitive is StiRectanglePrimitive)
				{
					stiEndPointPrimitive.Left = clientRectangle.Right;
				}
				else
				{
					stiEndPointPrimitive.Left = clientRectangle.Left;
				}
				stiEndPointPrimitive.Top = clientRectangle.Bottom;
				stiEndPointPrimitive.Width = 0.0;
				stiEndPointPrimitive.Height = 0.0;
				base.CurrentPage.Components.Add(stiEndPointPrimitive);
				stiStartPointPrimitive.ReferenceToGuid = stiCrossLinePrimitive.Guid;
				stiEndPointPrimitive.ReferenceToGuid = stiCrossLinePrimitive.Guid;
			}
		}
	}

	private void CheckSubreport(StiComponent component)
	{
		if (IsDragDropComponent || IsCreatingComponent)
		{
			StiSubReport stiSubReport = component as StiSubReport;
			if (stiSubReport == null)
			{
				stiSubReport = base.CurrentPage.GetFirstSelectableCompanent() as StiSubReport;
			}
			if (stiSubReport != null && string.IsNullOrEmpty(stiSubReport.SubReportUrl))
			{
				StiPage stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { Designer.Report }) as StiPage;
				stiPage.Skip = true;
				Designer.Report.Pages.Add(stiPage);
				stiSubReport.SubReportPage = stiPage;
				stiPage.Name = StiNameCreation.CreateName(Designer.Report, StiNameCreation.GenerateName(Designer.Report, stiSubReport.LocalizedName + "_", "subReport_"));
				((Control)Designer).Refresh();
			}
		}
	}

	internal void CheckSize(StiComponent component)
	{
		if (component.Width == 0.0)
		{
			if (component is IStiReportControl)
			{
				component.Width = StiAlignValue.AlignToMaxGrid(base.CurrentPage.Unit.ConvertFromHInches(component.DefaultClientRectangle.Width), base.CurrentPage.GridSize, base.Info.AlignToGrid);
			}
			else
			{
				component.Width = StiAlignValue.AlignToMaxGrid(base.CurrentPage.Unit.ConvertFromHInches(component.DefaultClientRectangle.Width), base.CurrentPage.GridSize, base.Info.AlignToGrid);
			}
		}
		if (component.Height == 0.0)
		{
			if (component is IStiReportControl)
			{
				component.Height = StiAlignValue.AlignToMaxGrid(base.CurrentPage.Unit.ConvertFromHInches(component.DefaultClientRectangle.Height), base.CurrentPage.GridSize, base.Info.AlignToGrid);
			}
			else
			{
				component.Height = StiAlignValue.AlignToMaxGrid(base.CurrentPage.Unit.ConvertFromHInches(component.DefaultClientRectangle.Height), base.CurrentPage.GridSize, base.Info.AlignToGrid);
			}
		}
	}

	private void CorrectScroll()
	{
		if (base.CurrentPage is IStiForm)
		{
			Designer.CurrentPageView.ScrollLeft = 0;
			((Control)Designer.CurrentPageView.View).Left = StiScale.I10;
			((ScrollBar)Designer.CurrentPageView.HScrollBar).Value = 0;
			Designer.CurrentPageView.ScrollTop = 0;
			((Control)Designer.CurrentPageView.View).Top = StiScale.I10;
			((ScrollBar)Designer.CurrentPageView.VScrollBar).Value = 0;
		}
	}

	private void Redraw(bool move)
	{
		StiComponent dockableComponent = base.CurrentPage.GetDockableComponent();
		bool flag = false;
		StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
		foreach (StiComponent item in selectedComponents)
		{
			if (item.ForceRedrawAll)
			{
				flag = true;
				break;
			}
		}
		RedrawAll = base.CurrentPage is IStiForm;
		if ((dockableComponent != null && !move) || RedrawAll || flag)
		{
			Designer.Builder.Redraw();
			RedrawAll = false;
			return;
		}
		RectangleD rectangle = base.CurrentPage.GetSelectedRectangleWithChilds();
		if (TableHelperV2 != null && TableHelperV2.SelectedCell is StiComponent { Parent: not null } stiComponent2)
		{
			rectangle = stiComponent2.Parent.ClientRectangle;
		}
		if (IsDragDropComponent && Designer.PageView.DraggingComponent is StiText)
		{
			LatestDragingComponent = Designer.PageView.DraggingComponent as StiText;
			ResetHighlightState();
			StiText textInControl = GetTextInControl(base.CurrentPage, rectangle.X, rectangle.Y);
			if (textInControl != null)
			{
				textInControl.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(textInControl.GetDisplayRectangle());
			}
		}
		if (IsDragDropComponent && Designer.PageView.DraggingComponent is StiImage)
		{
			LatestDragingComponent = Designer.PageView.DraggingComponent as StiImage;
			ResetHighlightState();
			StiImage imageInControl = GetImageInControl(base.CurrentPage, rectangle.X, rectangle.Y);
			if (imageInControl != null)
			{
				imageInControl.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(imageInControl.GetDisplayRectangle());
			}
		}
		bool flag2 = false;
		bool flag3 = Designer.PageView.DraggingComponent is IStiAltProperties { AltClientRectangle: var altClientRectangle } && !altClientRectangle.IsEmpty;
		if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer) && !flag3)
		{
			rectangle = Designer.PageView.DraggingComponent.GetDisplayRectangle();
			StiComponent stiComponent3 = GetTableElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent4 = GetPivotElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent5 = GetChartElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent6 = GetGaugeElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent7 = GetProgressElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent8 = GetIndicatorElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent9 = GetRegionMapElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent10 = GetOnlineMapElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent11 = GetListBoxElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent12 = GetComboBoxElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent13 = GetTreeViewElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent14 = GetTreeViewBoxElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			StiComponent stiComponent15 = GetDatePickerElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) as StiComponent;
			if (stiComponent3 != null)
			{
				ResetHighlightState();
				stiComponent3.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent3.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent4 != null)
			{
				ResetHighlightState();
				stiComponent4.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent4.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent5 != null)
			{
				ResetHighlightState();
				stiComponent5.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent5.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent6 != null)
			{
				ResetHighlightState();
				stiComponent6.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent6.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent7 != null)
			{
				ResetHighlightState();
				stiComponent7.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent7.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent8 != null)
			{
				ResetHighlightState();
				stiComponent8.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent8.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent9 != null)
			{
				ResetHighlightState();
				stiComponent9.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent9.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent10 != null)
			{
				ResetHighlightState();
				stiComponent10.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent10.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent11 != null)
			{
				ResetHighlightState();
				stiComponent11.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent11.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent12 != null)
			{
				ResetHighlightState();
				stiComponent12.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent12.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent13 != null)
			{
				ResetHighlightState();
				stiComponent13.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent13.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent14 != null)
			{
				ResetHighlightState();
				stiComponent14.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent14.GetDisplayRectangle());
				flag2 = true;
			}
			else if (stiComponent15 != null)
			{
				ResetHighlightState();
				stiComponent15.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent15.GetDisplayRectangle());
				flag2 = true;
			}
		}
		if (IsDragDropComponent && Designer.PageView.DraggingComponent is IStiTableElement && !flag2)
		{
			LatestDragingComponent = Designer.PageView.DraggingComponent;
			ResetHighlightState();
			if (GetTableElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) is StiComponent stiComponent16)
			{
				stiComponent16.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent16.GetDisplayRectangle());
				flag2 = true;
			}
		}
		if (IsDragDropComponent && Designer.PageView.DraggingComponent is IStiImageElement && !flag2)
		{
			LatestDragingComponent = Designer.PageView.DraggingComponent;
			ResetHighlightState();
			if (GetImageElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) is StiComponent stiComponent17)
			{
				stiComponent17.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent17.GetDisplayRectangle());
				flag2 = true;
			}
		}
		if (IsDragDropComponent && Designer.PageView.DraggingComponent is IStiTextElement && !flag2)
		{
			LatestDragingComponent = Designer.PageView.DraggingComponent;
			ResetHighlightState();
			if (GetTextElementInControl(base.CurrentPage, rectangle.X, rectangle.Y) is StiComponent stiComponent18)
			{
				stiComponent18.HighlightState = StiHighlightState.Active;
				rectangle = rectangle.FitToRectangle(stiComponent18.GetDisplayRectangle());
			}
		}
		Rectangle rectangle2 = StiConvert.RectangleToScreen(Designer, rectangle);
		bool flag4 = selectedComponents.ToList().Any((StiComponent c) => c is IStiSimpleShadow { Shadow: not null } stiSimpleShadow && stiSimpleShadow.Shadow.Visible);
		int num = (flag4 ? 8 : 4);
		int num2 = (flag4 ? 20 : 10);
		rectangle2.X -= (int)Math.Ceiling((double)num * scale);
		rectangle2.Y -= (int)Math.Ceiling((double)num * scale);
		rectangle2.Width += (int)Math.Ceiling((double)num2 * scale);
		rectangle2.Height += (int)Math.Ceiling((double)num2 * scale);
		StiUnit unit = base.CurrentPage.Unit;
		double gridSize = base.CurrentPage.GridSize;
		gridSize = Math.Max(gridSize, unit.ConvertFromHInches(20.0));
		gridSize = Math.Min(gridSize, unit.ConvertFromHInches(20.0));
		gridSize = Math.Max(gridSize, unit.ConvertFromHInches(6.0));
		if (base.Info.ShowDimensionLines)
		{
			int num3 = (int)Math.Round(Report.Unit.ConvertToHInches(gridSize * 2.0 * base.Info.Zoom * scale) + 2.0 * base.Info.Zoom * scale, 0);
			switch (Action)
			{
			case StiAction.SizeBottom:
				rectangle2.Width += num3;
				break;
			case StiAction.SizeLeft:
				rectangle2.Height += num3;
				break;
			case StiAction.SizeLeftBottom:
				rectangle2.X -= num3;
				rectangle2.Width += num3;
				rectangle2.Height += num3;
				break;
			case StiAction.SizeLeftTop:
				rectangle2.X -= num3;
				rectangle2.Y -= num3;
				rectangle2.Width += num3;
				rectangle2.Height += num3;
				break;
			case StiAction.SizeRight:
				rectangle2.Height += num3;
				break;
			case StiAction.SizeRightBottom:
				rectangle2.Width += num3;
				rectangle2.Height += num3;
				break;
			case StiAction.SizeRightTop:
				rectangle2.Y -= num3;
				rectangle2.Width += num3;
				rectangle2.Height += num3;
				break;
			case StiAction.SizeTop:
				rectangle2.Width += num3;
				break;
			}
		}
		if (StiDimensionLinesHelper.IsAllowLocationDimensionLine(base.Info.CurrentAction))
		{
			rectangle2.X -= (int)Math.Ceiling(50.0 * scale);
			rectangle2.Width += (int)Math.Ceiling(50.0 * scale);
			rectangle2.Y -= (int)Math.Ceiling(16.0 * scale);
			rectangle2.Height += (int)Math.Ceiling(16.0 * scale);
		}
		if (Designer.IsDashboardSelected)
		{
			rectangle2.X -= StiScale.XXI(StiInplaceEditor.ButtonSize + StiInplaceEditor.ButtonMargin);
			rectangle2.Width += StiScale.XXI(StiInplaceEditor.ButtonSize + StiInplaceEditor.ButtonMargin) * 2;
		}
		Rectangle rectangle3 = rectangle2;
		if (oldRect == Rectangle.Empty)
		{
			Designer.Builder.Redraw();
			RedrawAll = false;
			oldRect = rectangle3;
		}
		else
		{
			RedrawAll = false;
			rectangle2 = StiRectangleUtils.FitToRectangle(rectangle2, oldRect);
			oldRect = rectangle3;
			Designer.Builder.Redraw(rectangle2);
		}
	}

	public static StiSelectToolService GetSelectToolService()
	{
		return GetSelectToolService(null);
	}

	public static StiSelectToolService GetSelectToolService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Tools.FirstOrDefault((StiToolService c) => c.GetType() == typeof(StiSelectToolService)) as StiSelectToolService;
		}
		return designer.Services.GetServices(typeof(StiToolService)).OfType<StiSelectToolService>().FirstOrDefault();
	}

	public RectangleD GetActionRectangle()
	{
		StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
		if (selectedComponents.Count > 0)
		{
			return StiTableHelper.GetSelectedRectangle(Designer, isPaintRect: false, selectedComponents);
		}
		if (Action == StiAction.Select)
		{
			return base.CurrentPage.SelectedRectangle;
		}
		return RectangleD.Empty;
	}

	public override string GetStatusBarInformation()
	{
		if (Designer != null && Designer.SpecialStatusBarInformation != null)
		{
			return Designer.SpecialStatusBarInformation;
		}
		StiComponentsCollection selectedComponents = base.CurrentPage.GetSelectedComponents();
		RectangleD rectangleD = RectangleD.Empty;
		foreach (StiComponent item in selectedComponents)
		{
			RectangleD clientRectangle = item.ClientRectangle;
			clientRectangle = StiComponent.DoOffsetRect(item, clientRectangle, item.Page.OffsetRectangle);
			rectangleD = (rectangleD.IsEmpty ? clientRectangle : rectangleD.FitToRectangle(clientRectangle));
		}
		if (rectangleD.IsEmpty)
		{
			return base.GetStatusBarInformation();
		}
		return $"X:{rectangleD.X:F}  " + $"Y:{rectangleD.Y:F}  " + string.Format("{0}:{1:F}  ", Loc.GetMain("Width"), rectangleD.Width) + string.Format("{0}:{1:F}", Loc.GetMain("Height"), rectangleD.Height);
	}

	public override Cursor GetCursor()
	{
		if (!IsCreatingComponent)
		{
			return GetCursor(Action);
		}
		return StiCreateToolService.CursorPen;
	}

	public Cursor GetCursor(StiAction action)
	{
		switch (action)
		{
		case StiAction.None:
			return Cursors.Arrow;
		case StiAction.SizeLeft:
		case StiAction.SizeRight:
			return Cursors.SizeWE;
		case StiAction.SizeTop:
		case StiAction.SizeBottom:
			return Cursors.SizeNS;
		case StiAction.SizeLeftTop:
		case StiAction.SizeRightBottom:
			return Cursors.SizeNWSE;
		case StiAction.SizeLeftBottom:
		case StiAction.SizeRightTop:
			return Cursors.SizeNESW;
		case StiAction.ResizeColumns:
			return ResizeColumnsCursor;
		case StiAction.ResizeRows:
			return ResizeRowsCursor;
		default:
			return Cursors.Default;
		}
	}

	public void CheckMouseOverComponents(MouseEventArgs e)
	{
		StiComponentsCollection components = base.CurrentPage.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiTable)
			{
				StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, item.GetType());
				StiAction stiAction = componentDesigner.PointInRect(Designer, item, e.X, e.Y);
				if (stiAction == StiAction.SelectColumn || stiAction == StiAction.SelectRow)
				{
					Action = stiAction;
					return;
				}
			}
			if (!item.InvalidateOnMouseOver)
			{
				continue;
			}
			StiComponentDesigner componentDesigner2 = StiComponentDesigner.GetComponentDesigner(Designer, item.GetType());
			if (componentDesigner2.PointInRect(Designer, item, e.X, e.Y) != 0)
			{
				Rectangle clipRect = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, item.GetDisplayRectangle())).ToRectangle();
				clipRect.Inflate(6, 6);
				item.HighlightState = StiHighlightState.Show;
				Designer.Builder.Redraw(clipRect);
				continue;
			}
			RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, item.GetDisplayRectangle()));
			if (item.HighlightState != 0)
			{
				item.HighlightState = StiHighlightState.Hide;
				Rectangle clipRect2 = rectangleD.ToRectangle();
				clipRect2.Inflate(6, 6);
				Designer.Builder.Redraw(clipRect2);
			}
		}
		if (action == StiAction.SelectColumn || action == StiAction.SelectRow)
		{
			Action = StiAction.None;
		}
	}

	public void CheckInplaceEditorTitle()
	{
		if (!base.CurrentPage.IsDashboardOrScreen || base.Info.IsComponentsMoving)
		{
			return;
		}
		IEnumerable<StiComponent> enumerable = from c in base.CurrentPage.GetComponents().ToList()
			where c is IStiButtonElement || c is IStiComponentUI || (c.IsSelected && c is IStiElement)
			select c;
		Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
		foreach (StiComponent item in enumerable)
		{
			RectangleD rectangle = StiMarginHelper.ApplyMargin(item as IStiElement, item.GetDisplayRectangle());
			RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, rectangle));
			RectangleD rectangleD2 = StiInplaceEditorTitleElement.GetTitleRect(rectangleD, item);
			RectangleD rectangleD3 = StiInplaceEditorTitleElement.GetSortRect(rectangleD, item);
			bool flag = StiSortMenuHelper.IsAllowUserSorting(item as IStiElement);
			bool flag2 = StiUserViewStatesMenuHelper.IsAllowUserViewStatesChanging(item as IStiElement);
			if (rectangleD.Contains(point.X, point.Y))
			{
				if (!StiInplaceEditorTitleElement.IsComponentFocused(item))
				{
					StiInplaceEditorTitleElement.SetComponentFocused(item);
					if (!rectangleD2.IsEmpty)
					{
						rectangleD2 = rectangleD2.FitToRectangle(rectangleD);
						rectangleD2.Inflate(2.0, 2.0);
						Designer.Builder.Redraw(rectangleD2.ToRectangle());
					}
					if (flag && !rectangleD3.IsEmpty)
					{
						rectangleD3 = rectangleD3.FitToRectangle(rectangleD);
						rectangleD3.Inflate(2.0, 2.0);
						Designer.Builder.Redraw(rectangleD3.ToRectangle());
					}
				}
			}
			else if (StiInplaceEditorTitleElement.IsComponentFocused(item))
			{
				StiInplaceEditorTitleElement.ResetComponentFocused(item);
				if (!rectangleD2.IsEmpty)
				{
					rectangleD2 = rectangleD2.FitToRectangle(rectangleD);
					rectangleD2.Inflate(2.0, 2.0);
					Designer.Builder.Redraw(rectangleD2.ToRectangle());
				}
				if (flag && !rectangleD3.IsEmpty)
				{
					rectangleD3 = rectangleD3.FitToRectangle(rectangleD);
					rectangleD3.Inflate(2.0, 2.0);
					Designer.Builder.Redraw(rectangleD3.ToRectangle());
				}
			}
			if (rectangleD2.Contains(point.X, point.Y))
			{
				if (!StiInplaceEditorTitleElement.IsTitleFocused(item))
				{
					StiInplaceEditorTitleElement.SetTitleFocused(item);
					rectangleD2 = rectangleD2.FitToRectangle(rectangleD);
					rectangleD2.Inflate(2.0, 2.0);
					Designer.Builder.Redraw(rectangleD2.ToRectangle());
				}
			}
			else if (StiInplaceEditorTitleElement.IsTitleFocused(item))
			{
				StiInplaceEditorTitleElement.ResetTitleFocused(item);
				rectangleD2 = rectangleD2.FitToRectangle(rectangleD);
				rectangleD2.Inflate(2.0, 2.0);
				Designer.Builder.Redraw(rectangleD2.ToRectangle());
			}
			if (flag)
			{
				if (rectangleD3.Contains(point.X, point.Y))
				{
					if (!StiInplaceEditorTitleElement.IsSortFocused(item))
					{
						StiInplaceEditorTitleElement.SetSortFocused(item);
						rectangleD3 = rectangleD3.FitToRectangle(rectangleD);
						rectangleD3.Inflate(2.0, 2.0);
						Designer.Builder.Redraw(rectangleD3.ToRectangle());
					}
				}
				else if (StiInplaceEditorTitleElement.IsSortFocused(item))
				{
					StiInplaceEditorTitleElement.ResetSortFocused(item);
					rectangleD3 = rectangleD3.FitToRectangle(rectangleD);
					rectangleD3.Inflate(2.0, 2.0);
					Designer.Builder.Redraw(rectangleD3.ToRectangle());
				}
			}
			if (!flag2)
			{
				continue;
			}
			IStiUserViewStates stiUserViewStates = item as IStiUserViewStates;
			int num = 0;
			foreach (StiUserViewState userViewState in stiUserViewStates.UserViewStates)
			{
				RectangleD viewStateRect = StiInplaceEditorTitleElement.GetViewStateRect(rectangleD, item, num);
				if (viewStateRect.IsEmpty)
				{
					continue;
				}
				if (viewStateRect.Contains(point.X, point.Y))
				{
					if (!StiInplaceEditorTitleElement.IsViewStateFocused(item, num))
					{
						StiInplaceEditorTitleElement.SetViewStateFocused(item, num);
						viewStateRect = viewStateRect.FitToRectangle(rectangleD);
						viewStateRect.Inflate(2.0, 2.0);
						Designer.Builder.Redraw(viewStateRect.ToRectangle());
					}
				}
				else if (StiInplaceEditorTitleElement.IsViewStateFocused(item, num))
				{
					StiInplaceEditorTitleElement.ResetViewStateFocused(item);
					viewStateRect = viewStateRect.FitToRectangle(rectangleD);
					viewStateRect.Inflate(2.0, 2.0);
					Designer.Builder.Redraw(viewStateRect.ToRectangle());
				}
				num++;
			}
		}
	}

	public void CheckQuickButtons()
	{
		StiComponentsCollection components = base.CurrentPage.GetComponents();
		Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
		foreach (StiComponent item in components)
		{
			StiQuickButton[] quickButtons = Designer.GetQuickButtons(item);
			if (quickButtons == null || quickButtons.Length == 0 || !StiRestrictionsHelper.IsAllowChange(item))
			{
				continue;
			}
			RectangleD rect = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(Designer, item.GetDisplayRectangle()));
			if (rect.Contains(point.X, point.Y))
			{
				if (!item.ShowQuickButtons)
				{
					item.ShowQuickButtons = true;
					Rectangle clipRect = rect.ToRectangle();
					clipRect.X -= 2;
					clipRect.Y -= 2;
					clipRect.Width += 4;
					clipRect.Height += 4;
					Designer.Builder.Redraw(clipRect);
				}
			}
			else if (item.ShowQuickButtons)
			{
				item.ShowQuickButtons = false;
				Rectangle clipRect2 = rect.ToRectangle();
				clipRect2.X -= 2;
				clipRect2.Y -= 2;
				clipRect2.Width += 4;
				clipRect2.Height += 4;
				Designer.Builder.Redraw(clipRect2);
			}
			if (item is StiBand)
			{
				rect.Height = ((StiBand)item).HeaderSize * base.CurrentPage.Zoom * scale;
			}
			if (!StiQuickButtonHelper.CheckVisibleQuickButtons(rect))
			{
				continue;
			}
			int num = 0;
			StiQuickButton[] array = quickButtons;
			foreach (StiQuickButton stiQuickButton in array)
			{
				RectangleD quickButtonRect = StiQuickButtonHelper.GetQuickButtonRect(quickButtons, num, rect);
				if (quickButtonRect.Contains(point.X, point.Y))
				{
					if (!stiQuickButton.IsSelected)
					{
						stiQuickButton.IsSelected = true;
						Rectangle clipRect3 = quickButtonRect.ToRectangle();
						clipRect3.X -= 2;
						clipRect3.Y -= 2;
						clipRect3.Width += 4;
						clipRect3.Height += 4;
						Designer.Builder.Redraw(clipRect3);
					}
				}
				else if (stiQuickButton.IsSelected)
				{
					stiQuickButton.IsSelected = false;
					Rectangle clipRect4 = quickButtonRect.ToRectangle();
					clipRect4.X -= 2;
					clipRect4.Y -= 2;
					clipRect4.Width += 4;
					clipRect4.Height += 4;
					Designer.Builder.Redraw(clipRect4);
				}
				num++;
			}
		}
	}

	public void ResetQuickButtons()
	{
		StiComponentsCollection components = base.CurrentPage.GetComponents();
		foreach (StiComponent item in components)
		{
			StiQuickButton[] quickButtons = Designer.GetQuickButtons(item);
			if (quickButtons.Length != 0)
			{
				StiQuickButton[] array = quickButtons;
				foreach (StiQuickButton stiQuickButton in array)
				{
					stiQuickButton.IsSelected = false;
				}
			}
		}
	}
}
