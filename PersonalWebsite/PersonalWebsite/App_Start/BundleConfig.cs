﻿using System.Web;
using System.Web.Optimization;

namespace PersonalWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/prism.css",
                      "~/Content/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/jquery-1.10.2.js",
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/Portfolio/css").Include(
                      "~/Areas/Portfolio/Content/javascript-exercises.css"));

            bundles.Add(new ScriptBundle("~/bundles/Portfolio/js").Include(
                        "~/Areas/Portfolio/Scripts/*.js"));

            bundles.Add(new StyleBundle("~/bundles/CarFinder/css").Include(
                      "~/Areas/CarFinder/Content/trNgGrid.min.css",
                      "~/Areas/CarFinder/Content/car-finder-app.css"));

            bundles.Add(new ScriptBundle("~/bundles/CarFinder/js").Include(
                        "~/Areas/CarFinder/Scripts/angular.js",
                        "~/Areas/CarFinder/Scripts/ui-bootstrap.js",
                        "~/Areas/CarFinder/Scripts/*.js",
                        "~/Areas/CarFinder/NgApp/Controllers/*.js",
                        "~/Areas/CarFinder/NgApp/app.js"));

            /*bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));*/
        }
    }
}