
(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: { // Visible inside the template
                show: "=displayWhen" // Use from the consumer of directive
            },
            restrict: "E",
            templateUrl: "/views/waitCursor.html"
        };
    }

})();