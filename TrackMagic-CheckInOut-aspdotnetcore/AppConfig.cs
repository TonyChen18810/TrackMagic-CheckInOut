using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Ports;
using AllNet;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TrackMagic_CheckInOut_aspdotnetcore
{
	public class AppConfig
	{
		public string ScaleCOM { get; set; }
		public string CameraName { get; set; }

		public AppConfig()
		{
			ScaleCOM = "";
			CameraName = "";
		}

		public static AppConfig Load(string fileName)
		{
			if (!File.Exists(fileName))
				return new AppConfig();

			AppConfig config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(fileName));
			return config;
		}
	}
}


