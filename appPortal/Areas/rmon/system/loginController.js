var injectParams = ['$scope', '$window', '$location', 'appStoreFatory', 'linkFactory'];

var loginController = function ($scope, $window, $location, appStoreFatory, linkFactory) {

    // $rootScope.title = "登入資料設定";


    $scope.list = [];
    for (var i = 0; i < 50; i++) {
        $scope.list.push({ id: i, name: 'item ' + i });
    }
    $scope.selectedList = $scope.list[10];

    $scope.user;

    $scope.login = function () {
        appStoreFatory.login($scope.user).then(processSuccess, processError);

        function processSuccess(data) {
            var login = data;

            if (login == true) {
                //$window.location.replace("#/dt0");
                redirectToUrl($location, "/menu");
            }
        }

        function processError(error) {

        }
    };
    init();
    function init() {
        //getOrganization();
        //getDepartments(1037);
        getMainTools(1038);
        //getEquipments(1040);
        //getMainToolLinkTags(1038);
        //getEquipmentLinkTags();
    };
    function getOrganization() {
        linkFactory.getOrganization().then(processSuccess, processError);
        function processSuccess(data) { var d = data; }
        function processError(error) { }
    };
    function getDepartments(id) {
        linkFactory.getDepartments(id).then(processSuccess, processError);
        function processSuccess(data) {
            var d = data;
        }
        function processError(error) { }
    }
    function getMainTools(id) {
        linkFactory.getMainTools(id).then(processSuccess, processError);
        function processSuccess(data) {
            var d = data;
        }
        function processError(error) { }
    }
    function getEquipments(id) {
        linkFactory.getEquipments(id).then(processSuccess, processError);
        function processSuccess(data) {
            var d = data;
        }
        function processError(error) { }
    }
    //
    function getMainToolLinkTags(id) {
        linkFactory.getMainToolLinkTags(id).then(processSuccess, processError);
        function processSuccess(data) {
            var d = data;
        }
        function processError(error) { }
    }
    //
    function getEquipmentLinkTags(id) {
        linkFactory.getEquipmentLinkTags(id).then(processSuccess, processError);
        function processSuccess(data) {
            var d = data;
        }
        function processError(error) { }
    }

};

loginController.$inject = injectParams;


app.register.controller('loginController', loginController);
