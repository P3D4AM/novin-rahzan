namespace DevComponents.DotNetBar;

public interface IPropertyGridLocalizer
{
	string GetPropertyName(string propertyName);

	string GetCategoryName(string categoryName);

	string GetErrorTooltipMessage(string message);
}
