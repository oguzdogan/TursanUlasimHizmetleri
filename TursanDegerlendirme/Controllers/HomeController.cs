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
            Uri URL = new Uri("https://www.google.com.tr");
            WebClient wc = new WebClient();
            double starttime = Environment.TickCount;
            wc.DownloadFile(URL, Server.MapPath("file/speedtest.txt"));
            double endtime = Environment.TickCount;
            double secs = Math.Floor(endtime - starttime) / 1000;
            double secs2 = Math.Round(secs, 0);
            double kbsec = Math.Round(1024 / secs);
            if (Session["rate"]==null)
            {
                Session["rate"] = 0;
            }
            if (Convert.ToDouble(Session["rate"])>kbsec)
            {
                TempData["rateerror"] = "Bağlantı Hızı Düştü";
            }
            Session["rate"] = kbsec;
            PerformanceCounter availableBytes = new PerformanceCounter("Memory", "Available MBytes", true);
            float numBytes = availableBytes.NextValue();
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            var unused = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            if (Session["cpu"] == null)
            {
                Session["cpu"] = 0;
            }
            float a = cpuCounter.NextValue();
            if (float.Parse(Session["cpu"].ToString()) > a)
            {
                TempData["cpuerror"] = "CPU Değeri Düştü";
            }
            if (Session["ram"] == null)
            {
                Session["ram"] = 0;
            }
            if (float.Parse(Session["ram"].ToString()) > float.Parse(numBytes.ToString()))
            {
                TempData["ramerror"] = "Ram Değeri Düştü";
            }
            Session["cpu"] = a.ToString();
            Session["ram"] = numBytes.ToString();
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