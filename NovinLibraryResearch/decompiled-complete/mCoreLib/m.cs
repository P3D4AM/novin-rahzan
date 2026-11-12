using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.MyServices.Internal;

[GeneratedCode("MyTemplate", "8.0.0.0")]
[HideModuleName]
[StandardModule]
internal sealed class m
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
	internal sealed class a
	{
		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool a(object A_0)
		{
			return base.Equals(RuntimeHelpers.GetObjectValue(A_0));
		}

		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int c()
		{
			return base.GetHashCode();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
		internal Type b()
		{
			return typeof(a);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
		public string a()
		{
			return base.ToString();
		}

		[DebuggerHidden]
		private static T Create__Instance__<T>(T instance) where T : new()
		{
			if (instance == null)
			{
				return new T();
			}
			return instance;
		}

		[DebuggerHidden]
		private void Dispose__Instance__<T>(ref T instance)
		{
			instance = default(T);
		}

		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public a()
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[ComVisible(false)]
	internal sealed class ThreadSafeObjectProvider<T> where T : new()
	{
		private readonly ContextValue<T> m_a;

		[SpecialName]
		[DebuggerHidden]
		internal T a()
		{
			T val = this.a.Value;
			if (val == null)
			{
				val = new T();
				this.a.Value = val;
			}
			return val;
		}

		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ThreadSafeObjectProvider()
		{
			this.a = new ContextValue<T>();
		}
	}

	private static readonly ThreadSafeObjectProvider<l> m_a = new ThreadSafeObjectProvider<l>();

	private static readonly ThreadSafeObjectProvider<n> m_b = new ThreadSafeObjectProvider<n>();

	private static readonly ThreadSafeObjectProvider<User> m_c = new ThreadSafeObjectProvider<User>();

	private static readonly ThreadSafeObjectProvider<a> m_d = new ThreadSafeObjectProvider<a>();

	[SpecialName]
	[DebuggerHidden]
	internal static l d()
	{
		return m.m_a.a();
	}

	[SpecialName]
	[DebuggerHidden]
	internal static n c()
	{
		return m.m_b.a();
	}

	[SpecialName]
	[DebuggerHidden]
	internal static User b()
	{
		return m.m_c.a();
	}

	[SpecialName]
	[DebuggerHidden]
	internal static a a()
	{
		return m.m_d.a();
	}
}
