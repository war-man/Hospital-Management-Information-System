using System.Web.Mvc;
using System.Net.NetworkInformation;
using System.IO;
using System;

namespace Caresoft2._0
{
    public class LS : ActionFilterAttribute
    {
        private string fileName = "LS2018.txt";
        private string path = Path.GetTempPath();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!licenseExists())
            {
                initLicense();
                filterContext.HttpContext.Session["RequestActivation"] = true;
            }
            else
            {
                if (!isActiveLicense())
                {
                    filterContext.HttpContext.Session["RequestActivation"] = true;
                }
                else
                {
                    filterContext.HttpContext.Session["RequestActivation"] = false;
                }
            }
        }

        public bool isActiveLicense()
        {
            try
            {
                string readText = File.ReadAllText(path + "\\" + mac() + "\\" + fileName);
                string activeMode = EasyMD5.Hash(mac() + "active");

                return readText == activeMode;
            }catch(Exception)
            {

            }
            return false;
        }

        private void initLicense()
        {
            string createText = EasyMD5.Hash(mac() + "inactive");
            FileInfo file = new FileInfo(path + "\\" + mac() + "\\" + fileName);
            file.Directory.Create();
            File.WriteAllText(file.FullName, createText);

        }

        public bool licenseExists()
        {
            return File.Exists(path + "\\" + mac() + "\\" + fileName);
        }

        public void updateLicense(string state)
        {
            string createText = EasyMD5.Hash(mac() + state);
            FileInfo file = new FileInfo(path + "\\" + mac() + "\\" + fileName);
            file.Directory.Create();
            File.WriteAllText(file.FullName, createText);
        }


        public string mac()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return mac;
        }
    }
}