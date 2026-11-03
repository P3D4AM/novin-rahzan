using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDataParametersCollection : CollectionBase, IStiJsonReportObject
{
	private Hashtable CachedDataParameters = new Hashtable();

	private StiDataSource dataSource;

	public StiDataParameter this[int index]
	{
		get
		{
			return (StiDataParameter)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiDataParameter this[string name]
	{
		get
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			if (CachedDataParameters[name] is StiDataParameter result)
			{
				return result;
			}
			foreach (StiDataParameter item in base.List)
			{
				if (item.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					CachedDataParameters[name] = item;
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			for (int i = 0; i < base.List.Count; i++)
			{
				StiDataParameter stiDataParameter = base.List[i] as StiDataParameter;
				if (stiDataParameter.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

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
		foreach (StiDataParameter item in base.List)
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
			StiDataParameter stiDataParameter = new StiDataParameter();
			stiDataParameter.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiDataParameter);
		}
	}

	public List<StiDataParameter> ToList()
	{
		return this.Cast<StiDataParameter>().ToList();
	}

	protected override void OnInsert(int index, object value)
	{
		if (((StiDataParameter)value).DataSource == null)
		{
			((StiDataParameter)value).DataSource = dataSource;
		}
		if (((StiDataParameter)value).dataParametersCollection == null)
		{
			((StiDataParameter)value).dataParametersCollection = this;
		}
	}

	public void Add(StiDataParameter parameter)
	{
		base.List.Add(parameter);
	}

	public void AddRange(StiDataParametersCollection parameters)
	{
		foreach (StiDataParameter parameter in parameters)
		{
			Add(parameter);
		}
	}

	public void AddRange(StiDataParameter[] parameters)
	{
		foreach (StiDataParameter parameter in parameters)
		{
			Add(parameter);
		}
	}

	public bool Contains(StiDataParameter parameter)
	{
		return base.List.Contains(parameter);
	}

	public bool Contains(string name)
	{
		return base.List.Cast<StiDataParameter>().Any((StiDataParameter parameter) => parameter.Name == name);
	}

	public int IndexOf(StiDataParameter parameter)
	{
		return base.List.IndexOf(parameter);
	}

	public void Insert(int index, StiDataParameter parameter)
	{
		base.List.Insert(index, parameter);
	}

	public void Remove(StiDataParameter parameter)
	{
		base.List.Remove(parameter);
	}

	public StiDataParametersCollection()
		: this(null)
	{
	}

	public StiDataParametersCollection(StiDataSource dataSource)
	{
		this.dataSource = dataSource;
	}
}
