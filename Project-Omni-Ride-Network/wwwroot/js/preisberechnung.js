$(document).ready(
    function () {
        date = new Date();
        today = date.toLocaleString('sv', { timeZoneName: 'short' }).substring(0, 10);

        if ($('#category')[0].innerText.includes("Sharing")) {
            today += "T";

            if (date.getHours() < 10) {
                today += "0";
                today += date.toLocaleString('sv', { timeZoneName: 'short' }).substring(11, 15);
            }
            else {
                today += date.toLocaleString('sv', { timeZoneName: 'short' }).substring(11, 16);
            }

        }

        $("#pickupdate")[0].value = today;
        $("#returndate")[0].value = today;

        $("#pickupdate")[0].min = today;
        $("#returndate")[0].min = today;

        $("#pickupdate").change(function () {
            let ret = $("#returndate")[0];
            ret.min = this.value;
            if (ret.value < this.value)
                ret.value = this.value;
        });
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

    $('#gesamtpreis')[0].innerText = gesamtpreis.toFixed(2).replace(".", ",");

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

        let time = Math.ceil((returndate - pickupdate) / 1000 / 60 / 60);

        if ($('#category')[0].innerText.includes("Sharing")) {
            return time;
        } else {
            return (time / 24) + 1;
        }
    }
}
