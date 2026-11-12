using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal static class CliXml
{
	public delegate bool ValueGetter(XElement e, string path, int pathOffset, out object value);

	public static readonly XNamespace Namespace = XNamespace.op_Implicit("http://schemas.microsoft.com/powershell/2004/04");

	public static readonly XName ToStringElem = Namespace + "ToString";

	public static readonly XName PropsElem = Namespace + "Props";

	public static readonly XName MsElem = Namespace + "MS";

	private static readonly XName ObjElem = Namespace + "Obj";

	public static readonly XName ObjsElem = XName.op_Implicit("Objs");

	public static readonly XName StreamAttr = XName.Get("S");

	public static readonly XName NameAttr = XName.Get("N");

	public static readonly XName RefIdAttr = XName.Get("RefId");

	public const string OutputStreamName = "Output";

	public const string ProgressStreamName = "progress";

	public const string ErrorStreamName = "Error";

	private const char Dot = '.';

	public static Dictionary<XName, ValueGetter> Types { get; } = new Dictionary<XName, ValueGetter>
	{
		{ ObjElem, TryGetObjectValue },
		{
			Namespace + "Ref",
			TryGetRefValue
		},
		{
			Namespace + "S",
			TryGetValue<string>
		},
		{
			Namespace + "Nil",
			TryGetNilValue
		},
		{
			Namespace + "B",
			TryGetValue<bool>
		},
		{
			Namespace + "C",
			TryGetValue<string>
		},
		{
			Namespace + "DT",
			TryGetValue<string>
		},
		{
			Namespace + "TS",
			TryGetValue<string>
		},
		{
			Namespace + "G",
			TryGetValue<string>
		},
		{
			Namespace + "BA",
			TryGetValue<string>
		},
		{
			Namespace + "URI",
			TryGetValue<string>
		},
		{
			Namespace + "Version",
			TryGetValue<string>
		},
		{
			Namespace + "XD",
			TryGetValue<string>
		},
		{
			Namespace + "SBK",
			TryGetValue<string>
		},
		{
			Namespace + "SS",
			TryGetValue<string>
		},
		{
			Namespace + "By",
			TryGetValue<decimal>
		},
		{
			Namespace + "SB",
			TryGetValue<decimal>
		},
		{
			Namespace + "U16",
			TryGetValue<decimal>
		},
		{
			Namespace + "I16",
			TryGetValue<decimal>
		},
		{
			Namespace + "U32",
			TryGetValue<decimal>
		},
		{
			Namespace + "I32",
			TryGetValue<decimal>
		},
		{
			Namespace + "U64",
			TryGetValue<decimal>
		},
		{
			Namespace + "I64",
			TryGetValue<decimal>
		},
		{
			Namespace + "Sg",
			TryGetValue<decimal>
		},
		{
			Namespace + "Db",
			TryGetValue<decimal>
		},
		{
			Namespace + "D",
			TryGetValue<decimal>
		}
	};


	public static string GetStreamName(this XElement e)
	{
		XAttribute obj = e.Attribute(StreamAttr);
		if (obj == null)
		{
			return null;
		}
		return obj.Value;
	}

	public static string GetCliName(this XElement e)
	{
		XAttribute obj = e.Attribute(NameAttr);
		if (obj == null)
		{
			return null;
		}
		return obj.Value;
	}

	public static object GetCliValue(this XElement e)
	{
		if (e.TryGetCliValue(out var value))
		{
			return value;
		}
		return null;
	}

	public static bool TryGetCliValue(this XElement e, out object value)
	{
		return e.TryGetCliValue(string.Empty, 0, out value);
	}

	public static bool TryGetCliValue(this XElement e, string path, int pathOffset, out object value)
	{
		if (Types.TryGetValue(e.Name, out var value2) && value2(e, path, pathOffset, out value))
		{
			return true;
		}
		value = null;
		return false;
	}

	public static bool HasAttribute(this XElement e, XName attrName, string value)
	{
		XAttribute val = e.Attribute(attrName);
		if (val == null)
		{
			return false;
		}
		return string.CompareOrdinal(val.Value, value) == 0;
	}

	public static bool TryGetValue<T>(this XElement e, string path, int pathOffset, out object value)
	{
		if (pathOffset < path.Length)
		{
			value = null;
			return false;
		}
		value = e.Value.Normalize<T>();
		return true;
	}

	public static bool TryGetNilValue(XElement _1, string _2, int _3, out object value)
	{
		value = null;
		return true;
	}

	public static bool TryGetRefValue(this XElement element, string path, int pathOffset, out object value)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (element == null)
		{
			value = null;
			return false;
		}
		XElement element2 = ((XContainer)((XObject)element).Document).Descendants(ObjElem).FirstOrDefault((Func<XElement, bool>)delegate(XElement e)
		{
			XName refIdAttr = RefIdAttr;
			XAttribute obj = element.Attribute(RefIdAttr);
			return e.HasAttribute(refIdAttr, (obj != null) ? obj.Value : null);
		});
		return element2.TryGetObjectValue(path, pathOffset, out value);
	}

	public static bool TryGetObjectValue(this XElement element, string path, int pathOffset, out object value)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (element == null)
		{
			value = null;
			return false;
		}
		if (pathOffset >= path.Length)
		{
			XElement val = ((XContainer)element).Element(ToStringElem);
			value = ((val != null) ? val.Value : null) ?? element.Value;
			return true;
		}
		if (path[pathOffset++] != '.' || pathOffset == path.Length)
		{
			value = null;
			return false;
		}
		if (TryGetValueFromProperties(PropsElem, out value))
		{
			return true;
		}
		if (TryGetValueFromProperties(MsElem, out value))
		{
			return true;
		}
		value = null;
		return false;
		bool TryGetValueFromProperties(XName elementName, out object localValue)
		{
			XElement val2 = ((XContainer)element).Element(elementName);
			if (val2 != null)
			{
				foreach (XElement item in ((XContainer)val2).Elements())
				{
					string cliName = item.GetCliName();
					if (cliName != null && string.CompareOrdinal(path, pathOffset, cliName, 0, cliName.Length) == 0 && Types.TryGetValue(item.Name, out var value2) && value2(item, path, pathOffset + cliName.Length, out localValue))
					{
						return true;
					}
				}
			}
			localValue = null;
			return false;
		}
	}
}
