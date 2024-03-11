

function EventPermission() {

    $.ajax({
        url: "/Home/GetEventPermission",
        type: 'GET',
        dataType: 'JSON',
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, item) {

                    $("#btnStatus").attr("disabled", item.EditPermission == false ? true : false);
                    $("#btnSave").attr("disabled", item.SavePermission == false ? true : false);
                    $("#btnView").attr("disabled", item.ViewPermission == false ? true : false);
                    $("#btnDelete").attr("disabled", item.DeletePermission == false ? true : false);               
                
                });
                }
              
        }
    }); //End of ajax call
}


