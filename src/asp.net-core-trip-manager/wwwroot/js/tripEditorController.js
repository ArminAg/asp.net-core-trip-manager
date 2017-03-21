
(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http, $scope) {
        var vm = this;
        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.isBusy = true;
        vm.newStop = {};
        var url = "/api/trips/" + vm.tripName + "/stops";
        $(".page-title").text(vm.tripName);

        $http.get(url)
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            }, function (error) {
                // Failure
                Materialize.toast("Failed to load Stops", 3000);
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {
            vm.isBusy = true;
            $http.post(url, vm.newStop)
                .then(function (response) {
                    // Success
                    vm.stops.push(response.data);
                    vm.newStop = {};
                    $scope.newStopForm.$setUntouched();
                    $scope.newStopForm.$setPristine();
                    _showMap(vm.stops);
                }, function () {
                    // Failure
                    Materialize.toast("Failed to add new Stop", 3000);
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };

        vm.deleteStop = function (stop) {
            vm.isBusy = true;

            $http.delete(url + "/" + stop.id)
                .then(function () {
                    // Success
                    var index = vm.stops.indexOf(stop);
                    vm.stops.splice(index, 1);
                    _showMap(vm.stops);
                }, function () {
                    // Failure
                    Materialize.toast("Failed to delete Stop", 3000);
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }

    function _showMap(stops) {

        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });

            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: stops.length -1,
                initialZoom: 11,
                icon: {           // Icon details
                    url: "../img/map-pin.png",
                    width: 3,
                    height: 3,
                },
                pastStroke: {     // Settings for the lines before the currentStop
                    color: '#34495e',
                    opacity: 0.5,
                    weight: 2
                },
                futureStroke: {   // Settings for hte lines after the currentStop
                    color: '#34495e',
                    opacity: 0.6,
                    weight: 2
                },
                mapOptions: {     // Options for map (See GMaps for full list of options)
                    styles: [
    {
        "featureType": "all",
        "elementType": "geometry",
        "stylers": [
            {
                "lightness": "40"
            }
        ]
    },
    {
        "featureType": "landscape.natural",
        "elementType": "geometry.fill",
        "stylers": [
            {
                "visibility": "on"
            },
            {
                "color": "#92c83e"
            },
            {
                "lightness": "70"
            },
            {
                "saturation": "-25"
            }
        ]
    },
    {
        "featureType": "road",
        "elementType": "labels",
        "stylers": [
            {
                "visibility": "off"
            }
        ]
    },
    {
        "featureType": "road.highway",
        "elementType": "geometry.fill",
        "stylers": [
            {
                "visibility": "on"
            },
            {
                "color": "#d2d2d2"
            }
        ]
    },
    {
        "featureType": "road.highway",
        "elementType": "geometry.stroke",
        "stylers": [
            {
                "color": "#bebebe"
            },
            {
                "visibility": "off"
            }
        ]
    },
    {
        "featureType": "road.highway.controlled_access",
        "elementType": "geometry.fill",
        "stylers": [
            {
                "visibility": "on"
            },
            {
                "color": "#b9b9b9"
            }
        ]
    },
    {
        "featureType": "road.highway.controlled_access",
        "elementType": "geometry.stroke",
        "stylers": [
            {
                "color": "#d2d2d2"
            },
            {
                "visibility": "on"
            }
        ]
    },
    {
        "featureType": "water",
        "elementType": "all",
        "stylers": [
            {
                "color": "#46acb9"
            },
            {
                "lightness": "65"
            },
            {
                "saturation": "-15"
            }
        ]
    }
                    ]
                }
            });
        }

    }

})();