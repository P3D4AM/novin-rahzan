using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
internal sealed class StiAggregateFunctionsDataGridViewComboBoxColumn : DataGridViewComboBoxColumn
{
	public StiAggregateFunctionsDataGridViewComboBoxColumn()
	{
		((DataGridViewComboBoxColumn)this).MaxDropDownItems = 20;
		List<string> list = new List<string> { StiLocalization.Get("Wizards", "NoFunction") };
		foreach (StiAggregateFunctionService item in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled))
		{
			list.Add(item.ServiceName);
		}
		((DataGridViewComboBoxColumn)this).DataSource = list;
		((DataGridViewColumn)this).ValueType = typeof(string);
	}
}
