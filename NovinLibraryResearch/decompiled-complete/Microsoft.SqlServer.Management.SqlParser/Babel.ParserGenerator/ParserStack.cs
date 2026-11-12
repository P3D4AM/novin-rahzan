using System;
using System.Text;

namespace Babel.ParserGenerator;

internal class ParserStack<T>
{
	public T[] array = new T[64];

	public int top;

	public void Push(T value)
	{
		if (top >= array.Length)
		{
			T[] destinationArray = new T[array.Length * 2];
			Array.Copy(array, destinationArray, top);
			array = destinationArray;
		}
		array[top++] = value;
	}

	public T Pop()
	{
		T result = array[--top];
		array[top] = default(T);
		return result;
	}

	public void Pop(int count)
	{
		while (count > 0)
		{
			array[--top] = default(T);
			count--;
		}
	}

	public T Top()
	{
		return array[top - 1];
	}

	public bool IsEmpty()
	{
		return top == 0;
	}

	public void Clear()
	{
		for (int i = 0; i < top; i++)
		{
			array[i] = default(T);
		}
		top = 0;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("(");
		for (int i = 0; i < top; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(array[i].ToString());
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
