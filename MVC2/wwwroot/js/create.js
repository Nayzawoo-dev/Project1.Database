	$("#btnSave").click(function(){
		let item = {
	WalletUserName: $("#txtwalletUsername").val(),
	FullName: $("#txtfullName").val(),
	MobileNo: $("#txtmobileNo").val(),
			   }
	$.ajax({
		url:"/Wallet/Save",
	type:"POST",
	data:{requestModel:item},
	success:function(response){
					   // console.log({response});
					   if(!response.isSuccess){
		alert(response.message);
	return;
					   }
	alert(response.message);
	window.location.href = "/Wallet/Index";
				   },
	error:function(request,status,a){
		alert(request.responseText);
				   }
			   })
		   })

