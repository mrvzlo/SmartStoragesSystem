using System.Web.Optimization;

namespace SmartKitchen.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css").Include(
                "~/Content/MvcGrid/mvc-grid.css", 
                "~/Content/main.css"
            ));

            bundles.Add(new ScriptBundle("~/core").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/bootstrap.js"
            ));

            bundles.Add(new ScriptBundle("~/forms").Include(
                "~/Scripts/jquery.form.js",
                "~/Scripts/pages/form-helper.js"
            ));

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

            bundles.Add(new ScriptBundle("~/js/storageCreateType").Include(
                "~/Scripts/pages/storage-create-type.js"
            ));

            bundles.Add(new ScriptBundle("~/js/storageCreate").Include(
                "~/Scripts/pages/storage-create.js"
            ));

            bundles.Add(new ScriptBundle("~/js/storageIndex").Include(
                "~/Scripts/pages/storage-index.js"
            ));

            bundles.Add(new ScriptBundle("~/js/storageView").Include(
                "~/Scripts/pages/storage-view.js"
            ));

            bundles.Add(new ScriptBundle("~/js/amountPicker").Include(
                "~/Scripts/pages/amount-picker.js"
            ));

            bundles.Add(new ScriptBundle("~/js/datetimePicker").Include(
                "~/Scripts/pages/datetime-picker.js"
            ));

            #endregion

        }
    }
}
