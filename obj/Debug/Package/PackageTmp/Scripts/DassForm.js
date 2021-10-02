
$("body").on("click", "#btnReset", function () {

    console.log("test");
    $('input[name="rate1"]').prop('checked', false);
    $('input[name="rate2"]').prop('checked', false);
    $('input[name="rate3"]').prop('checked', false);
    $('input[name="rate4"]').prop('checked', false);
    $('input[name="rate5"]').prop('checked', false);
    $('input[name="rate6"]').prop('checked', false);
    $('input[name="rate6"]').prop('checked', false);
    $('input[name="rate7"]').prop('checked', false);
    $('input[name="rate8"]').prop('checked', false);
    $('input[name="rate9"]').prop('checked', false);
    $('input[name="rate10"]').prop('checked', false);
    $('input[name="rate11"]').prop('checked', false);
    $('input[name="rate12"]').prop('checked', false);
    $('input[name="rate13"]').prop('checked', false);
    $('input[name="rate14"]').prop('checked', false);
    $('input[name="rate15"]').prop('checked', false);
    $('input[name="rate16"]').prop('checked', false);
    $('input[name="rate17"]').prop('checked', false);
    $('input[name="rate18"]').prop('checked', false);
    $('input[name="rate19"]').prop('checked', false);
    $('input[name="rate20"]').prop('checked', false);
    $('input[name="rate21"]').prop('checked', false);

});






$("body").on("click", "#btnSaveSec1", function () {

    var data = new Array();
    var dat = {}, dat2 = {}, dat3 = {}, dat4 = {}, dat5 = {}, dat6 = {}, dat7 = {}, dat8 = {}, dat9 = {}, dat10 = {}, dat11 = {},
        dat12 = {}, dat13 = {}, dat14 = {}, dat15 = {}, dat16 = {}, dat17 = {}, dat18 = {}, dat19 = {}, dat20 = {}, dat21 = {};

    //question 1
    dat.Question = "1"; // untuk group radio button soalan 1
    dat.Answer = $("input[name=rate1]:checked").val(); // radio button value.. dpt by name = rate1 : checked
    if (dat.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 1 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat); // masukkan value radio button dlm array
    }

    //question 2
    dat2.Question = "2"; // untuk group radio button soalan 2
    dat2.Answer = $("input[name=rate2]:checked").val(); // radio button value.. dpt by name = rate2 : checked
    if (dat2.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 2 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat2); // masukkan value radio button dlm array
    }

    //question 3
    dat3.Question = "3"; // untuk group radio button soalan 3
    dat3.Answer = $("input[name=rate3]:checked").val(); // radio button value.. dpt by name = rate3 : checked
    if (dat3.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 3 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat3); // masukkan value radio button dlm array
    }

    //question 4
    dat4.Question = "4"; // untuk group radio button soalan 4
    dat4.Answer = $("input[name=rate4]:checked").val(); // radio button value.. dpt by name = rate4 : checked
    if (dat4.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 4 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat4); // masukkan value radio button dlm array
    }

    //question 5
    dat5.Question = "5"; // untuk group radio button soalan 5
    dat5.Answer = $("input[name=rate5]:checked").val(); // radio button value.. dpt by name = rate5 : checked
    if (dat5.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 5 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat5); // masukkan value radio button dlm array
    }

    //question 6
    dat6.Question = "6"; // untuk group radio button soalan 1
    dat6.Answer = $("input[name=rate6]:checked").val(); // radio button value.. dpt by name = rate6 : checked
    if (dat6.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 6 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat6); // masukkan value radio button dlm array
    }

    //question 7
    dat7.Question = "7"; // untuk group radio button soalan 1
    dat7.Answer = $("input[name=rate7]:checked").val(); // radio button value.. dpt by name = rate7 : checked
    if (dat7.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 7 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat7); // masukkan value radio button dlm array
    }

    //question 8
    dat8.Question = "8"; // untuk group radio button soalan 1
    dat8.Answer = $("input[name=rate8]:checked").val(); // radio button value.. dpt by name = rate8 : checked
    if (dat8.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 8 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat8); // masukkan value radio button dlm array
    }

    //question 9
    dat9.Question = "9"; // untuk group radio button soalan 1
    dat9.Answer = $("input[name=rate9]:checked").val(); // radio button value.. dpt by name = rate9 : checked
    if (dat9.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 9 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat9); // masukkan value radio button dlm array
    }

    //question 10
    dat10.Question = "10"; // untuk group radio button soalan 1
    dat10.Answer = $("input[name=rate10]:checked").val(); // radio button value.. dpt by name = rate10 : checked
    if (dat10.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 10 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat10); // masukkan value radio button dlm array
    }

    //question 11
    dat11.Question = "11"; // untuk group radio button soalan 1
    dat11.Answer = $("input[name=rate11]:checked").val(); // radio button value.. dpt by name = rate11 : checked
    if (dat11.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 11 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat11); // masukkan value radio button dlm array
    }

    //question 12
    dat12.Question = "12"; // untuk group radio button soalan 1
    dat12.Answer = $("input[name=rate12]:checked").val(); // radio button value.. dpt by name = rate12 : checked
    if (dat12.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 12 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat12); // masukkan value radio button dlm array
    }

    //question 13
    dat13.Question = "13"; // untuk group radio button soalan 1
    dat13.Answer = $("input[name=rate13]:checked").val(); // radio button value.. dpt by name = rate13 : checked
    if (dat13.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 13 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat13); // masukkan value radio button dlm array
    }

    //question 14
    dat14.Question = "14"; // untuk group radio button soalan 1
    dat14.Answer = $("input[name=rate14]:checked").val(); // radio button value.. dpt by name = rate14 : checked
    if (dat14.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 14 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat14); // masukkan value radio button dlm array
    }

    //question 15
    dat15.Question = "15"; // untuk group radio button soalan 1
    dat15.Answer = $("input[name=rate15]:checked").val(); // radio button value.. dpt by name = rate15 : checked
    if (dat15.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 15 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat15); // masukkan value radio button dlm array
    }

    //question 16
    dat16.Question = "16"; // untuk group radio button soalan 1
    dat16.Answer = $("input[name=rate16]:checked").val(); // radio button value.. dpt by name = rate16 : checked
    if (dat16.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 16 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat16); // masukkan value radio button dlm array
    }

    //question 17
    dat17.Question = "17"; // untuk group radio button soalan 1
    dat17.Answer = $("input[name=rate17]:checked").val(); // radio button value.. dpt by name = rate17 : checked
    if (dat17.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 17 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat17); // masukkan value radio button dlm array
    }

    //question 18
    dat18.Question = "18"; // untuk group radio button soalan 1
    dat18.Answer = $("input[name=rate18]:checked").val(); // radio button value.. dpt by name = rate18 : checked
    if (dat18.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 18 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat18); // masukkan value radio button dlm array
    }

    //question 19
    dat19.Question = "19"; // untuk group radio button soalan 1
    dat19.Answer = $("input[name=rate19]:checked").val(); // radio button value.. dpt by name = rate19 : checked
    if (dat19.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 19 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat19); // masukkan value radio button dlm array
    }

    //question 20
    dat20.Question = "20"; // untuk group radio button soalan 1
    dat20.Answer = $("input[name=rate20]:checked").val(); // radio button value.. dpt by name = rate20 : checked
    if (dat20.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 20 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat20); // masukkan value radio button dlm array
    }

    //question 21
    dat21.Question = "21"; // untuk group radio button soalan 1
    dat21.Answer = $("input[name=rate21]:checked").val(); // radio button value.. dpt by name = rate21 : checked
    if (dat21.Answer == null) { //alert kalau tk pilih lagi
        alert("Factor 21 not select yet, Please complete all factor before submit");
        return false;
    }
    else {
        data.push(dat21); // masukkan value radio button dlm array
    }


    //Send the JSON array to Controller using AJAX.
    $.ajax
        ({
            type: "POST",
            url: '/FarzanaDass/DassForm_post', // ActionResult name "controllername/actionname/" // check internet sbb ak dh lupe skit
            data: JSON.stringify(data), // push data ke controller
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {

                if (r.inSession == "Yes") {
                    alert("Check your new result in Counseling Session page.")
                }
                else {
                    alert("Your Dass Form result has been submitted. The counselor will accept your counseling session request, you can check if your request has been accepted by going to Counseling Session page. Thank you.");
                }
                window.location.href = r.redirectToUrl;
            }
        });
});