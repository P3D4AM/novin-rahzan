using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiBusinessObjectConverter))]
public class StiBusinessObject : ICloneable, IStiStateSaveRestore, IStiEnumerator, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject
{
	public enum Order
	{
		Name = 100,
		Alias = 200,
		Category = 300,
		Columns = 400
	}

	private StiStatesManager states;

	protected int positionValue;

	internal object currentObject;

	private object businessObjectValue;

	private StiDictionary dictionary;

	internal bool isEnumeratorCreated;

	private object specPrevValue;

	private object specNextValue;

	private bool specNextValueRead;

	private bool specMoveNextResult;

	private object specStoredCurrentValue;

	internal IEnumerator enumerator;

	protected internal Hashtable RowToLevel;

	internal bool specSetPrevValue;

	internal bool specSetNextValue;

	internal bool specFilterData;

	internal bool specSortGroup;

	internal bool specTotalsCalculation;

	internal bool previousResetException;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiBusinessObject;

	[Browsable(false)]
	public string PropName => Name;

	protected StiStatesManager States => states ?? (states = new StiStatesManager());

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool Inherited { get; set; }

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			return positionValue;
		}
		set
		{
			if (value != positionValue)
			{
				First();
				int num = value;
				while (num > 0)
				{
					num--;
					Next();
				}
			}
		}
	}

	[Browsable(false)]
	public int Count
	{
		get
		{
			CheckEnumerator();
			if (FilteredCount.HasValue)
			{
				return FilteredCount.Value;
			}
			if (BusinessObjectValue is ICollection)
			{
				return ((ICollection)BusinessObjectValue).Count;
			}
			if (BusinessObjectValue is IListSource)
			{
				IListSource listSource = BusinessObjectValue as IListSource;
				IList list = listSource.GetList();
				if (list != null)
				{
					return list.Count;
				}
			}
			if (BusinessObjectValue is Array)
			{
				Array array = BusinessObjectValue as Array;
				return array.Length;
			}
			try
			{
				SaveState("CalculateCount");
				int num = 0;
				CreateEnumerator();
				if (enumerator != null)
				{
					if (!IsEof)
					{
						num++;
					}
					while (enumerator.MoveNext())
					{
						num++;
					}
				}
				return num;
			}
			finally
			{
				RestoreState("CalculateCount");
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsBof { get; set; }

	[Browsable(false)]
	public virtual bool IsEof { get; set; }

	[Browsable(false)]
	public virtual bool IsEmpty { get; private set; }

	internal int? FilteredCount { get; set; }

	[Browsable(false)]
	public object Current
	{
		get
		{
			CheckEnumerator();
			if (currentObject == null && IsEof)
			{
				return specPrevValue;
			}
			return currentObject;
		}
	}

	private StiReport Report => Dictionary?.Report;

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiBusinessObjectsCollection BusinessObjects { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a column collection of the business object.")]
	[TypeConverter(typeof(StiDataColumnsCollectionConverter))]
	[StiOrder(400)]
	public virtual StiDataColumnsCollection Columns { get; set; }

	[StiSerializable]
	[Browsable(false)]
	public string Guid { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[Description("Gets or sets a category name of the business object.")]
	public string Category { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets a name of the business object.")]
	public string Name { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[Description("Gets or sets an alias of the business object.")]
	public string Alias { get; set; }

	[Browsable(false)]
	public object BusinessObjectValue
	{
		get
		{
			if (ParentBusinessObject != null && ParentBusinessObject.specTotalsCalculation)
			{
				businessObjectValue = ParentBusinessObject[Name];
			}
			return businessObjectValue;
		}
		set
		{
			businessObjectValue = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiDictionary Dictionary
	{
		get
		{
			return dictionary;
		}
		set
		{
			dictionary = value;
			BusinessObjects.dictionary = dictionary;
			if (value == null)
			{
				return;
			}
			foreach (StiBusinessObject businessObject in BusinessObjects)
			{
				businessObject.Dictionary = dictionary;
			}
		}
	}

	[Browsable(false)]
	public StiBusinessObject ParentBusinessObject { get; set; }

	[Browsable(false)]
	public StiDataBand OwnerBand { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	[Browsable(false)]
	public static Hashtable FieldsIgnoreList { get; set; } = new Hashtable();


	[Browsable(false)]
	public object this[string name]
	{
		get
		{
			bool flag = isEnumeratorCreated;
			try
			{
				object obj = Current;
				for (StiBusinessObject parentBusinessObject = ParentBusinessObject; parentBusinessObject != null; parentBusinessObject = parentBusinessObject.ParentBusinessObject)
				{
					if (parentBusinessObject.specSetPrevValue || parentBusinessObject.specSetNextValue || parentBusinessObject.specFilterData || parentBusinessObject.specSortGroup || parentBusinessObject.specTotalsCalculation)
					{
						obj = ParentBusinessObject[Name];
						break;
					}
				}
				if (obj == null && IsEof)
				{
					obj = specPrevValue;
				}
				if (obj == null)
				{
					return null;
				}
				object obj2 = null;
				if (businessObjectValue is ITypedList)
				{
					obj2 = StiBusinessObjectHelper.GetValueFromITypedList(businessObjectValue as ITypedList, name, obj);
				}
				else
				{
					if (businessObjectValue is ICustomTypeDescriptor)
					{
						return StiBusinessObjectHelper.GetValueFromICustomTypeDescriptor(businessObjectValue as ICustomTypeDescriptor, name, obj);
					}
					obj2 = StiBusinessObjectHelper.GetValueFromObject(obj, name);
				}
				if (obj2 is Type)
				{
					StiDataColumn stiDataColumn = Columns[name];
					if (stiDataColumn != null && stiDataColumn.Type != typeof(Type))
					{
						return null;
					}
				}
				return obj2;
			}
			finally
			{
				if (!flag)
				{
					BusinessObjectValue = null;
					enumerator = null;
					isEnumeratorCreated = false;
					FilteredCount = null;
				}
			}
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyJObject(val, "BusinessObjects", BusinessObjects.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Columns", Columns.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Guid", Guid);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Category", Category);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BusinessObjects":
				BusinessObjects.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Columns":
				Columns.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Guid":
				Guid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Category":
				Category = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.Category()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public virtual void SaveState(string stateName)
	{
		States.PushInt(stateName, this, "positionValue", positionValue);
		States.PushBool(stateName, this, "isEofValue", IsEof);
		States.PushBool(stateName, this, "isBofValue", IsBof);
		States.PushBool(stateName, this, "isEmpty", IsEmpty);
		States.Push(stateName, this, "current", currentObject);
		States.Push(stateName, this, "businessObjectValue", businessObjectValue);
		States.Push(stateName, this, "specPrevValue", specPrevValue);
		States.Push(stateName, this, "specNextValue", specNextValue);
		States.Push(stateName, this, "specStoredCurrentValue", specStoredCurrentValue);
		States.Push(stateName, this, "enumerator", enumerator);
		States.Push(stateName, this, "FilteredCount", FilteredCount);
		States.PushBool(stateName, this, "isEnumeratorCreated", isEnumeratorCreated);
		States.PushBool(stateName, this, "specNextValueRead", specNextValueRead);
		States.PushBool(stateName, this, "specMoveNextResult", specMoveNextResult);
	}

	public virtual void RestoreState(string stateName)
	{
		if (States.IsExist(stateName, this))
		{
			positionValue = States.PopInt(stateName, this, "positionValue");
			IsBof = States.PopBool(stateName, this, "isBofValue");
			IsEof = States.PopBool(stateName, this, "isEofValue");
			IsEmpty = States.PopBool(stateName, this, "isEmpty");
			currentObject = States.Pop(stateName, this, "current");
			businessObjectValue = States.Pop(stateName, this, "businessObjectValue");
			specPrevValue = States.Pop(stateName, this, "specPrevValue");
			specNextValue = States.Pop(stateName, this, "specNextValue");
			specStoredCurrentValue = States.Pop(stateName, this, "specStoredCurrentValue");
			enumerator = States.Pop(stateName, this, "enumerator") as IEnumerator;
			FilteredCount = States.Pop(stateName, this, "FilteredCount") as int?;
			isEnumeratorCreated = States.PopBool(stateName, this, "isEnumeratorCreated");
			specNextValueRead = States.PopBool(stateName, this, "specNextValueRead");
			specMoveNextResult = States.PopBool(stateName, this, "specMoveNextResult");
		}
	}

	public virtual void ClearAllStates()
	{
		states = null;
	}

	private void EnumeratorReset()
	{
		if (!StiOptions.Engine.AllowUseResetMethodInBusinessObject)
		{
			return;
		}
		if (enumerator != null && enumerator.GetType().GetMethod("Reset") != null)
		{
			try
			{
				enumerator.Reset();
				return;
			}
			catch
			{
				enumerator = StiBusinessObjectHelper.GetEnumeratorFromObject(BusinessObjectValue);
				FilteredCount = null;
				return;
			}
		}
		if (!previousResetException)
		{
			try
			{
				enumerator.Reset();
			}
			catch
			{
				previousResetException = true;
			}
		}
		if (previousResetException)
		{
			enumerator = StiBusinessObjectHelper.GetEnumeratorFromObject(BusinessObjectValue);
			FilteredCount = null;
		}
	}

	public virtual void First()
	{
		specNextValue = null;
		specNextValueRead = false;
		specPrevValue = null;
		positionValue = 0;
		IsEof = false;
		IsBof = true;
		if (enumerator != null)
		{
			EnumeratorReset();
			if (!enumerator.MoveNext())
			{
				IsEmpty = true;
				IsEof = true;
				currentObject = null;
				specPrevValue = null;
			}
			else
			{
				currentObject = enumerator.Current;
				specPrevValue = null;
				IsEmpty = false;
			}
		}
		else
		{
			currentObject = null;
			specPrevValue = null;
			IsEmpty = true;
			IsEof = true;
		}
	}

	public virtual void Prior()
	{
		throw new Exception("StiBusinessObject does not support IStiEnumerator.Prior");
	}

	public virtual void Next()
	{
		if (enumerator == null)
		{
			return;
		}
		if (specNextValueRead)
		{
			if (!specMoveNextResult)
			{
				IsEof = true;
				currentObject = null;
				specPrevValue = null;
			}
			else
			{
				specPrevValue = Current;
				currentObject = specNextValue;
				IsBof = false;
				IsEof = false;
				if (!IsEof)
				{
					positionValue++;
				}
			}
			specNextValueRead = false;
			specNextValue = null;
			return;
		}
		specNextValue = null;
		specNextValueRead = false;
		if (!enumerator.MoveNext())
		{
			IsEof = true;
			specPrevValue = currentObject;
			currentObject = null;
			return;
		}
		specPrevValue = currentObject;
		currentObject = enumerator.Current;
		IsBof = false;
		IsEof = false;
		if (!IsEof)
		{
			positionValue++;
		}
	}

	public virtual void Last()
	{
		while (!IsEof)
		{
			Next();
		}
	}

	public object Clone()
	{
		StiBusinessObject stiBusinessObject = (StiBusinessObject)MemberwiseClone();
		stiBusinessObject.Columns = new StiDataColumnsCollection(stiBusinessObject);
		stiBusinessObject.BusinessObjects = new StiBusinessObjectsCollection(dictionary, stiBusinessObject);
		foreach (StiDataColumn column in Columns)
		{
			StiDataColumn stiDataColumn2 = (StiDataColumn)column.Clone();
			stiDataColumn2.BusinessObject = stiBusinessObject;
			stiBusinessObject.Columns.Add(stiDataColumn2);
		}
		foreach (StiBusinessObject businessObject in BusinessObjects)
		{
			StiBusinessObject stiBusinessObject3 = (StiBusinessObject)businessObject.Clone();
			stiBusinessObject3.ParentBusinessObject = stiBusinessObject;
			stiBusinessObject.BusinessObjects.Add(stiBusinessObject3);
		}
		return stiBusinessObject;
	}

	public int GetLevel()
	{
		if (RowToLevel == null)
		{
			return 0;
		}
		object obj = RowToLevel[Current];
		if (obj is int)
		{
			return (int)obj;
		}
		return 0;
	}

	private void CheckEnumerator()
	{
		if (!isEnumeratorCreated)
		{
			SetDetails();
		}
	}

	public void SetPrevValue()
	{
		specStoredCurrentValue = Current;
		currentObject = specPrevValue;
		specSetPrevValue = true;
	}

	public void SetNextValue()
	{
		specStoredCurrentValue = Current;
		if (!specNextValueRead)
		{
			specMoveNextResult = enumerator.MoveNext();
			if (specMoveNextResult)
			{
				specNextValue = enumerator.Current;
				currentObject = specNextValue;
			}
			else
			{
				specNextValue = null;
				currentObject = null;
			}
			specNextValueRead = true;
		}
		else
		{
			currentObject = specNextValue;
		}
		specSetNextValue = true;
	}

	public void RestoreCurrentValue()
	{
		currentObject = specStoredCurrentValue;
		specSetPrevValue = false;
		specSetNextValue = false;
	}

	public StiBusinessObject GetTopParentBusinessObject()
	{
		StiBusinessObject stiBusinessObject = this;
		while (stiBusinessObject.ParentBusinessObject != null)
		{
			stiBusinessObject = stiBusinessObject.ParentBusinessObject;
		}
		return stiBusinessObject;
	}

	public void CreateEnumerator()
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		specSetPrevValue = false;
		specSetNextValue = false;
		FilteredCount = null;
		if (ParentBusinessObject == null)
		{
			BusinessObjectValue = GetBusinessObjectData();
			enumerator = StiBusinessObjectHelper.GetEnumeratorFromObject(BusinessObjectValue);
			if (enumerator != null && OwnerBand != null)
			{
				ArrayList arrayList = new ArrayList();
				try
				{
					enumerator.Reset();
				}
				catch
				{
				}
				while (enumerator.MoveNext())
				{
					arrayList.Add(enumerator.Current);
				}
				enumerator = arrayList.GetEnumerator();
			}
		}
		else
		{
			object current = ParentBusinessObject.Current;
			if (current == null)
			{
				return;
			}
			BusinessObjectValue = StiBusinessObjectHelper.GetValueFromObject(current, Name);
			enumerator = StiBusinessObjectHelper.GetEnumeratorFromObject(BusinessObjectValue);
		}
		if (OwnerBand != null && (int)OwnerBand.Report.EngineVersion == 2)
		{
			OwnerBand.DataBandInfoV2.GroupHeaderCachedResults = null;
			OwnerBand.DataBandInfoV2.GroupFooterCachedResults = null;
		}
		if (enumerator != null)
		{
			FilterData();
			if (OwnerBand is StiHierarchicalBand)
			{
				StiHierarchicalBusinessObjectSort stiHierarchicalBusinessObjectSort = new StiHierarchicalBusinessObjectSort(this, OwnerBand as StiHierarchicalBand, OwnerBand.Sort);
				stiHierarchicalBusinessObjectSort.Process();
			}
			else
			{
				SortData();
			}
		}
		First();
		isEnumeratorCreated = true;
		SortDataByGroups();
	}

	private void SortData()
	{
		if (OwnerBand != null && OwnerBand.Sort != null && OwnerBand.Sort.Length != 0 && enumerator != null)
		{
			ArrayList arrayList = new ArrayList();
			try
			{
				enumerator.Reset();
			}
			catch
			{
			}
			while (enumerator.MoveNext())
			{
				arrayList.Add(enumerator.Current);
			}
			if (OwnerBand is StiHierarchicalBand)
			{
				StiHierarchicalBusinessObjectSort comparer = new StiHierarchicalBusinessObjectSort(this, OwnerBand as StiHierarchicalBand, OwnerBand.Sort);
				arrayList.Sort(comparer);
			}
			else
			{
				StiBusinessObjectSort comparer2 = new StiBusinessObjectSort(OwnerBand.Sort, null, null);
				arrayList.Sort(comparer2);
			}
			enumerator = arrayList.GetEnumerator();
		}
	}

	private void SortDataByGroups()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Invalid comparison between Unknown and I4
		if (OwnerBand == null || this.enumerator == null || !StiDataHelper.NeedGroupSort(OwnerBand))
		{
			return;
		}
		StiComponentsCollection stiComponentsCollection = (((int)OwnerBand.Report.EngineVersion == 1) ? OwnerBand.DataBandInfoV1.GroupHeaderComponents : OwnerBand.DataBandInfoV2.GroupHeaders);
		int num = 0;
		foreach (StiGroupHeaderBand item in stiComponentsCollection)
		{
			if (item.SortDirection != StiGroupSortDirection.None)
			{
				num++;
			}
		}
		ArrayList arrayList = new ArrayList();
		try
		{
			this.enumerator.Reset();
		}
		catch
		{
		}
		int num2 = 0;
		while (this.enumerator.MoveNext())
		{
			num2++;
		}
		object[,,] array = new object[num2, num + 1, 2];
		Hashtable hashtable = new Hashtable();
		int num3 = 0;
		object obj2 = currentObject;
		specSortGroup = true;
		try
		{
			this.enumerator.Reset();
		}
		catch
		{
		}
		while (this.enumerator.MoveNext())
		{
			object current = this.enumerator.Current;
			arrayList.Add(current);
			hashtable[current] = num3;
			currentObject = current;
			int num4 = 0;
			foreach (StiGroupHeaderBand item2 in stiComponentsCollection)
			{
				if (item2.SortDirection != StiGroupSortDirection.None)
				{
					if ((int)OwnerBand.Report.EngineVersion == 1)
					{
						array[num3, num4, 0] = StiGroupHeaderBandV1Builder.GetCurrentConditionValue(item2);
					}
					else
					{
						array[num3, num4, 0] = StiGroupHeaderBandV2Builder.GetCurrentConditionValue(item2);
					}
					array[num3, num4, 1] = item2.SortDirection;
					num4++;
				}
			}
			array[num3, num, 0] = num3;
			array[num3, num, 1] = StiGroupSortDirection.Ascending;
			num3++;
		}
		specSortGroup = false;
		currentObject = obj2;
		StiBusinessObjectSort comparer = new StiBusinessObjectSort(OwnerBand.Sort, hashtable, array);
		arrayList.Sort(comparer);
		this.enumerator = arrayList.GetEnumerator();
		First();
	}

	public void FilterData()
	{
		if (enumerator == null)
		{
			return;
		}
		object filterEventHandler = StiDataHelper.GetFilterEventHandler(OwnerBand, this);
		if (OwnerBand == null || filterEventHandler == null)
		{
			return;
		}
		isEnumeratorCreated = true;
		int position = Position;
		int line = Dictionary.Report.Line;
		ArrayList arrayList = new ArrayList();
		specFilterData = true;
		if (filterEventHandler is StiFilterEventHandler)
		{
			int num = 0;
			try
			{
				enumerator.Reset();
			}
			catch
			{
			}
			while (enumerator.MoveNext())
			{
				Position = num;
				if (num == 0 && positionValue == 0)
				{
					currentObject = enumerator.Current;
				}
				Dictionary.Report.Line = num + 1;
				try
				{
					StiFilterEventHandler stiFilterEventHandler = (StiFilterEventHandler)filterEventHandler;
					StiFilterEventArgs stiFilterEventArgs = new StiFilterEventArgs();
					stiFilterEventHandler(this, stiFilterEventArgs);
					if (stiFilterEventArgs.Value)
					{
						arrayList.Add(enumerator.Current);
					}
				}
				catch
				{
				}
				num++;
			}
			enumerator = arrayList.GetEnumerator();
			FilteredCount = arrayList.Count;
		}
		else if (filterEventHandler is StiParser.StiFilterParserData)
		{
			StiParser.StiFilterParserData stiFilterParserData = (StiParser.StiFilterParserData)filterEventHandler;
			int num2 = 0;
			try
			{
				enumerator.Reset();
			}
			catch
			{
			}
			while (enumerator.MoveNext())
			{
				Position = num2;
				if (num2 == 0 && positionValue == 0)
				{
					currentObject = enumerator.Current;
				}
				Dictionary.Report.Line = num2 + 1;
				try
				{
					object obj4 = StiParser.ParseTextValue(stiFilterParserData.Expression, stiFilterParserData.Component);
					if (obj4 is bool && (bool)obj4)
					{
						arrayList.Add(enumerator.Current);
					}
				}
				catch
				{
				}
				num2++;
			}
			enumerator = arrayList.GetEnumerator();
			FilteredCount = arrayList.Count;
		}
		specFilterData = false;
		Position = position;
		Dictionary.Report.Line = line;
		isEnumeratorCreated = false;
	}

	private void DestroyEnumerator()
	{
		isEnumeratorCreated = false;
		FilteredCount = null;
	}

	public void SetDetails()
	{
		UpdateChilds();
		CreateEnumerator();
	}

	private void UpdateChilds()
	{
		isEnumeratorCreated = false;
		foreach (StiBusinessObject businessObject in BusinessObjects)
		{
			businessObject.UpdateChilds();
		}
	}

	private object GetBusinessObjectDataFromParent(StiBusinessObject businessObject)
	{
		foreach (StiBusinessObjectData item in businessObject.dictionary.Report.BusinessObjectsStore)
		{
			if (item.Name == businessObject.Name)
			{
				return item.BusinessObjectValue;
			}
		}
		return null;
	}

	public int GetColumnIndex(string columnName)
	{
		int num = 0;
		foreach (StiDataColumn column in Columns)
		{
			if (column.NameInSource == columnName)
			{
				return num;
			}
			num++;
		}
		num = 0;
		foreach (StiDataColumn column2 in Columns)
		{
			if (column2.Name == columnName || column2.Alias == columnName)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public object GetBusinessObjectData()
	{
		return GetBusinessObjectData(isColumnsRetrieve: false);
	}

	public object GetBusinessObjectData(bool isColumnsRetrieve)
	{
		if (ParentBusinessObject == null)
		{
			return GetBusinessObjectDataFromParent(this);
		}
		List<StiBusinessObject> list = new List<StiBusinessObject>();
		StiBusinessObject stiBusinessObject = this;
		while (stiBusinessObject.ParentBusinessObject != null)
		{
			list.Insert(0, stiBusinessObject);
			stiBusinessObject = stiBusinessObject.ParentBusinessObject;
		}
		object obj = GetBusinessObjectDataFromParent(stiBusinessObject);
		if (obj is IEnumerable)
		{
			object elementFromEnumerable = StiBusinessObjectHelper.GetElementFromEnumerable(obj as IEnumerable);
			obj = ((elementFromEnumerable != null) ? elementFromEnumerable : StiBusinessObjectHelper.GetElementType(obj.GetType()));
		}
		foreach (StiBusinessObject item in list)
		{
			obj = StiBusinessObjectHelper.GetElementFromObject(obj, item.Name, isColumnsRetrieve);
			stiBusinessObject = item;
		}
		return obj;
	}

	public string GetFullName()
	{
		if (StiOptions.Dictionary.ShowOnlyAliasForBusinessObject)
		{
			return StiBusinessObjectHelper.GetBusinessObjectFullAlias(this);
		}
		string businessObjectFullName = StiBusinessObjectHelper.GetBusinessObjectFullName(this);
		if (Name == Alias)
		{
			return businessObjectFullName;
		}
		string businessObjectFullAlias = StiBusinessObjectHelper.GetBusinessObjectFullAlias(this);
		return businessObjectFullName + " [" + businessObjectFullAlias + "]";
	}

	public string GetCorrectFullName()
	{
		if (ParentBusinessObject == null)
		{
			return Name;
		}
		return ParentBusinessObject.GetCorrectFullName() + "." + StiNameValidator.CorrectName(Name, Report);
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public void Connect()
	{
		UpdateChilds();
		CreateEnumerator();
	}

	public void Disconnect()
	{
		DestroyEnumerator();
		businessObjectValue = null;
		RowToLevel = null;
	}

	public StiBusinessObject()
		: this(string.Empty, string.Empty, string.Empty, null)
	{
	}

	public StiBusinessObject(string category, string name, string alias, string guid)
	{
		Category = category;
		Name = name;
		Alias = alias;
		Columns = new StiDataColumnsCollection(this);
		BusinessObjects = new StiBusinessObjectsCollection(null, this);
		if (guid == null)
		{
			guid = System.Guid.NewGuid().ToString().Replace("-", "");
		}
		Guid = guid;
	}

	public StiBusinessObject(string category, string name, string alias, string guid, string key)
		: this(category, name, alias, guid)
	{
		Key = key;
	}
}
