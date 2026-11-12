using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Design.Tools;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiViewControl : Control
{
	private bool cancelDragDrop;

	private bool keyProcessed;

	private bool lockShowkeyTips;

	private StiDesignerControl Designer => (((Control)this).Parent as StiPageViewControl)?.Designer;

	public StiComponent DraggingComponent
	{
		get
		{
			return Designer.PageView.DraggingComponent;
		}
		set
		{
			if (Designer != null)
			{
				Designer.PageView.DraggingComponent = value;
			}
		}
	}

	public StiComponent DraggingLabelComponent
	{
		get
		{
			return Designer.PageView.DraggingLabelComponent;
		}
		set
		{
			if (Designer != null)
			{
				Designer.PageView.DraggingLabelComponent = value;
			}
		}
	}

	private StiPageViewControl PageViewControl => ((Control)this).Parent as StiPageViewControl;

	public void OpenEditor(string str, bool isKeystroke = false)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (Designer.SelectedComponentsOnPage.Count <= 0)
		{
			return;
		}
		StiTextEditorToolService service = StiTextEditorToolService.GetService(Designer);
		if (service == null)
		{
			string text = string.Format(Loc.Get("Errors", "ServiceNotFound"), "StiTextEditorToolService");
			StiLogService.Write(((object)this).GetType(), text);
			MessageBox.Show(text);
			return;
		}
		Designer.UndoRedoSave(Loc.Get("Toolbox", "TextEditor"), onlyCurrentPage: true);
		foreach (StiComponent item in Designer.SelectedComponentsOnPage)
		{
			if (item is IStiText)
			{
				service.StartTextEditor(item);
				if (str != null)
				{
					service.EditorTextBox.Text = str;
					if (service.EditorTextBox is TextBox)
					{
						((TextBoxBase)(TextBox)service.EditorTextBox).SelectionStart = str.Length;
					}
					else
					{
						((TextBoxBase)(RichTextBox)service.EditorTextBox).SelectionStart = str.Length;
					}
					service.AllowCursorKeys = true;
				}
				break;
			}
			if (item is IStiTitleElement)
			{
				service.StartTextEditor(item);
			}
			if (item is IStiButtonElement)
			{
				service.StartTextEditor(item, isKeystroke);
			}
		}
	}

	public StiComponentDesigner GetComponentDesigner(out StiComponent component)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		component = PageViewControl.Page.GetComponentInControl(Designer.XToPage(point.X), Designer.YToPage(point.Y), usePage: true, StiGuiMode.Gdi, forGetDesigner: true);
		if (component != null)
		{
			return StiComponentDesigner.GetComponentDesigner(Designer, component.GetType());
		}
		component = null;
		return null;
	}

	public void DisposeRes()
	{
	}

	internal void Draw(Rectangle clipRect)
	{
		Draw(null, clipRect);
	}

	internal void Draw(Graphics g, Rectangle clipRect)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		StiPageViewControl stiPageViewControl = ((Control)this).Parent as StiPageViewControl;
		if (stiPageViewControl.Page == null)
		{
			return;
		}
		Graphics val = g;
		if (val == null)
		{
			val = ((Control)this).CreateGraphics();
		}
		if (clipRect.IsEmpty)
		{
			clipRect = ((Control)this).ClientRectangle;
		}
		GraphicsState val2 = val.Save();
		((StiComponent)stiPageViewControl.Page).Paint(new StiPaintEventArgs(val, RectangleD.CreateFromRectangle(clipRect))
		{
			IsPaintWinFormsViewer = true
		});
		val.Restore(val2);
		if (stiPageViewControl.Page != null && !stiPageViewControl.Page.IsDashboard)
		{
			Pen val3 = new Pen(Color.FromArgb(255, 119, 119, 119));
			try
			{
				val.DrawRectangle(val3, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		val.ResetTransform();
		val.ResetClip();
		if (g == null)
		{
			val.Dispose();
		}
	}

	private void SetFontStyleToPage(FontStyle styleForSet)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		StiComponentsCollection selectedComponents = Designer.Report.GetCurrentPage().GetSelectedComponents();
		foreach (StiComponent item in selectedComponents)
		{
			if (!item.Inherited && item is IStiFont stiFont)
			{
				FontStyle fontStyleFromFont = GetFontStyleFromFont(stiFont.Font);
				fontStyleFromFont = (FontStyle)(((fontStyleFromFont & styleForSet) <= 0) ? (fontStyleFromFont | styleForSet) : (fontStyleFromFont - styleForSet));
				stiFont.Font = new Font(stiFont.Font.FontFamily, stiFont.Font.Size, fontStyleFromFont, stiFont.Font.Unit, stiFont.Font.GdiCharSet, stiFont.Font.GdiVerticalFont);
			}
		}
		((Control)Designer).Refresh();
	}

	private void SetFontSiseToPage(float size)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (!(size > 1f))
		{
			return;
		}
		StiComponentsCollection selectedComponents = Designer.Report.GetCurrentPage().GetSelectedComponents();
		foreach (StiComponent item in selectedComponents)
		{
			if (!item.Inherited && item is IStiFont stiFont)
			{
				stiFont.Font = new Font(stiFont.Font.Name, size, GetFontStyleFromFont(stiFont.Font), stiFont.Font.Unit, stiFont.Font.GdiCharSet, stiFont.Font.GdiVerticalFont);
			}
		}
		((Control)Designer).Refresh();
	}

	private FontStyle GetFontStyleFromFont(Font font)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (font.Bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (font.Italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (font.Underline)
		{
			val = (FontStyle)(val | 4);
		}
		if (font.Strikeout)
		{
			val = (FontStyle)(val | 8);
		}
		return val;
	}

	private void LeaveDragDrop()
	{
		if (DraggingComponent != null && DraggingComponent.Parent != null)
		{
			DraggingComponent.Parent.Components.Remove(DraggingComponent);
			((Control)Designer.PageView.GetCurrentPageView().View).Invalidate();
		}
		if (DraggingLabelComponent != null && DraggingLabelComponent.Parent != null)
		{
			DraggingLabelComponent.Parent.Components.Remove(DraggingLabelComponent);
			((Control)Designer.PageView.GetCurrentPageView().View).Invalidate();
		}
	}

	private bool CheckAddComponent(RectangleD rectangleComp, RectangleD rectanglePage)
	{
		if (rectanglePage.Width >= rectangleComp.Right)
		{
			return rectanglePage.Height >= rectangleComp.Bottom;
		}
		return false;
	}

	private void CreateDatabandComponentFromFileData(string file)
	{
		foreach (object database in Designer.Report.Dictionary.Databases)
		{
			if (database is StiFileDatabase stiFileDatabase && stiFileDatabase.PathData == file)
			{
				StiDataSource stiDataSource = StiDataSourceHelper.GetDataSourcesFromDatabase(Designer.Report, stiFileDatabase).FirstOrDefault();
				if (stiDataSource != null)
				{
					CreateDatabandComponentFromDatabase(stiDataSource);
					break;
				}
			}
		}
	}

	internal void CreateDatabandComponentFromDatabase(StiDataSource database)
	{
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		if (database == null)
		{
			return;
		}
		StiPage currentPage = Designer.Report.GetCurrentPage();
		if (currentPage is IStiDashboard)
		{
			IStiTableElement stiTableElement = StiActivator.CreateObject("Stimulsoft.Dashboard.Components.Table.StiTableElement") as IStiTableElement;
			string baseName = (Designer.Report.Info.GenerateLocalizedName ? (Loc.Get("Components", "StiTable") + StiNameValidator.CorrectName(database.Name, Designer.Report)) : ("Data" + StiNameValidator.CorrectName(database.Name, Designer.Report)));
			stiTableElement.Name = StiNameCreation.CreateName(Designer.Report, baseName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
			foreach (StiDataColumn column in database.Columns)
			{
				stiTableElement.CreateMeter(column);
			}
			currentPage.Components.Add((StiComponent)stiTableElement);
			if (Designer.CurrentTool is StiSelectToolService stiSelectToolService)
			{
				stiSelectToolService.CheckSize((StiComponent)stiTableElement);
			}
			return;
		}
		StiDataBand stiDataBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.DataBand) as StiDataBand;
		stiDataBand.DataSourceName = StiNameValidator.CorrectName(database.Name, Designer.Report);
		string baseName2 = (Designer.Report.Info.GenerateLocalizedName ? (stiDataBand.LocalizedName + stiDataBand.DataSourceName) : ("Data" + stiDataBand.DataSourceName));
		stiDataBand.Name = StiNameCreation.CreateName(Designer.Report, baseName2, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
		currentPage.Components.Add(stiDataBand);
		if (Designer.CurrentTool is StiSelectToolService stiSelectToolService2)
		{
			stiSelectToolService2.CheckSize(stiDataBand);
		}
		stiDataBand.Dockable = true;
		StiDataBandPlacementWizardForm stiDataBandPlacementWizardForm = new StiDataBandPlacementWizardForm(stiDataBand, database);
		try
		{
			Designer.AddOwnedForm((Form)(object)stiDataBandPlacementWizardForm);
			((Form)stiDataBandPlacementWizardForm).ShowDialog();
			if (stiDataBandPlacementWizardForm.PlacementComponent == StiPlacementComponentWizard.Table)
			{
				currentPage.Components.Remove(stiDataBand);
				StiTable table = stiDataBandPlacementWizardForm.Table;
				currentPage.Components.Add(table);
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

	private void ProcessDraggingUnplacedElement()
	{
		Designer.UndoRedoSave(Loc.Get("PropertyMain", "Resize"), onlyCurrentPage: true);
		StiPage currentPage = Designer.Report.GetCurrentPage();
		currentPage.ResetSelection();
		DraggingComponent.IsSelected = true;
		Designer.SelectTool.Activate();
		Designer.SelectTool.Action = StiAction.Move;
		Designer.SelectTool.IsDragDropComponent = true;
		RectangleD defaultClientRectangle = DraggingComponent.DefaultClientRectangle;
		if (defaultClientRectangle.Width == defaultClientRectangle.Height)
		{
			defaultClientRectangle.Width = currentPage.Width / 2.0;
			defaultClientRectangle.Height = currentPage.Width / 2.0;
		}
		else
		{
			defaultClientRectangle.Width = currentPage.Width / 2.0;
		}
		RectangleD clientRectangle = Designer.Report.GetCurrentPage().Unit.ConvertFromHInches(defaultClientRectangle).AlignToGrid(currentPage.GridSize, Designer.Report.Info.AlignToGrid);
		PointD currentCursorPos = Designer.PageView.GetCurrentCursorPos();
		clientRectangle.X = currentCursorPos.X;
		clientRectangle.Y = currentCursorPos.Y;
		if (DraggingComponent.Width != 0.0)
		{
			clientRectangle.Width = DraggingComponent.Width;
		}
		if (DraggingComponent.Height != 0.0)
		{
			clientRectangle.Height = DraggingComponent.Height;
		}
		DraggingComponent.ClientRectangle = clientRectangle;
		Designer.PageView.StartPos = Cursor.Position;
		Designer.Report.Info.IsComponentsMoving = false;
		Designer.Builder.Redraw();
		Designer.AddSelectedToPropertyGrid(updateSelectedObjects: true);
		StiUnplacedElementsHelper.ElementsForm?.RemoveElement(DraggingComponent);
	}

	public override bool PreProcessMessage(ref Message msg)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Invalid comparison between Unknown and I4
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Invalid comparison between Unknown and I4
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Invalid comparison between Unknown and I4
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Invalid comparison between Unknown and I4
		if (((Message)(ref msg)).Msg == 261)
		{
			lockShowkeyTips = false;
		}
		if (((Message)(ref msg)).Msg == 256 || ((Message)(ref msg)).Msg == 260)
		{
			if (((Message)(ref msg)).Msg == 260 && !lockShowkeyTips && Designer.IsModalMode && Designer.CheckKeyTips())
			{
				lockShowkeyTips = true;
				return true;
			}
			double num = Designer.Report.GetCurrentPage().GridSize;
			if ((Control.ModifierKeys & 0x40000) > 0)
			{
				if (Designer.Report.Unit is StiCentimetersUnit)
				{
					num = 0.1;
				}
				if (Designer.Report.Unit is StiMillimetersUnit)
				{
					num = 1.0;
				}
				if (Designer.Report.Unit is StiHundredthsOfInchUnit)
				{
					num = 1.0;
				}
				if (Designer.Report.Unit is StiInchesUnit)
				{
					num = 0.01;
				}
			}
			if ((Control.ModifierKeys & 0x10000) > 0)
			{
				if ((int)((Message)(ref msg)).WParam == 37)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Resize"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item in selectedComponents)
					{
						if (!item.Inherited && StiRestrictionsHelper.IsAllowResize(item) && item.Width - num > 0.0)
						{
							item.Width -= num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					Designer.InvokeSelectedObjectsChanged();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 38)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Resize"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents2 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item2 in selectedComponents2)
					{
						if (!item2.Inherited && StiRestrictionsHelper.IsAllowResize(item2) && item2.Height - num > 0.0)
						{
							item2.Height -= num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					Designer.InvokeSelectedObjectsChanged();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 40)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Resize"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents3 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item3 in selectedComponents3)
					{
						if (!item3.Inherited && StiRestrictionsHelper.IsAllowResize(item3))
						{
							item3.Height += num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					Designer.InvokeSelectedObjectsChanged();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 39)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Resize"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents4 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item4 in selectedComponents4)
					{
						if (!item4.Inherited && StiRestrictionsHelper.IsAllowResize(item4))
						{
							item4.Width += num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					Designer.InvokeSelectedObjectsChanged();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
			}
			else if ((Control.ModifierKeys & 0x20000) > 0 || (Control.ModifierKeys & 0x40000) > 0)
			{
				if ((Control.ModifierKeys & 0x40000) > 0)
				{
					if (Designer.Report.Unit is StiInchesUnit)
					{
						num = 0.01;
					}
					else if (Designer.Report.Unit is StiHundredthsOfInchUnit)
					{
						num = 1.0;
					}
					else if (Designer.Report.Unit is StiCentimetersUnit)
					{
						num = 0.01;
					}
					else if (Designer.Report.Unit is StiMillimetersUnit)
					{
						num = 1.0;
					}
				}
				if ((int)((Message)(ref msg)).WParam == 37)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Move"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents5 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item5 in selectedComponents5)
					{
						if (!item5.Inherited && StiRestrictionsHelper.IsAllowMove(item5))
						{
							item5.Left -= num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 38)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Move"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents6 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item6 in selectedComponents6)
					{
						if (!item6.Inherited && StiRestrictionsHelper.IsAllowMove(item6))
						{
							item6.Top -= num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 40)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Move"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents7 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item7 in selectedComponents7)
					{
						if (!item7.Inherited && StiRestrictionsHelper.IsAllowMove(item7))
						{
							item7.Top += num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 39)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Move"), onlyCurrentPage: true);
					StiComponentsCollection selectedComponents8 = Designer.Report.GetCurrentPage().GetSelectedComponents();
					foreach (StiComponent item8 in selectedComponents8)
					{
						if (!item8.Inherited && StiRestrictionsHelper.IsAllowMove(item8))
						{
							item8.Left += num;
						}
					}
					Designer.PageView.GetCurrentPageView().Page.Correct();
					((Control)Designer).Refresh();
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
			}
			else
			{
				if ((int)((Message)(ref msg)).WParam == 39 && Designer.SelectedComponentsOnPage.Count > 0)
				{
					StiSelectionTransfer.MoveRight(Designer);
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 37 && Designer.SelectedComponentsOnPage.Count > 0)
				{
					StiSelectionTransfer.MoveLeft(Designer);
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 38 && Designer.SelectedComponentsOnPage.Count > 0)
				{
					StiSelectionTransfer.MoveUp(Designer);
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
				if ((int)((Message)(ref msg)).WParam == 40 && Designer.SelectedComponentsOnPage.Count > 0)
				{
					StiSelectionTransfer.MoveDown(Designer);
					StiElementActionHelper.CheckStatus(null, Designer);
					return true;
				}
			}
		}
		return ((Control)this).PreProcessMessage(ref msg);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Draw(e.Graphics, e.ClipRectangle);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragEnter(e);
		if (Designer.CurrentTool != null)
		{
			Designer.CurrentTool.OnDragEnter(this, e);
		}
		if (DraggingComponent is IStiElement && (DraggingComponent.Width == 0.0 || DraggingComponent.Height == 0.0))
		{
			ProcessDraggingUnplacedElement();
		}
		if (DraggingComponent != null && DraggingComponent.Parent == null)
		{
			Designer.Report.GetCurrentPage().Components.Add(DraggingComponent);
			((Control)Designer.PageView.GetCurrentPageView().View).Invalidate();
		}
		if (DraggingLabelComponent != null && DraggingLabelComponent.Parent == null)
		{
			Designer.Report.GetCurrentPage().Components.Add(DraggingLabelComponent);
			((Control)Designer.PageView.GetCurrentPageView().View).Invalidate();
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null && array.Length != 0)
		{
			string file = array[0];
			if (StiFileDialogHelper.ImageExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)))
			{
				if (Designer.IsDashboardSelected)
				{
					DraggingComponent = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && s is IStiImageElement).FirstOrDefault();
				}
				else
				{
					DraggingComponent = new StiImage();
				}
			}
		}
		StiOptions.Engine.GlobalEvents.InvokeDragEnterToPageInDesigner(Designer, e);
	}

	private async void OnDragDropAsync(object sender, DragEventArgs e)
	{
		_ = 2;
		try
		{
			if (Designer.CurrentTool != null)
			{
				Designer.CurrentTool.OnDragDrop(this, e);
			}
			if (DraggingComponent != null)
			{
				StiComponent draggingComponent = DraggingComponent;
				StiComponent draggingLabelComponent = DraggingLabelComponent;
				LeaveDragDrop();
				DraggingComponent = draggingComponent;
				DraggingLabelComponent = draggingLabelComponent;
				bool flag = false;
				StiComponent highlightedComponent = StiHighlightComponentService.GetHighlightedComponent(Designer.Report.GetCurrentPage());
				if (DraggingComponent is StiImage && highlightedComponent is StiImage)
				{
					(highlightedComponent as StiImage).CopyAllImageProperties(DraggingComponent as StiImage);
					flag = true;
					Designer.PageView.ResetDragging();
				}
				if (DraggingComponent is StiMap)
				{
					StiMap stiMap = (StiMap)DraggingComponent;
					if (!StiCustomMapFinder.IsCustom(stiMap.MapIdent))
					{
						StiMapCategoriesForm stiMapCategoriesForm = new StiMapCategoriesForm(Designer.Report);
						try
						{
							Designer.AddOwnedForm((Form)(object)stiMapCategoriesForm);
							if ((int)((Form)stiMapCategoriesForm).ShowDialog() == 1 && !string.IsNullOrEmpty(stiMapCategoriesForm.MapID))
							{
								stiMap.MapIdent = stiMapCategoriesForm.MapID;
							}
						}
						finally
						{
							((IDisposable)stiMapCategoriesForm)?.Dispose();
						}
					}
				}
				if (DraggingComponent is IStiImageElement && highlightedComponent is IStiImageElement)
				{
					(highlightedComponent as IStiImageElement).CopyAllImageProperties(DraggingComponent as IStiImageElement);
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(highlightedComponent);
				}
				if (DraggingComponent is IStiTextElement && highlightedComponent is IStiTextElement)
				{
					(highlightedComponent as IStiTextElement).Text = (DraggingComponent as IStiTextElement).Text;
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(highlightedComponent);
				}
				if (DraggingComponent is IStiTableElement && highlightedComponent is IStiTableElement)
				{
					IStiTableElement stiTableElement = highlightedComponent as IStiTableElement;
					stiTableElement.CreateMeters(DraggingComponent as IStiTableElement);
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiTableElement as StiComponent);
				}
				bool flag2 = StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer);
				if (flag2 && highlightedComponent is IStiTableElement)
				{
					IStiTableElement stiTableElement2 = highlightedComponent as IStiTableElement;
					StiComponent component2 = highlightedComponent;
					IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Columns"));
						stiTableElement2.CreateMeter(draggedDataColumnOrVariable);
						StiDashboardHelperCreator.CreateTableElementAutoSizer(stiTableElement2)?.AutoMeasureToColumn(stiTableElement2);
						Designer.PageView.ResetDragging();
						StiInplaceEvents.InvokeChanged(component2);
					}
					flag = true;
				}
				if (flag2 && highlightedComponent is IStiPivotTableElement)
				{
					IStiPivotTableElement stiPivotTableElement = highlightedComponent as IStiPivotTableElement;
					StiComponent component3 = highlightedComponent;
					IStiAppDataCell draggedDataColumnOrVariable2 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable2 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Field"));
						stiPivotTableElement.CreateNextMeter(draggedDataColumnOrVariable2);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(component3);
				}
				if (flag2 && highlightedComponent is IStiChartElement)
				{
					IStiChartElement stiChartElement = highlightedComponent as IStiChartElement;
					IStiAppDataCell draggedDataColumnOrVariable3 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable3 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiChartElement.AddValue(draggedDataColumnOrVariable3);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiChartElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiGaugeElement)
				{
					IStiGaugeElement stiGaugeElement = highlightedComponent as IStiGaugeElement;
					IStiAppDataCell draggedDataColumnOrVariable4 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable4 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiGaugeElement.AddValue(draggedDataColumnOrVariable4);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiGaugeElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiProgressElement)
				{
					IStiProgressElement stiProgressElement = highlightedComponent as IStiProgressElement;
					IStiAppDataCell draggedDataColumnOrVariable5 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable5 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiProgressElement.AddValue(draggedDataColumnOrVariable5);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiProgressElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiIndicatorElement)
				{
					IStiIndicatorElement stiIndicatorElement = highlightedComponent as IStiIndicatorElement;
					IStiAppDataCell draggedDataColumnOrVariable6 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable6 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiIndicatorElement.AddValue(draggedDataColumnOrVariable6);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiIndicatorElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiOnlineMapElement)
				{
					IStiOnlineMapElement stiOnlineMapElement = highlightedComponent as IStiOnlineMapElement;
					IStiAppDataCell draggedDataColumnOrVariable7 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable7 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Field"));
						stiOnlineMapElement.CreateNextMeter(draggedDataColumnOrVariable7);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiOnlineMapElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiRegionMapElement)
				{
					IStiRegionMapElement stiRegionMapElement = highlightedComponent as IStiRegionMapElement;
					IStiAppDataCell draggedDataColumnOrVariable8 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable8 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Field"));
						stiRegionMapElement.CreateNextMeter(draggedDataColumnOrVariable8);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiRegionMapElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiListBoxElement)
				{
					IStiListBoxElement stiListBoxElement = highlightedComponent as IStiListBoxElement;
					IStiAppDataCell draggedDataColumnOrVariable9 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable9 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Field"));
						stiListBoxElement.CreateNextMeter(draggedDataColumnOrVariable9);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiListBoxElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiComboBoxElement)
				{
					IStiComboBoxElement stiComboBoxElement = highlightedComponent as IStiComboBoxElement;
					IStiAppDataCell draggedDataColumnOrVariable10 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable10 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Field"));
						stiComboBoxElement.CreateNextMeter(draggedDataColumnOrVariable10);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiComboBoxElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiTreeViewBoxElement)
				{
					IStiTreeViewBoxElement stiTreeViewBoxElement = highlightedComponent as IStiTreeViewBoxElement;
					IStiAppDataCell draggedDataColumnOrVariable11 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable11 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiTreeViewBoxElement.AddKey(draggedDataColumnOrVariable11);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiTreeViewBoxElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiTreeViewElement)
				{
					IStiTreeViewElement stiTreeViewElement = highlightedComponent as IStiTreeViewElement;
					IStiAppDataCell draggedDataColumnOrVariable12 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable12 != null)
					{
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Key"));
						stiTreeViewElement.AddKey(draggedDataColumnOrVariable12);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiTreeViewElement as StiComponent);
				}
				if (flag2 && highlightedComponent is IStiDatePickerElement)
				{
					IStiDatePickerElement stiDatePickerElement = highlightedComponent as IStiDatePickerElement;
					IStiAppDataCell draggedDataColumnOrVariable13 = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
					if (draggedDataColumnOrVariable13 != null)
					{
						StiDataColumn stiDataColumn = draggedDataColumnOrVariable13 as StiDataColumn;
						StiVariable stiVariable = draggedDataColumnOrVariable13 as StiVariable;
						Type type = ((stiDataColumn != null) ? stiDataColumn.Type : stiVariable?.Type);
						if (type != null && !(type == typeof(DateTime)) && !(type == typeof(DateTime?)) && !(type == typeof(DateTimeRange)))
						{
							MessageBox.Show((IWin32Window)(object)this, string.Format(Loc.Get("Dashboard", "FieldTypeRestrictionHint"), Loc.Get("FormFormatEditor", "Date")), Loc.Get("FormDesigner", "title"));
							return;
						}
						Designer.UndoRedoSave(Loc.Get("FormDesigner", "PropertyChange") + " " + Loc.GetMain("Value"));
						stiDatePickerElement.AddValueMeter(draggedDataColumnOrVariable13);
					}
					flag = true;
					Designer.PageView.ResetDragging();
					StiInplaceEvents.InvokeChanged(stiDatePickerElement as StiComponent);
				}
				if (!flag)
				{
					Designer.UndoRedoSave(Loc.Get("PropertyMain", "Create") + " " + DraggingComponent.LocalizedName);
					Designer.Report.GetCurrentPage().Components.Add(DraggingComponent);
					if (DraggingLabelComponent != null)
					{
						Designer.Report.GetCurrentPage().Components.Add(DraggingLabelComponent);
					}
				}
				else
				{
					highlightedComponent.HighlightState = StiHighlightState.Hide;
				}
			}
			((Control)this).OnMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, 0, 0, 0));
			StiOptions.Engine.GlobalEvents.InvokeDragDropToPageInDesigner(Designer, e);
			DraggingComponent = null;
			DraggingLabelComponent = null;
			Point point = ((Control)this).PointToClient(Cursor.Position);
			StiPage currentPage = Designer.Report.GetCurrentPage();
			double value = Designer.XToPage(point.X);
			double value2 = Designer.YToPage(point.Y);
			double pointOnGridX = StiAlignValue.AlignToGrid(value, currentPage.GridSize, Designer.Report.Info.AlignToGrid);
			double pointOnGridY = StiAlignValue.AlignToGrid(value2, currentPage.GridSize, Designer.Report.Info.AlignToGrid);
			double currentPositionOnGridX = pointOnGridX;
			double currentPositionOnGridY = pointOnGridY;
			double nextRowPositionOnGrid = pointOnGridY;
			string firstDragFileDatabase = string.Empty;
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files != null)
			{
				List<string> exts = StiFileDialogHelper.GetAllResourceExtensions().ToList();
				exts.Add("dct");
				string[] array = files;
				foreach (string file in array)
				{
					StiComponent component = null;
					RectangleD componentRect = default(RectangleD);
					if (file.EndsWithInvariantIgnoreCase(".mrt") || file.EndsWithInvariantIgnoreCase(".mrz") || file.EndsWithInvariantIgnoreCase(".mrx"))
					{
						if (!File.Exists(file))
						{
							return;
						}
						StiDesignerControl designer = Designer;
						bool flag3 = false;
						try
						{
							flag3 = StiReportOpenDialog.OpenReport(designer, designer.Report, file);
						}
						catch (Exception exception)
						{
							StiDesignerControl designer2 = Designer;
							StiExceptionProvider.Show(exception, rethrow: false, (designer2 != null) ? ((Control)designer2).FindForm() : null);
						}
						finally
						{
							if (designer != null && designer.Report != null)
							{
								designer.Report.Dictionary.Connect(loadData: false);
								designer.Builder.BuildReport();
								designer.InvokeBuidingToolbox();
								designer.Builder.InvalidateRulers();
							}
						}
						if (flag3)
						{
							StiOptions.Engine.GlobalEvents.InvokeLoadedReportInDesigner(designer);
						}
						Windows7Taskbar.HideTaskbarProgress();
						return;
					}
					if (file.EndsWithInvariantIgnoreCase(".rtf") && !Designer.IsDashboardSelected)
					{
						string rtfText = string.Empty;
						try
						{
							rtfText = File.ReadAllText(file);
						}
						catch (Exception exception2)
						{
							StiDesignerControl designer3 = Designer;
							StiExceptionProvider.Show(exception2, rethrow: false, (designer3 != null) ? ((Control)designer3).FindForm() : null);
						}
						componentRect = new RectangleD(0.0, 0.0, 2.0, 1.0);
						component = new StiRichText();
						((StiRichText)component).RtfText = rtfText;
					}
					else if (file.EndsWithInvariantIgnoreCase(".txt"))
					{
						string text = string.Empty;
						try
						{
							text = File.ReadAllText(file);
						}
						catch (Exception exception3)
						{
							StiDesignerControl designer4 = Designer;
							StiExceptionProvider.Show(exception3, rethrow: false, (designer4 != null) ? ((Control)designer4).FindForm() : null);
						}
						componentRect = new RectangleD(0.0, 0.0, 2.0, 1.0);
						if (Designer.IsDashboardSelected)
						{
							component = StiDashboardCreator.CreateTextElement(Designer.Report) as StiComponent;
							((IStiTextElement)component).Text = text;
							componentRect = Designer.Report.GetCurrentPage().Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, 100.0, 40.0));
						}
						else
						{
							component = new StiText(componentRect, text);
						}
					}
					else if (StiFileDialogHelper.ImageExts.Any((string i) => file.EndsWithInvariantIgnoreCase(i)))
					{
						byte[] array2;
						try
						{
							array2 = File.ReadAllBytes(file);
						}
						catch (Exception exception4)
						{
							StiDesignerControl designer5 = Designer;
							StiExceptionProvider.Show(exception4, rethrow: false, (designer5 != null) ? ((Control)designer5).FindForm() : null);
							return;
						}
						if (array2.Length > 1048576)
						{
							return;
						}
						component = ((!Designer.IsDashboardSelected) ? new StiImage() : (StiDashboardCreator.CreateImageElement(Designer.Report) as StiComponent));
						RectangleD paintRectangle = currentPage.GetPaintRectangle();
						Image val = StiImageConverter.BytesToImage(array2);
						try
						{
							if (Designer.IsDashboardSelected)
							{
								((IStiImageElement)component).Image = array2;
								double num = currentPage.Width - currentPositionOnGridX;
								double num2 = currentPage.Height - currentPositionOnGridY;
								componentRect.Width = (((double)val.Width < num) ? ((double)val.Width) : num);
								componentRect.Height = (((double)val.Height < num2) ? ((double)val.Height) : num2);
							}
							else
							{
								((StiImage)component).PutImage(array2);
								componentRect.Width = (((double)val.Width < paintRectangle.Width) ? currentPage.Unit.ConvertFromHInches((double)val.Width) : currentPage.Width);
								componentRect.Height = (((double)val.Height < paintRectangle.Height) ? currentPage.Unit.ConvertFromHInches((double)val.Height) : currentPage.Height);
							}
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
					if (component == null)
					{
						if (exts.Any(file.EndsWithInvariantIgnoreCase))
						{
							string extension = Path.GetExtension(file).ToLowerInvariant();
							StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(extension);
							if (!typeFromExtension.HasValue)
							{
								continue;
							}
							if (StiResourceTypeHelper.PossibleConvertToDataSource(typeFromExtension.GetValueOrDefault()))
							{
								StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Designer);
								await service.LoadFileDatabaseAsync(file, files, throwException: true);
								firstDragFileDatabase = file;
								continue;
							}
							StiReport stiReport = Designer.Report;
							if (stiReport == null)
							{
								stiReport = new StiReport();
							}
							string newResourceName = StiDictionaryDropFilesControl.GetNewResourceName(file, stiReport);
							StiResource resource = new StiResource(newResourceName, newResourceName, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(file));
							stiReport.Dictionary.Resources.Add(resource);
							if (Designer.Report == null)
							{
								Designer.Report = stiReport;
							}
							else
							{
								StiDictionaryPanelHelper.BuildDictionary(Designer);
							}
							continue;
						}
						if (file.EndsWithInvariantIgnoreCase(".dct"))
						{
							if (!File.Exists(file))
							{
								continue;
							}
							try
							{
								StiReport stiReport2 = Designer.Report;
								if (stiReport2 == null)
								{
									stiReport2 = new StiReport();
								}
								stiReport2.Dictionary.Load(file);
								if (Designer.Report == null)
								{
									Designer.Report = stiReport2;
								}
								StiDictionaryPanelHelper.BuildDictionary(Designer);
								return;
							}
							catch (Exception exception5)
							{
								StiDesignerControl designer6 = Designer;
								StiExceptionProvider.Show(exception5, rethrow: false, (designer6 != null) ? ((Control)designer6).FindForm() : null);
								return;
							}
						}
						if (StiFileDialogHelper.ImageExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.FontExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.TextExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.DocumentExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)))
						{
							string extension2 = Path.GetExtension(file).ToLowerInvariant();
							StiResourceType? typeFromExtension2 = StiResourceTypeHelper.GetTypeFromExtension(extension2);
							if (typeFromExtension2.HasValue)
							{
								StiReport stiReport3 = Designer.Report;
								if (stiReport3 == null)
								{
									stiReport3 = new StiReport();
								}
								string newResourceName2 = StiNameHelper.GetNewResourceName(file, stiReport3);
								StiResource resource2 = new StiResource(newResourceName2, newResourceName2, typeFromExtension2.GetValueOrDefault(), File.ReadAllBytes(file));
								stiReport3.Dictionary.Resources.Add(resource2);
								if (Designer.Report == null)
								{
									Designer.Report = stiReport3;
								}
								StiDictionaryPanelService service2 = StiDictionaryPanelService.GetService();
								service2.BuildDictionary();
							}
							continue;
						}
						if (file.EndsWithInvariantIgnoreCase(".dbf") || file.EndsWithInvariantIgnoreCase(".csv") || file.EndsWithInvariantIgnoreCase(".xml") || file.EndsWithInvariantIgnoreCase(".json") || file.EndsWithInvariantIgnoreCase(".xls") || file.EndsWithInvariantIgnoreCase(".xlsx"))
						{
							try
							{
								StiDictionaryPanelHelper.ActivatePanel(Designer);
								StiDictionaryPanelHelper.ShowProgress(Designer, Loc.Get("DesignerFx", "LoadingData"));
								StiFileDatabase database = StiDictionaryPanelService.NewDatabaseFromExtension(file);
								if (database == null)
								{
									return;
								}
								StiReport report = Designer.Report;
								if (report == null)
								{
									report = new StiReport();
								}
								string name = (database.Alias = StiNameHelper.GetNewDatabaseName(file, report));
								database.Name = name;
								database.PathData = file;
								bool synchronize = false;
								StiXmlDatabase stiXmlDatabase = database as StiXmlDatabase;
								if (file.EndsWithInvariantIgnoreCase(".xml") && stiXmlDatabase != null)
								{
									stiXmlDatabase.PathSchema = files.FirstOrDefault((string f) => f.ToLowerInvariant() == Path.ChangeExtension(file.ToLowerInvariant(), "xsd"));
									stiXmlDatabase.XmlType = (string.IsNullOrWhiteSpace(stiXmlDatabase.PathSchema) ? StiXmlType.Xml : StiXmlType.AdoNetXml);
									if (file.EndsWithInvariantIgnoreCase("demo.xml") && stiXmlDatabase.PathSchema != null && stiXmlDatabase.PathSchema.EndsWithInvariantIgnoreCase("demo.xsd"))
									{
										synchronize = true;
									}
								}
								report.Dictionary.Databases.Add(database);
								await database.CreateDataSourcesAsync(report.Dictionary);
								if (synchronize)
								{
									await report.Dictionary.SynchronizeRelationsAsync(database.Name);
								}
								if (Designer.Report == null)
								{
									Designer.Report = report;
								}
								StiDictionaryPanelHelper.BuildDictionary(Designer);
							}
							catch
							{
							}
							finally
							{
								StiDictionaryPanelHelper.HideProgress(Designer);
							}
						}
					}
					if (CheckAddComponent(new RectangleD(currentPositionOnGridX, currentPositionOnGridY, componentRect.Width, componentRect.Height), currentPage.ClientRectangle))
					{
						componentRect.X = currentPositionOnGridX;
						componentRect.Y = currentPositionOnGridY;
						currentPositionOnGridX += componentRect.Width;
						if (nextRowPositionOnGrid < currentPositionOnGridY + componentRect.Height)
						{
							nextRowPositionOnGrid = currentPositionOnGridY + componentRect.Height;
						}
					}
					else if (CheckAddComponent(new RectangleD(0.0, nextRowPositionOnGrid, componentRect.Width, componentRect.Height), currentPage.ClientRectangle))
					{
						componentRect.X = 0.0;
						componentRect.Y = nextRowPositionOnGrid;
						currentPositionOnGridX = componentRect.Width;
						currentPositionOnGridY = nextRowPositionOnGrid;
						if (nextRowPositionOnGrid < currentPositionOnGridY + componentRect.Height)
						{
							nextRowPositionOnGrid = currentPositionOnGridY + componentRect.Height;
						}
					}
					else
					{
						componentRect.X = pointOnGridX;
						componentRect.Y = pointOnGridY;
						currentPositionOnGridX = pointOnGridX;
						currentPositionOnGridY = pointOnGridY;
						nextRowPositionOnGrid = pointOnGridY;
					}
					if (component != null)
					{
						component.ClientRectangle = componentRect;
						Designer.UndoRedoSave(Loc.Get("PropertyMain", "Create") + " " + component.LocalizedName, onlyCurrentPage: true);
						if (Designer.UseLastFormat)
						{
							Designer.SetStyleToComponent(component);
						}
						currentPage.Components.Add(component);
						component.Select();
						Designer.Report.GetCurrentPage().ResetSelection();
					}
				}
			}
			if (!string.IsNullOrEmpty(firstDragFileDatabase))
			{
				CreateDatabandComponentFromFileData(firstDragFileDatabase);
			}
			((Control)Designer.PageView.GetCurrentPageView().View).Invalidate();
		}
		catch (Exception exception6)
		{
			StiDesignerControl designer7 = Designer;
			StiExceptionProvider.Show(exception6, rethrow: false, (designer7 != null) ? ((Control)designer7).FindForm() : null);
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		((Control)this).OnDragLeave(e);
		if (Designer.CurrentTool != null)
		{
			Designer.CurrentTool.OnDragLeave(this, e);
		}
		LeaveDragDrop();
		StiOptions.Engine.GlobalEvents.InvokeDragLeaveFromPageInDesigner(Designer, e);
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		((Control)this).OnDragOver(e);
		((Control)this).OnMouseMove(new MouseEventArgs((MouseButtons)0, 0, 0, 0, 0));
		StiOptions.Engine.GlobalEvents.InvokeDragOverPageInDesigner(Designer, e);
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			List<string> exts = StiFileDialogHelper.GetAllResourceExtensions().ToList();
			exts.Add("dct");
			if (array.Any((string file) => exts.Any(file.EndsWithInvariantIgnoreCase)))
			{
				e.Effect = (DragDropEffects)(-2147483645);
				return;
			}
			string[] array2 = array;
			foreach (string file2 in array2)
			{
				if (file2.EndsWithInvariantIgnoreCase(".mrt") || file2.EndsWithInvariantIgnoreCase(".mrz") || file2.EndsWithInvariantIgnoreCase(".mrx") || file2.EndsWithInvariantIgnoreCase(".dct") || file2.EndsWithInvariantIgnoreCase(".dbf") || file2.EndsWithInvariantIgnoreCase(".csv") || file2.EndsWithInvariantIgnoreCase(".xml") || file2.EndsWithInvariantIgnoreCase(".json") || file2.EndsWithInvariantIgnoreCase(".xls") || file2.EndsWithInvariantIgnoreCase(".xlsx") || StiFileDialogHelper.FontExts.Any((string ext) => file2.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.ImageExts.Any((string ext) => file2.EndsWithInvariantIgnoreCase(ext)))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
	{
		((Control)this).OnQueryContinueDrag(e);
		if (cancelDragDrop)
		{
			e.Action = (DragAction)2;
			cancelDragDrop = false;
			LeaveDragDrop();
			DraggingComponent = null;
			DraggingLabelComponent = null;
		}
		StiOptions.Engine.GlobalEvents.InvokeQueryContinueDragOverPageInDesigner(Designer, e);
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (Designer.CurrentTool != null)
		{
			Designer.CurrentTool.OnClick(this, e);
		}
		((Control)this).Focus();
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		bool flag = false;
		if (Designer.CurrentTool != null)
		{
			flag = Designer.CurrentTool.OnDoubleClick(this, e);
		}
		if (Designer.Report != null)
		{
			Designer.Report.Info.IsComponentsMoving = false;
		}
		StiComponent component;
		StiComponentDesigner componentDesigner = GetComponentDesigner(out component);
		if (componentDesigner != null && component != null && StiRestrictionsHelper.IsAllowChange(component) && !flag)
		{
			Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
			componentDesigner.OnDoubleClick(component);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		if (Designer != null)
		{
			if (Designer.CurrentTool != null)
			{
				Designer.CurrentTool.OnMouseDown(this, e);
			}
			else
			{
				((Control)Designer.PageView.GetCurrentPageView()).Cursor = Cursors.Cross;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		((Control)this).OnMouseMove(e);
		if (Designer == null)
		{
			return;
		}
		Designer?.CurrentTool?.OnMouseMove(this, e);
		if (((Control)this).Parent == null || ((Control)this).Parent.Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Parent.Controls)
		{
			Control val = item;
			if (val is StiHorRollerLine stiHorRollerLine)
			{
				stiHorRollerLine.OnMouseMove(this, e);
			}
			if (val is StiVerRollerLine stiVerRollerLine)
			{
				stiVerRollerLine.OnMouseMove(this, e);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (Designer != null)
		{
			Designer.Report.Info.IsComponentsMoving = false;
			Designer?.CurrentTool?.OnMouseUp(this, e);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
		if (!keyProcessed && (char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || e.KeyChar == ' '))
		{
			OpenEditor(e.KeyChar.ToString());
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
		keyProcessed = false;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Invalid comparison between Unknown and I4
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Invalid comparison between Unknown and I4
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected I4, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Invalid comparison between Unknown and I4
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Invalid comparison between Unknown and I4
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if (e.Shift)
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 45)
			{
				StiActions.Clipboard.InvokePaste(Designer);
				keyProcessed = true;
				return;
			}
			if ((int)keyCode == 46)
			{
				StiActions.Clipboard.InvokeCut(Designer);
				keyProcessed = true;
				return;
			}
			if ((int)keyCode == 116)
			{
				StiActions.MainMenu.InvokeReportPreview(Designer);
				keyProcessed = true;
				return;
			}
		}
		if (e.Control)
		{
			Keys keyCode2 = e.KeyCode;
			if ((int)keyCode2 <= 76)
			{
				if ((int)keyCode2 <= 66)
				{
					if ((int)keyCode2 == 45)
					{
						StiActions.Clipboard.InvokeCopy(Designer);
						keyProcessed = true;
						return;
					}
					if ((int)keyCode2 == 66)
					{
						SetFontStyleToPage((FontStyle)1);
						keyProcessed = true;
						return;
					}
				}
				else
				{
					if ((int)keyCode2 == 69)
					{
						Designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Center);
						Designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Center);
						((Control)Designer).Refresh();
						keyProcessed = true;
						return;
					}
					switch (keyCode2 - 73)
					{
					case 0:
						SetFontStyleToPage((FontStyle)2);
						keyProcessed = true;
						return;
					case 3:
						Designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Left);
						Designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Left);
						((Control)Designer).Refresh();
						keyProcessed = true;
						return;
					case 1:
						Designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Width);
						((Control)Designer).Refresh();
						keyProcessed = true;
						return;
					}
				}
			}
			else if ((int)keyCode2 <= 85)
			{
				if ((int)keyCode2 == 82)
				{
					Designer.Report.GetCurrentPage().SetHorAlign(StiHorAlignment.Right);
					Designer.Report.GetCurrentPage().SetTextHorAlign(StiTextHorAlignment.Right);
					((Control)Designer).Refresh();
					keyProcessed = true;
					return;
				}
				if ((int)keyCode2 == 85)
				{
					SetFontStyleToPage((FontStyle)4);
					keyProcessed = true;
					return;
				}
			}
			else
			{
				if ((int)keyCode2 == 107)
				{
					object font = Designer.Report.GetCurrentPage().GetFont();
					Font val = (Font)((font is Font) ? font : null);
					if (val != null)
					{
						SetFontSiseToPage(val.Size + 0.5f);
						((Control)Designer).Refresh();
					}
					keyProcessed = true;
					return;
				}
				if ((int)keyCode2 == 109)
				{
					object font2 = Designer.Report.GetCurrentPage().GetFont();
					Font val2 = (Font)((font2 is Font) ? font2 : null);
					if (val2 != null)
					{
						SetFontSiseToPage(val2.Size - 0.5f);
						((Control)Designer).Refresh();
					}
					keyProcessed = true;
					return;
				}
			}
		}
		if ((int)e.KeyCode == 27)
		{
			if (StiInplaceDesigner.IsEditorActivated)
			{
				StiInplaceDesigner.Close();
			}
			else if (Designer.PlacementComponentService.IsOpen)
			{
				Designer.PlacementComponentService.ClosePopup();
			}
			else if (Designer.HighlightComponentService != null && Designer.HighlightComponentService.IsOpen)
			{
				Designer.HighlightComponentService.ClosePopup();
			}
			else
			{
				StiSelectToolService selectTool = Designer.SelectTool;
				if (selectTool != null && !selectTool.Active)
				{
					selectTool.Activate();
				}
				if (Designer.SelectTool != null && Designer.SelectTool.TableHelperV2 != null)
				{
					if (Designer.UndoRedo != null)
					{
						Designer.UndoRedo.UndoStackPop();
					}
					Designer.SelectTool.TableHelperV2.RestoreComponentsLocation();
					Designer.SelectTool.TableHelperV2.Dispose();
					Designer.SelectTool.TableHelperV2 = null;
					Designer.SelectTool.Action = StiAction.None;
					Designer.Builder.Redraw();
					((Control)Designer.PageView.GetCurrentPageView().View).Cursor = Cursors.Default;
				}
				else
				{
					cancelDragDrop = true;
					if (Designer.SelectedComponentsOnPage.Count > 0)
					{
						StiComponent stiComponent = Designer.SelectedComponentsOnPage[0];
						if (!(stiComponent is StiPage))
						{
							Designer.Report.GetCurrentPage().ResetSelection();
							stiComponent.Parent.Select();
							Designer.AddSelectedToPropertyGrid();
							((Control)Designer).Refresh();
						}
					}
				}
			}
			keyProcessed = true;
			return;
		}
		if ((int)e.KeyCode == 13 && (Control.ModifierKeys & 0x10000) > 0)
		{
			StiPropertiesPanelService service = StiPropertiesPanelService.GetService(Designer);
			if (service != null)
			{
				StiMainMenuService.GetService(Designer)?.PanelSelect(service, onlyActivate: true);
			}
			keyProcessed = true;
			return;
		}
		if ((int)e.KeyCode == 13 && e.Control)
		{
			if (Designer.SelectedComponentsOnPage.Count > 0)
			{
				StiComponent stiComponent2 = Designer.SelectedComponentsOnPage[0];
				Designer.UndoRedoSave(Loc.Get("Buttons", "Design"), onlyCurrentPage: true);
				StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(Designer, stiComponent2.GetType());
				componentDesigner.Design(stiComponent2);
				Designer.Builder.Redraw();
			}
			keyProcessed = true;
			return;
		}
		StiServiceContainer services = Designer.Services.GetServices(typeof(StiToolService));
		foreach (StiToolService item in services)
		{
			if (item.Shortcut == e.KeyData)
			{
				item.Activate();
				keyProcessed = true;
				return;
			}
		}
		if ((int)e.KeyCode == 46)
		{
			StiActions.Clipboard.InvokeDelete(Designer);
		}
		if ((int)e.KeyCode == 13 || (int)e.KeyCode == 113)
		{
			OpenEditor(null, isKeystroke: true);
		}
	}

	public StiViewControl()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).TabStop = true;
		if (StiOptions.Designer.AllowUseDragDrop)
		{
			((Control)this).AllowDrop = true;
		}
		((Control)this).DragDrop += new DragEventHandler(OnDragDropAsync);
	}
}
