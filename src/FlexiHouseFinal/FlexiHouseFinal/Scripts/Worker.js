$(window).load(function () {



    $('.viewLogs').click(function () {

        

        var id = $(this).attr('id');
        $('#tblLogs').html('');
        getWorkerDetails(id);

        $("#workerName").text(document.getElementById("w"+id).innerHTML);
        $('#myModal4').modal('show');

    });
    function getWorkerDetails(id) {
        var cordis = [];

        var currentjqXHR = jQuery.ajax({
            url: "/Account/GetWorkerLogs",
            data: { 'id': id },
            type: "GET",
            dataType: "json",
            success: function (data) {

             
                abc = loadItemData(data);


            },
            error: function (error) {

            }
        });
    }
    function loadItemData(data) {
       
        var Values = [];
        var i = 0;

       
        $.each(data, function (i, val) {
          
      
            $('#tblLogs').append("<tr><td>" + val.date + "</td><td>" + val.time + "</td><td>" + val.description + "</td></tr>");
 

        });
       
        return Values;
    };

});

