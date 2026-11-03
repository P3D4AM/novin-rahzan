using System;
using System.Collections.Generic;
using PacketParser.Scale.EventArguments;
using PacketParser.Scale.Provider;

namespace PacketParser.Scale;

public interface IScaleConnectionManager
{
	IScaleConnection this[MicroScaleInfo key] { get; set; }

	int Count { get; }

	bool IsReadOnly { get; }

	ICollection<MicroScaleInfo> Keys { get; }

	ICollection<IScaleConnection> Values { get; }

	event EventHandler<ScaleConnectionEventArgs> StatusChanged;

	void Add(KeyValuePair<MicroScaleInfo, IScaleConnection> item);

	void Add(MicroScaleInfo key, IScaleConnection value);

	void Clear();

	bool Contains(KeyValuePair<MicroScaleInfo, IScaleConnection> item);

	bool ContainsKey(MicroScaleInfo key);

	void CopyTo(KeyValuePair<MicroScaleInfo, IScaleConnection>[] array, int index);

	void Dispose();

	IEnumerator<KeyValuePair<MicroScaleInfo, IScaleConnection>> GetEnumerator();

	IScaleCommandProvider GetProvider(MicroScaleInfo key);

	bool Remove(KeyValuePair<MicroScaleInfo, IScaleConnection> item);

	bool Remove(MicroScaleInfo key);

	bool TryGetValue(MicroScaleInfo key, out IScaleConnection value);
}
