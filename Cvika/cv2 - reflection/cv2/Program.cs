using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CalcLib
{
	class Program
	{
		static void Main(string[] args)
		{
			//Ukol1();
			Ukol2();
		}

		static void Ukol1()
		{
			string path = @"C:\Users\Michal\Dropbox\School\5_semestr\AT.NET\Cvika\cv2 - reflection\CalcLib\bin\Debug\netcoreapp3.1\CalcLib.dll";
			path = Path.GetFullPath(path);

			Assembly calclib = Assembly.LoadFile(path);
			Type[] types = calclib.GetTypes();

			Type calcType = calclib.GetType("CalcLib.SimpleCalc");

			object calc = calclib.CreateInstance("CalcLib.SimpleCalc");
			//object calc = Activator.CreateInstance(calclib.GetType("CalcLib.SimpleCalc"));

			MethodInfo setXY = calcType.GetMethod("SetXY");

			setXY.Invoke(calc, new object[] { 10, 20 });

			calcType.GetField("x", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(calc, 2);

			calcType.GetMethod("Add").Invoke(calc, new object[0]);
			//calcType.GetMethod("Multiply", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(calc, new object[0]);
			calcType.GetMethod("ShowResult").Invoke(calc, new object[0]);

			foreach (Type t in types)
			{
				Console.WriteLine("Type: " + t.Name);

				Console.WriteLine("Methods:");
				MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				foreach (MethodInfo method in methods)
				{
					string visibility = method.IsPublic ? "public" : method.IsPrivate ? "private" : "protected";
					Console.WriteLine(visibility + " " + method.ReturnType + " " + method.Name);
					foreach (ParameterInfo par in method.GetParameters())
					{
						Console.WriteLine('\t'.ToString() + par.ParameterType + " " + par.Name);
					}
				}

				Console.WriteLine("\nFields:");
				FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo field in fields)
				{
					Console.WriteLine(field.FieldType + " " + field.Name);
				}
			}
		}

		static void Ukol2()
		{
			Person p = new Person()
			{
				Age = 50,
				Name = "Jan"
			};

			string txt = SimpleSerializer.Serialize(p);

			object person = SimpleSerializer.Deserialize(txt, typeof(Person));

			Console.WriteLine(person);
		}
	}

	class SimpleSeralizerIgnoreAttribute : Attribute
	{

	}

	class Person
	{
		[SimpleSeralizerIgnore]
		public int Age { get; set; }
		public string Name { get; set; }
	}

	class SimpleSerializer
	{
		public static string Serialize(object x)
		{
			StringBuilder sb = new StringBuilder();

			Type xType = x.GetType();
			foreach(PropertyInfo prop in xType.GetProperties())
			{
				bool hasIgnoreAttr = prop.GetCustomAttributes(typeof(SimpleSeralizerIgnoreAttribute)).Any();
				if (hasIgnoreAttr)
					continue;

				string name = prop.Name;
				string val = prop.GetValue(x)?.ToString();
				sb.AppendLine(name);
				sb.AppendLine(val);
			}

			return sb.ToString();
		}

		public static object Deserialize(string txt, Type t)
		{
			object obj = Activator.CreateInstance(t);
			//object obj = t.Assembly.CreateInstance(t.FullName);

			string[] lines = txt.Split('\n');
			
			for(int i = 0; i < lines.Length - 1; i += 2)
			{
				string name = lines[i];
				string val = lines[i + 1];

				PropertyInfo prop = t.GetProperty(name);
				if(prop == null)
					continue;

				if(prop.PropertyType == typeof(int))
				{
					prop.SetValue(obj, int.Parse(val));
				}
				else if (prop.PropertyType == typeof(string))
				{
					prop.SetValue(obj, val);
				}

				prop.SetValue(obj, val);
			}

			return obj;
		}
	}
}
