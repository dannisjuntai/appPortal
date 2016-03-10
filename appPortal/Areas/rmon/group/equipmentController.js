var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', '$rootScope'];

var equipmentController = function ($scope, $location, $routeParams, groupFactory, $rootScope) {
    $scope.equipments = [];
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 0;
    $rootScope.groupId = $scope.groupId;

    var replace = setInterval(function () { getEquipments() }, 3000);

    //導覽資料
    $scope.navDevice = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            redirectToUrl('/rmonDAT/' + o.identity);
        }
    };
    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }

    //取得所有 Equipments
    function getEquipments() {
        groupFactory.getLinkGroups($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.equipments = data;
        }

        function processError(error) {
            return null;
        }
    };

    function init() {
        getEquipments();
    };

    init();

    //切換頁面取消 timer
    $scope.$on("$routeChangeStart", function (next, current) {
        console.log('$routeChangeStart');
        //取消 timer
        if (replace) {
            clearInterval(replace);
        }

    });
};

equipmentController.$inject = injectParams;

app.register.controller('equipmentController', equipmentController);