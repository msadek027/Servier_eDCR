

function SetMasterData(data) {
    $.each(data, function (key, value) {
        $('#' + key).val(value);
    });
}


function SetDetailData(GridID, data) {
    $("#" + GridID).data('kendoGrid').dataSource.data(data);
}
