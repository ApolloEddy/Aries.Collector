using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{
	public class ConsoleProcess
	{
		public ConsoleProcess(int start, int max, int step = 1, string description = "\0")
		{
			title = Console.Title;
			this.start = start;
			this.max = max;
			this.step = step;
			this.description = description;
			current = start;
		}
		public bool changeTitle = true;
		public bool caculate = true;
		public string title;
		public string description;
		public readonly int start, max, step;
		public int current;
		public int top, left;

		private Messenger msg = new Messenger();
		private string process 
		{ 
			get 
			{
				string p = $"进度：[{string.Format("{0:D" + max.ToString().Length + "}", current)}/{max}";
				if (caculate) { p += $" 合 {string.Format("{0:F2}", current * 100 / max)}%"; }
				p += "]";
				return p;
			} 
		}

		public void Start()
		{
			if(description != "\0") { description += "  "; }
			msg.put(description);
			if (changeTitle) { setTitle(title + "  " + process); }
			top = Console.CursorTop;
			left = Console.CursorLeft;
			msg.put(process);
		}
		public void Step(int step)
		{
			current += step;
			Console.SetCursorPosition(left, top);
			if (changeTitle) { setTitle(title + "  " + description + "  " + process); }
			msg.put(process);
		}
		public void Step() { Step(step); }

		private void setTitle(string title) { Console.Title = title; }
		private string intFormation(int value)
		{
			string ret = string.Empty;
			for(int i = 0; i < value.ToString().Length; i++)
			{
				ret += "x";
			}
			return ret;
		}
	}
}
