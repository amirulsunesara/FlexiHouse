$(window).load(function () {

    var counter;
    var warehouseKey = $('#warehouseKey').val();

    if (window.location.pathname == "/Order/PlaceOrderComplete") {

        
       fillItemBox('1',warehouseKey );

    }

    $("#orderComplete").click(function (e) {

        var isValid = true;
        $('[name="ddItemName"]').each(function () {
            if ($(this).val() == "")
            {
                alert("Please Select Item Name");
                isValid = false;
            }
        });
        $('[name="ddQuantity"]').each(function () {
            if ($(this).val() == "") {
                alert("Please Select Item Quantity");
                isValid = false;
            }
        });
        if (isValid) {

            var arrayItems = [];



            for (var i = 1 ; i <= counter; i++) {

                Item = {};

                $('.r' + i).each(function () {

                    if ($(this).attr("id") == "ddItemName" + i) {

                        Item.itemName = $('#' + $(this).attr("id") + " option:selected").text();

                        Item.id = $('#' + $(this).attr("id")).val();
                    }
                    else if ($(this).attr("id") == "ddQuantity" + i) {
                        Item.Manufacturer = $('#' + $(this).attr("id")).val();
                    }


                });

                arrayItems.push(Item);


            }


            $.ajax({
                url: '/Order/PlaceOrderDoIt',
                type: "POST",
                data: JSON.stringify({ ware: arrayItems }),

                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function () {
                    // alert('Consignment Has Been Successfully Saved!');
                    window.location.href = "/Order/OrderReview";
                },
                error: function (err) {
                    //  alert('Consignment Has Been Successfully Saved!');
                    window.location.href = "/Order/OrderReview";
                }
            });


        }
    });


 
 
  
    $('#btnPlaceOrder').click(function () {

        window.location.href = "/Order/PlaceOrder";

    });
    $("#tblItems").on('click', '.remCF', function () {
        $(this).parent().parent().remove();
    });
   
 



    if ($('#currentCount').val() == "") {
        counter = 2;

    }
    else {
        counter = $('#currentCount').val();

    }

    $("#addButton").click(function (e) {
        e.preventDefault();
        var newTextBoxDiv = $(document.createElement('tr')).attr("id", 'TextBoxDiv' + counter);

        newTextBoxDiv.html('<td><select style="" class="form-control r' + counter + '" id="ddItemName' + counter + '" name="ddItemName"></select></td>     <td><label id="lblManufacturer' + counter + '" class="form-control r' + counter + '"></label></td>       <td><label id="lblItemCode' + counter + '" class="form-control r' + counter + '"></label></td>        <td><label id="lblAvailableQuantity' + counter + '" class="form-control r' + counter + '"></label></td>    <td><select style="" class="form-control r' + counter + '" id="ddQuantity' + counter + '" name="ddQuantity"></select></td>   <td><label id="lblItemId' + counter + '" style="visibility:hidden;"></label></td><td><a href="javascript:void(0);" class="remCF btn btn-danger">Remove</a></td>');
     
                                     
                                   
        newTextBoxDiv.appendTo("#tblItems");





        fillItemBox(counter, warehouseKey);



        $('[name="ddItemName"]').on('change', function () {

            var myid = $(this).attr('id');


            var matches = myid.match(/\d+$/);

            var newid = '#ddItemName' + matches[0];
     
            var selected = $(newid+' option:selected').val();

            fillOtherBox(matches[0], warehouseKey,selected);
          // 

            //   getDetails(selected);




        });







        counter++;
    });
    $('[name="ddItemName"]').on('change', function () {

        var myid = $(this).attr('id');


        var matches = myid.match(/\d+$/);

        var newid = '#ddItemName' + matches[0];

        var selected = $(newid + ' option:selected').val();
  
        fillOtherBox(matches[0], warehouseKey, selected);




    });
    function fillOtherBox(id, warehouseId,selected) {

        var currentjqXHR = jQuery.ajax({
            url: "/Order/GetItems",
            data: { 'warehouseId': warehouseId },
            type: "GET",
            dataType: "json",
            success: function (data) {

                // loadData(data)

                LoadOtherItems(data, id,selected);

            },
            error: function (e) {
                alert(e);
            }
        });




    }
    function LoadOtherItems(data, myid ,value) {
        // var count = 1;
     
        $.each(data, function (i, val) {

            if (val.id == value)
            {
              
                $('#lblManufacturer' + myid).text(val.Manufacturer);
                $('#lblItemCode' + myid).text(val.itemCode);
                $('#lblAvailableQuantity' + myid).text(val.Country);
                $('#ddQuantity' + myid).html('');
                $('#ddQuantity' + myid).append('<option value="">---Select Quantity---</option>');
                for (z = 1; z <= val.Country; z++)
                {
                    $('#ddQuantity'+myid).append('<option value="' + z + '">' + z + '</option>');
                }

            }

            //$('#ddItemName' + id).append('<option value="' + val.id + '">' + val.itemName + ' </option>');
            //        

        });


    }

    function fillItemBox(id,warehouseId)
    {

        var currentjqXHR = jQuery.ajax({
            url: "/Order/GetItems",
            data: { 'warehouseId':warehouseId },
            type: "GET",
            dataType: "json",
            success: function (data) {

                // loadData(data)
               
                LoadItems(data,id);

            },
            error: function (e) {
                alert(e);
            }
        });




    }
    function LoadItems(data,id)
    {
        // var count = 1;

        $('#ddItemName' + id).append('<option value="">---Select Items---</option>');
        $.each(data, function (i, val) {

     
            $('#ddItemName'+id).append('<option value="' + val.id + '">' + val.itemName + ' </option>');
 //        

        });


    }


    function loadData(data) {
        
        $('#orderWarehouse').html('');
        $('#orderWarehouse').append('<option value="">---Select Warehouse---</option>')
        var count = 0;
        $.each(data, function (i, val) {
            
            count = count + 1;
            $('#orderWarehouse').append('<option value="' + val.id + '">' + val.warehouseName + '(' + val.warehouseAddress + ') </option>');


        });
        if (count == 0)
        {
            alert("There are no warehouses associated with this country. Please Choose Another Country");
        

        }
    };
    function getDetails(id) {

        var cordis = [];

        var currentjqXHR = jQuery.ajax({
            url: "/Order/GetWarehouses",
            data: { 'id': id },
            type: "GET",
            dataType: "json",
            success: function (data) {

                 loadData(data);

           

          

            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });

        //  currentjqXHR.success(function (data) { alert("hogya"); cordis = loadData(data); });

    }

    populateCountries("orderCountries");
   
        if (window.location.pathname == "/Order/PlaceOrder") {

            $("select#orderCountries").on('change', function () {
              
           
                var selected = $('#orderCountries option:selected').text();
              

                getDetails(selected);




            });
        }


});