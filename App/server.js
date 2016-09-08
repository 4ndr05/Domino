// Setup basic express server
var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var port = 8081;
/*
var bodyParser = require('body-parser');
var urlencodedParser = bodyParser.urlencoded({ extended: false })
*/

// Routing
app.use(express.static(__dirname + '/Resources'));

app.get('/', function (req, res) {
   res.sendFile(__dirname + "/" + "index.html" );
})

app.get('/index.html', function (req, res) {
   res.sendFile(__dirname + "/" + "index.html" );
})
/*
app.get('/process_get', function (req, res) {

   // Prepare output in JSON format
   response = {
       first_name:req.query.first_name,
       last_name:req.query.last_name
   };
   console.log(response);
   res.end(JSON.stringify(response));
})

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


io.on('connection', function (socket) {
	console.log("Usuario conectado - " + socket.id);
	socket.emit('news', { hello: 'world' });
	//socket.broadcast.emit('new_user');
  /*socket.on('my other event', function (data) {
    console.log(data);
  });*/
  
    socket.on('disconnect', function () {
		//io.emit('player_diconected', { player: 'xxxxx' } );
	});
  
});


//Match
  /*
var numUsers = 0;

io.on('connection', function (socket) {
  var addedUser = false;


  // when the client emits 'new message', this listens and executes
  socket.on('new message', function (data) {
    // we tell the client to execute 'new message'
    socket.broadcast.emit('new message', {
      username: socket.username,
      message: data
    });
  });

  // when the client emits 'add user', this listens and executes
  socket.on('add user', function (username) {
    if (addedUser) return;

    // we store the username in the socket session for this client
    socket.username = username;
    ++numUsers;
    addedUser = true;
    socket.emit('login', {
      numUsers: numUsers
    });
    // echo globally (all clients) that a person has connected
    socket.broadcast.emit('user joined', {
      username: socket.username,
      numUsers: numUsers
    });
  });

  // when the client emits 'typing', we broadcast it to others
  socket.on('typing', function () {
    socket.broadcast.emit('typing', {
      username: socket.username
    });
  });

  // when the client emits 'stop typing', we broadcast it to others
  socket.on('stop typing', function () {
    socket.broadcast.emit('stop typing', {
      username: socket.username
    });
  });

  // when the user disconnects.. perform this
  socket.on('disconnect', function () {
    if (addedUser) {
      --numUsers;

      // echo globally that this client has left
      socket.broadcast.emit('user left', {
        username: socket.username,
        numUsers: numUsers
      });
    }
  });

  
});  */
