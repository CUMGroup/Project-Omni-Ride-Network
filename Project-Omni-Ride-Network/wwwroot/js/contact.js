let audio = new Audio("/images/iliketrains.mp3");
let job;


function checkEmail(mail) {
    let email = "" + mail;
    let criteria = "^[a-zA-Z0-9._%,+-]{2,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    if (!(email.match(criteria))) {
        $('#SenderEmail')[0].classList = "contact mailInFalse";
        $('#Mailincorrect')[0].classList = "registerpage RPTincorrect";
        $('#Mailincorrect')[0].innerText = "Bitte wählen Sie eine gültige E-Mail Adresse! \r\r";
        return false;
    }

    else {
        $('#SenderEmail')[0].classList = "contact mailIn";
        $('#Mailincorrect')[0].classList = "mailincorrect";
        $('#Mailincorrect')[0].innerText = "";
        return true;
    }
}

function check() {
    let input = $('#Subject')[0].value;

    if (input.includes("zug") || input.includes("Zug") ||
        input.includes("züge") || input.includes("Züge") ||
        input.includes("Bahn") || input.includes("bahn")) {

        audio.play();

        $('#zugzug')[0].hidden = "";

        job = window.setInterval("move()", 5);

    }

    else {
        $('#zugzug')[0].hidden = "hidden";
    }
}

function move() {
    console.log("moving");
    let step = 10;
  
    $('#zugzug')[0].style.left = parseInt($('#zugzug')[0].style.left) + step + "px";

    if (parseInt($('#zugzug')[0].style.left) >= 1800) {
        window.clearInterval(job);
        $('#zugzug')[0].hidden = "hidden";
        $('#zugzug')[0].style.left = "-4500px";
    }

}


