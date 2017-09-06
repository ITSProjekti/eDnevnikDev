using System.Web;
using System.Web.Optimization;

namespace eDnevnikDev
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",                     
                      "~/Scripts/respond.js",
                      "~/Scripts/lumino.glyphs.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/select2.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/styles.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/site.css"));
        }
    }
}
