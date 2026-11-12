using System.Collections;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design.Images;

public static class StiReportDesignImages
{
	public static class Actions
	{
		public static Image Build(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Build", size, allowCache);
		}

		public static Image Check(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Check", size, allowCache);
		}

		public static Image CheckForIssues(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.CheckForIssues", size, allowCache);
		}

		public static Image ChangeType(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ChangeType", size, allowCache);
		}

		public static Image Code(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Code", size, allowCache);
		}

		public static Image CollapseAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.CollapseAll", size, allowCache);
		}

		public static Image Components(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Components", size, allowCache);
		}

		public static Image Conditions(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Conditions", size, allowCache);
		}

		public static Image CopyStyle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.CopyStyle", size, allowCache);
		}

		public static Image DataTransformation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.DataTransformation", size, allowCache);
		}

		public static Image Design(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Design", size, allowCache);
		}

		public static Image DeleteColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.DeleteColumn", size, allowCache);
		}

		public static Image DeleteRow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.DeleteRow", size, allowCache);
		}

		public static Image Dictionary(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Dictionary", size, allowCache);
		}

		public static Image Dimension(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Dimension", size, allowCache);
		}

		public static Image Duplicate(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Duplicate", size, allowCache);
		}

		public static Image EmbedAllData(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.EmbedAllData", size, allowCache);
		}

		public static Image Edit(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Edit", size, allowCache);
		}

		public static Image EditData(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.EditData", size, allowCache);
		}

		public static Image EditSmall(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.EditSmall", size, allowCache);
		}

		public static Image EditTable(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.EditTable", size, allowCache);
		}

		public static Image Error(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Error", size, allowCache);
		}

		public static Image ExpandAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ExpandAll", size, allowCache);
		}

		public static Image ExportXml(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ExportXml", size, allowCache);
		}

		public static Image Expression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Expression", size, allowCache);
		}

		public static Image Events(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Events", size, allowCache);
		}

		public static Image GetCulture(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.GetCulture", size, allowCache);
		}

		public static Image GetStyle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.GetStyle", size, allowCache);
		}

		public static Image Filter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Filter", size, allowCache);
		}

		public static Image Help(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Help", size, allowCache);
		}

		public static Image HorizontalPlacement(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.HorizontalPlacement", size, allowCache);
		}

		public static Image ImportRelation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ImportRelation", size, allowCache);
		}

		public static Image ImportXml(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ImportXml", size, allowCache);
		}

		public static Image Information(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Information", size, allowCache);
		}

		public static Image InsertColumnToLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.InsertColumnToLeft", size, allowCache);
		}

		public static Image InsertColumnToRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.InsertColumnToRight", size, allowCache);
		}

		public static Image InsertRowAbove(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.InsertRowAbove", size, allowCache);
		}

		public static Image InsertRowBelow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.InsertRowBelow", size, allowCache);
		}

		public static Image Interaction(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Interaction", size, allowCache);
		}

		public static Image JoinCells(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.JoinCells", size, allowCache);
		}

		public static Image MakeRelationActive(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MakeRelationActive", size, allowCache);
		}

		public static Image MarkUsedItems(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MarkUsedItems", size, allowCache);
		}

		public static Image Measure(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Measure", size, allowCache);
		}

		public static Image MergeDictionary(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MergeDictionary", size, allowCache);
		}

		public static Image MergeXml(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MergeXml", size, allowCache);
		}

		public static Image MoveUp(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MoveUp", size, allowCache);
		}

		public static Image MoveDown(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MoveDown", size, allowCache);
		}

		public static Image MoveLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MoveLeft", size, allowCache);
		}

		public static Image MoveRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.MoveRight", size, allowCache);
		}

		public static Image NewBusinessObject(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewBusinessObject", size, allowCache);
		}

		public static Image NewCalculatedColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewCalculatedColumn", size, allowCache);
		}

		public static Image NewCategory(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewCategory", size, allowCache);
		}

		public static Image NewColor(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewColor", size, allowCache);
		}

		public static Image NewColorScaleCondition(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewColorScaleCondition", size, allowCache);
		}

		public static Image NewCondition(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewCondition", size, allowCache);
		}

		public static Image NewConstantLine(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewConstantLine", size, allowCache);
		}

		public static Image NewCulture(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewCulture", size, allowCache);
		}

		public static Image NewDashboard(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDashboard", size, allowCache);
		}

		public static Image NewDataBarCondition(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataBarCondition", size, allowCache);
		}

		public static Image NewDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataColumn", size, allowCache);
		}

		public static Image NewDataConnection(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataConnection", size, allowCache);
		}

		public static Image NewDataParameter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataParameter", size, allowCache);
		}

		public static Image NewDataRelation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataRelation", size, allowCache);
		}

		public static Image NewDataSource(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataSource", size, allowCache);
		}

		public static Image NewDataTransformation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewDataTransformation", size, allowCache);
		}

		public static Image NewFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewFile", size, allowCache);
		}

		public static Image NewFilter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewFilter", size, allowCache);
		}

		public static Image NewForm(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewForm", size, allowCache);
		}

		public static Image NewScreen(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewScreen", size, allowCache);
		}

		public static Image NewGauge(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewGauge", size, allowCache);
		}

		public static Image NewGroup(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewGroup", size, allowCache);
		}

		public static Image NewHighlightCondition(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewHighlightCondition", size, allowCache);
		}

		public static Image NewItem(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewItem", size, allowCache);
		}

		public static Image NewIconSetCondition(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewIconSetCondition", size, allowCache);
		}

		public static Image NewQuery(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewQuery", size, allowCache);
		}

		public static Image NewPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewPage", size, allowCache);
		}

		public static Image NewResource(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewResource", size, allowCache);
		}

		public static Image NewResult(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewResult", size, allowCache);
		}

		public static Image NewSeries(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewSeries", size, allowCache);
		}

		public static Image NewSort(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewSort", size, allowCache);
		}

		public static Image NewStyle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewStyle", size, allowCache);
		}

		public static Image NewStyleCollection(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewStyleCollection", size, allowCache);
		}

		public static Image NewVariable(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.NewVariable", size, allowCache);
		}

		public static Image OpenFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.OpenFile", size, allowCache);
		}

		public static Image OpenFolderSmall(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.OpenFolderSmall", size, allowCache);
		}

		public static Image Overview(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Overview", size, allowCache);
		}

		public static Image Preview(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Preview", size, allowCache);
		}

		public static Image Properties(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Properties", size, allowCache);
		}

		public static Image ProtectReport(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ProtectReport", size, allowCache);
		}

		public static Image Queries(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Queries", size, allowCache);
		}

		public static Image RunQuery(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RunQuery", size, allowCache);
		}

		public static Image RetrieveColumnsDown(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RetrieveColumnsDown", size, allowCache);
		}

		public static Image RetrieveColumnsPlus(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RetrieveColumnsPlus", size, allowCache);
		}

		public static Image RenderingMessage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RenderingMessage", size, allowCache);
		}

		public static Image Remove(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Remove", size, allowCache);
		}

		public static Image RemoveAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RemoveAll", size, allowCache);
		}

		public static Image RemoveMobileSurface(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RemoveMobileSurface", size, allowCache);
		}

		public static Image RemoveUnused(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RemoveUnused", size, allowCache);
		}

		public static Image Rename(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Rename", size, allowCache);
		}

		public static Image ReportTree(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ReportTree", size, allowCache);
		}

		public static Image ReportOptions(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ReportOptions", size, allowCache);
		}

		public static Image Run(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Run", size, allowCache);
		}

		public static Image RunStyleDesigner(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.RunStyleDesigner", size, allowCache);
		}

		public static Image SaveFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SaveFile", size, allowCache);
		}

		public static Image Search(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Search", size, allowCache);
		}

		public static Image SearchSmall(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SearchSmall", size, allowCache);
		}

		public static Image SelectColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SelectColumn", size, allowCache);
		}

		public static Image SelectPermissions(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SelectPermissions", size, allowCache);
		}

		public static Image SelectRow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SelectRow", size, allowCache);
		}

		public static Image Series(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Series", size, allowCache);
		}

		public static Image SetCulture(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SetCulture", size, allowCache);
		}

		public static Image Settings(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Settings", size, allowCache);
		}

		public static Image SetupPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SetupPage", size, allowCache);
		}

		public static Image Sort(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Sort", size, allowCache);
		}

		public static Image SortAndFilter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SortAndFilter", size, allowCache);
		}

		public static Image SortAZ(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SortAZ", size, allowCache);
		}

		public static Image SortZA(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SortZA", size, allowCache);
		}

		public static Image Swap(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Swap", size, allowCache);
		}

		public static Image SwapCells(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.SwapCells", size, allowCache);
		}

		public static Image Synchronize(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Synchronize", size, allowCache);
		}

		public static Image TextFormat(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.TextFormat", size, allowCache);
		}

		public static Image TopN(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.TopN", size, allowCache);
		}

		public static Image VerticalPlacement(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.VerticalPlacement", size, allowCache);
		}

		public static Image View(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.View", size, allowCache);
		}

		public static Image ViewData(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ViewData", size, allowCache);
		}

		public static Image ViewDesktop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ViewDesktop", size, allowCache);
		}

		public static Image ViewMobile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ViewMobile", size, allowCache);
		}

		public static Image Warning(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Warning", size, allowCache);
		}

		public static Image Zoom100(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.Zoom100", size, allowCache);
		}

		public static Image ZoomHeight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ZoomHeight", size, allowCache);
		}

		public static Image ZoomWidth(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ZoomWidth", size, allowCache);
		}

		public static Image ZoomSize(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Actions.ZoomSize", size, allowCache);
		}
	}

	public static class Check
	{
		public static Image NoErrors(StiImageSize size = StiImageSize.Double, bool allowCache = true)
		{
			return GetImage("Check.NoErrors", size, allowCache);
		}

		public static Image FirstPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Check.FirstPage", size, allowCache);
		}

		public static Image LastPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Check.LastPage", size, allowCache);
		}

		public static Image PrevPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Check.PrevPage", size, allowCache);
		}

		public static Image NextPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Check.NextPage", size, allowCache);
		}
	}

	public static class Components
	{
		public static Image BarCode(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiBarCode", size, allowCache);
		}

		public static Image Chart(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiChart", size, allowCache);
		}

		public static Image CheckBox(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCheckBox", size, allowCache);
		}

		public static Image ChildBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiChildBand", size, allowCache);
		}

		public static Image Clone(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiClone", size, allowCache);
		}

		public static Image ColumnFooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiColumnFooterBand", size, allowCache);
		}

		public static Image ColumnHeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiColumnHeaderBand", size, allowCache);
		}

		public static Image Component(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiComponent", size, allowCache);
		}

		public static Image Container(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiContainer", size, allowCache);
		}

		public static Image CrossDataBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossDataBand", size, allowCache);
		}

		public static Image CrossFooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossFooterBand", size, allowCache);
		}

		public static Image CrossGroupFooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossGroupFooterBand", size, allowCache);
		}

		public static Image CrossGroupHeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossGroupHeaderBand", size, allowCache);
		}

		public static Image CrossHeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossHeaderBand", size, allowCache);
		}

		public static Image CrossTab(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCrossTab", size, allowCache);
		}

		public static Image DataBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiDataBand", size, allowCache);
		}

		public static Image EmptyBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiEmptyBand", size, allowCache);
		}

		public static Image FooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiFooterBand", size, allowCache);
		}

		public static Image Gauge(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiGauge", size, allowCache);
		}

		public static Image GroupFooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiGroupFooterBand", size, allowCache);
		}

		public static Image GroupHeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiGroupHeaderBand", size, allowCache);
		}

		public static Image HeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiHeaderBand", size, allowCache);
		}

		public static Image HierarchicalBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiHierarchicalBand", size, allowCache);
		}

		public static Image HorizontalLinePrimitive(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiHorizontalLinePrimitive", size, allowCache);
		}

		public static Image Image(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiImage", size, allowCache);
		}

		public static Image Map(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiMap", size, allowCache);
		}

		public static Image OverlayBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiOverlayBand", size, allowCache);
		}

		public static Image Page(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPage", size, allowCache);
		}

		public static Image PageFooterBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPageFooterBand", size, allowCache);
		}

		public static Image PageHeaderBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPageHeaderBand", size, allowCache);
		}

		public static Image Panel(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPanel", size, allowCache);
		}

		public static Image RectanglePrimitive(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiRectanglePrimitive", size, allowCache);
		}

		public static Image Report(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiReport", size, allowCache);
		}

		public static Image ReportTitleBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiReportTitleBand", size, allowCache);
		}

		public static Image ReportSummaryBand(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiReportSummaryBand", size, allowCache);
		}

		public static Image RichText(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiRichText", size, allowCache);
		}

		public static Image TreeViewBox(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiRoundedRectanglePrimitive", size, allowCache);
		}

		public static Image Shape(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiShape", size, allowCache);
		}

		public static Image SubReport(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiSubReport", size, allowCache);
		}

		public static Image Table(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTable", size, allowCache);
		}

		public static Image Text(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiText", size, allowCache);
		}

		public static Image TextInCells(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTextInCells", size, allowCache);
		}

		public static Image WinControl(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiWinControl", size, allowCache);
		}

		public static Image VerticalLinePrimitive(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiVerticalLinePrimitive", size, allowCache);
		}

		public static Image ZipCode(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiZipCode", size, allowCache);
		}
	}

	public static class Controls
	{
		public static Image Form(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiForm", size, allowCache);
		}

		public static Image Button(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiButtonControl", size, allowCache);
		}
	}

	public static class ComponentUIs
	{
		public static Image Screen(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiScreen", size, allowCache);
		}
	}

	public static class Dashboards
	{
		public static Image Edit(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dashboards.Edit", size, allowCache);
		}

		public static Image FullScreen(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dashboards.FullScreen", size, allowCache);
		}

		public static Image Save(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dashboards.Save", size, allowCache);
		}

		public static Image Open(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dashboards.Open", size, allowCache);
		}

		public static Image Refresh(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dashboards.Refresh", size, allowCache);
		}
	}

	public static class Data
	{
		public static Image Csv(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Data.Csv", size, allowCache);
		}

		public static Image DBase(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Data.DBase", size, allowCache);
		}

		public static Image Excel(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Data.Excel", size, allowCache);
		}

		public static Image Json(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Data.Json", size, allowCache);
		}

		public static Image Xml(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Data.Xml", size, allowCache);
		}
	}

	public static class Dictionary
	{
		public static Image Favorites(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.Favorites", size, allowCache);
		}

		public static Image FavoritesYellow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.FavoritesYellow", size, allowCache);
		}

		public static Image Pin(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.Pin", size, allowCache);
		}

		public static Image Pinned(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.Pinned", size, allowCache);
		}

		public static Image NuGet(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.NuGet", size, allowCache);
		}

		public static Image BigNuGet(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.BigNuGet", size, allowCache);
		}

		public static Image RecentSave(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.RecentSave", size, allowCache);
		}

		public static Image SybaseAdsDatabase(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Dictionary.StiSybaseAdsDatabase", size, allowCache);
		}
	}

	public static class Editor
	{
		public static Image AlignBottom(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignBottom", size, allowCache);
		}

		public static Image AlignCenter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignCenter", size, allowCache);
		}

		public static Image AlignJustify(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignJustify", size, allowCache);
		}

		public static Image AlignLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignLeft", size, allowCache);
		}

		public static Image AlignMiddle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignMiddle", size, allowCache);
		}

		public static Image AlignRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignRight", size, allowCache);
		}

		public static Image AlignTop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.AlignTop", size, allowCache);
		}

		public static Image BorderAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderAll", size, allowCache);
		}

		public static Image BorderBottom(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderBottom", size, allowCache);
		}

		public static Image BorderLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderLeft", size, allowCache);
		}

		public static Image BorderNone(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderNone", size, allowCache);
		}

		public static Image BorderRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderRight", size, allowCache);
		}

		public static Image BorderShadow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderShadow", size, allowCache);
		}

		public static Image BorderTop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.BorderTop", size, allowCache);
		}

		public static Image Brush(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Brush", size, allowCache);
		}

		public static Image Bullets(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Bullets", size, allowCache);
		}

		public static Image Symbol(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Symbol", size, allowCache);
		}

		public static Image ClearContent(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ClearContent", size, allowCache);
		}

		public static Image ClipboardCopy(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ClipboardCopy", size, allowCache);
		}

		public static Image ClipboardCut(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ClipboardCut", size, allowCache);
		}

		public static Image ClipboardPaste(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ClipboardPaste", size, allowCache);
		}

		public static Image DockStyleBottom(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleBottom", size, allowCache);
		}

		public static Image DockStyleFill(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleFill", size, allowCache);
		}

		public static Image DockStyleLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleLeft", size, allowCache);
		}

		public static Image DockStyleNone(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleNone", size, allowCache);
		}

		public static Image DockStyleRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleRight", size, allowCache);
		}

		public static Image DockStyleTop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DockStyleTop", size, allowCache);
		}

		public static Image DropDown(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.DropDown", size, allowCache);
		}

		public static Image Fill(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Fill", size, allowCache);
		}

		public static Image Font(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Font", size, allowCache);
		}

		public static Image FontColor(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontColor", size, allowCache);
		}

		public static Image FontSizeDecrease(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontSizeDecrease", size, allowCache);
		}

		public static Image FontSizeIncrease(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontSizeIncrease", size, allowCache);
		}

		public static Image FontStyleBold(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontStyleBold", size, allowCache);
		}

		public static Image FontStyleItalic(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontStyleItalic", size, allowCache);
		}

		public static Image FontStyleStrikethrough(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontStyleStrikethrough", size, allowCache);
		}

		public static Image FontStyleUnderline(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontStyleUnderline", size, allowCache);
		}

		public static Image FontSubscript(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontSubscript", size, allowCache);
		}

		public static Image FontSuperscript(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.FontSuperscript", size, allowCache);
		}

		public static Image Function(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Function", size, allowCache);
		}

		public static Image IconAlignmentBottom(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentBottom", size, allowCache);
		}

		public static Image IconAlignmentCenter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentCenter", size, allowCache);
		}

		public static Image IconAlignmentLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentLeft", size, allowCache);
		}

		public static Image IconAlignmentNone(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentNone", size, allowCache);
		}

		public static Image IconAlignmentRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentRight", size, allowCache);
		}

		public static Image IconAlignmentTop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.IconAlignmentTop", size, allowCache);
		}

		public static Image LineSpacing(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.LineSpacing", size, allowCache);
		}

		public static Image PageMarginsNarrow(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.PageMarginsNarrow", size, allowCache);
		}

		public static Image PageMarginsNormal(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.PageMarginsNormal", size, allowCache);
		}

		public static Image PageMarginsWide(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.PageMarginsWide", size, allowCache);
		}

		public static Image Redo(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Redo", size, allowCache);
		}

		public static Image RotateText(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.RotateText", size, allowCache);
		}

		public static Image SelectAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SelectAll", size, allowCache);
		}

		public static Image ShowGrid(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ShowGrid", size, allowCache);
		}

		public static Image ShowHeaders(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ShowHeaders", size, allowCache);
		}

		public static Image ShowOrder(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ShowOrder", size, allowCache);
		}

		public static Image ShowQuickInfo(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ShowQuickInfo", size, allowCache);
		}

		public static Image ShowRulers(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.ShowRulers", size, allowCache);
		}

		public static Image SwitchAlignToGrid(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchAlignToGrid", size, allowCache);
		}

		public static Image SwitchGridMode(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchGridMode", size, allowCache);
		}

		public static Image SwitchGridModeToDots(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchGridModeToDots", size, allowCache);
		}

		public static Image SwitchGridModeToLines(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchGridModeToLines", size, allowCache);
		}

		public static Image SwitchToDesktop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchToDesktop", size, allowCache);
		}

		public static Image SwitchToPhone(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.SwitchToPhone", size, allowCache);
		}

		public static Image Undo(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.Undo", size, allowCache);
		}

		public static Image WrapText(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Editor.WrapText", size, allowCache);
		}
	}

	public static class Files
	{
		public static Image DashboardCloudFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Files.DashboardCloudFile", size, allowCache);
		}

		public static Image DashboardFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Files.DashboardFile", size, allowCache);
		}

		public static Image ReportCloudFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Files.ReportCloudFile", size, allowCache);
		}

		public static Image ReportFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Files.ReportFile", size, allowCache);
		}
	}

	public static class Elements
	{
		public static Image Cards(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiCardsElement", size, allowCache);
		}

		public static Image Chart(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiChartElement", size, allowCache);
		}

		public static Image ComboBox(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiComboBoxElement", size, allowCache);
		}

		public static Image Dashboard(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiDashboard", size, allowCache);
		}

		public static Image DatePicker(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiDatePickerElement", size, allowCache);
		}

		public static Image Filter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiFilterElement", size, allowCache);
		}

		public static Image Gauge(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiGaugeElement", size, allowCache);
		}

		public static Image Image(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiImageElement", size, allowCache);
		}

		public static Image Indicator(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiIndicatorElement", size, allowCache);
		}

		public static Image ListBox(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiListBoxElement", size, allowCache);
		}

		public static Image Map(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiMapElement", size, allowCache);
		}

		public static Image OnlineMap(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiOnlineMapElement", size, allowCache);
		}

		public static Image Panel(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPanelElement", size, allowCache);
		}

		public static Image PivotTable(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiPivotTableElement", size, allowCache);
		}

		public static Image Progress(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiProgressElement", size, allowCache);
		}

		public static Image RegionMap(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiRegionMapElement", size, allowCache);
		}

		public static Image Shape(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiShapeElement", size, allowCache);
		}

		public static Image Table(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTableElement", size, allowCache);
		}

		public static Image Text(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTextElement", size, allowCache);
		}

		public static Image TreeView(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTreeViewElement", size, allowCache);
		}

		public static Image TreeViewBox(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetToolboxImage("StiTreeViewBoxElement", size, allowCache);
		}
	}

	public static class Format
	{
		public static Image Boolean(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Boolean", size, allowCache);
		}

		public static Image Currency(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Currency", size, allowCache);
		}

		public static Image Date(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Date", size, allowCache);
		}

		public static Image DateAndTime(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.DateAndTime", size, allowCache);
		}

		public static Image General(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.General", size, allowCache);
		}

		public static Image Number(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Number", size, allowCache);
		}

		public static Image Percentage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Percentage", size, allowCache);
		}

		public static Image Time(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Formats.Time", size, allowCache);
		}
	}

	public static class Gauge
	{
		public static Image HalfDonuts(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Gauge.HalfDonuts", size, allowCache);
		}

		public static Image VerticalLinear(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Gauge.VerticalLinear", size, allowCache);
		}

		public static Image HorizontalLinear(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Gauge.HorizontalLinear", size, allowCache);
		}

		public static Image RadialBar(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Gauge.RadialBar", size, allowCache);
		}

		public static Image Bullet(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Gauge.Bullet", size, allowCache);
		}
	}

	public static class Guid
	{
		public static Image NewCulture(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetGuidImage("Guid.NewCulture", size, allowCache);
		}

		public static Image NewData(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetGuidImage("Guid.NewData", size, allowCache);
		}

		public static Image NewStyle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetGuidImage("Guid.NewStyle", size, allowCache);
		}
	}

	public static class Layout
	{
		public static Image AlignToGrid(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.AlignToGrid", size, allowCache);
		}

		public static Image BringToFront(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.BringToFront", size, allowCache);
		}

		public static Image CenterHorizontally(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.CenterHorizontally", size, allowCache);
		}

		public static Image CenterVertically(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.CenterVertically", size, allowCache);
		}

		public static Image Columns(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Columns", size, allowCache);
		}

		public static Image ColumnsOne(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ColumnsOne", size, allowCache);
		}

		public static Image ColumnsTwo(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ColumnsTwo", size, allowCache);
		}

		public static Image ColumnsThree(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ColumnsThree", size, allowCache);
		}

		public static Image ContentAlignLeft(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignLeft", size, allowCache);
		}

		public static Image ContentAlignCenter(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignCenter", size, allowCache);
		}

		public static Image ContentAlignRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignRight", size, allowCache);
		}

		public static Image ContentAlignTop(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignTop", size, allowCache);
		}

		public static Image ContentAlignMiddle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignMiddle", size, allowCache);
		}

		public static Image ContentAlignBottom(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.ContentAlignBottom", size, allowCache);
		}

		public static Image Link(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Link", size, allowCache);
		}

		public static Image Lock(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Lock", size, allowCache);
		}

		public static Image LockGray(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.LockGray", size, allowCache);
		}

		public static Image MakeHorizontalSpacingEqual(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MakeHorizontalSpacingEqual", size, allowCache);
		}

		public static Image MakeSameHeightAs(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MakeSameHeightAs", size, allowCache);
		}

		public static Image MakeSameSizeAs(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MakeSameSizeAs", size, allowCache);
		}

		public static Image MakeSameWidthAs(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MakeSameWidthAs", size, allowCache);
		}

		public static Image Margins(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Margins", size, allowCache);
		}

		public static Image MakeVerticalSpacingEqual(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MakeVerticalSpacingEqual", size, allowCache);
		}

		public static Image MoveForward(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MoveForward", size, allowCache);
		}

		public static Image MoveBackward(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.MoveBackward", size, allowCache);
		}

		public static Image Orientation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Orientation", size, allowCache);
		}

		public static Image OrientationLandscape(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.OrientationLandscape", size, allowCache);
		}

		public static Image OrientationPortrait(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.OrientationPortrait", size, allowCache);
		}

		public static Image SendToBack(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.SendToBack", size, allowCache);
		}

		public static Image Size(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Size", size, allowCache);
		}

		public static Image Watermark(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.Watermark", size, allowCache);
		}

		public static Image UnlockGray(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Layout.UnlockGray", size, allowCache);
		}
	}

	public static class Progress
	{
		public static Image Circle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Progress.Circle", size, allowCache);
		}

		public static Image DataBars(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Progress.DataBars", size, allowCache);
		}

		public static Image Pie(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Progress.Pie", size, allowCache);
		}
	}

	public static class QuickButtons
	{
		public static Image Band(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Band", size, allowCache);
		}

		public static Image Band_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Band_Selected", size, allowCache);
		}

		public static Image DataRelation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.DataRelation", size, allowCache);
		}

		public static Image DataRelation_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.DataRelation_Selected", size, allowCache);
		}

		public static Image DataSource(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.DataSource", size, allowCache);
		}

		public static Image DataSource_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.DataSource_Selected", size, allowCache);
		}

		public static Image Field(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Field", size, allowCache);
		}

		public static Image Field_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Field_Selected", size, allowCache);
		}

		public static Image MasterComponent(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.MasterComponent", size, allowCache);
		}

		public static Image MasterComponent_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.MasterComponent_Selected", size, allowCache);
		}

		public static Image SelectAll(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.SelectAll", size, allowCache);
		}

		public static Image SelectAll_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.SelectAll_Selected", size, allowCache);
		}

		public static Image SystemText(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.SystemText", size, allowCache);
		}

		public static Image SystemText_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.SystemText_Selected", size, allowCache);
		}

		public static Image Text(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Text", size, allowCache);
		}

		public static Image Text_Selected(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("QuickButtons.Text_Selected", size, allowCache);
		}
	}

	public static class RestoreDefault
	{
		public static Image Blockly(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.Blockly", size, allowCache);
		}

		public static Image DataAdapters(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.DataAdapters", size, allowCache);
		}

		public static Image DesignerSettings(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.DesignerSettings", size, allowCache);
		}

		public static Image GisMap(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.GisMap", size, allowCache);
		}

		public static Image SchedulerLogs(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.SchedulerLogs", size, allowCache);
		}

		public static Image Thumbnails(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.Thumbnails", size, allowCache);
		}

		public static Image WindowsSettings(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("RestoreDefault.WindowsSettings", size, allowCache);
		}
	}

	public static class Schedule
	{
		public static Image Daily(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.Daily", size, allowCache);
		}

		public static Image Hourly(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.Hourly", size, allowCache);
		}

		public static Image Monthly(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.Monthly", size, allowCache);
		}

		public static Image Once(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.Once", size, allowCache);
		}

		public static Image Weekly(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.Weekly", size, allowCache);
		}

		public static Image New(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Schedule.New", size, allowCache);
		}
	}

	public static class SystemParts
	{
		public static Image Back(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("SystemParts.Back", size, allowCache);
		}

		public static Image DropDown(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("SystemParts.DropDown", size, allowCache);
		}

		public static Image Remove(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("SystemParts.Remove", size, allowCache);
		}
	}

	public static class Table
	{
		public static Image DataBars(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.DataBars", size, allowCache);
		}

		public static Image ColorScale(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.ColorScale", size, allowCache);
		}

		public static Image Indicator(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.Indicator", size, allowCache);
		}

		public static Image Bubble(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.Bubble", size, allowCache);
		}

		public static Image Sparklines(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.Sparklines", size, allowCache);
		}

		public static Image SparklinesArea(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.SparklinesArea", size, allowCache);
		}

		public static Image SparklinesColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.SparklinesColumn", size, allowCache);
		}

		public static Image SparklinesWinLoss(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Table.SparklinesWinLoss", size, allowCache);
		}
	}

	public static class Tasks
	{
		public static Image ChartCommon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartCommon", size, allowCache);
		}

		public static Image ChartConditions(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartConditions", size, allowCache);
		}

		public static Image ChartConstantLines(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartConstantLines", size, allowCache);
		}

		public static Image ChartFilters(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartFilters", size, allowCache);
		}

		public static Image ChartGridLinesHorizontal(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartGridLinesHorizontal", size, allowCache);
		}

		public static Image ChartGridLinesHorizontalRight(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartGridLinesHorizontalRight", size, allowCache);
		}

		public static Image ChartGridLinesVertical(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartGridLinesVertical", size, allowCache);
		}

		public static Image ChartInteraction(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartInteraction", size, allowCache);
		}

		public static Image ChartInterlacingHorizontal(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartInterlacingHorizontal", size, allowCache);
		}

		public static Image ChartInterlacingVertical(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartInterlacingVertical", size, allowCache);
		}

		public static Image ChartLineMarker(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartLineMarker", size, allowCache);
		}

		public static Image ChartLegend(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartLegend", size, allowCache);
		}

		public static Image ChartMarker(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartMarker", size, allowCache);
		}

		public static Image ChartRadarGridLinesHorizontal(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarGridLinesHorizontal", size, allowCache);
		}

		public static Image ChartRadarGridLinesVertical(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarGridLinesVertical", size, allowCache);
		}

		public static Image ChartRadarInterlacingHorizontal(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarInterlacingHorizontal", size, allowCache);
		}

		public static Image ChartRadarInterlacingVertical(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarInterlacingVertical", size, allowCache);
		}

		public static Image ChartRadarXAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarXAxis", size, allowCache);
		}

		public static Image ChartRadarYAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartRadarYAxis", size, allowCache);
		}

		public static Image ChartSeriesCommon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartSeriesCommon", size, allowCache);
		}

		public static Image ChartSeriesLabels(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartSeriesLabels", size, allowCache);
		}

		public static Image ChartSeriesLabelsCommon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartSeriesLabelsCommon", size, allowCache);
		}

		public static Image ChartStrips(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartStrips", size, allowCache);
		}

		public static Image ChartTable(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartTable", size, allowCache);
		}

		public static Image ChartTitle(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartTitle", size, allowCache);
		}

		public static Image ChartTopN(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartTopN", size, allowCache);
		}

		public static Image ChartTrendLine(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartTrendLine", size, allowCache);
		}

		public static Image ChartXAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartXAxis", size, allowCache);
		}

		public static Image ChartXTopAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartXTopAxis", size, allowCache);
		}

		public static Image ChartYAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartYAxis", size, allowCache);
		}

		public static Image ChartYRightAxis(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ChartYRightAxis", size, allowCache);
		}

		public static Image Dashboard(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.Dashboard", size, allowCache);
		}

		public static Image DataDataSource(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DataDataSource", size, allowCache);
		}

		public static Image DataFilters(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DataFilters", size, allowCache);
		}

		public static Image DataMasterComponent(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DataMasterComponent", size, allowCache);
		}

		public static Image DataRelation(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DataRelation", size, allowCache);
		}

		public static Image DataSort(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DataSort", size, allowCache);
		}

		public static Image DesignerOptionAutoSave(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DesignerOptionAutoSave", size, allowCache);
		}

		public static Image DesignerOptionGrid(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DesignerOptionGrid", size, allowCache);
		}

		public static Image DesignerOptionMain(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DesignerOptionMain", size, allowCache);
		}

		public static Image DesignerOptionQuickInfo(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.DesignerOptionQuickInfo", size, allowCache);
		}

		public static Image GroupDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.GroupDataColumn", size, allowCache);
		}

		public static Image GroupExpression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.GroupExpression", size, allowCache);
		}

		public static Image GroupSummary(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.GroupSummary", size, allowCache);
		}

		public static Image ImageDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageDataColumn", size, allowCache);
		}

		public static Image ImageExpression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageExpression", size, allowCache);
		}

		public static Image ImageFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageFile", size, allowCache);
		}

		public static Image ImageHyperlink(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageHyperlink", size, allowCache);
		}

		public static Image ImageIcon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageIcon", size, allowCache);
		}

		public static Image ImageImage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ImageImage", size, allowCache);
		}

		public static Image InteractionCommon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.InteractionCommon", size, allowCache);
		}

		public static Image InteractionDrillDown(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.InteractionDrillDown", size, allowCache);
		}

		public static Image InteractionDrillDownParameters(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.InteractionDrillDownParameters", size, allowCache);
		}

		public static Image InteractionSort(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.InteractionSort", size, allowCache);
		}

		public static Image MapChoropleth(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.MapChoropleth", size, allowCache);
		}

		public static Image MapOnline(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.MapOnline", size, allowCache);
		}

		public static Image PageOptionColumns(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.PageOptionColumns", size, allowCache);
		}

		public static Image PageOptionPaper(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.PageOptionPaper", size, allowCache);
		}

		public static Image PageOptionWatermark(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.PageOptionWatermark", size, allowCache);
		}

		public static Image Report(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.Report", size, allowCache);
		}

		public static Image ReportOptionDescription(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ReportOptionDescription", size, allowCache);
		}

		public static Image ReportOptionMain(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.ReportOptionMain", size, allowCache);
		}

		public static Image RichTextDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.RichTextDataColumn", size, allowCache);
		}

		public static Image RichTextFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.RichTextFile", size, allowCache);
		}

		public static Image RichTextHyperlink(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.RichTextHyperlink", size, allowCache);
		}

		public static Image RichTextExpression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.RichTextExpression", size, allowCache);
		}

		public static Image SubReportFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.SubReportFile", size, allowCache);
		}

		public static Image SubReportHyperlink(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.SubReportHyperlink", size, allowCache);
		}

		public static Image SubReportPage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.SubReportPage", size, allowCache);
		}

		public static Image SubReportParameters(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.SubReportParameters", size, allowCache);
		}

		public static Image TextDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.TextDataColumn", size, allowCache);
		}

		public static Image TextExpression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.TextExpression", size, allowCache);
		}

		public static Image TextSummary(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.TextSummary", size, allowCache);
		}

		public static Image TextSystemVariable(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks.TextSystemVariable", size, allowCache);
		}
	}

	public static class Tasks2
	{
		public static Image ImageDataColumn(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageDataColumn", size, allowCache);
		}

		public static Image ImageExpression(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageExpression", size, allowCache);
		}

		public static Image ImageFile(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageFile", size, allowCache);
		}

		public static Image ImageHyperlink(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageHyperlink", size, allowCache);
		}

		public static Image ImageIcon(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageIcon", size, allowCache);
		}

		public static Image ImageImage(StiImageSize size = StiImageSize.Normal, bool allowCache = true)
		{
			return GetImage("Tasks2.ImageImage", size, allowCache);
		}
	}

	private const string RootPath = "Stimulsoft.Report.Design.Images";

	private const string ToolBoxRootPath = "Stimulsoft.Report.Design.Resources.Office2013.ToolBox";

	private static Hashtable mainCache = new Hashtable();

	private static Hashtable toolBoxCache = new Hashtable();

	private static Hashtable guidCache = new Hashtable();

	public static Image GetImage(string path, StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		Bitmap val = (Bitmap)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null && !IsDisposedImage((Image)(object)val))
		{
			return (Image)(object)val;
		}
		Assembly assembly = typeof(StiReportDesignImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Design.Images." + path, size);
		if (allowCache)
		{
			mainCache[path + size] = image;
		}
		return image;
	}

	public static Image GetToolboxImage(string path, StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		Image val = (Image)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null)
		{
			return val;
		}
		Assembly assembly = typeof(StiReportDesignImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + path, size);
		if (allowCache)
		{
			toolBoxCache[path + size] = image;
		}
		return image;
	}

	public static Image GetGuidImage(string path, StiImageSize size = StiImageSize.Normal, bool allowCache = true)
	{
		Image val = (Image)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null)
		{
			return val;
		}
		Assembly assembly = typeof(StiReportDesignImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Design.Images." + path, size);
		if (allowCache)
		{
			guidCache[path] = image;
		}
		return image;
	}

	private static bool IsDisposedImage(Image image)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)image.PixelFormat == 0)
			{
				return true;
			}
			return image.Width == 0;
		}
		catch
		{
			return true;
		}
	}
}
