using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

public class StiTableViewLinkCell : DataGridViewLinkCell
{
	private StiTableColumn column;

	internal StiTableColumn Column
	{
		get
		{
			return column;
		}
		set
		{
			column = value;
			if (column != null)
			{
				((DataGridViewLinkCell)this).LinkColor = ((column.ForeColor != Color.Transparent) ? column.ForeColor : StiColor.Get("165dc8"));
			}
		}
	}

	protected override void OnContentClick(DataGridViewCellEventArgs e)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		((DataGridViewCell)this).OnContentClick(e);
		try
		{
			StiTableViewControl stiTableViewControl = ((DataGridViewElement)((DataGridViewCell)this).OwningColumn).DataGridView as StiTableViewControl;
			StiDimensionColumn stiDimensionColumn = stiTableViewControl.GridColumnToTableColumn[((DataGridViewCell)this).OwningColumn] as StiDimensionColumn;
			DataGridViewRow val = ((DataGridViewElement)this).DataGridView.Rows[e.RowIndex];
			string value = val.Cells[e.ColumnIndex].Value?.ToString();
			string text = "";
			if (!string.IsNullOrWhiteSpace(stiDimensionColumn.HyperlinkPattern))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Value", value);
				int num = 0;
				foreach (DataGridViewCell item in (BaseCollection)val.Cells)
				{
					DataGridViewCell val2 = item;
					string key = "Row." + StiCodeDomSerializator.ReplaceSymbols(val2.OwningColumn.Name);
					dictionary.Add(key, val2.Value?.ToString());
					num++;
				}
				text = StiReportParser.Parse(stiDimensionColumn.HyperlinkPattern, stiTableViewControl.Element, allowCache: false, dictionary);
			}
			if (!string.IsNullOrWhiteSpace(text) && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(stiTableViewControl.Element, text))
			{
				StiProcess.Start(text);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public StiTableViewLinkCell()
		: this(null)
	{
	}

	public StiTableViewLinkCell(StiTableColumn column)
	{
		Column = column;
		((DataGridViewLinkCell)this).LinkBehavior = (LinkBehavior)1;
	}
}
