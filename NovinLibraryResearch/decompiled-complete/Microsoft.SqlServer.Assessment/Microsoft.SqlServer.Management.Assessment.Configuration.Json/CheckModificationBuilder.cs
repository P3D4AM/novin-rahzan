using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration.Customization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class CheckModificationBuilder
{
	public const string TargetPropertyName = "target";

	public const string DisplayNamePropertyName = "displayName";

	public const string DescriptionPropertyName = "description";

	public const string MessagePropertyName = "message";

	public const string HelpLinkPropertyName = "helpLink";

	public const string LevelPropertyName = "level";

	public const string ProbesPropertyName = "probes";

	public const string ConditionPropertyName = "condition";

	public const string EnabledPropertyName = "enabled";

	public const string TagsPropertyName = "tags";

	public const string IdPropertyName = "id";

	public const string TargetFilterPropertyName = "targetFilter";

	public const string ItemTypePropertyName = "itemType";

	public const string LocalsPropertyName = "locals";

	public const string DefinitionItemType = "DEFINITION";

	public const string OverrideItemType = "OVERRIDE";

	public CheckSelector Selector { get; set; }

	public string ItemType { get; set; }

	public SqlObjectLocatorPattern Target { get; set; }

	public SqlObjectLocatorPattern TargetFilter { get; set; }

	public List<IItemChange<Check>> Children { get; } = new List<IItemChange<Check>>();


	public JsonReader Reader { get; set; }

	public CheckRule Render()
	{
		if (ItemType == null)
		{
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorMissingJsonProperty, "itemType");
		}
		string text = ItemType.ToUpperInvariant();
		if (!(text == "DEFINITION"))
		{
			if (text == "OVERRIDE")
			{
				return CreateOverride();
			}
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorUnrecognizedChecksItemType, ItemType);
		}
		return CreateNewCheck();
	}

	private CheckRule CreateOverride()
	{
		if (Selector == null)
		{
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorCheckModificationSelectorResuired);
		}
		if (TargetFilter == null)
		{
			return new CheckChange(Selector, Target, Children);
		}
		if (Target != null)
		{
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorMultuallyExclusiveJsonProperties, "targetFilter, targetFilter");
		}
		return new CheckSpecialization(Selector, TargetFilter, Children);
	}

	private CheckDefinition CreateNewCheck()
	{
		if (TargetFilter != null)
		{
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorTargetFilerInDefinition);
		}
		if (!(Selector is CheckByIdSelector checkByIdSelector))
		{
			throw SqlAssessmentJsonException.Create(Reader, Resources.ErrorInvalidCheckId, Selector);
		}
		return new CheckDefinition(checkByIdSelector.CheckId, Target, Children);
	}
}
