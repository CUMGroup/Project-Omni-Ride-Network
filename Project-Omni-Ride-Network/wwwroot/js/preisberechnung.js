


function inputchange(basicprice, priceHD, priceIns) {
    console.log("change happened");

    console.log($("#pickupdate")[0].value);

    let gesamtpreis;
    gesamtpreis = fixNumber(basicprice);

    if ($('#vollkasko')[0].checked) {
        console.log(fixNumber(priceIns));
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
        console.log("no date");
        return 1;
    } else {
        /*pickMonth = pickupdate.substring(5, 7);
        returnMonth = returndate.substring(5, 7);

        pickDay = pickupdate.substring(8);
        returnDay = returndate.substring(8);

        if (pickMonth == returnMonth) {
            return (returnDay - pickDay) + 1;
        }*/

        pickupdate = new Date(pickupdate);
        returndate = new Date(returndate);

        return Math.round((returndate - pickupdate) / 1000 / 60 / 60 / 24);

        console.log(Math.round((returndate - pickupdate) / 1000 / 60 / 60 / 24));
    }
}