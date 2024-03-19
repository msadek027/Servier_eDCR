function ValidationMsg(msg) {
    if (msg == undefined || msg == null || msg == "") {
        msg == "Enter your value according to the field!"
    }
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html(msg).delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();

  
}
function AcknowledgeMsg() {
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html("Data not found!").delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();
}
function OperationMsg(Mode) {
   
    if (Mode == "I")    {
        $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "grren").html("Saved Successfully!").delay(800).fadeOut(10000);
        $("#MessageText").css("color", "white");
    }
   else if (Mode == "U")
   {       
       $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "grren").html("Updated Successfully!").delay(800).fadeOut(10000);
       $("#MessageText").css("color", "white");
    }
  else if (Mode == "No")   {     
      $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "grren").html("Not Saved!").delay(800).fadeOut(10000);
      $("#MessageText").css("color", "white");
    }
  else if (Mode == "D") {
      $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "red").html("Deleted Successfully!").delay(800).fadeOut(10000);
      $("#MessageText").css("color", "white");
  }
  else if (Mode == "NoDel") {
      $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "red").html("Not Deleted!").delay(800).fadeOut(10000);
      $("#MessageText").css("color", "white");
  }
  else if (Mode == "NoTP") {
      $("#MessageText").show(500).css("margin", "0 1px 20px 0", "color", "grren").html("Not Saved!").delay(800).fadeOut(10000);
      $("#MessageText").css("color", "white");
  }

    $("#MessageText").hide();
}
function WarningMsg() {
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html("Duplicate Data!").delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();
}
function ErrorFrmServerMsg(msgValue) {   
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html(msgValue).delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();
}
function ErrorFrmClientMsg() {
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html("Error occured!").delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();
}
function CompletedMsg() {
    $("#MessageText").show(500).css("margin", "0 1px 20px 0").html("Process Completed!").delay(800).fadeOut(10000);
    $("#MessageText").css("color", "white");
    $("#MessageText").hide();
}
