const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:7021/gamehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

let timerInterval;
let timeLeft = 10;

connection.on("StartBettingTime", (data) => {
    debugger;
    document.getElementById("bet_cashOut").innerHTML = `
        <h3>Bet</h3>
        <button onclick="sendBetRequest()">Bet</button>`;

    var parseJson = JSON.parse(data);
    timeLeft = parseJson.TimeLeft;
    // Start the timer
    startSecondTimer();
});

connection.on("StopBettingTime", (data) => {
    document.getElementById("bet_cashOut").innerHTML = '';
    document.getElementById("timer").innerText = '';
});

connection.on("StartRound", (data) => {
    debugger;

    document.getElementById("bet_cashOut").innerHTML = `
        <h3>Cash Out</h3>
        <button onclick="sendCashOutRequest()">Cash Out</button>`;
});

connection.on("CrashGame", (data) => {
    document.getElementById("bet_cashOut").innerHTML = `
        <h3>Game Crashed</h3> `;
});

connection.on("ReceiveMultiplier", (data) => {
    var parseJson = JSON.parse(data);
    debugger;
    document.getElementById("multiplier").innerText = parseJson.Multiplier;
});

connection.on("BET", (data) => {
        //Add real world logic

});


// Start the connection
connection.start()
    .catch(err => console.error('Error while starting connection: ', err));



function startSecondTimer() {
    // If there's already a timer running, clear it first
    if (timerInterval) {
        clearInterval(timerInterval);
    }
    debugger;
    // Set the timer display to 10 initially
    document.getElementById("timer").innerText = 'Betting time ' + timeLeft;

    timerInterval = setInterval(function () {
        timeLeft--;
        document.getElementById("timer").innerText = 'Betting time ' + timeLeft;

        if (timeLeft <= 0) {
            clearInterval(timerInterval);
            // Add any other logic you want to execute when timer ends
        }
    }, 1000);
}




function sendBetRequest() {
    const value = document.getElementById('value').value;
    const multiplier = document.getElementById('multiplier').value;
    const gameRoundId = document.getElementById('gameRoundId').value;

    fetch('http://localhost:7021/api/Game/bet', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            value,
            multiplier,
            gameRoundId,
        }),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function sendCashOutRequest() {
    const betId = document.getElementById('betId').value;
    const multiplier = document.getElementById('cashOutMultiplier').value;

    fetch('http://localhost:7021/api/Game/cashOut', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            betId,
            multiplier,
        }),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function startGame() {
    document.getElementById('start_game').innerHTML = '';

    fetch('http://localhost:7021/api/Game/start')
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}