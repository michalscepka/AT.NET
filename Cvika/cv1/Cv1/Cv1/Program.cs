using System;

namespace Cv1
{
	class Program
	{
		static void Main(string[] args)
		{
			MyStack<int> st = new MyStack<int>();

			st.OnPush += () => Console.WriteLine("Pushed");
			st.OnPop += () => Console.WriteLine("Poped");

			//Console.WriteLine("Top: " + st.Top);
			st.Push(1);
			Console.WriteLine("Top: " + st.Top);
			st.Push(2);
			Console.WriteLine("Top: " + st.Top);

			foreach(int x in st)
			{
				Console.WriteLine(x);
			}

			Console.WriteLine("Pop: " + st.Pop());

			Console.WriteLine("Top: " + st.Top);
			Console.WriteLine("Pop: " + st.Pop());

			//Console.WriteLine("Top: " + st.Top);
			//Console.WriteLine(st.Pop());
		}
	}
}
