function clickButton(){
    const res = $('#txtText').val();
    localStorage.setItem("Fruit",res);
    alert('Hello '+ res);
   res = $('#txtText').val('');
}

function readButton(){
    const res = localStorage.getItem("Fruit");
    $("#txtText").val(res);
}

function deleteButton(){
    const res = confirm('Are You Sure Want To Delete!');
    if(!res) return;
    localStorage.removeItem("Fruit");
    $("#txtText").val('');
}