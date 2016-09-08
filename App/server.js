// Setup basic express server
var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var port = 8081;

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

server.listen(port, function () {
  console.log("Example app listening at http://%s:%s", server.address().address, server.address().port)
})

function addParticipant(data) {

  //Agregar participante a juego o crear juego, se obtiene id del juego
  var matchId = "xxxx";
  //Agregar participante al room del juego
  data.join(matchId);
  //Avisar a los demas jugadores
  io.to(matchId).emit('NuevoJugador', { Name: "castro", photo: "castro" });
  //socket.emit('news', { hello: 'world' });
	//socket.broadcast.emit('new_user');
}

io.on('connection', function (socket) {
	console.log("Usuario conectado - " + socket.id);
  addParticipant(socket);
	socket.on('disconnect', function () {
	  //io.emit('player_diconected', { player: 'xxxxx' } );
	});
});
