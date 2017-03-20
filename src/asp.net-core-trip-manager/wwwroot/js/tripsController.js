
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
        
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.trips);
            }, function (error) {
                // Failure
                Materialize.toast("Failed to load data: " + error, 3000);
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addTrip = function () {
            vm.isBusy = true;
            
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
                    Materialize.toast("Failed to save new Trip", 3000);
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }

})();