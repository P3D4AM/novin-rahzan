using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public sealed class StiNameCreation
{
	public static StiNamingRule NamingRule
	{
		get
		{
			return StiOptions.Engine.NamingRule;
		}
		set
		{
			StiOptions.Engine.NamingRule = value;
		}
	}

	private static string RemoveSpacesFromName(string baseName, bool removeIncorrectSymbols, StiReport report)
	{
		if (!removeIncorrectSymbols)
		{
			return baseName;
		}
		StringBuilder stringBuilder = new StringBuilder(baseName);
		int num = 0;
		int length = baseName.Length;
		for (int i = 0; i < length; i++)
		{
			if (baseName[i] == ' ')
			{
				stringBuilder.Remove(num, 1);
				if (num < stringBuilder.Length && char.IsLetter(stringBuilder[num]))
				{
					stringBuilder[num] = char.ToUpper(stringBuilder[num]);
				}
			}
			else
			{
				num++;
			}
		}
		baseName = stringBuilder.ToString();
		return StiNameValidator.CorrectName(baseName, report);
	}

	public static string CreateSimpleName(StiReport report, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: true, report);
		return $"{baseName}{report.IndexName++}";
	}

	public static string CreateName(StiReport report, string baseName)
	{
		return CreateName(report, baseName, addOne: true, removeIncorrectSymbols: true);
	}

	public static string CreateName(StiReport report, string baseName, bool addOne, bool removeIncorrectSymbols)
	{
		return CreateName(report, baseName, addOne, removeIncorrectSymbols, forceAdvancedNamingRule: false);
	}

	public static string CreateName(StiReport report, string baseName, bool addOne, bool removeIncorrectSymbols, bool forceAdvancedNamingRule)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols, report);
		if (!forceAdvancedNamingRule && (!report.IsDesigning || NamingRule == StiNamingRule.Simple))
		{
			return $"{baseName}{report.IndexName++}";
		}
		StiComponentsCollection components = report.GetComponents();
		int num = 1;
		if (components.Count == 0 && report.DataSources.Count == 0 && report.Dictionary.DataSources.Count == 0 && report.Dictionary.Relations.Count == 0 && report.Dictionary.Variables.Count == 0)
		{
			if (addOne)
			{
				return baseName + "1";
			}
			return baseName;
		}
		string text;
		while (true)
		{
			text = baseName + num;
			string name = ((!addOne && num == 1) ? baseName : text);
			if (GetObjectWithName(null, report, components, name) == null)
			{
				break;
			}
			num++;
		}
		if (addOne || num != 1)
		{
			return text;
		}
		return baseName;
	}

	public static string CreateResourceName(StiReport report, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsResourceNameExists(report, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static string CreateConnectionName(StiReport report, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsConnectionNameExists(report, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static string CreateDataSourcesName(StiReport report, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsTableDataSourcesExists(report, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static string CreateRelationName(StiReport report, StiDataRelation dataRelation, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsRelationExists(report, dataRelation, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static bool IsRelationExists(StiReport report, StiDataRelation dataRelation, string name)
	{
		if (report == null)
		{
			return false;
		}
		name = name.ToLowerInvariant().Trim();
		return report.Dictionary.Relations.ToList().Any((StiDataRelation r) => r.Name.ToLowerInvariant().Trim() == name && r.ChildSource.Name == dataRelation.ChildSource.Name);
	}

	public static bool IsTableDataSourcesExists(StiReport report, string name)
	{
		if (report == null)
		{
			return false;
		}
		name = name.ToLowerInvariant().Trim();
		return report.Dictionary.DataSources.ToList().Any((StiDataSource r) => r.Name.ToLowerInvariant().Trim() == name);
	}

	public static bool IsResourceNameExists(StiReport report, string name)
	{
		if (report == null)
		{
			return false;
		}
		name = name.ToLowerInvariant().Trim();
		return report.Dictionary.Resources.ToList().Any((StiResource r) => r.Name.ToLowerInvariant().Trim() == name);
	}

	public static bool IsConnectionNameExists(StiReport report, string name)
	{
		if (report == null)
		{
			return false;
		}
		name = name.ToLowerInvariant().Trim();
		return report.Dictionary.Databases.ToList().Any((StiDatabase r) => r.Name.ToLowerInvariant().Trim() == name);
	}

	public static string CreateColumnName(StiDataSource dataSource, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, dataSource?.Dictionary?.Report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsColumnNameExists(dataSource, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static bool IsColumnNameExists(StiDataSource dataSource, string name)
	{
		name = name.ToLowerInvariant().Trim();
		return dataSource.Columns.ToList().Any((StiDataColumn r) => r.Name.ToLowerInvariant().Trim() == name);
	}

	public static string CreateParameterName(StiSqlSource dataSource, string baseName)
	{
		baseName = RemoveSpacesFromName(baseName, removeIncorrectSymbols: false, dataSource?.Dictionary?.Report);
		int num = 1;
		string text;
		while (true)
		{
			text = ((num == 1) ? baseName : (baseName + num));
			if (!IsParameterNameExists(dataSource, text))
			{
				break;
			}
			num++;
		}
		return text;
	}

	public static bool IsParameterNameExists(StiSqlSource dataSource, string name)
	{
		name = name.ToLowerInvariant().Trim();
		return dataSource.Parameters.ToList().Any((StiDataParameter r) => r.Name.ToLowerInvariant().Trim() == name);
	}

	public static bool IsValidName(StiReport report, string name)
	{
		if (string.IsNullOrEmpty(name) || (!char.IsLetter(name[0]) && name[0] != '_'))
		{
			return false;
		}
		for (int i = 0; i < name.Length; i++)
		{
			if (!char.IsLetterOrDigit(name[i]) && name[i] != '_')
			{
				return false;
			}
		}
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (name == item.Name)
			{
				return false;
			}
		}
		return true;
	}

	public static bool Exists(object checkedObject, StiReport report, string name)
	{
		return GetObjectWithName(checkedObject, report, name) != null;
	}

	public static bool Exists(StiReport report, string name)
	{
		return Exists(null, report, name);
	}

	public static bool CheckName(object checkedObject, StiReport report, string name, string messageBoxCaption)
	{
		return CheckName(checkedObject, report, name, messageBoxCaption, isValid: true);
	}

	public static bool CheckName(object checkedObject, StiReport report, string name, string messageBoxCaption, bool isValid)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (report == null || !report.IsDesigning)
		{
			return true;
		}
		if (Exists(checkedObject, report, name))
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "NameExists"), name), messageBoxCaption, (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		if (isValid && !IsValidName(report, name))
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "IdentifierIsNotValid"), name), messageBoxCaption, (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		return true;
	}

	public static object GetObjectWithName(object checkedObject, StiReport report, string name)
	{
		if (report == null)
		{
			return null;
		}
		return GetObjectWithName(checkedObject, report, report.GetComponents(), name);
	}

	private static object GetObjectWithName(object checkedObject, StiReport report, StiComponentsCollection comps, string name)
	{
		if (report == null)
		{
			return null;
		}
		for (int i = 0; i < comps.Count; i++)
		{
			StiComponent stiComponent = comps[i];
			if (name == stiComponent.Name && checkedObject != stiComponent)
			{
				return stiComponent;
			}
		}
		for (int j = 0; j < report.Dictionary.DataSources.Count; j++)
		{
			StiDataSource stiDataSource = report.Dictionary.DataSources[j];
			if (name == stiDataSource.Name && checkedObject != stiDataSource)
			{
				return stiDataSource;
			}
		}
		for (int k = 0; k < report.Dictionary.BusinessObjects.Count; k++)
		{
			StiBusinessObject stiBusinessObject = report.Dictionary.BusinessObjects[k];
			if (name == stiBusinessObject.Name && checkedObject != stiBusinessObject)
			{
				return stiBusinessObject;
			}
		}
		for (int l = 0; l < report.Dictionary.Variables.Count; l++)
		{
			StiVariable stiVariable = report.Dictionary.Variables[l];
			if (name == stiVariable.Name && checkedObject != stiVariable)
			{
				return stiVariable;
			}
		}
		return null;
	}

	public static string GenerateName(StiReport report, string localizedName, string name)
	{
		if (StiOptions.Engine.ForceGenerationLocalizedName)
		{
			return localizedName;
		}
		if (!StiOptions.Engine.ForceGenerationNonLocalizedName && report != null && report.Info.GenerateLocalizedName)
		{
			return localizedName;
		}
		string text = name;
		if (text.Length > 1 && text.Substring(0, 3) == "Sti")
		{
			text = text.Substring(3);
		}
		if (text.EndsWith("Element"))
		{
			text = text.Substring(0, text.Length - "Element".Length);
		}
		if (text.EndsWith("UI"))
		{
			text = text.Substring(0, text.Length - "UI".Length);
		}
		return text;
	}

	public static string GenerateName(StiReport report, string localizedName, Type type)
	{
		return GenerateName(report, localizedName, type.Name);
	}

	public static string GenerateName(StiComponent component)
	{
		return GenerateName(component.Report, component.LocalizedName, ((object)component).GetType());
	}

	public static string GenerateName(StiDataRelation relation)
	{
		return GenerateName(relation.Dictionary.Report, StiLocalization.Get("PropertyMain", "DataRelation"), relation.GetType());
	}

	public static string GenerateName(StiDataSource dataSource)
	{
		return GenerateName(dataSource.Dictionary.Report, StiLocalization.Get("PropertyMain", "DataSource"), dataSource.GetType());
	}
}
