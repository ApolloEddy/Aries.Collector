using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{
	public class Argument
	{
		public Argument(ArgType type, string key, bool isdefault = false) 
		{ this.key = key; this.type = type; isDefault = isDefault; }
		public string key { get; }
		public ArgType type { get; }
		public bool isDefault { get; set; }
		public Value value { get; set; }
	}
}
