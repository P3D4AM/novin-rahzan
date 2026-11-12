using System;
using System.IO;
using System.Reflection;
using System.Text;
using Ninject.Activation;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;

namespace Ninject.Infrastructure.Introspection;

public static class FormatExtensions
{
	public static string FormatActivationPath(this IRequest request)
	{
		using StringWriter stringWriter = new StringWriter();
		for (IRequest request2 = request; request2 != null; request2 = request2.ParentRequest)
		{
			stringWriter.WriteLine("{0,3}) {1}", request2.Depth + 1, request2.Format());
		}
		return stringWriter.ToString();
	}

	public static string Format(this IBinding binding, IContext context)
	{
		using StringWriter stringWriter = new StringWriter();
		if (binding.Condition != null)
		{
			stringWriter.Write("conditional ");
		}
		if (binding.IsImplicit)
		{
			stringWriter.Write("implicit ");
		}
		IProvider provider = binding.GetProvider(context);
		switch (binding.Target)
		{
		case BindingTarget.Self:
			stringWriter.Write("self-binding of {0}", binding.Service.Format());
			break;
		case BindingTarget.Type:
			stringWriter.Write("binding from {0} to {1}", binding.Service.Format(), provider.Type.Format());
			break;
		case BindingTarget.Provider:
			stringWriter.Write("provider binding from {0} to {1} (via {2})", binding.Service.Format(), provider.Type.Format(), provider.GetType().Format());
			break;
		case BindingTarget.Method:
			stringWriter.Write("binding from {0} to method", binding.Service.Format());
			break;
		case BindingTarget.Constant:
			stringWriter.Write("binding from {0} to constant value", binding.Service.Format());
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return stringWriter.ToString();
	}

	public static string Format(this IRequest request)
	{
		using StringWriter stringWriter = new StringWriter();
		if (request.Target == null)
		{
			stringWriter.Write("Request for {0}", request.Service.Format());
		}
		else
		{
			stringWriter.Write("Injection of dependency {0} into {1}", request.Service.Format(), request.Target.Format());
		}
		return stringWriter.ToString();
	}

	public static string Format(this ITarget target)
	{
		using StringWriter stringWriter = new StringWriter();
		switch (target.Member.MemberType)
		{
		case MemberTypes.Constructor:
			stringWriter.Write("parameter {0} of constructor", target.Name);
			break;
		case MemberTypes.Method:
			stringWriter.Write("parameter {0} of method {1}", target.Name, target.Member.Name);
			break;
		case MemberTypes.Property:
			stringWriter.Write("property {0}", target.Name);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		stringWriter.Write(" of type {0}", target.Member.ReflectedType.Format());
		return stringWriter.ToString();
	}

	public static string Format(this Type type)
	{
		string friendlyName = GetFriendlyName(type);
		if (friendlyName.Contains("AnonymousType"))
		{
			return "AnonymousType";
		}
		switch (friendlyName.ToLowerInvariant())
		{
		case "int16":
			return "short";
		case "int32":
			return "int";
		case "int64":
			return "long";
		case "string":
			return "string";
		case "object":
			return "object";
		case "boolean":
			return "bool";
		case "void":
			return "void";
		case "char":
			return "char";
		case "byte":
			return "byte";
		case "uint16":
			return "ushort";
		case "uint32":
			return "uint";
		case "uint64":
			return "ulong";
		case "sbyte":
			return "sbyte";
		case "single":
			return "float";
		case "double":
			return "double";
		case "decimal":
			return "decimal";
		default:
		{
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments.Length != 0)
			{
				return FormatGenericType(friendlyName, genericArguments);
			}
			return friendlyName;
		}
		}
	}

	private static string GetFriendlyName(Type type)
	{
		string text = type.FullName ?? type.Name;
		int num = text.IndexOf('[');
		if (num > 0)
		{
			text = text.Substring(0, num);
		}
		int num2 = text.IndexOf(',');
		if (num2 > 0)
		{
			text = text.Substring(0, num2);
		}
		int num3 = text.LastIndexOf('.');
		if (num3 >= 0)
		{
			text = text.Substring(num3 + 1);
		}
		return text;
	}

	private static string FormatGenericType(string friendlyName, Type[] genericArguments)
	{
		StringBuilder stringBuilder = new StringBuilder(friendlyName.Length + 10);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < friendlyName.Length; i++)
		{
			if (friendlyName[i] == '`')
			{
				int num3 = friendlyName[i + 1] - 48;
				stringBuilder.Append(friendlyName.Substring(num2, i - num2));
				AppendGenericArguments(stringBuilder, genericArguments, num, num3);
				num += num3;
				num2 = i + 2;
			}
		}
		if (num2 < friendlyName.Length)
		{
			stringBuilder.Append(friendlyName.Substring(num2));
		}
		return stringBuilder.ToString();
	}

	private static void AppendGenericArguments(StringBuilder sb, Type[] genericArguments, int start, int count)
	{
		sb.Append("{");
		for (int i = 0; i < count; i++)
		{
			if (i != 0)
			{
				sb.Append(", ");
			}
			sb.Append(genericArguments[start + i].Format());
		}
		sb.Append("}");
	}
}
