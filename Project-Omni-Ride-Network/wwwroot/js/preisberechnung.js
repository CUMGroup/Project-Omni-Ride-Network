
function inputchange(basicprice, priceHD, priceIns) {
    console.log("change happened");
    
    let gesamtpreis;
    gesamtpreis = basicprice;

    if ($('#vollkasko')[0].checked) {
        console.log(priceIns);
        gesamtpreis += Number(priceIns);
    } else if ($('#teilkasko')[0].checked){
        gesamtpreis += 50;
    }

    if ($('#refuel')[0].checked) {
        gesamtpreis += 35;
    }

    $('#gesamtpreis')[0].innerText = gesamtpreis;
}

function checkinsurance(uncheck) {
    uncheck[0].checked = false;
}