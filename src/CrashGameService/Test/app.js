const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:7021/gamehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("StartBettingTime", (data) => {
    console.log('StartBettingTime');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});

connection.on("StopBettingTime", (data) => {
    console.log('StopBettingTime');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});

connection.on("StartRound", (data) => {
    console.log('StartRound');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});

connection.on("CrashGame", (data) => {
    console.log('CrashGame');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});

connection.on("ReceiveMultiplier", (data) => {
    console.log('ReceiveMultiplier');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});

connection.on("BET", (data) => {
    console.log('BET');
    const messagesElement = document.getElementById('messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${data}`;
    messagesElement.appendChild(messageElement);
});


// Start the connection
connection.start()
    .catch(err => console.error('Error while starting connection: ', err));
