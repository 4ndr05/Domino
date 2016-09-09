var app = angular.module('DominoApp', []);
app.controller('AvatarController', function ($scope, $http)
{
    //carga inicial de avatars
	var avatars = new Array();
	avatars[0] = {Name:"Obama", Photo:"obama"};
	avatars[1] = {Name:"Celia Cruz", Photo:"celia"};
	avatars[2] = {Name:"Hugo Chavez", Photo:"chavez"};
	avatars[3] = {Name:"Che Guevara", Photo:"che"};
	avatars[4] = {Name:"Compay", Photo:"compay"};
	avatars[5] = {Name:"Fidel Castro", Photo:"fidel"};
	avatars[6] = {Name:"Kim", Photo:"kim"};
	avatars[7] = {Name:"Maduro", Photo:"maduro"};
	avatars[8] = {Name:"Andrés Manuel", Photo:"amlo"};
	avatars[9] = {Name:"Enrique Peña", Photo:"pena"};
	avatars[10] = {Name:"Putin", Photo:"putin"};
	avatars[11] = {Name:"Trump", Photo:"trump"};
	$scope.modelAvatars = avatars;

    $scope.step = 1;
    $scope.AvatarSelected = "";

	$scope.nombre = "";

	$scope.Play = function()
	{
		//Genera el websocket
		var socket = io.connect('http://172.16.2.16:8081');
		socket.emit('newPlayer',  $scope.nombre + '|' + $scope.AvatarSelected);

		//Nuevo Jugador
		socket.on('NuevoJugador', function (data) {
			console.log("Nuevo Jugador: " + data.Name);
		});

		socket.on('url', function (data) {
			window.location.href = data;
		});
	}

    $scope.Cancel = function()
	{
        $scope.step = 1;
    }

    $scope.Init = function()
	{
        $scope.step = 2;
    }

    $scope.Select = function()
	{
        $scope.step = 3;
    }

    $scope.SelectAvatar  = function(avatarName)
	{
        $scope.AvatarSelected = avatarName;
    }
});

app.controller('GameController', function ($scope, $http)
{

	var socket = io.connect('http://172.16.2.16:8081');
	socket.emit('join',  'MJSEFJBSFJABSFJKFBSKDV');

	$scope.Move = function(id)
	{
		socket.emit('tilePlayed',  'MJSEFJBSFJABSFJKFBSKDV|' + id);

		for(tile in $scope.myTiles.tiles.items)
		{
			if($scope.myTiles.tiles.items[tile].tileid == id)
			{
				//alert($scope.myTiles.tiles.items[tile].tileid + "    -   " + id);
				$scope.myTiles.tiles.items.splice(tile,1);
				break;
			}
		}

		$scope.modelGame.state.tiles.items.push({
			"tileid": id,
			"side_a":"6",
			"side_b":"6",
			"playerid":"PSDSFFDFNWJSDNFLDFK"
		 });
		 $scope.$evalAsync();

 		if ($scope.myTiles.tiles.items.length == 0)
		{
			socket.emit('gameOver',  'MJSEFJBSFJABSFJKFBSKDV');
			
		}
	}

	socket.on('playedtile', function (data) {
		//alert("Jugada: " + data);
		$scope.modelGame.state.tiles.items.push({
			"tileid": data,
			"side_a":"6",
			"side_b":"6",
			"playerid":"PSDSFFDFNWJSDNFLDFK"
		 });
		 $scope.$evalAsync();

		 //alert("Jugada: " + data);
	});

	socket.on('youLose', function () {

		 alert("TREMENDO PERDEDOR");
	});

	var tiles =
	{
   "tiles":{
      "points":"31",
      "items":[
         {
            "tileid":"6-6",
            "side_a":"6",
            "side_b":"6"
         },
         {
            "tileid":"4-6",
            "side_a":"6",
            "side_b":"4"
         },
         {
            "tileid":"4-5",
            "side_a":"4",
            "side_b":"5"
         }
      ]
   }
}

	var game =
	{
   "matchid":"MJSEFJBSFJABSFJKFBSKDV",
   "creation_date":"2015-11-30",
   "title":"Este Dominó no lo tiene ni Obama!",
   "state":{
      "max_tiles":28,
      "status": "playing",
      "next_player": "PLIJSMFMGRNFOGNDKF",
      "game_stream":"6-6,6-4,4-5",
      "side_a":"6",
      "side_b":"5",
      "tiles":{
         "items":[
            {
               "tileid":"6-6",
               "side_a":"6",
               "side_b":"6",
               "playerid":"PSDSFFDFNWJSDNFLDFK"
            },
            {
               "tileid":"6-4",
               "side_a":"6",
               "side_b":"4",
               "playerid":"PSDSFFDFNWJSDNFLDFK"
            },
            {
               "tileid":"4-5",
               "side_a":"4",
               "side_b":"5",
               "playerid":"PKDONFGFGMPFKGDDGD"
            }
         ]
      }
   },
   "players":{
      "items":[
         {
            "playerid":"PSDSFFDFNWJSDNFLDFK",
            "creation_date":"2015-11-30",
            "alias":"player_1",
            "avatar": "obama"
         },
         {
            "playerid":"PJDLSOWERNFLEKFND",
            "creation_date":"2015-11-30",
            "alias":"player_2",
            "avatar": "celia"
         },
         {
            "playerid":"PKDONFGFGMPFKGDDGD",
            "creation_date":"2015-11-30",
            "alias":"player_3",
            "avatar": "chavez"
         },
         {
            "playerid":"PLIJSMFMGRNFOGNDKF",
            "creation_date":"2015-11-30",
            "alias":"player_4",
            "avatar": "che"
         }
      ]
   }
}

	$scope.modelGame = game;
	$scope.myTiles = tiles;
});
