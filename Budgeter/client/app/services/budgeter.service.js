(function () {
    'use strict';

    angular.module('app')
        .factory('budgeter', ['$http', '$sce', budgeter]);


    function budgeter($http, $sce) {

        //var domain = "http://local.visualstudio.com:49236/Budgeter/api/";
        var domain = "/Budgeter/api/";

        var fillTargetHtml = function(data, target)
        {
            target.html = $sce.trustAsHtml(data);
            target.show();
        }

        return {
            getHouseholdsAsync: function (onSuccess) {
                return $http({
                    url: domain + "Households/",
                    method: "POST"
                }).success(function (data, status) {
                    if (typeof onSuccess == 'function' && status == 200)
                        onSuccess(data);
                });
            },

            createHouseholdAsync: function (onSuccess) {
                $http({
                    url: domain + "Households/Create",
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    data: $.param({ Name: "" })
                }).success(function (data) {
                    if (typeof onSuccess == 'function' && data.result === "Success")
                        onSuccess();
                });
            },

            editHouseholdNameAsync: function (id, newName, onSuccess) {
                $http({
                    url: domain + "Households/Edit/" + id,
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    data: $.param({ Name: newName })
                }).success(function (data) {
                    if (typeof onSuccess == 'function' && data.result === "Success")
                        onSuccess();
                });
            },

            inviteMemberView: function (id, target) {
                $http({
                    url: domain + "Households/InviteMember/" + id,
                    method: "GET"
                }).success(function (data) {
                    fillTargetHtml(data, target);
                });
            },
            inviteMemberAsync: function (invite, onSuccess) {
                $http({
                    url: domain + "Households/InviteMember",
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    data: $.param(invite)
                }).success(function (data) {
                    if (typeof onSuccess == 'function' && data.result === "Success")
                        onSuccess();
                });
            },

            addAccountView: function (id, target) {
                $http({
                    url: domain + "Accounts/Create/" + id,
                    method: "GET"
                }).success(function (data) {
                    fillTargetHtml(data, target);
                });
            },
            addAccountAsync: function (account, onSuccess) {
                $http({
                    url: domain + "Accounts/Create",
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    data: $.param(account)
                }).success(function (data) {
                    if (typeof onSuccess == 'function' && data.result === "Success")
                        onSuccess();
                });
            }
        }
    }
})(); 