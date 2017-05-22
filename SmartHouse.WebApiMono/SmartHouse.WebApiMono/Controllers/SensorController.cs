﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Sensor")]
	public class SensorController : BaseController
	{
		private readonly ITelemetryService TelemetryService;

		public SensorController(ISettingsService settingsService) 
            : base(settingsService)
		{
            this.TelemetryService = new TelemetryService();
		}

		[HttpPost]
		[Route("SaveTemperature")]
		public Result SaveTemperature(TelemetryData data)
		{
			return TelemetryService.SaveTemperature(data);
		}

		[HttpGet]
		[Route("GetCurrentTemperature")]
		public async Task<TelemetryData> GetCurrentTemperature()
		{
			return await TelemetryService.GetLastTemperature();
		}

        [HttpGet]
        [Route("GetAirConditionState")]
        public async Task<byte> GetAirConditionState()
        {
            return await TelemetryService.GetAirConditionState();
        }

        [HttpPost]
        [Route("AirCondition")]
        public async Task<Result> AirCondition(byte On)
        {
            return await TelemetryService.AirCondition(On);
        }
	}
}
