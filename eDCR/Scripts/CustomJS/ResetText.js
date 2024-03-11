
// Reset Data
function ResetData() {
 
    $('input[type="hidden"]').val("");
    $('input[type="text"]').val("");   
    $("textarea").val("");   
    $('input[type="checkbox"]:checked').prop('checked', false);
    //$("select")[0].selectedIndex = 0;
    $("select").prop('selectedIndex', 0);
    //$("select").each(function () { this.selectedIndex = 0 });
    $('.date').val(SetData);

}

function RemoveTxtChk() {
    $('.chk').remove();
    $('.txt').remove();

}
//For removing operational & required message after triggering other event
function ClearBorderRequiredMsg() {
    $(".RequiredField").css("border", "1px Solid black"); //Clear Red Color     
    $("#MessageText").html(""); //Clear operation Message  
}


