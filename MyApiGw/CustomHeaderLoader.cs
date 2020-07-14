using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MyApiGw
{
	public class CustomHeaderLoader
	{
        public CustomHeaderLoader(IConfiguration config)
        {
            using var reader = new StreamReader(config.GetValue<string>("klzPath"));
            string line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                Logo.Add(line);
            }
        }

		public List<string> Logo { get; } = new List<string>();

    }
}

