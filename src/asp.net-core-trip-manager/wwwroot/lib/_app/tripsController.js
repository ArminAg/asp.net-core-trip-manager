!function(){"use strict";function i(i,t){var e=this;e.trips=[],e.newTrip={},e.isBusy=!0;var n="/api/trips";$(".page-title").text("My Trips"),i.get(n).then(function(i){angular.copy(i.data,e.trips)},function(i){Materialize.toast("Failed to load data: "+i,3e3)}).finally(function(){e.isBusy=!1}),e.addTrip=function(){e.isBusy=!0,console.log(e.newTrip),i.post(n,e.newTrip).then(function(i){e.trips.push(i.data),e.newTrip={},t.newTripForm.$setUntouched(),t.newTripForm.$setPristine()},function(i){Materialize.toast("Failed to save new Trip",3e3)}).finally(function(){e.isBusy=!1})},e.deleteTrip=function(t){e.isBusy=!0,i.delete(n+"/"+t.id).then(function(){var i=e.trips.indexOf(t);e.trips.splice(i,1)},function(){Materialize.toast("Failed to delete Trip",3e3)}).finally(function(){e.isBusy=!1})}}i.$inject=["$http","$scope"],angular.module("app-trips").controller("tripsController",i)}();