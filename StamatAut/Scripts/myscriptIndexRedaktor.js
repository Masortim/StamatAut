function GetIdM() {

    var y = $('#Name').val();
    //alert(y);
    var obj = new Object();
    obj.name = y;


    $.ajax(
        {
            type: 'Post',
            url: '/Redaktor/GetIdM',
            data: obj,
            // contentType: 'application/json;charset=utf-8',
            success: function (h) {
                //alert(h[0] + "," + h[1]);
                $('#IdM').val(h[0]);
                $('#Class').val(h[1]);
                if (h[0] == "-1") {
                    $('#Class').prop('disabled', false);
                }
                else $('#Class').prop('disabled', true);
            }
        });
};
function FormAgregateName() {
    // alert("Старт");
    //var x = $('#Name').val();
    var y = $('#Class').val();
    var z = $('#Trailer').val();
    // alert(x + "," + y + "," + z);
    if (y == "" && z != "") {
        $('#Name').val(z);
        $('#Class').prop('required', false);
    }
    if (y != "" && z == "") {
        $('#Name').val(y);
        $('#Trailer').prop('required', false);
    }
    if (y != "" && z != "") {
        $('#Name').val(y + "+" + z);
    }    
    if (y == "" && z == "") {
        $('#Class').prop('required', true);
        $('#Trailer').prop('required', true);
    }        
};
function CorrectQuantity() {

    var y = $('#Trailer').val();
    // alert(y);
    var obj = new Object();
    obj.name = y;
    var z = $('#Quantity').val();     

    $.ajax(
        {
            type: 'Post',
            url: '/Redaktor/GetTrailerQuantity',
            data: obj,
            // contentType: 'application/json;charset=utf-8',
            success: function (h) {                
                //alert(z + "," + h)
                if (h > 0) {
                    alert("введено:" + z + " в базе:" + h);
                    if (h < z)
                    {
                        alert("В наличии только " + h + " единиц выбранной сельхозтехники");
                        $('#Quantity').val(h); 
                    }                                     
                    $('#Quantity').prop('max', +h);
                }
                else
                {
               //     alert("нет");
                    $('#Quantity').prop('max', 1);
                    $('#Quantity').val(0);
                }
                
            }
        });
};

function FixOperation(a) {
    //alert(a);                     
    var y = $('#' + a).prop('checked');
    // alert(a + "," + y);
    if (y == true) {
        $('#' + a + '-GSML').prop('hidden', false);
        $('#' + a + '-GSM').prop('type', "text");
        $('#' + a + '-GSM').prop('size', "7");
        $('#' + a + '-SRL').prop('hidden', false);
        $('#' + a + '-SR').prop('type', "text");
        $('#' + a + '-SR').prop('size', "7");
    }
    else {
        $('#' + a + '-GSML').prop('hidden', true);
        $('#' + a + '-GSM').prop('type', "hidden");
        $('#' + a + '-GSM').prop('value', "");
        $('#' + a + '-SRL').prop('hidden', true);
        $('#' + a + '-SR').prop('type', "hidden");
        $('#' + a + '-SR').prop('value', "");
    }
};

    window.onload = function () {
        var a = $('.aaaaa').val();
        //alert(a);
        var arr = a.split(/[,]+/);
        var filteredArray = arr.filter(function (item, pos) {
            return arr.indexOf(item) == pos;
        });
        //alert(filteredArray);
        for (var i = 0; i < filteredArray.length - 1; i++) {
            $('#l_' + filteredArray[i]).prop('style', "color: black;");
            // style = "color: gray;"
        }
    };