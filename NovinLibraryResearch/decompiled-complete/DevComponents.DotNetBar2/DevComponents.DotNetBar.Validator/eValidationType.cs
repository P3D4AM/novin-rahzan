using System;

namespace DevComponents.DotNetBar.Validator;

[Flags]
public enum eValidationType
{
	Manual = 1,
	ValidatingEventPerControl = 2,
	ValidatingEventOnContainer = 4
}
