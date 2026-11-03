using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using PacketParser.Entities;

namespace PacketParser.Performance;

public static class MemberBaseExtension
{
	public static string GetMethodName(this MethodBase method)
	{
		if (method.DeclaringType.GetInterfaces().Any((Type i) => i == typeof(IAsyncStateMachine)))
		{
			try
			{
				Type generatedType = method.DeclaringType;
				return generatedType.DeclaringType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType)?.Name ?? "foundMethod_is_null";
			}
			catch (Exception ex)
			{
				string text = "exception:" + ex.Message + "\r\n";
				try
				{
					Type declaringType = method.DeclaringType;
					IEnumerable<string> values = from p in declaringType.DeclaringType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
						select $"✔\ufe0fCGP:{p.ContainsGenericParameters}➡\ufe0fN:{p.Name}➡\ufe0fMT:{p.MemberType}➡\ufe0fA:{p.IsAbstract}➡\ufe0fG:{p.IsGenericMethod}";
					text = text + "methodNames:" + string.Join("\r\n", values) + "\r\n";
					text += $"originalType:{declaringType.DeclaringType}\r\n";
				}
				catch (Exception ex2)
				{
					text = text + "\r\n❌catch exception:" + ex2.Message;
				}
				WebTelegramMessage.GetNp_ErrorHandler_bot().Sent2web("خطا در GetMethodName\r\n" + text, null, "GetMethodName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\CallerEnricher.cs", 77);
				return "exception!\\" + ex.Message;
			}
		}
		return method.Name;
	}

	public static string GetMethodDeclaringName(this MethodBase method)
	{
		if (method.DeclaringType.GetInterfaces().Any((Type i) => i == typeof(IAsyncStateMachine)))
		{
			try
			{
				Type generatedType = method.DeclaringType;
				return generatedType.DeclaringType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType)?.DeclaringType?.Name ?? "FoundMethodIsNull!!";
			}
			catch (Exception ex)
			{
				string text = "exception:" + ex.Message + "\r\n";
				try
				{
					Type declaringType = method.DeclaringType;
					IEnumerable<string> values = from p in declaringType.DeclaringType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
						select $"✔\ufe0fCGP:{p.ContainsGenericParameters}➡\ufe0fN:{p.Name}➡\ufe0fMT:{p.MemberType}➡\ufe0fA:{p.IsAbstract}➡\ufe0fG:{p.IsGenericMethod}";
					text = text + "methodNames:" + string.Join("\r\n", values) + "\r\n";
					text += $"originalType:{declaringType.DeclaringType}\r\n";
				}
				catch (Exception ex2)
				{
					text = text + "\r\n❌catch exception:" + ex2.Message;
				}
				WebTelegramMessage.GetNp_ErrorHandler_bot().Sent2web(text, null, "GetMethodDeclaringName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\CallerEnricher.cs", 109);
				return "exception!\\" + ex.Message;
			}
		}
		return method.DeclaringType.Name;
	}
}
