(function () {
    'use strict';
    var homeFolder = document.getElementsByTagName('base')[0].href == "http://local.visualstudio.com:9009/" ? "" : "/Areas/Budgeter/NgApp/";
    angular.module('app')
        .config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
            var routes, setRoutes;

            routes = [
                'welcome',
                'households',
                'ui/typography', 'ui/buttons', 'ui/icons', 'ui/grids', 'ui/widgets', 'ui/components', 'ui/boxes', 'ui/timeline', 'ui/nested-lists', 'ui/pricing-tables', 'ui/maps',
                'table/static', 'table/dynamic', 'table/responsive',
                'form/elements', 'form/layouts', 'form/validation', 'form/wizard',
                'chart/echarts', 'chart/echarts-line', 'chart/echarts-bar', 'chart/echarts-pie', 'chart/echarts-scatter', 'chart/echarts-more',
                'page/404', 'page/500', 'page/blank', 'page/forgot-password', 'page/invoice', 'page/lock-screen', 'page/profile', 'page/invoice', 'page/signin', 'page/signup',
                'app/tasks', 'app/calendar'
            ]

            setRoutes = function(route) {
                var config, url;
                url = '/' + route;
                config = {
                    templateUrl: homeFolder + 'app/' + route + '.html'
                };
                $routeProvider.when(url, config);
                return $routeProvider;
            };

            routes.forEach(function(route) {
                return setRoutes(route);
            });

            $routeProvider
                .when('/', {redirectTo: '/welcome'})
                .when('/welcome', { redirectTo: '/households' })
                .when('/households', { templateUrl: homeFolder + 'app/households/households.html' })
                .when('/404', {templateUrl: homeFolder + 'app/page/404.html'})
                .otherwise({ redirectTo: '/404' });

            $locationProvider.html5Mode(true);
        }]
    );

})(); 