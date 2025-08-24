
const http = require('http');

const fs = require('fs');

const path = require('path');

const hostname = '127.0.0.1';
const port = 3000;

const filePath = path.join(__dirname, 'index.html');

const server = http.createServer((req, res) => {

  fs.readFile(filePath, (err, data) => {
   
    if (err) {
      res.statusCode = 404;
      res.setHeader('Content-Type', 'text/plain');
      res.end('Error: index.html not found!');
      return;
    }

  
    res.statusCode = 200;
    res.setHeader('Content-Type', 'text/html');

    res.end(data);
  });
});

server.listen(port, hostname, () => {
  
  console.log(`Server running at http://${hostname}:${port}/`);
});