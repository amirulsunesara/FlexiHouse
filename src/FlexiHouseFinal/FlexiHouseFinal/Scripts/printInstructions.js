function printpage() {

    var getImage = document.getElementById("imgLogo");
    var getItems = document.getElementById("tblItems");
    var getInstruction = document.getElementById("workerInstruction");
    var MainWindow = window.open('', '', 'height=500,width=800');
    MainWindow.document.write('<html><head><title>Print Page</title> ');
    MainWindow.document.write('</head><body style="border: 1px solid black;"><center>');
    MainWindow.document.write(getImage.outerHTML + "</center>");


    MainWindow.document.write("<br><center><u><label>Consignment Items<label></u></center><br/><center>" + getItems.innerHTML + "</center>");

    MainWindow.document.write("<br><center><u><label>Slotting Instructions<label></u></center><center>" + getInstruction.innerHTML+"</center>");

    MainWindow.document.write('</body></html>');
    MainWindow.document.close();
    setTimeout(function () {
        MainWindow.print();
    }, 500);
    return false;

}

function printOrderInstruction(id) {

    var getImage = document.getElementById("imgLogo");
    var getItems = document.getElementById("details"+id);
    var getInstruction = document.getElementById("instructions"+id);
    var MainWindow = window.open('', '', 'height=500,width=800');
    MainWindow.document.write('<html><head><title>Print Page</title> ');
    MainWindow.document.write('</head><body style="border: 1px solid black;"><center>');
    MainWindow.document.write(getImage.outerHTML + "</center>");


    MainWindow.document.write("<br><center><u><label>Orders Items<label></u></center><br/><center>" + getItems.innerHTML + "</center>");

    MainWindow.document.write("<br><center><u><label>Dispatch Instructions<label></u></center><center>" + getInstruction.innerHTML + "</center>");

    MainWindow.document.write('</body></html>');
    MainWindow.document.close();
    setTimeout(function () {
        MainWindow.print();
    }, 500);
    return false;

}
