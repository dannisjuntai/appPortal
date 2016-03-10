var injectParams = ['$scope', '$location', 'groupFactory'];

var rmonDATMapController = function ($scope, $location, groupFactory) {
    $scope.linkGroups = [];
    $scope.isET = "";
    
    //取得群組
    function getGroups() {
        groupFactory.getGroups()
        .then(function (data) {
            $scope.treedata = data;
        }, function (error) {
            //$window.alert('Sorry, an error occurred: ' + error.data.message);
        });
    };


    function getLinkGroups() {
        //
        if (typeof ($scope.group.currentNode) == 'undefined') {
            return null;
        }

        var id = $scope.group.currentNode.id;
       

        groupFactory.getLinkGroups(id).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkGroups = data;
        }

        function processError(error) {
            return null;
        }
    };


    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }

    //導覽資料
    $scope.navDataMap = function () {

        if ($scope.group && angular.isObject($scope.group.currentNode)) {
            redirectToUrl('/rmonDAT/' + $scope.group.currentNode.id);
        }
    };
    //controller 初始化
    function init() {
        getGroups();
    }


    init();
    //選擇階層
    $scope.$watch('group.currentNode', function (newObj, oldObj) {
        if ($scope.group && angular.isObject($scope.group.currentNode)) {

            //判斷是否切換畫面
            getLinkGroups();

            if ($scope.group.currentNode.data == '2') {
                $scope.isET = 'ET';
                //導到
                if ($scope.group && angular.isObject($scope.group.currentNode)) {
                    redirectToUrl('/rmonDAT/' + $scope.group.currentNode.id);
                }
            }
            else {
                $scope.isET = '';
            }
        }
    }, false);

    var dataPoint = setInterval(function () { getLinkGroups() }, 3000);

    //切換頁面取消 timer
    $scope.$on("$routeChangeStart", function (next, current) {
        console.log('$routeChangeStart');
        //取消 timer
        if (dataPoint) {
            clearInterval(dataPoint);
        }

    });
};

rmonDATMapController.$inject = injectParams;

app.register.controller('rmonDATMapController', rmonDATMapController);