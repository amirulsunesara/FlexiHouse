using System.Web;
using System.Web.Optimization;

namespace FlexiHouseFinal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           bundles.Add(new ScriptBundle("~/bundles/script").Include("~/Scripts/bootstrap.min.js", "~/Scripts/Custom.js", "~/Scripts/CustomCollision.js", "~/Scripts/jquery-1.7.2.min.js", "~/Scripts/jquery-ui.min.js", "~/Scripts/jQueryRotate.js", "~/Scripts/listgroup.min.js", "~/Scripts/material.min.js", "~/Scripts/menu.js", "~/Scripts/bootstrap-modal.js"));
            bundles.Add(new ScriptBundle("~/bundles/zoom").Include("~/Scripts/zoom/jquery.zoomooz.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/script2").Include("~/Scripts/bootstrap.min.js", "~/Scripts/Custom.js", "~/Scripts/jquery-1.7.2.min.js", "~/Scripts/CustomCollision.js", "~/Scripts/jquery-ui.min.js", "~/Scripts/listgroup.min.js",  "~/Scripts/menu.js", "~/Scripts/bootstrap-modal.js", "~/Scripts/Countries.js", "~/Scripts/Consignment.js", "~/Scripts/ShelveView.js"));

            bundles.Add(new StyleBundle("~/ContentZ/css").Include("~/Content/CustomCollision.css", "~/Content/menu.css"));
            bundles.Add(new StyleBundle("~/ContentZ/ZoomCss").Include("~/Content/zoomCss/jquery.gzoom.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/defaultScripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/defaultScripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/defaultScripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/defaultScripts/bootstrap.js",
                      "~/Scripts/defaultScripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/defaultContent/bootstrap.css",
                      "~/Content/defaultContent/site.css"));
        }
    }
}
