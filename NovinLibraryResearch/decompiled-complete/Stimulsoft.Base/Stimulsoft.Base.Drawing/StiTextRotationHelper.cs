namespace Stimulsoft.Base.Drawing;

public static class StiTextRotationHelper
{
	public static void CalculateAngle(StiTextDockMode dockMode, ref float angle, ref StiRotationMode rotationMode)
	{
		switch (dockMode)
		{
		case StiTextDockMode.Top:
			if (angle == 0f)
			{
				rotationMode = StiRotationMode.CenterBottom;
			}
			else if (angle >= 0f)
			{
				rotationMode = StiRotationMode.LeftCenter;
			}
			else if (angle <= 0f)
			{
				rotationMode = StiRotationMode.RightCenter;
			}
			break;
		case StiTextDockMode.Bottom:
			if (angle == 0f)
			{
				rotationMode = StiRotationMode.CenterTop;
			}
			else if (angle >= 0f)
			{
				rotationMode = StiRotationMode.RightCenter;
			}
			else if (angle <= 0f)
			{
				rotationMode = StiRotationMode.LeftCenter;
			}
			break;
		case StiTextDockMode.Left:
			rotationMode = StiRotationMode.RightCenter;
			break;
		case StiTextDockMode.Right:
			rotationMode = StiRotationMode.LeftCenter;
			break;
		}
	}
}
