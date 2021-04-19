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
        public IActionResult Index(String nombre, String plato, int cantidad)
        {
            ViewBag.Mensaje = "Su pedido es: " + cantidad + " unidades de " + plato + "Gracias "
                + nombre;

            this.telemetryClient.TrackEvent("Pedidos Realizado");

            MetricTelemetry metricPedido = new MetricTelemetry();
            metricPedido.Name = "Pedidos bocadillos";
            metricPedido.Sum = cantidad;
            this.telemetryClient.TrackMetric(metricPedido);

            String mensaje = "El pedido de " + nombre + " es: " + cantidad + " de bocadillos de " + plato;

            TraceTelemetry trace = new TraceTelemetry(mensaje);
            this.telemetryClient.TrackTrace(trace);

            ////Trace.TraceInformation
            ////    ("El pedido de " + nombre + " es: " + cantidad + " de bocadillos de " + plato);

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
