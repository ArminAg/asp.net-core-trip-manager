
(function () {
    "use strict";

    // Getting the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http, $scope) {
        var vm = this;
        vm.trips = [];
        vm.newTrip = {};
        vm.isBusy = true;
        var url = "/api/trips";
        $(".page-title").text("My Trips");
        
        $http.get(url)
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
            $http.post(url, vm.newTrip)
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

        vm.deleteTrip = function (trip) {
            vm.isBusy = true;

            $http.delete(url + "/" + trip.id)
                .then(function () {
                    // Success
                    var index = vm.trips.indexOf(trip);
                    vm.trips.splice(index, 1);
                }, function () {
                    // Failure
                    Materialize.toast("Failed to delete Trip", 3000);
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }

})();