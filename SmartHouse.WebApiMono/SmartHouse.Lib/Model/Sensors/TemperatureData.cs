﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Lib
{
	public class TemperatureData
	{
        [Required]
        [JsonProperty(PropertyName = "temperature")]
        public decimal Temperature { get; set; }

        [Required]
        [JsonProperty(PropertyName = "humidity")]
        public decimal Humidity { get; set; }

        [Required]
        [JsonProperty(PropertyName = "heatIndex")]
        public decimal HeatIndex { get; set; }

        [Required]
        [JsonProperty(PropertyName = "measured")]
        public DateTime Measured { get; set; }
	}
}
