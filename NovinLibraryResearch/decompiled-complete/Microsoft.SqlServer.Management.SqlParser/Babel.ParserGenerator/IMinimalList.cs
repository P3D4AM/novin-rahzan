namespace Babel.ParserGenerator;

public interface IMinimalList<T>
{
	int Count { get; }

	T this[int index] { get; }
}
