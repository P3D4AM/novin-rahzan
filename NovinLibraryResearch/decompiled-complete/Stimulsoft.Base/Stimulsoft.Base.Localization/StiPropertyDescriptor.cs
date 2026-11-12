using System;
using System.ComponentModel;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization;

public class StiPropertyDescriptor : PropertyDescriptor
{
	protected PropertyDescriptor PropertyDescriptor { get; set; }

	private bool IsEventDescriptor { get; set; }

	protected bool IsLocalizableProperties { get; set; }

	internal int Level { get; set; }

	internal int MaxLevel { get; set; }

	public override string Description
	{
		get
		{
			StiProcessDescriptionEventArgs stiProcessDescriptionEventArgs = new StiProcessDescriptionEventArgs(ComponentType, Name, base.Description);
			InvokeProcessDescription(this, stiProcessDescriptionEventArgs);
			return stiProcessDescriptionEventArgs.Description;
		}
	}

	public override string Category
	{
		get
		{
			StiCategoryAttribute stiCategoryAttribute = PropertyDescriptor.Attributes[typeof(StiCategoryAttribute)] as StiCategoryAttribute;
			StiCategorySource stiCategorySource = stiCategoryAttribute?.Source ?? StiCategorySource.PropertyCategory;
			string text = ((stiCategoryAttribute == null) ? "Misc" : stiCategoryAttribute.Category);
			if (!IsLocalizableProperties)
			{
				text = UpdateCategoryName(text);
				if (Level != 0)
				{
					if (MaxLevel == 0 || MaxLevel <= 9 || Level >= 10)
					{
						return $"{Level}. {text}";
					}
					return $"0{Level}. {text}";
				}
				return text;
			}
			switch (stiCategorySource)
			{
			case StiCategorySource.PropertyCategory:
				if (!text.EndsWithInvariant("Category"))
				{
					text += "Category";
				}
				text = StiLocalization.Get("PropertyCategory", text);
				break;
			case StiCategorySource.Components:
				if (!text.StartsWithInvariant("Sti"))
				{
					text = "Sti" + text;
				}
				text = StiLocalization.Get("Components", text);
				break;
			default:
				text = StiLocalization.Get("PropertyMain", text);
				break;
			}
			if (text != null)
			{
				text = UpdateCategoryName(text);
				if (Level != 0)
				{
					if (MaxLevel == 0 || MaxLevel <= 9 || Level >= 10)
					{
						return $"{Level}. {text}";
					}
					return $"0{Level}. {text}";
				}
				return text;
			}
			return text;
		}
	}

	public string OriginalCategory
	{
		get
		{
			if (PropertyDescriptor.Attributes[typeof(StiCategoryAttribute)] is StiCategoryAttribute stiCategoryAttribute)
			{
				return stiCategoryAttribute.Category;
			}
			return "Misc";
		}
	}

	public override string DisplayName
	{
		get
		{
			if (IsLocalizableProperties)
			{
				return LocalizedName;
			}
			return PropertyDescriptor.DisplayName;
		}
	}

	public string LocalizedName
	{
		get
		{
			if (IsLocalizableProperties && PropertyDescriptor.Attributes[typeof(StiLocalizationKeyAttribute)] is StiLocalizationKeyAttribute stiLocalizationKeyAttribute)
			{
				return StiLocalization.Get(stiLocalizationKeyAttribute.Category, stiLocalizationKeyAttribute.Key);
			}
			if (!IsEventDescriptor)
			{
				string key = PropertyDescriptor.ComponentType.Name + PropertyDescriptor.Name;
				string text = ((PropertyDescriptor.Name == "Category") ? null : StiLocalization.Get("PropertyMain", key, throwError: false));
				if (text == null)
				{
					text = StiLocalization.Get("PropertyMain", PropertyDescriptor.Name, throwError: false);
				}
				return text ?? PropertyDescriptor.DisplayName;
			}
			string text2 = StiLocalization.Get("PropertyEvents", PropertyDescriptor.Name, throwError: false);
			return text2 ?? PropertyDescriptor.DisplayName;
		}
	}

	public override Type ComponentType => PropertyDescriptor.ComponentType;

	public override bool IsReadOnly => PropertyDescriptor.IsReadOnly;

	public override Type PropertyType => PropertyDescriptor.PropertyType;

	public static event StiProcessDescriptionEventHandler ProcessDescription;

	public static void InvokeProcessDescription(object sender, StiProcessDescriptionEventArgs e)
	{
		StiPropertyDescriptor.ProcessDescription?.Invoke(sender, e);
	}

	public override string ToString()
	{
		return DisplayName;
	}

	private string UpdateCategoryName(string name)
	{
		for (int i = 1; i < name.Length; i++)
		{
			if (char.IsUpper(name[i]))
			{
				name = name.Insert(i, " ");
				i++;
			}
		}
		return name;
	}

	public override bool CanResetValue(object component)
	{
		return PropertyDescriptor.CanResetValue(component);
	}

	public override object GetValue(object component)
	{
		return PropertyDescriptor.GetValue(component);
	}

	public override void ResetValue(object component)
	{
		PropertyDescriptor.ResetValue(component);
	}

	public override void SetValue(object component, object value)
	{
		PropertyDescriptor.SetValue(component, value);
	}

	public override bool ShouldSerializeValue(object component)
	{
		return PropertyDescriptor.ShouldSerializeValue(component);
	}

	public StiPropertyDescriptor(PropertyDescriptor propertyDescriptor)
		: this(propertyDescriptor, isEventDescriptor: false)
	{
	}

	public StiPropertyDescriptor(PropertyDescriptor propertyDescriptor, bool isEventDescriptor)
		: base(propertyDescriptor)
	{
		IsLocalizableProperties = StiPropertyGridOptions.Localizable;
		PropertyDescriptor = propertyDescriptor;
		IsEventDescriptor = isEventDescriptor;
	}
}
