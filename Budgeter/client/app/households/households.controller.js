(function () {
    'use strict';

    angular.module('app.households')
        .controller('householdsCtrl', ['$scope', 'budgeter', householdsCtrl])

    function householdsCtrl($scope, budgeter) {
        // info #6BBCD7 107,188,215
        // success #81CA80 129,202,128
        $scope.pie1 = {};
        $scope.households;
        $scope.modal = { id: 'dynamic-modal', show: function () { $('#dynamic-modal').modal('show'); } };
        //[
        //    {
        //        name: "Household Name",
        //        users: [{ name: "Default Username" }, { name: "DefaultUsername 2" }],
        //        accounts: [{ name: "Default Account Name", balance: 0.55 }, { name: "Default Account Name 2", balance: 99.87 }]
        //    },
        //    {
        //        name: "Household Name 2",
        //        users: [{ name: "Default Username 3" }, { name: "DefaultUsername 4" }],
        //        accounts: [{ name: "Default Account Name 3", balance: 17.52 }, { name: "Default Account Name 4", balance: 31987.66 }]
        //    }
        //]

        $scope.goToAccount = function (index) { location.href = "/"; }
        $scope.goToUser = function (index) { location.href = "/"; }
        var getHouseholds = function (callback) {
            budgeter.getHouseholdsAsync(function (data) {
                $scope.households = data;
                if (typeof callback == "function")
                    callback($scope.households);
            });
        };
        $scope.createHousehold = function ()
        {
            budgeter.createHouseholdAsync(function () {
                getHouseholds($scope.editName(data.length - 1));
            })
        }
        $scope.editName = function (index) {
            var household = $scope.households[index];
            household.edit = true;
            setTimeout(function () { $('#householdName-' + index).focus(); }, 1);
        }
        $scope.commitName = function (index) {
            var household = $scope.households[index];
            household.edit = false;
            budgeter.editHouseholdNameAsync(household.householdId, household.name);
        }
        $scope.inviteMember = function (index)
        {
            var household = $scope.households[index];
            budgeter.inviteMemberView(household.userHouseholdId, $scope.modal);
        }
        $scope.addAccount = function (index)
        {
            var household = $scope.households[index];
            budgeter.addAccountView(household.householdId, $scope.modal);
        }

        var init = function () {
            getHouseholds();
        }
        init();

        $scope.pie1.options = {
            animation: true,
            //title : {
            //    text: 'Current Budget',
            //    x: 'center',
            //    textStyle: { fontFamily: '"Roboto","Helvetica Neue",Helvetica,Arial,sans-serif', fontWeight: 'bolder', color: '#2a9fd6'}
            //},
            //tooltip : {
            //    trigger: 'item',
            //    show: false,
            //    formatter: "{a} <br/>{b} : {c} ({d}%)"
            //},
            calculable : true,
            series : [
                {
                    name:'Budget source',
                    type:'pie',
                    radius : '75%',
                    center: ['50%', '50%'],
                    data:[
                        {
                            value:335,
                            name:'Category 1',
                            itemStyle:{
                                normal:{
                                    color: $scope.color.success,
                                    label: {
                                        show: true,
                                        textStyle: {
                                            color: $scope.color.success
                                        }
                                    },
                                    labelLine : {
                                        show: true,
                                        lineStyle: {
                                            color: $scope.color.success
                                        }
                                    }                                    
                                }
                            }
                        }, {
                            value:310,
                            name:'Category 2',
                            itemStyle:{
                                normal:{
                                    color: $scope.color.infoAlt,
                                    label: {
                                        show: true,
                                        textStyle: {
                                            color: $scope.color.infoAlt
                                        }
                                    },
                                    labelLine : {
                                        show: true,
                                        lineStyle: {
                                            color: $scope.color.infoAlt
                                        }
                                    }
                                }
                            }                            
                        },{
                            value:135,
                            name:'Category 3',
                            itemStyle:{
                                normal:{
                                    color: $scope.color.warning,
                                    label: {
                                        show: true,
                                        textStyle: {
                                            color: $scope.color.warning
                                        }
                                    },
                                    labelLine : {
                                        show: true,
                                        lineStyle: {
                                            color: $scope.color.warning
                                        }
                                    }
                                }
                            } 
                        }, {
                            value:1548,
                            name:'Category 4',
                            itemStyle:{
                                normal:{
                                    color: $scope.color.info,
                                    label: {
                                        show: true,
                                        textStyle: {
                                            color: $scope.color.info
                                        }
                                    },
                                    labelLine : {
                                        show: true,
                                        lineStyle: {
                                            color: $scope.color.info
                                        }
                                    }
                                }
                            } 
                        }
                    ]
                }
            ]
        };

        function random(){
            var r = Math.round(Math.random() * 100);
            return (r * (r % 2 == 0 ? 1 : -1));
        }
        function randomDataArray() {
            var d = [];
            var len = 100;
            while (len--) {
                d.push([
                    random(),
                    random(),
                    Math.abs(random()),
                ]);
            }
            return d;
        }   
        $scope.scatter2 = {};
        $scope.scatter2.options = {
            tooltip : {
                trigger: 'axis',
                showDelay : 0,
                axisPointer:{
                    show: true,
                    type : 'cross',
                    lineStyle: {
                        type : 'dashed',
                        width : 1
                    }
                }
            },
            legend: {
                data:['scatter1','scatter2']
            },
            xAxis : [
                {
                    type : 'value',
                    splitNumber: 4,
                    scale: true
                }
            ],
            yAxis : [
                {
                    type : 'value',
                    splitNumber: 4,
                    scale: true
                }
            ],
            series : [
                {
                    name:'scatter1',
                    type:'scatter',
                    symbolSize: function (value){
                        return Math.round(value[2] / 5);
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(107,188,215,.5)'
                        }
                    },
                    data: randomDataArray()
                },
                {
                    name:'scatter2',
                    type:'scatter',
                    symbolSize: function (value){
                        return Math.round(value[2] / 5);
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(129,202,128,.5)'
                        }
                    },                    
                    data: randomDataArray()
                }
            ]            
        }

    }


})(); 