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
    if (pw.includes()) {

    }
}