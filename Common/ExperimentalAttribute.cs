namespace Search.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ExperimentalAttribute : Attribute
	{
		public string Name { get; set; } = @"";
		public ExperimentalAttribute() { }
		public ExperimentalAttribute(string name) { this.Name = name; }
	}
}

