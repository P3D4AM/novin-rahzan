namespace DevComponents.DotNetBar;

public interface IPropertyElementStyleProvider
{
	ElementStyle ReadOnlyStyle { get; }

	ElementStyle ValueChangedStyle { get; }

	ElementStyle CategoryStyle { get; }

	ElementStyle DefaultEditCellStyle { get; }
}
