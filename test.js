const http = require("http");

let data = JSON.stringify({
  state: [1, 2, 3],
  change: []
});

let options = {
  method: "POST",
  hostname: "localhost",
  port: 5000,
  path: "/configure",
  headers: {
    "Content-Type": "application/json",
    "Content-Length": data.length,
  }
};

let request = http.request(options, (res) => {
  res.on("data", (data) => {
    console.log(data.toString());
  });

  res.on("end", () => {
    console.log("done");
  });

  console.log(res.statusCode);
  console.log(res.statusMessage);
});

request.write(data);
request.end()
