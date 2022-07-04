function checkPW_RPT(pw, pwRPT) {
    if (!(pw == pwRPT)) {
        $('#PasswordRPT')[0].classList = "loginpage user RPTincorrect";
        $('#RPTincorrect')[0].innerText = "Passwörter stimmen nicht überein."
        return false;
    }
    else {
        $('#PasswordRPT')[0].classList = "loginpage user";
        $('#RPTincorrect')[0].innerText = ""
        return true;
    }
}

function checkPW_norm(pw) {
    let password = "" + pw;
    let criteria = /^(?=.*?[A-Z])(?=(.*[a-z]))(?=(.*[\d]))(?=(.*[^a-zA-Z0-9])).{6,}$/g;
    if (!(password.match(criteria))) {
        $('#Password')[0].classList = "loginpage user RPTincorrect";
        $('#pwCriteria')[0].classList = "registerpage RPTincorrect";
        return false;
    }
    else {
        $('#Password')[0].classList = "loginpage user";
        $('#pwCriteria')[0].classList = "registerpage passwordRQMTS";
        return true;
    }
}

function check_age() {
    let bday = $('#KdBirth')[0].valueAsDate;
    let today = new Date();

    let age = (today - bday) / 1000 / 60 / 60 / 24 / 365;

    if (age < 18) {
        $('#KdBirth')[0].classList = "loginpage user RPTincorrect";
        $('#underage')[0].innerText = "Du musst mindestens 18 sein um bei uns ein Account anzulegen."
        return false;
    }
    else {
        $('#KdBirth')[0].classList = "loginpage user";
        $('#underage')[0].innerText = ""
        return true;
    }
}

function check_all() {
    if (
        checkPW_norm($('#Password')[0].value) &
        checkPW_RPT($('#Password')[0].value, $('#PasswordRPT')[0].value) &
        check_age()
    ) {
        return true;
    }
    return false;
}