namespace Search.Attributes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class SlowAttribute : Attribute
	{
	};
};
