/// <reference path="customDraggable.js" />
/// <reference path="customDraggable.js" />



       // Scroll way down to the bottom to see the example javascript

       // jquery-collision.min.js 1.0.2

       // Example





$(window).load(function () {


    function slotCalc(Awidth, Alength, Srows) {
        slotWidthNum = Math.floor(Awidth / 1.016);
        slotLengthNum = Math.floor(Alength / 1.2192);
        numberOfSlots = Math.floor(slotLengthNum * slotWidthNum * Srows);
        return numberOfSlots;
    }
    $('.dragMe').click(function () {

        $('.active').removeClass('active');

        $("#" + $(this).attr('id')).addClass('active');

    });

    var LengthandWidth = []
    LengthandWidth = getValues();
 
   

    $('#myModal').on('hidden.bs.modal', function () {
        location.reload();
    })
    $('#myModal').modal('show');




    $('#btnSaveMe').click(function (e) {
          e.preventDefault();

          if ($('#wLength').val() == "" || $('#wWidth').val() == "" || $('#sLength').val() == "" || $('#sWidth').val() == "" || $('#sHeight').val() == "" || $('#sRows').val() == "" || $('#wLength').val() <1 || $('#wWidth').val() < 1 || $('#sLength').val() < 1|| $('#sWidth').val() < 1 || $('#sHeight').val() < 1 || $('#sRows').val() <1)
        {
            alert("Please enter correct Details");
          }
          else if ($('#sHeight').val() / $('#sRows').val() < 2)
          {
              alert("Each Row must be at least two meters high");
         }

        else
        {

            var r = confirm("Confirm Save Changes? ");



            if (r == true) {

         


                $.getScript('/Scripts/jquery-1.11.3.js', function (data, textStatus, jqxhr) {
                    var warehouse = {};
                    warehouse.warehouseHtml = "";
                    warehouse.warehouseWidth = $('#wWidth').val();
                    warehouse.warehouseLength = $('#wLength').val();
                    warehouse.shelveLength = $('#sLength').val();
                    warehouse.shelveWidth = $('#sWidth').val();
                    warehouse.shelveHeight = $('#sHeight').val();
                    warehouse.shelveRows = $('#sRows').val();
                    var warehouseScale = wscale(warehouse.warehouseLength, warehouse.warehouseWidth);
                    var shelfn = shelfscale(warehouse.shelveLength, warehouse.shelveWidth);

                  
                    warehouse.scaledShelfLength = shelfn[0];
                    warehouse.scaledShelfWidth = shelfn[1];
                    warehouse.scaledWarehouseLength = warehouseScale[0];
                    warehouse.scaledWarehouseWidth = warehouseScale[1];
                    warehouse.shelfSlots = slotCalc(warehouse.shelveWidth, warehouse.shelveLength, warehouse.shelveRows);
                    warehouse.sections = Math.floor(warehouse.shelveWidth / 1.016);
                
                    $.ajax({
                        url: '/Home/Start',
                        method: 'POST',
                        data: '{warehouse: ' + JSON.stringify(warehouse) + '}',
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            alert('Successfully Saved!');
                        
                            window.location.href = "/Account/LoggedIn";
                        },
                        error: function (err) {
                            alert("Cannot Save this time");
                        }
                    });
                });



            }
        }
      });




    



   
    // This is for Get All Data
      function getValues() {
          var cordis = [];

          var currentjqXHR = jQuery.ajax({
                  url: "/Account/GetAllUser",
                  data: "",
                  type: "GET",
                  dataType: "json",
                  success: function (data) {
                    
                     abc = loadData(data);
                     cordis[0] = abc[0];
                     cordis[1] = abc[1];

                  },
                  error: function () {
                      alert("Failed! Please try again.");
                  }
          });

        //  currentjqXHR.success(function (data) { alert("hogya"); cordis = loadData(data); });
        
          return cordis;
      }

    // this will use for Get Data based on parameter


      function loadData(data) {
          // Here we will format & load/show data
          var Values = [];
          $.each(data, function (i, val) {
              // Append database data here
            
              Values[0] = val.scaledShelfLength;
              Values[1] = val.scaledShelfWidth;
            

          });
          return Values;
      };














    $(".tabs-menu a").click(function (event) {
        event.preventDefault();
        $(this).parent().addClass("current");
       
        $(this).parent().siblings().removeClass("current");
        var tab = $(this).attr("href");
        $(".tab-content").not(tab).css("display", "none");
        $(tab).fadeIn();
    
    });



    $("#btnGoBack").click(function (e) {
        e.preventDefault();
        window.location.href = "/Account/LoggedIn";

    });
    
    var scale = [];
    function wscale(length, width) {
        //width : 17cm , length: 16cm
    
        var l = parseFloat(length);
        var w = parseFloat(width);
        scale[0] = (l / 16).toFixed(2);
        scale[1] = (w / 17).toFixed(2);
      //  document.getElementById('lbllength').innerHTML = '1 cm =' + scale[0] + "m";
      //  document.getElementById('lblwidth').innerHTML = '1 cm =' + scale[1] + "m";
        return scale;
    }
    function shelfscale(length, width) {
        var shelf = [];
        var sl = parseFloat(length);
        var sw = parseFloat(width)
        shelf[0] = (length / scale[0]).toFixed(5);
        shelf[1] = (width / scale[1]).toFixed(5);
       // document.getElementById('lblslength').innerHTML = shelf[0] + "cm";
        //   document.getElementById('lblswidth').innerHTML = shelf[1] + "cm";
        return shelf;
    }




    var Wallcount = 0;
    var Shelvecount = 0;
    var Officecount = 0;

    LoadWall();
    LoadShelves();
  
    LoadOffice();
    LoadDepot();

    function LoadWall() {
     
 
        var maxnum = 0;
        $('.Wally').each(function (i, obj) {
            var myid = $(this).attr('id');

            myid = myid.split('w');

            newmyid = parseInt(myid[1]);

            if (maxnum < newmyid) {
                maxnum = newmyid;
            }

           
            var bid = $(this).attr('id');

            var ob = ''

            $('.Wally').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }

            });
            var ob2 = ''
            $('.dragMe').each(function (ii, obj) {


                ob2 = ob2 + ",#" + $(this).attr('id');


            });

            var obFinal = ob + ob2;
            $('#' + $(this).attr('id')).draggable({

                obstacle: obFinal + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            })





        });

        Wallcount = maxnum+1;
    }
    function LoadShelves() {
        var maxnum = 0;
        $('.dragMe').each(function (i, obj) {
            var myid = $(this).attr('id');
           
            myid = myid.split('s');
         
            newmyid = parseInt(myid[1]);
           
           if (maxnum < newmyid)
           {
              maxnum = newmyid;
           }
        
         
            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''

            $('.dragMe').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            })
            $('.Wally').each(function (ii, obj) {




                ob2 = ob2 + ",#" + $(this).attr('id');





            })
            var finalOb = ob + ob2;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            });

        });
        Shelvecount = maxnum+1;

    }
    function LoadDepot() {
        var depoCount = 0;
        $('.depot').each(function () {
            depoCount = depoCount + 1;

        })

        if (depoCount > 0) {
            $('#btnDepot').prop('disabled', true);
        }
        var obg = ''
        var obg2 = ''
        var obg3 = ''
        $('.dragMe').each(function (i, obj) {



            obg = obg + ",#s" + i;


        })
        $('.Wally').each(function (i, obj) {



            obg2 = obg2 + ",#w" + i;


        })
        $('.office').each(function (i, obj) {



            obg3 = obg3 + ",#o" + i;


        })
        obgFinal = obg + obg2 + obg3;

        $('.depot').draggable({

            obstacle: obgFinal,
            preventCollision: true,
            containment: "#droppable"



        });


    }
    function LoadOffice() {
        var maxnum = 0;
        $('.office').each(function (i, obj) {
            var myid = $(this).attr('id');

            myid = myid.split('o');

            newmyid = parseInt(myid[1]);

            if (maxnum < newmyid) {
                maxnum = newmyid;
            }

            Officecount = Officecount + 1;
            var bid = $(this).attr('id');

            var ob = ''

            $('.office').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }

            });
            var ob2 = ''
            $('.dragMe').each(function (ii, obj) {


                ob2 = ob2 + ",#" + $(this).attr('id');


            });

            var obFinal = ob + ob2;
            $('#' + $(this).attr('id')).draggable({

                obstacle: obFinal + ',.depot,.Wally',
                preventCollision: true,
                containment: "#droppable"


            })





        });
        Officecount = maxnum+1;

    }

    function getListofShelves() {
        var shelves = [];
        $('.dragMe').each(function (i, obj) {
            var offset = $(this).offset();
            id = $(this).attr('id');
            var Xval;
            var Yval;

            if ($("#" + id).hasClass('horiz')) {
                Left = parseFloat($("#" + id).css("left"));
                Leftcm = Left * 2.54 / 96;
                Top = parseFloat($("#" + id).css("top"));
                Topcm = Top * 2.54 / 96;

                Width = parseFloat($("#" + id).css('width'));
                Widthcm = Width * 2.54 / 96;
                Height = parseFloat($("#" + id).css('height'));
                Heightcm = Height * 2.54 / 96;


                a4X = Widthcm + Leftcm;
                a4Y = Heightcm + Topcm;

                Xval = (Leftcm + a4X) / 2;
                Yval = (Topcm + a4Y) / 2;

            }
            if ($("#" + id).hasClass('vert')) {
                Left = parseFloat($("#" + id).css("left"));
                Leftcm = Left * 2.54 / 96;
                Top = parseFloat($("#" + id).css("top"));
                Topcm = Top * 2.54 / 96;


                Width = parseFloat($("#" + id).css('width'));
                Widthcm = Width * 2.54 / 96;
                Height = parseFloat($("#" + id).css('height'));
                Heightcm = Height * 2.54 / 96;

                a4X = Widthcm + Leftcm;
                a4Y = Heightcm + Topcm;

                Xval = (Leftcm + a4X) / 2;
                Yval = (Topcm + a4Y) / 2;

            }

            shelves.push({
                'shelveID': id,
                'centerPointX': Xval,
                'centerPointY': Yval
            });


        });


        return shelves;

    }

    function getDepot() {

        var depot = [];
        $('.depot').each(function (i, obj) {
          
            var offset = $(this).offset();

            var Xval;
            var Yval;



            Left = parseFloat($('.depot').css("left"));
            Leftcm = Left * 2.54 / 96;
            Top = parseFloat($('.depot').css("top"));
            Topcm = Top * 2.54 / 96;


            Width = parseFloat($('.depot').css('width'));
            Widthcm = Width * 2.54 / 96;
            Height = parseFloat($('.depot').css('height'));
            Heightcm = Height * 2.54 / 96;

            a4X = Widthcm + Leftcm;
            a4Y = Heightcm + Topcm;

            Xval = (Leftcm + a4X) / 2;
            Yval = (Topcm + a4Y) / 2;



            depot.push({

                'centerPointX': Xval,
                'centerPointY': Yval
            });


        });


        return depot;

    }
    function alertFunc() {
        alert('Warehouse Has Been Successfully Saved!');
        location.reload();
    }
    $('#btnSave').click(function (e) {

        var depoCount = 0;
        var scount = 0;
        $('.depot').each(function () {
            depoCount = depoCount + 1;

        });
        $('.dragMe').each(function () {
            scount = scount + 1;

        })
        if (depoCount == 0 || scount<3) {
            alert('Please Add Depot and Minimum three Shelves');
        }
        else {

            var r = confirm("Confirm Save Changes? ");
            var xyz;
        

            if (r == true) {

                    var warehouse = {};
                    warehouse.warehouseHtml = $('#droppable').html();

                    var shelves = getListofShelves();
                    var dep = getDepot();

                    var jqueryAjaxMethod = jQuery.ajax({
                        url: '/Home/Save',
                        data: JSON.stringify({ ware: warehouse, shel: shelves, depot: dep }),
                        dataType: 'json',
                        type: 'POST',
                       
                        contentType: "application/json; charset=utf-8",
                     
                        success: function () 
                        {
                           

                           alert('Warehouse Has Been Successfully Saved!');
                            location.reload();
                        },
                        error: function (err) {
                         

                       
                            $('#loadingDiv').css('display', '');
                            xyz = setTimeout(alertFunc, 3000);

                           
                           

                        }
                    });
            



            }
        }
    });



    $('#btnWalls').click(function () {




        var obstacleString = ''
        var obstacleString2 = ''


        $('.Wally').each(function (i, obj) {



            obstacleString = obstacleString + ",#w" + i;


        })

        $('.dragMe').each(function (i, obj) {



            obstacleString2 = obstacleString2 + ",#s" + i;


        })


        if (document.getElementById('opt1').checked) {

            $("#droppable").append($("<img id='w" + Wallcount + "' src='/images/wall2.jpg'  class='Wally horizontalWall list-group-item'/>"));
          
         




        } else if (document.getElementById('opt2').checked) {
            $("#droppable").append($("<img id='w" + Wallcount + "' src='/images/wall3.jpg' class='Wally verticalWall list-group-item'/>"));

        }




        var obg = ''
        var obg2 = ''

        $('.Wally').each(function (i, obj) {



            obg = obg + ",#w" + i;


        })
        $('.dragMe').each(function (i, obj) {



            obg2 = obg2 + ",#s" + i;


        })

        var obgFinal = obg + obg2;
        $('.depot').draggable({

            obstacle: obgFinal + ',.office',
            preventCollision: true,
            containment: "#droppable"



        });
        $('.office').draggable({

            obstacle: obgFinal + ',.depot',
            preventCollision: true,
            containment: "#droppable"



        });

        var obFinal = obstacleString + obstacleString2;
        $('#w' + Wallcount).draggable({

            obstacle: obFinal + ',.office,.depot',
            preventCollision: true,
            containment: "#droppable"



        });


        $('.Wally').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''

            $('.Wally').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            });
            var ob2 = ''
            $('.dragMe').each(function (ii, obj) {





                ob2 = ob2 + ",#" + $(this).attr('id');





            });











            var obFinal = ob + ob2;
            $('#' + $(this).attr('id')).draggable({

                obstacle: obFinal + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            })





        })

        $('.dragMe').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''

            $('.dragMe').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            });
            var ob2 = ''
            $('.Wally').each(function (ii, obj) {





                ob2 = ob2 + ",#" + $(this).attr('id');





            });











            var obFinal = ob + ob2;
            $('#' + $(this).attr('id')).draggable({

                obstacle: obFinal + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            })





        })
        Wallcount = Wallcount + 1;





    });

  

    $('#btnShelves').click(function () {




        var obstacleString = ''
        var obstacleString2 = ''


        $('.dragMe').each(function (i, obj) {



            obstacleString = obstacleString + ",#s" + i;


        })
        $('.Wally').each(function (i, obj) {



            obstacleString2 = obstacleString2 + ",#w" + i;


        })
       
      
            $(".dragMe").each(function () {

                if($(this).attr('id')== "s" + Shelvecount)
                {
                    Shelvecount = Shelvecount + 1;
                    $(".dragMe").each(function () {
                        if ($(this).attr('id') == "s" + Shelvecount)
                        {
                            Shelvecount = Shelvecount + 1;
                        }

                    });
                }

            });
        


        if (document.getElementById('optionsRadiosInline1').checked) {

          
       
            $("#droppable").append($("<img id='s" + Shelvecount + "' class='dragMe list-group-item horiz' style='width:"+LengthandWidth[0]+"cm; height:"+LengthandWidth[1]+"cm;'    src='/images/shelve3.jpg' /> "));
              $('.dragMe.horiz').css({"width":LengthandWidth[0]+"cm","height":LengthandWidth[1]+"cm"});
 
    $('.dragMe.vert').css({ "width": LengthandWidth[1] + "cm", "height": LengthandWidth[0] + "cm" });


        } else if (document.getElementById('optionsRadiosInline2').checked) {
      

            $("#droppable").append($("<img id='s" + Shelvecount + "' class='dragMe list-group-item vert' style=' width:" + LengthandWidth[1] + "cm; height:" + LengthandWidth[0] + "cm;'    src='/images/shelve2.jpg' /> "));
         
            $('.dragMe.horiz').css({ "width": LengthandWidth[0] + "cm", "height": LengthandWidth[1] + "cm" });

            $('.dragMe.vert').css({ "width": LengthandWidth[1] + "cm", "height": LengthandWidth[0] + "cm" });

        }









        var obg = ''
        var obg2 = ''
        $('.dragMe').each(function (i, obj) {



            obg = obg + ",#s" + i;


        })
        $('.Wally').each(function (i, obj) {



            obg2 = obg2 + ",#w" + i;


        })
        obgFinal = obg + obg2;

        $('.depot').draggable({

            obstacle: '.dragMe,.office',
            preventCollision: true,
            containment: "#droppable"



        });

        $('.office').draggable({

            obstacle: obgFinal + ',.depot',
            preventCollision: true,
            containment: "#droppable"



        });
        var finalObstacleString = obstacleString + obstacleString2;
        $('#s' + Shelvecount).draggable({

            obstacle: finalObstacleString,
            preventCollision: true,
            containment: "#droppable"



        });


        $('.dragMe').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''

            $('.dragMe').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            })
            $('.Wally').each(function (ii, obj) {




                ob2 = ob2 + ",#" + $(this).attr('id');





            })
            var finalOb = ob + ob2;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            })





        })




        $('.Wally').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''

            $('.Wally').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            })
            $('.dragMe').each(function (ii, obj) {




                ob2 = ob2 + ",#" + $(this).attr('id');





            })
            var finalOb = ob + ob2;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot,.office',
                preventCollision: true,
                containment: "#droppable"


            })





        })


        Shelvecount = Shelvecount + 1;





    });

    $('.depot').draggable({

        obstacle: '.dragMe,.office',
        preventCollision: true,
        containment: "#droppable"



    });

    $('#btnDelete').click(function () {

        if ($('.list-group-item.active').length) {
            var $name = $('.list-group-item.active').find('span').html();
            if ($name == null) {

                $name = "selected item";

            }
            var r = confirm("Are you sure you want to delete " + $name + " ?");

            if (r == true) {

                $('.list-group-item.active').remove();

            }
        }







    });

    $('#btnDepot').click(function () {




        $('#btnDepot').prop('disabled', true);
        obstacleString = ''
        $("#droppable").append($("<img src='/images/depot.png' class='depot'  />"));

        var obstacleString = ''
        var obstacleString2 = ''
        var obstacleString3 = ''



        $('.depot').draggable({

            obstacle: ".dragMe,.office,.Wally",
            preventCollision: true,
            containment: "#droppable"



        });



    });

    $('#btnOffice').click(function () {


        var obstacleString = ''
        var obstacleString2 = ''
        var obstacleString3 = ''

        $('.dragMe').each(function (i, obj) {



            obstacleString = obstacleString + ",#s" + i;


        });
        $('.Wally').each(function (i, obj) {



            obstacleString2 = obstacleString2 + ",#w" + i;


        });
        $('.office').each(function (i, obj) {



            obstacleString3 = obstacleString3 + ",#o" + i;


        });



        $("#droppable").append($("<img id='o" + Officecount + "' src='/images/office.jpg' class='office list-group-item'  />"));




        var obg = ''
        var obg2 = ''
        var obg3 = ''
        $('.dragMe').each(function (i, obj) {



            obg = obg + ",#s" + i;


        })
        $('.Wally').each(function (i, obj) {



            obg2 = obg2 + ",#w" + i;


        })
        $('.office').each(function (i, obj) {



            obg3 = obg3 + ",#o" + i;


        })
        obgFinal = obg + obg2 + obg3;

        $('.depot').draggable({

            obstacle: obgFinal,
            preventCollision: true,
            containment: "#droppable"



        });

        var finalObstacleString = obstacleString + obstacleString2 + obstacleString3;
        $('#o' + Officecount).draggable({

            obstacle: finalObstacleString,
            preventCollision: true,
            containment: "#droppable"



        });


        $('.office').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''
            var ob3 = ''

            $('.office').each(function (ii, obj) {

                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }


            })
            $('.Wally').each(function (ii, obj) {


                ob2 = ob2 + ",#" + $(this).attr('id');

            })
            $('.dragMe').each(function (ii, obj) {


                ob3 = ob3 + ",#" + $(this).attr('id');

            })
            var finalOb = ob + ob2 + ob3;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot',
                preventCollision: true,
                containment: "#droppable"


            });




        })




        $('.Wally').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''
            var ob3 = ''
            $('.Wally').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            })
            $('.dragMe').each(function (ii, obj) {




                ob2 = ob2 + ",#" + $(this).attr('id');





            })
            $('.office').each(function (ii, obj) {




                ob3 = ob3 + ",#" + $(this).attr('id');





            })
            var finalOb = ob + ob2 + ob3;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot',
                preventCollision: true,
                containment: "#droppable"


            })





        })
        $('.dragMe').each(function (i, obj) {

            var bid = $(this).attr('id');

            var ob = ''
            var ob2 = ''
            var ob3 = ''
            $('.dragMe').each(function (ii, obj) {



                if ($(this).attr('id') == bid) {


                }
                else {

                    ob = ob + ",#" + $(this).attr('id');

                }




            })
            $('.Wally').each(function (ii, obj) {




                ob2 = ob2 + ",#" + $(this).attr('id');





            })
            $('.office').each(function (ii, obj) {




                ob3 = ob3 + ",#" + $(this).attr('id');





            })
            var finalOb = ob + ob2 + ob3;

            $('#' + $(this).attr('id')).draggable({

                obstacle: finalOb + ',.depot',
                preventCollision: true,
                containment: "#droppable"


            })





        })

        Officecount = Officecount + 1;




    });



});

        


// settings can be set for both the zoomTo and zoomTarget calls:


                   


                    
               
            
               
           
                        
        
        

      
     



