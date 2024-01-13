function scr(countWorkArea) {
    displ('noscr', countWorkArea);
    displ('scr', countWorkArea);
};

function displ(ddd, countWorkArea) {
    if (document.getElementById(ddd).style.display == 'none') {
        document.getElementById(ddd).style.display = 'block';
    }
    else {
        document.getElementById(ddd).style.display = 'none';
    }
    allOff(countWorkArea);
};

function display(ddd) {
    var idtkr = ddd.split(' ')[1];
    $('#EditTkr').val(idtkr);

    var obj = new Object();
    var url = '/Technologies/addWAreas';


    $.ajax(
        {
            type: 'get',
            url: url,
            data: obj,
            contentType: 'application/json;charset=utf-8',
            success: function (html) {
                window
                $('#tag2').remove();
                $('.wAreasAdd').append(html);
            }
        });

};

function allOn(col) {

    for (var i = 0; i < col; i++) {
        var qq = 'idListArea ' + i.toString();
        if (document.getElementById(qq) != null)
        {
            ell = document.getElementById(qq);
            ell.style.backgroundColor = 'darkseagreen';
        }
    }

};

function allOff(col) {
 
    for (var i = 0; i < col; i++) {
        var qq = 'idListArea ' + i.toString();
        if (document.getElementById(qq) != null)
        {
            ell = document.getElementById(qq);
            ell.style.backgroundColor = 'gainsboro';
        }
        
    }

};

function sel(act) {
    var wa = $('#selWAreas').val();

    var obj = new Object();
    obj.act = act;
    obj.wAreas = wa;
    obj.year = $('#year').val();
    var url = '/Technologies/select';


    $.ajax(
        {
            type: 'get',
            url: url,
            data: obj,
            contentType: 'application/json;charset=utf-8',
            success: function (html) {
                $('#tag').remove();
                $('.Col2').append(html);

            }
        });

};

$('#selectWA').on('change', function () {
    if ($('#selectWA').prop('checked')) {
        $('.okBtn').attr('disabled', false);
    } else {
        $('.okBtn').attr('disabled', true);
    }
});



function selYear2() {
    
    var y = $('#year').val();
    var obj = new Object();
    obj.year = y;


    $.ajax(
        {
            type: 'Post',
            url: '/Technologies/ChangeYear2',
            data: obj,
            // contentType: 'application/json;charset=utf-8',
            success: function (h) {
                document.body.innerHTML = h;
                
                document.getElementById('noscr').style.display = 'none';

               
                    
                
            }
        });

    scr(countWorkArea); 
};

function toggleButtonColor(el) {
    el.style.backgroundColor = el.style.backgroundColor == 'gainsboro' ? 'darkseagreen' : 'gainsboro';
};

function calc(id) {
    //изменение цвета ячеек в таблице    
    var q = 'idListArea ' + id;
    var el = document.getElementById(q);
    toggleButtonColor(el);

    //Заполнение поля id-ми выделенных зеленым

    var str = '0';
    var col = $('#col').val();


    for (var i = 0; i < col; i++) {
        qq = 'idListArea ' + i;


        ell = document.getElementById(qq);
        if (ell != null) {
            var qqq = '#' + i;

            var w = $(qqq).val();
            if (ell.style.backgroundColor == 'darkseagreen') {
                str = str + ',' + w;
            }
        }

    }

    $('#selWAreas').val(str);


    var obj = new Object();
    obj.str = str;
    obj.idFarm = idF;

    $.ajax(
        {
            type: 'get',
            url: '/Technologies/allSguare',
            data: obj,
            contentType: 'application/json;charset=utf-8',
            success: function (s) {
                $('#allSguare').val(s);
            }
        });


};

function calc2(id) {
    var col = $('#col').val(); 
    allOff(col);
    //изменение цвета ячеек в таблице    
    var q = 'idListArea ' + id;
    var el = document.getElementById(q);
    toggleButtonColor(el);

    var obj = new Object();
    obj.id = id;
    obj.year = $('#year').val();
    var url = '/Technologies/select';


    $.ajax(
        {
            type: 'get',
            url: url,
            data: obj,
            contentType: 'application/json;charset=utf-8',
            success: function (html) {
                $('#tag').remove();
                $('.Col2').append(html);

            }
        });
};


