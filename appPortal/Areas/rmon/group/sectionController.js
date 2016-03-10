var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory'];

var sectionController = function ($scope, $location, $routeParams, groupFactory) {
    //部門
    $scope.departments = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 1;

    init();
    //取得部門資料
    function getDepartment() {
        groupFactory.getDepartment($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.departments = data;
        
        }
        function processError(error) {

        }
    };

    function init() {
        getDepartment();
    }
};

sectionController.$inject = injectParams;

app.register.controller('sectionController', sectionController);