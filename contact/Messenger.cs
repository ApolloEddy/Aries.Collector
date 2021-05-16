using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{
	public class Messenger
	{
		public string title { get; set; }
		public int top
		{
			get
			{
				return Console.CursorTop;
			}
			set
			{
				Console.CursorTop = value;
			}
		}
		public int left
		{
			get
			{
				return Console.CursorLeft;
			}
			set
			{
				Console.CursorLeft = left;
			}
		}
		private ConsoleProcess process { get; set; }

		public Messenger() { title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; setTitle(title); }
		public Messenger(string title) { this.title = title; setTitle(title); }

		public void put(string message) { Console.Write(message); }
		public void puts() { Console.WriteLine(); }
		public void puts(string message) { Console.WriteLine(message); }
		public void puts(List<object> list)
		{
			foreach (object item in list)
			{
				Console.WriteLine(item.ToString());
			}
		}
		public void puts(Array array)
		{
			foreach (object item in array)
			{
				Console.WriteLine(item.ToString());
			}
		}
		public void error(string message) { pop("Error", message, ConsoleColor.Red); }
		public void success(string message) { pop("Success", message, ConsoleColor.Green); }
		public void warn(string message) { pop("Warnning", message, ConsoleColor.Yellow); }
		/// <summary>
		/// 等待用户输入指定的键后继续
		/// </summary>
		/// <param name="key">指定的字符</param>
		/// <param name="message">提示信息</param>
		public void waitkey(char key, string message = "\0")
		{
			Console.ResetColor();
			if (left != 0) { Console.WriteLine(); }
			Console.Write(message);
			while (Console.ReadKey().KeyChar != key)
			{
				continue;
			}
			Console.WriteLine();
		}
		public void startProcess(int start, int max, int step = 1, string description = "\0") 
		{ process = new ConsoleProcess(start, max, step, description); process.Start(); }
		public void stepProcess() { process.Step(); }
		public void stepProcess(int step) { process.Step(step); }

		/// <summary>
		/// 以某种颜色提示符发出指定消息
		/// </summary>
		/// <param name="message">要发出的字符串消息</param>
		/// <param name="color">发出的颜色</param>
		private void pop(string message, ConsoleColor color)
		{
			Console.ResetColor();
			Console.ForegroundColor = color;
			Console.Write(message);
			Console.ResetColor();
		}
		private void pop(string label, string message, ConsoleColor color)
		{
			Console.ResetColor();
			Console.ForegroundColor = color;
			Console.Write($"[{label}]");
			fill(11);
			Console.ResetColor();
			Console.WriteLine(message);
		}
		private void fill(int left)
		{
			while(this.left < left)
			{
				Console.Write(" ");
			}
		}
		private void setTitle(string title) { Console.Title = title; }
	}
}
