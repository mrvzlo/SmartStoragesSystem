using System.Web.Optimization;

namespace SmartKitchen.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/core").Include(
                    "~/Scripts/jquery-{version}.js", 
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/pages/form-helper.js"
            ));

            bundles.Add(new StyleBundle("~/css").Include(
                "~/Content/My.css"));

            bundles.Add(new ScriptBundle("~/MvcGrid").Include(
                "~/Scripts/MvcGrid/mvc-grid.js"));

            #region Pages

            bundles.Add(new ScriptBundle("~/js/accountIndex").Include(
                "~/Scripts/pages/account-index.js"
            ));

            bundles.Add(new ScriptBundle("~/js/productIndex").Include(
                "~/Scripts/pages/product-index.js"
            ));

            bundles.Add(new ScriptBundle("~/js/basketIndex").Include(
                "~/Scripts/pages/basket-index.js"
            ));

            bundles.Add(new ScriptBundle("~/js/categoryIndex").Include(
                "~/Scripts/pages/category-index.js"
            ));

            #endregion

        }
    }
}
