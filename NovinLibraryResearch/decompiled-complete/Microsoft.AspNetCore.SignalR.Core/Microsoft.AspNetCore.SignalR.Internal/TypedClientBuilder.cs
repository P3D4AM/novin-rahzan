using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal static class TypedClientBuilder<T>
{
	private const string ClientModuleName = "Microsoft.AspNetCore.SignalR.TypedClientBuilder";

	private static readonly Lazy<Func<IClientProxy, T>> _builder = new Lazy<Func<IClientProxy, T>>(() => GenerateClientBuilder());

	private static readonly PropertyInfo CancellationTokenNoneProperty = typeof(CancellationToken).GetProperty("None", BindingFlags.Static | BindingFlags.Public);

	public static T Build(IClientProxy proxy)
	{
		return _builder.Value(proxy);
	}

	public static void Validate()
	{
		_ = _builder.Value;
	}

	private static Func<IClientProxy, T> GenerateClientBuilder()
	{
		VerifyInterface(typeof(T));
		ModuleBuilder moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Microsoft.AspNetCore.SignalR.TypedClientBuilder"), AssemblyBuilderAccess.Run).DefineDynamicModule("Microsoft.AspNetCore.SignalR.TypedClientBuilder");
		Type clientType = GenerateInterfaceImplementation(moduleBuilder);
		return (IClientProxy proxy) => (T)Activator.CreateInstance(clientType, proxy);
	}

	private static Type GenerateInterfaceImplementation(ModuleBuilder moduleBuilder)
	{
		TypeBuilder typeBuilder = moduleBuilder.DefineType("Microsoft.AspNetCore.SignalR.TypedClientBuilder." + typeof(T).Name + "Impl", TypeAttributes.Public, typeof(object), new Type[1] { typeof(T) });
		FieldBuilder proxyField = typeBuilder.DefineField("_proxy", typeof(IClientProxy), FieldAttributes.Private);
		BuildConstructor(typeBuilder, proxyField);
		foreach (MethodInfo allInterfaceMethod in GetAllInterfaceMethods(typeof(T)))
		{
			BuildMethod(typeBuilder, allInterfaceMethod, proxyField);
		}
		return typeBuilder.CreateTypeInfo();
	}

	private static IEnumerable<MethodInfo> GetAllInterfaceMethods(Type interfaceType)
	{
		Type[] interfaces = interfaceType.GetInterfaces();
		foreach (Type interfaceType2 in interfaces)
		{
			foreach (MethodInfo allInterfaceMethod in GetAllInterfaceMethods(interfaceType2))
			{
				yield return allInterfaceMethod;
			}
		}
		MethodInfo[] methods = interfaceType.GetMethods();
		for (int i = 0; i < methods.Length; i++)
		{
			yield return methods[i];
		}
	}

	private static void BuildConstructor(TypeBuilder type, FieldInfo proxyField)
	{
		MethodBuilder methodBuilder = type.DefineMethod(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig);
		ConstructorInfo constructor = typeof(object).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
		methodBuilder.SetReturnType(typeof(void));
		methodBuilder.SetParameters(typeof(IClientProxy));
		ILGenerator iLGenerator = methodBuilder.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Call, constructor);
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Ldarg_1);
		iLGenerator.Emit(OpCodes.Stfld, proxyField);
		iLGenerator.Emit(OpCodes.Ret);
	}

	private static void BuildMethod(TypeBuilder type, MethodInfo interfaceMethodInfo, FieldInfo proxyField)
	{
		MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
		ParameterInfo[] parameters = interfaceMethodInfo.GetParameters();
		Type[] array = parameters.Select((ParameterInfo param) => param.ParameterType).ToArray();
		MethodBuilder methodBuilder = type.DefineMethod(interfaceMethodInfo.Name, attributes);
		MethodInfo method = typeof(IClientProxy).GetMethod("SendCoreAsync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
		{
			typeof(string),
			typeof(object[]),
			typeof(CancellationToken)
		}, null);
		methodBuilder.SetReturnType(interfaceMethodInfo.ReturnType);
		methodBuilder.SetParameters(array);
		string[] array2 = (from p in array
			where p.IsGenericParameter
			select p.Name).Distinct().ToArray();
		if (array2.Any())
		{
			methodBuilder.DefineGenericParameters(array2);
		}
		ILGenerator iLGenerator = methodBuilder.GetILGenerator();
		iLGenerator.DeclareLocal(typeof(object[]));
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Ldfld, proxyField);
		iLGenerator.Emit(OpCodes.Ldstr, interfaceMethodInfo.Name);
		iLGenerator.Emit(OpCodes.Ldc_I4, parameters.Length);
		iLGenerator.Emit(OpCodes.Newarr, typeof(object));
		iLGenerator.Emit(OpCodes.Stloc_0);
		for (int i = 0; i < array.Length; i++)
		{
			iLGenerator.Emit(OpCodes.Ldloc_0);
			iLGenerator.Emit(OpCodes.Ldc_I4, i);
			iLGenerator.Emit(OpCodes.Ldarg, i + 1);
			iLGenerator.Emit(OpCodes.Box, array[i]);
			iLGenerator.Emit(OpCodes.Stelem_Ref);
		}
		iLGenerator.Emit(OpCodes.Ldloc_0);
		iLGenerator.Emit(OpCodes.Call, CancellationTokenNoneProperty.GetMethod);
		iLGenerator.Emit(OpCodes.Callvirt, method);
		iLGenerator.Emit(OpCodes.Ret);
	}

	private static void VerifyInterface(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new InvalidOperationException("Type must be an interface.");
		}
		if (interfaceType.GetProperties().Length != 0)
		{
			throw new InvalidOperationException("Type must not contain properties.");
		}
		if (interfaceType.GetEvents().Length != 0)
		{
			throw new InvalidOperationException("Type must not contain events.");
		}
		MethodInfo[] methods = interfaceType.GetMethods();
		foreach (MethodInfo interfaceMethod in methods)
		{
			VerifyMethod(interfaceType, interfaceMethod);
		}
		Type[] interfaces = interfaceType.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			VerifyInterface(interfaces[i]);
		}
	}

	private static void VerifyMethod(Type interfaceType, MethodInfo interfaceMethod)
	{
		if (interfaceMethod.ReturnType != typeof(Task))
		{
			throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. All client proxy methods must return '" + typeof(Task).FullName + "'.");
		}
		ParameterInfo[] parameters = interfaceMethod.GetParameters();
		foreach (ParameterInfo parameterInfo in parameters)
		{
			if (parameterInfo.IsOut)
			{
				throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. Client proxy methods must not have 'out' parameters.");
			}
			if (parameterInfo.ParameterType.IsByRef)
			{
				throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. Client proxy methods must not have 'ref' parameters.");
			}
		}
	}
}
