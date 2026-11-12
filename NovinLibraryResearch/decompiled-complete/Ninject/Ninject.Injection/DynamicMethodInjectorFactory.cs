using System;
using System.Reflection;
using System.Reflection.Emit;
using Ninject.Components;

namespace Ninject.Injection;

public class DynamicMethodInjectorFactory : NinjectComponent, IInjectorFactory, INinjectComponent, IDisposable
{
	private static readonly MethodInfo UnboxPointer = typeof(Pointer).GetMethod("Unbox");

	public ConstructorInjector Create(ConstructorInfo constructor)
	{
		DynamicMethod dynamicMethod = new DynamicMethod(GetAnonymousMethodName(), typeof(object), new Type[1] { typeof(object[]) }, constructor.Module, skipVisibility: true);
		ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
		EmitLoadMethodArguments(iLGenerator, constructor);
		iLGenerator.Emit(OpCodes.Newobj, constructor);
		if (constructor.ReflectedType.IsValueType)
		{
			iLGenerator.Emit(OpCodes.Box, constructor.ReflectedType);
		}
		iLGenerator.Emit(OpCodes.Ret);
		return (ConstructorInjector)dynamicMethod.CreateDelegate(typeof(ConstructorInjector));
	}

	public PropertyInjector Create(PropertyInfo property)
	{
		DynamicMethod dynamicMethod = new DynamicMethod(GetAnonymousMethodName(), typeof(void), new Type[2]
		{
			typeof(object),
			typeof(object)
		}, property.Module, skipVisibility: true);
		ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		EmitUnboxOrCast(iLGenerator, property.DeclaringType);
		iLGenerator.Emit(OpCodes.Ldarg_1);
		EmitUnboxOrCast(iLGenerator, property.PropertyType);
		bool injectNonPublic = base.Settings.InjectNonPublic;
		EmitMethodCall(iLGenerator, property.GetSetMethod(injectNonPublic));
		iLGenerator.Emit(OpCodes.Ret);
		return (PropertyInjector)dynamicMethod.CreateDelegate(typeof(PropertyInjector));
	}

	public MethodInjector Create(MethodInfo method)
	{
		DynamicMethod dynamicMethod = new DynamicMethod(GetAnonymousMethodName(), typeof(void), new Type[2]
		{
			typeof(object),
			typeof(object[])
		}, method.Module, skipVisibility: true);
		ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		EmitUnboxOrCast(iLGenerator, method.DeclaringType);
		EmitLoadMethodArguments(iLGenerator, method);
		EmitMethodCall(iLGenerator, method);
		if (method.ReturnType != typeof(void))
		{
			iLGenerator.Emit(OpCodes.Pop);
		}
		iLGenerator.Emit(OpCodes.Ret);
		return (MethodInjector)dynamicMethod.CreateDelegate(typeof(MethodInjector));
	}

	private static void EmitLoadMethodArguments(ILGenerator il, MethodBase targetMethod)
	{
		ParameterInfo[] parameters = targetMethod.GetParameters();
		OpCode opcode = ((targetMethod is ConstructorInfo) ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
		for (int i = 0; i < parameters.Length; i++)
		{
			il.Emit(opcode);
			il.Emit(OpCodes.Ldc_I4, i);
			il.Emit(OpCodes.Ldelem_Ref);
			EmitUnboxOrCast(il, parameters[i].ParameterType);
		}
	}

	private static void EmitMethodCall(ILGenerator il, MethodInfo method)
	{
		OpCode opcode = (method.IsFinal ? OpCodes.Call : OpCodes.Callvirt);
		il.Emit(opcode, method);
	}

	private static void EmitUnboxOrCast(ILGenerator il, Type type)
	{
		if (type.IsValueType)
		{
			il.Emit(OpCodes.Unbox_Any, type);
		}
		else if (type.IsPointer)
		{
			il.Emit(OpCodes.Call, UnboxPointer);
		}
		else
		{
			il.Emit(OpCodes.Castclass, type);
		}
	}

	private static string GetAnonymousMethodName()
	{
		return "DynamicInjector" + Guid.NewGuid().ToString("N");
	}
}
