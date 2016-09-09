// Setup basic express server
var util = require('util');
var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var request = require('request');


var serverPort = 8081;
var APIServer = 'http://la-match-02:8080/domino/v1'

// Routing
app.use(express.static(__dirname + '/Resources'));

app.get('/', function (req, res) {
    res.sendFile(__dirname + "/" + "index.html" );
})

app.get('/index.html', function (req, res) {
    res.sendFile(__dirname + "/" + "index.html" );
})

/*
app.post('/process_post', urlencodedParser, function (req, res)
{

   // Prepare output in JSON format
   response = {
       first_name:req.body.first_name,
       last_name:req.body.last_name
   };
   console.log(response);
   res.end(JSON.stringify(response));
})*/

server.listen(serverPort, function () {
    console.log("Domino listening at http://%s:%s", server.address().address, server.address().port)
})

function addParticipant(data, nick, icon) {

    var currentGameId;
    var joined = false;

    //Obtener la lista de juegos creados
    request.get(util.format('%s/matches/',APIServer), function (error, response, body) {

        if (!error && response.statusCode == 200) {
            var obj = JSON.parse(body);

            //Tratamos de unir al usuario en alguno de los juegos
            currentGameId = obj.matches.items[0].matchid
            console.log(currentGameId);
            request.post({url:util.format('%s/matches/%s/players/', APIServer, currentGameId)}, function(error, response, body) {
                //En caso de que se una el jugador, lo metemos al canal del juego
                if(!error && response.statusCode == 201) {
                    console.log(util.format('%s %s %s', currentGameId, 'castro', 'castroimg'));
                    data.join(currentGameId);
                    io.to(currentGameId).emit('NuevoJugador', { Name: "castro", photo: "castro" });
                }
            });
        }
        else {
            return false;
        }
    });

    //socket.emit('news', { hello: 'world' });
	//socket.broadcast.emit('new_user');
}

io.on('connection', function (socket) {

    console.log("Usuario conectado - " + socket.id);

    socket.on('newPlayer', fu(nction(msg) {
        console.log(msg.split('|')));
        addParticipant(socket);
    });

    socket.on('disconnect', function () {
	  //io.emit('player_diconected', { player: 'xxxxx' } );
	});
});
