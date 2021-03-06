﻿
(function () {
    "use strict";

    // Creating the module
    angular.module("app-trips", ["simpleControls", "ngRoute", "ngAnimate"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm",
                templateUrl: "/views/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", {
                controller: "tripEditorController",
                controllerAs: "vm",
                templateUrl: "/views/tripEditorView.html"
            });

            // Default
            $routeProvider.otherwise({
                redirectTo: "/"
            });

        });

})();