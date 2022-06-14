function checkPW_RPT(pw, pwRPT) {
    console.log(pw);

    if (!(pw == pwRPT)) {
        $('#PasswordRPT')[0].classList = "loginpage user RPTincorrect";
        $('#RPTincorrect')[0].innerText = "Passwörter stimmen nicht überein."
    }
    else {
        $('#PasswordRPT')[0].classList = "loginpage user";
        $('#RPTincorrect')[0].innerText = ""
    }
}

function checkPW_norm(pw) {
    let password = "" + pw;
    let criteria = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{7,15}$/;

    if (password.length < 6 || !(password.match(criteria))) {
        $('#Password')[0].classList = "loginpage user RPTincorrect";
        $('#pwCriteria')[0].classList = "registerpage RPTincorrect";
    }
    else {
        $('#Password')[0].classList = "loginpage user";
        $('#pwCriteria')[0].classList = "registerpage passwordRQMTS";
    }
}

function check_age() {
    let bday = $('#KdBirth')[0].valueAsDate;
    let today = new Date();

    let age = (today - bday) / 1000 / 60 / 60 / 24 / 365;

    if (age < 18) {
        $('#KdBirth')[0].classList = "loginpage user RPTincorrect";
        $('#underage')[0].innerText = "Du musst mindestens 18 sein um bei uns ein Account anzulegen."
    }
    else {
        $('#KdBirth')[0].classList = "loginpage user";
        $('#underage')[0].innerText = ""
    }
}