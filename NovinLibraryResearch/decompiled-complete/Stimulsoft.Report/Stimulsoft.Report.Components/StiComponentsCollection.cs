using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Components;

public class StiComponentsCollection : CollectionBase, IStiStateSaveRestore, ICloneable, IStiJsonReportObject
{
	private StiContainer parent;

	public StiComponent this[int index]
	{
		get
		{
			return base.List[index] as StiComponent;
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiComponent this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			lock (base.List.SyncRoot)
			{
				foreach (StiComponent item in base.List)
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
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				StiComponent stiComponent = base.List[i] as StiComponent;
				if (stiComponent.Name.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			AddCore(value);
		}
	}

	public event EventHandler ComponentAdded;

	public event EventHandler ComponentRemoved;

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
		foreach (StiComponent item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiComponent stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent x) => ((object)x).GetType().Name == ident);
			if (stiComponent == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiComponent stiComponent2 = stiComponent.CreateNew();
			Add(stiComponent2);
			stiComponent2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiComponent stiComponent = (StiComponent)enumerator.Current;
					stiComponentsCollection.Add(((StiService)stiComponent).Clone() as StiComponent);
				}
				return stiComponentsCollection;
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

	internal StiComponent GetComponentByName(string componentName, StiContainer container)
	{
		lock (((ICollection)container.Components).SyncRoot)
		{
			foreach (StiComponent component in container.Components)
			{
				if (component.Name == componentName)
				{
					return component;
				}
				if (component is StiContainer container2)
				{
					StiComponent componentByName = GetComponentByName(componentName, container2);
					if (componentByName != null)
					{
						return componentByName;
					}
				}
			}
		}
		return null;
	}

	internal StiComponent GetComponentByGuid(string guid, StiContainer container)
	{
		lock (((ICollection)container.Components).SyncRoot)
		{
			foreach (StiComponent component in container.Components)
			{
				if (component.Guid == guid)
				{
					return component;
				}
				if (component is StiContainer container2)
				{
					StiComponent componentByGuid = GetComponentByGuid(guid, container2);
					if (componentByGuid != null)
					{
						return componentByGuid;
					}
				}
			}
		}
		return null;
	}

	public StiPage GetPageByAlias(string alias)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					if (stiPage.Alias == alias)
					{
						return stiPage;
					}
				}
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
		return null;
	}

	public void SetParent(StiContainer parent)
	{
		this.parent = parent;
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiComponent stiComponent = (StiComponent)enumerator.Current;
					stiComponent.Parent = parent;
					if (stiComponent is StiContainer stiContainer)
					{
						stiContainer.Components.SetParent(stiContainer);
					}
				}
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

	public List<StiComponent> ToList()
	{
		return this.Cast<object>().Cast<StiComponent>().ToList();
	}

	private void AddCore(StiComponent component)
	{
		if (parent != null)
		{
			component.Parent = parent;
			if (parent.Page != null)
			{
				component.Page = parent.Page;
				if (string.IsNullOrEmpty(component.Name))
				{
					if (component.Report != null && component.Report.IsDesigning)
					{
						component.Name = StiNameCreation.CreateName(parent.Report, StiNameCreation.GenerateName(component));
					}
					else
					{
						component.Name = StiNameCreation.CreateSimpleName(parent.Report, StiNameCreation.GenerateName(component));
					}
				}
			}
		}
		base.List.Add(component);
	}

	public void Add(StiComponent component)
	{
		AddCore(component);
		InvokeComponentAdded(component, EventArgs.Empty);
	}

	public void AddRange(StiComponentsCollection components)
	{
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent component in components)
			{
				Add(component);
			}
		}
	}

	public void AddRange(StiComponent[] components)
	{
		lock (components.SyncRoot)
		{
			foreach (StiComponent component in components)
			{
				Add(component);
			}
		}
	}

	public bool Contains(StiComponent component)
	{
		return base.List.Contains(component);
	}

	public int IndexOf(StiComponent component)
	{
		return base.List.IndexOf(component);
	}

	public int IndexOf(string name)
	{
		name = name.ToLowerInvariant();
		int num = 0;
		lock (base.List.SyncRoot)
		{
			foreach (StiComponent item in base.List)
			{
				if (item.Name.ToLowerInvariant() == name)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	public void InsertRange(int index, StiComponentsCollection components)
	{
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent component in components)
			{
				Insert(index, component);
			}
		}
	}

	public void Insert(int index, StiComponent component)
	{
		if (parent != null)
		{
			component.Parent = parent;
			if (parent.Page != null)
			{
				component.Page = parent.Page;
			}
		}
		base.List.Insert(index, component);
		InvokeComponentAdded(component, EventArgs.Empty);
	}

	public void Remove(StiComponentsCollection components)
	{
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent component in components)
			{
				Remove(component);
			}
		}
	}

	public void Remove(StiComponent component)
	{
		Remove(component, clearParent: true);
	}

	public void Remove(StiComponent component, bool clearParent)
	{
		if (clearParent && component.Page != null && component.Report != null)
		{
			component.Parent = null;
		}
		if (base.List.Contains(component))
		{
			base.List.Remove(component);
		}
		InvokeComponentRemoved(component, EventArgs.Empty);
	}

	public void CopyTo(Array array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected virtual void OnComponentAdded(EventArgs e)
	{
	}

	public virtual void InvokeComponentAdded(object sender, EventArgs e)
	{
		OnComponentAdded(e);
		this.ComponentAdded?.Invoke(sender, e);
	}

	protected virtual void OnComponentRemoved(EventArgs e)
	{
	}

	public virtual void InvokeComponentRemoved(object sender, EventArgs e)
	{
		OnComponentRemoved(e);
		this.ComponentRemoved?.Invoke(sender, e);
	}

	public virtual void SaveState(string stateName)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiComponent stiComponent = (StiComponent)enumerator.Current;
					stiComponent.SaveState(stateName);
				}
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

	public virtual void RestoreState(string stateName)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiComponent stiComponent = (StiComponent)enumerator.Current;
					stiComponent.RestoreState(stateName);
				}
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

	public virtual void ClearAllStates()
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiComponent stiComponent = (StiComponent)enumerator.Current;
					stiComponent.ClearAllStates();
				}
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

	public void SortBySelectionTick()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1].SelectionTick > this[num].SelectionTick)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortByPriority()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1].Priority > this[num].Priority)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortByTopPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		bool flag = true;
		int count = base.Count;
		if (count > 100)
		{
			flag = false;
			double num = this[0].Top;
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < count; i++)
			{
				double top = this[i].Top;
				hashtable[top] = null;
				if (top < num)
				{
					flag = true;
				}
				num = top;
			}
			int count2 = hashtable.Keys.Count;
			hashtable.Clear();
			if (flag && count2 < count / 2)
			{
				for (int j = 0; j < base.Count; j++)
				{
					StiComponent stiComponent = this[j];
					double top2 = stiComponent.Top;
					List<StiComponent> list = (List<StiComponent>)hashtable[top2];
					if (list == null)
					{
						list = new List<StiComponent>();
						hashtable[top2] = list;
					}
					list.Add(stiComponent);
				}
				object[] array = new object[hashtable.Keys.Count];
				hashtable.Keys.CopyTo(array, 0);
				Array.Sort(array);
				int num2 = 0;
				object[] array2 = array;
				foreach (object key in array2)
				{
					List<StiComponent> list2 = (List<StiComponent>)hashtable[key];
					foreach (StiComponent item in list2)
					{
						this[num2++] = item;
					}
				}
				hashtable.Clear();
				flag = false;
			}
		}
		if (!flag)
		{
			return;
		}
		int num3 = 1;
		while (num3 < count)
		{
			if (this[num3 - 1].Top > this[num3].Top)
			{
				StiComponent value = this[num3 - 1];
				this[num3 - 1] = this[num3];
				this[num3] = value;
				if (num3 != 1)
				{
					num3--;
				}
			}
			else
			{
				num3++;
			}
		}
	}

	public void SortByBottomPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1].Bottom > this[num].Bottom)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortByLeftPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1].Left > this[num].Left)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortByRightPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1].Right > this[num].Right)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortBandsByTopPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1] is StiBand && this[num] is StiBand && this[num - 1].DockStyle == this[num].DockStyle && ((this[num - 1].DockStyle == StiDockStyle.Top && this[num - 1].Top > this[num].Top) || (this[num - 1].DockStyle == StiDockStyle.Bottom && this[num - 1].Top < this[num].Top)))
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void SortBandsByLeftPosition()
	{
		if (base.Count <= 1)
		{
			return;
		}
		int num = 1;
		while (num < base.Count)
		{
			if (this[num - 1] is StiBand && this[num] is StiBand && this[num - 1].Left > this[num].Left)
			{
				StiComponent value = this[num - 1];
				this[num - 1] = this[num];
				this[num] = value;
				if (num != 1)
				{
					num--;
				}
			}
			else
			{
				num++;
			}
		}
	}

	public StiComponentsCollection(StiContainer parent)
	{
		this.parent = parent;
	}

	public StiComponentsCollection()
	{
		parent = null;
	}
}
