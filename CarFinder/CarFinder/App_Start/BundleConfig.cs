﻿using System.Web;
using System.Web.Optimization;

namespace CarFinder
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/carfinder/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/trNgGrid.min.css",
                      "~/Content/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/carfinder/js").Include(
                        "~/Scripts/carfinder/bootstrap.js",
                        "~/Scripts/carfinder/angular.js",
                        "~/Scripts/carfinder/ui-bootstrap.js",
                        "~/Scripts/carfinder/*.js",
                        "~/NgApp/Controllers/*.js",
                        "~/NgApp/app.js"));
        }
    }
}
