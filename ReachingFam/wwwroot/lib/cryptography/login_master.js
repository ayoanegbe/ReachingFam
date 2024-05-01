const frm = document.getElementById('login-form');
const sahan = "-@!8A0P.!nm099(+";
const eneh = "i+!_Ay(1_9-*!71O";


frm.addEventListener('submit', (event) => {
    event.preventDefault();
    const username = frm.elements['username'];
    const passwordtxt = frm.elements['passwordtxt'];
    const token = frm.elements['token'];
    const passPhrase = CryptoJS.enc.Utf8.parse(sahan);
    const iv = CryptoJS.enc.Utf8.parse(eneh);
    let un = CryptoJS.AES.encrypt(username.value, passPhrase, { iv: iv });
    let pw = CryptoJS.AES.encrypt(passwordtxt.value, passPhrase, { iv: iv });
    let tk = CryptoJS.AES.encrypt(token.value, passPhrase, { iv: iv });
       

    var form = document.createElement("form");
    var element1 = document.createElement("input");
    var element2 = document.createElement("input");
    var element3 = document.createElement("input");

    form.method = "POST";
    form.action = frm.action;

    element1.value = un;
    element1.name = "username";
    form.appendChild(element1);

    element2.value = pw;
    element2.name = "password";
    form.appendChild(element2);

    element3.value = tk;
    element3.name = "token";
    form.appendChild(element3);

    document.body.appendChild(form);

    form.submit();
});