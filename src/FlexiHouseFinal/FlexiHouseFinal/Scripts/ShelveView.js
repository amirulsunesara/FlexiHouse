
var currentShelve = "";
function getValues(id, l) {
    var cordis = [];

    var currentjqXHR = jQuery.ajax({
        url: "/Home/GetAllItems",
        data: { 'id': id },
        type: "GET",
        dataType: "json",
        success: function (data) {

            abc = loadData(data);

            cordis[0] = abc[0];
            cordis[1] = abc[1];
            cordis[2] = abc[2];
            cordis[3] = abc[3];

            $('#shelveID').text(cordis[0]);
            myList = jQuery.parseJSON(cordis[1]);
            $("#mytable tr").remove();
            $('#shelvePagination li').remove();
        
            $('#drawTable tr').remove();
            jsonFunction(myList);

            drawShelve(myList, cordis[2], l);
            drawPagination(cordis[3]);
            $('#pager' + l).addClass('active');
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });

    //  currentjqXHR.success(function (data) { alert("hogya"); cordis = loadData(data); });

}


function getDetails(id,slotNo) {
    var cordis = [];

    var currentjqXHR = jQuery.ajax({
        url: "/Home/GetAllItems",
        data: { 'id': id },
        type: "GET",
        dataType: "json",
        success: function (data) {

            abc = loadData(data);

            cordis[0] = abc[0];
            cordis[1] = abc[1];
            cordis[2] = abc[2];
            cordis[3] = abc[3];

        
            myList = jQuery.parseJSON(cordis[1]);
        
            detailsFunction(myList,slotNo);

        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });

    //  currentjqXHR.success(function (data) { alert("hogya"); cordis = loadData(data); });

}
function jsonFunction(obj) {
    var tr0 = "<tr><th>Slot No</th><th>Item Name</th></tr>";
    $("#mytable").append(tr0);
    for (var i = 0; i < obj.length; i++) {


        var tr = "<tr>";
        var td1 = "<td>" + obj[i]["slot_id"] + "</td>";
        var td5 = "<td>" + obj[i]["item_id"] + "</td>";

        var td2 = "<td>" + obj[i]["item_name"] + "</td>";
        var td3 = "<td>" + obj[i]["expiry_date"] + "</td></tr>";




        $("#mytable").append(tr + td1 + td2);

    }
}
function detailsFunction(obj,slotNo) {
    
    for (var i = 0; i < obj.length; i++) {

        if (obj[i]["slot_id"] == slotNo)
        {
            $('#itemName').text(obj[i]["item_name"]);
            $('#itemExpiry').text(obj[i]["expiry_date"]);
          
            $('#itemId').text(obj[i]["item_id"]);
        }
      

    }
}
function drawShelve(obj, RowSize, l) {

    var v1 = "<tr>";
    var backup = RowSize;



    for (var i = 0; i < obj.length; i++) {
        if (backup == 0) {

            backup = RowSize;
            v1 = v1 + "</tr>";
            $('#drawTable').append(v1);
            v1 = "<tr>";

        }
        if (obj[i]["section_id"] == l) {
            var status = "";
            if (obj[i]["item_name"] == "") {

                status = "empty";
            }
            else {

                status = "filled"
            }

            v1 = v1 + "<td class='shelf'><div id='"+obj[i]["slot_id"]+"' class='" + status + "'>'<label style='color:white;'>" +obj[i]["slot_id"]+ "</label>'</div></td>"
            backup--;
        }


    }
    v1 = v1 + "</tr>";
    $('#drawTable').append(v1);

   
}

function drawPagination(sections) {

    $('#shelvePagination').append('<li class="sPager" id="pager1"><a>1</a></li>');
    for (i = 2; i <= sections; i++) {
        $('#shelvePagination').append('<li id="pager' + i + '" class="sPager"> <a>' + i + '</a></li>');


    }

}
function loadData(data) {
    // Here we will format & load/show data
    var Values = [];
    $.each(data, function (i, val) {
        // Append database data here

        Values[0] = val.shelfName;
        Values[1] = val.shelfItems;
        Values[2] = val.warehouse_id;
        Values[3] = val.slotsRemaining;


    });
    return Values;
};





$(document).on("click", ".sPager", function (event) {
   
    $('#slotNo').text("");
    $('#itemName').text("");
    $('#itemExpiry').text("");
    $('#manufacturer').text("");


    var id = $(this).attr('id');
    var splits = id.split(/(\d+)/);

    setTimeout(function () { $('#drawTable').fadeOut(1000); }, 0);


  
  

    setTimeout(function () {

        getValues(currentShelve, splits[1]);
        $('#drawTable').fadeIn();
    }, 1000);




    $('li').removeClass('active');
    $(this).addClass('active');

});

$(document).on("hover", ".filled", function (event) {
    
    $('#slotNo').text($(this).attr('id'));
    $('#itemName').text("");
    $('#itemExpiry').text("");
    $('#manufacturer').text("");
   getDetails(currentShelve, $(this).attr('id'));

});
$(document).on("click", ".filled", function (event) {

    $('#myModal3').modal('show');
    $('#detailsFade').fadeOut(500);
    setTimeout(function () { getItemDetails($('#itemId').text()); }, 500);
    


});
function getItemDetails(id) {
    var cordis = [];

    var currentjqXHR = jQuery.ajax({
        url: "/Home/GetSpecificItem",
        data: { 'id': id },
        type: "GET",
        dataType: "json",
        success: function (data) {

            abc = loadItemData(data);

           
            var src = "data:image/jpeg;base64," + abc[4];
            $('#imgDiv').html('<img src="'+src+'" style="max-width: 175px; max-height: 250px">')
            $('#lblManufacturer').text(abc[0]);
            $('#lblItemCode').text(abc[1]);
            $('#lblCountry').text(abc[5]);
            $('#lblDimensions').text(abc[2]);
            $('#lblWeight').text(abc[3]);
            $('#detailsFade').fadeIn(500);
        },
        error: function (error) {
      
        }
    });
}
function loadItemData(data) {
    // Here we will format & load/show data
    var Values = [];
    var i = 0;
    $.each(data, function (i, val) {
        // Append database data here

        Values[i] = val;
        i++;

    });
    return Values;
};


$(document).on("hover", ".empty", function (event) {
    $('#itemName').text("");
    $('#slotNo').text($(this).attr('id'));
 
    $('#itemExpiry').text("");
    $('#manufacturer').text("");

});



$(window).load(function () {
    $("#accordion").accordion({
        collapsible: true,
        active: false
    });
    $("#accordion2").accordion({
        collapsible: true,
        active: false
    });
    $("#accordion3").accordion({
        collapsible: true,
        active: false
    });
    $("#accordion4").accordion({
        collapsible: true,
        active: false
    });
   // $('#warehouseCol').addClass('active');
    //  $('#dispme').css("display", "block");
   

  
    if (window.location.pathname == "/Home/ViewWarehouse") {




        $('.dragMe').click(function () {
            $('#theShelfName').text($(this).attr('id'));
            $('#slotNo').text("");
            $('#itemName').text("");
            $('#itemExpiry').text("");
            $('#manufacturer').text("");
            $('#myModal2').modal('show');
         
         
        });
    


    }


    if (window.location.pathname == "/Home/ViewWarehouse") {
        $('.dragMe')
         .draggable({ cancel: 'img' });

        $('.depot')
            .draggable({ cancel: 'img' });

        $('.Wally')
            .draggable({ cancel: 'img' });
        $('.office')
            .draggable({ cancel: 'img' });

    }

    myList = [];
    var col = [];
    $('.dragMe').click(function () {
        var id = $(this).attr('id');
        currentShelve = id;
        getValues(id,1);
       
    });
   

    
  
});