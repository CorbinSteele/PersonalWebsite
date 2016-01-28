var carFinderController = function ($scope, $http, $q) {
    var ctrl = this;
    $scope.defaultSlideSpeed = 4000;
    this.showDialog = false;
    this.searching = false;
    this.output = {
        years: [],
        makes: [],
        models: [],
        trims: [],
        styles: [],
        cars: [],
        car: {}
    };

    this.input = {
        year: null,
        make: null,
        model: null,
        trim: null,
        style: null
    };

    var getYears = function () {
        var options = {
            params: { make: ctrl.input.make, model: ctrl.input.model, trim: ctrl.input.trim, style: ctrl.input.style },
            timeout: canceller.promise
        };
        $http.get('api/years', options).then(function (response) {
            ctrl.output.years = response.data;
        });
    }
    var getMakes = function () {
        var options = {
            params: { year: ctrl.input.year, model: ctrl.input.model, trim: ctrl.input.trim, style: ctrl.input.style },
            timeout: canceller.promise
        };
        $http.get('api/makes', options).then(function (response) {
            ctrl.output.makes = response.data;
        });
    }
    var getModels = function () {
        var options = {
            params: { year: ctrl.input.year, make: ctrl.input.make, trim: ctrl.input.trim, style: ctrl.input.style },
            timeout: canceller.promise
        };
        $http.get('api/models', options).then(function (response) {
            ctrl.output.models = response.data;
        });
    }
    var getTrims = function () {
        var options = {
            params: { year: ctrl.input.year, make: ctrl.input.make, model: ctrl.input.model, style: ctrl.input.style },
            timeout: canceller.promise
        };
        $http.get('api/trims', options).then(function (response) {
            ctrl.output.trims = response.data;
            if (ctrl.output.trims[0] === '')
                ctrl.output.trims[0] = 'NONE';
        });
    }
    var getStyles = function () {
        var options = {
            params: { year: ctrl.input.year, make: ctrl.input.make, model: ctrl.input.model, trim: ctrl.input.trim },
            timeout: canceller.promise
        };
        $http.get('api/styles', options).then(function (response) {
            ctrl.output.styles = response.data;
            if (ctrl.output.styles[0] === '')
                ctrl.output.styles[0] = 'NONE';
        });
    }
    var getCars = function () {
        var options = {
            params: { year: ctrl.input.year, make: ctrl.input.make, model: ctrl.input.model, trim: ctrl.input.trim, style: ctrl.input.style },
            timeout: canceller.promise
        };
        $http.get('api/cars', options).then(function (response) {
            ctrl.searching = false;
            ctrl.output.cars = response.data;
            if (ctrl.output.cars.length == 1)
                ctrl.showCar(0);
            else
                $('body').animate({ scrollTop: 160 }, { duration: 800 });
        });
    }
    var initializeCar = function (car) {
        //car.ImageUrls = ['//:0'];
        var options = { params: { year: car.Model_Year, make: car.Make_Display, model: car.Model_Name, trim: car.Model_Trim } };
        car.ImageUrls = ['~/Areas/CarFinder/images/loading.gif'];
        $http.get('api/images', options).then(function (response) {
            car.ImageUrls = response.data;
        });
        options.params.make = car.Make;
        $http.get('api/recalls', options).then(function (response) {
            car.Recalls = response.data;
        });
        return car;
    }

    /*
    $scope.scrollImage = function () {
        var loop;
        return function (bool) {
            var animateNextImage = function () {
                if (ctrl.output.car.imageIndex == 4)
                    ctrl.output.car.imageIndex = 0;
                else
                    ctrl.output.car.imageIndex += 1;
            }
            if (!loop && bool)
                loop = $interval(animateNextImage, 10000);
            else {
                $interval.cancel(loop);
                loop = null;
            }
        };
    }();
    */

    var updateSearch = function () {
        ctrl.output.cars = [];
        canceller.resolve("New search request was initiated.");
        canceller = $q.defer();
        if (!(ctrl.input.year || ctrl.input.make || ctrl.input.model || ctrl.input.trim || ctrl.input.style))
            return false;
        return ctrl.searching = true;
    }
    this.updateYears = function () {
        if (updateSearch())
            getCars();
        getMakes();
        getModels();
        getTrims();
        getStyles();
    }
    this.updateMakes = function () {
        if (updateSearch())
            getCars();
        getYears();
        getModels();
        getTrims();
        getStyles();
    }
    this.updateModels = function () {
        if (updateSearch())
            getCars();
        getYears();
        getMakes();
        getTrims();
        getStyles();
    }
    this.updateTrims = function () {
        if (updateSearch())
            getCars();
        getYears();
        getMakes();
        getModels();
        getStyles();
    }
    this.updateStyles = function () {
        if (updateSearch())
            getCars();
        getYears();
        getMakes();
        getModels();
        getTrims();
    }

    this.showCar = function (index) {
        if (!ctrl.output.cars[index].ImageUrls)
            ctrl.output.car = initializeCar(ctrl.output.cars[index]);
        else
            ctrl.output.car = ctrl.output.cars[index];

        ctrl.showDialog = true;
    }
    this.showRecall = function (index) {
        ctrl.output.car.currentRecall = ctrl.output.car.Recalls[index];
        $("#normal").fadeOut(300, function () {
            $('#recall').fadeIn(300);
        });
    }
    this.hideRecall = function () {
        $("#recall").fadeOut(300, function () {
            ctrl.output.car.currentRecall = null;
            $("#normal").fadeIn(300);
        });
    }

    var canceller = $q.defer();
    getYears();
    getMakes();
    getModels();
    getTrims();
    getStyles();
};