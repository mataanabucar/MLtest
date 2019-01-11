using System.Web;
using System.Web.Optimization;

namespace Loftus
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.js",
						"~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/bootstrap.css",
						"~/Content/site.css"));


			bundles.Add(new ScriptBundle("~/bundles/MyScripts")
						.IncludeDirectory("~/MyContent/JS", "*.js", true));


			bundles.Add(new ScriptBundle("~/bundles/lightbox").Include(
						"~/MyContent/Test/LightBoxAll/js/lightbox.js"));

			//bundles.Add(new StyleBundle("~/Content/lightbox").Include(
			//"~/MyContent/Test/LightBoxAll/css/lightbox.css",
			//"~/MyContent/Test/LightBoxAll/images/*.png",
			//"~/MyContent/Test/LightBoxAll/images/*.gif"));

			bundles.Add(new StyleBundle("~/bundles/MyStyles")
				.IncludeDirectory("~/MyContent/CSS", "*.css", true));
		}
	}
}
