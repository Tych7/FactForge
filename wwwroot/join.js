let playerName = null;
let connection = null;
let countdownInterval = null;

const neonClasses = ["neon-red", "neon-green", "neon-blue", "neon-yellow"];

document.getElementById("joinBtn").addEventListener("click", joinQuiz);

function joinQuiz() {
  const nameInput = document.getElementById("name").value.trim();
  if (!nameInput) {
    alert("Please enter your name");
    return;
  }
  playerName = nameInput;

  toggleScreens("waitingScreen"); // after joining, show waiting screen

  connection = new signalR.HubConnectionBuilder()
    .withUrl("/quizhub")
    .build();

  connection.on("NewQuestion", renderQuestion);

  connection.start()
    .then(() => connection.invoke("JoinGame", playerName))
    .catch(console.error);
}

function toggleScreens(show) {
  document.getElementById("joinScreen").style.display = "none";
  document.getElementById("waitingScreen").style.display = "none";
  document.getElementById("questionScreen").style.display = "none";

  document.getElementById(show).style.display = "block";
}

function renderQuestion(q) {
  toggleScreens("questionScreen"); // switch to question screen

  document.getElementById("questionText").textContent = q.text;
  const choicesDiv = document.getElementById("choices");
  choicesDiv.innerHTML = "";

  startTimer(q.time || 10, () => {
    console.log("Time's up!");
    Array.from(choicesDiv.children).forEach(b => b.disabled = true);
    toggleScreens("waitingScreen"); // go back to waiting after time runs out
  });

  if (q.type === "open") {
    renderOpenQuestion(choicesDiv);
  } else {
    renderMultipleChoice(q.choices, choicesDiv);
  }
}


function startTimer(seconds, onTimeUp) {
  clearInterval(countdownInterval);
  const timerEl = document.getElementById("timer");
  const timerBar = document.getElementById("timerBar");
  let timeLeft = seconds;
  timerEl.textContent = timeLeft;
  timerBar.style.width = "100%";

  countdownInterval = setInterval(() => {
    timeLeft--;
    timerEl.textContent = timeLeft;
    timerBar.style.width = `${(timeLeft / seconds) * 100}%`;

    if (timeLeft <= 0) {
      clearInterval(countdownInterval);
      onTimeUp();
    }
  }, 1000);
}


function renderMultipleChoice(choices, container) {
  choices.forEach((choice, index) => {
    const btn = document.createElement("button");
    btn.textContent = choice;
    btn.classList.add(neonClasses[index % neonClasses.length]);
    btn.style.fontSize = getFontSize(choice.length);

    btn.addEventListener("click", () => {
      Array.from(container.children).forEach(b => b.classList.remove("selected"));
      btn.classList.add("selected");
      connection.invoke("SubmitAnswer", playerName, choice);
      Array.from(container.children).forEach(b => b.disabled = true);
    });

    container.appendChild(btn);
  });
}

function renderOpenQuestion(container) {
  const wrapper = document.createElement("div");
  wrapper.classList.add("open-question");

  const input = document.createElement("input");
  input.type = "text";
  input.placeholder = "Type your answer...";

  const submitBtn = document.createElement("button");
  submitBtn.textContent = "Submit";

  submitBtn.addEventListener("click", () => {
    const answer = input.value.trim();
    if (answer) {
      connection.invoke("SubmitAnswer", playerName, answer);
      input.disabled = true;
      submitBtn.disabled = true;
    }
  });

  wrapper.appendChild(input);
  wrapper.appendChild(submitBtn);
  container.appendChild(wrapper);
}


function getFontSize(length) {
  if (length <= 15) return "2.5rem";
  if (length <= 30) return "2rem";
  return "1.5rem";
}
