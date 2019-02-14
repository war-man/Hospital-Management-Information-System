using System.Web;
using System.Web.Optimization;

namespace Caresoft2._0
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-easing-out.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/notify.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jQuery.print.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/kogi_custom.css",
                      "~/Content/site.css"));
        
        }
    }
}
