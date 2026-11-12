using System.Reflection;

namespace Stimulsoft.Base.Helpers;

public static class StiMathHelper
{
	public static string NameAssembly = "Stimulsoft.MathFX";

	private static object lockObject = new object();

	private static bool tryToLoadAssembly = false;

	private static Assembly mathAssembly;

	public static Assembly MathAssembly
	{
		get
		{
			lock (lockObject)
			{
				if (mathAssembly == null && !tryToLoadAssembly)
				{
					try
					{
						mathAssembly = StiAssemblyFinder.GetAssembly(NameAssembly + ".dll");
						tryToLoadAssembly = true;
					}
					catch
					{
						return null;
					}
				}
				return mathAssembly;
			}
		}
	}

	public static bool IsMathAssemblyLoaded => mathAssembly != null;

	public static string GetMatmML(string latextContent)
	{
		object value = Reflection.Create(MathAssembly, NameAssembly + ".StiLatexService", latextContent);
		return Reflection.InvokeMethod<string>(value, "GetMathML", new object[0]);
	}

	public static string GetSvg(string matmMLContent, float fontSize, string colorHex)
	{
		object value = Reflection.Create(MathAssembly, NameAssembly + ".StiMathMLService", matmMLContent);
		return Reflection.InvokeMethod<string>(value, "GetSVG", new object[2] { fontSize, colorHex });
	}
}
