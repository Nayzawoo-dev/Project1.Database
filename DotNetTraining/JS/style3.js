$("#btnInsert").click(function(){
   var txtusername = $("#txtUserName").val();
   var txtemail = $("#txtEmail").val();
   var txtpassword = $("#txtPassword").val();
   var requestModel = {
    Username : txtusername,
    Email : txtemail,
    Password : txtpassword
   }
   var json = JSON.stringify(requestModel);
   localStorage.setItem("User Information",json);
   var txtusername = $("#txtUserName").val('');
   var txtemail = $("#txtEmail").val('');
   var txtpassword = $("#txtPassword").val('');
});

// $("#btnUpdate").click(function(){
//    var obj = localStorage.getItem("txtUserName");
//    $("#txtUserName").val(obj);
//    var txtemail = $("#txtEmail").val();
//    var txtpassword = $("#txtPassword").val();
// })

$("#btnDelete").click(function(){
   var res = confirm("Are You Sure Delete?")
   if(!res) return;
   localStorage.removeItem("User Information")
})