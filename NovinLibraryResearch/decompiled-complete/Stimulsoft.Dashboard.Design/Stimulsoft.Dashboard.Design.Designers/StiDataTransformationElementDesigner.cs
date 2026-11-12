using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.DataTransformation;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiDataTransformationElementDesigner : StiElementDesigner
{
	private StiReport report;

	private IStiElement element;

	private StiDataSource dataSource;

	private IStiTransformFilters filterCase;

	private IStiTransformSorts sortCase;

	private IStiTransformActions actionCase;

	private StiDataTransformation transformation;

	protected override StiInplaceEditorForm CreateEditorForm(object comp)
	{
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		StiComponent stiComponent = comp as StiComponent;
		IStiDesignerBase designer = stiComponent.Report.Designer;
		element = comp as StiElement;
		filterCase = element as IStiTransformFilters;
		sortCase = element as IStiTransformSorts;
		actionCase = element as IStiTransformActions;
		report = new StiReport();
		report.Designer = designer;
		StiDataTable table = StiElementDataCache.Create(element, StiDataRequestOption.DisallowTransform);
		DataTable dataTable = table.ToNetTable();
		dataTable.TableName = "Source";
		report.RegData("Source", dataTable);
		report.Dictionary.Synchronize();
		stiComponent.Report.Dictionary.Variables.ToList().ForEach(delegate(StiVariable v)
		{
			report.Dictionary.Variables.Add(v.Clone() as StiVariable);
		});
		dataSource = report.Dictionary.DataSources[0];
		List<StiDataColumn> dataColumns = dataSource.Columns.ToList();
		dataColumns.ForEach(delegate(StiDataColumn c)
		{
			c.Key = StiKeyHelper.GetOrGeneratedKey(c.Key);
		});
		transformation = new StiDataTransformation
		{
			NameInSource = "Source",
			Name = "Source"
		};
		if (filterCase?.TransformFilters != null)
		{
			transformation.Filters = filterCase?.TransformFilters;
			transformation.Filters.ForEach(delegate(StiDataFilterRule f)
			{
				f.Key = dataColumns.FirstOrDefault((StiDataColumn c) => c.Name == f.Path)?.Key;
				if (f.Path != null && !f.Path.StartsWith("Source."))
				{
					f.Path = "Source." + f.Path;
				}
			});
		}
		if (sortCase?.TransformSorts != null)
		{
			transformation.Sorts = sortCase?.TransformSorts;
			transformation.Sorts.ForEach(delegate(StiDataSortRule f)
			{
				f.Key = dataColumns.FirstOrDefault((StiDataColumn c) => c.Name == f.Key)?.Key;
			});
		}
		if (actionCase?.TransformActions != null)
		{
			transformation.Actions = actionCase?.TransformActions;
			transformation.Actions.ForEach(delegate(StiDataActionRule a)
			{
				a.Key = dataColumns.FirstOrDefault((StiDataColumn c) => c.Name == a.Path)?.Key;
				if (a.Path != null && !a.Path.StartsWith("Source."))
				{
					a.Path = "Source." + a.Path;
				}
			});
		}
		report.Dictionary.DataSources.Add(transformation);
		int columnIndex = 0;
		StiDataTransformationColumn[] source = dataColumns.Select(StiDataTransformationEditorForm.CreateTransformationColumnFromDataColumn).ToArray();
		source.ToList().ForEach(delegate(StiDataTransformationColumn column)
		{
			column.Name = StiNameCreation.CreateColumnName(transformation, column.Name);
			column.Expression = StiExpressionHelper.ReplaceFunction(column.Expression, "All");
			column.Type = typeof(object);
			column.Mode = StiDataTransformationMode.Dimension;
			column.Key = dataSource.Columns[columnIndex].Key;
			transformation.Columns.Add(column);
			columnIndex++;
		});
		StiDataTransformationRestrictions stiDataTransformationRestrictions = StiDataTransformationRestrictions.Priority;
		if (filterCase == null)
		{
			stiDataTransformationRestrictions |= StiDataTransformationRestrictions.Filters;
		}
		if (sortCase == null)
		{
			stiDataTransformationRestrictions |= StiDataTransformationRestrictions.Sorts;
		}
		if (actionCase == null)
		{
			stiDataTransformationRestrictions |= StiDataTransformationRestrictions.Actions;
		}
		StiDataTransformationEditorForm stiDataTransformationEditorForm = new StiDataTransformationEditorForm(transformation, isElementTransformation: true, stiDataTransformationRestrictions, activateDictionary: false);
		((Form)stiDataTransformationEditorForm).FormClosed += new FormClosedEventHandler(Form_FormClosed);
		return stiDataTransformationEditorForm;
	}

	private void Form_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (report == null)
		{
			return;
		}
		if (filterCase != null)
		{
			filterCase.TransformFilters = transformation.Filters;
			filterCase.TransformFilters.ForEach(delegate(StiDataFilterRule f)
			{
				f.Key = null;
				if (f.Path.StartsWith("Source."))
				{
					f.Path = f.Path.Replace("Source.", "");
				}
			});
		}
		if (sortCase != null)
		{
			List<StiDataColumn> columns = dataSource.Columns.ToList();
			sortCase.TransformSorts = transformation.Sorts;
			sortCase.TransformSorts.ForEach(delegate(StiDataSortRule s)
			{
				s.Key = columns.FirstOrDefault((StiDataColumn c) => c.Key == s.Key)?.Name;
			});
		}
		if (actionCase != null)
		{
			actionCase.TransformActions = transformation.Actions;
			actionCase.TransformActions.ForEach(delegate(StiDataActionRule a)
			{
				a.Key = null;
				if (a.Path.StartsWith("Source."))
				{
					a.Path = a.Path.Replace("Source.", "");
				}
				a.Priority = StiDataActionPriority.AfterGroupingData;
			});
		}
		report.Dispose();
		report = null;
	}

	public StiDataTransformationElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
