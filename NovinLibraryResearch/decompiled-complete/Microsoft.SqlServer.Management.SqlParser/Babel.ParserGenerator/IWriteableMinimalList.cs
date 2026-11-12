namespace Babel.ParserGenerator;

internal interface IWriteableMinimalList<T> : IMinimalList<T>
{
	new T this[int index] { get; set; }
}
