using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{
	public class ArgumentParser
	{
		public ArgumentParser()
		{
			dict = new Dictionary<Argument, Value>();
			argList = new List<Argument>();
		}
		public Dictionary<Argument, Value> dict { get; set; }
		public List<Argument> argList { get; set; }
		public Value this[Argument arg]
		{
			get
			{
				return arg.value;
			}
		}
		public void Parse(string[] args)
		{
			foreach (string v in args)
			{
				foreach (Argument arg in dict.Keys)
				{
					if (new Value(v).vertify(arg)) { arg.value = new Value(v); dict.Add(arg, new Value(v)); }
				}			
			}
		}
		public void add(Argument arg)
		{
			argList.Add(arg);
		}
	}
}
