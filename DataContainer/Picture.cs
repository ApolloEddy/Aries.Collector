using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.DataContainer
{
	public abstract class Picture
	{
		public string title { get; set; }
		public string artist { get; set; }
		public string link { get; set; }
		public DateTime update { get; set; }
	}
}
