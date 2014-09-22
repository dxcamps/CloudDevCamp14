var http = require('http');

http.createServer(function (req, res) {
    
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<h1>Hello World (in Node.js!)</h1>');
    
}).listen(process.env.PORT);