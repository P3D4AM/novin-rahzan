using System;

namespace Stimulsoft.Base;

public interface IStiFormRunner : IDisposable
{
	object OwnerWindow { get; set; }

	object this[string key] { get; set; }

	event StiShowDialogCompleteEventHandler Complete;

	void ShowDialog();

	void Create(string formType, string assemblyName);

	void Create(string formType, string assemblyName, params object[] args);
}
