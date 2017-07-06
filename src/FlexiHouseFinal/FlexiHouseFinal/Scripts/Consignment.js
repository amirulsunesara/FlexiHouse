

$(window).load(function () 
{
    var $imageupload = $('.imageupload');
    $imageupload.imageupload();
    $('#imageupload-disable').on('click', function () {
        $imageupload.imageupload('disable');
        $(this).blur();
    })
    $('#imageupload-enable').on('click', function () {
        $imageupload.imageupload('enable');
        $(this).blur();
    })
    $('#imageupload-reset').on('click', function () {
        $imageupload.imageupload('reset');
        $(this).blur();
    });


    function check(input) {
        if (input.value != document.getElementById('password').value) {
            input.setCustomValidity('Password Must be Matching.');
            alert('dasads');
        } else {
            // input is valid -- reset the error message
            input.setCustomValidity('');
        }
    }
    function alertFunction() {
        alert('Consignment Has Been Successfully Saved');
        window.location.href = "/Consignments/Index";
     
    }
    $(".dispatchOrders").click(function () {
        var id = $(this).attr("id");
      
        var confirmed = confirm("Are you sure you want to dispatch this order?");
        if (confirmed)
        {
            $.ajax({
                url: "/Order/DispatchOrders",
                type: "GET",
                data: { 'id': id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    alert("Order Has Been Successfully Dispatched");
                    window.location.href = "/Order/GetOrders";
                },
                error:function(){
        
                }


            });





           
        }

    });


    $(".cancelorder").click(function () {
        var confirmed = confirm("Are you sure you want to cancel this order?");
        if (confirmed) {
            var id = $(this).attr('id');
            myArray = id.split(/[a-zA-Z]+/);

            $.ajax({
                url: "/Order/UpdateOrderCancel",
                type: "POST",
                data: JSON.stringify({ status: myArray[1] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    window.location.href = "/Order/GetOrders";
                },
                error: function () {

                }


            });

        }
    });

    $(".ordername").click(function () {
        var id = $(this).attr('id');
        myArray = id.split(/[a-zA-Z]+/);
    


        $.ajax({
            url: '/Order/UpdateOrderStatus',
            type: "POST",
            data: JSON.stringify({ status: myArray[1] }),

            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function () {
               // alert('Warehouse Has Been Successfully Saved!');
        
            },
            error: function (err) {
//window.location.href = "/Consignments/Index";


            }
        });



    });

    if (window.location.pathname == "/Consignments/Create") {
        populateCountries("txtCountry1");
    }
    var counter;
    var LengthandWidth = [];
    LengthandWidth = getValues();
    if ($('#currentCount').val() == "") {
        counter = 2;
        
    }
    else {
        counter = $('#currentCount').val();
     
    }
    $("#addButton").click(function (e) {
        e.preventDefault();
   var newTextBoxDiv = $(document.createElement('tr')).attr("id", 'TextBoxDiv' + counter);

   newTextBoxDiv.html('<td><input type="text" name="itemName" id="txtItemName' + counter + '" class="form-control r' + counter + '" required></td><td><input type="number" name="itemQuantity" min="1" id="txtQuantity' + counter + '" class="form-control r' + counter + '" required></td><td><input type="date" name="expiryDate" id="txtExpiryDate' + counter + '" class="form-control datepicker r' + counter + '"></td><td><input type="text" name="productCode" id="txtProductCode' + counter + '" class="form-control r' + counter + '" required></td><td><input type="text" name="itemManufacturer" id="txtManufacturer' + counter + '" class="form-control r' + counter + '" required></td><td><select type="text" id="txtCountry' + counter + '" name="itemCountry" class="form-control r' + counter + '" required></select></td><td><a href="javascript:void(0);" class="remCF btn btn-danger">Remove</a><td>');

   newTextBoxDiv.appendTo("#TextBoxesGroup");
   populateCountries("txtCountry"+counter);

   $("#txtItemName"+counter).autocomplete({

       source: function (request, response) {
           $.ajax({
               url: '/Consignments/AutoComplete/',
               data: "{ 'prefix': '" + request.term + "'}",
               dataType: "json",
               type: "POST",
               contentType: "application/json; charset=utf-8",
               success: function (data) {

                   response($.map(data, function (item) {

                       return item;

                   }));
               },
               error: function (response) {
                   alert(response.responseText);
               },
               failure: function (response) {
                   alert(response.responseText);
               }
           });
       },
       select: function (e, i) {
           $("#hfCustomer").val(i.item.val);
       },
       minLength: 1
   }).on('autocompleteresponse autocompleteselect', function (e, ui) {


       var t = $(this),
             details = $('#txtCountry'+counter),
             label = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
             value = (e.type == 'autocompleteresponse' ? ui.content[0].country : ui.item.country);

       t.val(label);
       details.val(value);


       var t2 = $(this),
             details2 = $('#txtManufacturer'+counter),
             label2 = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
             value2 = (e.type == 'autocompleteresponse' ? ui.content[0].manufacturer : ui.item.manufacturer);
       t2.val(label2);
       details2.val(value2);

       var t3 = $(this),
           details3 = $('#txtProductCode'+counter),
           label3 = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
           value3 = (e.type == 'autocompleteresponse' ? ui.content[0].itemCode : ui.item.itemCode);

       t3.val(label3);
       details3.val(value3);
       return false;

   });




   $('#txtExpiryDate' + counter).datepicker({
       
      minDate: '+1d'

   });
        $("#btnDelete" + counter).click(function () {

            $('#TextBoxDiv' + counter).remove();
        });
     
        counter++;
    });
    $("#TextBoxesGroup").on('click', '.remCF', function () {
        $(this).parent().parent().remove();
    });
  

    $("#getButtonValue").click(function () {

        var msg = '';
        for (i = 1; i < counter; i++) {
            msg += "\n Textbox #" + i + " : " + $('#textbox' + i).val();
        }
        alert(msg);
    });


    function getValues() {
        var cordis = [];

        $.ajax({
            url: "/Consignments/GetConsignmentId",
            data: "",
            type: "GET",
            dataType: "json",
            success: function (data) {

            var abc = loadData(data);
            $('#consignmentNumber').text(abc[0]+1);

            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });

        //  currentjqXHR.success(function (data) { alert("hogya"); cordis = loadData(data); });
        return cordis;
    }


    function loadData(data) {
        // Here we will format & load/show data
        var Values = [];
        $.each(data, function (i, val) {
            // Append database data here

            Values[0] = val.id;
           


        });
        return Values;
    };

  
    $("#txtItemName1").autocomplete({
      
        source: function (request, response) {
            $.ajax({
                url: '/Consignments/AutoComplete/',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                   
                    response($.map(data, function (item) {
                     
                        return item;

                    }));
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#hfCustomer").val(i.item.val);
        },
        minLength: 1
    }).on('autocompleteresponse autocompleteselect', function (e, ui) {
       
           
        var t = $(this),
              details = $('#txtCountry1'),
              label = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
              value = (e.type == 'autocompleteresponse' ? ui.content[0].country : ui.item.country);

        t.val(label);
        details.val(value);


        var t2 = $(this),
              details2 = $('#txtManufacturer1'),
              label2 = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
              value2 = (e.type == 'autocompleteresponse' ? ui.content[0].manufacturer : ui.item.manufacturer);
        t2.val(label2);
        details2.val(value2);

        var t3 = $(this),
            details3 = $('#txtProductCode1'),
            label3 = (e.type == 'autocompleteresponse' ? ui.content[0].label : ui.item.label),
            value3 = (e.type == 'autocompleteresponse' ? ui.content[0].itemCode : ui.item.itemCode);

        t3.val(label3);
        details3.val(value3);

        return false;
    });

    $("#btnSaveConsignment").click(function (e) {
        e.preventDefault();
        var isCorrect = true;
        $('input[name="itemName"]').each(function () {
            if ($(this).val() == "") {
                alert("Please Enter Item Name");
                isCorrect = false;
            }

        });
        $('input[name="itemQuantity"]').each(function () {
            if ($(this).val() < 0 || $(this).val()=="") {
                alert("Please Enter valid Quanitity of Item");
                isCorrect = false;
            }

        });
        $('input[name="expiryDate"]').each(function () {

            var name = $(this).val();
            var ans = name.match("([0-9]{2}[/][a-zA-Z]{3}[/][0-9]{4})|([0-9]{2}[/][0-9]{2}[/][0-9]{4})");

            if ($(this).val() != "" && ans==null) {
                alert("Please Select Correct Expiry Date");
                isCorrect = false;
            }

        });
        $('input[name="productCode"]').each(function () {
            if ($(this).val() == "") {
                alert("Please Enter Valid Product Code");
                isCorrect = false;
            }

        });
        $('input[name="itemManufacturer"]').each(function () {
            if ($(this).val() == "") {
                alert("Please Enter Valid Manufacturer Name");
                isCorrect = false;
            }

        });
        $('select[name="itemCountry"]').each(function () {
         
            if ($(this).val() == "-1") {
                alert("Please Select Valid Country");
                isCorrect = false;
            }
    

        });

        if ($('#txtSupplierName').val() == "" || $('#txtArrivalDate').val()=="") {
            alert('Please Enter Correct Details');
            isCorrect = false;
        }
        if(isCorrect==true) {
            var r = confirm("Confirm Save Changes? ");

            if (r == true) {

                var arrayItems = [];
                var arrayItem_Consignment = [];
                var conn = [];
                var Consignment = {};
                var totalI = 0;
                Consignment.supplier = $('#txtSupplierName').val();
                Consignment.arrivalDate = $('#txtArrivalDate').val();
                var jkl;
                for (var i = 1 ; i <= counter; i++) {

                    Item = {};
                    Item_Consignment = {};
                    $('.r' + i).each(function () {

                        if ($(this).attr("id") == "txtItemName" + i) {
                            totalI = totalI + 1;

                            Item.itemName = $('#' + $(this).attr("id")).val();
                        }
                        else if ($(this).attr("id") == "txtManufacturer" + i) {
                            Item.Manufacturer = $('#' + $(this).attr("id")).val();
                        }
                        else if ($(this).attr("id") == "txtCountry" + i) {
                            Item.Country = $('#' + $(this).attr("id")).val();
                        }
                        else if ($(this).attr("id") == "txtProductCode" + i) {
                            Item.itemCode = $('#' + $(this).attr("id")).val();
                        }
                        else if ($(this).attr("id") == "txtQuantity" + i) {
                            Item_Consignment.quantity = $('#' + $(this).attr("id")).val();
                        }
                        else if ($(this).attr("id") == "txtExpiryDate" + i) {
                            Item_Consignment.expiry = $('#' + $(this).attr("id")).val();
                        }

                    });
                    Consignment.totalItems = totalI;
                    arrayItems.push(Item);
                    arrayItem_Consignment.push(Item_Consignment);
                    conn.push(Consignment);

                }


                $.ajax({
                    url: '/Consignments/Save',
                    type: "POST",
                    data: JSON.stringify({ ware: arrayItems, itemcon: arrayItem_Consignment, con: conn }),

                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function () {
                     
                        $('#loadingDiv').css('display', '');
                         jkl = setTimeout(alertFunction, 3000);
                     
                      
                          
                      
                    },
                    error: function (err) {

                        $('#loadingDiv').css('display', '');
                     jkl = setTimeout(alertFunction, 3000);
                           
                    
                    }
                });

            }
        }
    });

    $("#btnUpdateConsignment").click(function (e) {



        var r = confirm("Confirm Save Changes? ");

        if (r == true) {

            var arrayItems = [];
            var arrayItem_Consignment = [];
            var conn = [];
            var Consignment = {};
            var totalI = 0;
            Consignment.supplier = $('#txtSupplierName').val();
            Consignment.arrivalDate = $('#txtArrivalDate').val();

            for (var i = 1 ; i <= counter; i++) {

                Item = {};
                Item_Consignment = {};
                a = 1;
                $('.r' + i).each(function () {
                    a++;

                });
            
                $('.r' + i).each(function () {

                   
                    if ($(this).attr("id") == "txtItemName" + i) {
                        totalI = totalI + 1;

                        Item.itemName = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "txtManufacturer" + i) {
                        Item.Manufacturer = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "txtCountry" + i) {
                        Item.Country = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "txtProductCode" + i) {
                        Item.itemCode = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "txtQuantity" + i) {
                        Item_Consignment.quantity = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "txtExpiryDate" + i) {
                        Item_Consignment.expiry = $('#' + $(this).attr("id")).val();
                    }

                });
                if (a < 2)
                {
                    continue;
                }
                Consignment.totalItems = totalI;
                Consignment.id = $("#consignmentNumber2").text();
                arrayItems.push(Item);
                arrayItem_Consignment.push(Item_Consignment);
                conn.push(Consignment);

            }


            $.ajax({
                url: '/Consignments/Update',
                type: "POST",
                data: JSON.stringify({ ware: arrayItems, itemcon: arrayItem_Consignment, con: conn }),

                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function () {
                    alert('Warehouse Has Been Successfully Saved!');
                   
                },
                error: function (err) {
                    window.location.href = "/Consignments/Index";


                }
            });

        }

    });
    $(".deleter").click(function () {

        var r = confirm("Confirm delete this item? ");

        if (r == true){
        var iid = $(this).attr('id');
       
        var cid = $('#consignmentNumber2').text();

       
        var arrayItem_Consignment = [];
        Item_Consignment = {};
        Item_Consignment.itemId = iid;
        Item_Consignment.consignmentId = cid;
        arrayItem_Consignment.push(Item_Consignment);
        $.ajax({
            url: '/Consignments/DeleteItem',
            type: "POST",
            data: JSON.stringify({ itemcon: arrayItem_Consignment }),

            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function () {
               
                location.reload();
            },
            error: function (err) {
                location.reload();


            }
        });
    }
    });
  
});
