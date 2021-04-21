using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcPedidosAppInsights.Models;

namespace MvcPedidosAppInsights.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetryClient;

        public HomeController(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(String nombre, String plato, int cantidadbocadillos, String postre, int cantidadpostres)
        {
            ViewBag.Mensaje = "Gracias por realizar su pedido.";

            this.telemetryClient.TrackEvent("Pedido de Bocadillos");
            MetricTelemetry metricBocadillo= new MetricTelemetry();
            metricBocadillo.Name = "Pedidos bocadillos";
            metricBocadillo.Sum = cantidadbocadillos;
            this.telemetryClient.TrackMetric(metricBocadillo);

            this.telemetryClient.TrackEvent("Pedido de Postre");
            MetricTelemetry metricPostre = new MetricTelemetry();
            metricPostre.Name = "Pedidos postre";
            metricPostre.Sum = cantidadpostres;
            this.telemetryClient.TrackMetric(metricPostre);

            String mensajeBocadillo = "Nombre: " + nombre + ". Unidades: " + cantidadbocadillos + ". Bocadillo: "+ plato;
            String mensajePostre = "Nombre: " + nombre + ". Unidades: " + cantidadpostres + ". Postre: " + postre;

            TraceTelemetry traceBocadillo = new TraceTelemetry(mensajeBocadillo);
            TraceTelemetry tracePostres = new TraceTelemetry(mensajePostre);

            this.telemetryClient.TrackTrace(traceBocadillo);
            this.telemetryClient.TrackTrace(tracePostres);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
