using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

[HideModuleName]
[DebuggerNonUserCode]
[StandardModule]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
internal sealed class e
{
	private static ResourceManager m_a;

	private static CultureInfo m_b;

	[SpecialName]
	internal static ResourceManager b()
	{
		if (object.ReferenceEquals(e.m_a, null))
		{
			ResourceManager resourceManager = new ResourceManager("mCore.Resources", typeof(e).Assembly);
			e.m_a = resourceManager;
		}
		return e.m_a;
	}

	[SpecialName]
	internal static CultureInfo a()
	{
		return e.m_b;
	}

	[SpecialName]
	internal static void a(CultureInfo A_0)
	{
		e.m_b = A_0;
	}
}
