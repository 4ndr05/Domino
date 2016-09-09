var app = angular.module('DominoApp', []);
app.controller('AvatarController', function ($scope, $http)
{
    //carga inicial de avatars
	var avatars = new Array();
	avatars[0] = {Name:"Andrés Manuel", Photo:"amlo"};
	avatars[1] = {Name:"Celia Cruz", Photo:"celia"};
	avatars[2] = {Name:"Hugo Chavez", Photo:"chavez"};
	avatars[3] = {Name:"Che Guevara", Photo:"che"};
	avatars[4] = {Name:"Compay", Photo:"compay"};
	avatars[5] = {Name:"Fidel Castro", Photo:"fidel"};
	avatars[6] = {Name:"Kim", Photo:"kim"};
	avatars[7] = {Name:"Maduro", Photo:"maduro"};
	avatars[8] = {Name:"Obama", Photo:"obama"};
	avatars[9] = {Name:"Enrique Peña", Photo:"pena"};
	avatars[10] = {Name:"Putin", Photo:"putin"};
	avatars[11] = {Name:"Trump", Photo:"trump"};
	$scope.modelAvatars = avatars;

	$scope.Play = function()
	{
		//Genera el websocket
		var socket = io.connect('http://localhost:8081');
		socket.emit('newPlayer', 'Player|Icon1');


		//Nuevo Jugador
		socket.on('NuevoJugador', function (data) {
			console.log("Nuevo Jugador: " + data.Name);
		});
	}
});
