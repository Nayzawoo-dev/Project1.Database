alert("This is Wallet User List Page");
loadData();
function loadData() {
    $.ajax({
        url: "/Wallet/Index",
        type: "POST",
        success: function (response) {
            let count = 0;
            $("#tbDataTable").html('');
            for (let i = 0; i < response.data.length; i++) {
                // console.log(response.data[i]);
                let item = response.data[i];
                let row = `<tr>
				<td>${++count}</td>
				<td>${item.WalletUserName}</td>
				<td>${item.FullName}</td>
				<td>${item.MobileNo}</td>
				<td>${item.Balance}</td>
				<td>
				<button  data-id="${item.WalletId}" class="btn-edit btn btn-outline-warning">Edit</button>
				<button data-id="${item.WalletId}" class="btn-delete btn btn-outline-danger">Delete</button>
				</td>
			</tr>`;

                $("#tbDataTable").append(row);
            }

            bindDeleteClick();
            bindEditClick();
        },
        error: function (request, status, alert) {
            alert(request.responseText);
        }
    });
}

function bindDeleteClick() {
    $(".btn-delete").click(function () {
        const id = $(this).data("id");
        if (!confirm("Are You Sure Want To Delete")) {
            return;
        }
        const item = {
            WalletId: id
        }
        $.ajax({
            url: "/Wallet/Delete",
            type: "POST",
            data: { requestmodel: item },
            success: function (response) {
                if (!response.IsSuccess) {
                    alert(response.Message);
                }
                alert(response.Message);
                loadData();
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        })
    })
}

function bindEditClick() {
    $(".btn-edit").click(function () {
        const id = $(this).data("id");
        const item = {
            WalletId: id
        };
        $.ajax({
            url: "/Wallet/Edit",
            type: "POST",
            data: { requestmodel: item },
            success: function (response) {     
                localStorage.setItem("EditInfo", JSON.stringify(response.data));
                window.location.href = "/Wallet/Edit";
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }

        })
    })
}
