using System.Web;
using System.Web.Optimization;

namespace WebBanHangOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Core CSS
            bundles.Add(new StyleBundle("~/Content/core").Include(
                "~/Content/assets/styles/bootstrap4/bootstrap.min.css"));

            // Plugin CSS
            bundles.Add(new StyleBundle("~/Content/plugins").Include(
                "~/Content/assets/plugins/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/Content/assets/plugins/OwlCarousel2-2.2.1/owl.carousel.css",
                "~/Content/assets/plugins/OwlCarousel2-2.2.1/owl.theme.default.css",
                "~/Content/assets/plugins/OwlCarousel2-2.2.1/animate.css",
                "~/Content/assets/plugins/jquery-ui-1.12.1.custom/jquery-ui.css"));

            // Global CSS
            bundles.Add(new StyleBundle("~/Content/global").Include(
                "~/Content/assets/styles/main_styles.css",
               
                "~/Content/assets/styles/responsive.css"
               
                ));
        }
    }
}
