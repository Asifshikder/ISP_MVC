using System.Web;
using System.Web.Optimization;

namespace Project_ISP
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
              //"~/Scripts/LayoutScripts/jquery.min.js",
              "~/Scripts/LayoutScripts/jquery-ui.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/AdminLayoutJqueryScript").Include(

            //  "~/Scripts/LayoutScripts/jquery.min.js",
            //  "~/Scripts/LayoutScripts/jquery-ui.min.js"

            //  ));
            bundles.Add(new ScriptBundle("~/bundle/AppUtil").Include(
                      "~/Scripts/moment-with-locales.js",
                     "~/Scripts/CustomScripts/AppUtil.js",
                      "~/Scripts/notify.js",
                    // "~/Scripts/CustomScripts/waitingDialogManager.js",
                     "~/Scripts/CustomScripts/SearchClientInformationManager.js"
                     //,"~/Scripts/CustomScripts/ClientUpdateFromDashBoardManager.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/AdminLayoutScript").Include(

                "~/Scripts/LayoutScripts/bootstrap.min.js",
                 "~/Scripts/LayoutScripts/jquery.slimscroll.min.js",
                  "~/Scripts/LayoutScripts/jquery.sparkline.min.js",
                 "~/Scripts/LayoutScripts/jquery-jvectormap-1.2.2.min.js",
                 //"~/Scripts/LayoutScripts/jquery-jvectormap-world-mill-en.js",
                 "~/Scripts/LayoutScripts/jquery.knob.min.js",
                  //  "~/Scripts/LayoutScripts/moment.min.js",
                  //  "~/Scripts/LayoutScripts/daterangepicker.js",
                  //"~/Scripts/LayoutScripts/bootstrap-datepicker.min.js",
                  "~/Scripts/LayoutScripts/bootstrap3-wysihtml5.all.min.js",
                  "~/Scripts/LayoutScripts/jquery.slimscroll.min.js",
                  "~/Scripts/LayoutScripts/fastclick.js",
                  "~/Scripts/LayoutScripts/adminlte.min.js",
                  "~/Scripts/LayoutScripts/dashboard.js",
                  "~/Scripts/LayoutScripts/demo.js"
                ));



            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"
            //          //"~/Scripts/respond.js"
            //));


            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css"
            //          //"~/Content/site.css"
            //          ));

            bundles.Add(new StyleBundle("~/bundles/AdminLayoutStyles").Include(

                    "~/Content/LayoutContent/css/bootstrap.min.css",
                    "~/Content/LayoutContent/css/ionicons.min.css",
                    "~/Content/LayoutContent/css/AdminLTE.min.css",
                    "~/Content/LayoutContent/css/_all-skins.min.css",
                    "~/Content/LayoutContent/css/morris.css",
                    "~/Content/LayoutContent/css/jquery-jvectormap.css",
                    //"~/Content/LayoutContent/css/bootstrap-datepicker.min.css",
                    //  "~/Content/LayoutContent/css/daterangepicker.css",
                    "~/Content/LayoutContent/css/bootstrap3-wysihtml5.min.css"

                ));
        }
    }
}
