var speed = 10;

function incEltNbr() {
    eltUsers = document.getElementById('users');
    endUsers = Number(document.getElementById('users').innerHTML);
    incNbrRec(0, endUsers, eltUsers);

    eltTrips = document.getElementById('trips');
    endTrips = Number(document.getElementById('trips').innerHTML);
    incNbrRec(0, endTrips, eltTrips);
}

function incNbrRec(i, endNbr, elt) {
    if (i <= endNbr) {
        elt.innerHTML = i;
        setTimeout(function () {
            incNbrRec(i + 1, endNbr, elt);
        }, speed);
    }
}

window.onload = incEltNbr();