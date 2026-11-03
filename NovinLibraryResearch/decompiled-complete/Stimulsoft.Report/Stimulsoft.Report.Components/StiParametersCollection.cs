using System;
using System.Collections;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Components;

public class StiParametersCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiParameter this[int index]
	{
		get
		{
			return base.List[index] as StiParameter;
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiParameter this[string name]
	{
		get
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			lock (base.List.SyncRoot)
			{
				foreach (StiParameter item in base.List)
				{
					if (item.Name.ToLowerInvariant() == name)
					{
						return item;
					}
				}
			}
			return null;
		}
		set
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			for (int i = 0; i < base.List.Count; i++)
			{
				StiParameter stiParameter = base.List[i] as StiParameter;
				if (stiParameter.Name.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			AddCore(value);
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
		foreach (StiParameter item in base.List)
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
			StiParameter stiParameter = new StiParameter();
			stiParameter.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiParameter);
		}
	}

	public object Clone()
	{
		StiParametersCollection stiParametersCollection = new StiParametersCollection();
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiParameter stiParameter = (StiParameter)enumerator.Current;
					stiParametersCollection.Add(new StiParameter
					{
						Name = stiParameter.Name,
						Expression = (stiParameter.Expression.Clone() as StiExpression)
					});
				}
				return stiParametersCollection;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	private void AddCore(StiParameter parameter)
	{
		base.List.Add(parameter);
	}

	public void Add(StiParameter parameter)
	{
		AddCore(parameter);
	}

	public void AddRange(StiParametersCollection parameters)
	{
		lock (((ICollection)parameters).SyncRoot)
		{
			foreach (StiParameter parameter in parameters)
			{
				Add(parameter);
			}
		}
	}

	public void AddRange(StiParameter[] parameters)
	{
		lock (((ICollection)parameters).SyncRoot)
		{
			foreach (StiParameter parameter in parameters)
			{
				Add(parameter);
			}
		}
	}

	public bool Contains(StiParameter parameter)
	{
		return base.List.Contains(parameter);
	}

	public int IndexOf(StiParameter parameter)
	{
		return base.List.IndexOf(parameter);
	}

	public int IndexOf(string name)
	{
		name = name.ToLower(CultureInfo.InvariantCulture);
		int num = 0;
		lock (base.List.SyncRoot)
		{
			foreach (StiParameter item in base.List)
			{
				if (item.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	public void InsertRange(int index, StiParametersCollection parameters)
	{
		lock (((ICollection)parameters).SyncRoot)
		{
			foreach (StiParameter parameter in parameters)
			{
				Insert(index, parameter);
			}
		}
	}

	public void Insert(int index, StiParameter parameter)
	{
		base.List.Insert(index, parameter);
	}

	public void Remove(StiParametersCollection parameters)
	{
		lock (((ICollection)parameters).SyncRoot)
		{
			foreach (StiParameter parameter in parameters)
			{
				Remove(parameter);
			}
		}
	}

	public void Remove(StiParameter parameter)
	{
		if (base.List.Contains(parameter))
		{
			base.List.Remove(parameter);
		}
	}

	public void CopyTo(Array array, int index)
	{
		base.List.CopyTo(array, index);
	}
}
