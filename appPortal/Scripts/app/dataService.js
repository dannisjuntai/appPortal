//'use strict';
var injectParams = ['$http', '$q', '$resource'];

/*
appStore factory
*/
var appStoreFatory = function ($http, $q, $resource) {

    var serviceBase = '/api/authenticate/',
        factory = {};

    factory.getCustomer = function (id) {
        return getSideBarMenu();
    }
    factory.login = function (user) {
        return $http.post(serviceBase + 'Login', user).then(function (results) {
            return results.data;
        });
    }

    return factory;

};

appStoreFatory.$inject = injectParams;

app.factory('appStoreFatory', appStoreFatory);

/*
  Group Factory
*/
var groupFactory = function ($http, $q, $resource) {
    var serviceBase = '/api/group/',
    factory = {};

    var url = "";


    //取得 群組階層
    factory.getGroups = function () {
        return $http.get(serviceBase + 'GetGroups').then(function (results) {
            return results.data;
        });
    };
    //取得群組類型
    factory.getGroupTypes = function (groupTypeValue) {
        url = serviceBase + 'GetGroupTypes/' + 0 + '?groupTypeValue=' + groupTypeValue;
        //serviceBase + 'GetGroupData/' + id + '?property=' + property
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得圖檔
    factory.getGroupImages = function (id) {
        url = serviceBase + 'GetGroupImages/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //取得圖控資料
    factory.getGroupLocations = function (id) {
        url = serviceBase + 'GetGroupLocations/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //設定圖控資訊
    factory.setGroupLocations = function (vm) {
        return $http.post(serviceBase + 'SetGroupLocations', vm).then(function (results) {
            return results.data;
        });
    }
    //刪除圖控資訊
    factory.deleteGroupLocation = function (vm) {
        return $http.post(serviceBase + 'DeleteGroupLocation', vm).then(function (results) {
            return results.data;
        });
    }
    //新增
    factory.insertGroup = function (vm) {
        return $http.post(serviceBase + 'InsertGroup', vm).then(function (results) {
            return results.data;
        });
    }

    //取得群組資料
    factory.getGroup = function (id) {
        url = serviceBase + 'GetGroup/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };

    //更新群組
    factory.updateGroup = function (vm) {
        return $http.put(serviceBase + 'PutGroup/' + vm.currentGroup.groupId, vm).then(function (results) {
            return results.data;
        });
    };

    //刪除群組
    factory.deleteGroup = function (id) {
        //deleteGroup
        return $http.delete(serviceBase + 'DeleteGroup/' + id).then(function (results) {
            return results.data;
        });
    }

    factory.saveBackground = function (image) {
        return $http.post(serviceBase + 'SaveBackground', image).then(function (results) {
            return results.data;
        });
    }
    //LinkDevies
    factory.getLinkDevices = function () {
        return $http.get(serviceBase + 'GetLinkDevices').then(function (results) {
            return results.data;
        });
    };
    factory.getLinkDevices = function (link) {
        return $http.get(serviceBase + 'GetLinkDevices').then(function (results) {
            return results.data;
        });
    };
    //GetLinkTags
    factory.getLinkTags = function (id) {
        return $http.get(serviceBase + 'GetLinkTags/' + id).then(function (results) {
            return results.data;
        });
    };
    //取得linkTag 資料
    factory.getLinkTag = function (id) {
        return $http.get(serviceBase + 'GetLinkTag/' + id).then(function (results) {
            return results.data;
        });
    };
    //設定LinkTag 資料
    factory.setLinkTag = function (vm) {
        url = serviceBase + 'SetLinkTag';
        return $http.put(url, vm).then(function (results) {
            return results.data;
        });
    }

    //
    factory.getTagAlarm = function (id) {
        return $http.get(serviceBase + 'GetTagAlarm/' + id).then(function (results) {
            return results.data;
        });
    };
    //圖控訊息
    factory.getLinkGroups = function (id) {
        return $http.get(serviceBase + 'GetLinkGroups/' + id).then(function (results) {
            return results.data;
        });
    };
    //取得歷史資料
    factory.getTagHistory = function (param) {
        url = serviceBase + 'getTagHistory';
        return $http.post(url, param).then(function (results) {
            return results.data;
        });
    };
    //取得部門資料
    //factory.getDepartments = function () {
    //    url = serviceBase + 'getDepartments';
    //    return $http.get(url).then(function (results) {
    //        return results.data;
    //    });
    //};
    //取得部門資料
    factory.getDepartment = function (groupId) {
        url = serviceBase + 'getDepartment/' + groupId;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得Main Tool 資料
    factory.getMainTools = function (groupId) {
        url = serviceBase + 'getMainTools/' + groupId;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    // 取得 equipment 資料
    factory.getEquipments = function (groupId) {
        url = serviceBase + 'getEquipments/' + groupId;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //取得事件
    factory.getEvents = function (id) {
        url = serviceBase + 'getEvents/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得事件
    factory.getEvents = function (history) {
        url = serviceBase + 'getEvents';
        return $http.post(url, history).then(function (results) {
            return results.data;
        });
    };
    //設定事件
    factory.setEvents = function (id) {
        url = serviceBase + 'setEvents/' + id;
        return $http.delete(url).then(function (results) {
            return results.data;
        });
    };
    //取得歷史紀錄
    factory.getHistoryTags = function (vm) {
        return $http.post(serviceBase + 'GetHistoryTags', vm).then(function (results) {
            return results.data;
        });
    };
    //設定維護狀態
    factory.setMaintain = function (param) {
        url = serviceBase + 'setMaintain/';
        return $http.post(url, param).then(function (results) {
            return results.data;
        });
    }
    //取得保養項目
    factory.getMaintainItems = function () {
        url = serviceBase + 'getMaintainItems';
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //取得事件項目
    factory.getEventLevels = function () {
        url = serviceBase + 'getEventLevels';
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //取得參數設定List
    factory.getOptionFieldName = function () {
        url = serviceBase + 'getOptionFieldName';
        return $http.get(url).then(function (results) {
            return results.data;
        });
    }
    //
    factory.getOptionSets = function (param) {
        url = serviceBase + 'getOptionSets';
        return $http.post(url, param).then(function (results) {
            return results.data;
        });
    }
    //
    factory.setOptionSets = function (option) {
        url = serviceBase + 'setOptionSets';
        return $http.post(url, option).then(function (results) {
            return results.data;
        });
    }
    return factory;
};

groupFactory.$inject = injectParams;

app.factory('groupFactory', groupFactory);

var linkFactory = function ($http, $q, $resource) {
    var serviceBase = '/api/link/', factory = {};

    var url = "";
    //取得組織
    factory.getOrganization = function () {
        url = serviceBase + 'getOrganization';
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得部門
    factory.getDepartments = function (id) {
        url = serviceBase + 'getDepartments/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得主機台
    factory.getMainTools = function (id) {
        url = serviceBase + 'getMainTools/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得設備
    factory.getEquipments = function (id) {
        url = serviceBase + 'getEquipments/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //getMainToolLinkTags
    factory.getMainToolLinkTags = function (id) {
        url = serviceBase + 'getMainToolLinkTags/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //getEquipmentLinkTags
    factory.getEquipmentLinkTags = function (id) {
        url = serviceBase + 'getEquipmentLinkTags/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //getDeviceLinkTags
    factory.getDeviceLinkTags = function (id) {
        url = serviceBase + 'getDeviceLinkTags/' + id;
        return $http.get(url).then(function (results) {
            return results.data;
        });
    };
    //取得趨勢圖
    factory.getHistoryTags = function (vm) {
        url = serviceBase + 'getHistoryTags';
        return $http.post(url, vm).then(function (results) {
            return results.data;
        });
    };
    //取得趨勢圖 單點
    //factory.getHistoryTag = function (param) {
    //    url = serviceBase + 'getHistoryTag';
    //    return $http.post(url, param).then(function (results) {
    //        return results.data;
    //    });
    //};
    return factory;
};

linkFactory.$inject = injectParams;

app.factory('linkFactory', linkFactory);
var breadcrumbService = function () {
    var self = this;
    var breadcrumbs = [];

    var ensureIdIsRegistered = function (id) {
        if (angular.isUndefined(breadcrumbs[id])) {
            breadcrumbs[id] = [];
        }
    };

    //新增 Breadcrumb
    self.setBreadcrumb = function (id, item) {
        ensureIdIsRegistered(id);
        breadcrumbs.forEach(function (obj) {
            if (obj.label == item.label) {
                var index = breadcrumbs.indexOf(obj);
      
                if (breadcrumbs.length > 1 + index) {
                    breadcrumbs.splice(index, breadcrumbs.length - index);
                }
            }
        });
        breadcrumbs.push(item);
    };
    //取得 Breadcrumb
    self.getBreadcrumbs = function () {
        return breadcrumbs;
    };
    //移除 Breadcrumb
    self.setBreadcrumbs = function (obj) {
        var index = breadcrumbs.indexOf(obj);
        var herf = breadcrumbs[index].href;
        if (breadcrumbs.length > 1 + index) {
            breadcrumbs.splice(index, breadcrumbs.length - index);
        }

        return herf;
    };
    //清除 Breadcrumb
    self.clearBreadcrumbs = function (id) {
        breadcrumbs.splice(0, breadcrumbs.length);
        //ary.splice(0, ary.length);
    }
};

app.service('breadcrumbService', breadcrumbService);

function getSideBarMenu() {
    return [
        {
            "parentApp":
              { "appNo": 100000, "appName": "監控設定作業", "parentAppNo": 0, "typeName": "View", "symbol": null, "area": "rmon/set/", "templateUrl": "#/rmonSETMap", "controllerName": "rmonSETMap", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" },
            "items": []
        },
        {
            "parentApp": { "appNo": 110000, "appName": "即時資料監控", "parentAppNo": 0, "typeName": "View", "symbol": null, "area": "rmon/data/", "templateUrl": "#/rmonDATMap", "controllerName": "rmonDATMap", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }, "items": []
        }
        /*,
        {
            "parentApp": { "appNo": 120000, "appName": "監控群組設定", "parentAppNo": 0, "typeName": "Root", "symbol": null, "area": "rmon/group/", "templateUrl": "#/rmonGROUP", "controllerName": "rmonGROUP", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }, "items": [{ "appNo": 120001, "appName": "部門群組設定", "parentAppNo": 120000, "typeName": "View", "symbol": null, "area": "rmon/group/", "templateUrl": "#/groupDEPARTMENT", "controllerName": "groupDEPARTMENT", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }, { "appNo": 120002, "appName": "設備群組設定", "parentAppNo": 120000, "typeName": "View", "symbol": null, "area": "rmon/group/", "templateUrl": "#/groupDEVICE", "controllerName": "groupDEVICE", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }, { "appNo": 120003, "appName": "自設群組設定", "parentAppNo": 120000, "typeName": "View", "symbol": null, "area": "rmon/group/", "templateUrl": "#/groupDEFINE", "controllerName": "groupDEFINE", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }]
        },
        {
            "parentApp":
                {
                    "appNo": 130000, "appName": "系統設定作業", "parentAppNo": 0, "typeName": "Root", "symbol": null, "area": "rmon/system/", "templateUrl": "#", "controllerName": "sysSET", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00"
                },
            "items": [
                { "appNo": 130001, "appName": "登入資料設定", "parentAppNo": 130000, "typeName": "View", "symbol": null, "area": "rmon/system/", "templateUrl": "#/sysLOGIN", "controllerName": "sysLOGIN", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" },
                { "appNo": 130002, "appName": "權限資料設定", "parentAppNo": 130000, "typeName": "View", "symbol": null, "area": "rmon/system/", "templateUrl": "#/sysAUTHORITY\r\n", "controllerName": "sysAUTHORITY\r\n", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" },
                { "appNo": 130003, "appName": "參數資料設定", "parentAppNo": 130000, "typeName": "View", "symbol": null, "area": "rmon/system/", "templateUrl": "#/sysPARAM", "controllerName": "sysPARAM", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" },
                { "appNo": 130004, "appName": "部門資料設定", "parentAppNo": 130000, "typeName": "View", "symbol": null, "area": "rmon/system/", "templateUrl": "#/sysDEPARTMENT", "controllerName": "sysDEPARTMENT", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" },
                { "appNo": 130005, "appName": "設備資料設定", "parentAppNo": 130000, "typeName": "View", "symbol": null, "area": "rmon/system/", "templateUrl": "#/sysDEVICE", "controllerName": "sysDEVICE", "verNo": "1.0.0.0", "maintainUser": 9999, "systemUser": 9999, "systemTime": "2017-11-02T00:00:00" }]
        }*/]
};


