using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Management.Instrumentation;
using System.Management;

namespace TursanDegerlendirme.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Name = "https://www.google.com.tr")
        {
            Uri URL = new Uri(Name);
            WebClient wc = new WebClient();
            double starttime = Environment.TickCount;
            wc.DownloadFile(URL, Server.MapPath("file/speedtest.txt"));
            double endtime = Environment.TickCount;
            double secs = Math.Floor(endtime - starttime) / 1000;
            double secs2 = Math.Round(secs, 0);
            double kbsec = Math.Round(1024 / secs);
            TempData["rate"] = kbsec ;
            PerformanceCounter availableBytes = new PerformanceCounter("Memory", "Available MBytes", true);
            float numBytes = availableBytes.NextValue();
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            var unused = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            TempData["cpu"] = (cpuCounter.NextValue());
            TempData["ram"] = numBytes.ToString();
            availableBytes.Close();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}