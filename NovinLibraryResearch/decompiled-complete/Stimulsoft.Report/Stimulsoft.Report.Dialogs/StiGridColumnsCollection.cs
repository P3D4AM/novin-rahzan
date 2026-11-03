using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dialogs;

public class StiGridColumnsCollection : CollectionBase, IStiJsonReportObject
{
	private StiGridControl gridControl;

	public StiGridColumn this[int index]
	{
		get
		{
			return (StiGridColumn)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiGridControl GridControl => gridControl;

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiGridColumn item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiGridColumn stiGridColumn = new StiGridColumn();
			stiGridColumn.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiGridColumn);
		}
	}

	protected override void OnInsert(int index, object value)
	{
		((StiGridColumn)value).GridColumnCollection = this;
	}

	public void Add(StiGridColumn column)
	{
		base.List.Add(column);
	}

	public bool Contains(StiGridColumn column)
	{
		return base.List.Contains(column);
	}

	public int IndexOf(StiGridColumn column)
	{
		return base.List.IndexOf(column);
	}

	public void Insert(int index, StiGridColumn column)
	{
		base.List.Insert(index, column);
	}

	public void Remove(StiGridColumn column)
	{
		base.List.Remove(column);
	}

	public StiGridColumnsCollection(StiGridControl gridControl)
	{
		this.gridControl = gridControl;
	}

	public StiGridColumnsCollection()
		: this(null)
	{
	}
}
