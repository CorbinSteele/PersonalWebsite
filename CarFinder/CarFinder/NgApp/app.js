var app = angular.module("CarFinderApp", ['trNgGrid', 'ngAnimate', 'ui.bootstrap']);
angular.module("CarFinderApp").controller("CarFinderController", carFinderController);

app.directive("modalShow", function ($parse) {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            //Hide or show the modal
            scope.showModal = function (visible, elem) {
                if (!elem)
                    elem = element;

                if (visible)
                    $(elem).modal("show");
                else
                    $(elem).modal("hide");
            }
            //Watch for changes to the modal-visible attribute
            scope.$watch(attrs.modalShow, function (newValue, oldValue) {
                scope.showModal(newValue, attrs.$$element);
                if (newValue)
                    $(element).animate({ scrollTop: 33 }, { duration: 1200 });
            });

            /*
            //Prevent scrolling images when the modal is about to be shown
            $(element).on('show.bs.modal', function () {
                $(element).find('img').addClass("no-animation");
            });
            */
            //Start scrolling images when the modal is finished being shown
            $(element).on('shown.bs.modal', function () {
                scope.slideSpeed = scope.defaultSlideSpeed;
            });
            //Update the visible value when the dialog is closed through UI actions (Ok, cancel, etc.)
            $(element).on("hide.bs.modal", function () {
                $parse(attrs.modalShow).assign(scope, false);
                scope.slideSpeed = 0;
                //scope.scrollImage(false);
                if (!scope.$$phase && !scope.$root.$$phase)
                    scope.$apply();
            });
        }
    };
});