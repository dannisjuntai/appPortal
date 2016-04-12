var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', '$queueFactory'];

var departmentController = function ($scope, $location, $routeParams, groupFactory, $queueFactory) {
    $scope.showModal = false;
    $scope.departments = [];
    $scope.events = [];
    $scope.groupId = 0;
    //顯示Modal 
    $scope.toggleModal = function (o) {
        $scope.groupId = o.groupId;
        getEvents(o.groupId);
     };

    //導覽資料
    $scope.navEquipment = function (o) {
        if (o && angular.isObject(o)) {
            redirectToUrl($location, '/equipment/' + o.groupId);
        }
    };
    //
    $scope.setEvents = function () {
        groupFactory.setEvents($scope.groupId).then(processSuccess, processError);
    }

    //取得部門資料
    function getDepartments() {
        groupFactory.getDepartments()
        .then(function (data) {
            $scope.departments = data;
        }, function (error) {
        });
    };
    
    //取得事件資料
    function getEvents(groupId) {

        groupFactory.getEvents(groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.events = data;
            $scope.showModal = !$scope.showModal;
        }
        function processError(error) {

        }
    };

    function init() {
        getDepartments();
    }
    init();

    var queue = $queueFactory(0);
    var doDepartments = setInterval(function () { changDepartments() }, 3000);

    function changDepartments() {
        queue.enqueue(function () {
            getDepartments();
        });
        queue.enqueue(function (data) {
            // all tasks finished
            //console.log('工作順利完成！' + dt);
        });
    }
    $scope.$on("$routeChangeStart", function (next, current) {
        //console.log('$routeChangeStart');
        //取消 doDepartments
        if (doDepartments) {
            clearInterval(doDepartments);
        }
    });
};

departmentController.$inject = injectParams;

app.register.controller('departmentController', departmentController);