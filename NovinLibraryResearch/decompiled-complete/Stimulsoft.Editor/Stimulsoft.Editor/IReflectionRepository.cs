using System;
using System.Collections;
using System.Reflection;

namespace Stimulsoft.Editor;

public interface IReflectionRepository : ICodeCompletionRepository
{
	Hashtable Types { get; }

	Hashtable Objects { get; }

	IList Assemblies { get; }

	IList Namespaces { get; }

	void RegisterNamespace(string nspace);

	bool UnregisterNamespace(string nspace);

	void RegisterObject(string name, object obj);

	bool UnregisterObject(string name);

	void RegisterType(string name, Type type);

	void RegisterType(string name, Type type, bool global);

	bool UnregisterType(string name);

	void RegisterAssembly(Assembly assembly);

	bool RegisterAssembly(string name);

	bool UnregisterAssembly(Assembly assembly, bool removeReferences);

	bool UnregisterAssembly(string name, bool removeReferences);

	void RegisterAllAssemblies();

	void RegisterDefaultAssemblies();

	void RestrictTypeMembers(Type type);

	void AllowTypeMembers(Type type);

	void ClearNamespaces();

	void ClearObjects();

	void ClearAssemblies();

	void ClearTypes();
}
