loadData();
let editId = null;
function getData(){
  let res = localStorage.getItem("Data");
  let data = [];
  if (res != null) {
    data = JSON.parse(res);
  }
  return data;
}

function updateDB(lst){
   const json = JSON.stringify(lst);

  localStorage.setItem("Data", json);
}

function loadData() {
  let count = 0;
  let data = getData();
  $("#TbRow").html('');
  data.forEach((item) => {
    $("#TbRow").append(`<tr class="hover:bg-gray-100 even:bg-gray-50">
  <td class="px-4 py-2 border border-gray-200 text-sm text-gray-700">${++count}</td>
  <td class="px-4 py-2 border border-gray-200 text-sm text-gray-700">${
    item.Id
  }</td>
  <td class="px-4 py-2 border border-gray-200 text-sm text-gray-700">${
    item.Author
  }</td>
  <td class="px-4 py-2 border border-gray-200 text-sm text-gray-700">${
    item.Title
  }</td>
  <td class="px-4 py-2 border border-gray-200 text-sm text-gray-700">${
    item.Content
  }</td>
   <td>
   <button class="btn-edit p-1 bg-green-600 rounded-md w-20 text-white cursor-pointer" data-id="${item.Id}">Edit</button>
   <button class="btn-delete p-1 bg-red-600 rounded-md w-20 text-white cursor-pointer" data-id="${item.Id}">Delete</button>
   </td>
</tr>`);
  });

  $('.btn-edit').click(function(){
    let id = $(this).data('id');

    const lst = getData();
    let result = lst.filter(x => x.Id == id);
    console.log(result);
    if(result.length == 0){
      return;
    }
    const item = result[0];
    console.log(item);

    $('#txtAuthor').val(item.Author);
    $('#txtTitle').val(item.Title);
    $('#txtContent').val(item.Content);
    editId = item.Id;
  })

  $('.btn-delete').click(function(){
   let id = $(this).data('id');
  const isConfirm = confirm('Are You Sure Delete!')
  if(!isConfirm){
    return;
  }
  let res = getData();
  let result = res.filter(x => x.Id != id);
  updateDB(result);
  loadData();
});
  //   // const requestModel = {
  //   //   Title : "Test",
  //   //   Author : "Test",
  //   //   Content : "Test"
  //   // }
  //   // console.log(requestModel);
  //   // const data = [];
  //   // data.push(requestModel);
  //   // let result = JSON.stringify(data);
  //   // localStorage.setItem("Data",result);
  //   let res = localStorage.getItem("Data");
  //   console.log("data is " + res);
  //   if (res == null) {
  //     res = [];
  //   } else {
  //     res = JSON.parse(res);
  //   }
  //   console.log(res);
}



$("#btnSave").click(function () {
 if(editId == null){
   let res = localStorage.getItem("Data");
  if (res == null) {
    res = [];
  } else {
    res = JSON.parse(res);
  }

  const author = $("#txtAuthor").val();
  const title = $("#txtTitle").val();
  const content = $("#txtContent").val();

  const requestModel = {
    Id: uuidv4(),
    Author: author,
    Title: title,
    Content: content,
  };

  res.push(requestModel);

  updateDB(res);
  loadData();
 }else{
  let lst = getData();
  let index = lst.findIndex(x => x.Id == editId);
  lst[index].Author = $('#txtAuthor').val();
  lst[index].Title = $('#txtTitle').val();
  lst[index].Content = $('#txtContent').val();
  updateDB(lst);
  loadData();
  editId = null;
 }
  function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, (c) =>
      (
        +c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (+c / 4)))
      ).toString(16)
    );
  }
});






$("#btnCancel").click(function () {
  $("#txtTitle").val("");
  $("#txtAuthor").val("");
  $("#txtContent").val("");
});
