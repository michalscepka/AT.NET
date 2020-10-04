using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cv1
{
	delegate void StackEventHandler();

	class EmptyStackException : Exception { }

	class MyStack<T> : IEnumerable<T>
	{
		private T[] data;
		private int index;

		//"event" zaridi ze se nebude moct volat z jinych trid nez je tato
		public event StackEventHandler OnPush;
		public event StackEventHandler OnPop;

		public int Top
		{
			get
			{
				if (this.index == -1)
					throw new EmptyStackException();
				else
					return this.index;
			}
		}

		public MyStack()
		{
			data = new T[10];
			index = -1;
		}

		public void Push(T new_element)
		{
			data[++index] = new_element;
			OnPush?.Invoke();
		}

		public T Pop()
		{
			OnPop?.Invoke();
			return data[index--];
		}

		public IEnumerator<T> GetEnumerator()
		{
			for(int i = index; i >= 0; i--)
			{
				yield return data[i];
			}
		}

		// druha metoda, kvuli starym verzim .NET
		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = index; i >= 0; i--)
			{
				yield return data[i];
			}
		}
	}
}
