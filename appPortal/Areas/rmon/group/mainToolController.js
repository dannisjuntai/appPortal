var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory'];

var mainToolController = function ($scope, $location, $routeParams, groupFactory) {
    var indexedEqument = [];
    $scope.linkGroups = [];

    $scope.equmentToFilter = function () {
        indexedEqument = [];
        return $scope.linkGroups;
    }

    $scope.filterMainTool = function (equment) {
        var isNew = indexedEqument.indexOf(equment.parent) == -1;

        if (isNew) {
            indexedEqument.push(equment.parent);
        }
        return isNew;
    }

    //導覽資料
    $scope.navDataMap = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            redirectToUrl('/rmonDAT/' + o.identity);
        }
    };

    //取得所有main tool
    function getMainTool(id) {

        groupFactory.getLinkGroups(id).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkGroups = data;
        }

        function processError(error) {
            return null;
        }
    };
    //controller 初始化
    function init() {
        getMainTool(1);
    }

    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
    //取得監控資料
    function getLinkGroups() {
        groupFactory.getLinkGroups(1).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkGroups = data;
        }

        function processError(error) {
            return null;
        }
    };
    var dataPoint = setInterval(function () { getLinkGroups() }, 3000);

    //切換頁面取消 timer
    $scope.$on("$routeChangeStart", function (next, current) {
        console.log('$routeChangeStart');
        //取消 timer
        if (dataPoint) {
            clearInterval(dataPoint);
        }

    });
    init();
};

mainToolController.$inject = injectParams;

app.register.controller('mainToolController', mainToolController);