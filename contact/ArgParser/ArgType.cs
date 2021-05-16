using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{ 
	public class ArgType
	{
		public ArgType(string token) { this.token = token; }
		public string token { get; set; }
		public static ArgType
			Int = new ArgType("int"), Bool = new ArgType("bool"), String = new ArgType("string");
	}
}
