var app = angular.module('DominoApp', []);
app.controller('AvatarController', function ($scope, $http) 
{
    //carga inicial de avatars
	var avatars = new Array();
	avatars[0] = {Name:"Celia Cruz", Photo:"celia"};
	avatars[1] = {Name:"Obama", Photo:"obama"};
	avatars[2] = {Name:"Hugo Chavez", Photo:"hugo"};
	$scope.modelAvatars = avatars;	
});