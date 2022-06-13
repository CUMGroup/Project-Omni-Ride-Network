$(document).ready(
    function () {
        date = new Date();

        if ($('#category')[0].innerText.includes("Sharing")) {
            today = date.toISOString().substring(0, 16);
        } else {
            today = date.toISOString().substring(0, 10);
        }

        $("#pickupdate")[0].value = today;
        $("#returndate")[0].value = today;

        $("#pickupdate")[0].min = today;
        $("#returndate")[0].min = today;
    }
);


function inputchange(basicprice, priceHD, priceIns) {
    let gesamtpreis;
    gesamtpreis = fixNumber(basicprice);

    if ($('#vollkasko')[0].checked) {
        gesamtpreis += fixNumber(priceIns);
    } else if ($('#teilkasko')[0].checked){
        gesamtpreis += 50;
    }

    if ($('#refuel')[0].checked) {
        gesamtpreis += 35;
    }

    let dates = calculateDate($("#pickupdate")[0].value, $("#returndate")[0].value);

    gesamtpreis += dates * fixNumber(priceHD);

    $('#gesamtpreis')[0].innerText = gesamtpreis.toFixed(2);

    calcOptNumber();
}

function calcOptNumber() {
    let v = $('#vollkasko')[0].checked;
    let t = $('#teilkasko')[0].checked;
    let r = $('#refuel')[0].checked;
    let opt = 0

    if (!v && !t && !r)
        opt = 0;
    else if (v && !t && !r)
        opt = 1;
    else if (!v && t && !r)
        opt = 2;
    else if (!v && !t && r)
        opt = 3;
    else if (v && !t && r)
        opt = 4;
    else if (!v && t && r)
        opt = 5;
    else
        opt = -1;

    $('#optAdd').val(opt)
}

function checkinsurance(uncheck) {
    uncheck[0].checked = false;
}

function fixNumber(number) {

    numberFixed = number.replace(",", ".")

    return Number(Number(numberFixed).toFixed(2));
}

function calculateDate(pickupdate, returndate) {
    if (pickupdate == "" || returndate == "") {
        return 1;
    } else {
        pickupdate = new Date(pickupdate);
        returndate = new Date(returndate);

        let time = Math.round((returndate - pickupdate) / 1000 / 60 / 60);

        if ($('#category')[0].innerText.includes("Sharing")) {
            return time;
        } else {
            return (time / 24);
        }
    }
}
