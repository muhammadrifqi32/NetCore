var table = null;

$(document).ready(function () {
    //debugger;
    table = $('#Department').DataTable({
        "processing": true,
        "ajax": {
            url: "/Departments/LoadDepartment",
            type: "GET",
            "datatype": "json"
        },
        "columnDefs":
            [{
                "targets": [0,4],
                "orderable": false
            }],
        "order": [[0, 'asc']],
        "columns": [
            {
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1 + "."
                }
            },
            { "data": "name" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Done Yet";
                    //debugger;
                    if (data == null) {
                        return dateupdate;
                    } else {
                        return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning " data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                }
            }]
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Update').hide();
    $('#Save').show();
}

function Save() {
    if ($('#Name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Department Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        debugger;
        var Department = new Object();
        Department.Name = $('#Name').val();
        $.ajax({
            type: 'POST',
            url: '/Departments/InsertOrUpdate/',
            data: Department
        }).then((result) => {
            debugger;
            if (result.statusCode == 201 || result.statusCode == 204 || result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Department Added Successfully'
                });
                table.ajax.reload();
            } else {
                Swal.fire('Error', 'Failed to Input', 'error');
                ClearScreen();
            }
        })
    }
}

function GetById(id) {
    //debugger;
    $.ajax({
        url: "/Departments/GetById/",
        data: { id: id }
    }).then((result) => {
        if (result) {
            $('#Id').val(result.id);
            $('#Name').val(result.name);
            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        }
    })
}

function Update() {
    //debugger;
    if ($('#Name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Department Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        var Department = new Object();
        Department.id = $('#Id').val();
        Department.Name = $('#Name').val();
        $.ajax({
            type: "POST",
            url: '/Departments/InsertOrUpdate/',
            data: Department
        }).then((result) => {
            debugger;
            if (result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Department Updated Successfully'
                });
                table.ajax.reload();
            } else {
                Swal.fire('Error', 'Failed to Update', 'error');
                ClearScreen();
            }
        })
    }
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Departments/Delete/",
                data: { id: id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}