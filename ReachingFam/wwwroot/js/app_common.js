
'use strict'

const sahan = "-@!8A0P.!nm099(+";
const eneh = "i+!_Ay(1_9-*!71O";

var _encryptAES = function (_req) {
    var key = CryptoJS.enc.Utf8.parse(sahan);
    var iv = CryptoJS.enc.Utf8.parse(eneh);
    let json = JSON.stringify(_req);
    return CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(json), key, { iv: iv }).toString();
};

var _decryptAES = function (ciphertext) {
    //console.log(ciphertext);
    var ciphertextWA = CryptoJS.enc.Base64.parse(ciphertext);
    var key = CryptoJS.enc.Utf8.parse(sahan);
    var iv = CryptoJS.enc.Utf8.parse(eneh);
    var ciphertextCP = { ciphertext: ciphertextWA };
    var decrypted = CryptoJS.AES.decrypt(ciphertextCP, key, { iv: iv });
    //console.log(decrypted.toString(CryptoJS.enc.Utf8));
    return JSON.parse(decrypted.toString(CryptoJS.enc.Utf8))
};


function isDecimalKey(evt) {
    const items = ['-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    if (!items.includes(evt.key))
        return false;
    return true;
}

function isNumberKey(evt) {
    const items = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    if (!items.includes(evt.key))
        return false;
    return true;
}

function formatCurr(value) {
    if (isNaN(value))
        value = 0;

    let isPositive = value >= 0;
    value = Math.floor(Math.abs(value) * 100 + 0.50000000001);
    let decimal = value % 100;
    value = Math.floor(value / 100).toString();

    // Insert commas for thousands separation
    for (let i = 0; i < Math.floor(value.length / 3); i++) {
        let index = value.length - (3 * i + 3);
        if (index > 0)
            value = value.slice(0, index) + ',' + value.slice(index);
    }

    let sign = isPositive ? '' : '-';
    return sign + value + '.' + (decimal < 10 ? '0' + decimal : decimal);
}

function parseFormattedNumber(numberString) {
    // Remove commas from the string and convert it to a float
    let floatNumber = round(parseFloat(numberString.replace(/,/g, '')), 2);
    return floatNumber;
}

function setTotalWeight() {
    var npWeight = parseFormattedNumber(document.getElementById("NonPerishablesWeight").value);
    if (isNaN(npWeight))
        npWeight = 0;

    var pWeight = parseFormattedNumber(document.getElementById("PerishablesWeight").value);
    if (isNaN(pWeight))
        pWeight = 0;

    var fWeight = parseFormattedNumber(document.getElementById("FrozenWeight").value);
    if (isNaN(fWeight))
        fWeight = 0;

    var nfWeight = parseFormattedNumber(document.getElementById("NonFoodWeight").value);
    if (isNaN(nfWeight))
        nfWeight = 0;

    var total = npWeight + pWeight + fWeight + nfWeight;

    document.getElementById("TotalWeight").value = round(total, 2);
  
}

function round(num, decimalPlaces = 0) {
    var p = Math.pow(10, decimalPlaces);
    var n = (num * p) * (1 + Number.EPSILON);
    return Math.round(n) / p;
}

let NonPerishablesWeight = document.getElementById("NonPerishablesWeight");
NonPerishablesWeight.addEventListener("blur", formatNonPerishablesWeight, true);
function formatNonPerishablesWeight() {
    
    let value = parseFormattedNumber(document.getElementById("NonPerishablesWeight").value);

    this.value = formatCurr(value);

    setTotalWeight();   

}

let PerishablesWeight = document.getElementById("PerishablesWeight");
PerishablesWeight.addEventListener("blur", formatPerishablesWeight, true);
function formatPerishablesWeight() {

    let value = parseFormattedNumber(document.getElementById("PerishablesWeight").value);

    this.value = formatCurr(value);

    setTotalWeight();
}

let FrozenWeight = document.getElementById("FrozenWeight");
FrozenWeight.addEventListener("blur", formatFrozenWeight, true);
function formatFrozenWeight() {

    let value = parseFormattedNumber(document.getElementById("FrozenWeight").value);

    this.value = formatCurr(value);

    setTotalWeight();
}

let NonFoodWeight = document.getElementById("NonFoodWeight");
NonFoodWeight.addEventListener("blur", formatNonFoodWeight, true);
function formatNonFoodWeight() {

    let value = parseFormattedNumber(document.getElementById("NonFoodWeight").value);

    this.value = formatCurr(value);

    setTotalWeight();
}

let TotalWeight = document.getElementById("TotalWeight");
TotalWeight.addEventListener("blur", formatTotalWeight, true);
function formatTotalWeight() {

    let value = parseFormattedNumber(document.getElementById("TotalWeight").value);

    this.value = formatCurr(value);
}

var loadingTask = pdfjsLib.getDocument('path/to/your/document.pdf');
loadingTask.promise.then(function (pdf) {
    pdf.getPage(1).then(function (page) {
        var scale = 1.5;
        var viewport = page.getViewport({ scale: scale, });

        // Prepare canvas using PDF page dimensions
        var canvas = document.getElementById('the-canvas');
        var context = canvas.getContext('2d');
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: context,
            viewport: viewport,
        };
        page.render(renderContext);
    });
});

function isEmail(email) {
    var emailRegex = /^[a-z0-9]+@[a-z]+\.[a-z]{2,3}$/; return emailRegex.test(email);
}

function tableToCSV() {

    // Variable to store the final csv data
    let csv_data = [];

    // Get each row data
    let rows = document.getElementsByTagName('tr');
    for (let i = 0; i < rows.length; i++) {

        // Get each column data
        let cols = rows[i].querySelectorAll('td,th');

        // Stores each csv row data
        let csvrow = [];
        for (let j = 0; j < cols.length; j++) {

            // Get the text data of each cell
            // of a row and push it to csvrow
            csvrow.push(cols[j].innerHTML);
        }

        // Combine each column value with comma
        csv_data.push(csvrow.join(","));
    }

    // Combine each row data with new line character
    csv_data = csv_data.join('\n');

    // Call this function to download csv file
    downloadCSVFile(csv_data);

}

function downloadCSVFile(csv_data) {

    // Create CSV file object and feed
    // our csv_data into it
    CSVFile = new Blob([csv_data], {
        type: "text/csv"
    });

    // Create to temporary link to initiate
    // download process
    let temp_link = document.createElement('a');

    // Download csv file
    temp_link.download = "RFF.csv";
    let url = window.URL.createObjectURL(CSVFile);
    temp_link.href = url;

    // This link should not be displayed
    temp_link.style.display = "none";
    document.body.appendChild(temp_link);

    // Automatically click the link to
    // trigger download
    temp_link.click();
    document.body.removeChild(temp_link);
}

function handleNonPerishables(cb) {
    if (cb.checked) {
         //alert("checked");
        document.getElementById("NonPerishablesWeight").removeAttribute('disabled');
        document.getElementById("NonPerishablesWeight").style.backgroundColor = "";
    }
    else {
        //alert("unchecked");
        document.getElementById("NonPerishablesWeight").style.backgroundColor = '#f7f8f9';
        var currentValue = document.getElementById("NonPerishablesWeight").value;
        document.getElementById("TotalWeight").value = round(parseFormattedNumber(document.getElementById("TotalWeight").value) - parseFormattedNumber(currentValue), 2);
        document.getElementById("NonPerishablesWeight").value = "";
        document.getElementById("NonPerishablesWeight").setAttribute('disabled', 'disabled');
        
    }
}

function handlePerishables(cb) {
    if (cb.checked) {
        //alert("checked");
        document.getElementById("PerishablesWeight").removeAttribute('disabled');
        document.getElementById("PerishablesWeight").style.backgroundColor = "";
    }
    else {
        //alert("unchecked");
        document.getElementById("PerishablesWeight").style.backgroundColor = '#f7f8f9';
        var currentValue = document.getElementById("PerishablesWeight").value;
        document.getElementById("TotalWeight").value = round(parseFormattedNumber(document.getElementById("TotalWeight").value) - parseFormattedNumber(currentValue), 2);
        document.getElementById("PerishablesWeight").value = "";
        document.getElementById("PerishablesWeight").setAttribute('disabled', 'disabled');

    }
}

function handleFrozen(cb) {
    if (cb.checked) {
        //alert("checked");
        document.getElementById("FrozenWeight").removeAttribute('disabled');
        document.getElementById("FrozenWeight").style.backgroundColor = "";
    }
    else {
        //alert("unchecked");
        document.getElementById("FrozenWeight").style.backgroundColor = '#f7f8f9';
        var currentValue = document.getElementById("FrozenWeight").value;
        document.getElementById("TotalWeight").value = round(parseFormattedNumber(document.getElementById("TotalWeight").value) - parseFormattedNumber(currentValue), 2);
        document.getElementById("FrozenWeight").value = "";
        document.getElementById("FrozenWeight").setAttribute('disabled', 'disabled');

    }
}

function handleNonFood(cb) {
    if (cb.checked) {
        //alert("checked");
        document.getElementById("NonFoodWeight").removeAttribute('disabled');
        document.getElementById("NonFoodWeight").style.backgroundColor = "";
    }
    else {
        //alert("unchecked");
        document.getElementById("NonFoodWeight").style.backgroundColor = '#f7f8f9';
        var currentValue = document.getElementById("NonFoodWeight").value;
        document.getElementById("TotalWeight").value = round(parseFormattedNumber(document.getElementById("TotalWeight").value) - parseFormattedNumber(currentValue), 2);
        document.getElementById("NonFoodWeight").value = "";
        document.getElementById("NonFoodWeight").setAttribute('disabled', 'disabled');

    }
}


