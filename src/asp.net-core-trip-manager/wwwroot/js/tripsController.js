
(function () {
    "use strict";

    // Getting the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http, $scope) {
        var vm = this;
        vm.trips = [];

        vm.newTrip = {};
        $(".page-title").text("My Trips");
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.trips);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";
            console.log(vm.newTrip);
            $http.post("/api/trips", vm.newTrip)
                .then(function (response) {
                    // Success
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                    $scope.newTripForm.$setUntouched();
                    $scope.newTripForm.$setPristine();
                }, function (error) {
                    // Failure
                    vm.errorMessage = "Failed to save new Trip";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }

})();